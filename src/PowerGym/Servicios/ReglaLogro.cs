using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public abstract class ReglaLogro : IReglaInsignia
    {
        private readonly TipoLogro _tipo;
        private readonly string _nombre;
        private readonly string _descripcion;

        protected ReglaLogro(TipoLogro tipo, string nombre, string descripcion)
        {
            _tipo = tipo;
            _nombre = nombre;
            _descripcion = descripcion;
        }

        public bool EsPositiva => true;

        // Cada regla concreta define SU condición.
        public abstract bool Cumple(Datos datos);

        // "is InsigniaLogro logro" pregunta el tipo y, si coincide, lo deja en la variable.
        public bool Corresponde(Insignia insignia)
        {
            return insignia is InsigniaLogro logro && logro.Tipo == _tipo;
        }

        public Insignia Crear(int idUsuario, DateTime fecha)
        {
            return new InsigniaLogro(0, idUsuario, _nombre, _descripcion, fecha, _tipo);
        }
    }
}
