using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmMateria : Form
    {
        private string _siglaSel = null;
        public FrmMateria() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter("SELECT sigla, nombre FROM Materia ORDER BY sigla", con))
                { var dt = new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _siglaSel=dgv.CurrentRow.Cells["sigla"].Value?.ToString();
            txtSigla.Text=_siglaSel; txtNombre.Text=dgv.CurrentRow.Cells["nombre"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _siglaSel=null; txtSigla.Clear(); txtNombre.Clear(); txtSigla.ReadOnly=false; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSigla.Text))  { MessageBox.Show("Ingrese sigla.");  return; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) { MessageBox.Show("Ingrese nombre."); return; }
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    string sql = _siglaSel==null
                        ? "INSERT INTO Materia(sigla,nombre) VALUES(@s,@n)"
                        : "UPDATE Materia SET nombre=@n WHERE sigla=@s";
                    using (var cmd = new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@s",txtSigla.Text.Trim());
                        cmd.Parameters.AddWithValue("@n",txtNombre.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnEliminar_Click(object s, EventArgs e)
        {
            if (_siglaSel==null) { MessageBox.Show("Seleccione un registro."); return; }
            if (MessageBox.Show("¿Eliminar materia "+_siglaSel+"?","Confirmar",MessageBoxButtons.YesNo)!=DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand("DELETE FROM Materia WHERE sigla=@s",con))
                { cmd.Parameters.AddWithValue("@s",_siglaSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
