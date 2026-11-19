using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class MisUsuariosControlador : IMisUsuariosControlador
    {
        private readonly ServicioEntrenadores _servicio;

        public MisUsuariosControlador(ServicioEntrenadores servicio)
        {
            _servicio = servicio;
        }

        public List<Usuario> ObtenerUsuarios(Entrenador entrenador)
        {
            if (entrenador == null)
                throw new ArgumentNullException(nameof(entrenador));

            return _servicio.ObtenerUsuariosDe(entrenador.Id);
        }
    }
}
