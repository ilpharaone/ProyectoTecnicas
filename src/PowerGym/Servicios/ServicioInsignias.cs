using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioInsignias
    {
        private readonly IRepositorio<Insignia> _insignias;
        private readonly IRepositorio<Usuario> _usuarios;
        private readonly IRepositorio<Visitas> _visitas;
        private readonly IRepositorio<Medicion> _mediciones;
        private readonly List<IReglaInsignia> _reglas;

        
        public event Action<Insignia> InsigniaOtorgada;

        // Recibe las reglas desde afuera: el servicio no sabe cuáles ni cuántas son.
        public ServicioInsignias(IRepositorio<Insignia> insignias,
                                 IRepositorio<Usuario> usuarios,
                                 IRepositorio<Visitas> visitas,
                                 IRepositorio<Medicion> mediciones,
                                 IEnumerable<IReglaInsignia> reglas)
        {
            _insignias = insignias;
            _usuarios = usuarios;
            _visitas = visitas;
            _mediciones = mediciones;
            _reglas = reglas.ToList();
        }

        // Evalúa a todos los usuarios. Se llama al abrir el programa.
        public void EvaluarTodos()
        {
            Dictionary<int, int> conteo = ContarVisitas();
            HashSet<int> top = ObtenerTopMenosVisitas(conteo);
            List<Medicion> mediciones = _mediciones.ObtenerTodos();

            foreach (Usuario usuario in _usuarios.ObtenerTodos())
                EvaluarUsuario(usuario.Id, conteo, top, mediciones);
        }

        // Evalúa solo a un usuario (por ejemplo, al abrir su ventana de insignias).
        
        public void EvaluarUsuario(int idUsuario)
        {
            Dictionary<int, int> conteo = ContarVisitas();
            EvaluarUsuario(idUsuario, conteo, ObtenerTopMenosVisitas(conteo),
                           _mediciones.ObtenerTodos());
        }

        public List<Insignia> ObtenerInsignias(int idUsuario)
        {
            return _insignias.ObtenerTodos()
                             .Where(i => i.IdUsuario == idUsuario)
                             .OrderByDescending(i => i.FechaObtenida)
                             .ToList();
        }

        // Clave = id del usuario, valor = cuántas visitas tiene.
        private Dictionary<int, int> ContarVisitas()
        {
            return _visitas.ObtenerTodos()
                           .GroupBy(v => v.IdUsuario)
                           .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());
        }

        // Los 10 usuarios con menos visitas. El desempate es por id (siempre el mismo resultado).
        private HashSet<int> ObtenerTopMenosVisitas(Dictionary<int, int> conteo)
        {
            return _usuarios.ObtenerTodos()
                            .OrderBy(u => conteo.TryGetValue(u.Id, out int total) ? total : 0)
                            .ThenBy(u => u.Id)
                            .Take(10)
                            .Select(u => u.Id)
                            .ToHashSet();
        }

        private void EvaluarUsuario(int idUsuario, Dictionary<int, int> conteo,
                                    HashSet<int> top, List<Medicion> todasMediciones)
        {
            conteo.TryGetValue(idUsuario, out int totalVisitas);   // 0 si no tiene visitas

            List<Medicion> propias = todasMediciones
                .Where(m => m.IdUsuario == idUsuario)
                .OrderBy(m => m.Fecha)
                .ToList();

            var datos = new Datos(idUsuario, totalVisitas, propias, top.Contains(idUsuario));

            List<Insignia> actuales = _insignias.ObtenerTodos()
                                                .Where(i => i.IdUsuario == idUsuario)
                                                .ToList();

            foreach (IReglaInsignia regla in _reglas)
            {
                // La insignia que este usuario ya tiene de esta regla (o null).
                Insignia existente = actuales.FirstOrDefault(regla.Corresponde);
                bool cumple = regla.Cumple(datos);

                if (cumple && existente == null)
                {
                    // Se otorga UNA sola vez: solo si todavía no la tiene.
                    Insignia nueva = regla.Crear(idUsuario, DateTime.Today);
                    _insignias.Agregar(nueva);
                    InsigniaOtorgada?.Invoke(nueva);
                }
                else if (!cumple && existente != null && !regla.EsPositiva)
                {
                    // Vergüenza que ya no aplica: se retira. Los logros nunca se quitan.
                    _insignias.Eliminar(existente.Id);
                }
            }
        }
    }
}
