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
    public partial class ItemSalsa : UserControl
    {
        public salsa Salsa { get; set; }
        public int CobroExtra { get; set; }
        public int Cantidad { get; set; }

        public Color Amarillo { get; set; }
        public Color AmarilloOscuro { get; set; }
        public Color Verde { get; set; }
        public Color VerdeOscuro { get; set; }

        public ItemSalsa()
        {
            InitializeComponent();

            Amarillo = (Color)ColorConverter.ConvertFromString("#FFF26102");
            AmarilloOscuro = (Color)ColorConverter.ConvertFromString("#FFE35403");

            Verde = (Color)ColorConverter.ConvertFromString("#FF0F755C");
            VerdeOscuro = (Color)ColorConverter.ConvertFromString("#FF0A6456");

            Loaded += (se, a) =>
            {
                txtNombre.Text = Salsa.nombre.ToUpper();
                Cantidad = 0;
                lbCantidad.Content = Cantidad;
            };
            btnAgregado.Click += (se, a) =>
            {
                lbCantidad.Content = $"{++Cantidad}";
                PintarCuadrado();
            };
            btnQuitarUnidad.Click += (se, a) => QuitarUnidad();
            borderCantidad.MouseLeftButtonUp += (se, a) => QuitarUnidad();
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
                btnAgregado.Background = new SolidColorBrush(Verde);
                borderCantidad.Visibility = Visibility.Visible;
            }
            else
            {
                lbCantidad.Foreground = new SolidColorBrush(Amarillo);
                btnQuitarUnidad.Background = new SolidColorBrush(AmarilloOscuro);
                btnAgregado.Background = new SolidColorBrush(Amarillo);
                borderCantidad.Visibility = Visibility.Hidden;
            }
        }
    }
}
