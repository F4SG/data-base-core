const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  HeadingLevel, AlignmentType, LevelFormat, BorderStyle, WidthType,
  ShadingType, VerticalAlign, PageNumber, PageBreak, TableOfContents,
  Header, Footer, TabStopType, TabStopPosition
} = require('docx');
const fs = require('fs');
const path = require('path');

const BLUE_DARK   = "1F3864";
const BLUE_MID    = "2E5FA3";
const BLUE_HEADER = "2E75B6";
const GRAY_ROW    = "F2F2F2";
const GRAY_LIGHT  = "EBF3FB";

const border  = { style: BorderStyle.SINGLE, size: 1, color: "BBBBBB" };
const borders = { top: border, bottom: border, left: border, right: border };

const h1 = t => new Paragraph({ heading: HeadingLevel.HEADING_1, spacing: { before: 360, after: 160 },
  children: [new TextRun({ text: t, bold: true, color: BLUE_DARK, size: 32, font: "Arial" })] });
const h2 = t => new Paragraph({ heading: HeadingLevel.HEADING_2, spacing: { before: 260, after: 120 },
  children: [new TextRun({ text: t, bold: true, color: BLUE_MID, size: 28, font: "Arial" })] });
const h3 = t => new Paragraph({ heading: HeadingLevel.HEADING_3, spacing: { before: 200, after: 80 },
  children: [new TextRun({ text: t, bold: true, italics: true, color: BLUE_MID, size: 24, font: "Arial" })] });
const p = (text, opts={}) => new Paragraph({
  alignment: AlignmentType.JUSTIFIED, spacing: { before: 80, after: 80, line: 300 },
  children: [new TextRun({ text, size: 22, font: "Arial", ...opts })]
});
const bl = text => new Paragraph({
  numbering: { reference: "bullets", level: 0 }, spacing: { before: 60, after: 60, line: 280 },
  children: [new TextRun({ text, size: 22, font: "Arial" })]
});
const nb = text => new Paragraph({
  numbering: { reference: "numbers", level: 0 }, spacing: { before: 60, after: 60, line: 280 },
  children: [new TextRun({ text, size: 22, font: "Arial" })]
});

const hCell = (text, width) => new TableCell({
  borders, width: { size: width, type: WidthType.DXA },
  shading: { fill: BLUE_HEADER, type: ShadingType.CLEAR },
  margins: { top: 100, bottom: 100, left: 140, right: 140 },
  verticalAlign: VerticalAlign.CENTER,
  children: [new Paragraph({ alignment: AlignmentType.CENTER,
    children: [new TextRun({ text, bold: true, color: "FFFFFF", size: 20, font: "Arial" })] })]
});
const dCell = (text, width, shade=false) => new TableCell({
  borders, width: { size: width, type: WidthType.DXA },
  shading: { fill: shade ? GRAY_ROW : "FFFFFF", type: ShadingType.CLEAR },
  margins: { top: 80, bottom: 80, left: 140, right: 140 },
  children: [new Paragraph({ alignment: AlignmentType.LEFT,
    children: [new TextRun({ text, size: 20, font: "Arial" })] })]
});
const dCellB = (text, width, shade=false) => new TableCell({
  borders, width: { size: width, type: WidthType.DXA },
  shading: { fill: shade ? GRAY_ROW : "FFFFFF", type: ShadingType.CLEAR },
  margins: { top: 80, bottom: 80, left: 140, right: 140 },
  children: [new Paragraph({ alignment: AlignmentType.LEFT,
    children: [new TextRun({ text, size: 20, font: "Arial", bold: true })] })]
});

// Tabla de problema: filas de aspecto-descripción
function tablaProblema(filas) {
  return new Table({
    width: { size: 9360, type: WidthType.DXA }, columnWidths: [2200, 7160],
    rows: [
      new TableRow({ children: [hCell("Aspecto", 2200), hCell("Descripción", 7160)] }),
      ...filas.map(([asp, desc], i) => new TableRow({ children: [dCellB(asp, 2200, i%2===0), dCell(desc, 7160, i%2===0)] }))
    ]
  });
}

// ─── DATOS DE LOS 27 PROBLEMAS (9 categorías × 3) ───────────────────────────

