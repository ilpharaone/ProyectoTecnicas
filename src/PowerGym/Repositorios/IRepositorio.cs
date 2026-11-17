using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGym.Repositorios
{
    public interface IRepositorio<T>
    {
        List<T> ObtenerTodos();
        T ObtenerPorId(int id);
        void Agregar(T elemento);
        void Actualizar(T elemento);
        void Eliminar(int id);
    }
}
