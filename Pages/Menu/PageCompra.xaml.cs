using posk.Components;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageCompra : Page
    {
        public PageCompra()
        {
            InitializeComponent();
            gridPrincipal.Children.Add(new PrincipalComponent("STOCK"));
        }
    }
}
