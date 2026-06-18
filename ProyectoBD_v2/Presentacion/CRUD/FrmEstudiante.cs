using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmEstudiante : Form
    {
        private int _idSel=-1;
        public FrmEstudiante() { InitializeComponent(); }
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
                    "SELECT e.nro_registro, p.nombre, p.ci, e.cuenta_usuario FROM Estudiante e INNER JOIN Persona p ON e.idpersona=p.idpersona ORDER BY p.nombre",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["nro_registro"].Value);
            txtCuenta.Text=dgv.CurrentRow.Cells["cuenta_usuario"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtCuenta.Clear(); txtPin.Clear(); cmbPersona.SelectedIndex=-1; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbPersona.SelectedValue==null)           { MessageBox.Show("Seleccione persona.");   return; }
            if (string.IsNullOrWhiteSpace(txtCuenta.Text)){ MessageBox.Show("Ingrese cuenta.");       return; }
            if (_idSel==-1&&string.IsNullOrWhiteSpace(txtPin.Text)){ MessageBox.Show("Ingrese PIN."); return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql = _idSel==-1
                        ? "INSERT INTO Estudiante(cuenta_usuario,pin,idpersona) VALUES(@c,@p,@id)"
                        : "UPDATE Estudiante SET cuenta_usuario=@c"+(string.IsNullOrWhiteSpace(txtPin.Text)?"":",pin=@p")+" WHERE nro_registro=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@c",txtCuenta.Text.Trim());
                        if (!string.IsNullOrWhiteSpace(txtPin.Text)) cmd.Parameters.AddWithValue("@p",txtPin.Text);
                        if (_idSel==-1) cmd.Parameters.AddWithValue("@id",Convert.ToInt32(cmbPersona.SelectedValue));
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
                using (var cmd=new SqlCommand("DELETE FROM Estudiante WHERE nro_registro=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
