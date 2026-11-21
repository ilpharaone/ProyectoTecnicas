using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Servicios
{
    public interface IReglaInsignia
    {
        bool EsPositiva { get; }

        // ¿Este usuario cumple la condición de la regla?
        bool Cumple(Datos datos);

        // ¿Esta insignia ya guardada pertenece a esta regla?
        bool Corresponde(Insignia insignia);

        // Crea la insignia (Id 0: el repositorio asigna el real al guardar).
        Insignia Crear(int idUsuario, DateTime fecha);
    }
}
