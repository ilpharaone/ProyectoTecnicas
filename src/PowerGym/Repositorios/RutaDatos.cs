using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Repositorios
{

 

    
    
        /// <summary>
        /// Encuentra la carpeta "data" del repositorio, sin importar
        /// desde dónde se ejecute el programa.
        /// </summary>
        public static class RutaDatos
        {
            
            public static string Carpeta()
            {
                // BaseDirectory es la carpeta donde corre el .exe (bin\Debug\...).
                DirectoryInfo carpetaActual = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                // Sube hacia la carpeta padre mientras exista.
                while (carpetaActual != null)
                {
                    // Path.Combine une rutas con el separador correcto
                    string candidata = Path.Combine(carpetaActual.FullName, "data");

                    if (Directory.Exists(candidata))
                        return candidata;

                    carpetaActual = carpetaActual.Parent;   // sube un nivel
                }

                throw new DirectoryNotFoundException("No se encontró la carpeta 'data'.");
            }
        }
    }


