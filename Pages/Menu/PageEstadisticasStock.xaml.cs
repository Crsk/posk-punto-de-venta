using posk.BLL;
using posk.Controls;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageEstadisticasStock : Page
    {
        public PageEstadisticasStock()
        {
            InitializeComponent();
            Loaded += (se, a) =>
            {
                dgEstadisticasProductos.DataContext = StockBLL.ObtenerDetalleOmitiendoItemsSoloVenta();
                dgEstadisticasMateriaPrima.DataContext = StockmpBLL.ObtenerDetalle();
                dgEstadisticasDetalleCompra.DataContext = null;
                lbCompraDetalle.Content = "";
                CargarCompras();
            };
        }

        private void CargarCompras()
        {
            spCompras.Children.Clear();
            CompraBLL.ObtenerTodas().ForEach(c =>
            {
                var ilc = new ItemLineaCompra() { CompraID = c.id, Usuario = c.usuario, Fecha = c.fecha };
                ilc.spItem.Children.Remove(ilc.btnEliminar);
                ilc.btnVer.Click += (se, e) =>
                {
                    lbCompraDetalle.Content = $"DETALLE COMPRA {c.fecha}:";
                    dgEstadisticasDetalleCompra.DataContext = CompraProductoBLL.Obtener(ilc.CompraID);
                };
                spCompras.Children.Add(ilc);
            });
        }
    }
}
