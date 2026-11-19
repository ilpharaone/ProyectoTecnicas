using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    public class RepositorioMediciones : RepositorioCsv<Medicion>
    {
        public RepositorioMediciones(): base ("mediciones.csv", "Id,IdUsuario,Fecha,Peso,PorcentajeGrasa,MasaMuscular")
        {

        }

        protected override Medicion ConvertirDesdeFila(string[] campos)
        {
            return new Medicion(
                int.Parse(campos[0]),
                int.Parse(campos[1]),
                DateTime.Parse(campos[2], CultureInfo.InvariantCulture),
                double.Parse(campos[3], CultureInfo.InvariantCulture),
                double.Parse(campos[4], CultureInfo.InvariantCulture),
                double.Parse(campos[5], CultureInfo.InvariantCulture));
        }

        protected override string[] ConvertirAFila(Medicion m)
        {
            return new[]
            {
                m.Id.ToString(),
                m.IdUsuario.ToString(),
                m.Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                m.Peso.ToString(CultureInfo.InvariantCulture),
                m.PorcentajeGrasa.ToString(CultureInfo.InvariantCulture),
                m.MasaMuscular.ToString(CultureInfo.InvariantCulture)
            };
        }

        protected override int ObtenerId(Medicion m) => m.Id;

        protected override void AsignarId(Medicion m, int id) => m.Id = id;

    }
}
