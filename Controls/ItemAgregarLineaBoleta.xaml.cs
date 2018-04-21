using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemAgregarLineaBoleta : UserControl
    {
        public TextBox TxtCodigo { get; set; }
        public ComboBox CbProductos { get; set; }
        public Button BtnQuitarUnidad { get; set; }
        public Label LbCantidad { get; set; }
        public Button BtnAgregarCantidad { get; set; }
        public Label LbMonto { get; set; }
        public Button BtnAgregar { get; set; }

        public producto Producto { get; set; }
        public boleta Boleta { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnitario { get; set; }

        public ItemAgregarLineaBoleta()
        {
            InitializeComponent();
            TxtCodigo = txtCodigo;
            CbProductos = cbProductos;
            BtnQuitarUnidad = btnQuitarUnidad;
            LbCantidad = lbCantidad;
            BtnAgregarCantidad = btnAgregarUnidad;
            LbMonto = lbMonto;
            BtnAgregar = btnAgregar;

            Loaded += (se, ev) => 
            {
                LbCantidad.Content = $"x{Cantidad}";
                LbMonto.Content = $"";
            };

            btnQuitarUnidad.Click += (se, ev) =>
            {
                if (Cantidad > 1)
                {
                    Cantidad--;
                    lbCantidad.Content = $"x{Cantidad}";
                    lbMonto.Content = PrecioUnitario != 1 ? $"${PrecioUnitario * Cantidad}" : "";
                }
            };
            btnAgregarUnidad.Click += (se, ev) =>
            {
                Cantidad++;
                lbCantidad.Content = $"x{Cantidad}";
                lbMonto.Content = PrecioUnitario != 1 ? $"${PrecioUnitario * Cantidad}" : "";
            };
        }

        public void UpdateLabels()
        {
            LbCantidad.Content = $"x{Cantidad}";
            LbMonto.Content = $"${Cantidad*PrecioUnitario}";
        }
    }
}
