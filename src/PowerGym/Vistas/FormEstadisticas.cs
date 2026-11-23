using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Controladores;
using PowerGym.Modelos;

namespace PowerGym.Vistas
{
    public class FormEstadisticas : Form
    {
        private readonly IEstadisticasControlador _controlador;
        private readonly Usuario _usuario;

        private DateTimePicker dtpDesde;
        private DateTimePicker dtpHasta;
        private DataGridView tablaAumentos;
        private DataGridView tablaVisitantes;
        private DataGridView tablaAusentes;
        private Label lblMejoras;

        public FormEstadisticas(IEstadisticasControlador controlador, Usuario usuario)
        {
            _controlador = controlador;
            _usuario = usuario;
            ConstruirInterfaz();
            Actualizar();   // los reportes se calculan al abrir la ventana
        }

        private void ConstruirInterfaz()
        {
            Text = $"PowerGym - Reportes de {_usuario.Nombre}";
            ClientSize = new Size(800, 520);
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Desde:", AutoSize = true, Location = new Point(15, 20) });
            dtpDesde = new DateTimePicker
            {
                Location = new Point(65, 16),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-6),    // por defecto, los últimos 6 meses
                MaxDate = DateTime.Today                 // no deja elegir fechas futuras
            };

            Controls.Add(new Label { Text = "Hasta:", AutoSize = true, Location = new Point(225, 20) });
            dtpHasta = new DateTimePicker
            {
                Location = new Point(275, 16),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                MaxDate = DateTime.Today
            };

            Button btnConsultar = new Button
            {
                Text = "Consultar",
                Location = new Point(435, 13),
                Width = 110,
                Height = 30
            };
            btnConsultar.Click += (s, e) => Actualizar();

            tablaAumentos = CrearTabla("Ejercicio", "Peso inicial (kg)", "Peso final (kg)", "Aumento (kg)");
            tablaVisitantes = CrearTabla("Posición", "Usuario", "Visitas");
            tablaAusentes = CrearTabla("Posición", "Usuario", "Visitas");

            lblMejoras = new Label { Padding = new Padding(15), Font = new Font("Segoe UI", 11) };

            TabControl pestanas = new TabControl
            {
                Location = new Point(15, 60),
                Size = new Size(ClientSize.Width - 30, ClientSize.Height - 75),
                // Anchor hace que el TabControl se estire si se agranda la ventana.
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            pestanas.TabPages.Add(CrearPestana("Aumento de pesos", tablaAumentos));
            pestanas.TabPages.Add(CrearPestana("Mejoras en objetivos", lblMejoras));
            pestanas.TabPages.Add(CrearPestana("Top 10 visitantes", tablaVisitantes));
            pestanas.TabPages.Add(CrearPestana("Top 10 ausentes", tablaAusentes));

            Controls.AddRange(new Control[] { dtpDesde, dtpHasta, btnConsultar, pestanas });
        }

        // Mete un control dentro de una pestaña, ocupando todo el espacio.
        private static TabPage CrearPestana(string titulo, Control contenido)
        {
            contenido.Dock = DockStyle.Fill;
            TabPage pagina = new TabPage(titulo);
            pagina.Controls.Add(contenido);
            return pagina;
        }

        private static DataGridView CrearTabla(params string[] columnas)
        {
            DataGridView tabla = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            foreach (string columna in columnas)
                tabla.Columns.Add(columna, columna);
            return tabla;
        }

        // Pide los datos al controlador y llena las cuatro pestañas.
        private void Actualizar()
        {
            try
            {
                DateTime desde = dtpDesde.Value.Date;
                DateTime hasta = dtpHasta.Value.Date;

                LlenarAumentos(_controlador.ObtenerAumentos(_usuario, desde, hasta));
                lblMejoras.Text = ArmarTextoMejoras(_controlador.ObtenerResumenMediciones(_usuario, desde, hasta));
                LlenarRanking(tablaVisitantes, _controlador.ObtenerTopVisitantes(desde, hasta));
                LlenarRanking(tablaAusentes, _controlador.ObtenerTopAusentes(desde, hasta));
            }
            catch (ArgumentException ex)
            {
                // Fechas inválidas: el mensaje lo escribió el controlador.
                MessageBox.Show(ex.Message, "Rango de fechas inválido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LlenarAumentos(List<AumentoEjercicio> aumentos)
        {
            tablaAumentos.Rows.Clear();
            foreach (AumentoEjercicio a in aumentos)
            {
                int fila = tablaAumentos.Rows.Add(a.Ejercicio, a.PesoInicial, a.PesoFinal, Signo(a.Aumento));

                // Cells[3] = cuarta columna (Aumento). Verde si subió, rojo si bajó.
                if (a.Aumento > 0)
                    tablaAumentos.Rows[fila].Cells[3].Style.ForeColor = Color.Green;
                else if (a.Aumento < 0)
                    tablaAumentos.Rows[fila].Cells[3].Style.ForeColor = Color.Red;
            }
            tablaAumentos.ClearSelection();
        }

        private void LlenarRanking(DataGridView tabla, List<Ranking> ranking)
        {
            tabla.Rows.Clear();
            foreach (Ranking p in ranking)
            {
                int fila = tabla.Rows.Add(p.Posicion, p.Nombre, p.Visitas);

                // Resalta la fila de quien tiene la sesión abierta.
                if (p.IdUsuario == _usuario.Id)
                {
                    tabla.Rows[fila].DefaultCellStyle.Font = new Font(tabla.Font, FontStyle.Bold);
                    tabla.Rows[fila].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
            tabla.ClearSelection();
        }

        private static string ArmarTextoMejoras(ResumenMediciones r)
        {
            if (r == null)
                return "Se necesitan al menos 2 mediciones dentro del rango para calcular mejoras.";

            return $"Entre el {r.FechaInicial:dd/MM/yyyy} y el {r.FechaFinal:dd/MM/yyyy}:\n\n" +
                   $"Peso corporal: {Signo(r.CambioPeso)} kg\n" +
                   $"Grasa corporal: {Signo(r.CambioGrasa)} puntos (bajar es mejorar)\n" +
                   $"Masa muscular: {Signo(r.CambioMasaMuscular)} kg (subir es mejorar)";
        }

        private static string Signo(double valor)
        {
            return valor.ToString("+0.##;-0.##;0", CultureInfo.InvariantCulture);
        }
    }
}
