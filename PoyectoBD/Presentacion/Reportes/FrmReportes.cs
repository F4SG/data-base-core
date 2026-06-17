using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using UniversidadXYZ.Datos;

namespace UniversidadXYZ.Presentacion.Reportes
{
    /// <summary>
    /// Módulo de Reportes RDLC.
    /// Reportes disponibles:
    ///   1. Materias ofertadas por carrera/plan/gestión
    ///   2. Asistencia por materia (sigla)
    ///   3. Notas completas del estudiante
    ///   4. Boletín de aprobadas por semestre
    /// </summary>
    public partial class FrmReportes : Form
    {
        public FrmReportes() { InitializeComponent(); }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cmbReporte.SelectedIndex = 0;
            cmbReporte_SelectedIndexChanged(cmbReporte, EventArgs.Empty);
        }

        private void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mostrar/ocultar parámetros según el reporte seleccionado
            pnlParam1.Visible = false; pnlParam2.Visible = false;
            pnlParam3.Visible = false; pnlParam4.Visible = false;

            switch (cmbReporte.SelectedIndex)
            {
                case 0: pnlParam1.Visible = true; break;   // Materias ofertadas
                case 1: pnlParam2.Visible = true; break;   // Asistencia
                case 2: pnlParam3.Visible = true; break;   // Notas estudiante
                case 3: pnlParam4.Visible = true; break;   // Boletín
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                reportViewer.Reset();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                DataTable dt;
                List<ReportParameter> parametros;

                switch (cmbReporte.SelectedIndex)
                {
                    case 0: // Materias ofertadas
                        dt = CargarMateriasOfertadas();
                        reportViewer.LocalReport.ReportEmbeddedResource =
                            "UniversidadXYZ.Presentacion.Reportes.RptMateriasOfertadas.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsMateriasOfertadas", dt));
                        parametros = new List<ReportParameter>
                        {
                            CrearParametro("ParamCarrera", txtParamCarrera.Text),
                            CrearParametro("ParamPlan", txtParamPlan.Text),
                            CrearParametro("ParamGestion", txtParamGestion.Text)
                        };
                        break;

