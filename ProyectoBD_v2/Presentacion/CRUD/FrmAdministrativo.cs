using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.CRUD
{
    public partial class FrmAdministrativo : Form
    {
        private int _idSel=-1;
        public FrmAdministrativo() { InitializeComponent(); }
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
                using (var da=new SqlDataAdapter("SELECT a.id_admin, p.nombre, p.ci, a.cargo FROM Administrativo a INNER JOIN Persona p ON a.idpersona=p.idpersona ORDER BY p.nombre",con))
                { var dt=new DataTable(); da.Fill(dt); dgv.DataSource=dt; }
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow==null) return;
            _idSel=Convert.ToInt32(dgv.CurrentRow.Cells["id_admin"].Value);
            txtCargo.Text=dgv.CurrentRow.Cells["cargo"].Value?.ToString();
        }

        private void btnNuevo_Click(object s, EventArgs e)    { _idSel=-1; txtCargo.Clear(); cmbPersona.SelectedIndex=-1; dgv.ClearSelection(); }
        private void btnCancelar_Click(object s, EventArgs e) { btnNuevo_Click(s,e); }

        private void btnGuardar_Click(object s, EventArgs e)
        {
            if (cmbPersona.SelectedValue==null)         { MessageBox.Show("Seleccione persona."); return; }
            if (string.IsNullOrWhiteSpace(txtCargo.Text)){ MessageBox.Show("Ingrese cargo.");     return; }
            try
            {
                using (var con=Conexion.ObtenerConexion())
                {
                    string sql = _idSel==-1
                        ? "INSERT INTO Administrativo(cargo,idpersona) VALUES(@c,@p)"
                        : "UPDATE Administrativo SET cargo=@c WHERE id_admin=@id";
                    using (var cmd=new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@c",txtCargo.Text.Trim());
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
                using (var cmd=new SqlCommand("DELETE FROM Administrativo WHERE id_admin=@id",con))
                { cmd.Parameters.AddWithValue("@id",_idSel); cmd.ExecuteNonQuery(); }
                btnNuevo_Click(null,null); Cargar();
            }
            catch (Exception ex) { MessageBox.Show("Error: "+ex.Message); }
        }
    }
}
