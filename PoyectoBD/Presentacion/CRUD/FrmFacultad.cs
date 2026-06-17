using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmFacultad : Form
    {
        private int _idSel = -1;
        public FrmFacultad() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter("SELECT codigo,nombre,fecha_creacion FROM Facultad ORDER BY nombre", con))
                { var dt = new DataTable(); da.Fill(dt); dgv.DataSource = dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            _idSel = Convert.ToInt32(dgv.CurrentRow.Cells["codigo"].Value);
            txtNombre.Text = dgv.CurrentRow.Cells["nombre"].Value?.ToString();
        }

        private void btnNuevo_Click(object sender, EventArgs e)    { _idSel=-1; txtNombre.Clear(); dgv.ClearSelection(); }
        private void btnCancelar_Click(object sender, EventArgs e) { _idSel=-1; txtNombre.Clear(); dgv.ClearSelection(); }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Ingrese nombre."); return; }
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    string sql = _idSel == -1
                        ? "INSERT INTO Facultad(nombre,fecha_creacion) VALUES(@n,GETDATE())"
                        : "UPDATE Facultad SET nombre=@n WHERE codigo=@id";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
                        if (_idSel != -1) cmd.Parameters.AddWithValue("@id", _idSel);
                        cmd.ExecuteNonQuery();
                    }
                }
                btnNuevo_Click(null, null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSel == -1) { MessageBox.Show("Seleccione un registro."); return; }
            if (MessageBox.Show("¿Eliminar?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand("DELETE FROM Facultad WHERE codigo=@id", con))
                { cmd.Parameters.AddWithValue("@id", _idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null, null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}
