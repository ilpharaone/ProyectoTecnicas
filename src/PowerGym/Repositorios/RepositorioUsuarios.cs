using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using PowerGym.Modelos;

namespace PowerGym.Repositorios
{
    class RepositorioUsuarios : RepositorioCsv<Usuario>
    {
        public RepositorioUsuarios():base("usuarios.csv","Id,NombreUsuario,Nombre,Edad,Peso,Estatura,IdEntrenador")
        {

        }

        protected override Usuario ConvertirDesdeFila(string[] campos)
        {
            Usuario usuario = new Usuario(
                int.Parse(campos[0]),
                campos[1],
                campos[2],
                int.Parse(campos[3]),
                // InvariantCulture = el decimal SIEMPRE es punto (1.75), no coma.
                double.Parse(campos[4], CultureInfo.InvariantCulture),
                double.Parse(campos[5], CultureInfo.InvariantCulture));
            if (!string.IsNullOrWhiteSpace(campos[6]))
                usuario.IdEntrenador = int.Parse(campos[6]);

            return usuario;
        }

        protected override string[] ConvertirAFila(Usuario u)
        {
            return new[]
            {
                u.Id.ToString(),
                u.NombreUsuario,
                u.Nombre,
                u.Edad.ToString(),
                u.Peso.ToString(CultureInfo.InvariantCulture),
                u.Estatura.ToString(CultureInfo.InvariantCulture),
                u.IdEntrenador?.ToString() ?? ""
            };
        }
        protected override int ObtenerId(Usuario u) => u.Id;

        protected override void AsignarId(Usuario u, int id) => u.Id = id;
    }
}
