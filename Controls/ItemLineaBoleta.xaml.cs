using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemLineaBoleta : UserControl
    {

        public int ID { get; set; }
        public producto Producto { get; set; }
        public boleta Boleta { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }

        public ItemLineaBoleta()
        {
            InitializeComponent();

            btnQuitarUnidad.Click += (se, ev) => 
            {
                if (Cantidad > 1)
                {
                    Cantidad--;
                    lbCantidad.Content = $"x{Cantidad}";
                    lbMonto.Content = $"${PrecioUnitario * Cantidad}";
                }
            };
            btnAgregarUnidad.Click += (se, ev) =>
            {
                Cantidad++;
                lbCantidad.Content = $"x{Cantidad}";
                lbMonto.Content = $"${PrecioUnitario * Cantidad}";
            };
        }

        public void UpdateLabels()
        {
            lbCantidad.Content = $"x{Cantidad}";
            lbMonto.Content = $"${Cantidad * PrecioUnitario}";
        }
    }
}
