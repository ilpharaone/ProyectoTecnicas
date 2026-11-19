using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Controladores
{
    public interface IRegistroControlador
    {
        string SugerirNombreUsuario(string nombreCompleto);

        List<Entrenador> ObtenerEntrenador();

        
        Usuario Registrar(string nombre, string nombreUsuario,
                          string edad, string peso, string estatura,int? idEntrenador);

    }
}
