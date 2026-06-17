using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmEdicionMateria : Form
    {
        private int _idSel=-1;
        public FrmEdicionMateria() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarCombos(); Cargar(); }

        private void CargarCombos()
        {
            using (var con=Conexion.ObtenerConexion())
            {
                using (var da=new SqlDataAdapter("SELECT sigla,nombre FROM Materia ORDER BY sigla",con))
                { var dt=new DataTable(); da.Fill(dt); cmbMateria.DisplayMember="nombre"; cmbMateria.ValueMember="sigla"; cmbMateria.DataSource=dt; cmbMateria.SelectedIndex=-1; }
                using (var da=new SqlDataAdapter("SELECT cod_registro, (SELECT nombre FROM Persona WHERE idpersona=d.idpersona) AS nombre FROM Docente d ORDER BY cod_registro",con))
                { var dt=new DataTable(); da.Fill(dt); cmbDocente.DisplayMember="nombre"; cmbDocente.ValueMember="cod_registro"; cmbDocente.DataSource=dt; cmbDocente.SelectedIndex=-1; }
                using (var da=new SqlDataAdapter("SELECT id_aula,codigo FROM Aula ORDER BY codigo",con))
                { var dt=new DataTable(); da.Fill(dt); cmbAula.DisplayMember="codigo"; cmbAula.ValueMember="id_aula"; cmbAula.DataSource=dt; cmbAula.SelectedIndex=-1; }
                using (var da=new SqlDataAdapter("SELECT id_horario, dia_semana+' '+CONVERT(VARCHAR,hora_inicio,108)+'-'+CONVERT(VARCHAR,hora_fin,108) AS etiqueta FROM Horario ORDER BY dia_semana",con))
                { var dt=new DataTable(); da.Fill(dt); cmbHorario.DisplayMember="etiqueta"; cmbHorario.ValueMember="id_horario"; cmbHorario.DataSource=dt; cmbHorario.SelectedIndex=-1; }
                using (var da=new SqlDataAdapter("SELECT id_gestion, CAST(semestre AS VARCHAR)+'/'+CAST(anio AS VARCHAR) AS etiqueta FROM Gestion ORDER BY anio,semestre",con))
                { var dt=new DataTable(); da.Fill(dt); cmbGestion.DisplayMember="etiqueta"; cmbGestion.ValueMember="id_gestion"; cmbGestion.DataSource=dt; cmbGestion.SelectedIndex=-1; }
            }
        }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter(
                    @"SELECT em.cod_edicion, m.nombre AS materia, p.nombre AS docente,
                             a.codigo AS aula, em.fecha_inicio, em.fecha_fin,
                             CAST(g.semestre AS VARCHAR)+'/'+CAST(g.anio AS VARCHAR) AS gestion
                      FROM Edicion_Materia em
                      INNER JOIN Materia m ON em.sigla_materia=m.sigla
                      INNER JOIN Docente d ON em.cod_docente=d.cod_registro
                      INNER JOIN Persona p ON d.idpersona=p.idpersona
                      INNER JOIN Aula a ON em.id_aula=a.id_aula
                      INNER JOIN Gestion g ON em.id_gestion=g.id_gestion
                      ORDER BY em.cod_edicion",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["cod_edicion"].Value);
            if (dgv.CurrentRow.Cells["fecha_inicio"].Value!=DBNull.Value)
                dtpInicio.Value=Convert.ToDateTime(dgv.CurrentRow.Cells["fecha_inicio"].Value);
            if (dgv.CurrentRow.Cells["fecha_fin"].Value!=DBNull.Value)
                dtpFin.Value=Convert.ToDateTime(dgv.CurrentRow.Cells["fecha_fin"].Value);
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; cmbMateria.SelectedIndex=-1; cmbDocente.SelectedIndex=-1; cmbAula.SelectedIndex=-1; cmbHorario.SelectedIndex=-1; cmbGestion.SelectedIndex=-1; dtpInicio.Value=DateTime.Today; dtpFin.Value=DateTime.Today.AddMonths(4); dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbMateria.SelectedValue==null||cmbDocente.SelectedValue==null||cmbAula.SelectedValue==null||cmbHorario.SelectedValue==null||cmbGestion.SelectedValue==null)
            { MessageBox.Show("Complete todos los campos."); return; }
            if (dtpFin.Value<=dtpInicio.Value) { MessageBox.Show("Fecha fin debe ser > fecha inicio."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql=_idSel==-1
                        ? "INSERT INTO Edicion_Materia(fecha_inicio,fecha_fin,sigla_materia,cod_docente,id_aula,id_horario,id_gestion) VALUES(@fi,@ff,@m,@d,@a,@h,@g)"
                        : "UPDATE Edicion_Materia SET fecha_inicio=@fi,fecha_fin=@ff,sigla_materia=@m,cod_docente=@d,id_aula=@a,id_horario=@h,id_gestion=@g WHERE cod_edicion=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@fi",dtpInicio.Value.Date);
                        cmd.Parameters.AddWithValue("@ff",dtpFin.Value.Date);
                        cmd.Parameters.AddWithValue("@m",cmbMateria.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@d",Convert.ToInt32(cmbDocente.SelectedValue));
                        cmd.Parameters.AddWithValue("@a",Convert.ToInt32(cmbAula.SelectedValue));
                        cmd.Parameters.AddWithValue("@h",Convert.ToInt32(cmbHorario.SelectedValue));
                        cmd.Parameters.AddWithValue("@g",Convert.ToInt32(cmbGestion.SelectedValue));
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
                using (var cmd=new SqlCommand("DELETE FROM Edicion_Materia WHERE cod_edicion=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
