namespace UniversidadXYZ.Negocio
{
    /// <summary>Estudiante vinculado a una Persona.</summary>
    public class Estudiante
    {
        public int    NroRegistro   { get; set; }
        public string CuentaUsuario { get; set; }
        public string Pin           { get; set; }
        public int    IdPersona     { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombrePersona { get; set; }
        public string CI            { get; set; }
    }
}
