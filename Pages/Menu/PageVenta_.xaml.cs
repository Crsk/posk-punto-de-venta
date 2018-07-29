using System;
using System.Windows.Controls;
using System.Windows.Media;
using posk.Globals;
using System.Windows.Threading;
using posk.Controls;
using System.Collections.Generic;
using posk.BLL;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using posk.Popup;
using posk.Models;

namespace posk.Pages.Menu
{
    public partial class PageVenta_ : Page
    {
        #region Global
        private List<ItemVentaPedido> listaItemsPedidoEliminarDesdeBD;
        private ItemTeclado teclado;
        public bool bProductosFavToggle { get; set; }
        private SolidColorBrush colorDorado;
        private SolidColorBrush colorNeutro;
        private bool bFavorito;

        private ItemProducto itemProductoSeleccionado;
        private ItemAgregado itemAgregadoUno;
        private ItemAgregado itemAgregadoDos;


        #endregion

        #region Constructor
        public PageVenta_()
        {
            itemProductoSeleccionado = new ItemProducto();
            itemAgregadoUno = new ItemAgregado();
            itemAgregadoDos = new ItemAgregado();
            colorDorado = new SolidColorBrush(Color.FromRgb(255, 150, 0));
            colorNeutro = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            bFavorito = false;
            InitializeComponent();

            AgregadoBLL.ObtenerTodos().ForEach(itemLista =>
            {
                var ia = new ItemAgregado() { Agregado = itemLista };
                ia.btnAgregado.Click += (se, a) =>
                {
                    if (itemAgregadoUno.Agregado == null && itemAgregadoDos.Agregado == null)
                        itemAgregadoUno.Agregado = itemLista;
                    else if (itemAgregadoDos.Agregado == null && itemAgregadoUno.Agregado != null)
                        itemAgregadoDos.Agregado = itemLista;
                    if (itemAgregadoDos.Agregado != null && itemAgregadoUno.Agregado != null && itemProductoSeleccionado.Producto != null)
                        Incluir();
                };
                wrapAgregados.Children.Add(ia);

            });

            teclado = new ItemTeclado(new List<TextBox>() { txtBuscar });
            borderTeclado.Child = teclado;
            InitEvents();
            lbUsuario.Content = $"Garzón: {Settings.NombreUsuario}";
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); // Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (InternetChecker.IsConnectedToInternet())
                {
                    lbFecha.Content = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(46, 139, 87));
                }
                else
                {
                    lbFecha.Content = $"{DateTime.Now}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(252, 52, 52));
                }
            }
            catch (Exception)
            {
                //lbInfo.Content = "ERROR: " + err.Message;
            }
        }


        #endregion

        #region Metodos


        private void MostrarNotificacion(string mensaje, string mensajeSecundario)
        {
            var wmp = new Popup.Notification(mensaje, mensajeSecundario);
            wmp.Opacity = 0;
            wmp.Topmost = true;
            wmp.Show();

            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            wmp.BeginAnimation(OpacityProperty, fadeIn);

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            int i = 0;
            dtClockTime.Tick += (se, a) =>
            {
                i++;
                if (i >= 1)
                {
                    DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                    wmp.BeginAnimation(OpacityProperty, fadeOut);
                    if (i >= 3)
                    {
                        wmp.Close();
                        dtClockTime.Stop();
                    }
                }
            };
            dtClockTime.Start();
        }

        private void MostrarAccion(List<ItemAccion> listaItemsAccion, string titulo)
        {
            var wop = new WindowOpcionesPopup(listaItemsAccion, titulo);
            wop.Opacity = 0;
            wop.Topmost = true;
            wop.Show();

            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            wop.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void Incluir()
        {
            try
            {
                if (ProductoYaSeleccionado() && AgregadosSeleccionados() == 2)
                {
                    string agregadoStr = "";
                    if (itemAgregadoUno.Agregado == itemAgregadoDos.Agregado)
                        agregadoStr = $"{itemAgregadoUno.Agregado.nombre} x2";
                    else
                        agregadoStr = $"{itemAgregadoUno.Agregado.nombre} y {itemAgregadoDos.Agregado.nombre}";
                    MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre}", agregadoStr);

                    var itemVentaExtendido = new ItemVentaPlatoFondo() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado };
                    itemVentaExtendido.btnBorrar.Click += (se, a) =>
                    {
                        spVentaPlatosPrincipales.Children.Remove(itemVentaExtendido);
                    };
                    spVentaPlatosPrincipales.Children.Add(itemVentaExtendido);
                    DeseleccionarProductoAgregado();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void DeseleccionarProductoAgregado()
        {
            itemProductoSeleccionado.Reiniciar();
            itemProductoSeleccionado = new ItemProducto();
            itemAgregadoUno = new ItemAgregado();
            itemAgregadoDos = new ItemAgregado();
            foreach (ItemAgregado ia in wrapAgregados.Children)
            {
                ia.Reiniciar();
            }
        }

        private bool ProductoYaSeleccionado()
        {
            bool b = false;
            foreach (ItemProducto ip in wrapProductos.Children)
            {
                if (ip.Seleccionado)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        private int AgregadosSeleccionados()
        {
            int agregadosSeleccionados = 0;
            foreach (ItemAgregado ia in wrapAgregados.Children)
            {
                if (ia.Estado == 2)
                {
                    agregadosSeleccionados = 2;
                    return agregadosSeleccionados;
                }
                else if (ia.Estado == 1)
                    agregadosSeleccionados++;
            }
            return agregadosSeleccionados;
        }

        private void Limpiar()
        {

        }

        private void MostrarProductos(List<producto> listaProductos)
        {
            try
            {
                wrapProductos.Children.Clear();
                if (listaProductos != null)
                {
                    listaProductos.ForEach(p =>
                    {
                        var ip = new ItemProducto() { Producto = p };
                        wrapProductos.Children.Add(ip);

                        ip.btnProducto.Click += (se2, a2) =>
                        {
                            if (itemProductoSeleccionado != ip)
                            {
                                if (itemProductoSeleccionado != null)
                                    itemProductoSeleccionado.Reiniciar();
                                itemProductoSeleccionado = ip;
                            }
                            if (itemProductoSeleccionado != null && itemAgregadoUno != null && itemAgregadoDos != null)
                                Incluir();
                        };

                        ip.MouseDoubleClick += (se2, a2) =>
                        {
                            if (itemProductoSeleccionado != null)
                                itemProductoSeleccionado.Reiniciar();

                            ItemVentaPedido ivpExistente = spVentaItems.Children.Cast<ItemVentaPedido>().ToList().Where(ivp => ivp.Producto.id == ip.Producto.id).FirstOrDefault();

                            if (ivpExistente != null)
                            {
                                ivpExistente.Cantidad++;
                                ivpExistente.lbCantidad.Content = $"x{ivpExistente.Cantidad}";
                                MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre.ToUpper()}", "+1");
                            }
                            else
                            {
                                var ivpNuevo = new ItemVentaPedido() { Producto = p, Cantidad = 1 };
                                ivpNuevo.AlEliminar += (se3, a3) =>
                                {
                                    spVentaItems.Children.Remove(ivpNuevo);
                                };
                                spVentaItems.Children.Add(ivpNuevo);
                                MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre.ToUpper()}", "");
                            }
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion Metodos

        #region Eventos
        private void InitEvents()
        {
            Loaded += (se, a) =>
            {
                listaItemsPedidoEliminarDesdeBD = new List<ItemVentaPedido>();


                expBottom.IsExpanded = true;
                expLeft.IsExpanded = true;
                CategoriaBLL.ObtenerParaComboBox().ForEach(c =>
                {
                    /////spCategorias.Children.Add(new ItemBuscarPorSector() { Categoria = c });
                });
                spCategorias.Children.Cast<ItemBuscarPorSector>().ToList().ForEach(cbi =>
                {
                    //SubCategoriaBLL.ObtenerPorCategoria(cbi.Categoria.id).ForEach(sc =>
                    //{
                    //    var scbi = new SubCategoriaBuscarItem() { SubCategoria = sc };
                    //    scbi.btnSubCategoria.Click += (se2, a2) =>
                    //    {
                    //        MostrarProductos(SubCategoriaBLL.ObtenerProductos(sc.id));
                    //        bFavorito = false;
                    //        btnFavorito.Foreground = colorNeutro;
                    //    };
                    //    cbi.spItems.Children.Add(scbi);
                    //});
                });
            };

            //btnVolver.Click += (se, a) => MainWindow.MainFrame.Content = new PageMesas();

            btnLimpiarBusqueda.Click += (se, a) =>
            {
                wrapProductos.Children.Clear();
                txtBuscar.Clear();
                bFavorito = false;
                btnFavorito.Foreground = colorNeutro;
            };

            btnBorrarMensaje.Click += (se, a) =>
            {
                tbMensaje.Text = "";
                gridMensaje.Visibility = Visibility.Hidden;
            };

            btnLimpiarVenta.Click += (se, a) =>
            {
                spVentaItems.Children.Clear();
                spVentaPlatosPrincipales.Children.Clear();
            };

            txtBuscar.TextChanged += (se, a) =>
            {
                //if (txtBuscar.Text != "")
                //    MostrarProductos(ProductoBLL.ObtenerCoincidencias(txtBuscar.Text));
            };

            btnExpanderLeft.Click += (se, a) =>
            {
                expLeft.IsExpanded ^= true;
                if (expLeft.IsExpanded) btnExpanderLeft.Foreground = colorDorado;
                else btnExpanderLeft.Foreground = colorNeutro;
            };
            btnExpanderBottom.Click += (se, a) =>
            {
                expBottom.IsExpanded ^= true;
                if (expBottom.IsExpanded) btnExpanderBottom.Foreground = colorDorado;
                else btnExpanderBottom.Foreground = colorNeutro;
            };
            btnFavorito.Click += (se, a) =>
            {
                bFavorito ^= true;
                if (bFavorito)
                {
                    btnFavorito.Foreground = colorDorado;
                    ////MostrarProductos(ProductoBLL.ObtenerFavoritos());
                }
                else
                {
                    btnFavorito.Foreground = colorNeutro;
                    wrapProductos.Children.Clear();
                }
            };

        }
        #endregion Eventos
    }
}
