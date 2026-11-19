using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Servicios;

namespace PowerGym.Controladores
{
    public class RegistroControlador: IRegistroControlador
    {
        private readonly Registro _servicio;

        public RegistroControlador(Registro servicio)
        {
            _servicio = servicio;
        }
        public List<Entrenador> ObtenerEntrenadores()
        {
            return _servicio.ObtenerEntrenadores();
        }
        public string SugerirNombreUsuario(string nombreCompleto)
        {
            return _servicio.SugerirNombreUsuario(nombreCompleto);
        }

        public Usuario Registrar(string nombre, string nombreUsuario,
                                 string edad, string peso, string estatura, int? idEntrenador)
        {
            // TryParse intenta convertir. Devuelve false (sin romper) si no es un número.
            if (!int.TryParse(edad, out int edadNumero))
                throw new ArgumentException("La edad debe ser un número entero.");

            if (!TryParseDecimal(peso, out double pesoNumero))
                throw new ArgumentException("El peso debe ser un número (ej. 70 o 70.5).");

            if (!TryParseDecimal(estatura, out double estaturaNumero))
                throw new ArgumentException("La estatura debe ser un número en metros (ej. 1.75).");

            // Los rangos (edad 10-100, peso 1-400...) los valida el modelo.
            return _servicio.Registrar(nombre, nombreUsuario,
                                       edadNumero, pesoNumero, estaturaNumero,idEntrenador);
        }

        private static bool TryParseDecimal(string texto, out double valor)
        {
            // Replace cambia la coma por punto antes de convertir.
            // InvariantCulture: el punto siempre es el separador decimal.
            return double.TryParse((texto ?? "").Trim().Replace(',', '.'),
                                   NumberStyles.Float,
                                   CultureInfo.InvariantCulture,
                                   out valor);
        }
    }
}
