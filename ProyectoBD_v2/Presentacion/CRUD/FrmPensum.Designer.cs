namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmPensum
    {
        private System.ComponentModel.IContainer components=null;
        protected override void Dispose(bool d){if(d&&components!=null)components.Dispose();base.Dispose(d);}
        private void InitializeComponent()
        {
            this.dgv=new System.Windows.Forms.DataGridView();
            this.lblPlan=new System.Windows.Forms.Label(); this.cmbPlan=new System.Windows.Forms.ComboBox();
            this.lblMateria=new System.Windows.Forms.Label(); this.cmbMateria=new System.Windows.Forms.ComboBox();
            this.lblSemestre=new System.Windows.Forms.Label(); this.txtSemestre=new System.Windows.Forms.TextBox();
            this.btnGuardar=new System.Windows.Forms.Button(); this.btnEliminar=new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            this.dgv.Dock=System.Windows.Forms.DockStyle.Top; this.dgv.Height=280; this.dgv.ReadOnly=true;
            this.dgv.SelectionMode=System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect=false; this.dgv.AllowUserToAddRows=false;
            this.dgv.AutoSizeColumnsMode=System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            Helper.Etiqueta(lblPlan,"Plan:",10,290,80);
            this.cmbPlan.Location=new System.Drawing.Point(90,290); this.cmbPlan.Size=new System.Drawing.Size(300,22);
            this.cmbPlan.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblMateria,"Materia:",10,322,80);
            this.cmbMateria.Location=new System.Drawing.Point(90,322); this.cmbMateria.Size=new System.Drawing.Size(300,22);
            this.cmbMateria.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            Helper.Etiqueta(lblSemestre,"Semestre:",10,354,80); Helper.Caja(txtSemestre,90,354,50);
            Helper.Boton(btnGuardar,"Agregar",110,390,System.Drawing.Color.Green);
            Helper.Boton(btnEliminar,"Eliminar",210,390,System.Drawing.Color.Crimson);
            btnGuardar.Click+=new System.EventHandler(btnGuardar_Click);
            btnEliminar.Click+=new System.EventHandler(btnEliminar_Click);
            this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F); this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize=new System.Drawing.Size(700,440); this.Text="Gestión de Pensum";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblPlan,cmbPlan,lblMateria,cmbMateria,lblSemestre,txtSemestre,btnGuardar,btnEliminar});
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit(); this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblPlan,lblMateria,lblSemestre;
        private System.Windows.Forms.ComboBox cmbPlan,cmbMateria;
        private System.Windows.Forms.TextBox txtSemestre;
        private System.Windows.Forms.Button btnGuardar,btnEliminar;
    }
}
