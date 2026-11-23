using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Controladores
{
    public class ContenedorControladores
    {
        public IMisUsuariosControlador MisUsuarios { get; set; }
        public IRutinasControlador Rutinas { get; set; }
        public IAumentosControlador Aumentos { get; set; }
        public IClasesControlador Clases { get; set; }
        public IInsigniasControlador Insignias { get; set; }
        public INotificacionesControlador Notificaciones { get; set; }
        public IInformacionControlador Informacion { get; set; }

        public IEstadisticasControlador Estadisticas { get; set; }
    }
}
