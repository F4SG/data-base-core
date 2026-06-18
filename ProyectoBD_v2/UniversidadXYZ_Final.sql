-- ============================================================
--  UNIVERSIDAD XYZ -- Base de Datos de Gestion Academica
--  SQL Server Script Completo - v3
-- ============================================================
--
--  MODIFICACIONES v3:
--    A. Tabla Materia_Prerrequisito (recursividad)
--    B. Campo fecha_asistencia en Horario
--    C. SQL Agent Job para Backup completo
--    D. Triggers DDL y DML de auditoria por correo (Database Mail)
--
--  INDICE DEL SCRIPT:
--    PARTE 1  - Creacion de la base de datos
--    PARTE 2  - Creacion de tablas
--    PARTE 3  - Tabla de auditoria interna
--    PARTE 4  - Funciones
--    PARTE 5  - Triggers DML existentes (proteccion y auditoria interna)
--    PARTE 6  - Database Mail + Alertas + Backup Job
--      Bloque 1 - Configuracion de Database Mail con Gmail
--      Bloque 2 - Triggers de alerta por correo
--      Bloque 3 - SQL Server Agent Job (backup diario 23:00)
--    PARTE 7  - Verificacion final y ejecucion del job
--
--  CORREO DESTINO: franquito2712@gmail.com
--
--  INSTRUCCIONES DE USO (Parte 6):
--    1. Ejecutar el BLOQUE 1 primero (en contexto master/msdb).
--    2. Verificar que el correo de prueba llegue antes de continuar.
--    3. Ejecutar el BLOQUE 2 (Triggers de alerta).
--    4. Ejecutar el BLOQUE 3 (SQL Agent Job de backup).
--
--  REQUISITOS PREVIOS (Parte 6):
--    - SQL Server con SQL Server Agent en ejecucion.
--    - Acceso a salida SMTP (puerto 587 para Gmail).
--    - La BD UniversidadXYZ ya debe existir.
-- ============================================================


-- ============================================================
-- PARTE 1: CREACION DE LA BASE DE DATOS
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'UniversidadXYZ')
BEGIN
    ALTER DATABASE UniversidadXYZ SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE UniversidadXYZ;
END
GO

CREATE DATABASE UniversidadXYZ
    ON PRIMARY (
        NAME     = UniversidadXYZ_Data,
        FILENAME = 'C:\SQLData\UniversidadXYZ.mdf',
        SIZE     = 100MB,
        MAXSIZE  = 2GB,
        FILEGROWTH = 50MB
    )
    LOG ON (
        NAME     = UniversidadXYZ_Log,
        FILENAME = 'C:\SQLData\UniversidadXYZ.ldf',
        SIZE     = 50MB,
        MAXSIZE  = 500MB,
        FILEGROWTH = 25MB
    );
GO

USE UniversidadXYZ;
GO

-- (Duplicate CREATE DATABASE removed - already created above)
USE UniversidadXYZ;


-- ============================================================
-- PARTE 2: CREACION DE TABLAS
-- ============================================================

-- ----------------------------------------------------------
-- 2.1 PERSONA
-- ----------------------------------------------------------
CREATE TABLE Persona (
    idpersona        INT          NOT NULL IDENTITY(1,1),
    ci               VARCHAR(20)  NOT NULL,
    nombre           VARCHAR(150) NOT NULL,
    sexo             CHAR(1)      NOT NULL CHECK (sexo IN ('M','F')),
    fecha_nacimiento DATE         NOT NULL,
    CONSTRAINT PK_Persona    PRIMARY KEY (idpersona),
    CONSTRAINT UQ_Persona_CI UNIQUE (ci)
);
GO

-- ----------------------------------------------------------
-- 2.2 FACULTAD
-- ----------------------------------------------------------
CREATE TABLE Facultad (
    codigo         INT          NOT NULL IDENTITY(1,1),
    nombre         VARCHAR(150) NOT NULL,
    fecha_creacion DATE         NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_Facultad PRIMARY KEY (codigo)
);
GO

