namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmAdministrativo
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblPersona=new System.Windows.Forms.Label(); this.cmbPersona=new System.Windows.Forms.ComboBox();
            this.lblCargo=new System.Windows.Forms.Label(); this.txtCargo=new System.Windows.Forms.TextBox();
            this.btnNuevo=new System.Windows.Forms.Button(); this.btnGuardar=new System.Windows.Forms.Button();
            this.btnEliminar=new System.Windows.Forms.Button(); this.btnCancelar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit(); this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=280; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionChanged+=new System.EventHandler(this.dgv_SelectionChanged);
            Helper.Etiqueta(lblPersona,"Persona:",10,290,80);
            this.cmbPersona.Location=new System.Drawing.Point(90,290); this.cmbPersona.Size=new System.Drawing.Size(350,22);
            this.cmbPersona.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblCargo,"Cargo:",10,322,80); Helper.Caja(txtCargo,90,322,250);
            Helper.Boton(btnNuevo,"Nuevo",10,360,System.Drawing.Color.SteelBlue);
            Helper.Boton(btnGuardar,"Guardar",110,360,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,360,System.Drawing.Color.Crimson);
            Helper.Boton(btnCancelar,"Cancelar",310,360,System.Drawing.Color.Gray);
            btnNuevo.Click+=new System.EventHandler(btnNuevo_Click); btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click); btnCancelar.Click+=new System.EventHandler(btnCancelar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(750,410); this.Text="Gestión de Administrativos";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblPersona,cmbPersona,lblCargo,txtCargo,btnNuevo,btnGuardar,btnEliminar,btnCancelar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblPersona,lblCargo;
        private System.Windows.Forms.ComboBox cmbPersona;
        private System.Windows.Forms.TextBox txtCargo;
        private System.Windows.Forms.Button btnNuevo,btnGuardar,btnEliminar,btnCancelar;
    }
}
