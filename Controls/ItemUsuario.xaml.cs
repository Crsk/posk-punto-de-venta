using System;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemUsuario : UserControl
    {
        public int ID { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }
        public string Pass { get; set; }
        public string Tipo { get; set; }
        public bool Favorito { get; set; }
        public string Imagen { get; set; }
        public string RutaImagen { get; set; }
        public Button BotonUsuario { get; set; }
        public bool Seleccionado { get; set; }

        public ItemUsuario()
        {
            InitializeComponent();
            BotonUsuario = btnUsuario;

            Loaded += (se, ev) =>
            {
                lbUsuario.Content = $"{Nombre}";

                try
                {
                    RutaImagen = ConfigurationManager.AppSettings["RutaImagenUsuario"] + Imagen;
                    image.Source = new BitmapImage(new Uri(RutaImagen));
                }
                catch
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenUsuario"] + "default.jpg"));
                }
            };

            btnUsuario.Click += (se, a) => 
            {
                OverlayToggle();
            };
        }
        private void OverlayToggle()
        {
            Seleccionado ^= true;
            if (Seleccionado)
            {
                overlay.Visibility = System.Windows.Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                overlay.Visibility = System.Windows.Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
        }
    }
}