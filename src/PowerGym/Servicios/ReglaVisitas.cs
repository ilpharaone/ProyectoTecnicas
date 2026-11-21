using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ReglaVisitas : ReglaLogro
    {
        public ReglaVisitas()
                    : base(TipoLogro.Visitas, "Rey de las visitas", "20 o mas visitas al gimnasio")
        {
        }

        public override bool Cumple(Datos datos)
        {
            return datos.TotalVisitas >= Datos.MetaVisitas;
        }
    }
}