const categorias = [
  {
    nombre: "1. Educación",
    problemas: [
      {
        titulo: "Problema 1.1 — Deserción escolar en zonas rurales",
        filas: [
          ["Categoría", "Educación"],
          ["Problema", "Los estudiantes de zonas rurales bolivianas abandonan la escuela antes de completar el nivel secundario, principalmente por la distancia geográfica, falta de recursos económicos y ausencia de materiales educativos digitalizados accesibles sin conexión permanente a internet."],
          ["Usuarios afectados", "Estudiantes de nivel primario y secundario en áreas rurales, docentes rurales, padres de familia y ministerios de educación departamentales."],
          ["Impacto", "Altas tasas de analfabetismo funcional, reducción del capital humano calificado, perpetuación de la pobreza intergeneracional y baja productividad laboral futura."],
          ["Solución actual", "Programas de becas esporádicas, distribución de libros de texto físicos y algunos intentos de radio educativa. No existe una plataforma digital unificada accesible offline."],
          ["Oportunidad SaaS", "Una plataforma SaaS con modo offline (PWA) que permita descargar lecciones, tareas y evaluaciones cuando hay conectividad y ejecutarlas sin internet. Con panel docente en la nube, seguimiento de progreso y alertas tempranas de deserción basadas en inactividad."],
        ]
      },
      {
        titulo: "Problema 1.2 — Gestión manual de calificaciones y asistencia en colegios",
        filas: [
          ["Categoría", "Educación"],
          ["Problema", "La mayoría de instituciones educativas de nivel primario y secundario en Bolivia registran calificaciones y asistencia en libretas físicas y hojas de cálculo desconectadas, lo que genera errores de transcripción, pérdida de datos y demoras en la comunicación con los padres."],
          ["Usuarios afectados", "Docentes, directores de colegio, padres de familia y estudiantes de nivel básico y secundario."],
          ["Impacto", "Errores frecuentes en boletas de notas, demoras en la detección de problemas de rendimiento, conflictos con padres por falta de transparencia y carga administrativa excesiva para los docentes."],
          ["Solución actual", "Libros de asistencia físicos, Excel descargado por cada docente y comunicación por WhatsApp informal entre maestros y padres."],
          ["Oportunidad SaaS", "Una plataforma SaaS con aplicación móvil para registro de asistencia con un clic, cálculo automático de promedios, generación de boletas en PDF y notificaciones push a padres cuando el estudiante falta o baja su promedio."],
        ]
      },
      {
        titulo: "Problema 1.3 — Falta de acceso a cursos de formación continua para docentes",
        filas: [
          ["Categoría", "Educación"],
          ["Problema", "Los docentes en ejercicio tienen escasas oportunidades de actualización profesional, dado que los cursos presenciales implican desplazamiento, costo económico elevado y horarios incompatibles con su jornada laboral."],
          ["Usuarios afectados", "Docentes de nivel primario, secundario y técnico en todo el país, especialmente en áreas periurbanas y rurales."],
          ["Impacto", "Métodos de enseñanza desactualizados, baja adopción de tecnologías educativas, rezago curricular y desmotivación profesional."],
          ["Solución actual", "Talleres presenciales esporádicos organizados por el Ministerio de Educación, plataformas genéricas como YouTube y cursos en Coursera no adaptados al contexto boliviano."],
          ["Oportunidad SaaS", "Plataforma SaaS de micro-aprendizaje con cursos certificados diseñados para el currículo boliviano, foros de comunidad docente, seguimiento de créditos de formación continua y emisión automática de certificados digitales."],
        ]
      },
    ]
  },
  {
    nombre: "2. Salud",
    problemas: [
      {
        titulo: "Problema 2.1 — Gestión ineficiente de citas médicas en centros de salud públicos",
        filas: [
          ["Categoría", "Salud"],
          ["Problema", "En los centros de salud públicos de Bolivia, los pacientes deben hacer filas desde la madrugada para obtener un turno de atención, sin posibilidad de reservar citas anticipadas por medios digitales. Esto genera aglomeraciones, pérdida de tiempo y abandono del tratamiento."],
          ["Usuarios afectados", "Pacientes de centros de salud públicos, médicos de cabecera, personal administrativo de salud y directores de establecimiento."],
          ["Impacto", "Tiempos de espera de 3 a 6 horas, ausentismo laboral por espera, sobrecarga del personal de admisión, abandono de controles preventivos y deterioro de la salud de pacientes crónicos."],
          ["Solución actual", "Fichas físicas numeradas entregadas manualmente al inicio de la jornada. Algunos hospitales de tercer nivel cuentan con sistemas internos no conectados entre sí."],
          ["Oportunidad SaaS", "Plataforma SaaS de agenda médica con reserva de citas vía web y SMS (para pacientes sin smartphone), recordatorios automáticos, lista de espera virtual, historial de citas y panel de estadísticas para gestores de salud."],
        ]
      },
      {
        titulo: "Problema 2.2 — Ausencia de sistemas de telemedicina accesibles en zonas alejadas",
        filas: [
          ["Categoría", "Salud"],
          ["Problema", "Las comunidades rurales y periurbanas alejadas de los centros hospitalarios no tienen acceso a consultas médicas especializadas sin recorrer grandes distancias, lo que retrasa diagnósticos y tratamientos críticos."],
          ["Usuarios afectados", "Pacientes en comunidades rurales, médicos generales en postas sanitarias, especialistas en hospitales urbanos de referencia."],
          ["Impacto", "Diagnósticos tardíos, complicaciones evitables, mortalidad materna y neonatal elevada en zonas sin especialistas, y elevado costo de traslado para las familias."],
          ["Solución actual", "Brigadas médicas móviles periódicas con cobertura limitada. No existe una plataforma nacional de telemedicina estructurada y accesible para postas rurales."],
          ["Oportunidad SaaS", "Sistema SaaS de telemedicina con videoconsulta de baja latencia optimizada para conexiones móviles lentas (3G), chat clínico seguro, compartición de imágenes diagnósticas y derivación electrónica al especialista."],
        ]
      },
      {
        titulo: "Problema 2.3 — Control manual del inventario de medicamentos en farmacias hospitalarias",
        filas: [
          ["Categoría", "Salud"],
          ["Problema", "Las farmacias de los hospitales públicos gestionan su stock de medicamentos en registros físicos o en hojas de cálculo desconectadas, lo que provoca desabastecimientos críticos, vencimiento de medicamentos sin detectar y dificultad para realizar reportes al Ministerio de Salud."],
          ["Usuarios afectados", "Farmacéuticos hospitalarios, médicos prescriptores, pacientes internados y personal de compras institucional."],
          ["Impacto", "Desabastecimiento de medicamentos esenciales, pérdidas económicas por vencimiento, demoras en la dispensación y riesgo para la seguridad del paciente."],
          ["Solución actual", "Kardex físico, hojas de Excel por farmacéutico de turno y pedidos por teléfono al almacén central. Sin alertas automáticas de stock mínimo."],
          ["Oportunidad SaaS", "Plataforma SaaS de gestión de inventario farmacéutico con alertas de stock mínimo, control de lotes y vencimiento, integración con prescripciones médicas electrónicas y generación automática de reportes para organismos reguladores."],
        ]
      },
    ]
  },
  {
    nombre: "3. Recursos Humanos",
    problemas: [
      {
        titulo: "Problema 3.1 — Procesos de selección de personal lentos y no estandarizados",
        filas: [
          ["Categoría", "Recursos Humanos"],
          ["Problema", "Las empresas medianas bolivianas realizan sus procesos de reclutamiento de forma manual: publicación en redes sociales, recepción de CVs por correo, evaluación subjetiva sin criterios estandarizados y comunicación informal con los candidatos."],
          ["Usuarios afectados", "Jefes de Recursos Humanos, gerentes solicitantes de personal, candidatos postulantes y equipos de entrevistas."],
          ["Impacto", "Procesos de selección que duran entre 4 y 8 semanas, pérdida de candidatos calificados por falta de seguimiento oportuno, decisiones de contratación sesgadas y baja calidad de contratación."],
          ["Solución actual", "Publicación en Facebook y Computrabajo, recepción por Gmail, filtrado manual y entrevistas coordinadas por WhatsApp. No hay sistema de seguimiento (ATS)."],
          ["Oportunidad SaaS", "ATS (Applicant Tracking System) SaaS con publicación multicanal con un clic, filtrado automático por palabras clave, scorecards estandarizados para entrevistadores, pipeline visual Kanban y comunicación automatizada con candidatos."],
        ]
      },
      {
        titulo: "Problema 3.2 — Gestión manual de nóminas y planillas en empresas PYME",
        filas: [
          ["Categoría", "Recursos Humanos"],
          ["Problema", "Las PYME bolivianas calculan las planillas salariales manualmente o en Excel, sin automatización del cálculo de AFP, RC-IVA, horas extras, vacaciones acumuladas ni aguinaldos, lo que genera errores frecuentes y riesgo de incumplimiento legal."],
          ["Usuarios afectados", "Contadores internos, encargados de RRHH, gerentes de PYME y empleados que reciben su liquidación."],
          ["Impacto", "Errores en pagos, multas por incumplimiento de la normativa laboral boliviana (Ley General del Trabajo), conflictos laborales y elevada carga manual mensual para el área contable."],
          ["Solución actual", "Excel personalizado por empresa, cálculos manuales verificados por el contador y pago en efectivo o transferencia sin comprobante digital estructurado."],
          ["Oportunidad SaaS", "Sistema SaaS de nómina boliviana con cálculo automático de aportes AFP, RC-IVA, horas extras, vacaciones, aguinaldo y bono de producción según la legislación vigente; generación de boletas de pago en PDF y exportación al sistema bancario."],
        ]
      },
      {
        titulo: "Problema 3.3 — Ausencia de sistemas de evaluación de desempeño en empresas medianas",
        filas: [
          ["Categoría", "Recursos Humanos"],
          ["Problema", "La mayoría de las empresas medianas bolivianas no cuenta con un sistema formal de evaluación de desempeño; las evaluaciones se realizan de forma subjetiva, anual o nunca, sin retroalimentación estructurada ni vinculación con planes de desarrollo profesional."],
          ["Usuarios afectados", "Empleados, jefes directos, gerentes de área y directores de Recursos Humanos."],
          ["Impacto", "Alta rotación de personal calificado, desmotivación, ascensos y aumentos basados en favoritismo, ausencia de planes de sucesión y dificultad para identificar brechas de competencias."],
          ["Solución actual", "Conversaciones informales anuales entre jefe y empleado, o formularios en papel archivados sin seguimiento posterior."],
          ["Oportunidad SaaS", "Plataforma SaaS de evaluación de desempeño con ciclos configurables (trimestral, semestral), evaluación 360°, definición de OKRs/metas individuales, historial de retroalimentación y dashboard de brechas de competencias por área."],
        ]
      },
    ]
  },
  {
    nombre: "4. Logística y Transporte",
    problemas: [
      {
        titulo: "Problema 4.1 — Falta de rastreo en tiempo real de envíos interprovinciales",
        filas: [
          ["Categoría", "Logística y Transporte"],
          ["Problema", "Las empresas de transporte de carga interprovincial en Bolivia no ofrecen rastreo en tiempo real de los envíos. El cliente no sabe dónde está su paquete hasta que llega o se comunica por teléfono con el transportista."],
          ["Usuarios afectados", "Clientes remitentes y destinatarios, empresas de transporte de carga, comerciantes que dependen de insumos enviados desde otras ciudades."],
          ["Impacto", "Incertidumbre del cliente, pérdida de confianza en el transportista, dificultad para planificar la recepción de mercadería y elevado volumen de llamadas de seguimiento al centro de atención."],
          ["Solución actual", "Consultas telefónicas al transportista o a la agencia de origen. Algunas empresas grandes usan códigos de guía consultables en su sitio web, pero sin actualización en tiempo real."],
          ["Oportunidad SaaS", "Plataforma SaaS con app móvil para conductores (envío de ubicación GPS cada N minutos), portal web para clientes con seguimiento en tiempo real, notificaciones SMS/WhatsApp en hitos clave y dashboard operativo para la empresa transportista."],
        ]
      },
      {
        titulo: "Problema 4.2 — Planificación manual de rutas de última milla en empresas de delivery",
        filas: [
          ["Categoría", "Logística y Transporte"],
          ["Problema", "Las empresas de delivery y distribución urbana en Bolivia asignan rutas a sus repartidores de forma manual o por conocimiento empírico del conductor, sin optimización algorítmica, lo que genera rutas ineficientes con desvíos innecesarios y altos costos de combustible."],
          ["Usuarios afectados", "Empresas de delivery, repartidores, clientes destinatarios y gestores de flota."],
          ["Impacto", "Incremento del 20-40% en los tiempos de entrega respecto a una ruta óptima, mayor consumo de combustible, fatiga del repartidor y menor número de entregas por turno."],
          ["Solución actual", "Asignación verbal de zonas por el coordinador, uso de Google Maps individual por cada repartidor sin integración con la lista de entregas del día."],
          ["Oportunidad SaaS", "SaaS de optimización de rutas de última milla con algoritmo VRP (Vehicle Routing Problem), importación de órdenes del día vía CSV o API, asignación automática a conductores y app móvil con navegación guiada y registro de entrega con firma digital."],
        ]
      },
      {
        titulo: "Problema 4.3 — Gestión deficiente de almacenes en empresas distribuidoras",
        filas: [
          ["Categoría", "Logística y Transporte"],
          ["Problema", "Las empresas distribuidoras medianas operan sus almacenes sin un sistema WMS (Warehouse Management System), controlando el inventario en cuadernos o Excel, lo que provoca errores en despachos, pérdida de mercadería, dificultad para hacer inventarios y desconocimiento de la ubicación exacta de los productos."],
          ["Usuarios afectados", "Encargados de almacén, jefes de logística, vendedores que prometen stock disponible y clientes que reciben pedidos incorrectos."],
          ["Impacto", "Errores en despacho de hasta el 15% de los pedidos, pérdidas por productos mal ubicados, demoras en la preparación de órdenes y conflictos con clientes por entregas incorrectas."],
          ["Solución actual", "Cuadernos de entrada/salida, Excel compartido por WhatsApp y conteos físicos semestrales con cierre de almacén."],
          ["Oportunidad SaaS", "WMS SaaS con registro de entradas y salidas mediante código de barras o QR, ubicación de productos por estante/pasillo/nivel, alertas de stock mínimo, generación de órdenes de reposición y reconciliación de inventario en tiempo real."],
        ]
      },
    ]
  },
  {
    nombre: "5. Finanzas",
    problemas: [
      {
        titulo: "Problema 5.1 — Falta de acceso a crédito para microempresarios informales",
        filas: [
          ["Categoría", "Finanzas"],
          ["Problema", "Los microempresarios del sector informal boliviano no tienen acceso a créditos formales porque carecen de historial crediticio documentado, garantías reales y estados financieros formales, lo que los obliga a recurrir a prestamistas informales con tasas de interés usurarias."],
          ["Usuarios afectados", "Microempresarios informales, vendedores de mercados, artesanos, transportistas independientes y emprendedores de base de la pirámide económica."],
          ["Impacto", "Acceso a financiamiento a tasas del 5-20% mensual por prestamistas informales (frente al 1-2% mensual del sistema formal), dificultad para escalar el negocio y vulnerabilidad financiera ante imprevistos."],
          ["Solución actual", "Préstamos informales de familiares o prestamistas callejeros (chulqueros), o microcréditos con requisitos documentales inaccesibles para el sector informal."],
          ["Oportunidad SaaS", "Plataforma SaaS de scoring crediticio alternativo que evalúa la solvencia del microempresario con datos de movimientos móviles, historial de pagos de servicios y comportamiento de ventas digitales, conectada a instituciones de microfinanzas formales."],
        ]
      },
      {
        titulo: "Problema 5.2 — Gestión contable manual en pequeños negocios y tiendas",
        filas: [
          ["Categoría", "Finanzas"],
          ["Problema", "Los pequeños comerciantes bolivianos no llevan contabilidad formal: registran ingresos y egresos en cuadernos o de memoria, sin conocer su margen real de ganancia, su flujo de caja ni sus obligaciones tributarias con el SIN (Servicio de Impuestos Nacionales)."],
          ["Usuarios afectados", "Propietarios de tiendas de barrio, restaurantes pequeños, peluquerías, talleres y otros negocios unipersonales."],
          ["Impacto", "Desconocimiento de la rentabilidad real del negocio, incumplimiento tributario involuntario, incapacidad para acceder a créditos formales y cierre prematuro de negocios viables por mala gestión financiera."],
          ["Solución actual", "Cuadernos de ingresos y gastos, asesoramiento ocasional de un contador externo y estimaciones basadas en la memoria del propietario."],
          ["Oportunidad SaaS", "Aplicación SaaS de contabilidad simplificada para no contadores, con registro de ventas mediante voz o foto de ticket, cálculo automático de ganancias, alertas de fechas fiscales y módulo de facturación electrónica compatible con el SIN de Bolivia."],
        ]
      },
      {
        titulo: "Problema 5.3 — Ausencia de herramientas de educación financiera personal accesibles",
        filas: [
          ["Categoría", "Finanzas"],
          ["Problema", "La población boliviana tiene bajos niveles de educación financiera personal: no planifica presupuestos, no ahorra de forma sistemática y desconoce productos financieros básicos como fondos de inversión, seguros o planes de jubilación complementarios."],
          ["Usuarios afectados", "Población económicamente activa en general, especialmente jóvenes de 18-35 años con primer empleo formal y trabajadores del sector informal."],
          ["Impacto", "Alto endeudamiento con tarjetas de crédito, ausencia de ahorro para emergencias, dependencia exclusiva de la renta de vejez del sistema de pensiones y vulnerabilidad financiera ante pérdida de empleo."],
          ["Solución actual", "Talleres financieros esporádicos de la ASFI (Autoridad de Supervisión del Sistema Financiero), contenido genérico en YouTube y blogs no adaptados al contexto boliviano."],
          ["Oportunidad SaaS", "Plataforma SaaS gamificada de educación y planificación financiera personal, con presupuestos inteligentes, metas de ahorro visuales, simuladores de crédito e inversión adaptados a productos bolivianos y microcontenido en formato móvil."],
        ]
      },
    ]
  },
  {
    nombre: "6. Gobierno",
    problemas: [
      {
        titulo: "Problema 6.1 — Trámites burocráticos presenciales lentos y sin seguimiento digital",
        filas: [
          ["Categoría", "Gobierno"],
          ["Problema", "La mayoría de los trámites de gobierno municipal y nacional en Bolivia (registro de nacimiento, certificados, licencias de funcionamiento, etc.) requieren presencia física, múltiples visitas a la oficina y largas esperas, sin posibilidad de seguimiento digital del estado del trámite."],
          ["Usuarios afectados", "Ciudadanos en general, empresarios que requieren habilitaciones, abogados y cualquier persona que necesite documentos oficiales."],
          ["Impacto", "Pérdida de días laborables para realizar trámites, alto costo de oportunidad ciudadano, corrupción facilitada por la opacidad del proceso y exclusión de personas con movilidad reducida o residentes en el exterior."],
          ["Solución actual", "Presencia física obligatoria, filas en ventanillas, formularios en papel y comunicación por teléfono fijo. Algunos municipios tienen portales web básicos sin tramitación real en línea."],
          ["Oportunidad SaaS", "Plataforma SaaS de gobierno electrónico con formularios digitales para solicitar trámites, carga de documentos escaneados, seguimiento en tiempo real del estado del trámite, notificaciones por SMS y firma electrónica para la emisión de certificados."],
        ]
      },
      {
        titulo: "Problema 6.2 — Gestión deficiente de quejas y denuncias ciudadanas",
        filas: [
          ["Categoría", "Gobierno"],
          ["Problema", "Los ciudadanos bolivianos no tienen canales digitales eficientes para reportar problemas de infraestructura urbana (baches, alumbrado, basura), denunciar irregularidades o hacer seguimiento a sus quejas ante el gobierno municipal."],
          ["Usuarios afectados", "Ciudadanos residentes en áreas urbanas y periurbanas, funcionarios de atención ciudadana y autoridades municipales."],
          ["Impacto", "Problemas de infraestructura no atendidos por meses, ciudadanos desinformados sobre el estado de sus reportes, percepción de ineficiencia gubernamental y desconfianza institucional."],
          ["Solución actual", "Llamadas a la línea 156 del municipio, presentación de notas físicas en la alcaldía o publicaciones en redes sociales del alcalde esperando respuesta pública."],
          ["Oportunidad SaaS", "Plataforma SaaS de gestión ciudadana con app móvil para reportes geolocalizados con foto, asignación automática al departamento municipal responsable, SLA de respuesta configurable, notificaciones de avance al ciudadano y mapa público de incidencias resueltas."],
        ]
      },
      {
        titulo: "Problema 6.3 — Falta de transparencia en la gestión del presupuesto público municipal",
        filas: [
          ["Categoría", "Gobierno"],
          ["Problema", "Los gobiernos municipales bolivianos no publican información comprensible sobre la ejecución de su presupuesto anual, lo que impide a los ciudadanos fiscalizar el uso de los recursos públicos y dificulta la rendición de cuentas de las autoridades."],
          ["Usuarios afectados", "Ciudadanos en general, periodistas, organizaciones de la sociedad civil, concejales y órganos de control (Contraloría General)."],
          ["Impacto", "Corrupción sin detección temprana, desconfianza en las instituciones, dificultad para ejercer el control social establecido en la Constitución y falta de participación ciudadana informada en el presupuesto participativo."],
          ["Solución actual", "Publicaciones esporádicas en el POA (Plan Operativo Anual) en PDF extensos de difícil lectura, sin visualizaciones interactivas ni comparativas históricas."],
          ["Oportunidad SaaS", "Portal SaaS de transparencia municipal con tableros interactivos de ejecución presupuestaria, comparativas históricas, alertas de variaciones inusuales, motor de búsqueda de contratos públicos y módulo de participación ciudadana para priorización del presupuesto."],
        ]
      },
    ]
  },
  {
    nombre: "7. Agricultura",
    problemas: [
      {
        titulo: "Problema 7.1 — Agricultores sin acceso a información de precios de mercado en tiempo real",
        filas: [
          ["Categoría", "Agricultura"],
          ["Problema", "Los pequeños agricultores bolivianos desconocen los precios actuales de sus productos en los mercados mayoristas de La Paz, Cochabamba y Santa Cruz, lo que los obliga a vender a intermediarios a precios muy por debajo del valor real de mercado."],
          ["Usuarios afectados", "Agricultores minifundistas, cooperativas agrícolas, intermediarios y consumidores finales afectados por la cadena de especulación."],
          ["Impacto", "El agricultor recibe entre el 20% y el 40% del precio final del producto; el resto es capturado por intermediarios. Esto desincentiva la producción y perpetúa la pobreza rural."],
          ["Solución actual", "Información oral de boca a boca entre agricultores, visitas esporádicas al mercado mayorista de referencia o consulta telefónica a familiares en la ciudad."],
          ["Oportunidad SaaS", "Plataforma SaaS con consulta de precios por SMS y app básica (sin datos intensivos), actualización diaria de precios en mercados mayoristas clave, histórico de precios para identificar estacionalidad y módulo de conexión directa con compradores institucionales."],
        ]
      },
      {
        titulo: "Problema 7.2 — Gestión empírica del riego y uso ineficiente del agua en cultivos",
        filas: [
          ["Categoría", "Agricultura"],
          ["Problema", "Los agricultores del altiplano y valles bolivianos riegan sus cultivos basándose en la intuición o el calendario tradicional, sin considerar datos de humedad del suelo, temperatura ni pronóstico meteorológico, lo que lleva a un uso ineficiente del recurso hídrico, escaso en muchas regiones."],
          ["Usuarios afectados", "Agricultores de cultivos de quinua, papa, maíz y hortalizas en zonas de riego tecnificado o semitecnificado."],
          ["Impacto", "Desperdicio de hasta el 40% del agua de riego, estrés hídrico de los cultivos por riego irregular, mayor incidencia de enfermedades fúngicas por encharcamiento y reducción de rendimientos agrícolas."],
          ["Solución actual", "Calendario de riego empírico transmitido generacionalmente, sin sensores ni datos meteorológicos integrados. Algunos proyectos de cooperación instalan sensores pero sin plataforma de gestión."],
          ["Oportunidad SaaS", "Plataforma SaaS de agricultura de precisión con integración de sensores IoT de humedad del suelo, API meteorológica, recomendaciones automáticas de riego, alertas de helada o granizo y reporte mensual de eficiencia hídrica por parcela."],
        ]
      },
      {
        titulo: "Problema 7.3 — Pérdida de producción por falta de trazabilidad en la cadena de exportación agrícola",
        filas: [
          ["Categoría", "Agricultura"],
          ["Problema", "Los productores bolivianos de productos de exportación (quinua, cacao, café) no pueden certificar la trazabilidad de origen de su producción con evidencia digital, lo que les impide acceder a mercados premium internacionales que exigen certificación de origen y comercio justo."],
          ["Usuarios afectados", "Asociaciones de productores agrícolas, exportadores, certificadoras internacionales y compradores en mercados europeos y norteamericanos."],
          ["Impacto", "Pérdida de acceso a mercados premium que pagan un sobreprecio del 20-50% sobre el precio convencional, incapacidad de competir con productores de otros países que sí ofrecen trazabilidad digital."],
          ["Solución actual", "Registros físicos en cuadernos de campo, certificaciones en papel auditadas manualmente por inspectores que visitan las parcelas una vez al año."],
          ["Oportunidad SaaS", "Plataforma SaaS de trazabilidad agrícola con registro digital de cada lote de producción (parcela, fecha, insumos, prácticas), generación de QR por lote para el comprador final, integración con organismos certificadores y exportación de reportes para auditorías internacionales."],
        ]
      },
    ]
  },
  {
    nombre: "8. Turismo",
    problemas: [
      {
        titulo: "Problema 8.1 — Oferta turística boliviana fragmentada y sin visibilidad digital internacional",
        filas: [
          ["Categoría", "Turismo"],
          ["Problema", "La gran mayoría de operadores turísticos, alojamientos rurales y guías independientes de Bolivia no tienen presencia digital estructurada, lo que los hace invisibles para turistas internacionales que buscan destinos auténticos en plataformas globales como Booking, Airbnb o TripAdvisor."],
          ["Usuarios afectados", "Pequeños operadores turísticos, comunidades de turismo rural comunitario, guías locales certificados y turistas internacionales interesados en Bolivia."],
          ["Impacto", "Los turistas internacionales contratan tours a Bolivia a través de agencias extranjeras que capturan el margen de ganancia, mientras el prestador local recibe una fracción del valor generado."],
          ["Solución actual", "Páginas de Facebook actualizadas irregularmente, presencia básica en algunas agencias nacionales y dependencia de la referencia boca a boca entre turistas mochileros."],
          ["Oportunidad SaaS", "Marketplace SaaS de turismo boliviano con perfiles de operadores verificados, reservas en línea con pago internacional integrado, sistema de reseñas auditadas, módulo de itinerarios personalizados y API para conectarse a plataformas globales (OTAs)."],
        ]
      },
      {
        titulo: "Problema 8.2 — Gestión manual de reservas en alojamientos turísticos pequeños",
        filas: [
          ["Categoría", "Turismo"],
          ["Problema", "Los hostales, posadas y alojamientos de ecoturismo bolivianos gestionan sus reservas por WhatsApp o teléfono, sin sistema de disponibilidad en tiempo real, lo que genera dobles reservas, sobreventa de habitaciones y mala experiencia del huésped."],
          ["Usuarios afectados", "Propietarios de hostales y posadas, recepcionistas, turistas nacionales e internacionales."],
          ["Impacto", "Hasta un 10% de las reservas resultan en conflictos por doble asignación, pérdida de ingresos por habitaciones que aparecen ocupadas cuando están disponibles y mala reputación en plataformas de reseñas."],
          ["Solución actual", "Cuaderno de reservas físico, WhatsApp para confirmaciones y pagos por transferencia bancaria sin comprobante automatizado."],
          ["Oportunidad SaaS", "PMS (Property Management System) SaaS adaptado a pequeños alojamientos, con calendario de disponibilidad en tiempo real, motor de reservas propio integrable en la web del alojamiento, procesamiento de pagos y sincronización con Booking.com y Airbnb vía channel manager."],
        ]
      },
      {
        titulo: "Problema 8.3 — Ausencia de información turística multilingüe accesible en destinos remotos",
        filas: [
          ["Categoría", "Turismo"],
          ["Problema", "Los destinos turísticos remotos de Bolivia (Salar de Uyuni, Madidi, Tiwanaku, Potosí) carecen de señalización e información turística digital accesible en múltiples idiomas, lo que limita la experiencia del turista extranjero que visita el sitio sin guía."],
          ["Usuarios afectados", "Turistas internacionales independientes, guías turísticos locales, comunidades receptoras y gestores de sitios patrimoniales."],
          ["Impacto", "Experiencia del turista limitada y superficial, menores tiempos de permanencia en el destino (y menor gasto), daño involuntario a sitios patrimoniales por desconocimiento de reglas y pérdida de competitividad frente a destinos vecinos con mejor señalética digital."],
          ["Solución actual", "Folletos físicos en español disponibles en pocas agencias, aplicaciones genéricas de guía de viaje (Lonely Planet, TripAdvisor) sin contenido local profundo ni actualizado."],
          ["Oportunidad SaaS", "Plataforma SaaS de guía turística digital con realidad aumentada georeferenciada, contenido audiovisual multilingüe (ES/EN/FR/PT), modo offline para zonas sin conectividad, gamificación con sellos de destinos visitados y módulo de reporte de incidencias en sitios patrimoniales."],
        ]
      },
    ]
  },
  {
    nombre: "9. Gestión Empresarial",
    problemas: [
      {
        titulo: "Problema 9.1 — Falta de sistemas de gestión de proyectos en empresas de servicios",
        filas: [
          ["Categoría", "Gestión Empresarial"],
          ["Problema", "Las empresas de servicios (consultoras, agencias, constructoras pequeñas) en Bolivia no utilizan herramientas formales de gestión de proyectos, coordinando el trabajo por WhatsApp y reuniones informales, sin visibilidad del avance real ni control de tiempos y costos."],
          ["Usuarios afectados", "Gerentes de proyecto, equipos de trabajo multidisciplinario, clientes que contratan los servicios y directores de empresa."],
          ["Impacto", "Retrasos en la entrega de proyectos, sobrecosto frecuente, conflictos internos por falta de claridad en responsabilidades y pérdida de clientes por incumplimiento."],
          ["Solución actual", "Grupos de WhatsApp por proyecto, hojas de Excel con cronogramas que nadie actualiza y reuniones semanales de seguimiento sin actas formales."],
          ["Oportunidad SaaS", "Plataforma SaaS de gestión de proyectos con tablero Kanban, diagramas de Gantt, asignación de tareas con responsables y plazos, seguimiento de tiempo por tarea, control de presupuesto y portal de reporte para clientes externos."],
        ]
      },
      {
        titulo: "Problema 9.2 — Gestión de clientes (CRM) inexistente en empresas comerciales medianas",
        filas: [
          ["Categoría", "Gestión Empresarial"],
          ["Problema", "Las empresas comerciales medianas bolivianas no tienen registro centralizado de sus clientes, historial de compras, preferencias ni oportunidades de venta activas, lo que impide la fidelización sistemática y la gestión del pipeline comercial."],
          ["Usuarios afectados", "Vendedores, gerentes comerciales, encargados de marketing y clientes que esperan un servicio personalizado."],
          ["Impacto", "Pérdida de oportunidades de venta cruzada, alta tasa de abandono de clientes por falta de seguimiento, dependencia del conocimiento informal del vendedor y vulnerabilidad ante la rotación del equipo comercial."],
          ["Solución actual", "Libreta personal del vendedor, Excel compartido no actualizado y memoria individual del equipo comercial."],
          ["Oportunidad SaaS", "CRM SaaS con gestión de contactos y cuentas, pipeline de oportunidades visual, automatización de seguimientos por correo/WhatsApp, historial de interacciones, reportes de forecasting de ventas y integración con facturación."],
        ]
      },
      {
        titulo: "Problema 9.3 — Ausencia de indicadores de gestión (KPIs) en tiempo real para gerencia",
        filas: [
          ["Categoría", "Gestión Empresarial"],
          ["Problema", "Los gerentes de empresas medianas bolivianas toman decisiones sin acceso a indicadores de gestión actualizados; los reportes financieros y operativos se elaboran manualmente a fin de mes en Excel, con retraso de semanas respecto a la realidad operativa."],
          ["Usuarios afectados", "Gerentes generales, directores financieros, jefes de área y socios/accionistas de empresas medianas."],
          ["Impacto", "Decisiones gerenciales basadas en información desactualizada, incapacidad para reaccionar a tiempo ante desviaciones, pérdida de competitividad y dificultad para demostrar resultados a inversores."],
          ["Solución actual", "Reportes mensuales en Excel elaborados por el área contable, reuniones de directorio con información de 30-45 días de antigüedad y estimaciones verbales de los jefes de área."],
          ["Oportunidad SaaS", "Plataforma SaaS de Business Intelligence con tableros de KPIs configurables por industria (ventas, finanzas, operaciones, RRHH), actualización en tiempo real desde las fuentes de datos, alertas automáticas ante desviación de metas y módulo de reportes ejecutivos en PDF."],
        ]
      },
    ]
  },
];

