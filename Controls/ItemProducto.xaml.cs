using posk.Models;
using System;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemProducto : UserControl
    {
        //public int IdProductFromThisTicket { get; set; }
        public producto Producto { get; set; }

        public event EventHandler AlDeseleccionar;
        public event EventHandler AlSeleccionar;

        public Button BotonProducto { get; set; }

        public bool bOverlay { get; set; }
        private SolidColorBrush colorAzul;
        private SolidColorBrush colorVerde;
        private SolidColorBrush colorRojo;
        private SolidColorBrush colorMorado;
        private SolidColorBrush colorAmarillo;

        public bool Seleccionado { get; set; }

        public bool SeccionAdministrarProducto { get; set; } = false;

        public ItemProducto()
        {
            Producto = new producto();

            colorAzul = new SolidColorBrush(Color.FromRgb(0, 18, 41));
            colorVerde = new SolidColorBrush(Color.FromRgb(6, 112, 77));
            colorRojo = new SolidColorBrush(Color.FromRgb(153, 12, 12));
            colorMorado = new SolidColorBrush(Color.FromRgb(108, 24, 173));
            colorAmarillo = new SolidColorBrush(Color.FromRgb(219, 117, 2));

            InitializeComponent();
            BotonProducto = btnProducto;

            Loaded += (se, ev) => 
            {
                lbPrecio.Content = $"${Producto.precio}";

                if (Producto.contiene_agregado == true && SeccionAdministrarProducto != true && Producto.preparado_especial == false)
                    txtProducto.Background = colorRojo;
                else if (Producto.contiene_agregado == false && SeccionAdministrarProducto != true && Producto.preparado_especial == true)
                {
                    txtProducto.Background = colorMorado;
                }
                else if (Producto.preparado_especial == true && SeccionAdministrarProducto != true)
                {
                    //txtProducto.Background = colorMorado;
                    txtProducto.Background = colorRojo;
                    borderPreparadoEspecial.Visibility = System.Windows.Visibility.Visible;
                }
                else if ((Producto.es_handroll == true || Producto.es_superhandroll == true || Producto.es_envoltura == true) && SeccionAdministrarProducto != true)
                {
                    txtProducto.Background = colorAmarillo;
                }
                else
                    txtProducto.Background = colorVerde;

                txtProducto.Text = $"{Producto.nombre}";

                try
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + Producto.imagen));
                }
                catch
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + "default.jpg"));
                }
            };


            btnProducto.Click += (se, a) => Click();
        }

        private void Click()
        {
                bOverlay ^= true;
                Seleccionado = bOverlay;
                MostrarOverlay(bOverlay);
                if (bOverlay)
                    AlSeleccionar?.Invoke(this, null);
                else
                    AlDeseleccionar?.Invoke(this, null);
        }

        public void Reiniciar()
        {
            Seleccionado = false;
            bOverlay = false;
            MostrarOverlay(false);
        }

        private void MostrarOverlay(bool b)
        {
            if (b && Producto.contiene_agregado == true && SeccionAdministrarProducto != true)
            {
                overlay.Visibility = System.Windows.Visibility.Visible;
                txtProducto.Background = colorRojo;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
            else if (b && Producto.preparado_especial == true && SeccionAdministrarProducto != true)
            {
                overlay.Visibility = System.Windows.Visibility.Visible;
                txtProducto.Background = colorMorado;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
            else if (b && (Producto.es_handroll == true || Producto.es_superhandroll == true | Producto.es_envoltura == true) && SeccionAdministrarProducto != true)
            {
                overlay.Visibility = System.Windows.Visibility.Visible;
                txtProducto.Background = colorAmarillo;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                overlay.Visibility = System.Windows.Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);

                if (Producto.contiene_agregado == true && SeccionAdministrarProducto != true)
                    txtProducto.Background = colorRojo;
                else if (Producto.preparado_especial == true && SeccionAdministrarProducto != true)
                    txtProducto.Background = colorMorado;
                else if ((Producto.es_handroll == true || Producto.es_superhandroll == true || Producto.es_envoltura == true) && SeccionAdministrarProducto != true)
                    txtProducto.Background = colorAmarillo;
                else
                    txtProducto.Background = colorVerde;
            }
        }
    }
}
