using System;
using System.Windows.Forms;
using SqlServerTypes;

namespace UniversidadXYZ.Presentacion
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            }
            catch
            {
                // ReportViewer puede funcionar sin tipos espaciales de SQL Server.
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}
