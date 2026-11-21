using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public abstract class ReglaVerguenza : IReglaInsignia
    {
        private readonly TipoVerguenza _tipo;
        private readonly string _nombre;
        private readonly string _descripcion;

        protected ReglaVerguenza(TipoVerguenza tipo, string nombre, string descripcion)
        {
            _tipo = tipo;
            _nombre = nombre;
            _descripcion = descripcion;
        }

        public bool EsPositiva => false;

        public abstract bool Cumple(Datos datos);

        public bool Corresponde(Insignia insignia)
        {
            return insignia is InsigniaVerguenza verguenza && verguenza.Tipo == _tipo;
        }

        public Insignia Crear(int idUsuario, DateTime fecha)
        {
            return new InsigniaVerguenza(0, idUsuario, _nombre, _descripcion, fecha, _tipo);
        }
    }
}
