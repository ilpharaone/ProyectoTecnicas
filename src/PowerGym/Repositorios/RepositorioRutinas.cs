using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using System.Globalization;
using System.IO;


namespace PowerGym.Repositorios
{
    public class RepositorioRutinas :RepositorioCsv<Rutina>
    {
        private const string ArchivoEjercicios = "ejercicios.csv";
        private const string EncabezadoEjercicios = "IdRutina,Nombre,Series,Repeticiones,PesoKg";

        // Diccionario: clave = id de la rutina, valor = lista de sus ejercicios.
        private Dictionary<int, List<Ejercicio>> _ejerciciosPorRutina;

        // Carga perezosa, igual que en la base: se lee el archivo la primera vez que se pide.
        private Dictionary<int, List<Ejercicio>> EjerciciosPorRutina
            => _ejerciciosPorRutina ??= CargarEjercicios();

        public RepositorioRutinas()
            : base("rutinas.csv", "Id,IdUsuario,IdEntrenador,Nombre,FechaInicio,FechaFin,Version")
        {
        }

        private static Dictionary<int, List<Ejercicio>> CargarEjercicios()
        {
            string ruta = Path.Combine(RutaDatos.Carpeta(), ArchivoEjercicios);
            if (!File.Exists(ruta))
                throw new FileNotFoundException("No existe el archivo de datos.", ruta);

            var resultado = new Dictionary<int, List<Ejercicio>>();
            string[] lineas = File.ReadAllLines(ruta);

            for (int i = 1; i < lineas.Length; i++)   // 1 = salta el encabezado
            {
                if (string.IsNullOrWhiteSpace(lineas[i]))
                    continue;

                try
                {
                    string[] c = lineas[i].Split(',');   // 0=IdRutina 1=Nombre 2=Series 3=Reps 4=PesoKg
                    int idRutina = int.Parse(c[0]);

                    var ejercicio = new Ejercicio(
                        c[1],
                        int.Parse(c[2]),
                        int.Parse(c[3]),
                        double.Parse(c[4], CultureInfo.InvariantCulture));

                    // Si esta rutina aún no tiene lista, se crea; luego se agrega el ejercicio.
                    if (!resultado.ContainsKey(idRutina))
                        resultado[idRutina] = new List<Ejercicio>();
                    resultado[idRutina].Add(ejercicio);
                }
                catch (Exception ex) when (ex is FormatException
                                        || ex is ArgumentException
                                        || ex is IndexOutOfRangeException)
                {
                    throw new InvalidDataException(
                        $"Línea {i + 1} inválida en {ArchivoEjercicios}: {ex.Message}", ex);
                }
            }
            return resultado;
        }

        protected override Rutina ConvertirDesdeFila(string[] campos)
        {
            int id = int.Parse(campos[0]);

            var rutina = new Rutina(
                id,
                int.Parse(campos[1]),
                int.Parse(campos[2]),
                campos[3],
                DateTime.Parse(campos[4], CultureInfo.InvariantCulture),
                DateTime.Parse(campos[5], CultureInfo.InvariantCulture));

            // El constructor pone la versión en 1; aquí se pone la que trae el archivo.
            rutina.Version = int.Parse(campos[6]);

            // TryGetValue busca el id en el diccionario: si lo encuentra, devuelve true
            // y deja la lista en "ejercicios". Si no, la rutina queda sin ejercicios.
            if (EjerciciosPorRutina.TryGetValue(id, out List<Ejercicio> ejercicios))
            {
                foreach (Ejercicio e in ejercicios)
                    rutina.Agregar(e);
            }
            return rutina;
        }

        protected override string[] ConvertirAFila(Rutina r)
        {
            return new[]
            {
                r.Id.ToString(),
                r.IdUsuario.ToString(),
                r.IdEntrenador.ToString(),
                r.Nombre,
                r.FechaInicio.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                r.FechaFin.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                r.Version.ToString()
            };
        }

        protected override void Guardar()
        {
            base.Guardar();
            GuardarEjercicios();
        }

        private void GuardarEjercicios()
        {
            var lineas = new List<string> { EncabezadoEjercicios };

            foreach (Rutina rutina in ObtenerTodos())
            {
                foreach (Ejercicio e in rutina.Ejercicios)
                {
                    lineas.Add(string.Join(",",            
                        rutina.Id.ToString(),
                        e.Nombre,
                        e.Serie.ToString(),
                        e.Repeticiones.ToString(),
                        e.Peso.ToString(CultureInfo.InvariantCulture)
                    ));
                }
            }
            File.WriteAllLines(Path.Combine(RutaDatos.Carpeta(), ArchivoEjercicios), lineas);
        }

        protected override int ObtenerId(Rutina r) => r.Id;

        protected override void AsignarId(Rutina r, int id) => r.Id = id;

    }
}
