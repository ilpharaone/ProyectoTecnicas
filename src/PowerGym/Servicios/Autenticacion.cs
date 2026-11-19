using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class Autenticacion
    {
        private readonly IRepositorio<Usuario> _usuarios;
        private readonly IRepositorio<Entrenador> _entrenadores;

        public Autenticacion(IRepositorio<Usuario> usuarios,
                                         IRepositorio<Entrenador> entrenadores)
        {
            _usuarios = usuarios;
            _entrenadores = entrenadores;
        }

        public Persona Autenticar(string nombreUsuario)
        {
            // Trim() quita espacios de los extremos
            string buscado = nombreUsuario.Trim();

            // FirstOrDefault devuelve el primero que cumpla la condición, o null.
            // "u => ..." significa: "para cada u, compara su NombreUsuario".
            // OrdinalIgnoreCase no distingue mayusculas
            Persona encontrada = _usuarios.ObtenerTodos().FirstOrDefault(
                u => string.Equals(u.NombreUsuario, buscado, StringComparison.OrdinalIgnoreCase));

            if (encontrada != null)
                return encontrada;

            
            return _entrenadores.ObtenerTodos().FirstOrDefault(
                e => string.Equals(e.NombreUsuario, buscado, StringComparison.OrdinalIgnoreCase));
        }

    }
}
