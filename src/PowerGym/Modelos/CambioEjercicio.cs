using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class CambioEjercicio
    {
        public string Ejercicio { get; }
        public string Rutina { get; }
        public DateTime Fecha { get; }
        public int Series { get; }
        public int Repeticiones { get; }
        public double Peso { get; }

        public int DiferenciaSeries { get; }
        public int DiferenciaRepeticiones { get; }
        public double DiferenciaPeso { get; }

        public CambioEjercicio(string ejercicio, string rutina, DateTime fecha,
                               int series, int repeticiones, double peso,
                               int difSeries, int difRepeticiones, double difPeso)
        {
            Ejercicio = ejercicio;
            Rutina = rutina;
            Fecha = fecha;
            Series = series;
            Repeticiones = repeticiones;
            Peso = peso;
            DiferenciaSeries = difSeries;
            DiferenciaRepeticiones = difRepeticiones;
            DiferenciaPeso = difPeso;
        }
    }
}
