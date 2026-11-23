using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Repositorios;

namespace PowerGym.Servicios
{
    public class ServicioNotificaciones
    {
        private const int DiasEntreMediciones = 28;
        // Visitas en una misma semana para recibir el aviso de descanso.
        private const int VisitasParaDescanso = 5;

        private readonly IRepositorio<Notificacion> _notificaciones;
        private readonly IRepositorio<Usuario> _usuarios;
        private readonly IRepositorio<Rutina> _rutinas;
        private readonly IRepositorio<Medicion> _mediciones;
        private readonly IRepositorio<Visitas> _visitas;
        private readonly int _diasAnticipacion;


        public ServicioNotificaciones(IRepositorio<Notificacion> notificaciones,
                                      IRepositorio<Usuario> usuarios,
                                      IRepositorio<Rutina> rutinas,
                                      IRepositorio<Medicion> mediciones,
                                      IRepositorio<Visitas> visitas,
                                      int diasAnticipacion = 7)
        {
            _notificaciones = notificaciones;
            _usuarios = usuarios;
            _rutinas = rutinas;
            _mediciones = mediciones;
            _visitas = visitas;
            _diasAnticipacion = diasAnticipacion;
        }

        public void AlOtorgarseInsignia(Insignia insignia)
        {
            // EsPositiva() responde distinto según la hija (polimorfismo).
            if (insignia.EsPositiva())
                Crear(insignia.IdUsuario, false, TipoNotificacion.InsigniaGanada,
                      $"Has ganado la insignia {insignia.Nombre}");
            else
                Crear(insignia.IdUsuario, false, TipoNotificacion.InsigniaVerguenza,
                      $"Te asignaron la insignia de la verguenza {insignia.Nombre}");
        }

        // Se ejecuta cada vez que ServicioRutinas dispara RutinaAgregada.
        public void AlAgregarseRutina(Rutina rutina)
        {
            Crear(rutina.IdUsuario, false, TipoNotificacion.NuevaRutina,
                  $"Nueva rutina agregada: {rutina.Nombre}");
        }

        // ---------- Avisos que se calculan al cargar el sistema ----------

        public void GenerarAvisosAlCargar()
        {
            GenerarAvisosDescanso();
            GenerarAvisosRutinasPorExpirar();
            GenerarAvisosMedicionesCercanas();
        }

        private void GenerarAvisosDescanso()
        {
            // Agrupa las visitas por (usuario, lunes de esa semana).
            var semanas = _visitas.ObtenerTodos()
                .GroupBy(v => new { v.IdUsuario, Lunes = LunesDe(v.Fecha) })
                .Where(grupo => grupo.Count() >= VisitasParaDescanso);

            foreach (var semana in semanas)
            {
                // El aviso lleva la fecha de la visita que completó las 5.
                DateTime fecha = semana.OrderBy(v => v.Fecha)
                                       .ElementAt(VisitasParaDescanso - 1).Fecha;

                Crear(semana.Key.IdUsuario, false, TipoNotificacion.Descansa,
                      $"Descansa esta semana: ya acumulaste {VisitasParaDescanso} visitas " +
                      $"en la semana del {semana.Key.Lunes:dd/MM/yyyy}", fecha);
            }
        }

        // Rutinas que vencen dentro del plazo -> aviso al entrenador que las creó.
        private void GenerarAvisosRutinasPorExpirar()
        {
            DateTime limite = DateTime.Today.AddDays(_diasAnticipacion);

            foreach (Rutina rutina in _rutinas.ObtenerTodos()
                         .Where(r => r.FechaFin >= DateTime.Today && r.FechaFin <= limite))
            {
                Usuario dueno = _usuarios.ObtenerPorId(rutina.IdUsuario);
                string nombre = dueno == null ? "un usuario" : dueno.Nombre;

                Crear(rutina.IdEntrenador, true, TipoNotificacion.RutinaExpirando,
                      $"La rutina '{rutina.Nombre}' de {nombre} vence el " +
                      $"{rutina.FechaFin:dd/MM/yyyy}. Cargue una nueva.");
            }
        }

        // Mediciones que tocan pronto (o ya atrasadas) -> aviso al entrenador del usuario.
        private void GenerarAvisosMedicionesCercanas()
        {
            DateTime limite = DateTime.Today.AddDays(_diasAnticipacion);

            foreach (Usuario usuario in _usuarios.ObtenerTodos())
            {
                // Sin entrenador asignado no hay a quién avisar.
                if (usuario.IdEntrenador == null)
                    continue;

                List<Medicion> propias = _mediciones.ObtenerTodos()
                    .Where(m => m.IdUsuario == usuario.Id).ToList();

                // Sin mediciones no hay fecha de la cual contar.
                if (propias.Count == 0)
                    continue;

                DateTime proxima = propias.Max(m => m.Fecha).AddDays(DiasEntreMediciones);

                if (proxima <= limite)
                    Crear(usuario.IdEntrenador.Value, true, TipoNotificacion.MedicionPronto,
                          $"Se acerca la medicion de {usuario.Nombre}: corresponde el " +
                          $"{proxima:dd/MM/yyyy}");
            }
        }

        public List<Notificacion> ObtenerTimeline(int idDestinatario, bool esEntrenador)
        {
            return _notificaciones.ObtenerTodos()
                .Where(n => n.IdDestinatario == idDestinatario
                         && n.EsEntrenador == esEntrenador)
                .OrderByDescending(n => n.Fecha)
                .ToList();
        }

        // ---------- Auxiliares ----------

        // Crea la notificación solo si no existe una igual (evita duplicados al reabrir).
        // "fecha = null" es opcional: si no se manda, se usa la hora actual.
        private void Crear(int idDestinatario, bool esEntrenador, TipoNotificacion tipo,
                           string mensaje, DateTime? fecha = null)
        {
            bool yaExiste = _notificaciones.ObtenerTodos().Any(n =>
                n.IdDestinatario == idDestinatario
                && n.EsEntrenador == esEntrenador
                && n.Tipo == tipo
                && n.Mensaje == mensaje);

            if (yaExiste)
                return;

            // "fecha ?? DateTime.Now" = usa fecha; si es null, usa la hora actual.
            var nueva = new Notificacion(0, idDestinatario, fecha ?? DateTime.Now, tipo, mensaje);
            nueva.EsEntrenador = esEntrenador;
            _notificaciones.Agregar(nueva);
        }

        // Lunes de la semana de una fecha. DayOfWeek: domingo = 0 ... sábado = 6.
        private static DateTime LunesDe(DateTime fecha)
        {
            int diasDesdeLunes = ((int)fecha.DayOfWeek + 6) % 7;
            return fecha.Date.AddDays(-diasDesdeLunes);
        }
    }

}
