using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Controladores
{
    public interface IClasesControlador
    {
        List<ClasesGimnasio> ObtenerClases();
        string ObtenerNombreEntrenador(int idEntrenador);
        bool EstaInscrito(Usuario usuario, ClasesGimnasio clase);
        void Inscribir(Usuario usuario, ClasesGimnasio clase);
    }
}
