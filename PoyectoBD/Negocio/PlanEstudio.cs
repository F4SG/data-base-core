namespace UniversidadXYZ.Negocio
{
    /// <summary>Plan de estudio perteneciente a una Carrera.</summary>
    public class PlanEstudio
    {
        public int    NumPlan     { get; set; }
        public string Descripcion { get; set; }
        public string CodCarrera  { get; set; }

        // Propiedad de navegación (solo UI)
        public string NombreCarrera { get; set; }
    }
}
