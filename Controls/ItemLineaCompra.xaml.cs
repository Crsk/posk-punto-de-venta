using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemLineaCompra : UserControl
    {
        public int CompraID { get; set; }
        public usuario Usuario { get; set; }
        public DateTime? Fecha { get; set; }

        public ItemLineaCompra()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                txtUsuario.IsEnabled = false;
                txtFecha.IsEnabled = false;
                txtUsuario.Text = Usuario?.nombre;
                txtFecha.Text = $"{Fecha?.ToShortDateString()} {Fecha?.ToShortTimeString()}";
            };
        }
    }
}