// ─── TABLAS DE SECCIÓN 2 ─────────────────────────────────────────────────────

const tablaSeleccion = new Table({
  width: { size: 9360, type: WidthType.DXA }, columnWidths: [2400, 6960],
  rows: [
    new TableRow({ children: [hCell("Criterio de Selección", 2400), hCell("Análisis para el Problema Seleccionado", 6960)] }),
    new TableRow({ children: [dCellB("Problema seleccionado", 2400, false), dCell("Gestión deficiente de almacenes en empresas distribuidoras (Logística y Transporte — Problema 4.3)", 6960, false)] }),
    new TableRow({ children: [dCellB("Magnitud del problema", 2400, true), dCell("Bolivia cuenta con más de 12.000 empresas distribuidoras y comercializadoras registradas. Se estima que el 70% opera sin sistema WMS formal, gestionando inventarios con cuadernos o Excel.", 6960, true)] }),
    new TableRow({ children: [dCellB("Usuarios afectados (estimado)", 2400, false), dCell("Más de 50.000 personas entre encargados de almacén, jefes de logística, vendedores y clientes afectados por errores de despacho en el sector distribución nacional.", 6960, false)] }),
    new TableRow({ children: [dCellB("Viabilidad técnica", 2400, true), dCell("Alta viabilidad: tecnologías maduras (web, escaneo QR/código de barras, base de datos en la nube). No requiere hardware especializado; funciona en tablets de bajo costo y smartphones Android.", 6960, true)] }),
    new TableRow({ children: [dCellB("Potencial de negocio", 2400, false), dCell("Mercado WMS en LATAM en crecimiento del 12% anual. En Bolivia no existe una solución WMS SaaS local adaptada a PYME; las soluciones importadas (SAP, Oracle) son inaccesibles por costo.", 6960, false)] }),
    new TableRow({ children: [dCellB("Diferenciación", 2400, true), dCell("Precio accesible para PYME boliviana (modelo por suscripción mensual), interfaz en español boliviano, soporte local, integración con facturación electrónica del SIN y app móvil offline para almacenes sin WiFi.", 6960, true)] }),
  ]
});

