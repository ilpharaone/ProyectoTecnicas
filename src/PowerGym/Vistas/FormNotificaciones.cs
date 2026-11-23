using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Controladores;
using PowerGym.Modelos;

namespace PowerGym.Vistas
{
    public class FormNotificaciones : Form
    {
        public FormNotificaciones(INotificacionesControlador controlador, Persona persona)
        {
            Text = $"PowerGym - Notificaciones de {persona.Nombre}";
            ClientSize = new Size(820, 440);
            StartPosition = FormStartPosition.CenterParent;

            DataGridView tabla = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tabla.Columns.Add("colFecha", "Fecha");
            tabla.Columns.Add("colTipo", "Tipo");
            tabla.Columns.Add("colMensaje", "Mensaje");
            
            tabla.Columns["colMensaje"].FillWeight = 300;
            List<Notificacion> timeline = controlador.ObtenerTimeline(persona);
            foreach (Notificacion n in timeline)
            {
                int fila = tabla.Rows.Add(n.Fecha.ToString("dd/MM/yyyy HH:mm"),
                                          NombreDelTipo(n.Tipo), n.Mensaje);

                // Los mensajes de la vergüenza van en rojo.
                if (n.Tipo == TipoNotificacion.InsigniaVerguenza)
                    tabla.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
                else if (n.Tipo == TipoNotificacion.InsigniaGanada)
                    tabla.Rows[fila].DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            tabla.ClearSelection();

            Controls.Add(tabla);

            if (timeline.Count == 0)
                Text += " (sin notificaciones)";
        }
        private static string NombreDelTipo(TipoNotificacion tipo)
        {
            switch (tipo)
            {
                case TipoNotificacion.InsigniaGanada: return "Insignia ganada";
                case TipoNotificacion.InsigniaVerguenza: return "Vergüenza";
                case TipoNotificacion.NuevaRutina: return "Nueva rutina";
                case TipoNotificacion.Descansa: return "Descanso";
                case TipoNotificacion.RutinaExpirando: return "Rutina por expirar";
                case TipoNotificacion.MedicionPronto: return "Medición cercana";
                default: return tipo.ToString();
            }
        }
    }
}

