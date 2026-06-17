namespace UniversidadXYZ.Negocio
{
    /// <summary>Grupo de estudiantes para una edición de materia. Ej: Grupo A, B, C.</summary>
    public class Grupo
    {
        public int    IdGrupo     { get; set; }
        public string Descripcion { get; set; }
        public short  CupoMaximo  { get; set; }

        public override string ToString() => Descripcion;
    }
}
