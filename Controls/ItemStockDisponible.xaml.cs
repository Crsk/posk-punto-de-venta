using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemStockDisponible : UserControl
    {
        public producto Producto { get; set; }
        public materiasprima MateriaPrima { get; set; }
        public decimal? StockDisponible { get; set; }

        public ItemStockDisponible()
        {
            InitializeComponent();
            Loaded += (se, a) => 
            {
                txtProducto.IsEnabled = false;
                txtStockDisponible.IsEnabled = false;
                if (Producto != null)
                    txtProducto.Text = Producto.nombre;
                else if (MateriaPrima != null)
                    txtProducto.Text = MateriaPrima.nombre;

                txtStockDisponible.Text = $"{StockDisponible}";
            };
        }
    }
}
