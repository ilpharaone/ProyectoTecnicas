using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    public class RepositorioVisitas: RepositorioCsv<Visitas>
    {
        public RepositorioVisitas()
            : base("visitas.csv", "Id,IdUsuario,Fecha,IdClase")
        {
        }

        protected override Visitas ConvertirDesdeFila(string[] campos)
        {

            int? idClase = null;
            if (!string.IsNullOrWhiteSpace(campos[3]))
                idClase = int.Parse(campos[3]);

            return new Visitas(
                int.Parse(campos[0]),
                int.Parse(campos[1]),
                // InvariantCulture: lee la fecha igual en cualquier computadora.
                DateTime.Parse(campos[2], CultureInfo.InvariantCulture),
                idClase);
        }

        protected override string[] ConvertirAFila(Visitas v)
        {
            return new[]
            {
                v.Id.ToString(),
                v.IdUsuario.ToString(),
                // "yyyy-MM-dd HH:mm:ss" es el mismo formato que tiene el CSV.
                v.Fecha.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                v.IdClase?.ToString() ?? ""   // null se guarda como vacío
            };
        }

        protected override int ObtenerId(Visitas v) => v.Id;

        protected override void AsignarId(Visitas v, int id) => v.Id = id;
    }
}
