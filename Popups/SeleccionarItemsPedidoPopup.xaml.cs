using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace posk.Popups
{
    public partial class SeleccionarItemsPedidoPopup : Window
    {
        public event EventHandler<List<pedidos_productos>> AlIngresarVenta;
        public event EventHandler<List<pedidos_productos>> AlPedirCuenta;
        public event EventHandler<List<pedidos_productos>> AlRetornarLista;
        public event EventHandler<string[]> AlIngresar2;
        public event EventHandler<int[]> AlActualizar;

        public bool bClosing { get; set; }

        public SeleccionarItemsPedidoPopup(pedido ped)
        {
            InitializeComponent();

            Closing += (se, a) => bClosing = true;
            Deactivated += (se, a) => { if (!bClosing) Close(); };

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            List<pedido> listaPed = PedidoBLL.ObtenerPedidosPorMesa(ped.mesa_id);


            lbPedidoId.Content = $"Pedido ID: {ped.id}";
            lbPedidoFecha.Content = ped.fecha;

            cbGarzon.ItemsSource = UsuarioBLL.ObtenerGarzones();
            cbGarzon.DisplayMemberPath = "nombre";
            cbGarzon.Text = ped.usuario?.nombre;

            cbMesa.ItemsSource = MesaBLL.ObtenerTodas();
            cbMesa.DisplayMemberPath = "codigo";
            cbMesa.Text = ped?.mesa?.codigo;

            btnActualizarPedido.Click += (se, a) =>
            {

                List<ItemSeleccionarPendienteParaVenta> listaItems = spItems.Children.OfType<ItemSeleccionarPendienteParaVenta>().Where(x => x.Seleccionado == true).ToList();
                List<pedidos_productos> listaPP = new List<pedidos_productos>();
                listaItems.ForEach(x => listaPP.Add(x.PedidoProducto));

                AlRetornarLista.Invoke(this, listaPP);

                int[] args = new int[2] { (cbGarzon.SelectedItem as usuario).id, (cbMesa.SelectedItem as mesa).id };
                AlActualizar.Invoke(this, args);
            };

            listaPed.ForEach(p =>
            {
                PedidosProductosBLL.ObtenerTodos(p.id).ForEach(pp =>
                {
                    //var isppv = new ItemSeleccionarPendienteParaVenta()
                    spItems.Children.Add(new ItemSeleccionarPendienteParaVenta() { PedidoProducto = pp, Producto = pp.producto, Promo = pp.promocione, Seleccionado = true, Precio = pp.precio });
                });
            });

            btnIngresarVenta.Click += (se, a) =>
            {
                List<ItemSeleccionarPendienteParaVenta> listaItems = spItems.Children.OfType<ItemSeleccionarPendienteParaVenta>().Where(x => x.Seleccionado == true).ToList();
                List<pedidos_productos> listaPP = new List<pedidos_productos>();
                listaItems.ForEach(x => listaPP.Add(x.PedidoProducto));
                //if (listaItems.Count == spItems.Children.Count)
                AlIngresarVenta.Invoke(this, listaPP);
                string[] garzonMesa = new string[2];
                garzonMesa[0] = $"{ped.usuario?.nombre}";
                garzonMesa[1] = $"{ped.mesa.codigo}";
                AlIngresar2.Invoke(this, garzonMesa);
                //else
                //    AlIngresar.Invoke(this, Descomponer(listaPP));
            };

            btnPedirCuenta.Click += (se, a) =>
            {
                List<ItemSeleccionarPendienteParaVenta> listaItems = spItems.Children.OfType<ItemSeleccionarPendienteParaVenta>().Where(x => x.Seleccionado == true).ToList();
                List<pedidos_productos> listaPP = new List<pedidos_productos>();
                listaItems.ForEach(x => listaPP.Add(x.PedidoProducto));
                AlPedirCuenta.Invoke(this, listaPP);
            };

        }

        private List<pedidos_productos> Descomponer(List<pedidos_productos> listaPP)
        {
            List<pedidos_productos> listaPP_Descompuesta = new List<pedidos_productos>();
            listaPP.ForEach(pp =>
            {
                try
                {
                    int cantidad = Convert.ToInt32(pp.cantidad);
                    if (cantidad > 1)
                    {
                        for (int i = 0; i < cantidad; i++)
                        {
                            pp.cantidad = 1;
                            listaPP_Descompuesta.Add(pp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al descomponer lista");
                }
            });
            return listaPP_Descompuesta;
        }
    }
}
