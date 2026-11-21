using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;


namespace PowerGym.Controladores
{
    public class ClasesControlador : IClasesControlador
    {
        private readonly ServicioClases _servicio;

        public ClasesControlador(ServicioClases servicio)
        {
            _servicio = servicio;
        }

        public List<ClasesGimnasio> ObtenerClases()
        {
            return _servicio.ObtenerClases();
        }

        public string ObtenerNombreEntrenador(int idEntrenador)
        {
            return _servicio.ObtenerNombreEntrenador(idEntrenador);
        }

        public bool EstaInscrito(Usuario usuario, ClasesGimnasio clase)
        {
            return _servicio.EstaInscrito(usuario.Id, clase.Id);
        }

        public void Inscribir(Usuario usuario, ClasesGimnasio clase)
        {
            if (usuario == null || clase == null)
                throw new ArgumentException("Seleccione una clase.");

            _servicio.Inscribir(usuario.Id, clase.Id);
        }

    }
}
