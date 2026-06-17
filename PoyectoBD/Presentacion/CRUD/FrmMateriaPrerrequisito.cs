using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    /// <summary>
    /// CRUD para Materia_Prerrequisito.
    /// Regla de negocio: sigla_materia != sigla_prerrequisito (no autoreferencia).
    /// </summary>
    public partial class FrmMateriaPrerrequisito : Form
    {
        public FrmMateriaPrerrequisito() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarMaterias(); Cargar(); }

        private void CargarMaterias()
        {
            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter("SELECT sigla, nombre FROM Materia ORDER BY sigla", con))
            {
                var dt = new DataTable(); da.Fill(dt);
                cmbMateria.DisplayMember="nombre"; cmbMateria.ValueMember="sigla"; cmbMateria.DataSource=dt; cmbMateria.SelectedIndex=-1;
                var dt2 = dt.Copy();
                cmbPrereq.DisplayMember="nombre"; cmbPrereq.ValueMember="sigla"; cmbPrereq.DataSource=dt2; cmbPrereq.SelectedIndex=-1;
            }
        }

        private void Cargar()
        {
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var da = new SqlDataAdapter(
                    @"SELECT mp.sigla_materia, m1.nombre AS materia,
                             mp.sigla_prerrequisito, m2.nombre AS prerrequisito
                      FROM Materia_Prerrequisito mp
                      INNER JOIN Materia m1 ON mp.sigla_materia=m1.sigla
                      INNER JOIN Materia m2 ON mp.sigla_prerrequisito=m2.sigla
                      ORDER BY mp.sigla_materia", con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbMateria.SelectedValue==null||cmbPrereq.SelectedValue==null)
            { MessageBox.Show("Seleccione ambas materias."); return; }
            string m=cmbMateria.SelectedValue.ToString(), p=cmbPrereq.SelectedValue.ToString();
            // Regla: no autoreferencia
            if (m==p) { MessageBox.Show("Una materia no puede ser prerrequisito de sí misma.","Error de negocio",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd=new SqlCommand("INSERT INTO Materia_Prerrequisito(sigla_materia,sigla_prerrequisito) VALUES(@m,@p)",con))
                { cmd.Parameters.AddWithValue("@m",m); cmd.Parameters.AddWithValue("@p",p); cmd.ExecuteNonQuery(); }
                Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void btnEliminar_Click(object s, EventArgs e)
        {
            if (dgv.CurrentRow==null) { MessageBox.Show("Seleccione un registro."); return; }
            string m=dgv.CurrentRow.Cells["sigla_materia"].Value?.ToString();
            string p=dgv.CurrentRow.Cells["sigla_prerrequisito"].Value?.ToString();
            if (MessageBox.Show("¿Eliminar este prerrequisito?","Confirmar",MessageBoxButtons.YesNo)!=DialogResult.Yes) return;
            try
            {
                using (var con = Conexion.ObtenerConexion())
                using (var cmd=new SqlCommand("DELETE FROM Materia_Prerrequisito WHERE sigla_materia=@m AND sigla_prerrequisito=@p",con))
                { cmd.Parameters.AddWithValue("@m",m); cmd.Parameters.AddWithValue("@p",p); cmd.ExecuteNonQuery(); }
                Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
