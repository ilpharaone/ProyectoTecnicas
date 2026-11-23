using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Controladores
{
    public interface IInformacionControlador
    {
        List<Visitas> ObtenerVisitas(Usuario usuario);
        List<Medicion> ObtenerMediciones(Usuario usuario);
        List<Rutina> ObtenerUltimasRutinas(Usuario usuario);
    }
}
