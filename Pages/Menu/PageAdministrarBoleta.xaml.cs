using posk.BLL;
using posk.Controls;
using posk.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarBoleta : Page
    {
        public PageAdministrarBoleta()
        {
            InitializeComponent();
            //btnVerPorPeriodo.IsEnabled = false;

            btnVerUltimas.Click += (se, a) =>
            {
                spBoletas.Children.Clear();
                try
                {
                    CargarBoletas(Convert.ToInt32(txtCantidad.Text));
                }
                catch (Exception ex)
                {
                    //PoskException.Make(ex, "ERROR AL VER ULTIMAS BOLETAS");
                }
            };
        }

        private void CargarBoletas(int cantidad)
        {
            try
            {
                BoletaBLL.ObtenerUltimas(cantidad).ForEach(boleta =>
                {
                    ItemBoletaFactura ibf = new ItemBoletaFactura() { NumeroBoleta = boleta.numero_boleta, Total = boleta.total, Cliente = boleta.usuario.nombre, Fecha = boleta.fecha };
                    ibf.btnBorrar.Click += (se2, a2) =>
                    {
                        BoletaBLL.Delete(boleta.id);
                        spBoletas.Children.Clear();
                        CargarBoletas(cantidad);
                    };
                    /*
                    ibf.btnBoletaFactura.Click += (se2, a2) =>
                    {
                        ibf.SpDetalle.Children.Clear();
                        LineaDetalleBLL.ObtenerPorBoletaId(boleta.id).ForEach(detalle =>
                        {
                            ibf.spDetalle.Children.Add(new ItemLineaBoleta() { Producto = detalle.producto, Boleta = boleta, Cantidad = Convert.ToInt32(detalle.cantidad), PrecioUnitario = (int) detalle.producto.precio });
                        });
                    };
                    */
                    spBoletas.Children.Add(ibf);
                });
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL CARGAR BOLETAS");
            }
        }
    }
}
