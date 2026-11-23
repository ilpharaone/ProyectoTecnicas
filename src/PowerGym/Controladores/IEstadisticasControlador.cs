using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;


namespace PowerGym.Controladores
{
    public interface IEstadisticasControlador
    {
        List<AumentoEjercicio> ObtenerAumentos(Usuario usuario, DateTime desde, DateTime hasta);
        ResumenMediciones ObtenerResumenMediciones(Usuario usuario, DateTime desde, DateTime hasta);
        List<Ranking> ObtenerTopVisitantes(DateTime desde, DateTime hasta);
        List<Ranking> ObtenerTopAusentes(DateTime desde, DateTime hasta);
    }
}
