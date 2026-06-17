using System;

namespace UniversidadXYZ.Negocio
{
    /// <summary>Entidad base de todas las personas del sistema.</summary>
    public class Persona
    {
        public int    IdPersona       { get; set; }
        public string CI              { get; set; }
        public string Nombre          { get; set; }
        /// <summary>'M' o 'F'</summary>
        public char   Sexo            { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
