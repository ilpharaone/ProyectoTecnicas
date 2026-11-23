using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    public class RepositorioNotificaciones : RepositorioCsv<Notificacion>
    {
        public RepositorioNotificaciones()
            : base("notificaciones.csv", "Id,IdDestinatario,EsEntrenador,Fecha,Tipo,Leida,Mensaje")
        {
        }
        protected override Notificacion ConvertirDesdeFila(string[] campos)
        {
            var notificacion = new Notificacion(
                int.Parse(campos[0]),
                int.Parse(campos[1]),
                DateTime.Parse(campos[3], CultureInfo.InvariantCulture),
                (TipoNotificacion)Enum.Parse(typeof(TipoNotificacion), campos[4]),
                campos[6]);

            // Estos dos no van en el constructor: se asignan después.
            notificacion.EsEntrenador = bool.Parse(campos[2]);
            notificacion.Leida = bool.Parse(campos[5]);
            return notificacion;
        }

        protected override string[] ConvertirAFila(Notificacion n)
        {
            return new[]
            {
                n.Id.ToString(),
                n.IdDestinatario.ToString(),
                // ToString() de un bool da "True"/"False"; bool.Parse lo lee igual.
                n.EsEntrenador.ToString().ToLowerInvariant(),
                n.Fecha.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                n.Tipo.ToString(),
                n.Leida.ToString().ToLowerInvariant(),
                n.Mensaje
            };

        }
        protected override int ObtenerId(Notificacion n) => n.Id;

        protected override void AsignarId(Notificacion n, int id) => n.Id = id;
    }
}
