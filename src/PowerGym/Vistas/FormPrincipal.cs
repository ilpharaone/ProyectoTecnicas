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
    public class FormPrincipal:Form
    {
        private readonly Persona _persona;
        private MenuStrip _menu;
        private readonly ContenedorControladores _controladores;
        public bool CerroSesion { get; private set; }
        public FormPrincipal(Persona persona,ContenedorControladores controladores)
        {
            _persona = persona;
            _controladores = controladores;
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
            _menu.BringToFront();
        }

        private void AgregarOpcionUsuario()
        {
            AgregarOpcion("Mis rutinas", () => AbrirInformacion(0));
            AgregarOpcion("Mis visitas", () => AbrirInformacion(1));
            AgregarOpcion("Mis mediciones", () => AbrirInformacion(2));
            AgregarOpcion("Control de aumentos", () => AbrirInformacion(3));
            AgregarOpcion("Clases", AbrirClase);
            AgregarOpcion("Insignias", AbrirInsignias);
            AgregarOpcion("Notificaciones", AbrirNotificaciones);
            AgregarOpcion("Reportes");
        }

        private void AgregarOpcionEntrenador()
        {
            AgregarOpcion("Mis usuarios", AbrirMisUsuarios);
            AgregarOpcion("Rutinas",AbrirRutinas);
            AgregarOpcion("Clases",AbrirClase);
            AgregarOpcion("Notificaciones",AbrirNotificaciones);
            
        }

        private void AbrirInformacion(int pestana)
        {
            using (var ventana = new FormInformacion(_controladores.Aumentos,_controladores.Informacion, 
                (Usuario)_persona,pestana))
            {
                ventana.ShowDialog(this);
            }
        }
        private void AbrirMisUsuarios()
        {
            using(var ventana = new FormMisUsuarios(_controladores.MisUsuarios, (Entrenador)_persona))
            {
                ventana.ShowDialog(this);
            }
        }

        private void AbrirRutinas()
        {
            using (var ventana = new FormRutina(_controladores.Rutinas, (Entrenador)_persona))
            {
                ventana.ShowDialog(this);
            }
        }

        private void AbrirClase()
        {
            using (var ventana = new FormClases(_controladores.Clases, _persona))
            {
                ventana.ShowDialog(this);
            }
        }

        private void AbrirInsignias()
        {
            using (var ventana = new FormInsignias(_controladores.Insignias, (Usuario)_persona))
            {
                ventana.ShowDialog(this);
            }
        }

        private void AbrirNotificaciones()
        {
            using (var ventana = new FormNotificaciones(_controladores.Notificaciones, _persona)) {

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