                    case 1: // Asistencia
                        dt = CargarAsistencia();
                        reportViewer.LocalReport.ReportEmbeddedResource =
                            "UniversidadXYZ.Presentacion.Reportes.RptAsistenciaMateria.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsAsistencia", dt));
                        parametros = new List<ReportParameter>
                        {
                            CrearParametro("ParamSigla", txtParamSigla.Text)
                        };
                        break;

                    case 2: // Notas estudiante
                        if (!int.TryParse(txtParamNroReg.Text.Trim(), out int nroRegistro) || nroRegistro <= 0)
                        {
                            MessageBox.Show("Ingrese un número de registro válido.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        dt = CargarNotasEstudiante(nroRegistro);
                        reportViewer.LocalReport.ReportEmbeddedResource =
                            "UniversidadXYZ.Presentacion.Reportes.RptNotasEstudiante.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsNotasEstudiante", dt));
                        parametros = new List<ReportParameter>
                        {
                            CrearParametro("ParamNroRegistro", txtParamNroReg.Text)
                        };
                        break;

                    case 3: // Boletín
                        if (!int.TryParse(txtParamNroRegBol.Text.Trim(), out int nroRegistroBol) || nroRegistroBol <= 0)
                        {
                            MessageBox.Show("Ingrese un número de registro válido.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        dt = CargarBoletin(nroRegistroBol, txtParamSemBol.Text.Trim());
                        reportViewer.LocalReport.ReportEmbeddedResource =
                            "UniversidadXYZ.Presentacion.Reportes.RptBoletinAprobadas.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsBoletin", dt));
                        parametros = new List<ReportParameter>
                        {
                            CrearParametro("ParamNroRegistro", txtParamNroRegBol.Text),
                            CrearParametro("ParamSemestre", txtParamSemBol.Text)
                        };
                        break;

                    default: return;
                }

                reportViewer.LocalReport.SetParameters(parametros);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show("Error al generar reporte: " + detalle, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ReportParameter CrearParametro(string nombre, string valor)
        {
            return new ReportParameter(nombre, string.IsNullOrWhiteSpace(valor) ? string.Empty : valor.Trim());
        }

        // ── Consultas SQL de cada reporte ─────────────────────────────

        private DataTable CargarMateriasOfertadas()
        {
            string sql = @"
                SELECT c.codigo AS cod_carrera, c.descripcion AS carrera,
                       pe.num_plan, pe.descripcion AS [plan],
                       CAST(g.semestre AS VARCHAR)+'/'+CAST(g.anio AS VARCHAR) AS gestion,
                       m.sigla, m.nombre AS materia,
                       ps.semestre_en_plan AS semestre,
                       per.nombre AS docente,
                       a.codigo AS aula
                FROM Edicion_Materia em
                INNER JOIN Materia m          ON em.sigla_materia = m.sigla
                INNER JOIN Gestion g          ON em.id_gestion    = g.id_gestion
                INNER JOIN Pensum ps          ON ps.sigla_materia = m.sigla
                INNER JOIN Plan_Estudio pe    ON ps.num_plan      = pe.num_plan
                INNER JOIN Carrera c          ON pe.cod_carrera   = c.codigo
                INNER JOIN Docente d          ON em.cod_docente   = d.cod_registro
                INNER JOIN Persona per        ON d.idpersona      = per.idpersona
                INNER JOIN Aula a             ON em.id_aula       = a.id_aula
                WHERE (@carrera='' OR c.codigo=@carrera)
                  AND (@plan='' OR CAST(pe.num_plan AS VARCHAR)=@plan)
                  AND (@gestion='' OR CAST(g.semestre AS VARCHAR)+'/'+CAST(g.anio AS VARCHAR)=@gestion)
                ORDER BY c.descripcion, ps.semestre_en_plan, m.sigla";

            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@carrera", txtParamCarrera.Text.Trim());
                da.SelectCommand.Parameters.AddWithValue("@plan",    txtParamPlan.Text.Trim());
                da.SelectCommand.Parameters.AddWithValue("@gestion", txtParamGestion.Text.Trim());
                var dt = new DataTable(); da.Fill(dt); return dt;
            }
        }

        private DataTable CargarAsistencia()
        {
            string sql = @"
                SELECT m.sigla, m.nombre AS materia,
                       h.dia_semana, h.hora_inicio, h.hora_fin, h.fecha_asistencia,
                       a.codigo AS aula,
                       p.nombre AS docente
                FROM Edicion_Materia em
                INNER JOIN Materia m  ON em.sigla_materia = m.sigla
                INNER JOIN Horario h  ON em.id_horario    = h.id_horario
                INNER JOIN Aula a     ON em.id_aula       = a.id_aula
                INNER JOIN Docente d  ON em.cod_docente   = d.cod_registro
                INNER JOIN Persona p  ON d.idpersona      = p.idpersona
                WHERE (@sigla='' OR m.sigla=@sigla)
                ORDER BY h.fecha_asistencia, h.hora_inicio";

            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@sigla", txtParamSigla.Text.Trim());
                var dt = new DataTable(); da.Fill(dt); return dt;
            }
        }

        private DataTable CargarNotasEstudiante(int nroRegistro)
        {
            string sql = @"
                SELECT pe.nombre AS estudiante, pe.ci,
                       m.sigla, m.nombre AS materia,
                       CAST(g.semestre AS VARCHAR)+'/'+CAST(g.anio AS VARCHAR) AS gestion,
                       n.nota_parcial_1, n.nota_parcial_2, n.nota_final,
                       CAST(n.nota_parcial_1*0.35+n.nota_parcial_2*0.35+n.nota_final*0.30 AS DECIMAL(5,2)) AS ponderada,
                       n.estado_aprobacion
                FROM Nota n
                INNER JOIN Estudiante e      ON n.nro_registro  = e.nro_registro
                INNER JOIN Persona pe        ON e.idpersona     = pe.idpersona
                INNER JOIN Edicion_Materia em ON n.cod_edicion  = em.cod_edicion
                INNER JOIN Materia m          ON em.sigla_materia= m.sigla
                INNER JOIN Gestion g          ON em.id_gestion  = g.id_gestion
                WHERE e.nro_registro = @nr
                ORDER BY g.anio, g.semestre, m.sigla";

            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@nr", nroRegistro);
                var dt = new DataTable(); da.Fill(dt); return dt;
            }
        }

        private DataTable CargarBoletin(int nroRegistro, string semestreTexto)
        {
            string sql = @"
                SELECT pe.nombre AS estudiante, pe.ci,
                       ps.semestre_en_plan AS semestre,
                       m.sigla, m.nombre AS materia,
                       CAST(g.semestre AS VARCHAR)+'/'+CAST(g.anio AS VARCHAR) AS gestion,
                       n.nota_parcial_1, n.nota_parcial_2, n.nota_final,
                       n.estado_aprobacion
                FROM Nota n
                INNER JOIN Estudiante e       ON n.nro_registro  = e.nro_registro
                INNER JOIN Persona pe         ON e.idpersona     = pe.idpersona
                INNER JOIN Edicion_Materia em  ON n.cod_edicion  = em.cod_edicion
                INNER JOIN Materia m           ON em.sigla_materia= m.sigla
                INNER JOIN Gestion g           ON em.id_gestion  = g.id_gestion
                INNER JOIN Inscripcion_Plan ip ON ip.nro_registro = e.nro_registro
                INNER JOIN Pensum ps           ON ps.sigla_materia = m.sigla
                                              AND ps.num_plan = ip.num_plan
                WHERE n.estado_aprobacion = 'Aprobado'
                  AND e.nro_registro = @nr
                  AND (@sem=0 OR ps.semestre_en_plan=@sem)
                ORDER BY ps.semestre_en_plan, m.sigla";

            using (var con = Conexion.ObtenerConexion())
            using (var da = new SqlDataAdapter(sql, con))
            {
                int.TryParse(semestreTexto, out int sem);
                da.SelectCommand.Parameters.AddWithValue("@nr", nroRegistro);
                da.SelectCommand.Parameters.AddWithValue("@sem", sem);
                var dt = new DataTable(); da.Fill(dt); return dt;
            }
        }
    }
}
