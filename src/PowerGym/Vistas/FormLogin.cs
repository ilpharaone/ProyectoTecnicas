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
    public class FormLogin:Form
    {
        private readonly ILoginControlador _controlador;
        private readonly IRegistroControlador _registro;

        private TextBox txtUsuario;

        public Persona PersonaAutenticada { get; private set; }

        public FormLogin(ILoginControlador controlador,IRegistroControlador registro)
        {
            _controlador = controlador;
            _registro = registro;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            // Propiedades de la ventana.
            Text = "PowerGym - Iniciar sesión";
            ClientSize = new Size(320, 240);          // tamaño interior en píxeles
            StartPosition = FormStartPosition.CenterScreen;   // aparece centrada
            FormBorderStyle = FormBorderStyle.FixedDialog;    // no se puede redimensionar
            MaximizeBox = false;                      // sin botón de maximizar

            Label lblTitulo = new Label
            {
                Text = "Bienvenido a PowerGym",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                AutoSize = true,                      
                Location = new Point(40, 20)          
            };

            Label lblUsuario = new Label
            {
                Text = "Nombre de usuario:",
                AutoSize = true,
                Location = new Point(40, 70)
            };

            txtUsuario = new TextBox
            {
                Location = new Point(40, 92),
                Width = 240
            };

            Button btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new Point(40, 132),
                Width = 240,
                Height = 34
            };

            btnIngresar.Click += BtnIngresar_Click;
            
           
            Button btnRegistrar = new Button
            {
                Text = "Crear Cuenta",
                Location = new Point(40, 170),
                Width = 240,
                Height = 34
            };
            btnRegistrar.Click += BtnRegistrar_Click;

            AcceptButton = btnIngresar;
            // Sin esto los controles no aparecen: hay que agregarlos a la ventana.
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(btnIngresar);
            Controls.Add(btnRegistrar);
        }
        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // La vista NO decide nada: le pregunta al controlador.
                PersonaAutenticada = _controlador.IniciarSesion(txtUsuario.Text);

                // OK le avisa a Program.cs que el login fue exitoso. Cierra la ventana.
                DialogResult = DialogResult.OK;
            }

            catch (ArgumentException ex)
            {
                // El controlador lanza ArgumentException con el mensaje para el usuario.
                MessageBox.Show(ex.Message, "No se pudo iniciar sesión",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();    // deja el cursor listo para reintentar
            }


        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            using (var registro = new FormRegistro(_registro))
            {
                // Si se registró, deja su usuario escrito para entrar con un clic.
                if (registro.ShowDialog(this) == DialogResult.OK)
                    txtUsuario.Text = registro.UsuarioRegistrado.NombreUsuario;
            }
        }
    }
}