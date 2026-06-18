namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmAula
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblCodigo=new System.Windows.Forms.Label(); this.txtCodigo=new System.Windows.Forms.TextBox();
            this.lblCap=new System.Windows.Forms.Label(); this.txtCapacidad=new System.Windows.Forms.TextBox();
            this.lblUbi=new System.Windows.Forms.Label(); this.txtUbicacion=new System.Windows.Forms.TextBox();
            this.btnNuevo=new System.Windows.Forms.Button(); this.btnGuardar=new System.Windows.Forms.Button();
            this.btnEliminar=new System.Windows.Forms.Button(); this.btnCancelar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit(); this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=280; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionChanged+=new System.EventHandler(this.dgv_SelectionChanged);
            Helper.Etiqueta(lblCodigo,"Código:",10,290,80); Helper.Caja(txtCodigo,90,290,120);
            Helper.Etiqueta(lblCap,"Capacidad:",10,322,80); Helper.Caja(txtCapacidad,90,322,80);
            Helper.Etiqueta(lblUbi,"Ubicación:",10,354,80); Helper.Caja(txtUbicacion,90,354,300);
            Helper.Boton(btnNuevo,"Nuevo",10,390,System.Drawing.Color.SteelBlue);
            Helper.Boton(btnGuardar,"Guardar",110,390,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,390,System.Drawing.Color.Crimson);
            Helper.Boton(btnCancelar,"Cancelar",310,390,System.Drawing.Color.Gray);
            btnNuevo.Click+=new System.EventHandler(btnNuevo_Click); btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click); btnCancelar.Click+=new System.EventHandler(btnCancelar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(700,440); this.Text="Gestión de Aulas";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblCodigo,txtCodigo,lblCap,txtCapacidad,lblUbi,txtUbicacion,btnNuevo,btnGuardar,btnEliminar,btnCancelar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblCodigo,lblCap,lblUbi;
        private System.Windows.Forms.TextBox txtCodigo,txtCapacidad,txtUbicacion;
        private System.Windows.Forms.Button btnNuevo,btnGuardar,btnEliminar,btnCancelar;
    }
}
