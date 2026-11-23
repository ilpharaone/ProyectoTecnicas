using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class Notificacion
    {
        private string _mensaje;

        public int Id { get; set; }

        public int IdDestinatario { get; set; }

        
        public DateTime Fecha { get; set; }

        
        public TipoNotificacion Tipo { get; set; }

       
        public bool Leida { get; set; }

        
        public string Mensaje
        {
            get { return _mensaje; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El mensaje de la notificación no puede estar en blanco.");
                _mensaje = value;
            }
        }

        
        public Notificacion(int id, int idDestinatario, DateTime fecha,
                            TipoNotificacion tipo, string mensaje)
        {
            Id = id;
            IdDestinatario = idDestinatario;
            Fecha = fecha;
            Tipo = tipo;
            Mensaje = mensaje;   
            Leida = false;
        }
        public bool EsEntrenador { get; set; }
    }
}

