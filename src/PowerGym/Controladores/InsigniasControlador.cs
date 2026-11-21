using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    
    
        public class InsigniasControlador : IInsigniasControlador
        {
            private readonly ServicioInsignias _servicio;

            public InsigniasControlador(ServicioInsignias servicio)
            {
                _servicio = servicio;
            }

            public List<Insignia> ObtenerInsignias(Usuario usuario)
            {
                if (usuario == null)
                    throw new ArgumentNullException(nameof(usuario));

                // Primero actualiza (otorga o retira), luego devuelve la lista al día.
                _servicio.EvaluarUsuario(usuario.Id);
                return _servicio.ObtenerInsignias(usuario.Id);
            }
        }
    }

