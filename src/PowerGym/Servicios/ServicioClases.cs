using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioClases
    {
        private readonly IRepositorio<ClasesGimnasio> _clases;
        private readonly IRepositorio<Visitas> _visitas;
        private readonly IRepositorio<Entrenador> _entrenadores;

        public ServicioClases(IRepositorio<ClasesGimnasio> clases,
                              IRepositorio<Visitas> visitas,
                              IRepositorio<Entrenador> entrenadores)
        {
            _clases = clases;
            _visitas = visitas;
            _entrenadores = entrenadores;
        }

        // Todas las clases, ordenadas por horario.
        public List<ClasesGimnasio> ObtenerClases()
        {
            return _clases.ObtenerTodos().OrderBy(c => c.Horario).ToList();
        }

        public string ObtenerNombreEntrenador(int idEntrenador)
        {
            Entrenador entrenador = _entrenadores.ObtenerPorId(idEntrenador);
            return entrenador == null ? "Sin asignar" : entrenador.Nombre;
        }

        // Está inscrito si existe una visita suya con el id de esa clase.
        // "Any" devuelve true si al menos una visita cumple la condición.
        public bool EstaInscrito(int idUsuario, int idClase)
        {
            return _visitas.ObtenerTodos()
                           .Any(v => v.IdUsuario == idUsuario && v.IdClase == idClase);
        }

        public void Inscribir(int idUsuario, int idClase)
        {
            ClasesGimnasio clase = _clases.ObtenerPorId(idClase);
            if (clase == null)
                throw new KeyNotFoundException("La clase ya no existe.");

            // 1) El gimnasio libre no se inscribe: se entra sin reservar.
            if (!clase.RequiereInscripcion())
                throw new ArgumentException("Esta actividad no requiere inscripción.");

            // 2) No se permite inscribirse dos veces.
            if (EstaInscrito(idUsuario, idClase))
                throw new ArgumentException("Ya está inscrito en esta clase.");

            // 3) No se permite superar el cupo.
            if (clase.EstaLlena)
                throw new ArgumentException("La clase está llena.");

            // La asistencia cuenta como visita. Id 0: el repositorio asigna el real.
            _visitas.Agregar(new Visitas(0, idUsuario, clase.Horario, clase.Id));

            // Suma 1 a los inscritos y guarda clases.csv.
            clase.Inscribir();
            _clases.Actualizar(clase);
        }


    }
}
