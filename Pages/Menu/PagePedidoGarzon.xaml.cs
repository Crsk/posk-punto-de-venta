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
    public partial class PagePedidoGarzon : Page
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

        private mesa mesa;

        #endregion

        #region Constructor
        public PagePedidoGarzon(mesa _mesa)
        {
            mesa = _mesa;
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

                if (mesa.libre == false)
                {
                    pedido p = PedidoBLL.Obtener(mesa.id);

                    if (p != null)
                    {
                        List<pedidos_productos> listaPP = PedidosProductosBLL.ObtenerTodos(p.id);
                        foreach (pedidos_productos pp in listaPP)
                        {
                            //pedidos_agregados pa = PedidosAgregadosBLL.Obtener(pp.id);
                            //if (pa != null)
                            //{
                            //    List<agregado> listaAgregados = PedidosAgregadosBLL.ObtenerAgregados(pp.id);
                            //    agregado agregadoUno = new agregado(), agregadoDos = new agregado();
                            //    int i = 0;
                            //    foreach (agregado ag in listaAgregados)
                            //    {
                            //        if (i == 0)
                            //            agregadoUno = ag;
                            //        else
                            //            agregadoDos = ag;
                            //        i++;
                            //    }
                            //    spVentaPlatosPrincipales.Children.Add(new ItemVentaPlatoFondo() { Producto = pp.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos });
                            //}
                            //else
                            //{
                                var ivp = new ItemVentaPedido() { PedidoProductoID = pp.id, Cantidad = pp.cantidad, Producto = pp.producto };
                                ivp.AlEliminar += (se2, a2) =>
                                {
                                    spVentaItems.Children.Remove(ivp);
                                    listaItemsPedidoEliminarDesdeBD.Add(ivp);
                                };
                                spVentaItems.Children.Add(ivp);
                            //}
                        }
                    }
                }
                //if (mesa.libre == true)
                //{
                //    PedidosProductosBLL.ObtenerTodos((PedidoBLL.Obtener(mesa.id)).id).ForEach(pp => 
                //    {
                //        if (PedidosAgregadosBLL.Obtener(pp.id) != null)
                //        {
                //            agregado agregadoUno = new agregado(), agregadoDos = new agregado();
                //            int i = 0;
                //            PedidosAgregadosBLL.ObtenerAgregados(pp.id).ForEach(ag => 
                //            {
                //                if (i == 0)
                //                    agregadoUno = ag;
                //                else
                //                    agregadoDos = ag;
                //                i++;
                //            });
                //            spVentaItems.Children.Add( new ItemVentaPlatoFondo() { Producto = pp.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos });
                //        }
                //        else
                //        {
                //            var ivp = new ItemVentaPedido() { Cantidad = pp.cantidad, Producto = pp.producto };
                //            ivp.AlEliminar += (se2, a2) => { spVentaItems.Children.Remove(ivp); };
                //            spVentaItems.Children.Add(ivp);
                //        }
                //    });
                //}

                expBottom.IsExpanded = true;
                expLeft.IsExpanded = true;
                borderMesa.Child = new ItemMesa_pedido() { Mesa = mesa };
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

            btnVolver.Click += (se, a) => MainWindow.MainFrame.Content = new PageMesas();

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

            btnEnviar.Click += (se, a) =>
            {
                pedido ped = new pedido();
                mesa.usuario = Settings.Usuario;
                ped = PedidoBLL.Obtener(mesa.id);
                if (ped == null)
                    ped = PedidoBLL.Crear(Settings.Usuario, DateTime.UtcNow, tbMensaje.Text, mesa.id);

                // platos de fondo con sus agregados
                List<ItemVentaPlatoFondo> listaPlatosFondo = spVentaPlatosPrincipales.Children.Cast<ItemVentaPlatoFondo>().ToList();
                if (listaPlatosFondo.Count != 0)
                {
                    listaPlatosFondo.ForEach(ive =>
                    {
                        //pedidos_productos pedido_producto = PedidosProductosBLL.Crear(ped.id, ive.Producto?.id, ive.Promo?.id, ); // a los platos de fondo no se les puede aumentar la cantidad
                        //PedidosAgregadosBLL.Crear(pedido_producto.id, ive.AgregadoUno.id, ive.AgregadoDos.id);
                        //PedidosAgregadosBLL.Crear(pedido_producto.id, ive.AgregadoDos.id, ive.AgregadoDos.id);
                    });
                }


                string listaProductosImprimir_Nuevo = "\nNUEVO:\n";
                string listaProductosImprimir_Agregar = "\nAGREGAR:\n";
                string listaProductosImprimir_Quitar = "\nQUITAR UNIDAD(ES):\n";
                string listaProductosImprimir_QuitarTodasLasUnidades = "\nQUITAR TODAS LAS UNIDADES:\n";

                List<ItemVentaPedido> listaItemsPedido = spVentaItems.Children.Cast<ItemVentaPedido>().ToList();
                if (listaItemsPedido.Count != 0)
                {
                    listaItemsPedido.ForEach(ivp =>
                    {
                        pedidos_productos pp = PedidosProductosBLL.Obtener(ivp.PedidoProductoID);
                        if (pp != null)
                        {
                            if (pp.impreso_cantidad != ivp.Cantidad)
                            {
                                decimal? diferenciaCantidad = ivp.Cantidad - pp.impreso_cantidad;
                                PedidosProductosBLL.AgregarCantidad(pp.id, diferenciaCantidad);
                                if (diferenciaCantidad < 0)
                                {
                                    listaProductosImprimir_Quitar += $"{pp.producto.nombre} x{Math.Abs((decimal)diferenciaCantidad)}\n";
                                }
                                else
                                {
                                    listaProductosImprimir_Agregar += $"{pp.producto.nombre} x{diferenciaCantidad}\n";
                                }
                            }
                        }
                        else
                        {
                            //PedidosProductosBLL.Crear(ped.id, ivp.Producto?.id, ivp.Promo?.id, ivp.Cantidad);
                            listaProductosImprimir_Nuevo += $"{ivp.Producto.nombre} x{ivp.Cantidad}\n";
                        }
                    });
                }
                if (spVentaPlatosPrincipales.Children.Count == 0 && spVentaItems.Children.Count == 0)
                {
                    MesaBLL.Actualizar(mesa.id, 0, null, true);
                    // Mensaje
                    List<ItemAccion> _listaItemsAccion = new List<ItemAccion>();
                    ItemAccion _ia1 = new ItemAccion();
                    

                    _ia1.Accion = "SALIR";
                    _ia1.btnAccion.Click += (se2, a2) => { MainWindow.MainFrame.Content = new PageInicio(); };

                    ItemAccion _ia2 = new ItemAccion();
                    _ia2.Accion = "OTRO PEDIDO";
                    _ia2.btnAccion.Click += (se2, a2) => { MainWindow.MainFrame.Content = new PageMesas(); };

                    _listaItemsAccion.Add(_ia1);
                    _listaItemsAccion.Add(_ia2);

                    MostrarAccion(_listaItemsAccion, "¡LISTO!");
                    return;
                }

                List<pedidos_productos> listaPedidosProductosActualizar = new List<pedidos_productos>();
                spVentaItems.Children.Cast<ItemVentaPedido>().ToList().ForEach(x =>
                {
                    listaPedidosProductosActualizar.Add(new pedidos_productos() { id = x.PedidoProductoID, cantidad = x.Cantidad });
                });
                if (listaPedidosProductosActualizar != null)
                    PedidosProductosBLL.ActualizarVarios(ped.id, listaPedidosProductosActualizar);

                List<pedidos_productos> listaPedidosProductosEliminar = new List<pedidos_productos>();
                listaItemsPedidoEliminarDesdeBD.ForEach(x =>
                {
                    listaProductosImprimir_QuitarTodasLasUnidades += $"{x.Producto.nombre}\n";
                    listaPedidosProductosEliminar.Add(new pedidos_productos() { id = x.PedidoProductoID });
                });
                if (listaPedidosProductosEliminar != null)
                    PedidosProductosBLL.EliminarVarios(ped.id, listaPedidosProductosEliminar);

                string imprimir = "";
                if (listaProductosImprimir_Nuevo != "\nNUEVO:\n")
                    imprimir += listaProductosImprimir_Nuevo;
                if (listaProductosImprimir_Agregar != "\nAGREGAR:\n")
                    imprimir += listaProductosImprimir_Agregar;
                if (listaProductosImprimir_Quitar != "\nQUITAR UNIDAD(ES):\n")
                    imprimir += listaProductosImprimir_Quitar;
                if (listaProductosImprimir_QuitarTodasLasUnidades != "\nQUITAR TODAS LAS UNIDADES:\n")
                    imprimir += listaProductosImprimir_QuitarTodasLasUnidades;

                MessageBox.Show(imprimir);

                int contadorItems = 0;

                string imprimirActualización = $"TICKET COCINA {DateTime.Now.ToShortDateString().ToUpper()} - {DateTime.Now.ToShortTimeString()}\nACTUALIZACIÓN DE PEDIDO N°{ ped.id }\n \nSolicitado por: { Settings.Usuario.nombre }\npara mesa: {mesa.codigo} sector: {mesa.sectormesa.nombre}\n\n";
                spVentaPlatosPrincipales.Children.Cast<ItemVentaPlatoFondo>().ToList().ForEach(x =>
                {
                    if (x.AgregadoUno == x.AgregadoDos)
                        imprimirActualización += $"{x.Producto.nombre} con {x.AgregadoUno.nombre} x2\n";
                    else
                        imprimirActualización += $"{x.Producto.nombre} con {x.AgregadoUno.nombre} y {x.AgregadoDos.nombre}\n";
                    contadorItems++;
                });
                spVentaItems.Children.Cast<ItemVentaPedido>().ToList().ForEach(x =>
                {
                    imprimirActualización += $"x{x.Cantidad}    {x.Producto.nombre}\n";
                    contadorItems += Convert.ToInt32(x.Cantidad);
                });

                MessageBox.Show(imprimirActualización);
                mesa.items = contadorItems;

                MesaBLL.Actualizar(mesa.id, contadorItems, Settings.Usuario.id, false);

                // Mensaje
                List<ItemAccion> listaItemsAccion = new List<ItemAccion>();
                ItemAccion ia1 = new ItemAccion();
                ia1.Accion = "SALIR";
                ia1.btnAccion.Click += (se2, a2) => { MainWindow.MainFrame.Content = new PageInicio(); };

                ItemAccion ia2 = new ItemAccion();
                ia2.Accion = "OTRO PEDIDO";
                ia2.btnAccion.Click += (se2, a2) => { MainWindow.MainFrame.Content = new PageMesas(); };

                listaItemsAccion.Add(ia1);
                listaItemsAccion.Add(ia2);

                MostrarAccion(listaItemsAccion, "¡LISTO!");
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
                    ////MostrarProductos(ProductoBLL.GetFavs());
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
