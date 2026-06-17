namespace UniversidadXYZ.Negocio
{
    /// <summary>Carrera universitaria perteneciente a una Facultad.</summary>
    public class Carrera
    {
        public string Codigo       { get; set; }
        public string Descripcion  { get; set; }
        public int    CodFacultad  { get; set; }

        // Propiedad de navegación (solo UI)
        public string NombreFacultad { get; set; }
    }
}
