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
    public class FormMisUsuarios : Form
    {
        private readonly List<Usuario> _usuarios;
        private DataGridView tabla;
        private Label lblDetalle;

        public FormMisUsuarios(IMisUsuariosControlador controlador, Entrenador entrenador)
        {
            _usuarios = controlador.ObtenerUsuarios(entrenador);
            ConstruirInterfaz(entrenador);
            LlenarTabla();
        }

        private void ConstruirInterfaz(Entrenador entrenador)
        {
            Text = "PowerGym - Mis usuarios";
            ClientSize = new Size(640, 430);
            StartPosition = FormStartPosition.CenterParent;

            // Distingue entrenador personal y del gimnasio (criterio del PBI).
            string tipo = entrenador.Tipo == TipoEntrenador.Personal
                ? "Entrenador personal" : "Entrenador del gimnasio";

            Controls.Add(new Label
            {
                Text = $"{entrenador.Nombre} - {tipo} - {_usuarios.Count} usuarios",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            });


            tabla = new DataGridView
            {
                Location = new Point(15, 50),
                Size = new Size(610, 260),
                ReadOnly = true,                       // no se edita a mano
                AllowUserToAddRows = false,            // sin fila vacía al final
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            tabla.Columns.Add("colNombre", "Nombre");
            tabla.Columns.Add("colUsuario", "Usuario");
            tabla.Columns.Add("colEdad", "Edad");
            tabla.Columns.Add("colPeso", "Peso (kg)");
            tabla.Columns.Add("colImc", "IMC");

            // Se dispara cada vez que cambia la fila seleccionada.
            tabla.SelectionChanged += (s, e) => MostrarDetalle();

            lblDetalle = new Label
            {
                Location = new Point(15, 325),
                Size = new Size(610, 90),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Seleccione un usuario para ver su detalle."
            };

            Controls.Add(tabla);
            Controls.Add(lblDetalle);
        }

        private void LlenarTabla()
        {
            foreach (Usuario u in _usuarios)
                tabla.Rows.Add(u.Nombre, u.NombreUsuario, u.Edad, u.Peso, u.IMC);

            // Sin esto, Windows selecciona la primera fila sola y el detalle no coincide.
            tabla.ClearSelection();
        }

        // El índice de la fila coincide con el de la lista, así que se saca de ahí.
        private void MostrarDetalle()
        {
            if (tabla.SelectedRows.Count == 0)
                return;

            Usuario u = _usuarios[tabla.SelectedRows[0].Index];

            lblDetalle.Text =
                $"Nombre: {u.Nombre}\n" +
                $"Usuario: {u.NombreUsuario}   Edad: {u.Edad}\n" +
                $"Peso: {u.Peso} kg   Estatura: {u.Estatura} m   IMC: {u.IMC}";
        }
    }
}
