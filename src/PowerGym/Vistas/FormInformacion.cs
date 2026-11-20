using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Controladores;
using PowerGym.Modelos;

namespace PowerGym.Vistas
{
    public class FormInformacion :Form
    {
        private readonly IAumentosControlador _aumentos;
        private readonly Usuario _usuario;

        public FormInformacion(IAumentosControlador aumentos, Usuario usuario)
        {
            _aumentos = aumentos;
            _usuario = usuario;

            Text = $"PowerGym - Mi información ({usuario.Nombre})";
            ClientSize = new Size(900, 500);
            StartPosition = FormStartPosition.CenterParent;

            // TabControl = contenedor de pestañas. Dock = Fill lo estira a toda la ventana.
            TabControl pestanas = new TabControl { Dock = DockStyle.Fill };
            pestanas.TabPages.Add(CrearPestanaAumentos());
            Controls.Add(pestanas);
        }

        private TabPage CrearPestanaAumentos()
        {
            TabPage pagina = new TabPage("Control de aumentos");

            DataGridView tabla = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tabla.Columns.Add("colEjercicio", "Ejercicio");
            tabla.Columns.Add("colRutina", "Rutina");
            tabla.Columns.Add("colFecha", "Fecha");
            tabla.Columns.Add("colSeries", "Series");
            tabla.Columns.Add("colReps", "Repeticiones");
            tabla.Columns.Add("colPeso", "Peso (kg)");
            tabla.Columns.Add("colDifSeries", "Δ Series");
            tabla.Columns.Add("colDifReps", "Δ Reps");
            tabla.Columns.Add("colDifPeso", "Δ Peso");

            List<CambioEjercicio> historial = _aumentos.ObtenerHistorial(_usuario);
            foreach (CambioEjercicio c in historial)
            {
                int fila = tabla.Rows.Add(
                    c.Ejercicio, c.Rutina, c.Fecha.ToString("dd/MM/yyyy"),
                    c.Series, c.Repeticiones, c.PesoKg,
                    Signo(c.DiferenciaSeries), Signo(c.DiferenciaRepeticiones),
                    Signo(c.DiferenciaPeso));

                // Verde si subió el peso, rojo si bajó.
                if (c.DiferenciaPeso > 0)
                    tabla.Rows[fila].Cells["colDifPeso"].Style.ForeColor = Color.Green;
                else if (c.DiferenciaPeso < 0)
                    tabla.Rows[fila].Cells["colDifPeso"].Style.ForeColor = Color.Red;
            }
            tabla.ClearSelection();

            pagina.Controls.Add(tabla);
            return pagina;
        }

        private static string Signo(double valor)
        {
            return valor.ToString("+0.##;-0.##;0", CultureInfo.InvariantCulture);
        }
    }
}
