namespace UniversidadXYZ.Negocio
{
    /// <summary>Docente vinculado a una Persona.</summary>
    public class Docente
    {
        public int    CodRegistro  { get; set; }
        public string Especialidad { get; set; }
        public int    IdPersona    { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombrePersona { get; set; }
        public string CI            { get; set; }
    }
}
