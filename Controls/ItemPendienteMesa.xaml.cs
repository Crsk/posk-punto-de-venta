using posk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemPendienteMesa : UserControl
    {
        public promocione Promocion { get; set; }

        public string MensajeCocina { get; set; }

        public int Id { get; set; }

        private List<pedido> listaPedidos;
        public List<pedido> ListaPedidos
        {
            get { return listaPedidos; }
            set { listaPedidos = value; }
        }

        private pedido pedido;
        public pedido Pedido
        {
            get { return pedido; }
            set
            {
                pedido = value;
                lbFecha.Content = $"{Pedido.fecha?.ToShortDateString()} {Pedido.fecha?.ToShortTimeString()}";
            }
        }

        private mesa mesa;
        public mesa Mesa
        {
            get { return mesa; }
            set { mesa = value; lbMesa.Content = value?.codigo; }
        }


        private usuario usuario;
        public usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; lbDetalle.Content = value?.nombre.ToString().ToUpper(); }
        }


        public event EventHandler AlClickear;
        public event EventHandler AlEliminar;

        public ItemPendienteMesa()
        {
            InitializeComponent();
            btnPendiente.Click += (se, a) => AlClickear?.Invoke(this, null);
            btnEliminar.Click += (se, a) => AlEliminar?.Invoke(this, null);
            Loaded += (se, ev) =>
            {
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
