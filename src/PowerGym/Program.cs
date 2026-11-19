using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerGym
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var rutinas = new PowerGym.Repositorios.RepositorioRutinas().ObtenerTodos();
            int totalEjercicios = 0;
            foreach (var r in rutinas)
                totalEjercicios += r.Ejercicios.Count;
            MessageBox.Show($"Rutinas: {rutinas.Count}\nEjercicios: {totalEjercicios}");


            Application.Run(new FormPrincipal());
        }
    }
}
