using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;


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
        public boleta Boleta { get; set; }
        public delivery_item DeliveryItem { get; set; }

        public ItemDelivery()
        {
            InitializeComponent();
            Loaded += (se, a) =>
            {
                lbNombre.Content = $"{NombreCliente}";
                lbServirLlevar.Content = DeliveryItem.servir == true ? "S" : "L";
            };
        }
    }
}
