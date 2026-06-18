using System;
using System.Windows.Forms;
using UniversidadXYZ.Presentacion.CRUD;
using UniversidadXYZ.Presentacion.Procesos;
using UniversidadXYZ.Presentacion.Reportes;

namespace UniversidadXYZ.Presentacion
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        // ── Maestros ──────────────────────────────────────────────
        private void mnuPersona_Click(object sender, EventArgs e)        => AbrirForm(new FrmPersona());
        private void mnuFacultad_Click(object sender, EventArgs e)       => AbrirForm(new FrmFacultad());
        private void mnuCarrera_Click(object sender, EventArgs e)        => AbrirForm(new FrmCarrera());
        private void mnuPlanEstudio_Click(object sender, EventArgs e)    => AbrirForm(new FrmPlanEstudio());
        private void mnuMateria_Click(object sender, EventArgs e)        => AbrirForm(new FrmMateria());
        private void mnuPrerequisito_Click(object sender, EventArgs e)   => AbrirForm(new FrmMateriaPrerrequisito());
        private void mnuPensum_Click(object sender, EventArgs e)         => AbrirForm(new FrmPensum());
        private void mnuEstudiante_Click(object sender, EventArgs e)     => AbrirForm(new FrmEstudiante());
        private void mnuDocente_Click(object sender, EventArgs e)        => AbrirForm(new FrmDocente());
        private void mnuAdministrativo_Click(object sender, EventArgs e) => AbrirForm(new FrmAdministrativo());
        private void mnuGestion_Click(object sender, EventArgs e)        => AbrirForm(new FrmGestion());
        private void mnuAula_Click(object sender, EventArgs e)           => AbrirForm(new FrmAula());
        private void mnuHorario_Click(object sender, EventArgs e)        => AbrirForm(new FrmHorario());
        private void mnuGrupo_Click(object sender, EventArgs e)          => AbrirForm(new FrmGrupo());
        private void mnuEdicionMateria_Click(object sender, EventArgs e) => AbrirForm(new FrmEdicionMateria());

        // ── Procesos ─────────────────────────────────────────────
        private void mnuInscripcion_Click(object sender, EventArgs e)  => AbrirForm(new FrmInscripcion());
        private void mnuIngresNotas_Click(object sender, EventArgs e)  => AbrirForm(new FrmIngresNotas());

        // ── Reportes ─────────────────────────────────────────────
        private void mnuReportes_Click(object sender, EventArgs e)     => AbrirForm(new FrmReportes());

        // ── Salir ────────────────────────────────────────────────
        private void mnuSalir_Click(object sender, EventArgs e)        => Application.Exit();

        /// <summary>Abre un formulario hijo MDI; si ya está abierto lo trae al frente.</summary>
        private void AbrirForm(Form hijo)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == hijo.GetType())
                {
                    hijo.Dispose();
                    f.BringToFront();
                    return;
                }
            }
            hijo.MdiParent = this;
            hijo.Show();
        }
    }
}
