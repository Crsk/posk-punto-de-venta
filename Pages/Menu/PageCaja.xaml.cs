using posk.Globals;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageCaja : Page
    {
        public PageCaja()
        {
            InitializeComponent();
            btnCajaToggle.Click += (se, ev) => 
            {
                if (btnCajaToggle.Content.Equals("ABRIR CAJA"))
                {
                    Settings.bAperturaDeCajaRealizada = true;
                    btnCajaToggle.Content = "CERRAR CAJA";
                }
                else
                {
                    Settings.bAperturaDeCajaRealizada = false;
                    btnCajaToggle.Content = "ABRIR CAJA";
                }
            };
        }
    }
}
