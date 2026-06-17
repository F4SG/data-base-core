using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    /// <summary>
    /// CRUD para Pensum.
    /// Regla: semestre_en_plan entre 1 y 12.
    /// </summary>
    public partial class FrmPensum : Form
    {
        public FrmPensum() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarCombos(); Cargar(); }

        private void CargarCombos()
        {
            using (var con = Conexion.ObtenerConexion())
            {
                using (var da=new SqlDataAdapter("SELECT num_plan,descripcion FROM Plan_Estudio ORDER BY num_plan",con))
                { var dt=new DataTable(); da.Fill(dt); cmbPlan.DisplayMember="descripcion"; cmbPlan.ValueMember="num_plan"; cmbPlan.DataSource=dt; cmbPlan.SelectedIndex=-1; }
                using (var da=new SqlDataAdapter("SELECT sigla,nombre FROM Materia ORDER BY sigla",con))
                { var dt=new DataTable(); da.Fill(dt); cmbMateria.DisplayMember="nombre"; cmbMateria.ValueMember="sigla"; cmbMateria.DataSource=dt; cmbMateria.SelectedIndex=-1; }
            }
        }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter(
                    @"SELECT ps.num_plan, pe.descripcion AS plan, ps.sigla_materia, m.nombre AS materia, ps.semestre_en_plan
                      FROM Pensum ps
                      INNER JOIN Plan_Estudio pe ON ps.num_plan=pe.num_plan
                      INNER JOIN Materia m ON ps.sigla_materia=m.sigla
                      ORDER BY ps.num_plan, ps.semestre_en_plan",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbPlan.SelectedValue==null||cmbMateria.SelectedValue==null) { MessageBox.Show("Seleccione plan y materia."); return; }
            if (!byte.TryParse(txtSemestre.Text,out byte sem)||sem<1||sem>12) { MessageBox.Show("Semestre debe ser entre 1 y 12."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var cmd=new SqlCommand("INSERT INTO Pensum(num_plan,sigla_materia,semestre_en_plan) VALUES(@p,@m,@s)",con))
                { cmd.Parameters.AddWithValue("@p",Convert.ToInt32(cmbPlan.SelectedValue));
                  cmd.Parameters.AddWithValue("@m",cmbMateria.SelectedValue.ToString());
                  cmd.Parameters.AddWithValue("@s",sem); cmd.ExecuteNonQuery(); }
                Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnEliminar_Click(object s, EventArgs e)
        {
            if (dgv.CurrentRow==null) { MessageBox.Show("Seleccione un registro."); return; }
            int plan=Convert.ToInt32(dgv.CurrentRow.Cells["num_plan"].Value);
            string sig=dgv.CurrentRow.Cells["sigla_materia"].Value?.ToString();
            if (MessageBox.Show("¿Eliminar?","Confirmar",MessageBoxButtons.YesNo)!=DialogResult.Yes) return;
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var cmd=new SqlCommand("DELETE FROM Pensum WHERE num_plan=@p AND sigla_materia=@m",con))
                { cmd.Parameters.AddWithValue("@p",plan); cmd.Parameters.AddWithValue("@m",sig); cmd.ExecuteNonQuery(); }
                Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
