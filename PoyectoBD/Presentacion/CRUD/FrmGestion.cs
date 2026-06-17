using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmGestion : Form
    {
        private int _idSel=-1;
        public FrmGestion() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter("SELECT id_gestion, semestre, anio, CAST(semestre AS VARCHAR)+'/'+ CAST(anio AS VARCHAR) AS descripcion FROM Gestion ORDER BY anio,semestre",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["id_gestion"].Value);
            cmbSemestre.SelectedItem=dgv.CurrentRow.Cells["semestre"].Value?.ToString();
            txtAnio.Text=dgv.CurrentRow.Cells["anio"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; cmbSemestre.SelectedIndex=-1; txtAnio.Clear(); dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbSemestre.SelectedIndex<0) { MessageBox.Show("Seleccione semestre."); return; }
            if (!short.TryParse(txtAnio.Text,out short anio)||anio<2000||anio>2100) { MessageBox.Show("Año inválido (2000-2100)."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    byte sem=byte.Parse(cmbSemestre.SelectedItem.ToString());
                    string sql=_idSel==-1
                        ? "INSERT INTO Gestion(semestre,anio) VALUES(@s,@a)"
                        : "UPDATE Gestion SET semestre=@s,anio=@a WHERE id_gestion=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@s",sem); cmd.Parameters.AddWithValue("@a",anio);
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
                using (var cmd=new SqlCommand("DELETE FROM Gestion WHERE id_gestion=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
