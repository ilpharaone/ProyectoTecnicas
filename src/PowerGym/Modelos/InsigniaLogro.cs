using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class InsigniaLogro:Insignia
    {
        public TipoLogro Tipo { get; set; }


        public InsigniaLogro(int id, int idUsuario, string nombre, string descripcion, DateTime fechaObtenida, TipoLogro tipo): base(id, idUsuario, nombre, descripcion, fechaObtenida)
        {
            Tipo = tipo;
        }

        public override bool EsPositiva()
        {
            return true;
        }
    }
}
