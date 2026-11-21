using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ReglaMusculatura : ReglaLogro
    {
        public ReglaMusculatura()
            : base(TipoLogro.Musculo, "Musculo en alza", "Gano 1 kg o mas de masa muscular")
        {
        }

        public override bool Cumple(Datos datos)
        {
            return datos.CambioMasaMuscular >= Datos.MetaMusculoKg;
        }
    }
}
