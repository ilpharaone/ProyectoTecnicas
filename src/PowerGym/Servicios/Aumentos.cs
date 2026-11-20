using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Repositorios;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class Aumentos
    {
        private readonly IRepositorio<Rutina> _rutinas;

        public Aumentos(IRepositorio<Rutina> rutinas)
        {
            _rutinas = rutinas;
        }

        public List<CambioEjercicio> ObtenerHistorial(int idUsuario)
        {
            var resultado = new List<CambioEjercicio>();

            // Rutinas del usuario de la más antigua a la más reciente.
            List<Rutina> rutinas = _rutinas.ObtenerTodos()
                                           .Where(r => r.IdUsuario == idUsuario)
                                           .OrderBy(r => r.FechaInicio)
                                           .ToList();

            // Recuerda la última vez que apareció cada ejercicio.
            // Clave = nombre del ejercicio, sin distinguir mayúsculas.
            var anteriores = new Dictionary<string, Ejercicio>(StringComparer.OrdinalIgnoreCase);

            foreach (Rutina rutina in rutinas)
            {
                foreach (Ejercicio actual in rutina.Ejercicios)
                {
                    // Si no estaba, "previo" queda en null (primera vez del ejercicio).
                    anteriores.TryGetValue(actual.Nombre, out Ejercicio previo);

                    int difSeries = previo == null ? 0 : actual.Serie - previo.Serie;
                    int difReps = previo == null ? 0 : actual.Repeticiones - previo.Repeticiones;
                    double difPeso = previo == null ? 0 : actual.Peso - previo.Peso;

                    resultado.Add(new CambioEjercicio(
                        actual.Nombre, rutina.Nombre, rutina.FechaInicio,
                        actual.Serie, actual.Repeticiones, actual.Peso,
                        difSeries, difReps, difPeso));
                }

                anteriores[actual.Nombre] = actual;
            }

            return resultado.OrderBy(c => c.Ejercicio).ThenBy(c => c.Fecha).ToList();
        }
    }
}