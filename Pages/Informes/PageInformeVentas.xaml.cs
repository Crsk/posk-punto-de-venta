using posk.BLL;
using System.Windows.Controls;

namespace posk.Pages.Informes
{
    public partial class PageInformeVentas : Page
    {
        public PageInformeVentas()
        {
            InitializeComponent();

            dgVentas.DataContext = InformeBLL.InformeDeVentas();
        }
    }
}
