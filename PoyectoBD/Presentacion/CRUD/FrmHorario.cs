using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmHorario : Form
    {
        private int _idSel=-1;
        private static readonly string[] Dias = {"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado"};

        public FrmHorario() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); Cargar(); }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter("SELECT id_horario, dia_semana, hora_inicio, hora_fin, fecha_asistencia FROM Horario ORDER BY dia_semana,hora_inicio",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["id_horario"].Value);
            cmbDia.Text=dgv.CurrentRow.Cells["dia_semana"].Value?.ToString();
            if (dgv.CurrentRow.Cells["hora_inicio"].Value!=DBNull.Value)
                dtpInicio.Value=DateTime.Today.Add((TimeSpan)dgv.CurrentRow.Cells["hora_inicio"].Value);
            if (dgv.CurrentRow.Cells["hora_fin"].Value!=DBNull.Value)
                dtpFin.Value=DateTime.Today.Add((TimeSpan)dgv.CurrentRow.Cells["hora_fin"].Value);
            if (dgv.CurrentRow.Cells["fecha_asistencia"].Value!=DBNull.Value)
                dtpFechaAsist.Value=Convert.ToDateTime(dgv.CurrentRow.Cells["fecha_asistencia"].Value);
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; cmbDia.SelectedIndex=-1; dtpInicio.Value=DateTime.Today; dtpFin.Value=DateTime.Today; dtpFechaAsist.Value=DateTime.Today; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbDia.SelectedIndex<0) { MessageBox.Show("Seleccione día."); return; }
            if (dtpFin.Value.TimeOfDay<=dtpInicio.Value.TimeOfDay) { MessageBox.Show("Hora fin debe ser > hora inicio."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql=_idSel==-1
                        ? "INSERT INTO Horario(dia_semana,hora_inicio,hora_fin,fecha_asistencia) VALUES(@d,@hi,@hf,@fa)"
                        : "UPDATE Horario SET dia_semana=@d,hora_inicio=@hi,hora_fin=@hf,fecha_asistencia=@fa WHERE id_horario=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@d",cmbDia.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@hi",dtpInicio.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@hf",dtpFin.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@fa",dtpFechaAsist.Value.Date);
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
                using (var cmd=new SqlCommand("DELETE FROM Horario WHERE id_horario=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
