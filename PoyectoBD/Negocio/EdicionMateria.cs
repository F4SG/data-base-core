using System;

namespace UniversidadXYZ.Negocio
{
    /// <summary>
    /// Edición de una materia: apertura concreta de una materia en un período/gestión,
    /// asignada a un docente, aula y horario.
    /// El trigger trg_Nota_ProtegerCierre impide modificar nota_final si fecha_fin ya pasó.
    /// </summary>
    public class EdicionMateria
    {
        public int      CodEdicion   { get; set; }
        public DateTime FechaInicio  { get; set; }
        public DateTime FechaFin     { get; set; }
        public string   SiglaMateria { get; set; }
        public int      CodDocente   { get; set; }
        public int      IdAula       { get; set; }
        public int      IdHorario    { get; set; }
        public int      IdGestion    { get; set; }

        // Propiedades de navegación (solo UI)
        public string NombreMateria  { get; set; }
        public string NombreDocente  { get; set; }
        public string CodigoAula     { get; set; }
        public string DescGestion    { get; set; }

        /// <summary>True si la edición está cerrada (fecha_fin ya pasó).</summary>
        public bool EstaCerrada => FechaFin.Date < DateTime.Today;
    }
}
