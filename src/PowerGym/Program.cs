using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerGym
{
    static class Program
    {

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var repoUsuario = new PowerGym.Repositorios.RepositorioUsuarios();
            var repoEntrenadores = new PowerGym.Repositorios.RepositorioEntrenadores();

            var loginControlador = new PowerGym.Controladores.LoginControlador(
                new PowerGym.Servicios.Autenticacion(repoUsuario, repoEntrenadores));

            var registroControlador = new PowerGym.Controladores.RegistroControlador(
                new PowerGym.Servicios.Registro(repoUsuario, repoEntrenadores));

            var servicioEntrenadores = new PowerGym.Servicios.ServicioEntrenadores(repoUsuario);

            var misUsuariosControlador =
                new PowerGym.Controladores.MisUsuariosControlador(servicioEntrenadores);

            var repoRutinas = new PowerGym.Repositorios.RepositorioRutinas();
            var rutinasControlador = new PowerGym.Controladores.RutinasControlador(
                new PowerGym.Servicios.ServicioRutinas(repoRutinas), servicioEntrenadores);

            var aumentosControlador = new PowerGym.Controladores.AumentoControlador(new PowerGym.Servicios.Aumentos(repoRutinas));
            bool volver = true;
            while (volver)
            {
                using (var login = new PowerGym.Vistas.FormLogin(loginControlador,registroControlador))
                {
                    // ShowDialog abre la ventana y espera. Si la cierran sin ingresar, se sale.
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                    using (var principal = new PowerGym.Vistas.FormPrincipal(login.PersonaAutenticada,misUsuariosControlador,rutinasControlador,aumentosControlador))
                    {
                        principal.ShowDialog();
                        volver = principal.CerroSesion;
                    }
                   
                }

            }
        }
    }
}
