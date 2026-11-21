using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ReglaGrasa :ReglaLogro
    {
        public ReglaGrasa()
                    : base(TipoLogro.Grasa, "Quemagrasa", "Bajo 2 puntos o mas de grasa corporal")
        {
        }

        public override bool Cumple(Datos datos)
        {
            return datos.CambioGrasa >= Datos.MetaGrasaPuntos;
        }
    }
}
