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
        private readonly IInformacionControlador _informacion;
        private readonly Usuario _usuario;

        public FormInformacion(IAumentosControlador aumentos,IInformacionControlador informacion, Usuario usuario,
            int inicio = 0)
        {
            _aumentos = aumentos;
            _informacion = informacion;
            _usuario = usuario;

            Text = $"PowerGym - Mi información ({usuario.Nombre})";
            ClientSize = new Size(900, 500);
            StartPosition = FormStartPosition.CenterParent;

            // TabControl = contenedor de pestañas. Dock = Fill lo estira a toda la ventana.
            TabControl pestanas = new TabControl { Dock = DockStyle.Fill };
            pestanas.TabPages.Add(CrearPestanaAumentos());
            pestanas.TabPages.Add(CrearPestanaVisitas());
            pestanas.TabPages.Add(CrearPestanaMediciones());
            pestanas.TabPages.Add(CrearPestanaRutinas());
            pestanas.SelectedIndex = inicio;
            Controls.Add(pestanas);
        }

        private static DataGridView CrearTabla(params string[] columnas)
        {
            DataGridView tabla = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,                 // no se edita a mano
                AllowUserToAddRows = false,      // sin fila vacía al final
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // "params" permite pasar cualquier cantidad de textos separados por coma.
            foreach (string columna in columnas)
                tabla.Columns.Add(columna, columna);   // (nombre interno, encabezado visible)

            return tabla;
        }

        private TabPage CrearPestanaRutinas()
        {
            TabPage pagina = new TabPage("Últimas 5 rutinas");
            List<Rutina> rutinas = _informacion.ObtenerUltimasRutinas(_usuario);

            DataGridView tabla = CrearTabla("Rutina", "Inicio", "Fin", "Versión", "Estado");
            foreach (Rutina r in rutinas)
                tabla.Rows.Add(r.Nombre, r.FechaInicio.ToString("dd/MM/yyyy"),
                               r.FechaFin.ToString("dd/MM/yyyy"), r.Version,
                               r.EstaVencida ? "Vencida" : "Vigente");
            tabla.ClearSelection();

            ListBox detalle = new ListBox { Dock = DockStyle.Fill };
            detalle.Items.Add("Seleccione una rutina para ver sus ejercicios.");

            // Al cambiar de fila, muestra los ejercicios de esa rutina.
            tabla.SelectionChanged += (s, e) =>
            {
                if (tabla.SelectedRows.Count == 0)
                    return;

                // El índice de la fila coincide con el de la lista.
                Rutina elegida = rutinas[tabla.SelectedRows[0].Index];
                detalle.Items.Clear();
                foreach (Ejercicio ej in elegida.Ejercicios)
                    detalle.Items.Add($"{ej.Nombre} - {ej.Serie}x{ej.Repeticiones} - {ej.Peso} kg");
            };

            // TableLayoutPanel reparte la pestaña en dos filas: arriba la tabla (60%)
            // y abajo el detalle (40%).
            TableLayoutPanel division = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            division.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            division.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            division.Controls.Add(tabla, 0, 0);     // (control, columna, fila)
            division.Controls.Add(detalle, 0, 1);

            pagina.Controls.Add(division);
            return pagina;
        }

        private TabPage CrearPestanaVisitas()
        {
            TabPage pagina = new TabPage("Visitas");
            DataGridView tabla = CrearTabla("Fecha", "Tipo de visita");

            foreach (Visitas v in _informacion.ObtenerVisitas(_usuario))
            {
                // IdClase es int?: null = entrenó por su cuenta, sin clase.
                string tipo = v.IdClase == null ? "Entrenamiento libre" : $"Clase #{v.IdClase}";
                tabla.Rows.Add(v.Fecha.ToString("dd/MM/yyyy HH:mm"), tipo);
            }
            tabla.ClearSelection();

            pagina.Controls.Add(tabla);
            return pagina;
        }
        private TabPage CrearPestanaMediciones()
        {
            TabPage pagina = new TabPage("Mediciones");
            DataGridView tabla = CrearTabla("Fecha", "Peso (kg)", "% Grasa", "Masa muscular (kg)");

            foreach (Medicion m in _informacion.ObtenerMediciones(_usuario))
                tabla.Rows.Add(m.Fecha.ToString("dd/MM/yyyy"), m.Peso,
                               m.PorcentajeGrasa, m.MasaMuscular);
            tabla.ClearSelection();

            pagina.Controls.Add(tabla);
            return pagina;
        }
        private TabPage CrearPestanaAumentos()
        {
            TabPage pagina = new TabPage("Control de aumentos");

            DataGridView tabla = CrearTabla("Ejercicio", "Rutina", "Fecha", "Series",
                "Repeticiones", "Peso", "Dif Series", "Dif Reps", "Dif Peso");
            foreach (CambioEjercicio c in _aumentos.ObtenerHistorial(_usuario))
            {
                int fila = tabla.Rows.Add(
                        c.Ejercicio, c.Rutina, c.Fecha.ToString("dd/MM/yyyy"),
                        c.Series, c.Repeticiones, c.Peso,
                        Signo(c.DiferenciaSeries), Signo(c.DiferenciaRepeticiones),
                        Signo(c.DiferenciaPeso));
                if (c.DiferenciaPeso > 0)
                    tabla.Rows[fila].Cells["Dif Peso"].Style.ForeColor = Color.Green;
                else if (c.DiferenciaPeso < 0)
                    tabla.Rows[fila].Cells["Dif Peso"].Style.ForeColor = Color.Red;
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
