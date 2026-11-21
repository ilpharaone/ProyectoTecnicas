using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class Datos
    {
        public const int MetaVisitas = 20;
        public const double MetaMusculoKg = 1.0;
        public const double MetaGrasaPuntos = 2.0;

        public int IdUsuario { get; }
        public int TotalVisitas { get; }
        public bool EnTopMenosVisitas { get; }

        // Mediciones del usuario, de la más antigua a la más reciente.
        public List<Medicion> Mediciones { get; }

        public Datos(int idUsuario, int totalVisitas,
                            List<Medicion> mediciones, bool enTopMenosVisitas)
        {
            IdUsuario = idUsuario;
            TotalVisitas = totalVisitas;
            Mediciones = mediciones;
            EnTopMenosVisitas = enTopMenosVisitas;
        }
        public double CambioMasaMuscular => Mediciones.Count < 2 ? 0
                    : Mediciones[Mediciones.Count - 1].MasaMuscular - Mediciones[0].MasaMuscular;

        // Puntos de grasa perdidos (positivo = bajó la grasa).
        public double CambioGrasa => Mediciones.Count < 2 ? 0
            : Mediciones[0].PorcentajeGrasa - Mediciones[Mediciones.Count - 1].PorcentajeGrasa;

    }
}
