using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class AumentoControlador: IAumentosControlador
    {
        private readonly Aumentos _servicio;

        public AumentoControlador(Aumentos servicio)
        {
            _servicio = servicio;
        }

        public List<CambioEjercicio> ObtenerHistorial(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            return _servicio.ObtenerHistorial(usuario.Id);
        }
    }
}
