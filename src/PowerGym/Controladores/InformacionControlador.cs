using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class InformacionControlador : IInformacionControlador
    {
        private readonly ServicioInformacion _servicio;

        public InformacionControlador(ServicioInformacion servicio)
        {
            _servicio = servicio;
        }

        public List<Visitas> ObtenerVisitas(Usuario usuario)
        {
            return _servicio.ObtenerVisitas(IdDe(usuario));
        }

        public List<Medicion> ObtenerMediciones(Usuario usuario)
        {
            return _servicio.ObtenerMediciones(IdDe(usuario));
        }

        public List<Rutina> ObtenerUltimasRutinas(Usuario usuario)
        {
            return _servicio.ObtenerUltimasRutinas(IdDe(usuario));
        }

        private static int IdDe(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));
            return usuario.Id;
        }
    }
}
