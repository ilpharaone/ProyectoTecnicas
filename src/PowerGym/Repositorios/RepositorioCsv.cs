using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Repositorios
{
   public abstract class RepositorioCsv<T> : IRepositorio<T>
    {
        private readonly string _rutaArchivo;   // "readonly": solo se asigna en el constructor
        private readonly string _encabezado;    // primera línea del CSV
        private List<T> _elementos;             // copia en memoria del archivo

     
        protected RepositorioCsv(string nombreArchivo, string encabezado)
        {
            _rutaArchivo = Path.Combine(RutaDatos.Carpeta(), nombreArchivo);
            _encabezado = encabezado;
        }

        private List<T> Elementos => _elementos ??= Cargar();

        
        protected abstract T ConvertirDesdeFila(string[] campos);   // texto -> objeto
        protected abstract string[] ConvertirAFila(T elemento);     // objeto -> texto
        protected abstract int ObtenerId(T elemento);
        protected abstract void AsignarId(T elemento, int id);


        //Lee el archivo

        private List<T> Cargar()
        {
            if (!File.Exists(_rutaArchivo))
                throw new FileNotFoundException("No existe el archivo de datos.", _rutaArchivo);

            List<T> lista = new List<T>();
            string[] lineas = File.ReadAllLines(_rutaArchivo);   // cada línea = un texto

            // Empieza en 1 para saltarse el encabezado 
            for (int i = 1; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i]))
                    continue;   // ignora líneas en blanco

                try
                {
                    
                    lista.Add(ConvertirDesdeFila(lineas[i].Split(',')));
                }
                catch (Exception ex) when (ex is FormatException
                                        || ex is ArgumentException
                                        || ex is IndexOutOfRangeException)
                {
                    // Dice EN QUÉ línea y archivo está el error. Muy útil al depurar.
                    throw new InvalidDataException(
                        $"Línea {i + 1} inválida en {Path.GetFileName(_rutaArchivo)}: {ex.Message}", ex);
                }
            }
            return lista;
        }

        protected virtual void Guardar()
        {
            List<string> lineas = new List<string> { _encabezado };

            foreach (T elemento in Elementos)
                // string.Join une los campos poniendo comas entre ellos.
                lineas.Add(string.Join(",", ConvertirAFila(elemento)));

            File.WriteAllLines(_rutaArchivo, lineas);
        }

        public List<T> ObtenerTodos()
        {
            // Devuelve una COPIA para que de afuera no puedan romper la lista interna.
            return new List<T>(Elementos);
        }

        public T ObtenerPorId(int id)
        {
           
            return Elementos.FirstOrDefault(e => ObtenerId(e) == id);
        }

        public void Agregar(T elemento)
        {
            if (elemento == null)
                throw new ArgumentNullException(nameof(elemento));

            AsignarId(elemento, SiguienteId());   // el repositorio asigna el id
            Elementos.Add(elemento);
            Guardar();
        }

        public void Actualizar(T elemento)
        {
            if (elemento == null)
                throw new ArgumentNullException(nameof(elemento));

            int posicion = Elementos.FindIndex(e => ObtenerId(e) == ObtenerId(elemento));
            if (posicion < 0)
                throw new KeyNotFoundException("No existe un registro con ese id.");

            Elementos[posicion] = elemento;   // reemplaza el viejo por el nuevo
            Guardar();
        }

        public void Eliminar(int id)
        {
            T existente = ObtenerPorId(id);
            if (existente == null)
                throw new KeyNotFoundException("No existe un registro con ese id.");

            Elementos.Remove(existente);
            Guardar();
        }

        private int SiguienteId()
        {
            return Elementos.Count == 0 ? 1 : Elementos.Max(e => ObtenerId(e)) + 1;
        }



    }
}
