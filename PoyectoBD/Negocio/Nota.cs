namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Nota de un estudiante en una edición de materia.
    /// La columna estado_aprobacion es calculada/PERSISTED en la BD:
    ///   (p1*0.35 + p2*0.35 + nf*0.30) >= 51 → Aprobado; si falta componente → Pendiente.
    /// Regla: no puede haber dos registros para la misma (nro_registro, cod_edicion).
    /// </summary>
    public class Nota
    {
        public int      IdNota          { get; set; }
        public int      NroRegistro     { get; set; }
        public int      CodEdicion      { get; set; }
        public decimal? NotaParcial1    { get; set; }
        public decimal? NotaParcial2    { get; set; }
        public decimal? NotaFinal       { get; set; }
        /// <summary>Columna PERSISTED calculada por la BD. Solo lectura desde la app.</summary>
        public string   EstadoAprobacion { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombreEstudiante { get; set; }

        /// <summary>
        /// Calcula la nota ponderada localmente (para validación UI antes de grabar).
        /// Retorna null si algún componente falta.
        /// </summary>
        public decimal? NotaPonderada
        {
            get
            {
                if (NotaParcial1 == null || NotaParcial2 == null || NotaFinal == null)
                    return null;
                return NotaParcial1.Value * 0.35m
                     + NotaParcial2.Value * 0.35m
                     + NotaFinal.Value    * 0.30m;
            }
        }
    }
}
