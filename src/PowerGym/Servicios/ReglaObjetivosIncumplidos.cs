using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public class ReglaObjetivosIncumplidos : ReglaVerguenza 
    {
        public ReglaObjetivosIncumplidos()
                    : base(TipoVerguenza.ObjetivosIncumplidos, "Meta pendiente",
                           "No cumplio sus metas de musculatura ni de grasa")
        {
        }

        public override bool Cumple(Datos datos)
        {
            // Hacen falta al menos 2 mediciones para poder comparar.
            return datos.Mediciones.Count >= 2
                && datos.CambioMasaMuscular < Datos.MetaMusculoKg
                && datos.CambioGrasa < Datos.MetaGrasaPuntos;
        }
    }
}
