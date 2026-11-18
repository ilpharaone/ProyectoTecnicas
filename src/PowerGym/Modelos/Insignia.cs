using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    /// <summary>
    /// Clase base de todas las insignias, clase padre.
    /// </summary>
    public abstract class Insignia
    {
        private string _nombre;
        private string _descripcion;

        public int Id { get; set; }

        
        public int IdUsuario { get; set; }

        // Fecha en que se otorgó.
        public DateTime FechaObtenida { get; set; }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la insignia no puede ser en blanco.");
                _nombre = value;
            }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La descripción de la insignia no puede estar en blanco.");
                _descripcion = value;
            }
        }

        protected Insignia(int id, int idUsuario, string nombre,
                          string descripcion, DateTime fechaObtenida)
        {
            Id = id;
            IdUsuario = idUsuario;
            Nombre = nombre;               
            Descripcion = descripcion;     
            FechaObtenida = fechaObtenida;
        }

        //Método abstracto: la base NO tiene cuerpo, solo lo declara.
        // Devuelve true si la insignia es un premio, false si es de vergüenza.
        public abstract bool EsPositiva();
    }
}
