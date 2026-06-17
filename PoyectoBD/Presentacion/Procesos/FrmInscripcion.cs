using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.Procesos
{
    /// <summary>
    /// Formulario de Inscripción de Materias (Matriculación).
    /// 
    /// Flujo:
    ///   1. Docente ingresa nro_registro → autocompleta nombre del estudiante.
    ///   2. Ingresa num_plan → autocompleta carrera.
    ///   3. Selecciona gestión.
    ///   4. Se carga el DataGridView con las Edicion_Materia disponibles del plan/gestión.
    ///   5. El estudiante marca checkbox y selecciona grupo por cada materia.
    ///   6. Botón "Registrar" crea un registro en Nota por cada fila seleccionada.
    /// </summary>
    public partial class FrmInscripcion : Form
    {
        public FrmInscripcion() { InitializeComponent(); }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarGestiones();
            // Suprimir el error de sintaxis 'esc' del DataGridViewComboBoxColumn.
            // Este error ocurre cuando el usuario presiona Escape o el valor
            // del combo no coincide con ningún item de su lista.
            dgvMaterias.DataError += dgvMaterias_DataError;
        }

        // ── Handler de error DataGridView (suprime errores de ComboBox) ───
        private void dgvMaterias_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Ignorar silenciosamente errores de validación del ComboBox interno.
            // Causados por: Escape, valor no encontrado en lista, tipo incompatible.
            if (e.ColumnIndex >= 0 &&
                dgvMaterias.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                e.Cancel = true;   // No propagar el error
                return;
            }
            // Para otras columnas mostrar el error real
            MessageBox.Show($"Error en celda [{e.RowIndex},{e.ColumnIndex}]: {e.Exception?.Message}",
                "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
        }

        // ── Paso 1: Autocompletar nombre de estudiante ────────────────
        private void txtNroRegistro_Leave(object sender, EventArgs e)
        {
            txtNombreEstudiante.Clear();
            if (!int.TryParse(txtNroRegistro.Text, out int nr)) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand(
                    "SELECT p.nombre FROM Estudiante e INNER JOIN Persona p ON e.idpersona=p.idpersona WHERE e.nro_registro=@nr", con))
                {
                    cmd.Parameters.AddWithValue("@nr", nr);
                    var result = cmd.ExecuteScalar();
                    if (result != null) txtNombreEstudiante.Text = result.ToString();
                    else MessageBox.Show("Estudiante no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Paso 2: Autocompletar carrera ────────────────────────────
        private void txtNumPlan_Leave(object sender, EventArgs e)
        {
            txtCarrera.Clear();
            if (!int.TryParse(txtNumPlan.Text, out int plan)) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand(
                    "SELECT c.descripcion FROM Plan_Estudio p INNER JOIN Carrera c ON p.cod_carrera=c.codigo WHERE p.num_plan=@p", con))
                {
                    cmd.Parameters.AddWithValue("@p", plan);
                    var result = cmd.ExecuteScalar();
                    if (result != null) txtCarrera.Text = result.ToString();
                    else MessageBox.Show("Plan no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // ── Cargar gestiones en combo ─────────────────────────────────
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

        // ── Cargar materias ofertadas en DataGridView ─────────────────
        private void btnCargarMaterias_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtNumPlan.Text, out int plan)) { MessageBox.Show("Ingrese número de plan."); return; }
            if (cmbGestion.SelectedValue == null)              { MessageBox.Show("Seleccione gestión.");   return; }
            int idGestion = Convert.ToInt32(cmbGestion.SelectedValue);

            try
            {
                // Consulta: materias del plan que tienen edición en la gestión seleccionada + grupos disponibles
                string sql = @"SELECT em.cod_edicion,
                       m.sigla,
                       m.nombre     AS materia,
                       gr.descripcion AS grupo,
                       gr.id_grupo
                FROM Pensum ps
                INNER JOIN Materia m           ON ps.sigla_materia = m.sigla
                INNER JOIN Edicion_Materia em  ON em.sigla_materia = m.sigla
                                              AND em.id_gestion    = @g
                INNER JOIN EDI_GRU eg          ON eg.cod_edicion   = em.cod_edicion
                INNER JOIN Grupo gr            ON gr.id_grupo      = eg.id_grupo
                WHERE ps.num_plan = @p
                ORDER BY m.sigla, gr.descripcion";

                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(sql, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@g", idGestion);
                    da.SelectCommand.Parameters.AddWithValue("@p", plan);
                    var dt = new DataTable(); da.Fill(dt);
                    CargarGrilla(dt);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar materias: " + ex.Message); }
        }

        /// <summary>
        /// Construye el DataGridView con las columnas:
        /// [checkbox] | Sigla | Materia | Grupo (combo) | cod_edicion (oculta) | id_grupo (oculta)
        /// Se agrupa por materia → el combo muestra los grupos disponibles.
        /// </summary>
        private void CargarGrilla(DataTable dt)
        {
            dgvMaterias.Rows.Clear();
            dgvMaterias.Columns.Clear();

            // Columna Checkbox
            var colCheck = new DataGridViewCheckBoxColumn { Name="colSel", HeaderText="Sel.", Width=40 };
            // Columnas de datos
            var colSigla   = new DataGridViewTextBoxColumn { Name="colSigla",   HeaderText="Sigla",     Width=80,  ReadOnly=true };
            var colMateria = new DataGridViewTextBoxColumn { Name="colMateria",  HeaderText="Materia",   Width=220, ReadOnly=true };
            // Columna Grupo (combo)
            var colGrupo = new DataGridViewComboBoxColumn { Name="colGrupo", HeaderText="Grupo", Width=100 };
            // Columnas ocultas
            var colEdicion = new DataGridViewTextBoxColumn { Name="colEdicion", Visible=false };
            var colGrupoId = new DataGridViewTextBoxColumn { Name="colGrupoId", Visible=false };

            dgvMaterias.Columns.AddRange(colCheck, colSigla, colMateria, colGrupo, colEdicion, colGrupoId);

            // Agrupar por materia para construir filas únicas con combos de grupos
            // Estrategia: una fila por (sigla, materia), el combo de grupos tiene todos los grupos disponibles
            // y el cod_edicion se asigna al confirmar (tomamos el cod_edicion del grupo elegido)
            string siglaActual = "";
            int rowIdx = -1;
            foreach (DataRow dr in dt.Rows)
            {
                string sigla   = dr["sigla"].ToString();
                string materia = dr["materia"].ToString();
                string grupo   = dr["grupo"].ToString();
                string codEd   = dr["cod_edicion"].ToString();

                if (sigla != siglaActual)
                {
                    // Nueva fila para esta materia
                    dgvMaterias.Rows.Add(false, sigla, materia, grupo, codEd, dr["id_grupo"].ToString());
                    rowIdx = dgvMaterias.Rows.Count - 1;
                    siglaActual = sigla;
                    // Inicializar combo de grupos
                    var cell = (DataGridViewComboBoxCell)dgvMaterias.Rows[rowIdx].Cells["colGrupo"];
                    cell.Items.Clear();
                    cell.Items.Add(grupo);
                    cell.Value = grupo;
                }
                else
                {
                    // Agregar grupo adicional al combo de la fila existente
                    var cell = (DataGridViewComboBoxCell)dgvMaterias.Rows[rowIdx].Cells["colGrupo"];
                    if (!cell.Items.Contains(grupo)) cell.Items.Add(grupo);
                }
            }
        }

        // ── Registrar inscripción ─────────────────────────────────────
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtNroRegistro.Text, out int nr)) { MessageBox.Show("Ingrese número de registro."); return; }
            if (string.IsNullOrWhiteSpace(txtNombreEstudiante.Text)) { MessageBox.Show("Estudiante no encontrado."); return; }
            if (cmbGestion.SelectedValue == null) { MessageBox.Show("Seleccione gestión."); return; }

            int inscritos = 0;
            int errores   = 0;

            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    foreach (DataGridViewRow row in dgvMaterias.Rows)
                    {
                        bool sel = Convert.ToBoolean(row.Cells["colSel"].Value ?? false);
                        if (!sel) continue;

                        // Obtener cod_edicion según el grupo seleccionado
                        string grupoSel = row.Cells["colGrupo"].Value?.ToString();
                        string sigla    = row.Cells["colSigla"].Value?.ToString();
                        int    idGestion= Convert.ToInt32(cmbGestion.SelectedValue);

                        // Buscar el cod_edicion correcto para sigla + gestion + grupo
                        int codEdicion = -1;
                        string sqlBuscar = @"
                            SELECT em.cod_edicion FROM Edicion_Materia em
                            INNER JOIN EDI_GRU eg ON eg.cod_edicion=em.cod_edicion
                            INNER JOIN Grupo g    ON g.id_grupo=eg.id_grupo
                            WHERE em.sigla_materia=@s AND em.id_gestion=@g AND g.descripcion=@gr";
                        using (var cmd = new SqlCommand(sqlBuscar, con))
                        {
                            cmd.Parameters.AddWithValue("@s", sigla);
                            cmd.Parameters.AddWithValue("@g", idGestion);
                            cmd.Parameters.AddWithValue("@gr", grupoSel ?? "");
                            var res = cmd.ExecuteScalar();
                            if (res != null) codEdicion = Convert.ToInt32(res);
                        }

                        if (codEdicion == -1) { errores++; continue; }

                        // Insertar en Nota (inscripción)
                        try
                        {
                            using (var cmd = new SqlCommand(
                                "INSERT INTO Nota(nro_registro,cod_edicion) VALUES(@nr,@ce)", con))
                            {
                                cmd.Parameters.AddWithValue("@nr", nr);
                                cmd.Parameters.AddWithValue("@ce", codEdicion);
                                cmd.ExecuteNonQuery();
                                inscritos++;
                            }
                        }
                        catch
                        {
                            errores++; // Puede ser duplicado (UQ_Nota_Inscr)
                        }
                    }
                }

                string msg = $"Inscripción completada.\nMaterias inscritas: {inscritos}";
                if (errores > 0) msg += $"\nErrores/duplicados omitidos: {errores}";
                MessageBox.Show(msg, "Resultado", MessageBoxButtons.OK,
                    errores > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNroRegistro.Clear(); txtNombreEstudiante.Clear();
            txtNumPlan.Clear(); txtCarrera.Clear();
            cmbGestion.SelectedIndex = -1;
            dgvMaterias.Rows.Clear();
            dgvMaterias.Columns.Clear();
        }
    }
}