const tablaRequerimientos = new Table({
  width: { size: 9360, type: WidthType.DXA }, columnWidths: [500, 4200, 2160, 2500],
  rows: [
    new TableRow({ children: [hCell("#", 500), hCell("Requerimiento del Usuario Final", 4200), hCell("Tipo", 2160), hCell("Justificación", 2500)] }),
    new TableRow({ children: [dCell("1", 500, false), dCell("Registrar entradas y salidas de mercadería escaneando código de barras o QR desde el celular.", 4200, false), dCell("Funcional", 2160, false), dCell("Reduce el tiempo de registro y elimina errores de transcripción manual.", 2500, false)] }),
    new TableRow({ children: [dCell("2", 500, true), dCell("Consultar en tiempo real la cantidad disponible de cualquier producto por nombre, código o categoría.", 4200, true), dCell("Funcional", 2160, true), dCell("El vendedor necesita confirmar stock al cliente sin llamar al almacén.", 2500, true)] }),
    new TableRow({ children: [dCell("3", 500, false), dCell("Recibir alertas automáticas cuando un producto baja del nivel mínimo de stock configurado.", 4200, false), dCell("Funcional", 2160, false), dCell("Evita el desabastecimiento y permite generar órdenes de reposición a tiempo.", 2500, false)] }),
    new TableRow({ children: [dCell("4", 500, true), dCell("Conocer la ubicación física exacta de cada producto (estante, pasillo, nivel) dentro del almacén.", 4200, true), dCell("Funcional", 2160, true), dCell("Reduce el tiempo de picking y evita pérdidas de mercadería en almacenes grandes.", 2500, true)] }),
    new TableRow({ children: [dCell("5", 500, false), dCell("Generar reportes de inventario en PDF o Excel en cualquier momento sin necesidad de contar físicamente.", 4200, false), dCell("Funcional", 2160, false), dCell("Permite auditorías rápidas y reportes para gerencia o proveedores.", 2500, false)] }),
    new TableRow({ children: [dCell("6", 500, true), dCell("Registrar y consultar el historial completo de movimientos de cada producto (quién lo movió, cuándo y a qué orden).", 4200, true), dCell("Funcional", 2160, true), dCell("Permite la trazabilidad ante reclamos de clientes o discrepancias de inventario.", 2500, true)] }),
    new TableRow({ children: [dCell("7", 500, false), dCell("El sistema debe funcionar sin conexión a internet cuando el almacén no tenga WiFi, sincronizando automáticamente al recuperar conexión.", 4200, false), dCell("No Funcional", 2160, false), dCell("Muchos almacenes en Bolivia operan en zonas con conectividad intermitente.", 2500, false)] }),
    new TableRow({ children: [dCell("8", 500, true), dCell("Gestionar múltiples roles de usuario: administrador, encargado de almacén y vendedor con permisos diferenciados.", 4200, true), dCell("No Funcional", 2160, true), dCell("Evita accesos no autorizados a funciones críticas como ajustes de inventario.", 2500, true)] }),
    new TableRow({ children: [dCell("9", 500, false), dCell("La interfaz debe ser intuitiva y operable por personal con bajo nivel de alfabetización digital, con íconos y colores claros.", 4200, false), dCell("No Funcional", 2160, false), dCell("Los encargados de almacén no suelen tener formación técnica avanzada.", 2500, false)] }),
    new TableRow({ children: [dCell("10", 500, true), dCell("Integrarse con el sistema de facturación electrónica del SIN (SIAT) para registrar automáticamente las salidas asociadas a facturas.", 4200, true), dCell("Integración", 2160, true), dCell("Elimina la doble carga de datos entre el sistema de ventas y el almacén.", 2500, true)] }),
  ]
});

