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
    public class FormRegistro : Form
    {
        private readonly IRegistroControlador _controlador;

        private TextBox txtNombre;
        private TextBox txtUsuario;
        private TextBox txtEdad;
        private TextBox txtPeso;
        private TextBox txtEstatura;
        private ComboBox cmbEntrenador;

        public Usuario UsuarioRegistrado { get; private set; }

        public FormRegistro(IRegistroControlador controlador)
        {
            _controlador = controlador;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Text = "PowerGym - Registro";
            ClientSize = new Size(340, 470);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            // Cada campo es una etiqueta + un cuadro de texto, uno debajo de otro.
            txtNombre = AgregarCampo("Nombre completo:", 20);
            txtUsuario = AgregarCampo("Nombre de usuario:", 80);
            txtEdad = AgregarCampo("Edad:", 140);
            txtPeso = AgregarCampo("Peso (kg):", 200);
            txtEstatura = AgregarCampo("Estatura (m):", 260);

            // Leave se dispara al salir del cuadro (cambiar de campo).
            txtNombre.Leave += (s, e) => SugerirUsuario();
            Controls.Add(new Label
            {
                Text = "Entrenador: ",
                AutoSize = true,
                Location = new Point(40,320)
            });

            cmbEntrenador = new ComboBox
            {
                Location = new Point(40, 342),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FormattingEnabled = true
            };

            cmbEntrenador.Format += (s, e) =>
            {
                var t = (Entrenador)e.ListItem;
                e.Value = $"{t.Nombre}-{t.Especialidad}({t.Tipo})";
            };

            cmbEntrenador.DataSource = _controlador.ObtenerEntrenadores();
            Controls.Add(cmbEntrenador);


            Button btnRegistrar = new Button
            {
                Text = "Registrarse",
                Location = new Point(40, 330),
                Width = 120,
                Height = 34
            };
            btnRegistrar.Click += BtnRegistrar_Click;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(180, 330),
                Width = 120,
                Height = 34,
                DialogResult = DialogResult.Cancel   // cierra la ventana con "Cancelar"
            };

            AcceptButton = btnRegistrar;   // Enter = Registrarse
            CancelButton = btnCancelar;    // Esc = Cancelar

            Controls.Add(btnRegistrar);
            Controls.Add(btnCancelar);
        }
        private TextBox AgregarCampo(string etiqueta, int y)
        {
            Controls.Add(new Label
            {
                Text = etiqueta,
                AutoSize = true,
                Location = new Point(40, y)
            });

            TextBox caja = new TextBox { Location = new Point(40, y + 22), Width = 260 };
            Controls.Add(caja);
            return caja;
        }
        private void SugerirUsuario()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                txtUsuario.Text = _controlador.SugerirNombreUsuario(txtNombre.Text);
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idEntrenador = (cmbEntrenador.SelectedItem as Entrenador)?.Id;
                UsuarioRegistrado = _controlador.Registrar(
                    txtNombre.Text, 
                    txtUsuario.Text,
                    txtEdad.Text, 
                    txtPeso.Text,
                    txtEstatura.Text);

                // IMC viene calculado desde el modelo.
                MessageBox.Show(
                    $"Registro exitoso.\nUsuario: {UsuarioRegistrado.NombreUsuario}\n" +
                    $"IMC: {UsuarioRegistrado.IMC}",
                    "PowerGym", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (ArgumentException ex)
            {
                // El mensaje lo escribió el modelo, el servicio o el controlador.
                MessageBox.Show(ex.Message, "Datos inválidos",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
