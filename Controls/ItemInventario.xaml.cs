using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemInventario : UserControl
    {
        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; lbProducto.Content = value?.nombre; }
        }

        public ItemInventario()
        {
            InitializeComponent();
        }
    }
}
