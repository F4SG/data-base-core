using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmGrupo : Form
    {
        private int _idSel=-1;
        public FrmGrupo() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter("SELECT id_grupo,descripcion,cupo_maximo FROM Grupo ORDER BY descripcion",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["id_grupo"].Value);
            txtDesc.Text=dgv.CurrentRow.Cells["descripcion"].Value?.ToString();
            txtCupo.Text=dgv.CurrentRow.Cells["cupo_maximo"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtDesc.Clear(); txtCupo.Clear(); dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDesc.Text)) { MessageBox.Show("Ingrese descripción."); return; }
            if (!short.TryParse(txtCupo.Text,out short cupo)||cupo<=0) { MessageBox.Show("Cupo debe ser > 0."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql=_idSel==-1
                        ? "INSERT INTO Grupo(descripcion,cupo_maximo) VALUES(@d,@c)"
                        : "UPDATE Grupo SET descripcion=@d,cupo_maximo=@c WHERE id_grupo=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@d",txtDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@c",cupo);
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
                using (var cmd=new SqlCommand("DELETE FROM Grupo WHERE id_grupo=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