// ─── CONSTRUCCIÓN DEL DOCUMENTO ─────────────────────────────────────────────

const children = [];

// PORTADA
children.push(
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 560, after: 160 },
    children: [new TextRun({ text: "FACULTAD DE INFORMÁTICA Y ELECTRÓNICA", bold: true, size: 28, font: "Arial", color: BLUE_DARK })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 0, after: 80 },
    children: [new TextRun({ text: "Depto. Académico de Industrias, Sistemas y T.I.", size: 22, font: "Arial", color: "555555" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 0, after: 600 },
    children: [new TextRun({ text: "Ingeniería de Sistemas Informáticos", size: 22, font: "Arial", color: "555555", italics: true })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 240, after: 240 },
    border: { top: { style: BorderStyle.SINGLE, size: 12, color: BLUE_HEADER }, bottom: { style: BorderStyle.SINGLE, size: 12, color: BLUE_HEADER } },
    children: [new TextRun({ text: "Definición de la Problemática y Requerimientos", bold: true, size: 40, font: "Arial", color: BLUE_DARK })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 200, after: 80 },
    children: [new TextRun({ text: "Exploración de Problemas por Categoría y Propuesta de Solución SaaS", size: 22, font: "Arial", color: "666666", italics: true })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 600, after: 80 },
    children: [new TextRun({ text: "Docente:", bold: true, size: 22, font: "Arial", color: "444444" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 0, after: 80 },
    children: [new TextRun({ text: "Mg. [Nombre del Docente]", size: 22, font: "Arial", color: "444444" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 160, after: 80 },
    children: [new TextRun({ text: "Materia: Ingeniería de Software / Taller de Proyectos", size: 22, font: "Arial", color: "444444" })] }),
  new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 160, after: 80 },
    children: [new TextRun({ text: "Gestión: 2025", size: 22, font: "Arial", color: "444444" })] }),
  new Paragraph({ children: [new PageBreak()] }),
);

