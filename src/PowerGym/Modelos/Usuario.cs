using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Usuario dl gimnasio. Hereda de la clase persona.
    /// </summary>
    public class Usuario: Persona
    {
        private double _peso;
        private double _estatura;

        public Usuario(int id, string nombreUsuario, string nombre, int edad,double peso, double estatura):base(id,nombreUsuario,nombre, edad)
        {
            Peso = peso;
            Estatura = estatura;
        }

        public double Peso
        {
            get { return _peso; }
            set
            {
                if (value <= 20 || value > 150)
                    throw new ArgumentException("Digite el peso entre 20 y 150");
                _peso = value;
            }
        }

        public double Estatura
        {
            get { return _estatura; }

            set
            {
                if (value < 0.8 || value > 2.5)
                    throw new ArgumentException("La estatura debe estar entre 0.8 y 2.5 metros");
                _estatura = value;
            }
        }

        //IMC = Peso/Estatura al cuadrado

        public double IMC => Math.Round(Peso / (Estatura * Estatura), 2);
    }
}
