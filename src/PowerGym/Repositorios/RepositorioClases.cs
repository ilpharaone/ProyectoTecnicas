using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    public class RepositorioClases : RepositorioCsv<ClasesGimnasio>
    {


        public RepositorioClases()
                : base("clases.csv", "Id,Tipo,Nombre,IdEntrenador,Horario,CupoMaximo,InscritosActuales")
        {
        }

        protected override ClasesGimnasio ConvertirDesdeFila(string[] campos)
        {
            int id = int.Parse(campos[0]);
            string nombre = campos[2];
            int idEntrenador = int.Parse(campos[3]);
            DateTime horario = DateTime.Parse(campos[4], CultureInfo.InvariantCulture);
            int cupo = int.Parse(campos[5]);

            // "switch" elige qué clase hija crear según el texto de Tipo.
            ClasesGimnasio clase;
            switch (campos[1])
            {
                case "Yoga":
                    clase = new Yoga(id, idEntrenador, nombre, horario, cupo);
                    break;
                case "Crossfit":
                    clase = new Crossfit(id, idEntrenador, nombre, horario, cupo);
                    break;
                case "Zumba":
                    clase = new Zumba(id, idEntrenador, nombre, horario, cupo);
                    break;
                case "Pilates":
                    clase = new Pilates(id, idEntrenador, nombre, horario, cupo);
                    break;
                case "GimnasioLibre":
                    clase = new GimnasioLibre(id, idEntrenador, nombre, horario, cupo);
                    break;
                default:
                    // Tipo desconocido: el repositorio base lo reporta con el número de línea.
                    throw new ArgumentException($"Tipo de clase desconocido: {campos[1]}");
            }

            // Carga los inscritos (ya validados contra el cupo).
            clase.CantidadInscritos(int.Parse(campos[6]));
            return clase;
        }

        protected override string[] ConvertirAFila(ClasesGimnasio c)
        {
            return new[]
            {
                c.Id.ToString(),
                ObtenerTipoTexto(c),
                c.Nombre,
                c.IdEntrenador.ToString(),
                c.Horario.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                c.CupoMaximo.ToString(),
                c.InscritosActuales.ToString()
            };
        }

        private static string ObtenerTipoTexto(ClasesGimnasio c)
        {
            if (c is Yoga) return "Yoga";
            if (c is Crossfit) return "Crossfit";
            if (c is Zumba) return "Zumba";
            if (c is Pilates) return "Pilates";
            if (c is GimnasioLibre) return "GimnasioAbierto";
            throw new InvalidOperationException("Tipo de clase no soportado.");
        }

        protected override int ObtenerId(ClasesGimnasio c) => c.Id;

        protected override void AsignarId(ClasesGimnasio c, int id) => c.Id = id;

    }
}