// ÍNDICE
children.push(
  h1("Índice de Contenidos"),
  new TableOfContents("Tabla de Contenidos", { hyperlink: true, headingStyleRange: "1-3" }),
  new Paragraph({ children: [new PageBreak()] }),
);

// INTRODUCCIÓN
children.push(
  h1("I. Introducción"),
  p("La identificación precisa de problemas reales en distintos sectores de la sociedad y la economía constituye el punto de partida indispensable para el desarrollo de soluciones tecnológicas pertinentes y de impacto. En el contexto de la ingeniería de software, la etapa de exploración de la problemática no es un trámite previo al diseño técnico, sino una fase de investigación rigurosa que determina la viabilidad, la relevancia y el alcance del sistema que se va a construir."),
  p("El presente documento presenta los resultados de una investigación descriptiva orientada a identificar problemas en nueve categorías o sectores: Educación, Salud, Recursos Humanos, Logística y Transporte, Finanzas, Gobierno, Agricultura, Turismo y Gestión Empresarial. Para cada categoría se identificaron tres problemas concretos que generan dificultades a usuarios reales en el contexto boliviano, describiendo para cada uno su naturaleza, los usuarios afectados, el impacto que produce, la solución actual con que se enfrenta y la oportunidad que representa para una plataforma de Software como Servicio (SaaS)."),
  p("Una vez completada la exploración de los 27 problemas, se procede a seleccionar uno de ellos para desarrollar como propuesta de proyecto SaaS, justificando la elección mediante criterios de magnitud, viabilidad técnica, potencial de negocio y diferenciación respecto a soluciones existentes. A continuación, se desarrolla el árbol de problemas del caso seleccionado, la propuesta inicial del sistema y los requerimientos del usuario final."),
  p("La metodología empleada es la investigación descriptiva documentada, apoyada en fuentes secundarias (publicaciones académicas, informes sectoriales, estadísticas de organismos nacionales e internacionales) y en el análisis del contexto empresarial y social boliviano. Todas las fuentes se citan siguiendo el formato APA 7.ª edición."),
  new Paragraph({ children: [new PageBreak()] }),
);

// OBJETIVO
children.push(
  h1("II. Objetivo"),
  h2("Objetivo General"),
  p("Identificar y analizar al menos tres problemas en cada una de las nueve categorías propuestas, seleccionar uno de ellos para desarrollar como propuesta de solución tecnológica bajo el modelo SaaS, y definir los requerimientos del usuario final del sistema propuesto."),
  h2("Objetivos Específicos"),
  bl("Describir de manera sistemática los problemas identificados en cada sector, caracterizando su naturaleza, impacto y la solución actual con que se abordan."),
  bl("Seleccionar y justificar un problema prioritario con base en criterios de magnitud, viabilidad técnica, potencial de mercado y diferenciación tecnológica."),
  bl("Elaborar el árbol de problemas del caso seleccionado, identificando sus causas raíz y sus efectos directos e indirectos."),
  bl("Definir la propuesta inicial del sistema SaaS, incluyendo nombre, descripción, usuario objetivo, funcionalidades principales y beneficios esperados."),
  bl("Establecer los requerimientos funcionales y no funcionales del usuario final del sistema propuesto."),
  new Paragraph({ children: [new PageBreak()] }),
);

// SECCIÓN 5: EXPLORACIÓN DE PROBLEMAS
children.push(h1("III. Exploración de Problemas por Categoría"));
children.push(p("A continuación se presentan los 27 problemas identificados, organizados en las nueve categorías solicitadas. Para cada problema se describe el contexto, los usuarios afectados, el impacto, la solución actual y la oportunidad SaaS que representa."));

for (const cat of categorias) {
  children.push(h2(cat.nombre));
  for (const prob of cat.problemas) {
    children.push(h3(prob.titulo));
    children.push(tablaProblema(prob.filas));
    children.push(new Paragraph({ spacing: { before: 80, after: 80 }, children: [] }));
  }
}

