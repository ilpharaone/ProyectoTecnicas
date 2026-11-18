using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Clase base de todas las actividades del gimnasio.
    /// </summary>
    public abstract class ClasesGimnasio
    {
        private string _nombre;
        private int _cupoMaximo;

        public int Id { get; set; }

        
        public int IdEntrenador { get; set; }

        
        public DateTime Horario { get; set; }

        public int InscritosActuales { get; private set; }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la clase no puede estar en blanco.");
                _nombre = value;
            }
        }
        public int CupoMaximo
        {
            get { return _cupoMaximo; }
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException("El cupo debe estar entre 1 y 100.");
                _cupoMaximo = value;
            }
        }

        // calcula si ya no caben más personas.
        public bool EstaLlena => InscritosActuales >= CupoMaximo;

        protected ClasesGimnasio(int id, int idEntrenador, string nombre,
                                DateTime horario, int cupoMaximo)
        {
            Id = id;
            IdEntrenador = idEntrenador;
            Nombre = nombre;
            Horario = horario;
            CupoMaximo = cupoMaximo;  
        }

        public abstract int DuracionMinutos();

        public abstract bool RequiereInscripcion();

        public virtual void Inscribir()
        {
            
            if (EstaLlena)
                throw new InvalidOperationException("La clase está llena.");
            InscritosActuales++;   
        }

        public void CantidadInscritos(int cantidad)
        {
            if (cantidad < 0 || cantidad > CupoMaximo)
                throw new ArgumentException("Cantidad de inscritos inválida.");
            InscritosActuales = cantidad;
        }
    }
}
