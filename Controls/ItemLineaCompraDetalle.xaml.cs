using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemLineaCompraDetalle : UserControl
    {
        public producto Producto { get; set; }
        public decimal? CostoUnitario { get; set; }
        public decimal? CantidadDisponible { get; set; }
        public decimal? CantidadCompra { get; set; }

        public ItemLineaCompraDetalle()
        {
            InitializeComponent();

            Loaded += (se, e) =>
            {
                txtProducto.IsEnabled = false;
                txtCostoUnitario.IsEnabled = false;
                txtCantidadDisponible.IsEnabled = false;
                txtCantidadCompra.IsEnabled = false;
                txtProducto.Text = Producto?.nombre;
                txtCostoUnitario.Text = $"{CostoUnitario}";
                txtCantidadDisponible.Text = $"{CantidadDisponible}";
                txtCantidadCompra.Text = $"{CantidadCompra}";
            };
        }
    }
}
