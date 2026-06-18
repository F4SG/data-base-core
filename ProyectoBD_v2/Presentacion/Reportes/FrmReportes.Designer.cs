namespace UniversidadXYZ.Presentacion.Reportes
{
    partial class FrmReportes
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            this.lblTitulo      = new System.Windows.Forms.Label();
            this.lblReporte     = new System.Windows.Forms.Label();
            this.cmbReporte     = new System.Windows.Forms.ComboBox();
            this.btnGenerar     = new System.Windows.Forms.Button();
            // Paneles de parámetros (uno por reporte)
            this.pnlParam1      = new System.Windows.Forms.Panel();   // Materias ofertadas
            this.pnlParam2      = new System.Windows.Forms.Panel();   // Asistencia
            this.pnlParam3      = new System.Windows.Forms.Panel();   // Notas estudiante
            this.pnlParam4      = new System.Windows.Forms.Panel();   // Boletín
            // Campos de parámetros - Reporte 1
            this.lblParamCarrera= new System.Windows.Forms.Label(); this.txtParamCarrera = new System.Windows.Forms.TextBox();
            this.lblParamPlan   = new System.Windows.Forms.Label(); this.txtParamPlan    = new System.Windows.Forms.TextBox();
            this.lblParamGestion= new System.Windows.Forms.Label(); this.txtParamGestion = new System.Windows.Forms.TextBox();
            // Reporte 2
            this.lblParamSigla  = new System.Windows.Forms.Label(); this.txtParamSigla   = new System.Windows.Forms.TextBox();
            // Reporte 3
            this.lblParamNroReg = new System.Windows.Forms.Label(); this.txtParamNroReg  = new System.Windows.Forms.TextBox();
            // Reporte 4
            this.lblParamNroRegBol=new System.Windows.Forms.Label(); this.txtParamNroRegBol=new System.Windows.Forms.TextBox();
            this.lblParamSemBol = new System.Windows.Forms.Label(); this.txtParamSemBol  = new System.Windows.Forms.TextBox();
            // ReportViewer
            this.reportViewer   = new Microsoft.Reporting.WinForms.ReportViewer();

            this.SuspendLayout();

            // Título
            this.lblTitulo.Text     = "MÓDULO DE REPORTES";
            this.lblTitulo.Font     = new System.Drawing.Font("Segoe UI",13F,System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(10,8); this.lblTitulo.Size=new System.Drawing.Size(300,28);

            // Combo reporte
            this.lblReporte.Text    = "Reporte:"; this.lblReporte.Location=new System.Drawing.Point(10,46); this.lblReporte.Size=new System.Drawing.Size(70,20); this.lblReporte.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.cmbReporte.Items.AddRange(new object[]{
                "1. Materias Ofertadas (carrera/plan/gestión)",
                "2. Asistencia por Materia",
                "3. Notas Completas del Estudiante",
                "4. Boletín de Aprobadas por Semestre"});
            this.cmbReporte.DropDownStyle=System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReporte.Location=new System.Drawing.Point(85,43); this.cmbReporte.Size=new System.Drawing.Size(380,22); this.cmbReporte.Name="cmbReporte";
            this.cmbReporte.SelectedIndexChanged+=new System.EventHandler(this.cmbReporte_SelectedIndexChanged);

            // Botón generar
            this.btnGenerar.Text     = "▶  Generar Reporte";
            this.btnGenerar.Location = new System.Drawing.Point(480,41); this.btnGenerar.Size=new System.Drawing.Size(150,28);
            this.btnGenerar.BackColor=System.Drawing.Color.DarkGreen; this.btnGenerar.ForeColor=System.Drawing.Color.White;
            this.btnGenerar.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font     = new System.Drawing.Font("Segoe UI",9F,System.Drawing.FontStyle.Bold);
            this.btnGenerar.Click   +=new System.EventHandler(this.btnGenerar_Click);

            // ── Panel 1: Materias ofertadas ──────────────────────────
            this.pnlParam1.Location=new System.Drawing.Point(10,75); this.pnlParam1.Size=new System.Drawing.Size(760,36); this.pnlParam1.Visible=false;
            this.lblParamCarrera.Text="Carrera:"; this.lblParamCarrera.Location=new System.Drawing.Point(0,8); this.lblParamCarrera.Size=new System.Drawing.Size(70,20); this.lblParamCarrera.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamCarrera.Location=new System.Drawing.Point(75,5); this.txtParamCarrera.Size=new System.Drawing.Size(90,22); this.txtParamCarrera.Name="txtParamCarrera";
            this.lblParamPlan.Text="Plan:"; this.lblParamPlan.Location=new System.Drawing.Point(175,8); this.lblParamPlan.Size=new System.Drawing.Size(45,20); this.lblParamPlan.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamPlan.Location=new System.Drawing.Point(225,5); this.txtParamPlan.Size=new System.Drawing.Size(60,22); this.txtParamPlan.Name="txtParamPlan";
            this.lblParamGestion.Text="Gestión:"; this.lblParamGestion.Location=new System.Drawing.Point(295,8); this.lblParamGestion.Size=new System.Drawing.Size(65,20); this.lblParamGestion.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamGestion.Location=new System.Drawing.Point(365,5); this.txtParamGestion.Size=new System.Drawing.Size(80,22); this.txtParamGestion.Name="txtParamGestion";
            var lblHint1=new System.Windows.Forms.Label();lblHint1.Text="(ej: 1/2025)";lblHint1.Location=new System.Drawing.Point(450,8);lblHint1.Size=new System.Drawing.Size(90,20);lblHint1.ForeColor=System.Drawing.Color.Gray;
            this.pnlParam1.Controls.AddRange(new System.Windows.Forms.Control[]{lblParamCarrera,txtParamCarrera,lblParamPlan,txtParamPlan,lblParamGestion,txtParamGestion,lblHint1});

            // ── Panel 2: Asistencia ───────────────────────────────────
            this.pnlParam2.Location=new System.Drawing.Point(10,75); this.pnlParam2.Size=new System.Drawing.Size(760,36); this.pnlParam2.Visible=false;
            this.lblParamSigla.Text="Sigla Materia:"; this.lblParamSigla.Location=new System.Drawing.Point(0,8); this.lblParamSigla.Size=new System.Drawing.Size(100,20); this.lblParamSigla.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamSigla.Location=new System.Drawing.Point(105,5); this.txtParamSigla.Size=new System.Drawing.Size(120,22); this.txtParamSigla.Name="txtParamSigla";
            this.pnlParam2.Controls.AddRange(new System.Windows.Forms.Control[]{lblParamSigla,txtParamSigla});

            // ── Panel 3: Notas estudiante ─────────────────────────────
            this.pnlParam3.Location=new System.Drawing.Point(10,75); this.pnlParam3.Size=new System.Drawing.Size(760,36); this.pnlParam3.Visible=false;
            this.lblParamNroReg.Text="Nro. Registro:"; this.lblParamNroReg.Location=new System.Drawing.Point(0,8); this.lblParamNroReg.Size=new System.Drawing.Size(100,20); this.lblParamNroReg.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamNroReg.Location=new System.Drawing.Point(105,5); this.txtParamNroReg.Size=new System.Drawing.Size(100,22); this.txtParamNroReg.Name="txtParamNroReg";
            this.pnlParam3.Controls.AddRange(new System.Windows.Forms.Control[]{lblParamNroReg,txtParamNroReg});

            // ── Panel 4: Boletín ──────────────────────────────────────
            this.pnlParam4.Location=new System.Drawing.Point(10,75); this.pnlParam4.Size=new System.Drawing.Size(760,36); this.pnlParam4.Visible=false;
            this.lblParamNroRegBol.Text="Nro. Registro:"; this.lblParamNroRegBol.Location=new System.Drawing.Point(0,8); this.lblParamNroRegBol.Size=new System.Drawing.Size(100,20); this.lblParamNroRegBol.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamNroRegBol.Location=new System.Drawing.Point(105,5); this.txtParamNroRegBol.Size=new System.Drawing.Size(100,22); this.txtParamNroRegBol.Name="txtParamNroRegBol";
            this.lblParamSemBol.Text="Semestre:"; this.lblParamSemBol.Location=new System.Drawing.Point(215,8); this.lblParamSemBol.Size=new System.Drawing.Size(75,20); this.lblParamSemBol.TextAlign=System.Drawing.ContentAlignment.MiddleRight;
            this.txtParamSemBol.Location=new System.Drawing.Point(295,5); this.txtParamSemBol.Size=new System.Drawing.Size(50,22); this.txtParamSemBol.Name="txtParamSemBol";
            this.pnlParam4.Controls.AddRange(new System.Windows.Forms.Control[]{lblParamNroRegBol,txtParamNroRegBol,lblParamSemBol,txtParamSemBol});

            // ReportViewer
            this.reportViewer.Location           = new System.Drawing.Point(10, 118);
            this.reportViewer.Size               = new System.Drawing.Size(960, 540);
            this.reportViewer.Name               = "reportViewer";
            this.reportViewer.ShowExportButton   = true;
            this.reportViewer.ShowPrintButton    = true;

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(985, 670);
            this.Text                = "Reportes — UniversidadXYZ";
            this.Controls.AddRange(new System.Windows.Forms.Control[]{
                lblTitulo, lblReporte, cmbReporte, btnGenerar,
                pnlParam1, pnlParam2, pnlParam3, pnlParam4,
                reportViewer
            });
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitulo, lblReporte;
        private System.Windows.Forms.Label lblParamCarrera, lblParamPlan, lblParamGestion;
        private System.Windows.Forms.Label lblParamSigla, lblParamNroReg;
        private System.Windows.Forms.Label lblParamNroRegBol, lblParamSemBol;
        private System.Windows.Forms.ComboBox cmbReporte;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Panel pnlParam1, pnlParam2, pnlParam3, pnlParam4;
        private System.Windows.Forms.TextBox txtParamCarrera, txtParamPlan, txtParamGestion;
        private System.Windows.Forms.TextBox txtParamSigla;
        private System.Windows.Forms.TextBox txtParamNroReg;
        private System.Windows.Forms.TextBox txtParamNroRegBol, txtParamSemBol;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
    }
}
