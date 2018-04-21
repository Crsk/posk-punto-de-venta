using posk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemPromoVentaPedido : UserControl
    {
        //public int IdProductFromThisTicket { get; set; }
        public promocione Promocion { get; set; }

        public Button BotonProducto { get; set; }

        public bool bOverlay { get; set; }
        private SolidColorBrush colorAzul;
        private SolidColorBrush colorVerde;
        private SolidColorBrush colorRojo;

        public bool Seleccionado { get; set; }


        public ItemPromoVentaPedido()
        {
            Promocion = new promocione();

            colorAzul = new SolidColorBrush(Color.FromRgb(2, 95, 201));
            colorVerde = new SolidColorBrush(Color.FromRgb(6, 112, 77));
            colorRojo = new SolidColorBrush(Color.FromRgb(153, 12, 12));

            InitializeComponent();
            BotonProducto = btnProducto;

            Loaded += (se, ev) =>
            {
                lbPromo.Background = colorAzul;

                lbPromo.Content = $"{Promocion.nombre}";

                try
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + Promocion.imagen));
                }
                catch
                {
                    image.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + "default.jpg"));
                }
            };
        }
    }
}
