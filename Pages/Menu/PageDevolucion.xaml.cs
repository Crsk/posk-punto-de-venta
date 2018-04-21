using posk.Components;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageDevolucion : Page
    {
        public PageDevolucion()
        {
            InitializeComponent();
            gridPrincipal.Children.Add(new PrincipalComponent("DEVOLUCION"));
        }
    }
}
