using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class Registro
    {
        private readonly IRepositorio<Usuario> _usuarios;
        private readonly IRepositorio<Entrenador> _entrenadores;

        public Registro(IRepositorio<Usuario> usuarios,
                                IRepositorio<Entrenador> entrenadores)
        {
            _usuarios = usuarios;
            _entrenadores = entrenadores;
        }

        public bool NombreUsuarioExiste(string nombreUsuario)
        {
            
            return _usuarios.ObtenerTodos().Any(u => Coincide(u.NombreUsuario, nombreUsuario))
                || _entrenadores.ObtenerTodos().Any(e => Coincide(e.NombreUsuario, nombreUsuario));
        }

        private static bool Coincide(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public string SugerirNombreUsuario(string nombreCompleto)
        {
            // Parte el texto por espacios y descarta los vacíos.
            string[] partes = (nombreCompleto ?? "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Hace falta al menos nombre y apellido.
            if (partes.Length < 2)
                return "";

            string nombre = Normalizar(partes[0]);
            string apellido = Normalizar(partes[partes.Length - 1]);   // última palabra
            if (nombre.Length == 0 || apellido.Length == 0)
                return "";

            string baseUsuario = apellido + nombre[0];   // nombre[0] = primera letra
            string candidato = baseUsuario;
            int contador = 2;

            // Mientras el candidato esté ocupado, prueba con el siguiente número.
            while (NombreUsuarioExiste(candidato))
            {
                candidato = baseUsuario + contador;
                contador++;
            }
            return candidato;
        }
            private static string Normalizar(string texto)
            {
                // FormD separa cada letra de su tilde: "é" -> "e" + "´".
                string descompuesto = texto.ToLowerInvariant().Normalize(NormalizationForm.FormD);
                var resultado = new StringBuilder();

                foreach (char c in descompuesto)
                {
                    // Descarta las tildes (marcas) y todo lo que no sea letra.
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark
                        && char.IsLetter(c))
                        resultado.Append(c);
                }
                return resultado.ToString();
            }

            public Usuario Registrar(string nombre, string nombreUsuario,
                                 int edad, double peso, double estatura, int idEntrenador)
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                    throw new ArgumentException("El nombre de usuario es obligatorio.");

                // La coma separa columnas en el CSV: un nombre con coma dañaría el archivo.
                if ((nombre ?? "").Contains(',') || nombreUsuario.Contains(','))
                    throw new ArgumentException("No se permiten comas en el nombre ni en el usuario.");
            
                if (idEntrenador == null)
                throw new ArgumentException("Seleccione un entrenador.");

                if (_entrenadores.ObtenerPorId(idEntrenador.Value) == null)
                throw new ArgumentException("El entrenador seleccionado no existe.");

                string usuarioFinal = nombreUsuario.Trim().ToLowerInvariant();

                if (NombreUsuarioExiste(usuarioFinal))
                    throw new ArgumentException("Ese nombre de usuario ya existe, elija otro.");
                Usuario nuevo = new Usuario(0, usuarioFinal, nombre, edad, peso, estatura);
                nuevo.IdEntrenador = idEntrenador;
                _usuarios.Agregar(nuevo);
                return nuevo;
            }
    }
}
