using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;


namespace PowerGym.Repositorios
{
    public class RepositorioEntrenadores : RepositorioCsv<Entrenador>
    {
        public RepositorioEntrenadores()
            : base("entrenadores.csv", "Id,NombreUsuario,Nombre,Edad,Especialidad,Tipo")
        {
        }

        protected override Entrenador ConvertirDesdeFila(string[] campos)
        {
            return new Entrenador(
                int.Parse(campos[0]),
                campos[1],
                campos[2],
                int.Parse(campos[3]),
                campos[4],
                // Enum.Parse convierte el texto "Gimnasio" o "Personal" en el valor del enum.
                // Si el texto no existe en el enum, lanza ArgumentException,
                
                (TipoEntrenador)Enum.Parse(typeof(TipoEntrenador), campos[5]));
        }

        protected override string[] ConvertirAFila(Entrenador e)
        {
            return new[]
            {
                e.Id.ToString(),
                e.NombreUsuario,
                e.Nombre,
                e.Edad.ToString(),
                e.Especialidad,
                e.Tipo.ToString()   // el enum se guarda como texto: "Gimnasio" o "Personal"
            };
        }

        protected override int ObtenerId(Entrenador e) => e.Id;

        protected override void AsignarId(Entrenador e, int id) => e.Id = id;

    }
}
