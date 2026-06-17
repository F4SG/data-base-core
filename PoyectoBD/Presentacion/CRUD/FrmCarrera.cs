using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmCarrera : Form
    {
        private string _codSel = null;
        public FrmCarrera() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarFacultades(); Cargar(); }

        private void CargarFacultades()
        {
            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter("SELECT codigo,nombre FROM Facultad ORDER BY nombre", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                cmbFacultad.DisplayMember = "nombre";
                cmbFacultad.ValueMember   = "codigo";
                cmbFacultad.DataSource    = dt;
                cmbFacultad.SelectedIndex = -1;
            }
        }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    "SELECT c.codigo, c.descripcion, f.nombre AS facultad FROM Carrera c INNER JOIN Facultad f ON c.cod_facultad=f.codigo ORDER BY c.codigo", con))
                { var dt = new DataTable(); da.Fill(dt); dgv.DataSource = dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            _codSel = dgv.CurrentRow.Cells["codigo"].Value?.ToString();
            txtCodigo.Text = _codSel;
            txtDesc.Text   = dgv.CurrentRow.Cells["descripcion"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _codSel=null; txtCodigo.Clear(); txtDesc.Clear(); cmbFacultad.SelectedIndex=-1; }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text)) { MessageBox.Show("Ingrese código."); return; }
            if (string.IsNullOrWhiteSpace(txtDesc.Text))   { MessageBox.Show("Ingrese descripción."); return; }
            if (cmbFacultad.SelectedValue == null)         { MessageBox.Show("Seleccione facultad."); return; }
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    string sql = _codSel == null
                        ? "INSERT INTO Carrera(codigo,descripcion,cod_facultad) VALUES(@c,@d,@f)"
                        : "UPDATE Carrera SET descripcion=@d,cod_facultad=@f WHERE codigo=@c";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@c", txtCodigo.Text.Trim());
                        cmd.Parameters.AddWithValue("@d", txtDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@f", Convert.ToInt32(cmbFacultad.SelectedValue));
                        cmd.ExecuteNonQuery();
                    }
                }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnEliminar_Click(object s, EventArgs e)
        {
            if (_codSel == null) { MessageBox.Show("Seleccione un registro."); return; }
            if (MessageBox.Show("¿Eliminar?","Confirmar",MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand("DELETE FROM Carrera WHERE codigo=@c", con))
                { cmd.Parameters.AddWithValue("@c",_codSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}
