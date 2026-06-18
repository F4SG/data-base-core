namespace UniversidadXYZ.Presentacion.Procesos
{
    partial class FrmInscripcion
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            // ── Controles de cabecera ──────────────────────────────────
            this.lblTitulo          = new System.Windows.Forms.Label();
            this.lblNroReg          = new System.Windows.Forms.Label();
            this.txtNroRegistro     = new System.Windows.Forms.TextBox();
            this.lblNomEst          = new System.Windows.Forms.Label();
            this.txtNombreEstudiante= new System.Windows.Forms.TextBox();
            this.lblPlan            = new System.Windows.Forms.Label();
            this.txtNumPlan         = new System.Windows.Forms.TextBox();
            this.lblCarreraLabel    = new System.Windows.Forms.Label();
            this.txtCarrera         = new System.Windows.Forms.TextBox();
            this.lblGestion         = new System.Windows.Forms.Label();
            this.cmbGestion         = new System.Windows.Forms.ComboBox();
            this.btnCargarMaterias  = new System.Windows.Forms.Button();
            // ── DataGridView ──────────────────────────────────────────
            this.dgvMaterias        = new System.Windows.Forms.DataGridView();
            // ── Botones inferiores ─────────────────────────────────────
            this.btnRegistrar       = new System.Windows.Forms.Button();
            this.btnLimpiar         = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterias)).BeginInit();
            this.SuspendLayout();

            // Título
            this.lblTitulo.Text      = "INSCRIPCIÓN DE MATERIAS";
            this.lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location  = new System.Drawing.Point(10, 8);
            this.lblTitulo.Size      = new System.Drawing.Size(400, 28);

            // Fila 1: Nro Registro
            this.lblNroReg.Text      = "Nro. Registro:";
            this.lblNroReg.Location  = new System.Drawing.Point(10, 48); this.lblNroReg.Size = new System.Drawing.Size(100, 20); this.lblNroReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtNroRegistro.Location= new System.Drawing.Point(115, 45); this.txtNroRegistro.Size = new System.Drawing.Size(90, 22); this.txtNroRegistro.Name = "txtNroRegistro";
            this.txtNroRegistro.Leave  += new System.EventHandler(this.txtNroRegistro_Leave);
            this.lblNomEst.Text      = "Nombre:";
            this.lblNomEst.Location  = new System.Drawing.Point(215, 48); this.lblNomEst.Size = new System.Drawing.Size(60, 20); this.lblNomEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtNombreEstudiante.Location= new System.Drawing.Point(280, 45); this.txtNombreEstudiante.Size = new System.Drawing.Size(280, 22); this.txtNombreEstudiante.ReadOnly = true; this.txtNombreEstudiante.BackColor = System.Drawing.Color.LightYellow;

            // Fila 2: Plan / Carrera
            this.lblPlan.Text        = "Cód. Plan:";
            this.lblPlan.Location    = new System.Drawing.Point(10, 76); this.lblPlan.Size = new System.Drawing.Size(100, 20); this.lblPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtNumPlan.Location = new System.Drawing.Point(115, 73); this.txtNumPlan.Size = new System.Drawing.Size(70, 22); this.txtNumPlan.Name = "txtNumPlan";
            this.txtNumPlan.Leave   += new System.EventHandler(this.txtNumPlan_Leave);
            this.lblCarreraLabel.Text= "Carrera:";
            this.lblCarreraLabel.Location= new System.Drawing.Point(195, 76); this.lblCarreraLabel.Size = new System.Drawing.Size(60, 20); this.lblCarreraLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtCarrera.Location = new System.Drawing.Point(260, 73); this.txtCarrera.Size = new System.Drawing.Size(300, 22); this.txtCarrera.ReadOnly = true; this.txtCarrera.BackColor = System.Drawing.Color.LightYellow;

            // Fila 3: Gestión + Cargar
            this.lblGestion.Text     = "Gestión:";
            this.lblGestion.Location = new System.Drawing.Point(10, 104); this.lblGestion.Size = new System.Drawing.Size(100, 20); this.lblGestion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbGestion.Location = new System.Drawing.Point(115, 101); this.cmbGestion.Size = new System.Drawing.Size(120, 22); this.cmbGestion.Name = "cmbGestion"; this.cmbGestion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.btnCargarMaterias.Text     = "Cargar Materias";
            this.btnCargarMaterias.Location = new System.Drawing.Point(250, 99);
            this.btnCargarMaterias.Size     = new System.Drawing.Size(130, 28);
            this.btnCargarMaterias.BackColor= System.Drawing.Color.SteelBlue;
            this.btnCargarMaterias.ForeColor= System.Drawing.Color.White;
            this.btnCargarMaterias.FlatStyle= System.Windows.Forms.FlatStyle.Flat;
            this.btnCargarMaterias.Click   += new System.EventHandler(this.btnCargarMaterias_Click);

            // DataGridView
            this.dgvMaterias.Location = new System.Drawing.Point(10, 140);
            this.dgvMaterias.Size     = new System.Drawing.Size(760, 330);
            this.dgvMaterias.Name     = "dgvMaterias";
            this.dgvMaterias.AllowUserToAddRows    = false;
            this.dgvMaterias.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMaterias.EditMode              = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;

            // Botones inferiores
            this.btnRegistrar.Text      = "✓  REGISTRAR INSCRIPCIÓN";
            this.btnRegistrar.Location  = new System.Drawing.Point(10, 485);
            this.btnRegistrar.Size      = new System.Drawing.Size(210, 34);
            this.btnRegistrar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.Click    += new System.EventHandler(this.btnRegistrar_Click);

            this.btnLimpiar.Text      = "Limpiar";
            this.btnLimpiar.Location  = new System.Drawing.Point(235, 485);
            this.btnLimpiar.Size      = new System.Drawing.Size(90, 34);
            this.btnLimpiar.BackColor = System.Drawing.Color.Gray;
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Click    += new System.EventHandler(this.btnLimpiar_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(790, 535);
            this.Text                = "Inscripción de Materias";
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitulo,
                lblNroReg, txtNroRegistro, lblNomEst, txtNombreEstudiante,
                lblPlan, txtNumPlan, lblCarreraLabel, txtCarrera,
                lblGestion, cmbGestion, btnCargarMaterias,
                dgvMaterias,
                btnRegistrar, btnLimpiar
            });
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaterias)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label      lblTitulo, lblNroReg, lblNomEst, lblPlan, lblCarreraLabel, lblGestion;
        private System.Windows.Forms.TextBox    txtNroRegistro, txtNombreEstudiante, txtNumPlan, txtCarrera;
        private System.Windows.Forms.ComboBox   cmbGestion;
        private System.Windows.Forms.Button     btnCargarMaterias, btnRegistrar, btnLimpiar;
        private System.Windows.Forms.DataGridView dgvMaterias;
    }
}
