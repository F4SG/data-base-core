namespace UniversidadXYZ.Presentacion
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1         = new System.Windows.Forms.MenuStrip();
            this.mnuMaestros        = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPersona         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFacultad        = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCarrera         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlanEstudio     = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMateria         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrerequisito    = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPensum          = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEstudiante      = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDocente         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAdministrativo  = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGestion         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAula            = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHorario         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGrupo           = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdicionMateria  = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuProcesos        = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInscripcion     = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIngresNotas     = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReportesMenu    = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReportes        = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSistema         = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSalir           = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();

            // menuStrip1
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuMaestros, this.mnuProcesos, this.mnuReportesMenu, this.mnuSistema });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;

            // mnuMaestros
            this.mnuMaestros.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuPersona, this.mnuFacultad, this.mnuCarrera, this.mnuPlanEstudio,
                this.mnuMateria, this.mnuPrerequisito, this.mnuPensum,
                new System.Windows.Forms.ToolStripSeparator(),
                this.mnuEstudiante, this.mnuDocente, this.mnuAdministrativo,
                new System.Windows.Forms.ToolStripSeparator(),
                this.mnuGestion, this.mnuAula, this.mnuHorario, this.mnuGrupo, this.mnuEdicionMateria });
            this.mnuMaestros.Name = "mnuMaestros";
            this.mnuMaestros.Text = "Maestros";

            // items Maestros
            this.mnuPersona.Name        = "mnuPersona";        this.mnuPersona.Text        = "Personas";
            this.mnuFacultad.Name       = "mnuFacultad";       this.mnuFacultad.Text       = "Facultades";
            this.mnuCarrera.Name        = "mnuCarrera";        this.mnuCarrera.Text        = "Carreras";
            this.mnuPlanEstudio.Name    = "mnuPlanEstudio";    this.mnuPlanEstudio.Text    = "Planes de Estudio";
            this.mnuMateria.Name        = "mnuMateria";        this.mnuMateria.Text        = "Materias";
            this.mnuPrerequisito.Name   = "mnuPrerequisito";   this.mnuPrerequisito.Text   = "Prerrequisitos";
            this.mnuPensum.Name         = "mnuPensum";         this.mnuPensum.Text         = "Pensum";
            this.mnuEstudiante.Name     = "mnuEstudiante";     this.mnuEstudiante.Text     = "Estudiantes";
            this.mnuDocente.Name        = "mnuDocente";        this.mnuDocente.Text        = "Docentes";
            this.mnuAdministrativo.Name = "mnuAdministrativo"; this.mnuAdministrativo.Text = "Administrativos";
            this.mnuGestion.Name        = "mnuGestion";        this.mnuGestion.Text        = "Gestiones";
            this.mnuAula.Name           = "mnuAula";           this.mnuAula.Text           = "Aulas";
            this.mnuHorario.Name        = "mnuHorario";        this.mnuHorario.Text        = "Horarios";
            this.mnuGrupo.Name          = "mnuGrupo";          this.mnuGrupo.Text          = "Grupos";
            this.mnuEdicionMateria.Name = "mnuEdicionMateria"; this.mnuEdicionMateria.Text = "Ediciones de Materia";

            this.mnuPersona.Click        += new System.EventHandler(this.mnuPersona_Click);
            this.mnuFacultad.Click       += new System.EventHandler(this.mnuFacultad_Click);
            this.mnuCarrera.Click        += new System.EventHandler(this.mnuCarrera_Click);
            this.mnuPlanEstudio.Click    += new System.EventHandler(this.mnuPlanEstudio_Click);
            this.mnuMateria.Click        += new System.EventHandler(this.mnuMateria_Click);
            this.mnuPrerequisito.Click   += new System.EventHandler(this.mnuPrerequisito_Click);
            this.mnuPensum.Click         += new System.EventHandler(this.mnuPensum_Click);
            this.mnuEstudiante.Click     += new System.EventHandler(this.mnuEstudiante_Click);
            this.mnuDocente.Click        += new System.EventHandler(this.mnuDocente_Click);
            this.mnuAdministrativo.Click += new System.EventHandler(this.mnuAdministrativo_Click);
            this.mnuGestion.Click        += new System.EventHandler(this.mnuGestion_Click);
            this.mnuAula.Click           += new System.EventHandler(this.mnuAula_Click);
            this.mnuHorario.Click        += new System.EventHandler(this.mnuHorario_Click);
            this.mnuGrupo.Click          += new System.EventHandler(this.mnuGrupo_Click);
            this.mnuEdicionMateria.Click += new System.EventHandler(this.mnuEdicionMateria_Click);

            // mnuProcesos
            this.mnuProcesos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuInscripcion, this.mnuIngresNotas });
            this.mnuProcesos.Name = "mnuProcesos";
            this.mnuProcesos.Text = "Procesos";
            this.mnuInscripcion.Name  = "mnuInscripcion";  this.mnuInscripcion.Text  = "Inscripción de Materias";
            this.mnuIngresNotas.Name  = "mnuIngresNotas";  this.mnuIngresNotas.Text  = "Ingreso de Notas";
            this.mnuInscripcion.Click += new System.EventHandler(this.mnuInscripcion_Click);
            this.mnuIngresNotas.Click += new System.EventHandler(this.mnuIngresNotas_Click);

            // mnuReportesMenu
            this.mnuReportesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mnuReportes });
            this.mnuReportesMenu.Name = "mnuReportesMenu";
            this.mnuReportesMenu.Text = "Reportes";
            this.mnuReportes.Name  = "mnuReportes";  this.mnuReportes.Text  = "Ver Reportes";
            this.mnuReportes.Click += new System.EventHandler(this.mnuReportes_Click);

            // mnuSistema
            this.mnuSistema.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mnuSalir });
            this.mnuSistema.Name = "mnuSistema";
            this.mnuSistema.Text = "Sistema";
            this.mnuSalir.Name  = "mnuSalir";  this.mnuSalir.Text  = "Salir";
            this.mnuSalir.Click += new System.EventHandler(this.mnuSalir_Click);

            // FrmPrincipal
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmPrincipal";
            this.Text = "UniversidadXYZ — Sistema de Gestión Académica";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuMaestros;
        private System.Windows.Forms.ToolStripMenuItem mnuPersona, mnuFacultad, mnuCarrera, mnuPlanEstudio;
        private System.Windows.Forms.ToolStripMenuItem mnuMateria, mnuPrerequisito, mnuPensum;
        private System.Windows.Forms.ToolStripMenuItem mnuEstudiante, mnuDocente, mnuAdministrativo;
        private System.Windows.Forms.ToolStripMenuItem mnuGestion, mnuAula, mnuHorario, mnuGrupo, mnuEdicionMateria;
        private System.Windows.Forms.ToolStripMenuItem mnuProcesos, mnuInscripcion, mnuIngresNotas;
        private System.Windows.Forms.ToolStripMenuItem mnuReportesMenu, mnuReportes;
        private System.Windows.Forms.ToolStripMenuItem mnuSistema, mnuSalir;
    }
}