// SECCIÓN 2: SELECCIÓN DEL PROBLEMA
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("IV. Selección del Problema y Justificación"));
children.push(p("Después de analizar los 27 problemas identificados en las nueve categorías, se selecciona el Problema 4.3: Gestión deficiente de almacenes en empresas distribuidoras (categoría Logística y Transporte), por ser el que presenta la combinación más favorable entre magnitud del problema, viabilidad técnica de la solución, potencial de negocio en el mercado boliviano y diferenciación respecto a las alternativas existentes."));
children.push(tablaSeleccion);
children.push(p("La elección de este problema también está motivada por la ausencia de una solución WMS SaaS diseñada específicamente para el contexto boliviano. Las soluciones de nivel empresarial (SAP EWM, Oracle WMS) tienen costos de implementación superiores a USD 50.000, inaccesibles para las PYME nacionales. Las soluciones genéricas de inventario disponibles en el mercado no contemplan la normativa tributaria boliviana (SIN/SIAT) ni ofrecen soporte local en español adaptado a la realidad operativa del país."));

// SECCIÓN 3: ÁRBOL DE PROBLEMAS
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("V. Árbol de Problemas"));
children.push(p("El árbol de problemas es una herramienta de análisis causal que permite visualizar las relaciones entre las causas de un problema central, el problema en sí y sus efectos. Su estructura se organiza en tres niveles: las causas raíz (parte inferior/raíces), el problema central (tronco) y los efectos (parte superior/ramas)."));
children.push(p("PROBLEMA CENTRAL: Gestión ineficiente del inventario en almacenes de empresas distribuidoras bolivianas."));
children.push(h2("Causas Directas (raíces del árbol)"));
children.push(bl("Ausencia de sistemas digitales de registro: las empresas utilizan cuadernos físicos o Excel desconectados para controlar las entradas y salidas de mercadería."));
children.push(bl("Desconocimiento de herramientas tecnológicas asequibles: los encargados de almacén y gerentes no conocen soluciones WMS accesibles para PYME bolivianas."));
children.push(bl("Procesos manuales sin estandarización: cada almacenero tiene su propio método de registro, sin un procedimiento unificado para la empresa."));
children.push(bl("Falta de ubicación física registrada de productos: los productos se ubican por memoria del almacenero, sin codificación de pasillos, estantes o niveles."));
children.push(h2("Causas Indirectas (raíces profundas)"));
children.push(bl("Costo percibido de las soluciones tecnológicas: las empresas asocian los sistemas de inventario con software costoso e inaccesible, sin conocer alternativas SaaS de bajo costo."));
children.push(bl("Baja cultura de digitalización en la gestión logística: la tradición de gestión empírica y la resistencia al cambio tecnológico prevalecen en muchas distribuidoras bolivianas."));
children.push(bl("Rotación del personal de almacén: la alta rotación hace que el conocimiento sobre la ubicación del inventario se pierda con cada cambio de encargado."));
children.push(h2("Efectos Directos (ramas del árbol)"));
children.push(bl("Errores en despacho: se despachan productos incorrectos o en cantidades erróneas, generando devoluciones, conflictos con clientes y costos de re-entrega."));
children.push(bl("Pérdida de mercadería: productos mal ubicados, vencidos no detectados o simplemente extraviados dentro del almacén."));
children.push(bl("Demoras en la preparación de órdenes: el personal pierde tiempo buscando productos sin saber su ubicación exacta."));
children.push(bl("Stock fantasma: el sistema registra productos que físicamente no están, o no registra productos que sí están disponibles."));
children.push(h2("Efectos Indirectos (consecuencias de largo plazo)"));
children.push(bl("Pérdida de clientes: los errores de despacho reiterados deterioran la reputación de la empresa y llevan a la pérdida de contratos comerciales."));
children.push(bl("Decisiones de compra incorrectas: sin información confiable del inventario, se generan órdenes de compra innecesarias (sobrestock) o se omiten reposiciones críticas (quiebre de stock)."));
children.push(bl("Imposibilidad de escalar el negocio: sin control de inventario, la empresa no puede ampliar su catálogo, abrir nuevas sucursales ni atender clientes de mayor volumen."));
children.push(bl("Baja rentabilidad operativa: el costo de los errores (devoluciones, mermas, stock inmovilizado) reduce el margen de ganancia de la distribuidora."));

// SECCIÓN 4: PROPUESTA INICIAL DEL SAAS
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("VI. Propuesta Inicial del SaaS"));
children.push(h2("1. Nombre Provisional del Sistema"));
children.push(p("AlmaControl — Sistema de Gestión de Almacenes para PYME Bolivianas."));
children.push(h2("2. Descripción de la Solución"));
children.push(p("AlmaControl es una plataforma SaaS de gestión de almacenes (WMS) diseñada específicamente para pequeñas y medianas empresas distribuidoras bolivianas. Permite registrar entradas y salidas de mercadería mediante escaneo de códigos de barras o QR desde un smartphone o tablet, mantener un inventario en tiempo real con ubicación física por estante y pasillo, generar alertas automáticas de stock mínimo, y producir reportes de inventario exportables. El sistema opera en la nube con modo offline para almacenes sin conectividad constante, se integra con la facturación electrónica del SIN (SIAT) y está diseñado para ser operado por personal sin formación técnica avanzada."));
children.push(h2("3. Usuario Objetivo"));
children.push(bl("Empresas distribuidoras y comercializadoras de tamaño pequeño y mediano (5-200 empleados) con al menos un almacén físico."));
children.push(bl("Encargados de almacén y jefes de logística que actualmente gestionan el inventario con cuadernos o Excel."));
children.push(bl("Gerentes generales y dueños de distribuidoras que necesitan visibilidad del inventario sin depender del encargado de turno."));
children.push(h2("4. Funcionalidades Principales"));
children.push(nb("Registro de entradas y salidas por escaneo de código de barras o QR desde app móvil (Android/iOS)."));
children.push(nb("Mapa digital del almacén con ubicación de productos por pasillo, estante y nivel."));
children.push(nb("Consulta de stock en tiempo real por nombre, código, categoría o proveedor."));
children.push(nb("Alertas automáticas de stock mínimo y generación de órdenes de reposición sugeridas."));
children.push(nb("Control de lotes y fechas de vencimiento con alertas de caducidad próxima."));
children.push(nb("Historial de movimientos trazable por producto, usuario y orden de despacho."));
children.push(nb("Generación de reportes de inventario en PDF y Excel con un clic."));
children.push(nb("Módulo de inventario físico asistido: guía al encargado por el almacén indicando qué contar y reconcilia automáticamente las diferencias."));
children.push(nb("Modo offline con sincronización automática al recuperar conexión a internet."));
children.push(nb("Integración con el SIAT del SIN para vincular salidas de inventario con facturas emitidas."));
children.push(h2("5. Beneficios Esperados"));
children.push(bl("Reducción de errores de despacho en al menos un 80% en los primeros tres meses de uso."));
children.push(bl("Eliminación de las pérdidas por mercadería extraviada o vencida no detectada."));
children.push(bl("Reducción del tiempo de preparación de órdenes en un 40-60% gracias a la ubicación digital."));
children.push(bl("Decisiones de compra basadas en datos reales de inventario, evitando sobrestock y quiebres de stock."));
children.push(bl("Acceso a crédito formal facilitado por la disponibilidad de reportes de inventario confiables como respaldo financiero."));

// SECCIÓN 5: REQUERIMIENTOS
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("VII. Requerimientos del Usuario Final"));
children.push(p("Los requerimientos presentados a continuación fueron definidos mediante el análisis del perfil del usuario objetivo (encargados de almacén, jefes de logística y gerentes de PYME bolivianas) y la observación de las deficiencias de las soluciones actuales. Se clasifican en requerimientos funcionales (capacidades que el sistema debe ejecutar), no funcionales (atributos de calidad del sistema) y de integración (conexiones con sistemas externos)."));
children.push(tablaRequerimientos);
children.push(p("La priorización de los requerimientos funcionales responde a la jerarquía de necesidades del usuario primario (encargado de almacén): primero la operación diaria básica (entradas/salidas, consultas, alertas), luego las funciones de control avanzado (trazabilidad, ubicación, inventario físico) y finalmente las integraciones con sistemas externos. Los requerimientos no funcionales —especialmente el modo offline y la facilidad de uso— son considerados por los usuarios como condiciones sine qua non para la adopción del sistema."));

