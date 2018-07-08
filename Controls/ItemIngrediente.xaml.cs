using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemIngrediente : UserControl
    {
        private ingrediente ingrediente;

        public ingrediente Ingrediente
        {
            get { return ingrediente; }
            set { ingrediente = value; txtNombre.Text = value.nombre.ToUpper(); }
        }

        private int cantidad;

        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; lbCantidad.Content = value.ToString(); }
        }

        public event EventHandler AlCambiarEstado;

        public ItemIngrediente()
        {
            InitializeComponent();

            btnIngrediente.Click += (se, a) =>
            {
                Cantidad++;
                lbCantidad.Content = $"{Cantidad}";
                AlCambiarEstado?.Invoke(this, null);
            };
            btnQuitarUnidad.Click += (se, a) =>
            {
                QuitarUnidad();
                AlCambiarEstado?.Invoke(this, null);
            };
            borderCantidad.MouseLeftButtonUp += (se, a) =>
            {
                QuitarUnidad();
                AlCambiarEstado?.Invoke(this, null);
            };
        }

        public void QuitarUnidad()
        {
            if (Cantidad >= 1)
                lbCantidad.Content = $"{--Cantidad}";
        }
    }
}
