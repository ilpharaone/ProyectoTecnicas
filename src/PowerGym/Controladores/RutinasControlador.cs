using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class RutinasControlador : IRutinasControlador
    {
        private readonly ServicioRutinas _rutinas;
        private readonly ServicioEntrenadores _entrenadores;

        public RutinasControlador(ServicioRutinas rutinas, ServicioEntrenadores entrenadores)
        {
            _rutinas = rutinas;
            _entrenadores = entrenadores;
        }

        public List<Usuario> ObtenerUsuarios(Entrenador entrenador)
        {
            return _entrenadores.ObtenerUsuariosDe(entrenador.Id);
        }

        public List<Rutina> ObtenerRutinas(int idUsuario)
        {
            return _rutinas.ObtenerRutinasDeUsuario(idUsuario);
        }

        public Ejercicio CrearEjercicio(string nombre, string series,
                                        string repeticiones, string pesoKg)
        {
            if (!int.TryParse(series, out int seriesNum))
                throw new ArgumentException("Las series deben ser un número entero.");

            if (!int.TryParse(repeticiones, out int repsNum))
                throw new ArgumentException("Las repeticiones deben ser un número entero.");

            // Acepta "60", "60.5" y "60,5".
            if (!double.TryParse((pesoKg ?? "").Trim().Replace(',', '.'),
                                 NumberStyles.Float, CultureInfo.InvariantCulture,
                                 out double pesoNum))
                throw new ArgumentException(
                    "El peso debe ser un número. Escriba 0 si el ejercicio es con el propio cuerpo.");

            // Los rangos los valida el modelo (Ejercicio).
            return new Ejercicio(nombre, seriesNum, repsNum, pesoNum);
        }

        public Rutina Crear(Entrenador entrenador, int idUsuario, string nombre,
                            DateTime inicio, DateTime fin, List<Ejercicio> ejercicios)
        {
            return _rutinas.Crear(idUsuario, entrenador.Id, nombre, inicio, fin, ejercicios);
        }

        public void Editar(int idRutina, string nombre, DateTime inicio,
                           DateTime fin, List<Ejercicio> ejercicios)
        {
            _rutinas.Editar(idRutina, nombre, inicio, fin, ejercicios);
        }

        public void Eliminar(int idRutina)
        {
            _rutinas.Eliminar(idRutina);
        }

        public List<string> ObtenerNombresEjercicios()
        {
            return _rutinas.ObtenerNombresEjercicios();
        }

        public bool EsDePesoCorporal(string nombre)
        {
            return _rutinas.EsDePesoCorporal(nombre);
        }




    }
}
