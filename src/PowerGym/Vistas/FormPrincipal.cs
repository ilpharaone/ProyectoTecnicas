using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGym.Modelos;

namespace PowerGym.Vistas
{
    class FormPrincipal:Form
    {
        public FormPrincipal(Persona persona)
        {
            string rol = persona is Entrenador ? "Entrenador" : "Usuario";
            Text = $"PowerGym - {persona.Nombre} ({rol})";
            ClientSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label
            {
                Text = $"Hola, {persona.Nombre}. Entró como {rol}.",
                AutoSize = true,
                Location = new Point(20, 20)
            });
        }
    }
}
