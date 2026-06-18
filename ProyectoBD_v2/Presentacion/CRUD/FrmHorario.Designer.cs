namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmHorario
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblDia=new System.Windows.Forms.Label(); this.cmbDia=new System.Windows.Forms.ComboBox();
            this.lblInicio=new System.Windows.Forms.Label(); this.dtpInicio=new System.Windows.Forms.DateTimePicker();
            this.lblFin=new System.Windows.Forms.Label(); this.dtpFin=new System.Windows.Forms.DateTimePicker();
            this.lblFechaA=new System.Windows.Forms.Label(); this.dtpFechaAsist=new System.Windows.Forms.DateTimePicker();
            this.btnNuevo=new System.Windows.Forms.Button(); this.btnGuardar=new System.Windows.Forms.Button();
            this.btnEliminar=new System.Windows.Forms.Button(); this.btnCancelar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit(); this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=260; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionChanged+=new System.EventHandler(this.dgv_SelectionChanged);
            Helper.Etiqueta(lblDia,"Día:",10,270,90);
            this.cmbDia.Items.AddRange(new object[]{"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado"});
            this.cmbDia.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDia.Location=new System.Drawing.Point(100,270); this.cmbDia.Size=new System.Drawing.Size(130,22);
            Helper.Etiqueta(lblInicio,"Hora inicio:",10,300,90);
            this.dtpInicio.Location=new System.Drawing.Point(100,300); this.dtpInicio.Size=new System.Drawing.Size(120,22);
            this.dtpInicio.Format=System.Windows.Forms.DateTimePickerFormat.Time; this.dtpInicio.ShowUpDown=true;
            Helper.Etiqueta(lblFin,"Hora fin:",10,330,90);
            this.dtpFin.Location=new System.Drawing.Point(100,330); this.dtpFin.Size=new System.Drawing.Size(120,22);
            this.dtpFin.Format=System.Windows.Forms.DateTimePickerFormat.Time; this.dtpFin.ShowUpDown=true;
            Helper.Etiqueta(lblFechaA,"Fecha clase:",10,360,90);
            this.dtpFechaAsist.Location=new System.Drawing.Point(100,360); this.dtpFechaAsist.Size=new System.Drawing.Size(150,22);
            this.dtpFechaAsist.Format=System.Windows.Forms.DateTimePickerFormat.Short;
            Helper.Boton(btnNuevo,"Nuevo",10,400,System.Drawing.Color.SteelBlue);
            Helper.Boton(btnGuardar,"Guardar",110,400,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,400,System.Drawing.Color.Crimson);
            Helper.Boton(btnCancelar,"Cancelar",310,400,System.Drawing.Color.Gray);
            btnNuevo.Click+=new System.EventHandler(btnNuevo_Click); btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click); btnCancelar.Click+=new System.EventHandler(btnCancelar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(750,445); this.Text="Gestión de Horarios";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblDia,cmbDia,lblInicio,dtpInicio,lblFin,dtpFin,lblFechaA,dtpFechaAsist,btnNuevo,btnGuardar,btnEliminar,btnCancelar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblDia,lblInicio,lblFin,lblFechaA;
        private System.Windows.Forms.ComboBox cmbDia;
        private System.Windows.Forms.DateTimePicker dtpInicio,dtpFin,dtpFechaAsist;
        private System.Windows.Forms.Button btnNuevo,btnGuardar,btnEliminar,btnCancelar;
    }
}
