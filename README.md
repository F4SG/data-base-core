# Data Base Core - UniversidadXYZ

Este proyecto contiene el sistema completo de `UniversidadXYZ`, incluyendo su backend y base de datos en C#/SQL Server, y scripts auxiliares en Node.js.

## Requisitos Previos

Para poder ejecutar este proyecto en cualquier máquina, necesitas instalar lo siguiente:

- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** (o la versión que uses) con la carga de trabajo de "Desarrollo de escritorio de .NET".
- **[SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)** y SQL Server local.
- **[Node.js](https://nodejs.org/)** (versión LTS recomendada).

## Estructura del Proyecto

- `PoyectoBD/`: Contiene la solución de Visual Studio (`UniversidadXYZ.sln`) en C# y la capa de acceso a datos.
  - **Base de Datos**: El archivo `UniversidadXYZ_Final.sql` contiene los scripts para crear la base de datos y todas sus tablas.
- `WEB/`: Scripts hechos en Node.js (ej. `index.js`, `generar_doc.js`) que interactúan o generan documentos a partir del sistema.

## Pasos para la Instalación

### 1. Configuración de la Base de Datos
1. Abre **SQL Server Management Studio**.
2. Conéctate a tu servidor local (usualmente `.` o `localhost\SQLEXPRESS`).
3. Abre el archivo de script `PoyectoBD/UniversidadXYZ_Final.sql`.
4. Ejecuta el script. Esto creará la base de datos `UniversidadXYZ` (o el nombre especificado en el script) y sus tablas correspondientes.

### 2. Configurar y Ejecutar C# / .NET
1. Entra a la carpeta `PoyectoBD/`.
2. Abre la solución `UniversidadXYZ.sln` con Visual Studio.
3. El administrador de paquetes NuGet descargará las dependencias automáticamente la primera vez que intentes compilar. Si no, haz clic derecho sobre la Solución y selecciona **Restaurar paquetes NuGet**.
4. Es posible que tengas que actualizar tu **Cadena de Conexión (Connection String)** en el archivo de configuración del proyecto (`App.config` o similar dentro de la capa `gDatos` o `Presentacion`) para que apunte a tu servidor de base de datos local.
5. Ejecuta el proyecto presionando `F5` o dándole al botón de **Iniciar** en Visual Studio.

### 3. Configurar Scripts de Node.js (Carpeta WEB)
1. Abre una terminal (Símbolo del sistema o PowerShell).
2. Navega hasta la carpeta `WEB/`:
   ```bash
   cd WEB
   ```
3. Instala las dependencias:
   ```bash
   npm install
   ```
4. Para ejecutar algún script, como por ejemplo `index.js` o `generar_doc.js`, usa el comando:
   ```bash
   node index.js
   # o
   node generar_doc.js
   ```

## Notas Adicionales
- La carpeta `node_modules` y las carpetas de compilación `bin/` y `obj/` no están incluidas en este repositorio por buenas prácticas, por eso se deben instalar las dependencias localmente y compilar el código.
