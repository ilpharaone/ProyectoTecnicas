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
    public class FormClases : Form
    {
        private readonly IClasesControlador _controlador;
        private readonly Persona _persona;
        private DataGridView tabla;

        public FormClases(IClasesControlador controlador, Persona persona)
        {
            _controlador = controlador;
            _persona = persona;
            ConstruirInterfaz();
            CargarClases();
        }

        private void ConstruirInterfaz()
        {
            Text = "PowerGym - Clases";
            ClientSize = new Size(850, 460);
            StartPosition = FormStartPosition.CenterParent;

            tabla = new DataGridView
            {
                Location = new Point(15, 15),
                Size = new Size(820, 380),
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tabla.Columns.Add("colClase", "Clase");
            tabla.Columns.Add("colEntrenador", "Entrenador");
            tabla.Columns.Add("colHorario", "Horario");
            tabla.Columns.Add("colDuracion", "Duración");
            tabla.Columns.Add("colCupo", "Cupo");
            tabla.Columns.Add("colInscripcion", "Inscripción");
            tabla.Columns.Add("colEstado", "Estado");
            Controls.Add(tabla);

            // Solo el usuario básico ve el botón. "is" pregunta el tipo de la persona.
            if (_persona is Usuario)
            {
                Button btnInscribir = new Button
                {
                    Text = "Inscribirme",
                    Location = new Point(15, 410),
                    Width = 140,
                    Height = 34
                };
                btnInscribir.Click += BtnInscribir_Click;
                Controls.Add(btnInscribir);
            }
        }

        private void CargarClases()
        {
            // Recuerda la fila elegida para volver a marcarla después de recargar.
            int seleccionada = tabla.SelectedRows.Count > 0 ? tabla.SelectedRows[0].Index : -1;

            tabla.Rows.Clear();
            foreach (ClasesGimnasio c in _controlador.ObtenerClases())
            {
                int fila = tabla.Rows.Add(
                    c.Nombre,
                    _controlador.ObtenerNombreEntrenador(c.IdEntrenador),
                    c.Horario.ToString("ddd dd/MM HH:mm"),
                    $"{c.DuracionMinutos()} min",
                    $"{c.InscritosActuales}/{c.CupoMaximo}",
                    c.RequiereInscripcion() ? "Requerida" : "No requerida",
                    ObtenerEstado(c));
                tabla.Rows[fila].Tag = c;
            }

            if (seleccionada >= 0 && seleccionada < tabla.Rows.Count)
                tabla.Rows[seleccionada].Selected = true;
            else
                tabla.ClearSelection();
        }

        private string ObtenerEstado(ClasesGimnasio clase)
        {
            if (!clase.RequiereInscripcion())
                return "Libre";

            // "is Usuario u" pregunta el tipo y, si coincide, lo deja en la variable u.
            if (_persona is Usuario u && _controlador.EstaInscrito(u, clase))
                return "Inscrito";

            return clase.EstaLlena ? "Llena" : "Disponible";
        }

        private void BtnInscribir_Click(object sender, EventArgs e)
        {
            if (tabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una clase.");
                return;
            }

            // "as" convierte el Tag de la fila a clase de gimnasio.
            ClasesGimnasio clase = tabla.SelectedRows[0].Tag as ClasesGimnasio;

            try
            {
                _controlador.Inscribir((Usuario)_persona, clase);

                MessageBox.Show($"Inscripción exitosa en '{clase.Nombre}'. Se registró su visita.",
                                "PowerGym", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClases();   // "Actualizar la vista de la clase"
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "No se pudo inscribir",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Clase inexistente",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarClases();
            }
        }
    }
}

