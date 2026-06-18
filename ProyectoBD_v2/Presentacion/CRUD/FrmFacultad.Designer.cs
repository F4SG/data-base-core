namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmFacultad
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }
        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();

            this.dgv.Dock = System.Windows.Forms.DockStyle.Top; this.dgv.Height = 300;
            this.dgv.ReadOnly = true; this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect = false; this.dgv.AllowUserToAddRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);

            Helper.Etiqueta(this.lblNombre, "Nombre:", 10, 310, 80);
            Helper.Caja(this.txtNombre, 90, 310, 300);
            Helper.Boton(this.btnNuevo,    "Nuevo",    10,  350, System.Drawing.Color.SteelBlue);
            Helper.Boton(this.btnGuardar,  "Guardar",  110, 350, System.Drawing.Color.Green);
            Helper.Boton(this.btnEliminar, "Eliminar", 210, 350, System.Drawing.Color.Crimson);
            Helper.Boton(this.btnCancelar, "Cancelar", 310, 350, System.Drawing.Color.Gray);
            this.btnNuevo.Click    += new System.EventHandler(this.btnNuevo_Click);
            this.btnGuardar.Click  += new System.EventHandler(this.btnGuardar_Click);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 400);
            this.Controls.AddRange(new System.Windows.Forms.Control[]{dgv,lblNombre,txtNombre,btnNuevo,btnGuardar,btnEliminar,btnCancelar});
            this.Text = "Gestión de Facultades";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnNuevo, btnGuardar, btnEliminar, btnCancelar;
    }
}
