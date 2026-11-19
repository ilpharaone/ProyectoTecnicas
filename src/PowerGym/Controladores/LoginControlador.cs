using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    class LoginControlador : ILoginControlador
    {
        private readonly Autenticacion _servicio;

        public LoginControlador(Autenticacion servicio)
        {
            _servicio = servicio;
        }

        public Persona IniciarSesion(string nombreUsuario)
        {
            // Primera validación: no puede venir vacío.
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Escriba su nombre de usuario.");

            Persona persona = _servicio.Autenticar(nombreUsuario);

            // Segunda validación: tiene que existir.
            if (persona == null)
                throw new ArgumentException("El usuario no existe.");

            return persona;
        }
    }
}
