using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Pages.Menu
{
    public partial class PageMesas : Page
    {
        private SolidColorBrush verde;
        private SolidColorBrush blanco;
        private SolidColorBrush negro;

        public PageMesas()
        {
            InitializeComponent();

            verde = new SolidColorBrush(Color.FromRgb(22, 160, 133));
            blanco = new SolidColorBrush(Color.FromRgb(236, 240, 241));
            negro = new SolidColorBrush(Color.FromRgb(2, 2, 2));

            btnSector1.Click += (se, a) =>
            {
                frameMesas.Content = new PageMesasSectorUno();
                btnSector1.Background = verde;
                btnSector1.BorderBrush = verde;
                btnSector1.Foreground = blanco;
                btnSector2.Background = blanco;
                btnSector2.BorderBrush = blanco;
                btnSector2.Foreground = negro;
            };
            btnSector2.Click += (se, a) =>
            {
                frameMesas.Content = new PageMesasSectorDos();
                btnSector1.Background = blanco;
                btnSector1.BorderBrush = blanco;
                btnSector1.Foreground = negro;
                btnSector2.Background = verde;
                btnSector2.BorderBrush = verde;
                btnSector2.Foreground = blanco;
            };
        }
    }
}
