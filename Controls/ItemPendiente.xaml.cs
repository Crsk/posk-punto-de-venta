using posk.Models;
using System;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemPendiente : UserControl
    {
        public pendiente Pendiente { get; set; }
        public producto Producto { get; set; }
        public promocione Promocion { get; set; }
        public usuario Usuario { get; set; }
        public event EventHandler AlClickear;


        public ItemPendiente()
        {
            InitializeComponent();

            btnPendiente.Click += (se, a) => AlClickear?.Invoke(this, null);

            Loaded += (se, ev) =>
            {
                if (Producto != null)
                    lbDetalle.Content = Producto.nombre.ToUpper();
                else if (Promocion != null)
                    lbDetalle.Content = Promocion.nombre.ToUpper();

                lbFecha.Content = $"{Pendiente.fecha.ToShortDateString()} {Pendiente.fecha.ToShortTimeString()}";

                if (Producto != null)
                    lbPrecio.Content = $"${Producto.precio}";
                else if (Promocion != null)
                    lbPrecio.Content = $"${Promocion.precio}";

                btnPendiente.ToolTip = $"{Usuario.nombre}";
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
                    imageUsuario.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenUsuario"] + Usuario.imagen));
                }
                catch
                {
                    imageUsuario.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenUsuario"] + "default.jpg"));
                }
            };
        }
    }
}