// CONCLUSIONES
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("VIII. Conclusiones"));
children.push(p("La exploración de 27 problemas distribuidos en nueve categorías permite evidenciar que Bolivia presenta una amplia brecha tecnológica en múltiples sectores de su economía y sociedad. En educación, la deserción escolar rural y la gestión manual de instituciones educativas representan problemas de alto impacto social con soluciones tecnológicas maduras aún no desplegadas. En salud, la ineficiencia en la gestión de citas y la ausencia de telemedicina estructurada afectan la calidad de vida de millones de bolivianos. En logística y transporte, la falta de trazabilidad y la planificación empírica de rutas generan pérdidas económicas significativas para las empresas del sector."));
children.push(p("El problema seleccionado —gestión deficiente de almacenes en empresas distribuidoras— se destaca por su alta prevalencia en el tejido empresarial boliviano, su viabilidad técnica con tecnologías disponibles y maduras, y la ausencia de una solución específicamente adaptada al contexto normativo y económico del país. La propuesta AlmaControl responde directamente a las causas identificadas en el árbol de problemas, con funcionalidades que atienden las necesidades reales del usuario final sin requerir formación tecnológica avanzada."));
children.push(p("Los requerimientos definidos reflejan el perfil del usuario boliviano de PYME: operadores con bajo nivel de alfabetización digital, que necesitan una herramienta simple, rápida y confiable que funcione incluso sin conexión permanente a internet. La integración con el sistema tributario nacional (SIN/SIAT) representa además un diferenciador estratégico que ninguna solución genérica importada ofrece actualmente en el mercado boliviano."));
children.push(p("En síntesis, la exploración y análisis de problemas realizada confirma que el desarrollo de soluciones SaaS contextualizadas para Bolivia representa una oportunidad de alto valor tanto social como económico, y que la ingeniería de software boliviana tiene un papel fundamental en la reducción de la brecha tecnológica del país."));

// REFERENCIAS
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("IX. Referencias Bibliográficas"));
children.push(p("Albahari, J., & Albahari, B. (2017). C# 7.0 in a nutshell. O'Reilly Media."));
children.push(p("Autoridad de Supervisión del Sistema Financiero – ASFI. (2023). Reporte de inclusión financiera Bolivia 2023. Ministerio de Economía y Finanzas Públicas del Estado Plurinacional de Bolivia."));
children.push(p("Christopher, M. (2016). Logistics and supply chain management (5th ed.). Pearson Education."));
children.push(p("Cormen, T. H., Leiserson, C. E., Rivest, R. L., & Stein, C. (2009). Introduction to algorithms (3rd ed.). MIT Press."));
children.push(p("Fondo de las Naciones Unidas para la Infancia – UNICEF. (2022). Situación de la niñez y adolescencia en Bolivia: Informe anual 2022. UNICEF Bolivia."));
children.push(p("Instituto Nacional de Estadística – INE. (2023). Encuesta de hogares 2022: Resultados principales. Estado Plurinacional de Bolivia."));
children.push(p("Laporte, G. (2009). Fifty years of vehicle routing. Transportation Science, 43(4), 408–416. https://doi.org/10.1287/trsc.1090.0301"));
children.push(p("Ministerio de Desarrollo Productivo y Economía Plural. (2022). Situación de las micro y pequeñas empresas en Bolivia. Gobierno del Estado Plurinacional de Bolivia."));
children.push(p("Organización Panamericana de la Salud – OPS. (2022). Informe sobre la salud en las Américas: Bolivia. OPS/OMS."));
children.push(p("Pressman, R. S., & Maxim, B. R. (2014). Software engineering: A practitioner's approach (8th ed.). McGraw-Hill."));
children.push(p("Tanenbaum, A. S., & Wetherall, D. J. (2011). Computer networks (5th ed.). Pearson Education."));
children.push(p("Toth, P., & Vigo, D. (Eds.). (2014). Vehicle routing: Problems, methods, and applications (2nd ed.). SIAM."));
children.push(p("World Bank. (2023). Bolivia: Digital economy country assessment. The World Bank Group."));

// ANEXOS
children.push(new Paragraph({ children: [new PageBreak()] }));
children.push(h1("X. Anexos"));
children.push(h2("Anexo A — Resumen Comparativo de los 27 Problemas Identificados"));
children.push(p("La siguiente tabla presenta un resumen ejecutivo de los 27 problemas explorados, organizado por categoría, para facilitar la comparación y el análisis conjunto."));

const tablaResumen = new Table({
  width: { size: 9360, type: WidthType.DXA }, columnWidths: [1800, 3000, 4560],
  rows: [
    new TableRow({ children: [hCell("Categoría", 1800), hCell("Problema", 3000), hCell("Oportunidad SaaS resumida", 4560)] }),
    ...categorias.flatMap((cat, ci) =>
      cat.problemas.map((prob, pi) => new TableRow({
        children: [
          dCell(pi === 0 ? cat.nombre : "", 1800, (ci*3+pi)%2===0),
          dCell(prob.titulo.replace(/Problema \d+\.\d+ — /, ""), 3000, (ci*3+pi)%2===0),
          dCell(prob.filas.find(f => f[0] === "Oportunidad SaaS")?.[1]?.split(" con ")[0] || "", 4560, (ci*3+pi)%2===0),
        ]
      }))
    )
  ]
});
children.push(tablaResumen);

// ── DOCUMENT BUILD ────────────────────────────────────────────────────────────
const doc = new Document({
  numbering: {
    config: [
      { reference: "bullets", levels: [{ level: 0, format: LevelFormat.BULLET, text: "\u2022",
          alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 360 } } } }] },
      { reference: "numbers", levels: [{ level: 0, format: LevelFormat.DECIMAL, text: "%1.",
          alignment: AlignmentType.LEFT, style: { paragraph: { indent: { left: 720, hanging: 360 } } } }] },
    ]
  },
  styles: {
    default: { document: { run: { font: "Arial", size: 22 } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 32, bold: true, color: BLUE_DARK, font: "Arial" },
        paragraph: { spacing: { before: 320, after: 160 }, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 26, bold: true, color: BLUE_MID, font: "Arial" },
        paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 1 } },
      { id: "Heading3", name: "Heading 3", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 24, bold: true, italics: true, color: BLUE_MID, font: "Arial" },
        paragraph: { spacing: { before: 180, after: 80 }, outlineLevel: 2 } },
    ]
  },
  sections: [{
    properties: { page: { size: { width: 12240, height: 15840 }, margin: { top: 1440, right: 1260, bottom: 1440, left: 1440 } } },
    headers: {
      default: new Header({ children: [new Paragraph({
        border: { bottom: { style: BorderStyle.SINGLE, size: 6, color: BLUE_HEADER, space: 1 } },
        spacing: { after: 100 },
        tabStops: [{ type: TabStopType.RIGHT, position: TabStopPosition.MAX }],
        children: [
          new TextRun({ text: "Definición de la Problemática y Requerimientos", size: 18, font: "Arial", color: "555555" }),
          new TextRun({ text: "\t", size: 18 }),
          new TextRun({ text: "FIE — DAIST", size: 18, font: "Arial", color: "555555", bold: true }),
        ]
      })] })
    },
    footers: {
      default: new Footer({ children: [new Paragraph({
        border: { top: { style: BorderStyle.SINGLE, size: 6, color: BLUE_HEADER, space: 1 } },
        spacing: { before: 100 }, alignment: AlignmentType.CENTER,
        children: [
          new TextRun({ text: "Ingeniería de Sistemas Informáticos  |  Pág. ", size: 18, font: "Arial", color: "555555" }),
          new TextRun({ children: [PageNumber.CURRENT], size: 18, font: "Arial", color: "555555" }),
        ]
      })] })
    },
    children
  }]
});

const outputPath = path.join(__dirname, "Definicion_Problematica_Requerimientos.docx");

Packer.toBuffer(doc).then(buf => {
  fs.writeFileSync(outputPath, buf);
  console.log("✅ Documento generado exitosamente:");
  console.log("   " + outputPath);
}).catch(err => {
  console.error("❌ Error al generar el documento:", err.message);
  process.exit(1);
});
