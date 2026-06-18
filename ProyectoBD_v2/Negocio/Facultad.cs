using System;

namespace UniversidadXYZ.Negocio
{
    /// <summary>Facultad universitaria.</summary>
    public class Facultad
    {
        public int      Codigo        { get; set; }
        public string   Nombre        { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Today;
    }
}
