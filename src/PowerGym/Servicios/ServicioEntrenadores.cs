using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioEntrenadores
    {
        private readonly IRepositorio<Usuario> _usuarios;

        public ServicioEntrenadores(IRepositorio<Usuario> usuarios)
        {
            _usuarios = usuarios;
        }

        
        public List<Usuario> ObtenerUsuariosDe(int idEntrenador)
        {
            return _usuarios.ObtenerTodos()
                            .Where(u => u.IdEntrenador == idEntrenador)
                            .ToList();
        }
    }
}
