using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    ///Clase abstracta de persona, no se puede crear una "Persona" sola, solo sus hijas(usuarios, entrenadores)
    public abstract class Persona
    {

        private string _nombre;
        private int _edad;
        
        protected Persona(int id, string nombreUsuario, string nombre, int edad)
        {
            Id = id;
            NombreUsuario = nombreUsuario;
            Nombre = nombre;
            Edad = edad;
        }
        public int Id { get; set; }
        public string NombreUsuario { get; set; }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                // IsNullOrWhiteSpace es true si es null, "" o solo espacios.
                if (string.IsNullOrWhiteSpace(value))
                    //Validacion
                    throw new ArgumentException("El nombre no puede estar en blanco.");
                // Si no da error, se guarda.
                _nombre = value;
            }
        }

        // confirmacion de edades validas
        public int Edad
        {
            get { return _edad; }
            set
            {
                if (value < 15 || value > 80)
                    throw new ArgumentException("La edad debe estar entre 15 y 80");
                _edad = value;

            }
        
        }



    }
}
