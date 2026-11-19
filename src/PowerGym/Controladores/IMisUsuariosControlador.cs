using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Controladores
{
    public interface IMisUsuariosControlador
    {
        List<Usuario> ObtenerUsuarios(Entrenador entrenador);
    }
}
