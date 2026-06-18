using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.Procesos
{
    /// <summary>
    /// Formulario de Ingreso de Notas.
    /// 
    /// Reglas implementadas:
    ///   - Si fecha_fin de la edición ya pasó → botón Bloquear ya está "activo" (edición cerrada),
    ///     el trigger trg_Nota_ProtegerCierre refuerza esto en la BD.
    ///   - Botón Modificar: habilita edición de celdas.
    ///   - Botón Grabar: persiste cambios de parcial_1, parcial_2 y nota_final.
    ///   - Botón Bloquear: actualiza fecha_fin de Edicion_Materia a hoy para cerrarla.
    /// </summary>
    public partial class FrmIngresNotas : Form
    {
        private int    _codEdicion   = -1;
        private bool   _editando     = false;
        private bool   _edicionCerrada = false;

        public FrmIngresNotas() { InitializeComponent(); }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarGestiones();
            // Suprimir error de sintaxis 'esc' en celdas editables del DataGridView
            dgvNotas.DataError += (s, ev) => { ev.Cancel = true; };
        }

        // ── Paso 1: Cargar gestiones ──────────────────────────────────
        private void CargarGestiones()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    "SELECT id_gestion, CAST(semestre AS VARCHAR)+'/'+CAST(anio AS VARCHAR) AS etiqueta FROM Gestion ORDER BY anio DESC,semestre DESC", con))
                {
                    var dt = new DataTable(); da.Fill(dt);
                    cmbGestion.DisplayMember = "etiqueta"; cmbGestion.ValueMember = "id_gestion";
                    cmbGestion.DataSource = dt; cmbGestion.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Paso 2: Al cambiar gestión, cargar ediciones del docente ──
        private void txtCodDocente_Leave(object sender, EventArgs e) => CargarEdiciones();
        private void cmbGestion_SelectedIndexChanged(object sender, EventArgs e) => CargarEdiciones();

        private void CargarEdiciones()
        {
            cmbEdicion.DataSource = null;
            cmbGrupo.DataSource   = null;
            if (!int.TryParse(txtCodDocente.Text, out int cod)) return;
            if (cmbGestion.SelectedValue == null) return;
            int idGestion = Convert.ToInt32(cmbGestion.SelectedValue);
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    @"SELECT em.cod_edicion,
                             m.sigla+'-'+CAST(em.cod_edicion AS VARCHAR) AS etiqueta,
                             em.fecha_fin
                      FROM Edicion_Materia em
                      INNER JOIN Materia m ON em.sigla_materia=m.sigla
                      WHERE em.cod_docente=@d AND em.id_gestion=@g
                      ORDER BY m.sigla", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@d", cod);
                    da.SelectCommand.Parameters.AddWithValue("@g", idGestion);
                    var dt = new DataTable(); da.Fill(dt);
                    cmbEdicion.DisplayMember = "etiqueta"; cmbEdicion.ValueMember = "cod_edicion";
                    cmbEdicion.DataSource = dt; cmbEdicion.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Paso 3: Al cambiar edición, cargar grupos ─────────────────
        private void cmbEdicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbGrupo.DataSource = null;
            lblEstadoEdicion.Text = "";
            if (cmbEdicion.SelectedIndex < 0 || cmbEdicion.DataSource == null) return;
            if (cmbEdicion.SelectedValue == null) return;
            int codEd = Convert.ToInt32(cmbEdicion.SelectedValue);

            // Verificar si edición está cerrada
            try
            {
                var dt = (DataTable)cmbEdicion.DataSource;
                if (cmbEdicion.SelectedIndex >= dt.Rows.Count) return;
                var row = dt.Rows[cmbEdicion.SelectedIndex];
                _edicionCerrada = Convert.ToDateTime(row["fecha_fin"]).Date < DateTime.Today;
            }
            catch { _edicionCerrada = false; }

            btnBloquear.Enabled = !_edicionCerrada;
            lblEstadoEdicion.Text = _edicionCerrada ? "🔒 EDICIÓN CERRADA" : "🔓 Edición abierta";
            lblEstadoEdicion.ForeColor = _edicionCerrada ? System.Drawing.Color.Crimson : System.Drawing.Color.Green;

            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    @"SELECT gr.id_grupo, gr.descripcion FROM EDI_GRU eg
                      INNER JOIN Grupo gr ON eg.id_grupo=gr.id_grupo
                      WHERE eg.cod_edicion=@ce ORDER BY gr.descripcion", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ce", codEd);
                    var dtGr = new DataTable(); da.Fill(dtGr);
                    cmbGrupo.DisplayMember = "descripcion"; cmbGrupo.ValueMember = "id_grupo";
                    cmbGrupo.DataSource = dtGr; cmbGrupo.SelectedIndex = dtGr.Rows.Count > 0 ? 0 : -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Paso 4: Cargar estudiantes con notas ─────────────────────
        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (cmbEdicion.SelectedValue == null) { MessageBox.Show("Seleccione edición."); return; }
            _codEdicion = Convert.ToInt32(cmbEdicion.SelectedValue);

            string sql = @"
                SELECT n.id_nota, p.nombre AS estudiante,
                       n.nota_parcial_1, n.nota_parcial_2, n.nota_final,
                       n.estado_aprobacion, n.nro_registro
                FROM Nota n
                INNER JOIN Estudiante e ON n.nro_registro=e.nro_registro
                INNER JOIN Persona p    ON e.idpersona=p.idpersona
                WHERE n.cod_edicion=@ce
                ORDER BY p.nombre";
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(sql, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@ce", _codEdicion);
                    var dt = new DataTable(); da.Fill(dt);
                    dgvNotas.DataSource = dt;
                    ConfigurarGrilla();
                }
                _editando = false;
                dgvNotas.ReadOnly = true;
                btnGrabar.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void ConfigurarGrilla()
        {
            if (dgvNotas.Columns.Count == 0) return;
            dgvNotas.Columns["id_nota"].Visible       = false;
            dgvNotas.Columns["nro_registro"].Visible  = false;
            dgvNotas.Columns["estudiante"].HeaderText = "Estudiante";
            dgvNotas.Columns["estudiante"].ReadOnly   = true;
            dgvNotas.Columns["nota_parcial_1"].HeaderText = "Parcial 1";
            dgvNotas.Columns["nota_parcial_2"].HeaderText = "Parcial 2";
            dgvNotas.Columns["nota_final"].HeaderText     = "Nota Final";
            dgvNotas.Columns["estado_aprobacion"].HeaderText = "Estado";
            dgvNotas.Columns["estado_aprobacion"].ReadOnly   = true;
        }

        // ── Botón Modificar ───────────────────────────────────────────
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (_edicionCerrada)
            { MessageBox.Show("Esta edición está cerrada. No se pueden modificar las notas.", "Bloqueado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (dgvNotas.Rows.Count == 0) { MessageBox.Show("Cargue primero los estudiantes."); return; }
            _editando = true;
            dgvNotas.ReadOnly = false;
            // Solo las columnas de notas son editables
            if (dgvNotas.Columns.Contains("estudiante"))       dgvNotas.Columns["estudiante"].ReadOnly = true;
            if (dgvNotas.Columns.Contains("estado_aprobacion")) dgvNotas.Columns["estado_aprobacion"].ReadOnly = true;
            btnGrabar.Enabled = true;
        }

        // ── Botón Grabar ──────────────────────────────────────────────
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (!_editando) return;
            int actualizados = 0;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    foreach (DataGridViewRow row in dgvNotas.Rows)
                    {
                        if (row.IsNewRow) continue;
                        int  idNota = Convert.ToInt32(row.Cells["id_nota"].Value);
                        decimal? p1 = ParseNota(row.Cells["nota_parcial_1"].Value);
                        decimal? p2 = ParseNota(row.Cells["nota_parcial_2"].Value);
                        decimal? nf = ParseNota(row.Cells["nota_final"].Value);

                        // Validar rangos
                        if ((p1.HasValue && (p1<0||p1>100)) ||
                            (p2.HasValue && (p2<0||p2>100)) ||
                            (nf.HasValue && (nf<0||nf>100)))
                        { MessageBox.Show($"Nota fuera de rango (0-100) en fila {row.Index+1}."); return; }

                        using (var cmd = new SqlCommand(
                            "UPDATE Nota SET nota_parcial_1=@p1, nota_parcial_2=@p2, nota_final=@nf WHERE id_nota=@id", con))
                        {
                            cmd.Parameters.AddWithValue("@p1", (object)p1 ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@p2", (object)p2 ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@nf", (object)nf ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@id", idNota);
                            cmd.ExecuteNonQuery();
                            actualizados++;
                        }
                    }
                }
                MessageBox.Show($"Notas guardadas: {actualizados}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _editando = false; dgvNotas.ReadOnly = true; btnGrabar.Enabled = false;
                // Recargar para ver estado_aprobacion actualizado por la BD
                if (_codEdicion > 0) btnCargar_Click(btnCargar, EventArgs.Empty);
            }
            catch (SqlException ex)
            {
                // Captura RAISERROR del trigger trg_Nota_ProtegerCierre
                MessageBox.Show("Error al grabar: " + ex.Message, "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Botón Bloquear ────────────────────────────────────────────
        private void btnBloquear_Click(object sender, EventArgs e)
        {
            if (_codEdicion == -1) { MessageBox.Show("Cargue la edición primero."); return; }
            if (_edicionCerrada) { MessageBox.Show("La edición ya estaba cerrada."); return; }

            if (MessageBox.Show(
                "¿Confirmar cierre de esta edición? Las notas finales ya no podrán modificarse.",
                "Confirmar Bloqueo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand(
                    "UPDATE Edicion_Materia SET fecha_fin=CAST(GETDATE()-1 AS DATE) WHERE cod_edicion=@ce", con))
                {
                    cmd.Parameters.AddWithValue("@ce", _codEdicion);
                    cmd.ExecuteNonQuery();
                }
                _edicionCerrada = true;
                btnBloquear.Enabled = false;
                lblEstadoEdicion.Text = "🔒 EDICIÓN CERRADA";
                lblEstadoEdicion.ForeColor = System.Drawing.Color.Crimson;
                dgvNotas.ReadOnly = true; _editando = false; btnGrabar.Enabled = false;
                MessageBox.Show("Edición bloqueada exitosamente.", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private decimal? ParseNota(object val)
        {
            if (val == null || val == DBNull.Value || val.ToString().Trim() == "") return null;
            if (decimal.TryParse(val.ToString(), out decimal d)) return d;
            return null;
        }
    }
}
