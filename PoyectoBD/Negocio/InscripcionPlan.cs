using System;

namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Inscripción de un estudiante en un plan de estudios.
    /// Regla de negocio: un estudiante no puede tener más de 2 planes simultáneos
    /// (gestionado por trigger trg_InscripcionPlan_LimiteCarreras).
    /// </summary>
    public class InscripcionPlan
    {
        public int      NroRegistro     { get; set; }
        public int      NumPlan         { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Today;

        // Propiedades de navegación (solo UI)
        public string NombreEstudiante { get; set; }
        public string DescPlan         { get; set; }
    }
}
