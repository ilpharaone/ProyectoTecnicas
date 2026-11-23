using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class EstadisticasControlador : IEstadisticasControlador
    {
        private readonly ServicioEstadisticas _servicio;

        public EstadisticasControlador(ServicioEstadisticas servicio)
        {
            _servicio = servicio;
        }

        public List<AumentoEjercicio> ObtenerAumentos(Usuario usuario, DateTime desde, DateTime hasta)
        {
            ValidarRango(desde, hasta);
            return _servicio.ObtenerAumentos(usuario.Id, desde, hasta);
        }

        public ResumenMediciones ObtenerResumenMediciones(Usuario usuario, DateTime desde, DateTime hasta)
        {
            ValidarRango(desde, hasta);
            return _servicio.ObtenerResumenMediciones(usuario.Id, desde, hasta);
        }

        public List<Ranking> ObtenerTopVisitantes(DateTime desde, DateTime hasta)
        {
            ValidarRango(desde, hasta);
            return _servicio.ObtenerTopVisitantes(desde, hasta);
        }

        public List<Ranking> ObtenerTopAusentes(DateTime desde, DateTime hasta)
        {
            ValidarRango(desde, hasta);
            return _servicio.ObtenerTopAusentes(desde, hasta);
        }

        private static void ValidarRango(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            if (hasta.Date > DateTime.Today)
                throw new ArgumentException("La fecha de fin no puede ser futura.");
        }
    }
}
