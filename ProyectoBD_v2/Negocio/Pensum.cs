namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Asocia una materia a un plan de estudio indicando el semestre en que se dicta.
    /// Regla: semestre_en_plan entre 1 y 12.
    /// </summary>
    public class Pensum
    {
        public int    NumPlan        { get; set; }
        public string SiglaMateria   { get; set; }
        /// <summary>Semestre dentro del plan: entre 1 y 12.</summary>
        public byte   SemestreEnPlan { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombreMateria  { get; set; }
        public string DescPlan       { get; set; }
    }
}
