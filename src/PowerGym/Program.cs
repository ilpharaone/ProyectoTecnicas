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
            var servicioRutinas = new PowerGym.Servicios.ServicioRutinas(repoRutinas);
            var rutinasControlador = new PowerGym.Controladores.RutinasControlador(
                servicioRutinas, servicioEntrenadores);

            var aumentosControlador = new PowerGym.Controladores.AumentoControlador(new PowerGym.Servicios.Aumentos(repoRutinas));

            var repoClases = new PowerGym.Repositorios.RepositorioClases();
            var repoVisitas = new PowerGym.Repositorios.RepositorioVisitas();
            var repoMediciones = new PowerGym.Repositorios.RepositorioMediciones();
            var clasesControlador = new PowerGym.Controladores.ClasesControlador(new PowerGym.Servicios.ServicioClases(repoClases, repoVisitas, repoEntrenadores));
            var repoInsignias = new PowerGym.Repositorios.RepositorioInsignias();
            var reglas = new List<PowerGym.Servicios.IReglaInsignia>
            {
                new PowerGym.Servicios.ReglaVisitas(),
                new PowerGym.Servicios.ReglaMusculatura(),
                new PowerGym.Servicios.ReglaGrasa(),
                new PowerGym.Servicios.ReglaPocasVisitas(),
                new PowerGym.Servicios.ReglaSinProgreso(),
                new PowerGym.Servicios.ReglaObjetivosIncumplidos()
            };

            var servicioInsignias = new PowerGym.Servicios.ServicioInsignias(repoInsignias, repoUsuario, repoVisitas, repoMediciones, reglas);
            servicioInsignias.EvaluarTodos();
            var insigniasControlador = new PowerGym.Controladores.InsigniasControlador(servicioInsignias);
           
            var repoNotificaciones = new PowerGym.Repositorios.RepositorioNotificaciones();
            var servicioNotificaciones = new PowerGym.Servicios.ServicioNotificaciones(repoNotificaciones, repoUsuario, repoRutinas, repoMediciones, repoVisitas);
            servicioInsignias.InsigniaOtorgada += servicioNotificaciones.AlOtorgarseInsignia;
            servicioRutinas.RutinaAgregada += servicioNotificaciones.AlAgregarseRutina;
            servicioNotificaciones.GenerarAvisosAlCargar();

            var notificacionesControlador = new PowerGym.Controladores.NotificacionesControlador(servicioNotificaciones);
            bool volver = true;
            while (volver)
            {
                using (var login = new PowerGym.Vistas.FormLogin(loginControlador,registroControlador))
                {
                    // ShowDialog abre la ventana y espera. Si la cierran sin ingresar, se sale.
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                    using (var principal = new PowerGym.Vistas.FormPrincipal(login.PersonaAutenticada,misUsuariosControlador,rutinasControlador,
                        aumentosControlador,clasesControlador,insigniasControlador,notificacionesControlador))
                    {
                        principal.ShowDialog();
                        volver = principal.CerroSesion;
                    }
                   
                }

            }
        }
    }
}
