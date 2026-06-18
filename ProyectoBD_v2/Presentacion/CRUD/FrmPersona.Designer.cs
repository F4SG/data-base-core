namespace UniversidadXYZ.Presentacion.CRUD
{
    partial class FrmPersona
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.dgv        = new System.Windows.Forms.DataGridView();
            this.pnlCampos  = new System.Windows.Forms.Panel();
            this.lblCI      = new System.Windows.Forms.Label();
            this.txtCI      = new System.Windows.Forms.TextBox();
            this.lblNombre  = new System.Windows.Forms.Label();
            this.txtNombre  = new System.Windows.Forms.TextBox();
            this.lblSexo    = new System.Windows.Forms.Label();
            this.cmbSexo    = new System.Windows.Forms.ComboBox();
            this.lblFechaNac= new System.Windows.Forms.Label();
            this.dtpFechaNac= new System.Windows.Forms.DateTimePicker();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnNuevo   = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar= new System.Windows.Forms.Button();
            this.btnCancelar= new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlCampos.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();

            // dgv
            this.dgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv.Height = 300;
            this.dgv.ReadOnly = true;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.MultiSelect = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.Name = "dgv";
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);

            // pnlCampos
            int x = 10, y = 10, lw = 100, tw = 200, rh = 28;
            this.pnlCampos.Location = new System.Drawing.Point(0, 300);
            this.pnlCampos.Size = new System.Drawing.Size(650, 140);
            this.pnlCampos.Name = "pnlCampos";

            Helper.Etiqueta(this.lblCI,       "C.I.:",           x, y,      lw);
            Helper.Caja(this.txtCI,                               x+lw, y,   tw);
            Helper.Etiqueta(this.lblNombre,   "Nombre:",         x, y+rh,   lw);
            Helper.Caja(this.txtNombre,                           x+lw, y+rh, tw);
            Helper.Etiqueta(this.lblSexo,     "Sexo:",           x, y+2*rh, lw);
            this.cmbSexo.Items.AddRange(new object[]{"M","F"});
            this.cmbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSexo.Location = new System.Drawing.Point(x+lw, y+2*rh);
            this.cmbSexo.Size     = new System.Drawing.Size(100, 22);
            this.cmbSexo.Name     = "cmbSexo";
            Helper.Etiqueta(this.lblFechaNac, "Fecha Nac.:",     x, y+3*rh, lw);
            this.dtpFechaNac.Location = new System.Drawing.Point(x+lw, y+3*rh);
            this.dtpFechaNac.Size     = new System.Drawing.Size(180, 22);
            this.dtpFechaNac.Format   = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaNac.Name     = "dtpFechaNac";
            this.pnlCampos.Controls.AddRange(new System.Windows.Forms.Control[]{
                lblCI,txtCI,lblNombre,txtNombre,lblSexo,cmbSexo,lblFechaNac,dtpFechaNac});

            // pnlBotones
            this.pnlBotones.Location = new System.Drawing.Point(0, 440);
            this.pnlBotones.Size = new System.Drawing.Size(650, 40);
            this.pnlBotones.Name = "pnlBotones";
            Helper.Boton(this.btnNuevo,    "Nuevo",    10,  5, System.Drawing.Color.SteelBlue);
            Helper.Boton(this.btnGuardar,  "Guardar",  110, 5, System.Drawing.Color.Green);
            Helper.Boton(this.btnEliminar, "Eliminar", 210, 5, System.Drawing.Color.Crimson);
            Helper.Boton(this.btnCancelar, "Cancelar", 310, 5, System.Drawing.Color.Gray);
            this.btnNuevo.Click    += new System.EventHandler(this.btnNuevo_Click);
            this.btnGuardar.Click  += new System.EventHandler(this.btnGuardar_Click);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.pnlBotones.Controls.AddRange(new System.Windows.Forms.Control[]{
                btnNuevo,btnGuardar,btnEliminar,btnCancelar});

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlCampos);
            this.Controls.Add(this.pnlBotones);
            this.Name = "FrmPersona";
            this.Text = "Gestión de Personas";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlCampos.ResumeLayout(false);
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlCampos, pnlBotones;
        private System.Windows.Forms.Label lblCI, lblNombre, lblSexo, lblFechaNac;
        private System.Windows.Forms.TextBox txtCI, txtNombre;
        private System.Windows.Forms.ComboBox cmbSexo;
        private System.Windows.Forms.DateTimePicker dtpFechaNac;
        private System.Windows.Forms.Button btnNuevo, btnGuardar, btnEliminar, btnCancelar;
    }

    /// <summary>Helper estático para construir controles de forma compacta en todos los formularios CRUD.</summary>
    internal static class Helper
    {
        internal static void Etiqueta(System.Windows.Forms.Label lbl, string texto, int x, int y, int w)
        {
            lbl.Text = texto; lbl.Location = new System.Drawing.Point(x, y+4);
            lbl.Size = new System.Drawing.Size(w-5, 18); lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        }
        internal static void Caja(System.Windows.Forms.TextBox txt, int x, int y, int w)
        {
            txt.Location = new System.Drawing.Point(x, y); txt.Size = new System.Drawing.Size(w, 22);
        }
        internal static void Boton(System.Windows.Forms.Button btn, string texto, int x, int y, System.Drawing.Color color)
        {
            btn.Text = texto; btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(90, 30);
            btn.BackColor = color; btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }
    }
}
