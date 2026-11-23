using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class AumentoEjercicio
    {
        public string Ejercicio { get; }
        public double PesoInicial { get; }
        public double PesoFinal { get; }

        // Propiedad calculada (sin set): se recalcula cada vez que se lee.
        public double Aumento => PesoFinal - PesoInicial;

        public AumentoEjercicio(string ejercicio, double pesoInicial, double pesoFinal)
        {
            Ejercicio = ejercicio;
            PesoInicial = pesoInicial;
            PesoFinal = pesoFinal;
        }
    }
}
