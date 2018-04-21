using posk.Models;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemCliente : UserControl
    {
        public cliente Cliente { get; set; }

        public ItemCliente()
        {
            InitializeComponent();

            Loaded += (se, ev) =>
            {
                lbNombre.Content = $"{Cliente.nombre}";
                try
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenCliente"] + Cliente.imagen));
                }
                catch
                {
                    image.Source = new BitmapImage(new Uri("http://www.lacasadelasnavajas.com/img/cms/recursos/producto-exclusivo.png"));
                }
            };
        }
    }
}
