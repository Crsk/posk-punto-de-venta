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
        private int precio;

        public int Precio
        {
            get { return precio; }
            set
            {
                precio = value;
            }
        }

        private ingrediente ingrediente;

        public ingrediente Ingrediente
        {
            get { return ingrediente; }
            set
            {
                ingrediente = value;
                txtNombre.Text = value.nombre.ToUpper();
                if (value.precio != 0)
                {
                    lbPrecio.Content = $"${value.precio}";
                    borderPrecio.Visibility = Visibility.Visible;
                }
            }
        }

        public int Cantidad { get; set; }

        public event EventHandler AlCambiarEstado;

        public Color Amarillo { get; set; }
        public Color AmarilloOscuro { get; set; }
        public Color Verde { get; set; }
        public Color VerdeOscuro { get; set; }


        public ItemIngrediente()
        {
            InitializeComponent();

            Amarillo = (Color)ColorConverter.ConvertFromString("#FFF26102");
            AmarilloOscuro = (Color)ColorConverter.ConvertFromString("#FFE35403");

            Verde = (Color)ColorConverter.ConvertFromString("#FF0F755C");
            VerdeOscuro = (Color)ColorConverter.ConvertFromString("#FF0A6456");

            btnIngrediente.Click += (se, a) =>
            {
                Cantidad++;
                PintarCuadrado();
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
            PintarCuadrado();
        }

        private void PintarCuadrado()
        {
            lbCantidad.Content = $"{Cantidad}";
            if (Cantidad > 0)
            {
                lbCantidad.Foreground = new SolidColorBrush(Verde);
                btnQuitarUnidad.Background = new SolidColorBrush(VerdeOscuro);
                btnIngrediente.Background = new SolidColorBrush(Verde);
                borderCantidad.Visibility = Visibility.Visible;
            }
            else
            {
                lbCantidad.Foreground = new SolidColorBrush(Amarillo);
                btnQuitarUnidad.Background = new SolidColorBrush(AmarilloOscuro);
                btnIngrediente.Background = new SolidColorBrush(Amarillo);
                borderCantidad.Visibility = Visibility.Hidden;
            }
        }
    }
}
