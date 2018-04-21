using posk.BLL;
using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarStock : Page
    {
        private ItemTeclado teclado;
        private List<TextBox> listaItemsTeclado;

        public PageAdministrarStock()
        {
            InitializeComponent();
            listaItemsTeclado = new List<TextBox>();
            Loaded += (se, a) =>
            {
                CargarDisponibilidadProducto();
                CargarDisponibilidadMateriaPrima();
                CargarCompras();
            };
        }

        private void CargarCompraDetalle(int compraID)
        {
            spCompraDetalle.Children.Clear();
            CompraProductoBLL.Obtener(compraID).ForEach(cp => 
            {
                var ilcd = new ItemLineaCompraDetalle() { Producto = cp.producto, CostoUnitario = cp.costo_unitario, CantidadDisponible = cp.cantidad_disponible, CantidadCompra = cp.cantidad_compra };

                ilcd.spItem.Children.Remove(ilcd.btnGuardar);
                     

                ilcd.btnEliminar.Click += (se2, a2) =>
                {
                    CompraProductoBLL.Delete(cp.id);
                    CargarCompraDetalle(compraID);
                };
                ilcd.btnEditar.Click += (se, a) =>
                {
                    ilcd.txtCostoUnitario.IsEnabled = true;
                    ilcd.spItem.Children.Clear();
                    ilcd.spItem.Children.Add(ilcd.btnEliminar);
                    ilcd.spItem.Children.Add(ilcd.borderProducto);
                    ilcd.spItem.Children.Add(ilcd.borderCostoUnitario);
                    ilcd.spItem.Children.Add(ilcd.borderCantidadDisponible);
                    ilcd.spItem.Children.Add(ilcd.borderCantidadCompra);
                    ilcd.spItem.Children.Add(ilcd.btnGuardar);
                };
                ilcd.btnGuardar.Click += (se, a) =>
                {
                    try
                    {
                        CompraProductoBLL.Actualizar(cp.id, Convert.ToDecimal(ilcd.txtCostoUnitario.Text));
                        ilcd.spItem.Children.Clear();
                        ilcd.spItem.Children.Add(ilcd.btnEliminar);
                        ilcd.spItem.Children.Add(ilcd.borderProducto);
                        ilcd.spItem.Children.Add(ilcd.borderCostoUnitario);
                        ilcd.txtCostoUnitario.IsEnabled = false;
                        ilcd.spItem.Children.Add(ilcd.borderCantidadDisponible);
                        ilcd.spItem.Children.Add(ilcd.borderCantidadCompra);
                        ilcd.spItem.Children.Add(ilcd.btnEditar);
                    }
                    catch
                    {
                        MessageBox.Show("No se guardó, intenta denuevo");
                    }
                };



                spCompraDetalle.Children.Add(ilcd);

                listaItemsTeclado.Add(ilcd.txtCostoUnitario);
            });

            teclado = new ItemTeclado(listaItemsTeclado);
            borderTeclado.Child = teclado;
        }

        private void CargarCompras()
        {
            spCompras.Children.Clear();
            CompraBLL.ObtenerTodas().ForEach(c => 
            {
                var ilc = new ItemLineaCompra() { CompraID = c.id, Usuario = c.usuario, Fecha = c.fecha };
                ilc.btnEliminar.Click += (se, a) =>
                {
                    CompraBLL.DeleteById(c.id);
                    CargarCompras();
                };
                ilc.btnVer.Click += (se, e) => CargarCompraDetalle(ilc.CompraID);
                spCompras.Children.Add(ilc);
            });
        }

        private void CargarDisponibilidadMateriaPrima()
        {
            spEstadisticaStockDisponibleMateriaPrima.Children.Clear();
            StockmpBLL.ObtenerTodo().ForEach(smp => 
            {
                var isd = new ItemStockDisponible() { MateriaPrima = smp.materiasprima, StockDisponible = smp.entrada - smp.salida + smp.ajuste};

                isd.spItem.Children.Remove(isd.btnGuardar);

                isd.btnEliminar.Click += (se2, a2) =>
                {
                    StockmpBLL.Eliminar(smp.id);
                    CargarDisponibilidadMateriaPrima();
                };
                isd.btnEditar.Click += (se, a) =>
                {
                    isd.txtStockDisponible.IsEnabled = true;
                    isd.spItem.Children.Clear();
                    isd.spItem.Children.Add(isd.btnEliminar);
                    isd.spItem.Children.Add(isd.borderProducto);
                    isd.spItem.Children.Add(isd.borderstockDisponible);
                    isd.spItem.Children.Add(isd.btnGuardar);
                };
                isd.btnGuardar.Click += (se, a) =>
                {
                    try
                    {
                        StockmpBLL.Actualizar(smp.id, Convert.ToDecimal(isd.txtStockDisponible.Text));
                        isd.txtStockDisponible.IsEnabled = false;
                        isd.spItem.Children.Remove(isd.btnGuardar);
                        isd.spItem.Children.Add(isd.btnEditar);
                    }
                    catch
                    {
                        MessageBox.Show("No se guardó, intenta denuevo");
                    }
                };
                spEstadisticaStockDisponibleMateriaPrima.Children.Add(isd);

                listaItemsTeclado.Add(isd.txtProducto);
                listaItemsTeclado.Add(isd.txtStockDisponible);
            });
            teclado = new ItemTeclado(listaItemsTeclado);
            borderTeclado.Child = teclado;
        }


        private void CargarDisponibilidadProducto()
        {
            spEstadisticaStockDisponibleProducto.Children.Clear();
            StockBLL.ObtenerTodo().ForEach(s =>
            {
                var isd = new ItemStockDisponible() { Producto = s.producto, StockDisponible = s.entrada - s.salida + s.ajuste};

                isd.spItem.Children.Remove(isd.btnGuardar);

                isd.btnEliminar.Click += (se2, a2) =>
                {
                    StockBLL.Eliminar(s.id);
                    CargarDisponibilidadProducto();
                };
                isd.btnEditar.Click += (se, a) =>
                {
                    isd.txtStockDisponible.IsEnabled = true;
                    isd.spItem.Children.Clear();
                    isd.spItem.Children.Add(isd.btnEliminar);
                    isd.spItem.Children.Add(isd.borderProducto);
                    isd.spItem.Children.Add(isd.borderstockDisponible);
                    isd.spItem.Children.Add(isd.btnGuardar);
                };
                isd.btnGuardar.Click += (se, a) => 
                {
                    try
                    {
                        StockBLL.Actualizar(s.id, Convert.ToDecimal(isd.txtStockDisponible.Text));
                        isd.txtStockDisponible.IsEnabled = false;
                        isd.spItem.Children.Remove(isd.btnGuardar);
                        isd.spItem.Children.Add(isd.btnEditar);
                    }
                    catch
                    {
                        MessageBox.Show("No se guardó, intenta denuevo");
                    }
                };
                spEstadisticaStockDisponibleProducto.Children.Add(isd);

                listaItemsTeclado.Add(isd.txtProducto);
                listaItemsTeclado.Add(isd.txtStockDisponible);
            });
            teclado = new ItemTeclado(listaItemsTeclado);
            borderTeclado.Child = teclado;
        }
    }
}
