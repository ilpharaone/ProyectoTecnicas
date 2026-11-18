using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Rutina asignada a usuarios
    /// </summary>
    public class Rutina
    {
        private string _nombre;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;

        //Identificador de rutina
        public int Id { get; set; }

        //Id del usuario de la rutina, se guarda solo el id
        public int IdUsuario { get; set; }

        //Id del entrenador creador
        public int IdEntrenador { get; set; }

        public int Version { get; set; }

        //Lista de ejercicios, se inicializa vacia.
        public List<Ejercicio> Ejercicios { get; set; } = new List<Ejercicio>();

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la rutina no puede estar en blanco.");
                _nombre = value;
            }
        }

        // Fecha desde la que la rutina se hizo.
        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }

        //Fecha que termina y recordatorio
        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set
            {
                
                if (value < _fechaInicio)
                    throw new ArgumentException("La fecha de fin no puede ser anterior a la de inicio.");
                _fechaFin = value;
            }
        }

        public bool EstaVencida => FechaFin < DateTime.Today;

        public Rutina(int id, int idUsuario, int idEntrenador, string nombre,
                      DateTime fechaInicio, DateTime fechaFin)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdEntrenador = idEntrenador;
            Version = 1;
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public void Agregar(Ejercicio ejercicio)
        {
            if (ejercicio == null)
                throw new ArgumentException(nameof(ejercicio));
            Ejercicios.Add(ejercicio);
        }
    }
}
