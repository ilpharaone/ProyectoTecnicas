using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Controladores;

namespace PowerGym.Vistas
{
    public class FormRutina : Form
    {
        private readonly IRutinasControlador _controlador;
        private readonly Entrenador _entrenador;
        private List<Rutina> _rutinas = new List<Rutina>();

        private ComboBox cmbUsuario;
        private DataGridView tabla;

        public FormRutina(IRutinasControlador controlador, Entrenador entrenador)
        {
            _controlador = controlador;
            _entrenador = entrenador;
            ConstruirInterfaz();
            CargarRutinas();
        }

        private void ConstruirInterfaz()
        {
            Text = "PowerGym - Rutinas";
            ClientSize = new Size(700, 440);
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Usuario:", AutoSize = true, Location = new Point(15, 18) });

            cmbUsuario = new ComboBox
            {
                Location = new Point(80, 15),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Nombre"     // muestra la propiedad Nombre de cada Usuario
            };
            cmbUsuario.DataSource = _controlador.ObtenerUsuarios(_entrenador);
            
            cmbUsuario.SelectedIndexChanged += (s, e) => CargarRutinas();

            tabla = new DataGridView
            {
                Location = new Point(15, 55),
                Size = new Size(670, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            tabla.Columns.Add("colNombre", "Rutina");
            tabla.Columns.Add("colInicio", "Inicio");
            tabla.Columns.Add("colFin", "Fin");
            tabla.Columns.Add("colVersion", "Versión");
            tabla.Columns.Add("colEjercicios", "Ejercicios");
            tabla.Columns.Add("colEstado", "Estado");

            Button btnNueva = Boton("Nueva", 15);
            btnNueva.Click += BtnNueva_Click;
            Button btnEditar = Boton("Editar", 135);
            btnEditar.Click += BtnEditar_Click;
            Button btnEliminar = Boton("Eliminar", 255);
            btnEliminar.Click += BtnEliminar_Click;

            Controls.AddRange(new Control[] { cmbUsuario, tabla, btnNueva, btnEditar, btnEliminar });
        }
        private Button Boton(string texto, int x)
        {
            return new Button { Text = texto, Location = new Point(x, 390), Width = 110, Height = 34 };
        }

        private Usuario UsuarioSeleccionado() => cmbUsuario.SelectedItem as Usuario;

        // Rutina de la fila elegida, o null si no hay ninguna.
        private Rutina RutinaSeleccionada()
        {
            if (tabla.SelectedRows.Count == 0)
                return null;
            return _rutinas[tabla.SelectedRows[0].Index];
        }

        private void CargarRutinas()
        {
            tabla.Rows.Clear();
            Usuario usuario = UsuarioSeleccionado();
            if (usuario == null)
            {
                _rutinas = new List<Rutina>();
                return;
            }

            _rutinas = _controlador.ObtenerRutinas(usuario.Id);
            foreach (Rutina r in _rutinas)
                tabla.Rows.Add(r.Nombre, r.FechaInicio.ToString("dd/MM/yyyy"),
                               r.FechaFin.ToString("dd/MM/yyyy"), r.Version,
                               r.Ejercicios.Count, r.EstaVencida ? "Vencida" : "Vigente");
            tabla.ClearSelection();
        }

        private void BtnNueva_Click(object sender, EventArgs e)
        {
            Usuario usuario = UsuarioSeleccionado();
            if (usuario == null)
            {
                MessageBox.Show("Seleccione un usuario.");
                return;
            }

            using (var form = new FormEditarRutina(_controlador, _entrenador, usuario.Id))
            {
                // Si guardó, recarga la tabla para ver la rutina nueva.
                if (form.ShowDialog(this) == DialogResult.OK)
                    CargarRutinas();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            Rutina rutina = RutinaSeleccionada();
            if (rutina == null)
            {
                MessageBox.Show("Seleccione una rutina.");
                return;
            }

            using (var form = new FormEditarRutina(_controlador, _entrenador,
                                                   UsuarioSeleccionado().Id, rutina))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    CargarRutinas();   // "Los cambios se reflejan inmediatamente"
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Rutina rutina = RutinaSeleccionada();
            if (rutina == null)
            {
                MessageBox.Show("Seleccione una rutina.");
                return;
            }

            // La eliminación pide confirmación (criterio del PBI 1.4.2).
            DialogResult respuesta = MessageBox.Show(
                $"¿Eliminar la rutina '{rutina.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                _controlador.Eliminar(rutina.Id);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Rutina inexistente");
            }
            CargarRutinas();
        }
    }
}
