namespace UniversidadXYZ.Negocio
{
    /// <summary>Aula física donde se dictan las clases.</summary>
    public class Aula
    {
        public int    IdAula    { get; set; }
        public string Codigo    { get; set; }
        public short  Capacidad { get; set; }
        public string Ubicacion { get; set; }
    }
}
