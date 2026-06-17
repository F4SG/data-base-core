namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmEdicionMateria
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblMateria=new System.Windows.Forms.Label(); this.cmbMateria=new System.Windows.Forms.ComboBox();
            this.lblDocente=new System.Windows.Forms.Label(); this.cmbDocente=new System.Windows.Forms.ComboBox();
            this.lblAula=new System.Windows.Forms.Label(); this.cmbAula=new System.Windows.Forms.ComboBox();
            this.lblHorario=new System.Windows.Forms.Label(); this.cmbHorario=new System.Windows.Forms.ComboBox();
            this.lblGestion=new System.Windows.Forms.Label(); this.cmbGestion=new System.Windows.Forms.ComboBox();
            this.lblInicio=new System.Windows.Forms.Label(); this.dtpInicio=new System.Windows.Forms.DateTimePicker();
            this.lblFin=new System.Windows.Forms.Label(); this.dtpFin=new System.Windows.Forms.DateTimePicker();
            this.btnNuevo=new System.Windows.Forms.Button(); this.btnGuardar=new System.Windows.Forms.Button();
            this.btnEliminar=new System.Windows.Forms.Button(); this.btnCancelar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit(); this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=200; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionChanged+=new System.EventHandler(this.dgv_SelectionChanged);
            int x=10,lw=90,cw=280,y=210,rh=28;
            Helper.Etiqueta(lblMateria,"Materia:",x,y,lw); this.cmbMateria.Location=new System.Drawing.Point(x+lw,y); this.cmbMateria.Size=new System.Drawing.Size(cw,22); this.cmbMateria.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblDocente,"Docente:",x,y+rh,lw); this.cmbDocente.Location=new System.Drawing.Point(x+lw,y+rh); this.cmbDocente.Size=new System.Drawing.Size(cw,22); this.cmbDocente.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblAula,"Aula:",x,y+2*rh,lw); this.cmbAula.Location=new System.Drawing.Point(x+lw,y+2*rh); this.cmbAula.Size=new System.Drawing.Size(150,22); this.cmbAula.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblHorario,"Horario:",x,y+3*rh,lw); this.cmbHorario.Location=new System.Drawing.Point(x+lw,y+3*rh); this.cmbHorario.Size=new System.Drawing.Size(cw,22); this.cmbHorario.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblGestion,"Gestión:",x,y+4*rh,lw); this.cmbGestion.Location=new System.Drawing.Point(x+lw,y+4*rh); this.cmbGestion.Size=new System.Drawing.Size(100,22); this.cmbGestion.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblInicio,"Inicio:",x,y+5*rh,lw); this.dtpInicio.Location=new System.Drawing.Point(x+lw,y+5*rh); this.dtpInicio.Size=new System.Drawing.Size(140,22); this.dtpInicio.Format=System.Windows.Forms.DateTimePickerFormat.Short;
            Helper.Etiqueta(lblFin,"Fin:",x,y+6*rh,lw); this.dtpFin.Location=new System.Drawing.Point(x+lw,y+6*rh); this.dtpFin.Size=new System.Drawing.Size(140,22); this.dtpFin.Format=System.Windows.Forms.DateTimePickerFormat.Short;
            Helper.Boton(btnNuevo,"Nuevo",10,y+7*rh+4,System.Drawing.Color.SteelBlue);
            Helper.Boton(btnGuardar,"Guardar",110,y+7*rh+4,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,y+7*rh+4,System.Drawing.Color.Crimson);
            Helper.Boton(btnCancelar,"Cancelar",310,y+7*rh+4,System.Drawing.Color.Gray);
            btnNuevo.Click+=new System.EventHandler(btnNuevo_Click); btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click); btnCancelar.Click+=new System.EventHandler(btnCancelar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(800,y+8*rh+10); this.Text="Gestión de Ediciones de Materia";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblMateria,cmbMateria,lblDocente,cmbDocente,lblAula,cmbAula,lblHorario,cmbHorario,lblGestion,cmbGestion,lblInicio,dtpInicio,lblFin,dtpFin,btnNuevo,btnGuardar,btnEliminar,btnCancelar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblMateria,lblDocente,lblAula,lblHorario,lblGestion,lblInicio,lblFin;
        private System.Windows.Forms.ComboBox cmbMateria,cmbDocente,cmbAula,cmbHorario,cmbGestion;
        private System.Windows.Forms.DateTimePicker dtpInicio,dtpFin;
        private System.Windows.Forms.Button btnNuevo,btnGuardar,btnEliminar,btnCancelar;
    }
}
