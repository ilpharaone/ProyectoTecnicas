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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var controlador = new PowerGym.Controladores.LoginControlador(
                new PowerGym.Servicios.Autenticacion(
                    new PowerGym.Repositorios.RepositorioUsuarios(),
                    new PowerGym.Repositorios.RepositorioEntrenadores()));
            bool volver = true;
            while (volver)
            {
                using (var login = new PowerGym.Vistas.FormLogin(controlador))
                {
                    // ShowDialog abre la ventana y espera. Si la cierran sin ingresar, se sale.
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                    using (var principal = new PowerGym.Vistas.FormPrincipal(login.PersonaAutenticada))
                    {
                        principal.ShowDialog();
                        volver = principal.CerroSesion;
                    }
                    Application.Run(new PowerGym.Vistas.FormPrincipal(login.PersonaAutenticada));
                }

            }
        }
    }
}
