using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemBuscarPorCategoria : UserControl
    {
        public categoria Categoria { get; set; }

        public ItemBuscarPorCategoria()
        {
            InitializeComponent();

            Loaded += (se, a) =>
                expCategoria.Header = Categoria?.nombre;
        }
    }
}
