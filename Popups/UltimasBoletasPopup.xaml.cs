using posk.BLL;
using posk.Controls;
using posk.Models;
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

namespace posk.Popup
{
    public partial class UltimasBoletasPopup : UserControl, IDisposable
    {
        public UltimasBoletasPopup()
        {
            InitializeComponent();
            MostrarUltimasVentas();
        }


        private void ConfigurarItemBoletaFactura(ItemLineaBoleta lb, TextBox tb, ComboBox cb)
        {
            cb.ItemsSource = ProductoBLL.ObtenerTodo();
            cb.DisplayMemberPath = "nombre";

            bool bPausa = false;

            tb.TextChanged += (se2, ev2) =>
            {
                bPausa = true;
                // TODO - cambiar para que reciba el id del productp
                //producto p = ProductoBLL.GetProduct(tb.Text); 
                //if (p != null)
                //{
                //    cb.SelectedItem = p;
                //    lb.Producto = p;
                //    lb.PrecioUnitario = (int)p.precio;
                //    lb.UpdateLabels();
                //}
                //else
                //    cb.SelectedIndex = -1;
                //bPausa = false;
            };
            cb.SelectionChanged += (se2, ev2) =>
            {
                if (!bPausa)
                {
                    producto p = cb.SelectedItem as producto;
                    if (p != null)
                    {
                        tb.Text = p.codigo_barras;
                        lb.Producto = p;
                        lb.PrecioUnitario = (int)p.precio;
                        lb.UpdateLabels();
                    }
                    else
                        tb.Text = "";
                }
            };
        }

        void IDisposable.Dispose()
        {
        }

        private void MostrarUltimasVentas()
        {
            spUltimasBoletas.Children.Clear();
            List<boleta> ultimasBoletas = BoletaBLL.ObtenerUltimas(10);
            foreach (boleta boleta in ultimasBoletas)
            {
                if (boleta.total == 0)
                    BoletaBLL.Delete(boleta.id);
                else
                {
                    ItemBoletaFactura ibf = new ItemBoletaFactura()
                    {
                        ID = boleta.id,
                        Fecha = boleta.fecha,
                        Total = boleta.total,
                        Cliente = boleta.cliente != null ? $"{boleta.cliente.nombre}" : ""
                    };
                    ibf.BtnBorrar.Click += (se, ev) =>
                    {
                        BoletaBLL.Delete(ibf.ID); MostrarUltimasVentas();
                    };
                    ibf.BtnBoletaFactura.Click += (se, ev) =>
                    {
                        //ibf.ExpDetalle.IsExpanded ^= true;
                        ibf.spDetalle.Children.Clear();
                        foreach (detalle_boleta lineaDetalle in LineaDetalleBLL.Get(ibf.ID))
                        {
                            ItemLineaBoleta lb = new ItemLineaBoleta();
                            ConfigurarItemBoletaFactura(lb, lb.txtCodigo, lb.cbProductos);
                            lb.ID = lineaDetalle.id;
                            lb.Boleta = lineaDetalle.boleta;
                            lb.txtCodigo.Text = lineaDetalle.producto.codigo_barras;
                            lb.lbCantidad.Content = $"x{lineaDetalle.cantidad}";
                            lb.lbMonto.Content = $"${lineaDetalle.monto}";
                            lb.Cantidad = (int)lineaDetalle.cantidad;
                            lb.PrecioUnitario = (int)lineaDetalle.producto.precio;

                            lb.btnEliminar.Click += (se2, ev2) =>
                            {
                                ibf.SpDetalle.Children.Remove(lb);
                                LineaDetalleBLL.Delete(lb.ID);
                                MostrarUltimasVentas();
                            };
                            lb.btnGuardar.Click += (se2, ev2) =>
                            {
                                LineaDetalleBLL.Update(lb.ID, lb.Producto.id, lb.Cantidad, lb.Cantidad * lb.PrecioUnitario);
                                MostrarUltimasVentas();
                            };
                            ibf.SpDetalle.Children.Add(lb);
                        }
                    };

                    ItemAgregarLineaBoleta ialb = new ItemAgregarLineaBoleta();
                    // ConfigurarItemBoletaFactura(ialb.TxtCodigo, ialb.CbProductos);
                    ialb.CbProductos.ItemsSource = ProductoBLL.ObtenerTodo();
                    ialb.CbProductos.DisplayMemberPath = "nombre";

                    bool bPausa = false;
                    producto p = new producto();
                    int precioUnitario = 1;
                    ialb.TxtCodigo.TextChanged += (se2, ev2) =>
                    {
                        bPausa = true;
                        // TODO - cambiar para que reciba el id del producto
                        //p = ProductoBLL.GetProduct(ialb.TxtCodigo.Text);
                        //if (p != null)
                        //{
                        //    ialb.CbProductos.SelectedItem = p;
                        //    ialb.PrecioUnitario = (int)p.precio;
                        //    ialb.UpdateLabels();
                        //}
                        //else
                        //    ialb.CbProductos.SelectedIndex = -1;
                        //bPausa = false;
                    };
                    ialb.CbProductos.SelectionChanged += (se2, ev2) =>
                    {
                        if (!bPausa)
                        {
                            p = ialb.CbProductos.SelectedItem as producto;
                            if (p != null)
                            {
                                ialb.TxtCodigo.Text = p.codigo_barras;
                                ialb.PrecioUnitario = (int)p.precio;
                                ialb.UpdateLabels();
                            }
                            else
                                ialb.TxtCodigo.Text = "";
                        }
                    };
                    ialb.Producto = p;
                    ialb.Boleta = boleta;
                    ialb.Cantidad = 1;
                    ialb.BtnAgregar.Click += (se, ev) =>
                    {
                        if (ialb.CbProductos.SelectedIndex != -1 && ialb.Boleta != null && ialb.Cantidad != 0)
                        {
                            LineaDetalleBLL.Set(p, ialb.Boleta, ialb.Cantidad);
                            MostrarUltimasVentas();
                        }
                    };
                    ibf.SpContenidoPrincipal.Children.Add(ialb);
                    spUltimasBoletas.Children.Add(ibf);
                };
            }
        }
    }
}
