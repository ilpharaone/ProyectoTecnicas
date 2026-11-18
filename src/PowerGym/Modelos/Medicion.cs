using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Medidicion de los usuarios en una fecha especifica
    /// </summary>
    public class Medicion
    {
        private double _peso;
        private double _porcentajeGrasa;
        private double _masaMuscular;

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public DateTime Fecha { get; set; }

        public double Peso
        {
            get { return _peso; }
            set
            {
                if (value <= 40 || value > 150)
                    throw new ArgumentException("El peso debe estar entre 40 y 150.");
                _peso = value;
            }
        }

        // Porcentaje de grasa corporal (de 0 a 100).
        public double PorcentajeGrasa
        {
            get { return _porcentajeGrasa; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("El porcentaje de grasa debe estar entre 0 y 100.");
                _porcentajeGrasa = value;
            }
        }

        // Masa muscular en kg.
        public double MasaMuscular
        {
            get { return _masaMuscular; }
            set
            {
                if (value < 0 || value > 200)
                    throw new ArgumentException("La masa muscular debe estar entre 0 y 200 kg.");
                _masaMuscular = value;
            }
        }

        public Medicion(int id, int idUsuario, DateTime fecha, double peso,
                        double porcentajeGrasa, double masaMuscular)
        {
            Id = id;
            IdUsuario = idUsuario;
            Fecha = fecha;
            Peso = peso;
            PorcentajeGrasa = porcentajeGrasa;
            MasaMuscular = masaMuscular;
        }
    }
}
