using System;
using System.Collections.Generic;
using System.Windows.Controls;
using posk.Models;
using posk.BLL;
using posk.Globals;
using posk.Controls;

namespace posk.Components
{
    public partial class BoletaVentaComponent : UserControl, IDisposable
    {
        public BoletaVentaComponent(List<ItemVenta> listaItemsVenta, TextBox txtClient_rut)
        {
            InitializeComponent();

            cliente cliente = ClienteBLL.GetClient(txtClient_rut.Text);
            txtNumeroBoleta.TextChanged += (se2, ev2) =>
            {
                lbBoletaFactura.Content = txtNumeroBoleta.Text != "" ? "BOLETA N°" : "";
                lbNumeroBoleta.Content = txtNumeroBoleta.Text;
                txtNumeroFactura.Clear();
            };
            txtNumeroFactura.TextChanged += (se2, ev2) =>
            {
                lbBoletaFactura.Content = txtNumeroFactura.Text != "" ? "FACTURA N°" : "";
                lbNumeroBoleta.Content = txtNumeroFactura.Text;
                txtNumeroBoleta.Clear();
            };
            datos_negocio dn = DatosNegocioBLL.GetDatosNegocio();
            if (dn != null)
            {
                lbNombreNegocio.Content = dn.nombre;
                lbDescripcionNegocio.Content = dn.mensaje;
                lbDireccionNegocio.Content = dn.direccion;
            }
            else
            {
                lbNombreNegocio.Content = "";
                lbDescripcionNegocio.Content = "";
                lbDireccionNegocio.Content = "";
            }
            lbFechaBoleta.Content = $"{DateTime.Now.ToShortDateString().Replace('-', '/')} {DateTime.Now.ToShortTimeString()}";

            if (cliente != null)
            {
                string nombreSinApellido = cliente.nombre;
                int x = cliente.nombre.IndexOf(" ");
                if (x > 0)
                {
                    nombreSinApellido = cliente.nombre.Substring(0, x);
                }
                lbClienteMsg1.Content = $"{nombreSinApellido}";
                // TODO - descomentar estas dos lineas cuando libere el sistema de puntos
                // lbClienteMsg2.Content = $"Acabas de acumular {VentaActual_puntos} puntos";
                // lbClienteMsg3.Content = $"Tienes {cliente.punto.puntos_activos + VentaActual_puntos} puntos, expiran el {DateTime.Now.Date.AddDays(30).ToString("dd/MM/yyyy")}";
            }
            else
            {
                spClienteMsg.Children.Clear();
            }

            var total = 0;
            foreach (ItemVenta item in listaItemsVenta)
            {
                VentaDetailLineControl plc = new VentaDetailLineControl()
                {
                    Desc = $"{item.Producto.nombre}          x{item.Cantidad}",
                    Valor = Convert.ToInt32(item.Cantidad * item.Producto.precio),
                    ToolTip = $"${item.Producto.precio} C/U"
                };
                spDetalleBoleta.Children.Add(plc);
                total += plc.Valor;
            }
            lbTotalBoleta.Content = total;
        }

        void IDisposable.Dispose() { }
    }
}
