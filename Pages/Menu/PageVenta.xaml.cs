using posk.Components;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageVenta : Page
    {
        public PageVenta()
        {
            InitializeComponent();
            gridPrincipal.Children.Add(new PrincipalComponent("VENTA"));
        }
    }
}
