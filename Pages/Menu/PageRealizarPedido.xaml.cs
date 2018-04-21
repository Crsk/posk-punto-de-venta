using posk.Components;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageRealizarPedido : Page
    {
        public PageRealizarPedido()
        {
            InitializeComponent();
            BuscarProductoComponent bpc = new BuscarProductoComponent("ENVIAR PEDIDO");
            gridPrincipal.Children.Add(bpc);
        }
    }
}
