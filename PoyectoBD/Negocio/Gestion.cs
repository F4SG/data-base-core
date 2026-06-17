namespace UniversidadXYZ.Negocio
{
    /// <summary>Período académico (semestre/año). Ej: semestre=1, anio=2025 → "1/2025".</summary>
    public class Gestion
    {
        public int    IdGestion { get; set; }
        /// <summary>1 o 2 (primer o segundo semestre).</summary>
        public byte   Semestre  { get; set; }
        public short  Anio      { get; set; }

        /// <summary>Representación legible: "1/2025".</summary>
        public string Descripcion => $"{Semestre}/{Anio}";

        public override string ToString() => Descripcion;
    }
}
