using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace posk.Controls
{
    public partial class ItemDelivery : UserControl
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public string NombreCliente { get; set; }
        public cliente Cliente { get; set; }
        public string Comentario { get; set; }
        public string Incluye { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime FechaPedido { get; set; }
        public boleta Boleta { get; set; }
        public delivery_item DeliveryItem { get; set; }
        private TimeSpan span;
        private SolidColorBrush color;
        private Byte rojo;

        public ItemDelivery()
        {
            InitializeComponent();
            color = new SolidColorBrush(Color.FromRgb(0, 40, 0));
            Loaded += (se, a) =>
            {
                lbNombre.Content = $"{NombreCliente}";
                span = DateTime.Now.Subtract(FechaPedido);
                // indica hace cuantos minútos se hizo el pedido
                lbMinutosTranscurridos.Content = span.TotalMinutes.ToString("0.0") + " min";
            };
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); // Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                span = DateTime.Now.Subtract(FechaPedido);
                // colorear a medida que pasa el tiempo
                if (span.TotalMinutes < 1)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(15,53,130));
                else if (span.TotalMinutes >= 1 && span.TotalMinutes < 5)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(11, 157, 193));
                else if (span.TotalMinutes >= 5 && span.TotalMinutes < 10)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(6, 155, 90));
                else if (span.TotalMinutes >= 10 && span.TotalMinutes < 15)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(90, 150, 12));
                else if (span.TotalMinutes >= 15 && span.TotalMinutes < 20)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(188, 139, 3));
                else if (span.TotalMinutes >= 20 && span.TotalMinutes < 25)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(255, 84, 0));
                else if (span.TotalMinutes >= 25)
                    lbMinutosTranscurridos.Foreground = new SolidColorBrush(Color.FromRgb(150, 0, 0));

                lbMinutosTranscurridos.Content = span.TotalMinutes.ToString("0.0") + " min";
            }
            catch (Exception)
            {
            }
        }
    }
}
