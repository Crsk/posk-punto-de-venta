using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class PromoItem : UserControl
    {
        public producto Producto { get; set; }

        public PromoItem()
        {
            InitializeComponent();
        }
    }
}
