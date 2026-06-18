namespace UniversidadXYZ.Negocio
{
    /// <summary>Relación N:M entre Edicion_Materia y Grupo.</summary>
    public class EdiGru
    {
        public int CodEdicion { get; set; }
        public int IdGrupo    { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombreMateria { get; set; }
        public string DescGrupo     { get; set; }
    }
}
