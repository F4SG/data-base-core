namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmMateriaPrerrequisito
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblMateria=new System.Windows.Forms.Label(); this.cmbMateria=new System.Windows.Forms.ComboBox();
            this.lblPrereq=new System.Windows.Forms.Label(); this.cmbPrereq=new System.Windows.Forms.ComboBox();
            this.btnGuardar=new System.Windows.Forms.Button(); this.btnEliminar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=280; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            Helper.Etiqueta(lblMateria,"Materia:",10,290,90);
            this.cmbMateria.Location=new System.Drawing.Point(100,290); this.cmbMateria.Size=new System.Drawing.Size(300,22);
            this.cmbMateria.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblPrereq,"Prerrequisito:",10,322,90);
            this.cmbPrereq.Location=new System.Drawing.Point(100,322); this.cmbPrereq.Size=new System.Drawing.Size(300,22);
            this.cmbPrereq.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Boton(btnGuardar,"Agregar",110,360,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,360,System.Drawing.Color.Crimson);
            btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(700,410); this.Text="Gestión de Prerrequisitos";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblMateria,cmbMateria,lblPrereq,cmbPrereq,btnGuardar,btnEliminar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblMateria,lblPrereq;
        private System.Windows.Forms.ComboBox cmbMateria,cmbPrereq;
        private System.Windows.Forms.Button btnGuardar,btnEliminar;
    }
}
