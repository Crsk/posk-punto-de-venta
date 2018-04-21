using posk.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemAgregado : UserControl
    {
        public agregado Agregado { get; set; }
        public int CobroExtra { get; set; }

        public int Estado { get; set; }
        private SolidColorBrush colorRojo;
        private SolidColorBrush colorVerde;
        private SolidColorBrush colorVerdeOscuro;

        public ItemAgregado()
        {
            Estado = 0;
            InitializeComponent();

            Loaded += (se, a) =>
            {
                btnAgregado.Content = Agregado.nombre.ToUpper();
                btnAgregado.FontSize = Agregado.font_size;
            };

            colorRojo = new SolidColorBrush(Color.FromRgb(153, 12, 12));
            colorVerde = new SolidColorBrush(Color.FromRgb(6, 112, 17));
            colorVerdeOscuro = new SolidColorBrush(Color.FromRgb(6, 82, 17));

            btnAgregado.Click += (se, a) => 
            {
                switch (Estado)
                {
                    case 0: // rojo pasa a verde
                        btnAgregado.Background = colorVerde;
                        Estado = 1;
                        break;
                    case 1: // verde pasa a verde oscuro
                        btnAgregado.Background = colorVerdeOscuro;
                        Estado = 2;
                        break;
                    case 2: // verde oscuro pasa a rojo
                        btnAgregado.Background = colorRojo;
                        Estado = 0;
                        break;
                    default:
                        btnAgregado.Background = colorRojo;
                        Estado = 0;
                        break;
                }
            };
        }

        public void Reiniciar()
        {
            btnAgregado.Background = colorRojo;
            Estado = 0;
        }
    }
}
