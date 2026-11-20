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
    public class FormEditarRutina:Form
    {
        private readonly IRutinasControlador _controlador;
        private readonly Entrenador _entrenador;
        private readonly int _idUsuario;
        private readonly Rutina _rutinaExistente;

        private readonly List<Ejercicio> _ejercicios = new List<Ejercicio>();

        private TextBox txtNombre;
        private DateTimePicker dtpInicio;
        private DateTimePicker dtpFin;
        private ComboBox cmbEjercicio;
        private TextBox txtSeries, txtRepeticiones, txtPeso;
        private ListBox lstEjercicios;

        public FormEditarRutina(IRutinasControlador controlador, Entrenador entrenador,
                                int idUsuario, Rutina rutinaExistente = null)
        {
            _controlador = controlador;
            _entrenador = entrenador;
            _idUsuario = idUsuario;
            _rutinaExistente = rutinaExistente;

            ConstruirInterfaz();
            CargarDatosExistentes();
        }

        private void ConstruirInterfaz()
        {
            Text = _rutinaExistente == null ? "Nueva rutina" : "Editar rutina";
            ClientSize = new Size(500, 550);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            Etiqueta("Nombre de la rutina:", 15, 15);
            txtNombre = new TextBox { Location = new Point(15, 37), Width = 470 };

            Etiqueta("Fecha de inicio:", 15, 75);
            dtpInicio = new DateTimePicker
            {
                Location = new Point(15, 97),
                Width = 220,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            Etiqueta("Fecha de fin (vigencia):", 265, 75);
            dtpFin = new DateTimePicker
            {
                Location = new Point(265, 97),
                Width = 220,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(28)
            };

            Etiqueta("Ejercicio:", 15, 140);
            cmbEjercicio = new ComboBox
            {
                Location = new Point(15, 160),
                Width = 470,
                // DropDown = se puede elegir de la lista O escribir uno propio.
                DropDownStyle = ComboBoxStyle.DropDown,
                // Mientras se escribe, sugiere ejercicios de la lista.
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            cmbEjercicio.Items.AddRange(_controlador.ObtenerNombresEjercicios().ToArray());

            cmbEjercicio.SelectedIndexChanged += (s, e) =>
            {
                string elegido = cmbEjercicio.SelectedItem?.ToString();
                if (elegido != null && _controlador.EsDePesoCorporal(elegido))
                    txtPeso.Text = "0";
            };

            Etiqueta("Series:", 15, 195);
            txtSeries = new TextBox { Location = new Point(15, 215), Width = 100 };
            Etiqueta("Repeticiones:", 135, 195);
            txtRepeticiones = new TextBox { Location = new Point(135, 215), Width = 100 };
            Etiqueta("Peso (kg):", 255, 195);
            txtPeso = new TextBox { Location = new Point(255, 215), Width = 100 };

            // Ayuda visible ANTES de que el usuario se equivoque (lo que pediste).
            Controls.Add(new Label
            {
                Text = "Peso en kg. Escriba 0 si el ejercicio es con el propio cuerpo.",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 245)
            });

            Button btnAgregar = new Button
            {
                Text = "Agregar ejercicio",
                Location = new Point(15, 270),
                Width = 150,
                Height = 28
            };
            btnAgregar.Click += BtnAgregarEjercicio_Click;

            lstEjercicios = new ListBox { Location = new Point(15, 310), Size = new Size(470, 140) };

            Button btnQuitar = new Button
            {
                Text = "Quitar seleccionado",
                Location = new Point(15, 455),
                Width = 150,
                Height = 28
            };
            btnQuitar.Click += BtnQuitarEjercicio_Click;

            Button btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(240, 500),
                Width = 110,
                Height = 34
            };
            btnGuardar.Click += BtnGuardar_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(375, 500),
                Width = 110,
                Height = 34,
                DialogResult = DialogResult.Cancel
            };
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                txtNombre, dtpInicio, dtpFin, cmbEjercicio, txtSeries, txtRepeticiones,
                txtPeso, btnAgregar, lstEjercicios, btnQuitar, btnGuardar, btnCancelar
            });
        }

        private void Etiqueta(string texto, int x, int y)
        {
            Controls.Add(new Label { Text = texto, AutoSize = true, Location = new Point(x, y) });
        }

        private void CargarDatosExistentes()
        {
            if (_rutinaExistente == null)
                return;

            txtNombre.Text = _rutinaExistente.Nombre;
            dtpInicio.Value = _rutinaExistente.FechaInicio;
            dtpFin.Value = _rutinaExistente.FechaFin;

            // Se copia la lista para no tocar la original hasta que se guarde.
            _ejercicios.AddRange(_rutinaExistente.Ejercicios);
            RefrescarLista();
        }

        private void BtnAgregarEjercicio_Click(object sender, EventArgs e)
        {
            try
            {
                Ejercicio ejercicio = _controlador.CrearEjercicio(
                    cmbEjercicio.Text, txtSeries.Text, txtRepeticiones.Text, txtPeso.Text);

                _ejercicios.Add(ejercicio);
                RefrescarLista();

                // Limpia los campos para escribir el siguiente ejercicio.
                cmbEjercicio.Text=""; txtSeries.Clear(); txtRepeticiones.Clear(); txtPeso.Clear();
                cmbEjercicio.Focus();
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ejercicio inválido",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BtnQuitarEjercicio_Click(object sender, EventArgs e)
        {
            // SelectedIndex vale -1 si no hay nada seleccionado.
            if (lstEjercicios.SelectedIndex < 0)
                return;

            _ejercicios.RemoveAt(lstEjercicios.SelectedIndex);
            RefrescarLista();
        }

        private void RefrescarLista()
        {
            lstEjercicios.Items.Clear();
            foreach (Ejercicio ej in _ejercicios)
                lstEjercicios.Items.Add($"{ej.Nombre} - {ej.Serie}x{ej.Repeticiones} - {ej.Peso} kg");
        }
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_rutinaExistente == null)
                    _controlador.Crear(_entrenador, _idUsuario, txtNombre.Text,
                                       dtpInicio.Value.Date, dtpFin.Value.Date, _ejercicios);
                else
                    _controlador.Editar(_rutinaExistente.Id, txtNombre.Text,
                                        dtpInicio.Value.Date, dtpFin.Value.Date, _ejercicios);

                DialogResult = DialogResult.OK;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Datos inválidos",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (KeyNotFoundException ex)
            {
                // Pasa si la rutina se eliminó mientras se editaba.
                MessageBox.Show(ex.Message, "Rutina inexistente",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
        }


    }
}
