using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Entrenadores del gimnasio, hereda de persona.
    /// </summary>
    public class Entrenador :Persona
    {
        private string _especialidad;

        public string Especialidad
        {
            get { return _especialidad; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La especialidad no puede estar en blanco");
                _especialidad = value;
            }
        }

        public TipoEntrenador Tipo { get; set; }

        //Constructor del entrenador
        public Entrenador(int id, string nombreUsuario, string nombre,int edad, string especialidad, TipoEntrenador tipo):base(id, nombreUsuario,nombre, edad)
        {
            Especialidad = especialidad;
            Tipo = tipo;
        }
            
    }
}
