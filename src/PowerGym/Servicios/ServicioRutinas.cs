using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioRutinas
    {
        private readonly IRepositorio<Rutina> _rutinas;

        // "event" permite que otras clases se "suscriban" y reciban un aviso.
        // Aquí avisamos cuando se agrega una rutina. 
        public event Action<Rutina> RutinaAgregada;

        public ServicioRutinas(IRepositorio<Rutina> rutinas)
        {
            _rutinas = rutinas;
        }

        public List<Rutina> ObtenerRutinasDeUsuario(int idUsuario)
        {
            return _rutinas.ObtenerTodos()
                           .Where(r => r.IdUsuario == idUsuario)
                           .OrderByDescending(r => r.FechaInicio)
                           .ToList();
        }

        public Rutina Crear(int idUsuario, int idEntrenador, string nombre,
                            DateTime inicio, DateTime fin, List<Ejercicio> ejercicios)
        {
            ValidarEjercicios(ejercicios);

            // Id 0 provisional: el repositorio asigna el real al agregar.
            // El constructor valida el nombre y que fin no sea anterior a inicio.
            Rutina nueva = new Rutina(0, idUsuario, idEntrenador, nombre, inicio, fin);

            foreach (Ejercicio e in ejercicios)
                nueva.Agregar(e);

            _rutinas.Agregar(nueva);

            // "?." significa: si hay alguien suscrito, avísale; si no, no hagas nada.
            RutinaAgregada?.Invoke(nueva);
            return nueva;
        }

        public void Editar(int idRutina, string nombre, DateTime inicio,
                           DateTime fin, List<Ejercicio> ejercicios)
        {
            Rutina rutina = _rutinas.ObtenerPorId(idRutina);
            if (rutina == null)
                throw new KeyNotFoundException("La rutina ya no existe.");

            // Se valida TODO antes de tocar la rutina. El repositorio devuelve el
            // mismo objeto que tiene en memoria, así que un error a medias la dejaría
            // con datos mezclados.
            ValidarEjercicios(ejercicios);
            if (fin < inicio)
                throw new ArgumentException("La fecha de fin no puede ser anterior a la de inicio.");

            rutina.Nombre = nombre;      // lanza error si está vacío, antes de cambiar lo demás
            rutina.FechaInicio = inicio; // inicio va antes que fin (fin se valida contra inicio)
            rutina.FechaFin = fin;

            rutina.Ejercicios.Clear();
            foreach (Ejercicio e in ejercicios)
                rutina.Agregar(e);

            
            rutina.Version++;

            _rutinas.Actualizar(rutina);
        }

        public void Eliminar(int idRutina)
        {
            
            _rutinas.Eliminar(idRutina);
        }

        private static void ValidarEjercicios(List<Ejercicio> ejercicios)
        {
            if (ejercicios == null || ejercicios.Count == 0)
                throw new ArgumentException("La rutina debe tener al menos un ejercicio.");
        }

        public List<string> ObtenerNombresEjercicios()
        {
            return _rutinas.ObtenerTodos()
                           .SelectMany(r => r.Ejercicios)   // junta los ejercicios de TODAS las rutinas
                           .Select(e => e.Nombre)           // se queda solo con el nombre
                           .Distinct(StringComparer.OrdinalIgnoreCase)   // quita repetidos
                           .OrderBy(nombre => nombre)       // orden alfabético
                           .ToList();
        }

        public bool EsDePesoCorporal(string nombre)
        {
            List<Ejercicio> coincidencias = _rutinas.ObtenerTodos()
                .SelectMany(r => r.Ejercicios)
                .Where(e => string.Equals(e.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // "All" es true si TODOS cumplen la condición. Si no hay coincidencias, es false.
            return coincidencias.Count > 0 && coincidencias.All(e => e.Peso == 0);
        }
    }
}
