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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemArriendo : UserControl
    {
        //public arriendo Arriendo { get; set; }
        public producto Producto { get; set; }
        public promocione Promocion { get; set; }
        public cliente Cliente { get; set; }
        public event EventHandler AlClickear;


        public ItemArriendo()
        {
            InitializeComponent();

            btnPendiente.Click += (se, a) => AlClickear?.Invoke(this, null);

            Loaded += (se, ev) =>
            {
                if (Producto != null)
                    lbDetalle.Content = Producto.nombre.ToUpper();
                else if (Promocion != null)
                    lbDetalle.Content = Promocion.nombre.ToUpper();

                //lbFechaInicio.Content = $"{Arriendo.fecha_inicio?.ToShortDateString()} {Arriendo.fecha_inicio?.ToShortTimeString()}";
                //lbFechaTermino.Content = $"{Arriendo.fecha_termino?.ToShortDateString()} {Arriendo.fecha_termino?.ToShortTimeString()}";

                btnPendiente.ToolTip = $"{Cliente.nombre}";
                try
                {
                    if (Producto != null)
                        imageProducto.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + Producto.imagen));
                    else if (Promocion != null)
                        imageProducto.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + Promocion.imagen));
                }
                catch
                {
                    imageProducto.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + "default.jpg"));
                }
                try
                {
                    imageUsuario.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenCliente"] + Cliente.imagen));
                }
                catch
                {
                    imageUsuario.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenCliente"] + "default.jpg"));
                }
            };
        }
    }
}