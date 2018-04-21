using posk.Components;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageMerma : Page
    {
        public PageMerma()
        {
            InitializeComponent();
            gridPrincipal.Children.Add(new PrincipalComponent("MERMA"));
        }
    }
}
