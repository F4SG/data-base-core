using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmPersona : Form
    {
        private int _idSeleccionado = -1;

        public FrmPersona() { InitializeComponent(); }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    "SELECT idpersona, ci, nombre, sexo, fecha_nacimiento FROM Persona ORDER BY nombre", con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgv.DataSource = dt;
                }
                ConfigurarGrid();
            }
            catch (Exception ex) { MostrarError(ex); }
        }

        private void ConfigurarGrid()
        {
            if (dgv.Columns.Count == 0) return;
            dgv.Columns["idpersona"].HeaderText       = "ID";
            dgv.Columns["ci"].HeaderText              = "C.I.";
            dgv.Columns["nombre"].HeaderText          = "Nombre";
            dgv.Columns["sexo"].HeaderText            = "Sexo";
            dgv.Columns["fecha_nacimiento"].HeaderText = "Fecha Nac.";
            dgv.Columns["idpersona"].Width = 50;
            dgv.Columns["fecha_nacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var row = dgv.CurrentRow;
            _idSeleccionado              = Convert.ToInt32(row.Cells["idpersona"].Value);
            txtCI.Text                   = row.Cells["ci"].Value?.ToString();
            txtNombre.Text               = row.Cells["nombre"].Value?.ToString();
            cmbSexo.Text                 = row.Cells["sexo"].Value?.ToString();
            dtpFechaNac.Value            = Convert.ToDateTime(row.Cells["fecha_nacimiento"].Value);
        }

        private void btnNuevo_Click(object sender, EventArgs e)   => Limpiar();
        private void btnCancelar_Click(object sender, EventArgs e) => Limpiar();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    string sql;
                    if (_idSeleccionado == -1)
                        sql = "INSERT INTO Persona (ci,nombre,sexo,fecha_nacimiento) VALUES (@ci,@n,@s,@fn)";
                    else
                        sql = "UPDATE Persona SET ci=@ci,nombre=@n,sexo=@s,fecha_nacimiento=@fn WHERE idpersona=@id";

                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ci", txtCI.Text.Trim());
                        cmd.Parameters.AddWithValue("@n",  txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@s",  cmbSexo.Text.Trim());
                        cmd.Parameters.AddWithValue("@fn", dtpFechaNac.Value.Date);
                        if (_idSeleccionado != -1)
                            cmd.Parameters.AddWithValue("@id", _idSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Guardado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Limpiar();
                Cargar();
            }
            catch (Exception ex) { MostrarError(ex); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == -1) { MessageBox.Show("Seleccione un registro."); return; }
            if (MessageBox.Show("¿Eliminar esta persona?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand("DELETE FROM Persona WHERE idpersona=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", _idSeleccionado);
                    cmd.ExecuteNonQuery();
                }
                Limpiar(); Cargar();
            }
            catch (Exception ex) { MostrarError(ex); }
        }

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtCI.Text))     { MessageBox.Show("Ingrese C.I.");     return false; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Ingrese nombre.");  return false; }
            if (cmbSexo.SelectedIndex < 0)                 { MessageBox.Show("Seleccione sexo."); return false; }
            return true;
        }

        private void Limpiar()
        {
            _idSeleccionado = -1;
            txtCI.Clear(); txtNombre.Clear();
            cmbSexo.SelectedIndex = -1;
            dtpFechaNac.Value = DateTime.Today;
            dgv.ClearSelection();
        }

        private void MostrarError(Exception ex) =>
            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
