using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmPlanEstudio : Form
    {
        private int _idSel = -1;
        public FrmPlanEstudio() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarCarreras(); Cargar(); }

        private void CargarCarreras()
        {
            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter("SELECT codigo,descripcion FROM Carrera ORDER BY codigo", con))
            { var dt = new DataTable(); da.Fill(dt); cmbCarrera.DisplayMember="descripcion"; cmbCarrera.ValueMember="codigo"; cmbCarrera.DataSource=dt; cmbCarrera.SelectedIndex=-1; }
        }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter("SELECT p.num_plan, p.descripcion, c.descripcion AS carrera FROM Plan_Estudio p INNER JOIN Carrera c ON p.cod_carrera=c.codigo ORDER BY p.num_plan", con))
                { var dt = new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["num_plan"].Value);
            txtDesc.Text=dgv.CurrentRow.Cells["descripcion"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtDesc.Clear(); cmbCarrera.SelectedIndex=-1; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDesc.Text)) { MessageBox.Show("Ingrese descripción."); return; }
            if (cmbCarrera.SelectedValue==null) { MessageBox.Show("Seleccione carrera."); return; }
            try
            {
                using (var con = Conexion.ObtenerConexion())
                {
                    string sql = _idSel==-1
                        ? "INSERT INTO Plan_Estudio(descripcion,cod_carrera) VALUES(@d,@c)"
                        : "UPDATE Plan_Estudio SET descripcion=@d,cod_carrera=@c WHERE num_plan=@id";
                    using (var cmd = new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@d",txtDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@c",cmbCarrera.SelectedValue.ToString());
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
                using (var con = Conexion.ObtenerConexion())
                using (var cmd = new SqlCommand("DELETE FROM Plan_Estudio WHERE num_plan=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
