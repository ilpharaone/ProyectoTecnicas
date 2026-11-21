using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ReglaPocasVisitas:ReglaVerguenza
    
    {
        public ReglaPocasVisitas()
            : base(TipoVerguenza.PocasVisitas, "Sillon fiel",
                   "Esta entre los 10 usuarios con menos visitas")
        {
        }

        // El top 10 lo calcula el servicio (hace falta comparar contra todos los usuarios).
        public override bool Cumple(Datos datos)
        {
            return datos.EnTopMenosVisitas;
        }

    }
}
