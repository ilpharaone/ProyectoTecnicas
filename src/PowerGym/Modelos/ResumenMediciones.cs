using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class ResumenMediciones
    {
        public DateTime FechaInicial { get; }
        public DateTime FechaFinal { get; }
        public double CambioPeso { get; }
        public double CambioGrasa { get; }          // negativo = bajó (es una mejora)
        public double CambioMasaMuscular { get; }   // positivo = ganó músculo


        public ResumenMediciones(Medicion primera, Medicion ultima)
        {
            FechaInicial = primera.Fecha;
            FechaFinal = ultima.Fecha;
            CambioPeso = ultima.Peso - primera.Peso;
            CambioGrasa = ultima.PorcentajeGrasa - primera.PorcentajeGrasa;
            CambioMasaMuscular = ultima.MasaMuscular - primera.MasaMuscular;
        }
    }
}
