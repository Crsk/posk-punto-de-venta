using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemMesa_pedido : UserControl
    {
        private mesa mesa;

        public mesa Mesa
        {
            get { return mesa; }
            set { mesa = value; lbMesa.Content = (value as mesa)?.codigo; }
        }

        public ItemMesa_pedido()
        {
            InitializeComponent();
        }
    }
}
