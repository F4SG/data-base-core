using System.Configuration;
using System.Data.SqlClient;

namespace UniversidadXYZ.Datos
{
    /// <summary>
    /// Capa de Datos: únicamente responsable de proveer la conexión a SQL Server.
    /// NO contiene CRUD, Stored Procedures ni lógica de negocio.
    /// La cadena de conexión se lee del App.config del proyecto Presentacion.
    /// </summary>
    public static class Conexion
    {
        // ---------------------------------------------------------------
        // Cadena de conexión leída desde App.config / Web.config
        // Clave: "UniversidadXYZ"
        // Ejemplo: Server=.\SQLEXPRESS;Database=UniversidadXYZ;Trusted_Connection=True;
        // ---------------------------------------------------------------
        public static string CadenaConexion
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["UniversidadXYZ"].ConnectionString;
            }
        }

        /// <summary>
        /// Devuelve una SqlConnection abierta lista para usar.
        /// El llamador es responsable de cerrarla (using o Close()).
        /// </summary>
        public static SqlConnection ObtenerConexion()
        {
            SqlConnection con = new SqlConnection(CadenaConexion);
            con.Open();
            return con;
        }
    }
}
