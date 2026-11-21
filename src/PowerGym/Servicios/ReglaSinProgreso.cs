using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
   
        public class ReglaSinProgreso : ReglaVerguenza
    {
        public ReglaSinProgreso()
            : base(TipoVerguenza.SinProgreso, "Sin rastro", "No ha registrado ninguna medicion")
        {
        }

        public override bool Cumple(Datos datos)
        {
            return datos.Mediciones.Count == 0;
        }
    }
}
