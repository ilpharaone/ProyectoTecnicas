using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioInformacion
    {
        private readonly IRepositorio<Visitas> _visitas;
        private readonly IRepositorio<Medicion> _mediciones;
        private readonly IRepositorio<Rutina> _rutinas;

        public ServicioInformacion(IRepositorio<Visitas> visitas,
                                   IRepositorio<Medicion> mediciones,
                                   IRepositorio<Rutina> rutinas)
        {
            _visitas = visitas;
            _mediciones = mediciones;
            _rutinas = rutinas;
        }

        // Visitas del usuario, la más reciente primero.
        public List<Visitas> ObtenerVisitas(int idUsuario)
        {
            return _visitas.ObtenerTodos()
                           .Where(v => v.IdUsuario == idUsuario)   // solo las de este usuario
                           .OrderByDescending(v => v.Fecha)        // de la más nueva a la más vieja
                           .ToList();
        }

        // Mediciones del usuario, la más reciente primero.
        public List<Medicion> ObtenerMediciones(int idUsuario)
        {
            return _mediciones.ObtenerTodos()
                              .Where(m => m.IdUsuario == idUsuario)
                              .OrderByDescending(m => m.Fecha)
                              .ToList();
        }

        // Las 5 rutinas más recientes. Take(5) se queda con las primeras 5 de la lista ya ordenada.
        public List<Rutina> ObtenerUltimasRutinas(int idUsuario, int cantidad = 5)
        {
            return _rutinas.ObtenerTodos()
                           .Where(r => r.IdUsuario == idUsuario)
                           .OrderByDescending(r => r.FechaInicio)
                           .Take(cantidad)
                           .ToList();
        }
    }
}
