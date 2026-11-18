using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class InsigniaVerguenza:Insignia
    {
        public TipoVerguenza Tipo { get; set; }

        public InsigniaVerguenza(int id, int idUsuario, string nombre,
                                 string descripcion, DateTime fechaObtenida,
                                 TipoVerguenza tipo)
            : base(id, idUsuario, nombre, descripcion, fechaObtenida)
        {
            Tipo = tipo;
        }

        
        public override bool EsPositiva()
        {
            return false;
        }
    }
}
