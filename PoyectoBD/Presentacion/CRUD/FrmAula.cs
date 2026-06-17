using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmAula : Form
    {
        private int _idSel=-1;
        public FrmAula() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter("SELECT id_aula, codigo, capacidad, ubicacion FROM Aula ORDER BY codigo",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["id_aula"].Value);
            txtCodigo.Text=dgv.CurrentRow.Cells["codigo"].Value?.ToString();
            txtCapacidad.Text=dgv.CurrentRow.Cells["capacidad"].Value?.ToString();
            txtUbicacion.Text=dgv.CurrentRow.Cells["ubicacion"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtCodigo.Clear(); txtCapacidad.Clear(); txtUbicacion.Clear(); dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text)) { MessageBox.Show("Ingrese código."); return; }
            if (!short.TryParse(txtCapacidad.Text, out short cap)||cap<=0) { MessageBox.Show("Capacidad debe ser > 0."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql=_idSel==-1
                        ? "INSERT INTO Aula(codigo,capacidad,ubicacion) VALUES(@c,@ca,@u)"
                        : "UPDATE Aula SET codigo=@c,capacidad=@ca,ubicacion=@u WHERE id_aula=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@c",txtCodigo.Text.Trim());
                        cmd.Parameters.AddWithValue("@ca",cap);
                        cmd.Parameters.AddWithValue("@u",(object)txtUbicacion.Text.Trim() ?? DBNull.Value);
                        if (_idSel!=-1) cmd.Parameters.AddWithValue("@id",_idSel);
                        cmd.ExecuteNonQuery();
                    }
                }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnEliminar_Click(object s, EventArgs e)
        {
            if (_idSel==-1) { MessageBox.Show("Seleccione un registro."); return; }
            if (MessageBox.Show("¿Eliminar?","Confirmar",MessageBoxButtons.YesNo)!=DialogResult.Yes) return;
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var cmd=new SqlCommand("DELETE FROM Aula WHERE id_aula=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
