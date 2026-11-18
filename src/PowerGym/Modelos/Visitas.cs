using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{ 
    /// <summary>
    /// Registro de las visitas de los usuarios al gimnasio
    /// </summary>
    public class Visitas
    {
        public int Id { get; set; }

        // Id del usuario que asistió.
        public int IdUsuario { get; set; }

          // Fecha (y hora) de la visita.
        public DateTime Fecha { get; set; }

        //? significa que puede ser null
        public int? IdClase { get; set; }

        public Visitas(int id, int idUsuario, DateTime fecha, int? idClase = null)
        {
            Id = id;
            IdUsuario = idUsuario;
            Fecha = fecha;
            IdClase = idClase;
        }

    }


}
