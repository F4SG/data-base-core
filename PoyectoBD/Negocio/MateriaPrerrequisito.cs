namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Relación recursiva: una materia requiere haber aprobado otra materia (prerrequisito).
    /// Regla de negocio: sigla_materia != sigla_prerrequisito (no autoreferencia).
    /// </summary>
    public class MateriaPrerrequisito
    {
        public string SiglaMateria       { get; set; }
        public string SiglaPrerrequisito { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombreMateria       { get; set; }
        public string NombrePrerrequisito { get; set; }
    }
}
