using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    class RepositorioInsignias : RepositorioCsv<Insignia>
    {
        public RepositorioInsignias()
            : base("insignias.csv", "Id,IdUsuario,Categoria,Tipo,Nombre,Descripcion,FechaObtenida")
        {
        }

        // Orden: 0=Id 1=IdUsuario 2=Categoria 3=Tipo 4=Nombre 5=Descripcion 6=Fecha
        protected override Insignia ConvertirDesdeFila(string[] campos)
        {
            int id = int.Parse(campos[0]);
            int idUsuario = int.Parse(campos[1]);
            DateTime fecha = DateTime.Parse(campos[6], CultureInfo.InvariantCulture);

            // La columna Categoria decide qué clase hija crear (polimorfismo).
            if (campos[2] == "Logro")
                return new InsigniaLogro(id, idUsuario, campos[4], campos[5], fecha,
                                         (TipoLogro)Enum.Parse(typeof(TipoLogro), campos[3]));

            if (campos[2] == "Verguenza")
                return new InsigniaVerguenza(id, idUsuario, campos[4], campos[5], fecha,
                                             (TipoVerguenza)Enum.Parse(typeof(TipoVerguenza), campos[3]));

            throw new ArgumentException($"Categoría de insignia desconocida: {campos[2]}");
        }

        protected override string[] ConvertirAFila(Insignia i)
        {
            string categoria;
            string tipo;

            if (i is InsigniaLogro logro)
            {
                categoria = "Logro";
                tipo = logro.Tipo.ToString();
            }
            else if (i is InsigniaVerguenza verguenza)
            {
                categoria = "Verguenza";
                tipo = verguenza.Tipo.ToString();
            }
            else
            {
                throw new InvalidOperationException("Tipo de insignia no soportado.");
            }

            return new[]
            {
                i.Id.ToString(), i.IdUsuario.ToString(), categoria, tipo,
                i.Nombre, i.Descripcion,
                i.FechaObtenida.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
        }

        protected override int ObtenerId(Insignia i) => i.Id;

        protected override void AsignarId(Insignia i, int id) => i.Id = id;
    }
}
