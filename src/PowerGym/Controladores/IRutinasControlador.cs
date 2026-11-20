using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Controladores
{
    public interface IRutinasControlador
    {
        List<Usuario> ObtenerUsuarios(Entrenador entrenador);
        List<Rutina> ObtenerRutinas(int idUsuario);

        // Recibe TEXTOS y devuelve un Ejercicio ya validado por el modelo.
        Ejercicio CrearEjercicio(string nombre, string series, string repeticiones, string pesoKg);

        Rutina Crear(Entrenador entrenador, int idUsuario, string nombre,
                     DateTime inicio, DateTime fin, List<Ejercicio> ejercicios);

        void Editar(int idRutina, string nombre, DateTime inicio,
                    DateTime fin, List<Ejercicio> ejercicios);

        void Eliminar(int idRutina);

        List<string> ObtenerNombresEjercicios();
        bool EsDePesoCorporal(string nombre);
    }
}
