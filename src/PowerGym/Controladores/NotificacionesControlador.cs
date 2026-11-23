using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class NotificacionesControlador:INotificacionesControlador
    {
        private readonly ServicioNotificaciones _servicio;

        public NotificacionesControlador(ServicioNotificaciones servicio)
        {
            _servicio = servicio;
        }

        public List<Notificacion> ObtenerTimeline(Persona persona)
        {
            if (persona == null)
                throw new ArgumentNullException(nameof(persona));

            // El servicio necesita saber el rol porque los ids se repiten entre roles.
            return _servicio.ObtenerTimeline(persona.Id, persona is Entrenador);
        }
    }
}
