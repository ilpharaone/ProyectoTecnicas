using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Repositorios;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ServicioEstadisticas
    {
        private readonly IRepositorio<Rutina> _rutinas;
        private readonly IRepositorio<Medicion> _mediciones;
        private readonly IRepositorio<Visitas> _visitas;
        private readonly IRepositorio<Usuario> _usuarios;

        public ServicioEstadisticas(IRepositorio<Rutina> rutinas,
                                    IRepositorio<Medicion> mediciones,
                                    IRepositorio<Visitas> visitas,
                                    IRepositorio<Usuario> usuarios)
        {
            _rutinas = rutinas;
            _mediciones = mediciones;
            _visitas = visitas;
            _usuarios = usuarios;
        }
        public List<AumentoEjercicio> ObtenerAumentos(int idUsuario, DateTime desde, DateTime hasta)
        {
            return _rutinas.ObtenerTodos()
                .Where(r => r.IdUsuario == idUsuario
                         && r.FechaInicio.Date >= desde.Date
                         && r.FechaInicio.Date <= hasta.Date)
                .OrderBy(r => r.FechaInicio)            // de la más antigua a la más reciente
                .SelectMany(r => r.Ejercicios)          // junta los ejercicios de todas las rutinas
                .GroupBy(e => e.Nombre, StringComparer.OrdinalIgnoreCase)   // un grupo por ejercicio
                .Where(grupo => grupo.Count() >= 2)     // hace falta comparar al menos 2 veces
                                                        // First = la primera vez, Last = la última. GroupBy conserva el orden.
                .Select(grupo => new AumentoEjercicio(grupo.Key, grupo.First().Peso, grupo.Last().Peso))
                .OrderBy(a => a.Ejercicio)
                .ToList();
        }

        public ResumenMediciones ObtenerResumenMediciones(int idUsuario, DateTime desde, DateTime hasta)
        {
            List<Medicion> enRango = _mediciones.ObtenerTodos()
                .Where(m => m.IdUsuario == idUsuario
                         && m.Fecha.Date >= desde.Date
                         && m.Fecha.Date <= hasta.Date)
                .OrderBy(m => m.Fecha)
                .ToList();

            if (enRango.Count < 2)
                return null;

            return new ResumenMediciones(enRango[0], enRango[enRango.Count-1]);
        }

        public List<Ranking> ObtenerTopVisitantes(DateTime desde, DateTime hasta)
        {
            return CalcularRanking(desde, hasta, true);
        }

        public List<Ranking> ObtenerTopAusentes(DateTime desde, DateTime hasta)
        {
            return CalcularRanking(desde, hasta, false);
        }

        // Un solo método para los dos rankings: solo cambia el orden.
        private List<Ranking> CalcularRanking(DateTime desde, DateTime hasta, bool masVisitas)
        {
            // Clave = id del usuario, valor = visitas dentro del rango.
            Dictionary<int, int> conteo = _visitas.ObtenerTodos()
                .Where(v => v.Fecha.Date >= desde.Date && v.Fecha.Date <= hasta.Date)
                .GroupBy(v => v.IdUsuario)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());

            // Se incluyen TODOS los usuarios: quien no fue ni una vez cuenta 0.
            var filas = _usuarios.ObtenerTodos()
                .Select(u => new { Usuario = u, Total = conteo.TryGetValue(u.Id, out int t) ? t : 0 });

            // El desempate es por id para que el resultado siempre sea el mismo.
            var ordenadas = masVisitas
                ? filas.OrderByDescending(f => f.Total).ThenBy(f => f.Usuario.Id)
                : filas.OrderBy(f => f.Total).ThenBy(f => f.Usuario.Id);

            // Select con índice: "i" es la posición (0, 1, 2...), por eso i + 1.
            return ordenadas.Take(10)
                .Select((f, i) => new Ranking(i + 1, f.Usuario.Id, f.Usuario.Nombre, f.Total))
                .ToList();
        }

    }
}
