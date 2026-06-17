namespace UniversidadXYZ.Negocio
{
    /// <summary>Personal administrativo vinculado a una Persona.</summary>
    public class Administrativo
    {
        public int    IdAdmin    { get; set; }
        public string Cargo      { get; set; }
        public int    IdPersona  { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombrePersona { get; set; }
        public string CI            { get; set; }
    }
}
