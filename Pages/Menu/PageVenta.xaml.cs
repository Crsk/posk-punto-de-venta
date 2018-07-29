using posk.Components;
using posk.Models;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageVenta : Page
    {
        public PageVenta(usuario u, mesa m)
        {
            InitializeComponent();
            gridPrincipal.Children.Add(new PrincipalComponent("VENTA", u, m));
        }
    }
}