-- ----------------------------------------------------------
-- 2.3 CARRERA
-- ----------------------------------------------------------
CREATE TABLE Carrera (
    codigo       VARCHAR(10)  NOT NULL,
    descripcion  VARCHAR(200) NOT NULL,
    cod_facultad INT          NOT NULL,
    CONSTRAINT PK_Carrera PRIMARY KEY (codigo),
    CONSTRAINT FK_Carrera_Facultad
        FOREIGN KEY (cod_facultad) REFERENCES Facultad(codigo)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.4 PLAN_ESTUDIO
-- ----------------------------------------------------------
CREATE TABLE Plan_Estudio (
    num_plan    INT          NOT NULL IDENTITY(1,1),
    descripcion VARCHAR(200) NOT NULL,
    cod_carrera VARCHAR(10)  NOT NULL,
    CONSTRAINT PK_PlanEstudio PRIMARY KEY (num_plan),
    CONSTRAINT FK_PlanEstudio_Carrera
        FOREIGN KEY (cod_carrera) REFERENCES Carrera(codigo)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.5 MATERIA
-- ----------------------------------------------------------
CREATE TABLE Materia (
    sigla  VARCHAR(15)  NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    CONSTRAINT PK_Materia PRIMARY KEY (sigla)
);
GO

-- ----------------------------------------------------------
-- 2.6 MATERIA_PRERREQUISITO  [NUEVO - MODIFICACION A]
-- Tabla asociativa recursiva sobre Materia.
-- Una materia puede tener cero, uno o varios prerequisitos.
-- La restriccion CK_NoAutorreferencia impide que una materia
-- sea prerequisito de si misma directamente.
-- ----------------------------------------------------------
CREATE TABLE Materia_Prerrequisito (
    sigla_materia      VARCHAR(15) NOT NULL,   -- la materia que requiere
    sigla_prerrequisito VARCHAR(15) NOT NULL,  -- la materia que se debe haber aprobado antes
    CONSTRAINT PK_MateriaPrerrequisito
        PRIMARY KEY (sigla_materia, sigla_prerrequisito),
    CONSTRAINT FK_MP_Materia
        FOREIGN KEY (sigla_materia)       REFERENCES Materia(sigla)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_MP_Prerrequisito
        FOREIGN KEY (sigla_prerrequisito) REFERENCES Materia(sigla)
        ON UPDATE NO ACTION  ON DELETE NO ACTION,
    CONSTRAINT CK_NoAutorreferencia
        CHECK (sigla_materia <> sigla_prerrequisito)
);
GO

-- ----------------------------------------------------------
-- 2.7 PENSUM  (N:M Plan_Estudio -- Materia)
-- ----------------------------------------------------------
CREATE TABLE Pensum (
    num_plan         INT         NOT NULL,
    sigla_materia    VARCHAR(15) NOT NULL,
    semestre_en_plan TINYINT     NOT NULL CHECK (semestre_en_plan BETWEEN 1 AND 12),
    CONSTRAINT PK_Pensum PRIMARY KEY (num_plan, sigla_materia),
    CONSTRAINT FK_Pensum_Plan
        FOREIGN KEY (num_plan)      REFERENCES Plan_Estudio(num_plan)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_Pensum_Materia
        FOREIGN KEY (sigla_materia) REFERENCES Materia(sigla)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.8 ESTUDIANTE
-- ----------------------------------------------------------
CREATE TABLE Estudiante (
    nro_registro   INT          NOT NULL IDENTITY(1,1),
    cuenta_usuario VARCHAR(50)  NOT NULL,
    pin            VARCHAR(100) NOT NULL,
    idpersona      INT          NOT NULL,
    CONSTRAINT PK_Estudiante        PRIMARY KEY (nro_registro),
    CONSTRAINT UQ_Estudiante_Cuenta UNIQUE (cuenta_usuario),
    CONSTRAINT FK_Estudiante_Persona
        FOREIGN KEY (idpersona) REFERENCES Persona(idpersona)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.9 DOCENTE
-- ----------------------------------------------------------
CREATE TABLE Docente (
    cod_registro INT          NOT NULL IDENTITY(1,1),
    especialidad VARCHAR(150) NOT NULL,
    idpersona    INT          NOT NULL,
    CONSTRAINT PK_Docente PRIMARY KEY (cod_registro),
    CONSTRAINT FK_Docente_Persona
        FOREIGN KEY (idpersona) REFERENCES Persona(idpersona)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.10 ADMINISTRATIVO
-- ----------------------------------------------------------
CREATE TABLE Administrativo (
    id_admin  INT          NOT NULL IDENTITY(1,1),
    cargo     VARCHAR(100) NOT NULL,
    idpersona INT          NOT NULL,
    CONSTRAINT PK_Administrativo PRIMARY KEY (id_admin),
    CONSTRAINT FK_Admin_Persona
        FOREIGN KEY (idpersona) REFERENCES Persona(idpersona)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.11 INSCRIPCION_PLAN  (N:M Estudiante -- Plan_Estudio)
-- ----------------------------------------------------------
CREATE TABLE Inscripcion_Plan (
    nro_registro      INT  NOT NULL,
    num_plan          INT  NOT NULL,
    fecha_inscripcion DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_InscripcionPlan PRIMARY KEY (nro_registro, num_plan),
    CONSTRAINT FK_IP_Estudiante
        FOREIGN KEY (nro_registro) REFERENCES Estudiante(nro_registro)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_IP_PlanEstudio
        FOREIGN KEY (num_plan) REFERENCES Plan_Estudio(num_plan)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.12 GESTION
-- ----------------------------------------------------------
CREATE TABLE Gestion (
    id_gestion INT      NOT NULL IDENTITY(1,1),
    semestre   TINYINT  NOT NULL CHECK (semestre IN (1,2)),
    anio       SMALLINT NOT NULL,
    CONSTRAINT PK_Gestion PRIMARY KEY (id_gestion),
    CONSTRAINT UQ_Gestion UNIQUE (semestre, anio)
);
GO

-- ----------------------------------------------------------
-- 2.13 AULA
-- ----------------------------------------------------------
CREATE TABLE Aula (
    id_aula   INT          NOT NULL IDENTITY(1,1),
    codigo    VARCHAR(20)  NOT NULL,
    capacidad SMALLINT     NOT NULL CHECK (capacidad > 0),
    ubicacion VARCHAR(200) NULL,
    CONSTRAINT PK_Aula PRIMARY KEY (id_aula),
    CONSTRAINT UQ_Aula UNIQUE (codigo)
);
GO

-- ----------------------------------------------------------
-- 2.14 HORARIO  [MODIFICACION B]
-- Se agrega la columna fecha_asistencia (DATE) para registrar
-- de forma explicita cada fecha en la que el estudiante debe
-- asistir a clases presenciales o virtuales.
-- Razon del tipo DATE: representa un dia calendario concreto,
-- sin componente de hora (la hora ya la tiene hora_inicio/hora_fin).
-- Si se necesitan rangos, se puede combinar con fecha_fin_asistencia;
-- se opto por fecha individual para maxima granularidad y flexibilidad
-- en sistemas con calendarios academicos irregulares.
-- ----------------------------------------------------------
CREATE TABLE Horario (
    id_horario        INT         NOT NULL IDENTITY(1,1),
    dia_semana        VARCHAR(20) NOT NULL
        CHECK (dia_semana IN ('Lunes','Martes','Miercoles','Jueves','Viernes','Sabado')),
    hora_inicio       TIME        NOT NULL,
    hora_fin          TIME        NOT NULL,
    fecha_asistencia  DATE        NOT NULL,   -- [NUEVO v3] fecha exacta de clase
    CONSTRAINT PK_Horario       PRIMARY KEY (id_horario),
    CONSTRAINT CK_Horario_Horas CHECK (hora_fin > hora_inicio)
);
GO

-- ----------------------------------------------------------
-- 2.15 GRUPO
-- ----------------------------------------------------------
CREATE TABLE Grupo (
    id_grupo    INT          NOT NULL IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL,
    cupo_maximo SMALLINT     NOT NULL CHECK (cupo_maximo > 0),
    CONSTRAINT PK_Grupo PRIMARY KEY (id_grupo)
);
GO

-- ----------------------------------------------------------
-- 2.16 EDICION_MATERIA
-- ----------------------------------------------------------
CREATE TABLE Edicion_Materia (
    cod_edicion   INT         NOT NULL IDENTITY(1,1),
    fecha_inicio  DATE        NOT NULL,
    fecha_fin     DATE        NOT NULL,
    sigla_materia VARCHAR(15) NOT NULL,
    cod_docente   INT         NOT NULL,
    id_aula       INT         NOT NULL,
    id_horario    INT         NOT NULL,
    id_gestion    INT         NOT NULL,
    CONSTRAINT PK_EdicionMateria PRIMARY KEY (cod_edicion),
    CONSTRAINT CK_EM_Fechas      CHECK (fecha_fin > fecha_inicio),
    CONSTRAINT FK_EM_Materia
        FOREIGN KEY (sigla_materia) REFERENCES Materia(sigla)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_EM_Docente
        FOREIGN KEY (cod_docente)   REFERENCES Docente(cod_registro)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_EM_Aula
        FOREIGN KEY (id_aula)       REFERENCES Aula(id_aula)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_EM_Horario
        FOREIGN KEY (id_horario)    REFERENCES Horario(id_horario)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_EM_Gestion
        FOREIGN KEY (id_gestion)    REFERENCES Gestion(id_gestion)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.17 EDI_GRU
-- ----------------------------------------------------------
CREATE TABLE EDI_GRU (
    cod_edicion INT NOT NULL,
    id_grupo    INT NOT NULL,
    CONSTRAINT PK_EdiGru PRIMARY KEY (cod_edicion, id_grupo),
    CONSTRAINT FK_EdiGru_Edicion
        FOREIGN KEY (cod_edicion) REFERENCES Edicion_Materia(cod_edicion)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_EdiGru_Grupo
        FOREIGN KEY (id_grupo)    REFERENCES Grupo(id_grupo)
        ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------
-- 2.18 PERIODO
-- ----------------------------------------------------------
CREATE TABLE Periodo (
    id_horario  INT NOT NULL,
    cod_edicion INT NOT NULL,
    CONSTRAINT PK_Periodo PRIMARY KEY (id_horario, cod_edicion),
    CONSTRAINT FK_Periodo_Horario
        FOREIGN KEY (id_horario)  REFERENCES Horario(id_horario)
        ON UPDATE CASCADE ON DELETE NO ACTION,
    CONSTRAINT FK_Periodo_Edicion
        FOREIGN KEY (cod_edicion) REFERENCES Edicion_Materia(cod_edicion)
        ON DELETE CASCADE
);
GO

-- ----------------------------------------------------------
-- 2.19 NOTA
-- ----------------------------------------------------------
SET QUOTED_IDENTIFIER ON;
CREATE TABLE Nota (
    id_nota        INT          NOT NULL IDENTITY(1,1),
    nro_registro   INT          NOT NULL,
    cod_edicion    INT          NOT NULL,
    nota_parcial_1 DECIMAL(5,2) NULL CHECK (nota_parcial_1 BETWEEN 0 AND 100),
    nota_parcial_2 DECIMAL(5,2) NULL CHECK (nota_parcial_2 BETWEEN 0 AND 100),
    nota_final     DECIMAL(5,2) NULL CHECK (nota_final     BETWEEN 0 AND 100),
    estado_aprobacion AS (
        CASE
            WHEN nota_final     IS NULL THEN 'Pendiente'
            WHEN nota_parcial_1 IS NULL THEN 'Pendiente'
            WHEN nota_parcial_2 IS NULL THEN 'Pendiente'
            WHEN (
                CAST(nota_parcial_1 AS DECIMAL(10,4)) * 0.35 +
                CAST(nota_parcial_2 AS DECIMAL(10,4)) * 0.35 +
                CAST(nota_final     AS DECIMAL(10,4)) * 0.30
            ) >= 51 THEN 'Aprobado'
            ELSE 'Reprobado'
        END
    ) PERSISTED,
    CONSTRAINT PK_Nota       PRIMARY KEY (id_nota),
    CONSTRAINT UQ_Nota_Inscr UNIQUE (nro_registro, cod_edicion),
    CONSTRAINT FK_Nota_Estud
        FOREIGN KEY (nro_registro) REFERENCES Estudiante(nro_registro)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_Nota_Edicion
        FOREIGN KEY (cod_edicion)  REFERENCES Edicion_Materia(cod_edicion)
        ON UPDATE NO ACTION ON DELETE NO ACTION
);
GO


-- ============================================================
-- PARTE 3: TABLA DE AUDITORIA INTERNA (existente + ampliada)
-- ============================================================

CREATE TABLE Auditoria_Inscripciones (
    id_auditoria    INT          NOT NULL IDENTITY(1,1) PRIMARY KEY,
    accion          VARCHAR(20)  NOT NULL,
    nro_registro    INT,
    cod_edicion     INT,
    fecha_hora      DATETIME     NOT NULL DEFAULT GETDATE(),
    usuario_sistema VARCHAR(128) NOT NULL DEFAULT SYSTEM_USER
);
GO


-- ============================================================
-- PARTE 4: FUNCIONES
-- ============================================================

-- ----------------------------------------------------------
-- 4.1 Nota ponderada
-- ----------------------------------------------------------
CREATE OR ALTER FUNCTION fn_NotaPonderada (
    @nro_registro INT,
    @cod_edicion  INT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @p1 DECIMAL(5,2), @p2 DECIMAL(5,2), @nf DECIMAL(5,2);
    SELECT @p1 = nota_parcial_1,
           @p2 = nota_parcial_2,
           @nf = nota_final
    FROM Nota
    WHERE nro_registro = @nro_registro
      AND cod_edicion  = @cod_edicion;
    IF @p1 IS NULL OR @p2 IS NULL OR @nf IS NULL
        RETURN NULL;
    RETURN (@p1 * 0.35) + (@p2 * 0.35) + (@nf * 0.30);
END;
GO

-- ----------------------------------------------------------
-- 4.2 Contar materias inscritas por gestion
-- ----------------------------------------------------------
CREATE OR ALTER FUNCTION fn_ContarMateriasInscritas (
    @nro_registro INT,
    @id_gestion   INT
)
RETURNS INT
AS
BEGIN
    DECLARE @total INT;
    SELECT @total = COUNT(*)
    FROM Nota n
    INNER JOIN Edicion_Materia em ON n.cod_edicion = em.cod_edicion
    WHERE n.nro_registro = @nro_registro
      AND em.id_gestion  = @id_gestion;
    RETURN ISNULL(@total, 0);
END;
GO


-- ============================================================
-- PARTE 5: TRIGGERS DML EXISTENTES (proteccion y auditoria interna)
-- ============================================================

-- ----------------------------------------------------------
-- 5.1 Proteger nota_final de ediciones cerradas
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_Nota_ProtegerCierre
ON Nota
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deleted d  ON i.id_nota = d.id_nota
        INNER JOIN Edicion_Materia em ON i.cod_edicion = em.cod_edicion
        WHERE em.fecha_fin < CAST(GETDATE() AS DATE)
          AND d.nota_final IS NOT NULL
          AND i.nota_final <> d.nota_final
    )
    BEGIN
        RAISERROR('No se puede modificar la nota final de una edicion ya cerrada.',16,1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- ----------------------------------------------------------
-- 5.2 Limite de 2 planes simultaneos por estudiante
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_InscripcionPlan_LimiteCarreras
ON Inscripcion_Plan
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT nro_registro FROM Inscripcion_Plan
        WHERE nro_registro IN (SELECT nro_registro FROM inserted)
        GROUP BY nro_registro HAVING COUNT(*) > 2
    )
    BEGIN
        RAISERROR('Un estudiante no puede estar inscrito en mas de 2 planes simultaneamente.',16,1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- ----------------------------------------------------------
-- 5.3 Auditoria interna de inscripciones/anulaciones
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_Audit_Nota
ON Nota
AFTER INSERT, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted)
        INSERT INTO Auditoria_Inscripciones (accion, nro_registro, cod_edicion)
        SELECT 'INSCRIPCION', nro_registro, cod_edicion FROM inserted;
    IF EXISTS (SELECT 1 FROM deleted)
        INSERT INTO Auditoria_Inscripciones (accion, nro_registro, cod_edicion)
        SELECT 'ANULACION', nro_registro, cod_edicion FROM deleted;
END;
GO


-- ============================================================
-- PARTE 6: DATABASE MAIL + ALERTAS + BACKUP JOB
-- Script: Configuracion de Database Mail + Alertas + Backup Job
-- (Reemplaza los triggers del script original con correo actualizado)
-- ============================================================


-- ============================================================
-- BLOQUE 1: CONFIGURACION DE DATABASE MAIL CON GMAIL
-- Contexto: msdb
-- ============================================================

USE msdb;
GO

-- ----------------------------------------------------------
-- 1.1 Habilitar Database Mail si no esta activo
-- ----------------------------------------------------------
IF NOT EXISTS (
    SELECT value_in_use
    FROM sys.configurations
    WHERE name = 'Database Mail XPs'
      AND value_in_use = 1
)
BEGIN
    EXEC sp_configure 'show advanced options', 1;
    RECONFIGURE;
    EXEC sp_configure 'Database Mail XPs', 1;
    RECONFIGURE;
    PRINT 'Database Mail XPs habilitado.';
END
ELSE
    PRINT 'Database Mail XPs ya estaba habilitado.';
GO

-- ----------------------------------------------------------
-- 1.2 Crear perfil de correo (si no existe)
-- ----------------------------------------------------------
IF NOT EXISTS (
    SELECT profile_id FROM msdb.dbo.sysmail_profile
    WHERE name = 'PerfilNotificaciones'
)
BEGIN
    EXEC msdb.dbo.sysmail_add_profile_sp
        @profile_name       = 'PerfilNotificaciones',
        @description        = 'Perfil de notificaciones para UniversidadXYZ';
    PRINT 'Perfil PerfilNotificaciones creado.';
END
ELSE
    PRINT 'Perfil PerfilNotificaciones ya existe.';
GO

-- ----------------------------------------------------------
-- 1.3 Crear cuenta SMTP de Gmail (si no existe)
--
--  *** IMPORTANTE: Reemplaza los valores marcados ***
--     @username    = tu cuenta Gmail completa
--     @password    = Contrasena de aplicacion Gmail
--                    (en Google: Cuenta > Seguridad >
--                     Verificacion en 2 pasos >
--                     Contrasenas de aplicacion)
--     @email_address = la misma cuenta Gmail
-- ----------------------------------------------------------
IF NOT EXISTS (
    SELECT account_id FROM msdb.dbo.sysmail_account
    WHERE name = 'CuentaGmail_UniversidadXYZ'
)
BEGIN
    EXEC msdb.dbo.sysmail_add_account_sp
        @account_name            = 'CuentaGmail_UniversidadXYZ',
        @description             = 'Cuenta Gmail para alertas de UniversidadXYZ',
        @email_address           = 'franquito2712@gmail.com',
        @display_name            = 'Alertas BD UniversidadXYZ',
        @username                = 'franquito2712@gmail.com',
        @password                = 'dpimylhxipfzxmzv',
        @mailserver_name         = 'smtp.gmail.com',
        @mailserver_type         = 'SMTP',
        @port                    = 587,
        @enable_ssl              = 1,
        @use_default_credentials = 0;
    PRINT 'Cuenta CuentaGmail_UniversidadXYZ creada.';
END
ELSE
    PRINT 'Cuenta CuentaGmail_UniversidadXYZ ya existe.';
GO

-- ----------------------------------------------------------
-- 1.4 Asociar cuenta al perfil
-- ----------------------------------------------------------
IF NOT EXISTS (
    SELECT pa.profile_id
    FROM msdb.dbo.sysmail_profileaccount pa
    INNER JOIN msdb.dbo.sysmail_profile   pr ON pa.profile_id  = pr.profile_id
    INNER JOIN msdb.dbo.sysmail_account   ac ON pa.account_id  = ac.account_id
    WHERE pr.name = 'PerfilNotificaciones'
      AND ac.name = 'CuentaGmail_UniversidadXYZ'
)
BEGIN
    EXEC msdb.dbo.sysmail_add_profileaccount_sp
        @profile_name   = 'PerfilNotificaciones',
        @account_name   = 'CuentaGmail_UniversidadXYZ',
        @sequence_number = 1;
    PRINT 'Cuenta asociada al perfil.';
END
ELSE
    PRINT 'La cuenta ya estaba asociada al perfil.';
GO

-- ----------------------------------------------------------
-- 1.5 Hacer el perfil publico (accesible por cualquier usuario)
-- ----------------------------------------------------------
IF NOT EXISTS (
    SELECT profile_id
    FROM msdb.dbo.sysmail_principalprofile
    WHERE profile_id = (
        SELECT profile_id FROM msdb.dbo.sysmail_profile
        WHERE name = 'PerfilNotificaciones'
    )
    AND principal_sid = 0x00  -- public
)
BEGIN
    EXEC msdb.dbo.sysmail_add_principalprofile_sp
        @profile_name   = 'PerfilNotificaciones',
        @principal_name = 'public',
        @is_default     = 1;
    PRINT 'Perfil configurado como publico y predeterminado.';
END
ELSE
    PRINT 'Perfil ya era publico.';
GO

-- ----------------------------------------------------------
-- 1.6 CORREO DE PRUEBA - Verificar antes de continuar
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_send_dbmail
    @profile_name = 'PerfilNotificaciones',
    @recipients   = 'franquito2712@gmail.com',
    @subject      = '[TEST] Database Mail UniversidadXYZ',
    @body         = 'Si recibes este correo, Database Mail esta configurado correctamente para UniversidadXYZ.',
    @body_format  = 'TEXT';
GO

-- Verificar estado del correo de prueba (esperar unos segundos)
 SELECT * FROM msdb.dbo.sysmail_allitems ORDER BY send_request_date DESC;
 SELECT * FROM msdb.dbo.sysmail_event_log ORDER BY log_date DESC;


-- ============================================================
-- BLOQUE 2: TRIGGERS DE ALERTA POR CORREO
-- Contexto: UniversidadXYZ
-- ============================================================

USE UniversidadXYZ;
GO

-- ----------------------------------------------------------
-- 2.1 TRIGGER DDL: detecta CREATE/ALTER/DROP en tablas
--     y procedimientos almacenados
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_DDL_Auditoria_Correo
ON DATABASE
FOR CREATE_TABLE,    ALTER_TABLE,    DROP_TABLE,
    CREATE_PROCEDURE, ALTER_PROCEDURE, DROP_PROCEDURE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EventData   XML           = EVENTDATA();
    DECLARE @EventType   NVARCHAR(100) = @EventData.value('(/EVENT_INSTANCE/EventType)[1]',  'NVARCHAR(100)');
    DECLARE @ObjectType  NVARCHAR(100) = @EventData.value('(/EVENT_INSTANCE/ObjectType)[1]', 'NVARCHAR(100)');
    DECLARE @ObjectName  NVARCHAR(256) = @EventData.value('(/EVENT_INSTANCE/ObjectName)[1]', 'NVARCHAR(256)');
    DECLARE @SchemaName  NVARCHAR(100) = @EventData.value('(/EVENT_INSTANCE/SchemaName)[1]', 'NVARCHAR(100)');
    DECLARE @CommandText NVARCHAR(MAX) = @EventData.value('(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]', 'NVARCHAR(MAX)');
    DECLARE @LoginName   NVARCHAR(256) = @EventData.value('(/EVENT_INSTANCE/LoginName)[1]',  'NVARCHAR(256)');
    DECLARE @PostTime    NVARCHAR(50)  = @EventData.value('(/EVENT_INSTANCE/PostTime)[1]',   'NVARCHAR(50)');

    DECLARE @Asunto NVARCHAR(500);
    DECLARE @Cuerpo NVARCHAR(MAX);

    SET @Asunto = N'[ALERTA DDL - UniversidadXYZ] ' + @EventType
                + N' sobre ' + ISNULL(@ObjectType,'') + N': ' + ISNULL(@ObjectName,'');

    SET @Cuerpo =
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'  ALERTA DE SEGURIDAD - EVENTO DDL     '  + CHAR(13)+CHAR(10) +
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'Base de datos  : UniversidadXYZ'           + CHAR(13)+CHAR(10) +
        N'Evento         : ' + ISNULL(@EventType,  'N/A') + CHAR(13)+CHAR(10) +
        N'Tipo de objeto : ' + ISNULL(@ObjectType, 'N/A') + CHAR(13)+CHAR(10) +
        N'Esquema        : ' + ISNULL(@SchemaName, 'N/A') + CHAR(13)+CHAR(10) +
        N'Objeto afectado: ' + ISNULL(@ObjectName, 'N/A') + CHAR(13)+CHAR(10) +
        N'Usuario login  : ' + ISNULL(@LoginName,  SYSTEM_USER) + CHAR(13)+CHAR(10) +
        N'Fecha / Hora   : ' + ISNULL(@PostTime, CONVERT(NVARCHAR,GETDATE(),120)) + CHAR(13)+CHAR(10) +
        N'----------------------------------------'  + CHAR(13)+CHAR(10) +
        N'Comando ejecutado:'                         + CHAR(13)+CHAR(10) +
        ISNULL(LEFT(@CommandText, 2000), 'No disponible') + CHAR(13)+CHAR(10) +
        N'========================================';

    BEGIN TRY
        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'PerfilNotificaciones',
            @recipients   = 'franquito2712@gmail.com',
            @subject      = @Asunto,
            @body         = @Cuerpo,
            @body_format  = 'TEXT';
    END TRY
    BEGIN CATCH
        PRINT 'ADVERTENCIA: No se pudo enviar alerta DDL. Error: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- ----------------------------------------------------------
-- 2.2 TRIGGER DML: tabla Nota (INSERT / UPDATE / DELETE)
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_DML_Nota_Correo
ON Nota
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Accion  NVARCHAR(10);
    DECLARE @IdNota  INT;
    DECLARE @NroReg  INT;
    DECLARE @CodEd   INT;
    DECLARE @Asunto  NVARCHAR(500);
    DECLARE @Cuerpo  NVARCHAR(MAX);
    DECLARE @CantFilas INT;

    -- Determinar operacion
    IF EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted)
        SET @Accion = 'UPDATE';
    ELSE IF EXISTS(SELECT 1 FROM inserted)
        SET @Accion = 'INSERT';
    ELSE
        SET @Accion = 'DELETE';

    -- Tomar primer registro afectado como referencia
    IF @Accion = 'DELETE'
        SELECT TOP 1 @IdNota = id_nota, @NroReg = nro_registro, @CodEd = cod_edicion
        FROM deleted ORDER BY id_nota;
    ELSE
        SELECT TOP 1 @IdNota = id_nota, @NroReg = nro_registro, @CodEd = cod_edicion
        FROM inserted ORDER BY id_nota;

    -- Contar filas afectadas
    IF @Accion = 'UPDATE'
        SELECT @CantFilas = COUNT(*) FROM inserted;
    ELSE IF @Accion = 'DELETE'
        SELECT @CantFilas = COUNT(*) FROM deleted;
    ELSE
        SELECT @CantFilas = COUNT(*) FROM inserted;

    SET @Asunto = N'[ALERTA DML - Nota] Operacion ' + @Accion + N' detectada en UniversidadXYZ';

    SET @Cuerpo =
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'  ALERTA DE SEGURIDAD - EVENTO DML     '  + CHAR(13)+CHAR(10) +
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'Base de datos  : UniversidadXYZ'           + CHAR(13)+CHAR(10) +
        N'Tabla          : Nota'                      + CHAR(13)+CHAR(10) +
        N'Operacion      : ' + @Accion               + CHAR(13)+CHAR(10) +
        N'Filas afectadas: ' + CAST(@CantFilas AS NVARCHAR(10)) + CHAR(13)+CHAR(10) +
        N'Primer id_nota : ' + ISNULL(CAST(@IdNota AS NVARCHAR(20)), 'N/A') + CHAR(13)+CHAR(10) +
        N'nro_registro   : ' + ISNULL(CAST(@NroReg AS NVARCHAR(20)), 'N/A') + CHAR(13)+CHAR(10) +
        N'cod_edicion    : ' + ISNULL(CAST(@CodEd  AS NVARCHAR(20)), 'N/A') + CHAR(13)+CHAR(10) +
        N'Usuario SQL    : ' + SYSTEM_USER             + CHAR(13)+CHAR(10) +
        N'Host           : ' + HOST_NAME()              + CHAR(13)+CHAR(10) +
        N'Fecha / Hora   : ' + CONVERT(NVARCHAR, GETDATE(), 120) + CHAR(13)+CHAR(10) +
        N'========================================';

    BEGIN TRY
        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'PerfilNotificaciones',
            @recipients   = 'franquito2712@gmail.com',
            @subject      = @Asunto,
            @body         = @Cuerpo,
            @body_format  = 'TEXT';
    END TRY
    BEGIN CATCH
        PRINT 'ADVERTENCIA: No se pudo enviar alerta DML (Nota). Error: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- ----------------------------------------------------------
-- 2.3 TRIGGER DML: tabla Inscripcion_Plan (INSERT / UPDATE / DELETE)
-- ----------------------------------------------------------
CREATE OR ALTER TRIGGER trg_DML_InscripcionPlan_Correo
ON Inscripcion_Plan
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Accion    NVARCHAR(10);
    DECLARE @NroReg    INT;
    DECLARE @NumPlan   INT;
    DECLARE @Asunto    NVARCHAR(500);
    DECLARE @Cuerpo    NVARCHAR(MAX);
    DECLARE @CantFilas INT;

    IF EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted)
        SET @Accion = 'UPDATE';
    ELSE IF EXISTS(SELECT 1 FROM inserted)
        SET @Accion = 'INSERT';
    ELSE
        SET @Accion = 'DELETE';

    IF @Accion = 'DELETE'
        SELECT TOP 1 @NroReg = nro_registro, @NumPlan = num_plan FROM deleted;
    ELSE
        SELECT TOP 1 @NroReg = nro_registro, @NumPlan = num_plan FROM inserted;

    IF @Accion = 'DELETE'
        SELECT @CantFilas = COUNT(*) FROM deleted;
    ELSE
        SELECT @CantFilas = COUNT(*) FROM inserted;

    SET @Asunto = N'[ALERTA DML - Inscripcion_Plan] Operacion ' + @Accion + N' detectada en UniversidadXYZ';

    SET @Cuerpo =
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'  ALERTA DE SEGURIDAD - EVENTO DML     '  + CHAR(13)+CHAR(10) +
        N'========================================'  + CHAR(13)+CHAR(10) +
        N'Base de datos  : UniversidadXYZ'           + CHAR(13)+CHAR(10) +
        N'Tabla          : Inscripcion_Plan'          + CHAR(13)+CHAR(10) +
        N'Operacion      : ' + @Accion               + CHAR(13)+CHAR(10) +
        N'Filas afectadas: ' + CAST(@CantFilas AS NVARCHAR(10)) + CHAR(13)+CHAR(10) +
        N'nro_registro   : ' + ISNULL(CAST(@NroReg  AS NVARCHAR(20)), 'N/A') + CHAR(13)+CHAR(10) +
        N'num_plan       : ' + ISNULL(CAST(@NumPlan AS NVARCHAR(20)), 'N/A') + CHAR(13)+CHAR(10) +
        N'Usuario SQL    : ' + SYSTEM_USER             + CHAR(13)+CHAR(10) +
        N'Host           : ' + HOST_NAME()              + CHAR(13)+CHAR(10) +
        N'Fecha / Hora   : ' + CONVERT(NVARCHAR, GETDATE(), 120) + CHAR(13)+CHAR(10) +
        N'========================================';

    BEGIN TRY
        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'PerfilNotificaciones',
            @recipients   = 'franquito2712@gmail.com',
            @subject      = @Asunto,
            @body         = @Cuerpo,
            @body_format  = 'TEXT';
    END TRY
    BEGIN CATCH
        PRINT 'ADVERTENCIA: No se pudo enviar alerta DML (Inscripcion_Plan). Error: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO


-- ============================================================
-- BLOQUE 3: SQL SERVER AGENT JOB - BACKUP DIARIO A LAS 23:00
-- Contexto: msdb
-- ============================================================

USE msdb;
GO

-- ----------------------------------------------------------
-- 3.1 Crear operador AdminBD (recibe notificacion de fallo)
-- ----------------------------------------------------------
IF NOT EXISTS (SELECT name FROM msdb.dbo.sysoperators WHERE name = N'AdminBD')
BEGIN
    EXEC msdb.dbo.sp_add_operator
        @name          = N'AdminBD',
        @enabled       = 1,
        @email_address = N'franquito2712@gmail.com',
        @pager_days    = 0;
    PRINT 'Operador AdminBD creado.';
END
ELSE
BEGIN
    -- Actualizar el correo del operador existente
    EXEC msdb.dbo.sp_update_operator
        @name          = N'AdminBD',
        @email_address = N'franquito2712@gmail.com';
    PRINT 'Operador AdminBD actualizado con el nuevo correo.';
END
GO

-- ----------------------------------------------------------
-- 3.2 Eliminar job anterior si existe (idempotencia)
-- ----------------------------------------------------------
IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs WHERE name = N'Job_Backup_UniversidadXYZ')
BEGIN
    EXEC msdb.dbo.sp_delete_job
        @job_name               = N'Job_Backup_UniversidadXYZ',
        @delete_unused_schedule = 1;
    PRINT 'Job anterior eliminado.';
END
GO

-- ----------------------------------------------------------
-- 3.3 Crear el job de backup
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_add_job
    @job_name                   = N'Job_Backup_UniversidadXYZ',
    @enabled                    = 1,
    @description                = N'Respaldo completo nocturno de la base de datos UniversidadXYZ. Notifica a franquito2712@gmail.com en caso de fallo.',
    @category_name              = N'Database Maintenance',
    @notify_level_eventlog      = 2,    -- Log de eventos solo en fallo
    @notify_level_email         = 2,    -- Correo al operador solo en fallo
    @notify_email_operator_name = N'AdminBD';
GO

-- ----------------------------------------------------------
-- 3.4 Agregar paso: ejecutar el BACKUP y notificar por
--     correo tanto en exito como en fallo
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_add_jobstep
    @job_name          = N'Job_Backup_UniversidadXYZ',
    @step_name         = N'Paso1_BackupCompleto',
    @step_id           = 1,
    @subsystem         = N'TSQL',
    @database_name     = N'master',
    @on_success_action = 3,   -- 3 = ir al siguiente paso (paso de notificacion exito)
    @on_fail_action    = 4,   -- 4 = ir al paso indicado en on_fail_step_id
    @on_fail_step_id   = 3,   -- saltar al paso 3 (notificacion de fallo)
    @command           = N'
-- =============================================
-- Paso 1: Ejecutar Backup Completo
-- =============================================
DECLARE @RutaBackup NVARCHAR(500);
DECLARE @FechaStr   NVARCHAR(20) = CONVERT(NVARCHAR(20), GETDATE(), 112);  -- YYYYMMDD
DECLARE @HoraStr    NVARCHAR(10) = REPLACE(CONVERT(NVARCHAR(8), GETDATE(), 108),'':'',''''); -- HHMMSS

SET @RutaBackup = N''C:\Backups\UniversidadXYZ_'' + @FechaStr + N''_'' + @HoraStr + N''_FULL.bak'';

-- Crear carpeta si no existe (requiere xp_cmdshell habilitado)
-- EXEC xp_cmdshell ''mkdir C:\Backups'', no_output;

BACKUP DATABASE UniversidadXYZ
    TO DISK = @RutaBackup
    WITH
        FORMAT,
        INIT,
        NAME        = N''UniversidadXYZ - Backup Completo'',
        COMPRESSION,
        CHECKSUM,
        STATS       = 10;

PRINT ''Backup completado exitosamente: '' + @RutaBackup;
';
GO

-- ----------------------------------------------------------
-- 3.5 Paso 2: Notificacion de EXITO por correo
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_add_jobstep
    @job_name          = N'Job_Backup_UniversidadXYZ',
    @step_name         = N'Paso2_NotificarExito',
    @step_id           = 2,
    @subsystem         = N'TSQL',
    @database_name     = N'msdb',
    @on_success_action = 1,   -- 1 = terminar con exito
    @on_fail_action    = 2,   -- 2 = terminar con fallo
    @command           = N'
DECLARE @FechaStr NVARCHAR(30) = CONVERT(NVARCHAR(30), GETDATE(), 120);
DECLARE @Asunto   NVARCHAR(500);
DECLARE @Cuerpo   NVARCHAR(MAX);

SET @Asunto = N''[OK] Backup Exitoso - UniversidadXYZ - '' + @FechaStr;

SET @Cuerpo =
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''  BACKUP COMPLETADO EXITOSAMENTE      '' + CHAR(13)+CHAR(10) +
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''Base de datos : UniversidadXYZ''         + CHAR(13)+CHAR(10) +
    N''Tipo          : FULL BACKUP''             + CHAR(13)+CHAR(10) +
    N''Destino       : C:\Backups\''             + CHAR(13)+CHAR(10) +
    N''Servidor      : '' + @@SERVERNAME         + CHAR(13)+CHAR(10) +
    N''Fecha / Hora  : '' + @FechaStr            + CHAR(13)+CHAR(10) +
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''El respaldo se realizo sin errores.''     + CHAR(13)+CHAR(10) +
    N''Verifica el archivo .bak en C:\Backups\'';

EXEC msdb.dbo.sp_send_dbmail
    @profile_name = ''PerfilNotificaciones'',
    @recipients   = ''franquito2712@gmail.com'',
    @subject      = @Asunto,
    @body         = @Cuerpo,
    @body_format  = ''TEXT'';
';
GO

-- ----------------------------------------------------------
-- 3.6 Paso 3: Notificacion de FALLO por correo
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_add_jobstep
    @job_name          = N'Job_Backup_UniversidadXYZ',
    @step_name         = N'Paso3_NotificarFallo',
    @step_id           = 3,
    @subsystem         = N'TSQL',
    @database_name     = N'msdb',
    @on_success_action = 2,   -- terminar con fallo (el backup fallo)
    @on_fail_action    = 2,
    @command           = N'
DECLARE @FechaStr NVARCHAR(30) = CONVERT(NVARCHAR(30), GETDATE(), 120);
DECLARE @Asunto   NVARCHAR(500);
DECLARE @Cuerpo   NVARCHAR(MAX);

SET @Asunto = N''[ERROR] Backup FALLIDO - UniversidadXYZ - '' + @FechaStr;

SET @Cuerpo =
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''  !!! ERROR EN EL BACKUP !!!         '' + CHAR(13)+CHAR(10) +
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''Base de datos : UniversidadXYZ''         + CHAR(13)+CHAR(10) +
    N''Tipo          : FULL BACKUP''             + CHAR(13)+CHAR(10) +
    N''Servidor      : '' + @@SERVERNAME         + CHAR(13)+CHAR(10) +
    N''Fecha / Hora  : '' + @FechaStr            + CHAR(13)+CHAR(10) +
    N''======================================'' + CHAR(13)+CHAR(10) +
    N''ACCION REQUERIDA: El backup nocturno fallo.'' + CHAR(13)+CHAR(10) +
    N''Revisa el historial del job en SQL Server Agent:'' + CHAR(13)+CHAR(10) +
    N''  msdb > SQL Server Agent > Jobs >''     + CHAR(13)+CHAR(10) +
    N''  Job_Backup_UniversidadXYZ > Ver Historial'';

EXEC msdb.dbo.sp_send_dbmail
    @profile_name = ''PerfilNotificaciones'',
    @recipients   = ''franquito2712@gmail.com'',
    @subject      = @Asunto,
    @body         = @Cuerpo,
    @body_format  = ''TEXT'';
';
GO

-- ----------------------------------------------------------
-- 3.7 Asociar job al servidor local
-- ----------------------------------------------------------
EXEC msdb.dbo.sp_add_jobserver
    @job_name    = N'Job_Backup_UniversidadXYZ',
    @server_name = N'(local)';
GO

-- ----------------------------------------------------------
-- 3.8 Crear programacion diaria a las 23:00
-- ----------------------------------------------------------
-- Eliminar schedule anterior si existe
IF EXISTS (SELECT schedule_id FROM msdb.dbo.sysschedules WHERE name = N'Diario_23hs_UniversidadXYZ')
BEGIN
    EXEC msdb.dbo.sp_delete_schedule
        @schedule_name          = N'Diario_23hs_UniversidadXYZ',
        @force_delete           = 1;
END
GO

EXEC msdb.dbo.sp_add_schedule
    @schedule_name        = N'Diario_23hs_UniversidadXYZ',
    @enabled              = 1,
    @freq_type            = 4,        -- 4 = Diario
    @freq_interval        = 1,        -- cada 1 dia
    @freq_subday_type     = 1,        -- 1 = una vez por dia
    @freq_subday_interval = 0,
    @active_start_time    = 230000,   -- 23:00:00
    @active_end_time      = 235959;
GO

EXEC msdb.dbo.sp_attach_schedule
    @job_name      = N'Job_Backup_UniversidadXYZ',
    @schedule_name = N'Diario_23hs_UniversidadXYZ';
GO


-- ============================================================
-- PARTE 7: VERIFICACION FINAL Y EJECUCION DEL JOB
-- ============================================================

-- ----------------------------------------------------------
-- 7.1 Verificar perfil, cuenta SMTP y job de backup (msdb)
-- ----------------------------------------------------------
USE msdb;
GO

PRINT '--- Perfil y cuenta de correo ---';
SELECT
    p.name          AS perfil,
    a.name          AS cuenta,
    a.email_address,
    a.display_name,
    a.description
FROM msdb.dbo.sysmail_profile p
INNER JOIN msdb.dbo.sysmail_profileaccount pa ON p.profile_id  = pa.profile_id
INNER JOIN msdb.dbo.sysmail_account        a  ON pa.account_id = a.account_id
WHERE p.name = 'PerfilNotificaciones';

PRINT '--- Servidor SMTP de la cuenta ---';
SELECT
    a.name          AS cuenta,
    s.servername,
    s.port,
    s.use_ssl,
    s.servertype
FROM msdb.dbo.sysmail_account a
INNER JOIN msdb.dbo.sysmail_server s ON a.account_id = s.account_id
WHERE a.name = 'CuentaGmail_UniversidadXYZ';

PRINT '--- Job de backup ---';
SELECT
    j.name              AS job_nombre,
    j.enabled,
    s.name              AS schedule,
    s.active_start_time AS hora_inicio
FROM msdb.dbo.sysjobs j
INNER JOIN msdb.dbo.sysjobschedules js ON j.job_id      = js.job_id
INNER JOIN msdb.dbo.sysschedules    s  ON js.schedule_id = s.schedule_id
WHERE j.name = 'Job_Backup_UniversidadXYZ';

PRINT '--- Pasos del job ---';
SELECT step_id, step_name, subsystem, on_success_action, on_fail_action
FROM msdb.dbo.sysjobsteps
WHERE job_id = (SELECT job_id FROM msdb.dbo.sysjobs WHERE name = 'Job_Backup_UniversidadXYZ')
ORDER BY step_id;

PRINT '--- Operador AdminBD ---';
SELECT name, email_address, enabled
FROM msdb.dbo.sysoperators
WHERE name = 'AdminBD';
GO

-- ----------------------------------------------------------
-- 7.2 Verificar triggers de alerta activos (UniversidadXYZ)
-- ----------------------------------------------------------
USE UniversidadXYZ;
GO

PRINT '--- Triggers de alerta activos ---';
SELECT
    name        AS trigger_nombre,
    type_desc,
    is_disabled,
    create_date,
    modify_date
FROM sys.triggers
WHERE name IN (
    'trg_DDL_Auditoria_Correo',
    'trg_DML_Nota_Correo',
    'trg_DML_InscripcionPlan_Correo'
);
GO

-- ----------------------------------------------------------
-- 7.3 Ejecutar job de backup manualmente
-- ----------------------------------------------------------
USE msdb;
GO

EXEC msdb.dbo.sp_start_job @job_name = N'Job_Backup_UniversidadXYZ';
GO

-- ----------------------------------------------------------
-- 7.4 Consultar historial de correos enviados
-- ----------------------------------------------------------
USE msdb;
GO

-- Ver todos los correos enviados y su estado
SELECT TOP 20
    sent_date,
    subject,
    recipients,
    sent_status,
    send_request_date
FROM msdb.dbo.sysmail_allitems
ORDER BY send_request_date DESC;

-- Ver errores si hay
SELECT TOP 10
    log_date,
    event_type,
    description
FROM msdb.dbo.sysmail_event_log
ORDER BY log_date DESC;


-- ============================================================
-- PARTE 8: DATOS DE PRUEBA (SEED DATA)
-- Ejecutar en el contexto de UniversidadXYZ.
-- Cubre TODAS las tablas del sistema para que cada formulario
-- de la aplicacion tenga datos reales con los que trabajar.
--
-- Las gestiones, ediciones y horarios usan YEAR(GETDATE()) para
-- que inscripcion, notas y reportes sigan siendo validos cada año.
--
-- GUIA RAPIDA (gestion actual = 1/<año en curso>, id_gestion = 3):
--   Inscripcion  -> Pablo (8) plan 1 | Natalia (9) plan 2 | Ricardo (10) plan 3
--   Notas        -> Docente 1, gestion actual, edicion MAT101 (cod 1)
--   Reportes     -> INF / plan 1 / gestion 1/<año> | asistencia MAT101 | notas registro 1
-- ============================================================

USE UniversidadXYZ;
GO

-- ----------------------------------------------------------
-- 8.1 FACULTADES
-- ----------------------------------------------------------
SET IDENTITY_INSERT Facultad OFF;
INSERT INTO Facultad (nombre, fecha_creacion) VALUES
    ('Facultad de Ingenieria',         '2000-03-15'),
    ('Facultad de Ciencias Economicas','2001-06-01'),
    ('Facultad de Humanidades',        '1998-09-01');
GO

-- ----------------------------------------------------------
-- 8.2 CARRERAS
-- (cod_facultad referencia identidades insertadas arriba)
-- ----------------------------------------------------------
INSERT INTO Carrera (codigo, descripcion, cod_facultad) VALUES
    ('INF',  'Ingenieria en Informatica',          1),
    ('SIS',  'Ingenieria de Sistemas',              1),
    ('ECO',  'Licenciatura en Economia',            2),
    ('ADM',  'Administracion de Empresas',          2),
    ('COM',  'Comunicacion Social',                 3);
GO

-- ----------------------------------------------------------
-- 8.3 PLANES DE ESTUDIO
-- ----------------------------------------------------------
INSERT INTO Plan_Estudio (descripcion, cod_carrera) VALUES
    ('Plan Informatica 2020',     'INF'),
    ('Plan Sistemas 2021',        'SIS'),
    ('Plan Economia 2019',        'ECO');
GO

-- ----------------------------------------------------------
-- 8.4 MATERIAS
-- ----------------------------------------------------------
INSERT INTO Materia (sigla, nombre) VALUES
    ('MAT101', 'Calculo I'),
    ('MAT201', 'Calculo II'),
    ('PRG101', 'Programacion I'),
    ('PRG201', 'Programacion II'),
    ('BDD101', 'Bases de Datos I'),
    ('BDD201', 'Bases de Datos II'),
    ('ALG101', 'Algebra Lineal'),
    ('ECO101', 'Microeconomia'),
    ('ECO201', 'Macroeconomia'),
    ('EST101', 'Estadistica I');
GO

-- ----------------------------------------------------------
-- 8.5 PRERREQUISITOS
-- ----------------------------------------------------------
INSERT INTO Materia_Prerrequisito (sigla_materia, sigla_prerrequisito) VALUES
    ('MAT201', 'MAT101'),
    ('PRG201', 'PRG101'),
    ('BDD101', 'PRG101'),
    ('BDD201', 'BDD101'),
    ('ECO201', 'ECO101');
GO

-- ----------------------------------------------------------
-- 8.6 PENSUM  (materias por semestre en cada plan)
-- ----------------------------------------------------------
-- Plan 1: Informatica
INSERT INTO Pensum (num_plan, sigla_materia, semestre_en_plan) VALUES
    (1, 'MAT101', 1),
    (1, 'PRG101', 1),
    (1, 'ALG101', 1),
    (1, 'MAT201', 2),
    (1, 'PRG201', 2),
    (1, 'BDD101', 2),
    (1, 'BDD201', 3),
    (1, 'EST101', 3);

-- Plan 2: Sistemas
INSERT INTO Pensum (num_plan, sigla_materia, semestre_en_plan) VALUES
    (2, 'MAT101', 1),
    (2, 'PRG101', 1),
    (2, 'BDD101', 2),
    (2, 'BDD201', 3);

-- Plan 3: Economia
INSERT INTO Pensum (num_plan, sigla_materia, semestre_en_plan) VALUES
    (3, 'ECO101', 1),
    (3, 'MAT101', 1),
    (3, 'ECO201', 2),
    (3, 'EST101', 2);
GO

-- ----------------------------------------------------------
-- 8.7 PERSONAS  (base para docentes, estudiantes y admin)
-- ----------------------------------------------------------
INSERT INTO Persona (ci, nombre, sexo, fecha_nacimiento) VALUES
    -- Docentes (idpersona 1-5)
    ('1234567',  'Dr. Carlos Mendoza Rios',      'M', '1975-04-12'),
    ('2345678',  'Dra. Laura Gutierrez Paz',      'F', '1980-08-23'),
    ('3456789',  'Mg. Roberto Vidal Tapia',       'M', '1978-11-05'),
    ('4567890',  'Lic. Maria Torrez Salinas',     'F', '1982-02-18'),
    ('5678901',  'Ing. Jorge Quispe Mamani',      'M', '1970-07-30'),
    -- Estudiantes (idpersona 6-15)
    ('6789012',  'Ana Flores Choque',             'F', '2002-01-15'),
    ('7890123',  'Luis Mamani Condori',           'M', '2001-05-22'),
    ('8901234',  'Sofia Ramos Vargas',            'F', '2003-03-08'),
    ('9012345',  'Diego Alvarez Luna',            'M', '2002-09-17'),
    ('0123456',  'Valeria Cruz Ortiz',            'F', '2001-12-01'),
    ('1122334',  'Marcos Ticona Apaza',           'M', '2000-06-14'),
    ('2233445',  'Carla Huanca Leon',             'F', '2002-07-25'),
    ('3344556',  'Pablo Soto Benavides',          'M', '2001-11-30'),
    ('4455667',  'Natalia Rojas Herrera',         'F', '2003-04-10'),
    ('5566778',  'Ricardo Blanco Morales',        'M', '2002-08-19'),
    -- Administrativo (idpersona 16)
    ('6677889',  'Lic. Patricia Medina Suarez',  'F', '1985-03-25');
GO

-- ----------------------------------------------------------
-- 8.8 DOCENTES
-- ----------------------------------------------------------
INSERT INTO Docente (especialidad, idpersona) VALUES
    ('Calculo y Algebra',          1),
    ('Programacion y Algoritmos',  2),
    ('Bases de Datos',             3),
    ('Estadistica e Investigacion',4),
    ('Economia y Finanzas',        5);
GO
-- cod_registro: 1=Mendoza, 2=Gutierrez, 3=Vidal, 4=Torrez, 5=Quispe

-- ----------------------------------------------------------
-- 8.9 ESTUDIANTES
-- ----------------------------------------------------------
INSERT INTO Estudiante (cuenta_usuario, pin, idpersona) VALUES
    ('aflores',   '1234', 6),
    ('lmamani',   '2345', 7),
    ('sramos',    '3456', 8),
    ('dalvarez',  '4567', 9),
    ('vcruz',     '5678', 10),
    ('mticona',   '6789', 11),
    ('chuanca',   '7890', 12),
    ('psoto',     '8901', 13),
    ('nrojas',    '9012', 14),
    ('rblanco',   '0123', 15);
GO
-- nro_registro: 1=Ana, 2=Luis, 3=Sofia, 4=Diego, 5=Valeria,
--               6=Marcos, 7=Carla, 8=Pablo, 9=Natalia, 10=Ricardo

-- ----------------------------------------------------------
-- 8.10 ADMINISTRATIVO
-- ----------------------------------------------------------
INSERT INTO Administrativo (cargo, idpersona) VALUES
    ('Secretaria Academica', 16);
GO

-- ----------------------------------------------------------
-- 8.11 AULAS
-- ----------------------------------------------------------
INSERT INTO Aula (codigo, capacidad, ubicacion) VALUES
    ('A-101', 40, 'Edificio A - Piso 1'),
    ('A-102', 35, 'Edificio A - Piso 1'),
    ('B-201', 50, 'Edificio B - Piso 2'),
    ('B-202', 30, 'Edificio B - Piso 2'),
    ('C-LAB1',25, 'Edificio C - Laboratorio 1'),
    ('C-LAB2',25, 'Edificio C - Laboratorio 2');
GO

-- ----------------------------------------------------------
-- 8.12 GRUPOS
-- ----------------------------------------------------------
INSERT INTO Grupo (descripcion, cupo_maximo) VALUES
    ('Grupo A', 30),
    ('Grupo B', 30),
    ('Grupo C', 25),
    ('Grupo D', 25);
GO
-- id_grupo: 1=A, 2=B, 3=C, 4=D

-- ----------------------------------------------------------
-- 8.13 a 8.19 GESTIONES, HORARIOS, EDICIONES, INSCRIPCIONES Y NOTAS
-- Fechas calculadas respecto al año en curso (siempre hay gestion abierta).
-- ----------------------------------------------------------
DECLARE @Anio       INT  = YEAR(GETDATE());
DECLARE @AnioAnt    INT  = @Anio - 1;
DECLARE @IniActual  DATE = DATEFROMPARTS(@Anio, 2, 3);
DECLARE @FinActual  DATE = DATEFROMPARTS(@Anio, 12, 20);
DECLARE @IniAnt2    DATE = DATEFROMPARTS(@AnioAnt, 7, 1);
DECLARE @FinAnt2    DATE = DATEFROMPARTS(@AnioAnt, 12, 15);
DECLARE @FechaInscr DATE = DATEFROMPARTS(@AnioAnt, 1, 20);

-- 8.13 GESTIONES
-- id_gestion: 1=1/año anterior, 2=2/año anterior, 3=1/año actual (EN CURSO)
INSERT INTO Gestion (semestre, anio) VALUES
    (1, @AnioAnt),
    (2, @AnioAnt),
    (1, @Anio);

-- 8.14 HORARIOS (fechas de asistencia alineadas con cada gestion)
-- id_horario 1-6: semana inicial de la gestion actual
-- id_horario 7-9: semana inicial de la gestion 2/año anterior (cerrada)
INSERT INTO Horario (dia_semana, hora_inicio, hora_fin, fecha_asistencia) VALUES
    ('Lunes',     '07:00', '09:00', @IniActual),
    ('Martes',    '09:00', '11:00', DATEADD(DAY, 1, @IniActual)),
    ('Miercoles', '07:00', '09:00', DATEADD(DAY, 2, @IniActual)),
    ('Jueves',    '11:00', '13:00', DATEADD(DAY, 3, @IniActual)),
    ('Viernes',   '14:00', '16:00', DATEADD(DAY, 4, @IniActual)),
    ('Lunes',     '14:00', '16:00', DATEADD(DAY, 7, @IniActual)),
    ('Martes',    '16:00', '18:00', @IniAnt2),
    ('Miercoles', '09:00', '11:00', DATEADD(DAY, 1, @IniAnt2)),
    ('Jueves',    '07:00', '09:00', DATEADD(DAY, 2, @IniAnt2)),
    ('Sabado',    '08:00', '12:00', DATEADD(DAY, 4, @IniActual));

-- 8.15 EDICION_MATERIA
-- cod_edicion 1-6: gestion actual ABIERTA (plan Informatica sem 1-3 + ECO para plan Economia)
-- cod_edicion 7-9: gestion 2/año anterior CERRADA (historico)
INSERT INTO Edicion_Materia
    (fecha_inicio, fecha_fin, sigla_materia, cod_docente, id_aula, id_horario, id_gestion)
VALUES
    (@IniActual, @FinActual, 'MAT101', 1, 1, 1, 3),  -- cod_edicion 1
    (@IniActual, @FinActual, 'PRG101', 2, 5, 2, 3),  -- cod_edicion 2
    (@IniActual, @FinActual, 'ALG101', 1, 2, 3, 3),  -- cod_edicion 3
    (@IniActual, @FinActual, 'BDD101', 3, 6, 4, 3),  -- cod_edicion 4
    (@IniActual, @FinActual, 'ECO101', 5, 3, 5, 3),  -- cod_edicion 5
    (@IniActual, @FinActual, 'EST101', 4, 4, 6, 3),  -- cod_edicion 6
    (@IniAnt2,   @FinAnt2,   'MAT101', 1, 1, 7, 2),  -- cod_edicion 7
    (@IniAnt2,   @FinAnt2,   'PRG101', 2, 5, 8, 2),  -- cod_edicion 8
    (@IniAnt2,   @FinAnt2,   'ECO101', 5, 3, 9, 2);  -- cod_edicion 9

-- 8.16 EDI_GRU (grupos disponibles por edicion para inscripcion)
INSERT INTO EDI_GRU (cod_edicion, id_grupo) VALUES
    (1, 1), (1, 2),  -- MAT101: Grupos A y B
    (2, 1), (2, 2),  -- PRG101: Grupos A y B
    (3, 3),          -- ALG101: Grupo C
    (4, 3), (4, 4),  -- BDD101: Grupos C y D
    (5, 1),          -- ECO101: Grupo A
    (6, 2),          -- EST101: Grupo B
    (7, 1), (7, 2),  -- MAT101 historico
    (8, 1),          -- PRG101 historico
    (9, 1);          -- ECO101 historico

-- 8.17 INSCRIPCION_PLAN (cada estudiante inscrito en su carrera)
INSERT INTO Inscripcion_Plan (nro_registro, num_plan, fecha_inscripcion) VALUES
    (1,  1, @FechaInscr),                          -- Ana     -> Informatica
    (2,  1, @FechaInscr),                          -- Luis    -> Informatica
    (3,  1, DATEADD(DAY, 2, @FechaInscr)),         -- Sofia   -> Informatica
    (4,  2, DATEADD(DAY, 1, @FechaInscr)),         -- Diego   -> Sistemas
    (5,  2, DATEADD(DAY, 1, @FechaInscr)),         -- Valeria -> Sistemas
    (6,  3, DATEADD(DAY, 3, @FechaInscr)),         -- Marcos  -> Economia
    (7,  3, DATEADD(DAY, 3, @FechaInscr)),         -- Carla   -> Economia
    (8,  1, DATEADD(DAY, 12, @FechaInscr)),        -- Pablo   -> Informatica (sin materias aun)
    (9,  2, DATEADD(DAY, 12, @FechaInscr)),        -- Natalia -> Sistemas   (sin materias aun)
    (10, 3, DATEADD(DAY, 12, @FechaInscr));        -- Ricardo -> Economia   (sin materias aun)

-- 8.18 NOTAS - Gestion actual (cod_edicion 1-6, ediciones ABIERTAS)
-- Estudiantes 8, 9 y 10 sin notas aqui -> listos para probar INSCRIPCION.
-- Ana (1): MAT, PRG aprobadas; ALG reprobada; puede inscribir BDD101 y EST101
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (1, 1, 75.00, 80.00, 72.00),  -- MAT101  -> Aprobado
    (1, 2, 90.00, 88.00, 85.00),  -- PRG101  -> Aprobado
    (1, 3, 45.00, 50.00, 48.00);  -- ALG101  -> Reprobado

-- Luis (2): MAT aprobada; PRG pendiente (probar ingreso de notas)
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (2, 1, 60.00, 70.00, 65.00),  -- MAT101  -> Aprobado
    (2, 2, NULL,  NULL,  NULL);   -- PRG101  -> Pendiente

-- Sofia (3): solo MAT101
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (3, 1, 85.00, 90.00, 88.00);  -- MAT101  -> Aprobado

-- Diego (4) plan Sistemas: BDD aprobada; EST pendiente
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (4, 4, 70.00, 65.00, 72.00),  -- BDD101  -> Aprobado
    (4, 6, NULL,  NULL,  NULL);   -- EST101  -> Pendiente

-- Valeria (5) plan Sistemas: PRG pendiente (coherente con su pensum)
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (5, 2, NULL,  NULL,  NULL);   -- PRG101  -> Pendiente

-- Marcos (6) plan Economia: ECO reprobada; EST aprobada
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (6, 5, 40.00, 45.00, 42.00),  -- ECO101  -> Reprobado
    (6, 6, 78.00, 80.00, 75.00);  -- EST101  -> Aprobado

-- 8.19 NOTAS - Gestion 2/año anterior (cod_edicion 7-9, CERRADAS)
INSERT INTO Nota (nro_registro, cod_edicion, nota_parcial_1, nota_parcial_2, nota_final) VALUES
    (1, 7, 72.00, 75.00, 70.00),  -- Ana   MAT101 historico
    (2, 7, 55.00, 60.00, 52.00),  -- Luis  MAT101 historico
    (4, 8, 88.00, 85.00, 90.00),  -- Diego PRG101 historico
    (6, 9, 62.00, 68.00, 65.00),  -- Marcos ECO101 historico
    (7, 9, 78.00, 80.00, 82.00);  -- Carla  ECO101 historico
GO

-- ----------------------------------------------------------
-- 8.20 VERIFICACION FINAL: contar registros en cada tabla
-- ----------------------------------------------------------
USE UniversidadXYZ;
GO

SELECT 'Facultad'            AS tabla, COUNT(*) AS registros FROM Facultad
UNION ALL
SELECT 'Carrera',            COUNT(*) FROM Carrera
UNION ALL
SELECT 'Plan_Estudio',       COUNT(*) FROM Plan_Estudio
UNION ALL
SELECT 'Materia',            COUNT(*) FROM Materia
UNION ALL
SELECT 'Materia_Prerrequisito', COUNT(*) FROM Materia_Prerrequisito
UNION ALL
SELECT 'Pensum',             COUNT(*) FROM Pensum
UNION ALL
SELECT 'Persona',            COUNT(*) FROM Persona
UNION ALL
SELECT 'Docente',            COUNT(*) FROM Docente
UNION ALL
SELECT 'Estudiante',         COUNT(*) FROM Estudiante
UNION ALL
SELECT 'Administrativo',     COUNT(*) FROM Administrativo
UNION ALL
SELECT 'Gestion',            COUNT(*) FROM Gestion
UNION ALL
SELECT 'Aula',               COUNT(*) FROM Aula
UNION ALL
SELECT 'Horario',            COUNT(*) FROM Horario
UNION ALL
SELECT 'Grupo',              COUNT(*) FROM Grupo
UNION ALL
SELECT 'Edicion_Materia',    COUNT(*) FROM Edicion_Materia
UNION ALL
SELECT 'EDI_GRU',            COUNT(*) FROM EDI_GRU
UNION ALL
SELECT 'Inscripcion_Plan',   COUNT(*) FROM Inscripcion_Plan
UNION ALL
SELECT 'Nota',               COUNT(*) FROM Nota
ORDER BY tabla;
GO

PRINT '=== SEED DATA INSERTADO CORRECTAMENTE ===';

