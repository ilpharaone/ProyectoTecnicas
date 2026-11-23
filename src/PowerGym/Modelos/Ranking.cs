using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class Ranking
    {
        public int Posicion { get; }
        public int IdUsuario { get; }
        public string Nombre { get; }
        public int Visitas { get; }

        public Ranking(int posicion, int idUsuario, string nombre, int visitas)
        {
            Posicion = posicion;
            IdUsuario = idUsuario;
            Nombre = nombre;
            Visitas = visitas;
        }
    }
}
