using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Un ejercicio forma parte de una rutina(clase de "muchos")
    /// </summary>
    public class Ejercicio
    {
        private string _nombre;
        private int _series;
        private int _repeticiones;
        private double _peso;

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del ejercicio no puede estar vacio.");
                _nombre = value;
            }
        }
        public int Serie
        {
            get { return _series; }
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentException("Las series deben estar entre 1 y 10.");
                _series = value;

            }
        }
        public int Repeticiones
        {
            get { return _repeticiones; }
            set
            {
                if (value < 1 || value > 30)
                    throw new ArgumentException("Las repeticiones deben estar entre 1 y 30.");
                _repeticiones = value;
            }
        }

        //Peso que se usa al hacer el ejercicio, si se hace solo con el cuerpo poner 0
        public double Peso
        {
            get { return _peso; }

            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("El peso debe estar entre 0 y 500 kg.");
                _peso = value;
            }
        }

        public Ejercicio(string nombre, int serie, int repeticiones, double peso)
        {
            Nombre = nombre;
            Serie = serie;
            Repeticiones = repeticiones;
            Peso = peso;
        }


    }
}
