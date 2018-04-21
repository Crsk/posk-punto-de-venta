using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemBuscarPorCategoriaSeleccionable : UserControl
    {
        public categoria Categoria { get; set; }

        public ItemBuscarPorCategoriaSeleccionable()
        {
            InitializeComponent();

            Loaded += (se, a) =>
                expCategoria.Header = Categoria?.nombre;
        }
    }
}
