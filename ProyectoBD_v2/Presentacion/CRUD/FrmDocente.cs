using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmDocente : Form
    {
        private int _idSel=-1;
        public FrmDocente() { InitializeComponent(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); CargarPersonas(); Cargar(); }

        private void CargarPersonas()
        {
            using (var con=Conexion.ObtenerConexion())
            using (var da=new SqlDataAdapter("SELECT idpersona, nombre+' ('+ci+')' AS display FROM Persona ORDER BY nombre",con))
            { var dt=new DataTable(); da.Fill(dt); cmbPersona.DisplayMember="display"; cmbPersona.ValueMember="idpersona"; cmbPersona.DataSource=dt; cmbPersona.SelectedIndex=-1; }
        }

        private void Cargar()
        {
            try
            {
                using (var con=Conexion.ObtenerConexion())
                using (var da=new SqlDataAdapter(
                    "SELECT d.cod_registro, p.nombre, p.ci, d.especialidad FROM Docente d INNER JOIN Persona p ON d.idpersona=p.idpersona ORDER BY p.nombre",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["cod_registro"].Value);
            txtEspecialidad.Text=dgv.CurrentRow.Cells["especialidad"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtEspecialidad.Clear(); cmbPersona.SelectedIndex=-1; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbPersona.SelectedValue==null)                    { MessageBox.Show("Seleccione persona.");     return; }
            if (string.IsNullOrWhiteSpace(txtEspecialidad.Text))   { MessageBox.Show("Ingrese especialidad.");   return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql = _idSel==-1
                        ? "INSERT INTO Docente(especialidad,idpersona) VALUES(@e,@p)"
                        : "UPDATE Docente SET especialidad=@e WHERE cod_registro=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@e",txtEspecialidad.Text.Trim());
                        if (_idSel==-1) cmd.Parameters.AddWithValue("@p",Convert.ToInt32(cmbPersona.SelectedValue));
                        else            cmd.Parameters.AddWithValue("@id",_idSel);
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
                using (var cmd=new SqlCommand("DELETE FROM Docente WHERE cod_registro=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
