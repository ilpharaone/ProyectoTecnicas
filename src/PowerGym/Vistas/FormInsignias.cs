using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Controladores;
using PowerGym.Modelos;

namespace PowerGym.Vistas
{
    public class FormInsignias : Form
    {
        public FormInsignias(IInsigniasControlador controlador, Usuario usuario)
        {
            Text = $"PowerGym - Insignias de {usuario.Nombre}";
            ClientSize = new Size(760, 420);
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
            tabla.Columns.Add("colTipo", "Tipo");
            tabla.Columns.Add("colNombre", "Insignia");
            tabla.Columns.Add("colDescripcion", "Descripción");
            tabla.Columns.Add("colFecha", "Obtenida");

            List<Insignia> insignias = controlador.ObtenerInsignias(usuario);
            foreach (Insignia insignia in insignias)
            {
                // EsPositiva() responde distinto según la hija: polimorfismo.
                bool positiva = insignia.EsPositiva();

                int fila = tabla.Rows.Add(positiva ? "Logro" : "Vergüenza",
                                          insignia.Nombre, insignia.Descripcion,
                                          insignia.FechaObtenida.ToString("dd/MM/yyyy"));

                tabla.Rows[fila].DefaultCellStyle.ForeColor = positiva ? Color.DarkGreen : Color.Firebrick;
            }
            tabla.ClearSelection();

            Controls.Add(tabla);

            // Mensaje cuando todavía no tiene ninguna.
            if (insignias.Count == 0)
                Text += " (aún sin insignias)";
        }
    }
}
