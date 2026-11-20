using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;
using PowerGym.Controladores;

namespace PowerGym.Vistas
{
    class FormPrincipal:Form
    {
        private readonly Persona _persona;
        private MenuStrip _menu;
        private readonly IMisUsuariosControlador _misUsuarios;
        private readonly IRutinasControlador _rutinas;
        public bool CerroSesion { get; private set; }
        public FormPrincipal(Persona persona,IMisUsuariosControlador misUsuarios, IRutinasControlador rutinas)
        {
            _persona = persona;
            _misUsuarios = misUsuarios;
            _rutinas = rutinas;
            Text = $"PowerGym - {persona.Nombre} ({Rol()})";
            ClientSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            HacerMenu();

            Shown += (s, e) => Activate();

            Controls.Add(new Label
            {
                Text = $"Hola, {persona.Nombre}. Entró como {Rol()}.",
                AutoSize = true,
                Location = new Point(20, 20)
            });
        }

        private string Rol()
        {
            return _persona is Entrenador ? "Entrenador" : "Usuario";
        }

        private void HacerMenu()
        {
            _menu = new MenuStrip();

            if (_persona is Entrenador)
                AgregarOpcionEntrenador();
            else
                AgregarOpcionUsuario();

            ToolStripMenuItem sesion = new ToolStripMenuItem("Cerrar Sesion");
            sesion.Click += (s, e) => CerrarSesion();
            _menu.Items.Add(sesion);

            MainMenuStrip = _menu;
            Controls.Add(_menu);
        }

        private void AgregarOpcionUsuario()
        {
            AgregarOpcion("Mis rutinas");
            AgregarOpcion("Mis visitas");
            AgregarOpcion("Mis mediciones");
            AgregarOpcion("Clases");
            AgregarOpcion("Insignias");
            AgregarOpcion("Notificaciones");
            AgregarOpcion("Reportes");
        }

        private void AgregarOpcionEntrenador()
        {
            AgregarOpcion("Mis usuarios", AbrirMisUsuarios);
            AgregarOpcion("Rutinas",AbrirRutinas);
            AgregarOpcion("Clases");
            AgregarOpcion("Notificaciones");
            
        }

        private void AbrirMisUsuarios()
        {
            using(var ventana = new FormMisUsuarios(_misUsuarios, (Entrenador)_persona))
            {
                ventana.ShowDialog(this);
            }
        }

        private void AbrirRutinas()
        {
            using (var ventana = new FormRutina(_rutinas, (Entrenador)_persona))
            {
                ventana.ShowDialog(this);
            }
        }
        private void AgregarOpcion(string texto, Action accion = null)
        {
            ToolStripMenuItem opcion = new ToolStripMenuItem(texto);

            // "(s, e) => ..." es una función corta que se ejecuta al hacer clic.
            opcion.Click += (s, e) =>
            {
                if (accion != null)
                    accion();
                else
                    MessageBox.Show($"'{texto}' estará disponible pronto.");
            };
            _menu.Items.Add(opcion);
        }

        private void CerrarSesion()
        {
            CerroSesion = true;   // avisa a Program.cs que debe volver al login
            Close();              // cierra esta ventana
        }
    }
}
