using System;

namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Horario de clase: día, hora inicio/fin y fecha específica de asistencia.
    /// Días válidos: Lunes, Martes, Miercoles, Jueves, Viernes, Sabado.
    /// </summary>
    public class Horario
    {
        public int      IdHorario       { get; set; }
        public string   DiaSemana       { get; set; }
        public TimeSpan HoraInicio      { get; set; }
        public TimeSpan HoraFin         { get; set; }
        /// <summary>Fecha exacta de clase (nuevo campo v3).</summary>
        public DateTime FechaAsistencia { get; set; }
    }
}
