using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerGym.Controladores;
using PowerGym.Repositorios;
using PowerGym.Servicios;
using PowerGym.Vistas;

namespace PowerGym
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            var repoUsuario = new RepositorioUsuarios();
            var repoEntrenadores = new RepositorioEntrenadores();
            var repoRutinas = new RepositorioRutinas();
            var repoVisitas = new RepositorioVisitas();
            var repoMediciones = new RepositorioMediciones();
            var repoClases = new RepositorioClases();
            var repoInsignias = new RepositorioInsignias();
            var repoNotificaciones = new RepositorioNotificaciones();

            var servicioEntrenadores = new ServicioEntrenadores(repoUsuario);
            var servicioRutinas = new ServicioRutinas(repoRutinas);

            var reglas = new List<IReglaInsignia>
            {
                new ReglaVisitas(), new ReglaMusculatura(), new ReglaGrasa(), new ReglaPocasVisitas(),
                new ReglaSinProgreso(), new ReglaObjetivosIncumplidos()
            };

            var servicioInsignias = new ServicioInsignias(repoInsignias, repoUsuario, repoVisitas, repoMediciones, reglas);
            var servicioNotificaciones = new ServicioNotificaciones(repoNotificaciones, repoUsuario, repoRutinas, repoMediciones, repoVisitas);

            servicioInsignias.InsigniaOtorgada += servicioNotificaciones.AlOtorgarseInsignia;
            servicioRutinas.RutinaAgregada += servicioNotificaciones.AlAgregarseRutina;

            servicioNotificaciones.GenerarAvisosAlCargar();
            servicioInsignias.EvaluarTodos();


            var loginControlador = new LoginControlador(new Autenticacion(repoUsuario, repoEntrenadores));

            var registroControlador = new RegistroControlador(
                new Registro(repoUsuario, repoEntrenadores));

            var controladores = new ContenedorControladores
            {
                MisUsuarios = new MisUsuariosControlador(servicioEntrenadores),
                Rutinas = new RutinasControlador(servicioRutinas, servicioEntrenadores),
                Aumentos = new AumentoControlador(new Aumentos(repoRutinas)),
                Clases = new ClasesControlador(
                    new ServicioClases(repoClases, repoVisitas, repoEntrenadores)),
                Insignias = new InsigniasControlador(servicioInsignias),
                Notificaciones = new NotificacionesControlador(servicioNotificaciones),
                Informacion = new InformacionControlador(
                    new ServicioInformacion(repoVisitas, repoMediciones, repoRutinas)),
                Estadisticas = new EstadisticasControlador(
                    new ServicioEstadisticas(repoRutinas, repoMediciones, repoVisitas, repoUsuario))
            };

            bool volverAlLogin = true;
            while (volverAlLogin)
            {
                using (var login = new FormLogin(loginControlador, registroControlador))
                {
                    // Si cierran el login sin ingresar, el programa termina.
                    if (login.ShowDialog() != DialogResult.OK)
                        return;

                    using (var principal = new FormPrincipal(login.PersonaAutenticada, controladores))
                    {
                        principal.ShowDialog();
                        // Cerró con "Cerrar sesión" -> repite; cerró con la X -> termina.
                        volverAlLogin = principal.CerroSesion;
                    }
                }
            }
            
        }
    }
}
