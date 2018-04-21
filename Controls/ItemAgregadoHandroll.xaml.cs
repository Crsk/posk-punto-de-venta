using posk.Models;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemAgregadoHandroll : UserControl
    {
        public agregado Agregado { get; set; }
        public int CobroExtra { get; set; }
        public int Cantidad { get; set; }

        public ItemAgregadoHandroll(bool bEsconderContador = false)
        {
            InitializeComponent();

            var bc = new BrushConverter();

            Loaded += (se, a) =>
            {

                if (bEsconderContador)
                {
                    borderContador.Visibility = System.Windows.Visibility.Hidden;
                    borderContador.Height = 0;
                    btnQuitarUnidad.Visibility = System.Windows.Visibility.Hidden;
                    btnQuitarUnidad.Width = 0;
                }
                txtNombre.Text = Agregado.nombre;
                Cantidad = 0;
                lbCantidad.Content = Cantidad;
                if (Agregado.es_vegetal == true)
                {
                    btnQuitarUnidad.Background = (Brush)bc.ConvertFrom("#024728");
                    btnAgregado.Background = (Brush)bc.ConvertFrom("#004F2B");
                }
            };
            btnAgregado.Click += (se, a) => lbCantidad.Content = $"{++Cantidad}";
            btnQuitarUnidad.Click += (se, a) => 
            {
                if (Cantidad >= 1)
                    lbCantidad.Content = $"{--Cantidad}";
            };
        }
    }
}
