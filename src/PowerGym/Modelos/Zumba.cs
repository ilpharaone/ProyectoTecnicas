using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Modelos
{
    public class Zumba:ClasesGimnasio
    {
        public Zumba(int id, int idEntrenador, string nombre,
                                  DateTime horario, int cupoMaximo)
                    : base(id, idEntrenador, nombre, horario, cupoMaximo)
        {
        }

        public override int DuracionMinutos() => 50;
        public override bool RequiereInscripcion() => true;

    }
}
