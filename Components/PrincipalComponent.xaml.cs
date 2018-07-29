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
using posk.Models;
using posk.Pages.Menu;
using posk.Controls.Accion;
using posk.Popup;
using posk.Popups;
using System.Windows.Documents;
using System.Drawing;
using System.Printing;
using System.Media;
using System.Windows.Input;
using System.Text.RegularExpressions;
using posk.Partials;

namespace posk.Components
{
    public partial class PrincipalComponent : UserControl
    {
        #region Global
        private ItemCalcularTotal itemCalcularTotal;
        private ItemTeclado teclado;
        public bool bProductosFavToggle { get; set; }
        private SolidColorBrush colorDorado;
        private SolidColorBrush colorNeutro;
        private bool bFavorito;
        private bool bPromo;
        private ItemProducto itemProductoSeleccionado;
        private ItemAgregado itemAgregadoUno;
        private ItemAgregado itemAgregadoDos;
        private ItemEnvoltura itemEnvoltura;

        private ItemAgregadoHandroll itemPaltaCebollin;
        private ItemMensajeCocina imc;
        private int ultimoSyncIdPedido;
        private int ultimoSyncIdDelivery;
        private ItemEscogerGarzonMesa iegm;
        private ItemEscogerGarzonSeccionVenta ieg;
        private ItemEscogerMesaSeccionVenta iem;
        private ItemMedioPago imp;

        private bool bDelimitadorPresionado = false;

        private mesa mesa;
        public string Modo { get; set; }

        private List<ItemMoneda> listaItemsMoneda1 { get; set; }
        private List<ItemMoneda> listaItemsMoneda2 { get; set; }

        private List<preparacione> listaItemsPreparadoEspecial { get; set; }

        private ItemDelimitador itemDelimitador { get; set; }

        private ItemDcto itemDcto { get; set; }

        public int Propina { get; set; }


        #endregion

        #region Constructor
        /// <summary>
        /// modo: VENTA, PEDIDO, STOCK, DEVOLUCION, MERMA, 
        /// </summary>
        /// <param name="_mesa"></param>
        /// <param name="modo"></param>
        public PrincipalComponent(string modo, mesa _mesa = null)
        {
            InitializeComponent();
            Modo = modo;
            mesa = _mesa;

            itemDelimitador = new ItemDelimitador();
            itemDcto = new ItemDcto();

            listaItemsPreparadoEspecial = new List<preparacione>();
            #region Inicializar otros componentes

            itemCalcularTotal = new ItemCalcularTotal();
            itemProductoSeleccionado = new ItemProducto();
            itemAgregadoUno = new ItemAgregado();
            itemAgregadoDos = new ItemAgregado();
            itemEnvoltura = new ItemEnvoltura();
            itemPaltaCebollin = new ItemAgregadoHandroll();
            imc = new ItemMensajeCocina();

            iegm = new ItemEscogerGarzonMesa();
            ieg = new ItemEscogerGarzonSeccionVenta() { Usuarios = UsuarioBLL.ObtenerGarzones() };
            iem = new ItemEscogerMesaSeccionVenta() { Mesas = MesaBLL.ObtenerTodas() };
            imp = new ItemMedioPago() { MediosPago = MedioPagoBLL.ObtenerTodos() };

            colorDorado = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 150, 0));
            colorNeutro = new SolidColorBrush(System.Windows.Media.Color.FromRgb(230, 230, 230));
            btnExpanderLeft.Foreground = colorDorado;
            expLeft.IsExpanded = true;
            ultimoSyncIdPedido = 0;
            ultimoSyncIdDelivery = SyncBLL.ObtenerUltimoSyncId("delivery");


            listaItemsMoneda1 = new List<ItemMoneda>()
            {
                new ItemMoneda() { Monto = 1000 },
                new ItemMoneda() { Monto = 2000 },
                new ItemMoneda() { Monto = 3000 },
                new ItemMoneda() { Monto = 4000 },
                new ItemMoneda() { Monto = 5000 },
                new ItemMoneda() { Monto = 10000 },
                new ItemMoneda() { Monto = 20000 }
            };
            listaItemsMoneda1.ForEach(x =>
            {
                x.Focusable = false;
                spMoneda1.Children.Add(x);
            });

            listaItemsMoneda2 = new List<ItemMoneda>()
            {
                new ItemMoneda() { Monto = 10 },
                new ItemMoneda() { Monto = 20 },
                new ItemMoneda() { Monto = 30 },
                new ItemMoneda() { Monto = 40 },
                new ItemMoneda() { Monto = 50 },
                new ItemMoneda() { Monto = 50 },
                new ItemMoneda() { Monto = 60 },
                new ItemMoneda() { Monto = 80 },
                new ItemMoneda() { Monto = 90 },
                new ItemMoneda() { Monto = 100 },
                new ItemMoneda() { Monto = 200 },
                new ItemMoneda() { Monto = 300 },
                new ItemMoneda() { Monto = 400 },
                new ItemMoneda() { Monto = 500 },
                new ItemMoneda() { Monto = 600 },
                new ItemMoneda() { Monto = 700 },
                new ItemMoneda() { Monto = 800 },
                new ItemMoneda() { Monto = 900 }
            };
            listaItemsMoneda2.ForEach(x =>
            {
                x.Focusable = false;
                spMoneda2.Children.Add(x);
            });

            teclado = new ItemTeclado(new List<TextBox>() { txtBuscar });
            borderTeclado.Child = teclado;

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); // Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
            #endregion Inicializar otros componentes

            InitEvents();
        }

        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                //Sync("pedido");
                //Sync("delivery");
                lbFecha.Content = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}";
                if (InternetChecker.IsConnectedToInternet())
                    internetStatus.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(46, 139, 87));
                else
                    internetStatus.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(252, 52, 52));
            }
            catch (Exception)
            {
                //lbInfo.Content = "ERROR: " + err.Message;
            }
        }
        #endregion Constructor

        #region Metodos

        private void Sync(string nombre)
        {
            switch (nombre)
            {
                case "pedido":
                    if (ultimoSyncIdPedido == SyncBLL.ObtenerUltimoSyncId("pedido")) return;
                    else
                    {
                        ultimoSyncIdPedido = SyncBLL.ObtenerUltimoSyncId("pedido");
                        if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                        {
                            new Notification("NUEVO PEDIDO");
                            spDerecha.Children.Clear();
                            CargarPendientes();
                            CargarItemVerPendientes();
                            expDerecha.IsExpanded = true;
                            PlaySound(@"C:/posk/sound/campana.mp3");
                        }
                    }
                    break;
                case "delivery":
                    if (ultimoSyncIdPedido == SyncBLL.ObtenerUltimoSyncId("delivery")) return;
                    else
                    {
                        ultimoSyncIdPedido = SyncBLL.ObtenerUltimoSyncId("delivery");
                        // if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                        // {
                        new Notification("NUEVO PEDIDO");
                        spDerecha.Children.Clear();
                        // CargarPendientes();
                        // CargarItemVerPendientes();
                        CargarDeliveryPendientesDeEntrega();
                        expDerecha.IsExpanded = true;
                        PlaySound(@"C:/posk/sound/campana.mp3");
                        DeliveryInfo di = new DeliveryInfo() { NombreCliente = "Pedido qr", PagaCon = 1000 };
                        GenerarTicket(1000, ((usuario)ieg.cbGarzones.SelectedItem)?.nombre, ((mesa)iem.cbMesas.SelectedValue)?.codigo, "", "Ticket Caja", di);
                        // }
                    }
                    break;
                default:
                    break;
            }
        }

        private void PlaySound(string ruta)
        {
            var uri = new Uri(ruta, UriKind.Absolute);
            var player = new MediaPlayer();
            player.Open(uri);
            player.Play();
        }

        /// <summary>
        /// Deja la sección de búsqueda y Venta/Pedidos como nueva
        /// </summary>
        private void LimpiarTodo()
        {
            MostrarOverlay(false);
            imc.txtMensaje.Text = "";
            ieg.cbGarzones.Text = "";
            iem.cbMesas.Text = "";
            imp.cbMedioPago.Text = "Efectivo";
            spVentaItems.Children.Clear();
            if (itemCalcularTotal != null)
            {
                itemCalcularTotal.txtPagaCon.Clear();
                itemCalcularTotal.txtVuelto.Clear();
                itemCalcularTotal.txtTotalVenta.Clear();
            }
            iconFavorito.Foreground = colorDorado;
            iconPromo.Foreground = colorNeutro;
            MostrarPedidos((ProductoBLL.GetFavs()));
            txtBuscar.Text = "FAVORITOS";
            txtBuscar.Foreground = colorDorado;
            bFavorito = true;

            /*
            bFavorito = false;
            iconFavorito.Foreground = colorNeutro;
            txtBuscar.Text = "";
            bPromo = false;
            */
            itemDcto.Reset();
            gridDcto.Children.Clear();

            btnCrearDelimitador.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(218, 222, 224));
            bDelimitadorPresionado = false;
        }

        /// <summary>
        /// Si b es true, Muestra overlay con efeceto fadein y abre un popup el cual recibe contenido previamente.
        /// Si b es false, oculta el overlay y cierra el popup.
        /// </summary>
        /// <param name="b"></param>
        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                overlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
                //expPopup.IsExpanded = true;
            }
            else
            {
                teclado.ExpTeclado.IsExpanded = false;
                expTecladoPopup.IsExpanded = false;
                overlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = false;
            }
        }

        /// <summary>
        /// Muestra popup con un título y subtítulo - efecto fadein fadeout
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="mensajeSecundario"></param>
        private void MostrarNotificacion(string mensaje, string mensajeSecundario)
        {
            new Notification(mensaje, mensajeSecundario, Notification.Type.Successful, 1, Notification.Size.Sm, Notification.Position.Top);
        }

        /// <summary>
        /// Mensaje popup con una lista de botones programados (ItemAccion)
        /// </summary>
        /// <param name="listaItemsAccion"></param>
        /// <param name="titulo"></param>
        private void MostrarAccion(List<ItemAccion> listaItemsAccion, string titulo)
        {
            var wop = new WindowOpcionesPopup(listaItemsAccion, titulo);
            wop.Opacity = 0;
            wop.Topmost = true;
            wop.Show();

            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            wop.BeginAnimation(OpacityProperty, fadeIn);
        }

        /// <summary>
        /// Luego de Incluir() un plato, reestablecer configuración de ItemProducto e ItemAgregado
        /// </summary>
        private void DeseleccionarProductoAgregado()
        {
            itemProductoSeleccionado.Reiniciar();
            itemProductoSeleccionado = new ItemProducto();
            itemAgregadoUno = new ItemAgregado();
            itemAgregadoDos = new ItemAgregado();
            foreach (ItemAgregado ia in wrapAgregados.Children.OfType<ItemAgregado>().ToList())
            {
                ia.Reiniciar();
            }
            expBottom.IsExpanded = false;
        }

        /// <summary>
        /// Retorna true si es que existe un ItemProducto seleccionado en wrapProductos
        /// </summary>
        /// <returns></returns>
        private bool ProductoYaSeleccionado()
        {
            bool b = false;
            foreach (ItemProducto ip in wrapProductos.Children.OfType<ItemProducto>())
            {
                if (ip.Seleccionado)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// Retorna el número de ItemAgregado (para plato de fondo) seleccionados
        /// Mínimo 0, Máximo 2
        /// </summary>
        /// <returns></returns>
        private int AgregadosSeleccionados()
        {
            int agregadosSeleccionados = 0;
            foreach (ItemAgregado ia in wrapAgregados.Children.OfType<ItemAgregado>().ToList())
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

        /// <summary>
        /// Crea productos que se muestran como resultado para seleccionar, con tal de ingresarlos a sección PEDIDOS
        /// Recibe lista de productos en tupla para crear ItemProducto los cuales se muestran en wrapProductos
        /// Recibe bool en tupla para saber si la lista de productos coincide con la busqueda de una subcategoria
        /// si la lista coincide con una subcategoría, muestra el texto de la cada de búsqueda en color dorado
        /// si el producto lleva agregado, al hacerle click se selecciona esperando seleccionar 2 agregados
        /// si el producto lleva agregado, al hacerle doble click se agrega a la sección venta
        /// si el producto no lleva agregado, al hacerle click se agrega a la sección venta
        /// TODO - optimizar código para evitar duplicado
        /// </summary>
        /// <param name="tupla"></param>
        private void MostrarPedidos((List<producto> listaProductos, List<promocione> listaPromociones, bool bSubCategoria) tupla)
        {
            if (Modo == "VENTA" || Modo == "DEVOLUCION")
                MostrarProductos(tupla);
            else
            {
                try
                {
                    wrapProductos.Children.Clear();
                    txtBuscar.Foreground = colorNeutro;

                    if (tupla.listaProductos != null)
                    {
                        tupla.listaProductos.OrderBy(x => x.z_index).ToList();
                        if (tupla.bSubCategoria)
                        {
                            txtBuscar.Foreground = colorDorado;
                            iconFavorito.Foreground = colorNeutro;
                            bFavorito = false;

                            txtBuscar.Foreground = colorDorado;
                            iconPromo.Foreground = colorNeutro;
                            bPromo = false;
                        }
                        else
                        {
                            txtBuscar.Foreground = colorNeutro;
                        }

                        foreach (producto p in tupla.listaProductos)
                        {

                            if (Modo == "STOCK" && p.tipo_producto_id != null || Modo == "STOCK" && p.solo_venta == true)
                                continue;
                            else if (p.tipo_producto_id != null)
                            {
                                var ip = new ItemProducto() { Producto = p };

                                ip.AlDeseleccionar += (se3, a3) =>
                                {
                                    expBottom.IsExpanded = false;
                                };
                                ip.AlSeleccionar += (se3, a3) => expBottom.IsExpanded = true;

                                ip.btnProducto.Click += (se2, a2) =>
                                {
                                    if (itemProductoSeleccionado != ip)
                                    {
                                        if (itemProductoSeleccionado != null)
                                            itemProductoSeleccionado.Reiniciar();
                                        itemProductoSeleccionado = ip;
                                    }
                                    teclado.expTeclado.IsExpanded = false;
                                };

                                ip.MouseDoubleClick += (se2, a2) =>
                                {
                                    if (itemProductoSeleccionado != null)
                                        itemProductoSeleccionado.Reiniciar();

                                    ItemVentaPedido ivpExistente = spVentaItems.Children.OfType<ItemVentaPedido>().ToList().Where(ivp => ivp.Producto?.id == ip.Producto?.id).FirstOrDefault();

                                    if (ivpExistente != null)
                                    {
                                        ivpExistente.AgregarCantidad(1);
                                        MostrarNotificacion($"{itemProductoSeleccionado.Producto?.nombre.ToUpper()}", "+1");
                                        // new Notification($"{itemProductoSeleccionado.Producto?.nombre.ToUpper()}", "+1", Notification.Type.Successful, 1, Notification.Size.Sm, Notification.Position.Top);
                                    }
                                    else
                                    {
                                        var ivpNuevo = new ItemVentaPedido() { Producto = p, Cantidad = 1 };
                                        ivpNuevo.AlEliminar += (se3, a3) =>
                                        {
                                            spVentaItems.Children.Remove(ivpNuevo);
                                            CalcularTotal();
                                        };

                                        ivpNuevo.btnCantidad.Click += (se3, a3) =>
                                        {
                                            MostrarOverlay(true);
                                            AgregarCantidadPopup acp = new AgregarCantidadPopup(null, ivpNuevo);
                                            acp.OnAgregarCantidad += (se4, ev4) => ivpNuevo.AgregarCantidad(ev4);
                                            acp.OnQuitarCantidad += (se4, ev4) => ivpNuevo.AgregarCantidad(-ev4);
                                            acp.OnFinish += (se4, a4) => MostrarOverlay(false);
                                            expPopup.Content = acp;
                                        };

                                        spVentaItems.Children.Add(ivpNuevo);
                                        CalcularTotal();

                                        MostrarNotificacion($"{itemProductoSeleccionado.Producto?.nombre.ToUpper()}", "");
                                    }
                                    teclado.expTeclado.IsExpanded = false;
                                };
                                wrapProductos.Children.Add(ip);
                            }
                            else
                            {
                                var ip = new ItemProducto() { Producto = p };

                                ip.AlDeseleccionar += (se3, a3) =>
                                {
                                    expBottom.IsExpanded = false;
                                };

                                ip.btnProducto.Click += (se, a) =>
                                {
                                    ItemVentaPedido ivpExistente = spVentaItems.Children.OfType<ItemVentaPedido>().ToList().Where(ivp => ivp.Producto?.id == ip.Producto?.id).FirstOrDefault();

                                    if (ivpExistente != null)
                                    {
                                        ivpExistente.AgregarCantidad(1);
                                        MostrarNotificacion($"{ivpExistente.Producto?.nombre.ToUpper()}", "+1");
                                    }
                                    else
                                    {
                                        var ivpNuevo = new ItemVentaPedido() { PedidoProductoID = p.id, Producto = p, Cantidad = 1 };
                                        ivpNuevo.AlEliminar += (se3, a3) =>
                                        {
                                            spVentaItems.Children.Remove(ivpNuevo);
                                            CalcularTotal();
                                        };
                                        ivpNuevo.btnCantidad.Click += (se3, a3) =>
                                        {
                                            MostrarOverlay(true);
                                            ModificarCantidadPopup acp = new ModificarCantidadPopup(null, ivpNuevo);
                                            acp.Show();
                                            /*
                                            acp.OnAgregarCantidad += (se4, ev4) => ivpNuevo.AgregarCantidad(ev4);
                                            acp.OnQuitarCantidad += (se4, ev4) => ivpNuevo.AgregarCantidad(-ev4);
                                            */
                                            acp.OnFinish += (se4, a4) =>
                                            {
                                                MostrarOverlay(false);
                                                acp.bCerrado = true;
                                                acp.Close();
                                            };
                                        };
                                        spVentaItems.Children.Add(ivpNuevo);
                                        CalcularTotal();

                                        MostrarNotificacion($"{ivpNuevo.Producto?.nombre.ToUpper()}", "");
                                    }
                                    teclado.expTeclado.IsExpanded = false;
                                };
                                wrapProductos.Children.Add(ip);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Crea productos que se muestran como resultado para seleccionar, con tal de ingresarlos a sección VENTA.
        /// bSubCategoria es un booleano que retorna true cuando el texto ingresado coincide con el nombre de una subcategoria
        /// </summary>
        /// <param name="tupla"></param>
        private void MostrarProductos((List<producto> listaProductos, List<promocione> listaPromociones, bool bSubCategoria) tupla)
        {
            try
            {

                // obtengo los favoritos de la base de datos, muestro los items y luego cambio el texto de txtBuscar a FAVORITOS
                // por ende, textchanged va a volver a intentar mostrar los resultados del texto puesto
                // asi que hay que frenarlo para que no intente mostrar los items del resultado al escribir FAVORITOS


                if (txtBuscar.Text == "FAVORITOS")
                {
                    iconPromo.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    bPromo = false;

                    iconFavorito.Foreground = colorDorado;
                    txtBuscar.Foreground = colorDorado;
                    bFavorito = true;
                    return;
                }
                else if (txtBuscar.Text == "PROMOS")
                {
                    iconFavorito.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    bFavorito = false;

                    iconPromo.Foreground = colorDorado;
                    txtBuscar.Foreground = colorDorado;
                    bPromo = true;
                    return;
                }
                //else
                //{
                //    iconFavorito.Foreground = colorNeutro;
                //    txtBuscar.Foreground = colorNeutro;
                //    bFavorito = false;

                //    iconPromo.Foreground = colorNeutro;
                //    txtBuscar.Foreground = colorNeutro;
                //    bPromo = false;
                //}

                wrapProductos.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                wrapProductos.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                if (tupla.listaProductos != null || tupla.listaPromociones != null)
                {
                    if (tupla.listaProductos != null)
                        tupla.listaProductos.OrderBy(x => x.z_index).ToList();

                    if (tupla.bSubCategoria)
                    {
                        txtBuscar.Foreground = colorDorado;
                    }
                    else
                    {
                        txtBuscar.Foreground = colorNeutro;
                    }
                    tupla.listaPromociones?.ForEach(promo => CrearMostrarItemPromo(promo));
                    tupla.listaProductos?.ForEach(p => CrearMostrarItemProducto(p));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CrearMostrarItemPromo(promocione p)
        {
            var ip = new ItemPromoVentaPedido() { Promocion = p };

            ip.btnProducto.Click += (se, a) =>
            {
                CrearItemVenta(null, p);
                teclado.expTeclado.IsExpanded = false;
            };
            wrapProductos.Children.Add(ip);
        }

        private void CrearMostrarItemProducto(producto p)
        {
            if (Modo == "VENTA" && p.solo_compra == true)
                return;
            if (p.tipo_producto_id != null)
            {
                var ip = new ItemProducto() { Producto = p };
                ip.AlSeleccionar += (se, a) =>
                {
                    ArmarProductoEspecialPopup apep = new ArmarProductoEspecialPopup(p);
                    apep.Deactivated += (se2, a2) =>
                    {
                        apep.bCerrado = true;
                        MostrarOverlay(false);
                        ip.Reiniciar();
                    };
                    MostrarOverlay(true);
                    apep.Show();
                    apep.AlTerminarArmado += (se2, ivArmado) =>
                    {
                        CrearItemVentaDesdeItemVenta(ivArmado);
                        MostrarOverlay(false);
                    };
                };
                wrapProductos.Children.Add(ip);
            }
            else
            {
                var ip = new ItemProducto() { Producto = p };

                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottom.IsExpanded = false;
                };

                ip.btnProducto.Click += (se, a) =>
                {
                    CrearItemVenta(p);
                    teclado.expTeclado.IsExpanded = false;
                };
                wrapProductos.Children.Add(ip);
            }
        }


        private void ArmarTabla(producto p)
        {
            ArmarTablaPopup atp = new ArmarTablaPopup(p);
            atp.AlTerminarArmado += (se, tablaItemVenta) => CrearItemVentaDesdeItemVenta(tablaItemVenta);
            atp.Show();
        }

        /// <summary>
        /// Crea item venta sea de producto o promoción
        /// escoger uno (prod o promo), el otro dejarlo null
        /// </summary>
        /// <param name="prod"></param>
        /// <param name="promo"></param>
        /// 
        private void CrearItemVenta(producto prod = null, promocione promo = null, int cantidad = 1)
        {
            if (prod != null)
            {
                ItemVenta ivExistente = spVentaItems.Children.OfType<ItemVenta>().ToList().Where(iv => iv.Producto?.id == prod.id).FirstOrDefault();

                ivExistente = null; // sirve para que modo delimitador cree un item venta nuevo en vez de agregar cantidad
                if (ivExistente != null)
                {
                    ivExistente.AgregarCantidad(cantidad);
                    MostrarNotificacion($"{ivExistente.Producto?.nombre.ToUpper()}", "+" + cantidad);
                }
                else
                {
                    var ivNuevo = new ItemVenta() { ProductoID = prod.id, Producto = prod, Cantidad = cantidad };
                    ivNuevo.AlEliminar += (se3, a3) =>
                    {
                        spVentaItems.Children.Remove(ivNuevo);
                        CalcularTotal();
                    };
                    ivNuevo.btnCantidad.Click += (se3, a3) =>
                    {
                        MostrarOverlay(true);
                        AgregarCantidadPopup acp = new AgregarCantidadPopup(ivNuevo);
                        acp.OnAgregarCantidad += (se2, ev2) => ivNuevo.AgregarCantidad(ev2);
                        acp.OnQuitarCantidad += (se2, ev2) => ivNuevo.AgregarCantidad(-ev2);
                        acp.OnFinish += (se, a) => MostrarOverlay(false);
                        expPopup.Content = acp;
                    };
                    ivNuevo.AlModificarCantidad += (se3, a3) => CalcularTotal();
                    ivNuevo.AlModificarTotal += (se3, a3) => CalcularTotal();
                    spVentaItems.Children.Add(ivNuevo);
                    MostrarNotificacion($"{ivNuevo.Producto?.nombre.ToUpper()}", "");
                    CalcularTotal();
                }
            }
            else if (promo != null)
            {
                ItemVenta ivExistente = spVentaItems.Children.OfType<ItemVenta>().ToList().Where(iv => iv.Promocion?.id == promo.id).FirstOrDefault();

                if (ivExistente != null)
                {
                    ivExistente.AgregarCantidad(cantidad);
                    MostrarNotificacion($"{ivExistente.Promocion.nombre.ToUpper()}", "+" + cantidad);
                }
                else
                {
                    var ivNuevo = new ItemVenta() { Promocion = promo, Cantidad = cantidad };
                    ivNuevo.AlEliminar += (se3, a3) =>
                    {
                        spVentaItems.Children.Remove(ivNuevo);
                        CalcularTotal();
                    };
                    ivNuevo.AlModificarCantidad += (se3, a3) => CalcularTotal();
                    ivNuevo.AlModificarTotal += (se3, a3) => CalcularTotal();
                    spVentaItems.Children.Add(ivNuevo);
                    MostrarNotificacion($"{ivNuevo.Promocion.nombre.ToUpper()}", "");
                    CalcularTotal();
                }
            }
        }


        private void CrearItemVentaDesdeItemVenta(ItemVenta _iv, int cantidad = 1)
        {
            _iv.AlEliminar += (se3, a3) =>
            {
                spVentaItems.Children.Remove(_iv);
                CalcularTotal();
            };
            _iv.btnCantidad.Click += (se3, a3) =>
            {
                MostrarOverlay(true);
                AgregarCantidadPopup acp = new AgregarCantidadPopup(_iv);
                acp.OnAgregarCantidad += (se2, ev2) => _iv.AgregarCantidad(ev2);
                acp.OnQuitarCantidad += (se2, ev2) => _iv.AgregarCantidad(-ev2);
                acp.OnFinish += (se, a) => MostrarOverlay(false);
                expPopup.Content = acp;
            };
            _iv.AlModificarCantidad += (se3, a3) => CalcularTotal();
            _iv.AlModificarTotal += (se3, a3) => CalcularTotal();
            spVentaItems.Children.Add(_iv);
            MostrarNotificacion($"{_iv.Producto?.nombre.ToUpper()}", "");
            CalcularTotal();
        }




        /// <summary>
        /// Itera todos los platos de fondo y los elementos de venta para calcular el total de la sección venta
        /// </summary>
        private void CalcularTotal()
        {
            int? total = 0;
            //spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList().ForEach(x => total += x.ObtenerTotal());

            //spVentaItems.Children.OfType<ItemVentaPreparadoEspecial>().ToList().ForEach(x => total += x.ObtenerTotal());
            spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(x => total += x.ObtenerTotal());

            total -= itemDcto.DctoPesos;

            itemCalcularTotal.txtTotalVenta.Text = total + "";
            if (total == 0)
                itemCalcularTotal.txtTotalVenta.Clear();

            // tambien calcular vuelto
            try
            {
                if (!string.IsNullOrEmpty(itemCalcularTotal.txtPagaCon.Text))
                    itemCalcularTotal.txtVuelto.Text = $"{Convert.ToInt32(itemCalcularTotal.txtPagaCon.Text) - Convert.ToInt32(itemCalcularTotal.txtTotalVenta.Text)}";
                else
                    itemCalcularTotal.txtVuelto.Clear();
            }
            catch (Exception ex)
            {
                itemCalcularTotal.txtVuelto.Clear();
                itemCalcularTotal.txtPagaCon.Clear();
                PoskException.Make(ex, "Error al calcular total");
            }
        }

        private void ReCalcularTotal()
        {
            int? total = 0;
            spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(x => total += x.ObtenerTotal());

            itemCalcularTotal.txtTotalVenta.Text = total + "";
            if (total == 0)
                itemCalcularTotal.txtTotalVenta.Clear();

            // tambien calcular vuelto
            try
            {
                if (!string.IsNullOrEmpty(itemCalcularTotal.txtPagaCon.Text))
                    itemCalcularTotal.txtVuelto.Text = $"{Convert.ToInt32(itemCalcularTotal.txtPagaCon.Text) - Convert.ToInt32(itemCalcularTotal.txtTotalVenta.Text)}";
                else
                    itemCalcularTotal.txtVuelto.Clear();
            }
            catch (Exception ex)
            {
                itemCalcularTotal.txtVuelto.Clear();
                itemCalcularTotal.txtPagaCon.Clear();
                PoskException.Make(ex, "Error al calcular total");
            }
        }

        /// <summary>
        /// Crea un expandible por cada sector, luego crea un expandible por cada categoria de cada sector
        /// Luego crea elemento subcategoría que al clickear, muestra los elementos que coinciden con la subcategoría
        /// </summary>
        private void CrearMenuBusquedaPorCategoria()
        {
            spCategorias.Children.Clear();
            SectorBLL.ObtenerTodo().ForEach(s =>
            {
                if (s.seleccionable == true)
                {
                    var scbi = new SubCategoriaBuscarItem() { Sector = s };
                    scbi.btnSubCategoria.Click += (se2, a2) =>
                    {
                        txtBuscar.Text = $"{s.nombre}";
                        bFavorito = false;
                        btnFavorito.Foreground = colorNeutro;

                        bPromo = false;
                        btnPromo.Foreground = colorNeutro;
                    };
                    spCategorias.Children.Add(scbi);
                }
                else
                {
                    ItemBuscarPorSector ibps = new ItemBuscarPorSector() { Sector = s };
                    ibps.expSector.IsExpanded = true;
                    spCategorias.Children.Add(ibps);
                }
            });

            spCategorias.Children.OfType<ItemBuscarPorSector>().ToList().ForEach(ibs =>
            {
                ibs.spItems.Children.Clear();
                CategoriaSectorBLL.ObtenerCategorias(ibs.Sector.id).ForEach(c =>
                {
                    if (c.seleccionable == true)
                    {
                        var scbi = new SubCategoriaBuscarItem() { Categoria = c };
                        scbi.btnSubCategoria.Click += (se2, a2) =>
                        {
                            txtBuscar.Text = $"{c.nombre}";
                            bFavorito = false;
                            btnFavorito.Foreground = colorNeutro;

                            bPromo = false;
                            btnPromo.Foreground = colorNeutro;
                        };
                        ibs.spItems.Children.Add(scbi);
                    }
                    else
                    {
                        ItemBuscarPorCategoria ibpc = new ItemBuscarPorCategoria() { Categoria = c };
                        ibpc.expCategoria.IsExpanded = false;
                        ibpc.expCategoria.Expanded += (se, a) =>
                        {
                            ibs.spItems.Children.OfType<ItemBuscarPorCategoria>().ToList().ForEach(ibpc2 =>
                            {
                                if (ibpc2.Categoria?.id != c.id)
                                    ibpc2.expCategoria.IsExpanded = false;
                            });
                        };
                        ibs.spItems.Children.Add(ibpc);
                    }
                });

                ibs.spItems.Children.OfType<ItemBuscarPorCategoria>().ToList().ForEach(ibc =>
                {
                    ibc.spItems.Children.Clear();
                    CategoriaSubcategoriaBLL.ObtenerSubcategorias(ibc.Categoria.id).ForEach(sc =>
                    {
                        var scbi = new SubCategoriaBuscarItem() { SubCategoria = sc };
                        scbi.btnSubCategoria.Click += (se2, a2) =>
                        {
                            txtBuscar.Text = $"{sc.nombre}";
                            //MostrarPedidos(SubCategoriaBLL.ObtenerProductos(sc.id));
                            bFavorito = false;
                            btnFavorito.Foreground = colorNeutro;
                            bPromo = false;
                            btnPromo.Foreground = colorNeutro;
                        };
                        ibc.spItems.Children.Add(scbi);
                    });
                });
            });
        }

        #endregion Metodos-

        #region secciones

        private void CargarPendientes()
        {
            PendienteBLL.GetAll().ForEach(pend =>
            {
                var itemPend = new ItemPendiente()
                {
                    Pendiente = pend,
                    Producto = pend.producto,
                    Promocion = pend.promocione,
                    Usuario = pend.usuario
                };
                itemPend.btnPendiente.Click += (se3, a3) =>
                {
                    ItemVenta itemVentaExistente = null;
                    producto p = null;
                    promocione promo = null;

                    if (itemPend.Producto != null)
                    {
                        itemVentaExistente = spVentaItems.Children.OfType<ItemVenta>().ToList().Where(x => x.Producto == itemPend.Producto).FirstOrDefault();
                        if (itemVentaExistente?.Producto?.id != null)
                            p = ProductoBLL.GetProduct(itemPend.Producto?.id);
                    }
                    else
                    {
                        if (itemPend.Promocion != null)
                        {
                            itemVentaExistente = spVentaItems.Children.OfType<ItemVenta>().ToList().Where(x => x.Promocion == itemPend.Promocion).FirstOrDefault();
                            if (itemVentaExistente?.Promocion?.id != null)
                                promo = PromocionBLL.ObtenerPromocion(itemPend.Promocion.id);
                        }
                    }

                    if (itemVentaExistente?.Producto != null || itemVentaExistente?.Promocion != null)
                    {
                        itemVentaExistente?.AgregarCantidad(1);
                        MostrarNotificacion($"{itemVentaExistente?.Producto?.nombre.ToUpper()}", "+1");
                    }
                    else
                    {
                        var ivpNuevo = new ItemVenta() { Producto = itemPend.Producto, Promocion = itemPend.Promocion, Cantidad = 1 };

                        ivpNuevo.AlEliminar += (se4, a4) =>
                        {
                            spVentaItems.Children.Remove(ivpNuevo);
                            CalcularTotal();
                        };
                        ivpNuevo.AlModificarCantidad += (se4, a4) => CalcularTotal();
                        ivpNuevo.AlModificarTotal += (se4, a4) => CalcularTotal();
                        spVentaItems.Children.Add(ivpNuevo);
                        CalcularTotal();

                        if (itemPend.Producto != null)
                            MostrarNotificacion($"{ivpNuevo.Producto?.nombre.ToUpper()}", "");
                        else if (itemPend.Promocion != null)
                            MostrarNotificacion($"{ivpNuevo.Promocion?.nombre.ToUpper()}", "");
                    }
                    PendienteBLL.Delete(itemPend.Pendiente.id);
                    spDerecha.Children.Clear();
                    CargarPendientes();
                    CargarItemVerPendientes();
                    CalcularTotal();
                };
                spDerecha.Children.Add(itemPend);
            });
        }


        private void CargarItemVerPendientes()
        {
            spDerecha.Children.Clear();

            ItemVerPendientes_venta itemVerPendientes = new ItemVerPendientes_venta();

            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
            {
                //CargarPendientesRestaurant();
                //itemVerPendientes.lbVerEntregasNumero.Content = $"{ PedidoBLL.ObtenerTodos().Count }";
            }
            else //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
            {
                CargarPendientes();
                //itemVerPendientes.lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Count}";
            }

            itemVerPendientes.btnVerPendientes.Click += (se2, a2) =>
            {

                //    if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                //    {
                //        CargarPendientesRestaurant();
                //        itemVerPendientes.lbVerEntregasNumero.Content = $"{ PedidoBLL.ObtenerTodos().Count }";
                //    }
                //    else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                //    {
                //        CargarArriendos();
                //    }
                //    else //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
                //    {
                //        CargarPendientes();
                //        itemVerPendientes.lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Count}";
                //    }
                expDerecha.IsExpanded ^= true;
            };

            //if (!Settings.Usuario.tipo.ToLower().Equals("g"))
            borderSuperiorDerecha.Child = itemVerPendientes;
        }

        private void ConfigurarItemMoneda(ItemMoneda x)
        {
            x.Reiniciar();
            x.AlSumarCantidad += (se3, a3) =>
            {
                int pagaCon = 0;
                try { if (itemCalcularTotal.txtPagaCon.Text != "") pagaCon = Convert.ToInt32(itemCalcularTotal.txtPagaCon.Text); }
                catch { expTecladoPagar.IsExpanded = false; itemCalcularTotal.txtPagaCon.Text = "0"; }
                itemCalcularTotal.txtPagaCon.Text = $"{pagaCon + x.Monto}";
            };
            x.AlRestarCantidad += (se3, a3) =>
            {
                int pagaCon = 0;
                try { if (itemCalcularTotal.txtPagaCon.Text != "") pagaCon = Convert.ToInt32(itemCalcularTotal.txtPagaCon.Text); }
                catch { expTecladoPagar.IsExpanded = false; itemCalcularTotal.txtPagaCon.Text = "0"; }
                if (pagaCon - x.Monto == 0)
                    itemCalcularTotal.txtPagaCon.Clear();
                else
                    itemCalcularTotal.txtPagaCon.Text = $"{pagaCon - x.Monto}";
            };
        }

        /*
        public void PrintReceiptForTransaction()
        {
            PrintDocument recordDoc = new PrintDocument();

            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(PrintReceiptPage); // function below
            recordDoc.PrintController = new StandardPrintController(); // hides status dialog popup
                                                                       // Comment if debugging 
            PrinterSettings ps = new PrinterSettings();
            //ps.PrinterName = "EPSON TM-T20II Receipt";
            ps.PrinterName = "THERMAL Receipt Printer";
            recordDoc.PrinterSettings = ps;
            recordDoc.Print();
            // --------------------------------------

            // Uncomment if debugging - shows dialog instead
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.Document = recordDoc;
            //printPrvDlg.Width = 1200;
            //printPrvDlg.Height = 800;
            //printPrvDlg.ShowDialog();
            // --------------------------------------

            recordDoc.Dispose();
        }

        private void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            float x = 10;
            float y = 5;
            float width = 270.0F; // max width I found through trial and error
            float height = 0F;

            //Font regular = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Regular);
            //Font bold = new Font(FontFamily.GenericSansSerif, 10.0f, FontStyle.Bold);

            Font drawFontArial12Bold = new Font("Arial", 12, System.Drawing.FontStyle.Bold);
            Font drawFontArial10Regular = new Font("Arial", 10, System.Drawing.FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Set format of string.
            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;

            // Draw string to screen.
            string text = "BUENOS AIRES RESTAURANT";
            e.Graphics.DrawString(text, drawFontArial12Bold, drawBrush, new RectangleF(x, y, width, height), drawFormatCenter);
            y += e.Graphics.MeasureString(text, drawFontArial12Bold).Height;

            spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(item =>
            {
                int count = 1;
                //text = $"{item.Producto?.nombre}\t\t${item.Producto?.precio}";
                //e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                //y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;


                e.Graphics.DrawString(item.Producto?.nombre.ToString(), drawFontArial10Regular, System.Drawing.Brushes.Black, 20, 150 + count * 20);
                count++;
            });

            // ... and so on
        }
        */


        private void CargarSeccionVenta()
        {
            Loaded += (se, e) =>
            {
                ultimoSyncIdPedido = SyncBLL.ObtenerUltimoSyncId("pedido");
                ultimoSyncIdDelivery = SyncBLL.ObtenerUltimoSyncId("delivery");

                lbInfo.Content = "SECCIÓN VENTA";

                if (Settings.Usuario.tipo.ToLower().Equals("g"))
                    lbInfo.Content = "SECCIÓN PEDIDOS";

                // esconder seccion pendiete a garzón
                //if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                CargarItemVerPendientes();

                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
                    CargarDeliveryPendientesDeEntrega();

                /*
                lbNombreLista.Content = "VENTA";
                else
                    lbNombreLista.Content = "PEDIDO";
                */
                if (!GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                {
                    //btnExpanderBottom.Visibility = Visibility.Hidden;
                    //btnExpanderBottom.Width = 0;
                    btnPromo.Visibility = Visibility.Hidden;
                    btnPromo.Width = 0;
                }
                try
                {
                    boleta _ts = BoletaBLL.ObtenerUltima();
                    ItemUltimaVenta _iuv = new ItemUltimaVenta() { Mensaje = $"ÚLTIMA VENTA: ${_ts?.total}  EL {_ts?.fecha.ToShortDateString()}  A LAS  {_ts?.fecha.ToLongTimeString()} ", Boleta = _ts };
                    borderUltimaVenta.Child = null;
                    if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                        borderUltimaVenta.Child = _iuv;

                    if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    {
                        btnInfo.Click += (se2, a2) =>
                        {
                            try
                            {
                                MostrarOverlay(true);
                                DateTime? inicioJornadaAnterior = JornadaBLL.UltimaJornadaCerrada().fecha_apertura;
                                DateTime? inicioJornadaActual = JornadaBLL.UltimaJornada().fecha_apertura;
                                expPopup.Content = new InfoVentaPopup()
                                {
                                    UsuarioIniciadorJornadaAnterior = JornadaBLL.UltimaJornadaCerrada().usuario?.nombre,
                                    UsuarioIniciadorJornadaActual = JornadaBLL.UltimaJornada().usuario?.nombre,
                                    InicioJornadaAnterior = (DateTime)inicioJornadaAnterior,
                                    InicioJornadaActual = (DateTime)inicioJornadaActual
                                };
                                expPopup.IsExpanded = true;
                            }
                            catch (Exception ex)
                            {
                                PoskException.Make(ex, "Error al abrir popup caja");
                            }
                        };
                    }
                    else
                        btnInfo.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al obtener boleta");
                }


                // items acción:


                ItemEnviar_pedido itemEnviar = new ItemEnviar_pedido();

                Button btnImprimir = new Button() { Content = "imprimir" };
                btnImprimir.Click += (se2, a2) =>
                {
                    //new Notification("Imprimiendo...");
                    //PrintText("asdasdasd");

                    //PrintReceiptForTransaction();
                };
                //borderAccionCentro.Child = btnImprimir;

                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()) || GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
                {
                    if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    {
                        if (spGarzonMesa.Children.OfType<ItemEscogerGarzonSeccionVenta>().ToList().Count == 0)
                            spGarzonMesa.Children.Add(ieg);
                        if (spGarzonMesa.Children.OfType<ItemEscogerMesaSeccionVenta>().ToList().Count == 0)
                            spGarzonMesa.Children.Add(iem);
                        if (spGarzonMesa.Children.OfType<ItemEscogerGarzonMesa>().ToList().Count == 0)
                            spGarzonMesa.Children.Add(iegm);
                    }
                }

                if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    borderAccionDerecha.Child = itemEnviar;
                //mesa _mesa = MesaBLL.Obtener("2");


                itemCalcularTotal.btnDcto.Click += (se2, a2) =>
                {
                    try
                    {
                        int? totalOriginal = 0;
                        spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(x => totalOriginal += x.ObtenerTotal());

                        totalOriginal += itemDcto.DctoPesos;

                        if (totalOriginal == 0 || totalOriginal == null)
                        {
                            new Notification(":(", "Ingresa algo primero...", Notification.Type.Warning);
                            return;
                        }

                        DctoPopup dctoPopup = new DctoPopup(itemDcto, (int)totalOriginal);
                        dctoPopup.Show();
                        MostrarOverlay(true);

                        dctoPopup.Loaded += (se4, a4) =>
                        {
                            dctoPopup.DctoPct = (dctoPopup.DctoPesos * 100) / (int)totalOriginal;
                        };

                        dctoPopup.AlActualizarDescuento += (se4, a4) =>
                        {
                            itemDcto.lbDescuento.Content = $"Descuento: ${dctoPopup.DctoPesos}";
                        };

                        dctoPopup.btnCerrar.Click += (se3, a3) =>
                        {
                            dctoPopup.bCerrado = true;
                            dctoPopup.Close();
                            MostrarOverlay(false);
                        };

                        dctoPopup.Deactivated += (se3, a3) =>
                        {
                            dctoPopup.bCerrado = true;
                            MostrarOverlay(false);
                        };

                        dctoPopup.AlAplicarDcto += (se3, a3) =>
                        {
                            itemDcto.btnCerrar.Click += (se4, a4) =>
                            {
                                gridDcto.Children.Remove(itemDcto);
                                itemDcto.Reset();
                                new Notification("DCTO BORRADO", "$" + dctoPopup.DctoPesos);
                                ReCalcularTotal();
                            };
                            itemDcto.DctoPesos = dctoPopup.DctoPesos;
                            itemDcto.DctoPct = dctoPopup.DctoPct;
                            itemCalcularTotal.txtTotalVenta.Text = dctoPopup.lbTotalConDctoValor.Content.ToString();

                            gridDcto.Children.Remove(itemDcto);
                            gridDcto.Children.Add(itemDcto);

                            MostrarOverlay(false);
                            dctoPopup.bCerrado = true;
                            dctoPopup.Close();
                        };
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al abrir popup dcto");
                    }
                };

                itemCalcularTotal.txtPagaCon.TextChanged += (se2, a2) =>
                {
                    try
                    {
                        itemCalcularTotal.txtVuelto.Text = $"{Convert.ToInt32(itemCalcularTotal.txtPagaCon.Text) - Convert.ToInt32(itemCalcularTotal.txtTotalVenta.Text)}";
                    }
                    catch
                    {
                        itemCalcularTotal.txtVuelto.Clear();
                        itemCalcularTotal.txtPagaCon.Clear();
                        listaItemsMoneda1.ForEach(x => x.Reiniciar());
                        listaItemsMoneda2.ForEach(x => x.Reiniciar());
                    }
                };

                itemCalcularTotal.txtPagaCon.GotFocus += (se2, a2) =>
                {
                    itemCalcularTotal.txtPagaCon.Clear();
                    expTecladoPagar.IsExpanded = true;

                    listaItemsMoneda1.ForEach(x => ConfigurarItemMoneda(x));
                    listaItemsMoneda2.ForEach(x => ConfigurarItemMoneda(x));
                };

                spContenidoSeccionLista.Children.Clear();
                if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    spContenidoSeccionLista.Children.Add(itemCalcularTotal);

                itemEnviar.btnEnviar.Click += (se2, a2) =>
                {
                    if (Settings.Usuario.tipo.ToLower().Equals("g"))
                    {
                        try
                        {
                            usuario u = Settings.Usuario;
                            foreach (ItemVenta iv in spVentaItems.Children.OfType<ItemVenta>().ToList())
                            {
                                decimal? cantidad = iv.Cantidad;
                                for (int i = 1; i <= cantidad; i++)
                                {
                                    if (iv.Producto?.id != null)
                                    {
                                        producto prod = ProductoBLL.GetProduct(iv.Producto?.id);
                                        // ingresar pendiente a base de datos
                                        PendienteBLL.IngresarPendienteProducto(prod, u, DateTime.Now);
                                    }
                                    else if (iv.Promocion?.id != null)
                                    {
                                        promocione promo = PromocionBLL.ObtenerPromocion(iv.Promocion.id);
                                        // ingresar pendiente a base de datos
                                        PendienteBLL.IngresarPendientePromo(promo, u, DateTime.Now);
                                    }
                                }
                            }

                            LimpiarTodo();
                            SyncBLL.AumentarSyncId("pedido");
                            new Notification("ENVIADO");
                            /*
                            Notification notificacion = new Notification("ENVIADO");
                            notificacion.Left = 0;
                            notificacion.Top = -400;
                            */
                        }
                        catch (Exception ex)
                        {
                            PoskException.Make(ex, "Error al enviar un pedido");
                        }
                    }
                    else
                    {
                        try
                        {
                            if (spVentaItems.Children.Count == 0)
                                return;

                            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
                            {
                                try
                                {
                                    int puntos = 0;
                                    foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                                    {
                                        if (item.Producto?.puntos_cantidad != null)
                                            puntos += (int)item.Producto?.puntos_cantidad;
                                    }
                                    int totalVenta = 0;
                                    if (itemCalcularTotal.txtTotalVenta.Text != "")
                                        totalVenta = Convert.ToInt32(itemCalcularTotal.txtTotalVenta?.Text);

                                    RealizarVentaSushi rvs = new RealizarVentaSushi(totalVenta, puntos);
                                    rvs.Show();
                                    MostrarOverlay(true);

                                    rvs.Deactivated += (se3, a3) => { MostrarOverlay(false); };
                                    rvs.AlCerrar += (se3, a3) =>
                                    {
                                        rvs.bCerrado = true;
                                        rvs.Close();
                                        MostrarOverlay(false);
                                    };
                                    rvs.AlVender += (se3, di) =>
                                    {
                                        int _calcularTotal = 0;
                                        if (itemCalcularTotal?.txtTotalVenta?.Text != "")
                                            _calcularTotal = Convert.ToInt32(itemCalcularTotal?.txtTotalVenta?.Text);

                                        List<boleta_mediopago> listaBMP = new List<boleta_mediopago>();


                                        cliente cli = ClienteBLL.GetClient(di.NombreCliente);
                                        BoletaBLL.Set(0, Settings.Usuario.id, puntos, _calcularTotal, di.Propina, cli?.id);
                                        PuntoBLL.Sumar(cli?.puntos_id, puntos);
                                        boleta ultimaBoleta = BoletaBLL.ObtenerUltima();
                                        int medioPagoId = 1;

                                        if (di.Efectivo != 0)
                                        {
                                            medioPagoId = 1;
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, medioPagoId, di.Efectivo, Settings.Usuario.id);
                                        }
                                        if (di.TransBank != 0)
                                        {
                                            medioPagoId = 2;
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, medioPagoId, di.TransBank, Settings.Usuario.id);
                                        }
                                        if (di.Junaeb != 0)
                                        {
                                            medioPagoId = 3;
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, medioPagoId, di.Junaeb, Settings.Usuario.id);
                                        }
                                        if (di.Otro != 0)
                                        {
                                            medioPagoId = 4;
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, medioPagoId, di.Otro, Settings.Usuario.id);
                                        }

                                        foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                                        {
                                            detalle_boleta dl = new detalle_boleta()
                                            {
                                                producto_id = item.Producto?.id,
                                                promocion_id = item.Promocion?.id,
                                                monto = item.ObtenerTotal(),
                                                cantidad = (int)item.Cantidad,
                                                descuento = 0,
                                                boleta_id = ultimaBoleta.id
                                            };
                                            DetalleBoletaBLL.Agregar(dl);
                                            detalle_boleta dBoleta = DetalleBoletaBLL.ObtenerUltima();
                                            CompraBLL.ReduceStockByProduct(item.Producto?.id, item.Promocion?.id, (int)item.Cantidad);
                                            VentasJornadaBLL.Agregar(JornadaBLL.UltimaJornada().id, dBoleta.id, item.Opcion?.nombre, (int)item.Cantidad, item.Extra_);
                                        }
                                        int? _subTotal = 0;
                                        int _propinaSugerida = 0;
                                        int? _total = 0;

                                        spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(x => _subTotal += x.ObtenerTotal());
                                        expTecladoPagar.IsExpanded = false;
                                        MostrarNotificacion("VENDIDO", "");
                                        if (_subTotal != null && _subTotal != 0)
                                        {
                                            _propinaSugerida = Convert.ToInt32(_subTotal * 0.10);
                                            _total = _propinaSugerida + _subTotal;
                                        }

                                        GenerarTicket(_subTotal, ((usuario)ieg.cbGarzones.SelectedItem)?.nombre, ((mesa)iem.cbMesas.SelectedValue)?.codigo, "", "Ticket Caja", di);

                                        LimpiarTodo();
                                        bool bServir = di.ServirLlevar.ToUpper() == "SERVIR" ? true : false;

                                        DeliveryItemBLL.Crear(ultimaBoleta.id, null, DateTime.Now, di.Direccion, di.NombreCliente, null, "", di.Incluye, bServir, di.PagaCon, di.Vuelto, medioPagoId);
                                        CargarDeliveryPendientesDeEntrega();
                                    };
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    PoskException.Make(ex, "ERROR AL MOSTRAR RV POPUP");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            PoskException.Make(ex, "ERROR AL VENDER");
                        }
                    }
                };
            };
        }

        private void CargarDeliveryPendientesDeEntrega()
        {
            spDerecha.Children.Clear();
            DeliveryItemBLL.ObtenerPendientesDeEntrega().ForEach(d =>
            {
                ItemDelivery id = new ItemDelivery()
                {
                    Direccion = d.direccion,
                    NombreCliente = d.nombre_cliente,
                    Cliente = d.cliente,
                    Comentario = d.comentario,
                    Incluye = d.incluye,
                    FechaEntrega = d.fecha_entrega,
                    FechaPedido = d.fecha_pedido,
                    Boleta = d.boleta,
                    DeliveryItem = d
                };

                id.btnItem.Click += (se, a) =>
                {
                    DeliveryPopup dp = new DeliveryPopup(d);
                    dp.Deactivated += (se2, a2) => MostrarOverlay(false);
                    dp.AlEntregar += (se2, a2) =>
                    {
                        MostrarOverlay(false);
                        dp.Cerrar();
                        CargarDeliveryPendientesDeEntrega();
                    };
                    dp.Show();
                    MostrarOverlay(true);
                };
                spDerecha.Children.Add(id);
            });
        }

        bool bYaRealizado = false;

        private void GenerarTicket(int? subTotal, string garzon, string mesa, string pedidoId, string docName = "Ticket", DeliveryInfo di = null)
        {
            try
            {
                CrearTicket ticket = new CrearTicket();

                bool bImprimirEnCocina = false;
                bool bEsPedido = false;
                bool bParaBar = false;
                bool bImprimir = false;

                bool bContieneParaCocina = false;
                if (!string.IsNullOrEmpty(pedidoId))
                    bEsPedido = true;

                if (!bEsPedido)
                {
                    ticket.TextoCentro($"{Settings.NombreDelNegocio}");
                    ticket.AbreCajon();
                }
                else
                    ticket.TextoCentro($"TICKET COCINA");

                ticket.TextoIzquierda("");

                if (!bEsPedido)
                {
                    if (!string.IsNullOrEmpty(mesa))
                        ticket.TextoIzquierda($"MESA: {mesa}".ToUpper());
                    ticket.TextoDerecha($"ID: {BoletaBLL.ObtenerUltimoNumeroBleta()}");
                }
                else
                {
                    if (!string.IsNullOrEmpty(mesa))
                        ticket.TextoExtremos($"MESA: {mesa}".ToUpper(), $"ID: {BoletaBLL.ObtenerUltimoNumeroBleta()}");
                    else
                        ticket.TextoDerecha($"ID: {BoletaBLL.ObtenerUltimoNumeroBleta()}");
                }

                if (!string.IsNullOrEmpty(garzon))
                    ticket.TextoIzquierda($"GARZON: {garzon}".ToUpper());

                ticket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                ticket.TextoIzquierda("");

                if (spVentaItems.Children.OfType<ItemVenta>().ToList().Count != 0)
                {
                    foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                    {
                        if (item.Producto?.sector_impresion?.nombre != "NINGUNO")
                            bImprimir = true;

                        bContieneParaCocina = true;
                        if (item.Producto?.sector_impresion?.nombre == "COCINA")
                            bImprimirEnCocina = true;

                        string espaciosStr = "";
                        string espaciosValor = "";
                        switch ($"{item.ObtenerTotal()}".Length)
                        {
                            case 3:
                                espaciosValor = "  ";
                                break;
                            case 4:
                                espaciosValor = " ";
                                break;
                            case 5:
                                espaciosValor = "";
                                break;
                            default:
                                break;
                        }


                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre} ".Length; i++)
                            espaciosStr += ".";

                        if (bEsPedido)
                        {
                            if (item.Cantidad < 10)
                            {
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} ".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                            }
                        }
                        else
                        {
                            if (item.Cantidad < 10)
                            {
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                            }

                        }

                        if (item.Opcion != null)
                        {
                            if (!item.Opcion.nombre.Contains("N/A"))
                                ticket.TextoIzquierda($"TIPO: {item.Opcion.nombre}");
                        }
                        if (item.listaIngredientes != null)
                            ticket.TextoIzquierda($"{item?.ObtenerIngredientesStr()}");
                        if (!string.IsNullOrEmpty(item.txtNota?.Text))
                            ticket.TextoIzquierda("   " + item.txtNota?.Text.ToUpper());
                        ticket.TextoIzquierda("");

                        if (bEsPedido == true)
                            ticket.lineasGuion();
                    }
                    ticket.TextoIzquierda("");
                    if (bEsPedido)
                    {
                        if (!string.IsNullOrEmpty(imc.txtMensaje.Text) && (((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "COCINA" || ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "TODOS"))
                        {
                            ticket.TextoIzquierda("");
                            ticket.TextoIzquierda($"Mensaje: {imc.txtMensaje.Text}".ToUpper());
                        }
                    }
                }

                if (di != null && bEsPedido)
                {
                    ticket.TextoCentro("");
                    if (di.ServirLlevar != "")
                        ticket.TextoCentro($"Para {di.ServirLlevar.ToLower()}");
                    if (di.Incluye != "" && !di.Incluye.ToLower().Contains("sin adicional"))
                        ticket.TextoCentro($"{di.IncluyeStrUnaLinea}");
                    if (di.MensajeTicket != "")
                        ticket.TextoCentro($"Mensaje cocina: {di.MensajeTicket}");
                    ticket.TextoCentro("");
                }
                if (di != null && !bEsPedido)
                {
                    if (di.NombreCliente != "")
                        ticket.TextoCentro($"Cliente: {di.NombreCliente}");
                    if (di.Telefono != "" && di.Telefono.Replace(" ", "") != "+569")
                        ticket.TextoCentro($"Teléfono: {di.Telefono}");
                    if (di.Direccion != "")
                        ticket.TextoCentro($"Direccion: {di.Direccion}");
                    if (di.MensajeDeliveryUno != "")
                        ticket.TextoCentro($"{di.MensajeDeliveryUno}");
                    if (di.PagaCon != 0)
                        ticket.TextoCentro($"PagaCon: {di.PagaCon}");
                    if (!String.IsNullOrEmpty(di.Vuelto))
                        ticket.TextoCentro($"Vuelto: {di.Vuelto}");
                }

                if (!bEsPedido)
                {
                    ticket.TextoIzquierda("");
                    if (di.Propina != 0)
                    {
                        int? total = di.Propina + subTotal;
                        ticket.TextoIzquierda($"SubTotal:                     ${subTotal}".ToUpper());
                        ticket.TextoIzquierda($"Propina:                      $ {di.Propina}".ToUpper());
                        ticket.TextoIzquierda($"Total:                        ${total}".ToUpper());
                    }
                    else
                    {
                        ticket.TextoIzquierda($"Total:                        ${subTotal}".ToUpper());
                    }
                }
                else
                {
                    //if (!string.IsNullOrEmpty(mensajeCocina))
                    //    ticket.TextoIzquierda($"Mensaje cocina: {mensajeCocina}".ToUpper());
                }

                ticket.CortaTicket();

                if (bEsPedido && !bParaBar && bImprimir == true)
                    ticket.ImprimirTicket(Settings.ImpresoraCocina, "Ticket Cocina");

                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()) && bYaRealizado == false && bImprimir == true)
                {
                    bYaRealizado = true;
                    ticket.ImprimirTicket(Settings.ImpresoraBar, "Ticket Caja");
                    GenerarTicket(subTotal, garzon, mesa, "-1", docName, di);
                    bYaRealizado = false;
                }
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al generar el ticket");
            }
        }

        private void CargarSeccionStock()
        {
            Loaded += (se, a) =>
            {
                lbInfo.Content = "SECCIÓN STOCK";
                //lbNombreLista.Content = "STOCK";

                ItemEnviar_pedido itemEnviar = new ItemEnviar_pedido();
                borderAccionDerecha.Child = itemEnviar;

                itemEnviar.btnEnviar.Click += (se2, a2) =>
                {
                    ComprarPopup2 cp = new ComprarPopup2(spVentaItems);
                    cp.Show();

                    //expPopup.Content = cp;
                    MostrarOverlay(true);

                    cp.AlComprar += (se3, spCompras) =>
                    {
                        LimpiarTodo();
                        string contenidoCorreo = "* * * * C O M P R A * * * *\n\n";
                        foreach (PurchaseLineControl item in spCompras.Children)
                        {
                            contenidoCorreo += $"{item.lbProducto?.Content}\nUnitario bruto: ${item.txtCostoUnitarioBruto.Text}\nTotal bruto: ${item.txtTotalBruto.Text}\n\n";
                        }
                        //EnviarCorreo.Enviar(contenidoCorreo);
                        cp.bCerrado = true;
                        cp.Close();
                    };
                    cp.AlCerrar += (se3, a3) =>
                    {
                        MostrarOverlay(false);
                        expPopup.IsExpanded = false;
                        cp.bCerrado = true;
                        cp.Close();
                    };

                };
            };
        }

        private void CargarSeccionDevolucion()
        {
            Loaded += (se, a) =>
            {
                lbInfo.Content = "SECCIÓN DEVOLUCIÓN";
                //lbNombreLista.Content = "DEVOLUCIÓN";

                ItemEnviar_pedido itemEnviar = new ItemEnviar_pedido();
                borderAccionDerecha.Child = itemEnviar;

                itemEnviar.btnEnviar.Click += (se2, a2) =>
                {
                    try
                    {
                        spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(iv =>
                           DevolucionBLL.Agregar(iv.Producto?.id, iv.Promocion?.id, iv.Cantidad, (int)iv.ObtenerTotal()));
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "ERROR AL DEVOLVER ITEM VENTA");
                    }
                    LimpiarTodo();
                    new Notification("DEVUELTO");
                };
            };
        }

        private void CargarSeccionMerma()
        {
            Loaded += (se, a) =>
            {
                lbInfo.Content = "SECCIÓN MERMA";
                //lbNombreLista.Content = "MERMA";

                ItemEnviar_pedido itemEnviar = new ItemEnviar_pedido();
                borderAccionDerecha.Child = itemEnviar;

                itemEnviar.btnEnviar.Click += (se2, a2) =>
                {
                    try
                    {
                        spVentaItems.Children.OfType<ItemVentaPedido>().ToList().ForEach(iv =>
                            MermaBLL.Agregar(iv.Producto.id, iv.Cantidad));
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "ERROR AL MERMAR ITEM VENTA");
                    }
                    LimpiarTodo();
                    new Notification("MERMADO");
                };
            };
        }

        #endregion secciones

        #region Eventos

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void InitEvents()
        {
            txtCantidad.KeyDown += (se, a) =>
            {
                if (a.Key == Key.Right)
                    txtBarCode.Focus();
            };
            txtCantidad.GotFocus += (se, a) =>
            {
                txtCantidad.Clear();
            };

            txtBarCode.KeyDown += (se, a) =>
            {
                if (a.Key == Key.Enter)
                {
                    producto productoDesdeCodigo = ProductoBLL.ObtenerPorCodigo(txtBarCode.Text);
                    if (productoDesdeCodigo != null)
                    {
                        CrearItemVenta(productoDesdeCodigo, null, Convert.ToInt32(txtCantidad.Text));
                        new Notification("Item Agregado", $"{productoDesdeCodigo.nombre} x{txtCantidad.Text}");
                        txtBarCode.Text = "";
                        txtCantidad.Text = "1";
                        txtBarCode.Focus();
                    }
                    else
                    {
                        new Notification("No encontrado", $"Codigo {txtBarCode.Text}");
                        txtBarCode.Clear();
                        txtBarCode.Focus();
                    }
                }
                if (a.Key == Key.Left)
                    txtCantidad.Focus();
            };

            switch (Modo)
            {
                case "VENTA":
                    CargarSeccionVenta();
                    break;
                case "STOCK":
                    CargarSeccionStock();
                    break;
                case "DEVOLUCION":
                    CargarSeccionDevolucion();
                    break;
                case "MERMA":
                    CargarSeccionMerma();
                    break;
                default:
                    CargarSeccionVenta();
                    break;
            }
            Loaded += (se, a) =>
            {
                string tipoUsuario = "";
                if (Settings.Usuario?.tipo.ToUpper() == "G")
                    tipoUsuario = "Garzón";
                else if (Settings.Usuario?.tipo.ToUpper() == "C")
                    tipoUsuario = "Cajero(a)";
                else if (Settings.Usuario?.tipo.ToUpper() == "A")
                    tipoUsuario = "Admin";
                lbUsuario.Content = $"{tipoUsuario}: {Settings.Usuario?.nombre}";

                CrearMenuBusquedaPorCategoria();
            };

            btnCrearDelimitador.Click += (se, a) =>
            {
                bDelimitadorPresionado ^= true;
                var colorPlomo = new SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 210, 210));

                //Establecer los item venta existentes en entrada
                //agregar delimitador a spVentaItems

                if (bDelimitadorPresionado)
                {
                    btnCrearDelimitador.Background = colorPlomo;
                    if (spVentaItems.Children.OfType<ItemDelimitador>().ToList().Count == 0)
                    {
                        spVentaItems.Children.Add(itemDelimitador);
                        // dejar como entrada a todos los item venta existentes
                        foreach (ItemVenta iv in spVentaItems.Children.OfType<ItemVenta>().ToList())
                        {
                            iv.Entrada = true;
                        }
                    }
                }
                else
                {
                    btnCrearDelimitador.Background = null;
                    if (spVentaItems.Children.OfType<ItemDelimitador>().ToList().Count != 0)
                        spVentaItems.Children.Remove(itemDelimitador);
                }
            };

            btnLimpiarBusqueda.Click += (se, a) =>
            {
                iconFavorito.Foreground = colorDorado;
                // descomentar fav
                MostrarPedidos((ProductoBLL.GetFavs()));
                txtBuscar.Text = "FAVORITOS";
                bFavorito = true;

                iconPromo.Foreground = colorNeutro;
                bPromo = false;
            };

            btnLimpiarVenta.Click += (se, a) =>
            {
                spVentaItems.Children.Clear();
                if (itemCalcularTotal != null)
                {
                    itemCalcularTotal.txtPagaCon.Clear();
                    itemCalcularTotal.txtVuelto.Clear();
                    itemCalcularTotal.txtTotalVenta.Clear();
                }
            };

            txtBuscar.GotFocus += (se, a) =>
            {
                txtBuscar.Text = "";
                iconFavorito.Foreground = colorNeutro;
                bFavorito = false;

                iconPromo.Foreground = colorNeutro;
                bPromo = false;
            };

            txtBuscar.TextChanged += (se, a) =>
            {
                if (txtBuscar.Text != "")
                {
                    MostrarPedidos(ProductoBLL.ObtenerCoincidencias(txtBuscar.Text));
                }
                else
                {
                    wrapProductos.Children.Clear();
                    txtBuscar.Foreground = colorNeutro;
                }
            };

            btnExpanderLeft.Click += (se, a) =>
            {
                AbrirCategoriasToogle();
            };

            btnFavorito.Click += (se, a) =>
            {
                iconPromo.Foreground = colorNeutro;
                bPromo = false;
                if (!bFavorito)
                {
                    iconFavorito.Foreground = colorDorado;
                    // descomentar fav
                    MostrarPedidos((ProductoBLL.GetFavs()));
                    txtBuscar.Text = "FAVORITOS";
                    txtBuscar.Foreground = colorDorado;
                    bFavorito = true;
                }
                else
                {
                    txtBuscar.Text = "";
                    iconFavorito.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    wrapProductos.Children.Clear();
                    bFavorito = false;
                }
            };

            btnPromo.Click += (se, a) =>
            {
                iconFavorito.Foreground = colorNeutro;
                bFavorito = false;
                if (!bPromo)
                {
                    iconPromo.Foreground = colorDorado;
                    // descomentar fav
                    MostrarPedidos((ProductoBLL.GetPromos()));
                    txtBuscar.Text = "PROMOS";
                    txtBuscar.Foreground = colorDorado;
                    bPromo = true;
                }
                else
                {
                    txtBuscar.Text = "";
                    iconPromo.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    wrapProductos.Children.Clear();
                    bPromo = false;
                }
            };

            overlay.MouseLeftButtonDown += (se, e) => MostrarOverlay(false);

            btnCerraTecladoMoneda.Click += (se, a) => expTecladoPagar.IsExpanded = false;

            expLeft.MouseEnter += (se, a) => AbrirCategorias(true);
            expLeft.MouseLeave += (se, a) => AbrirCategorias(false);

            expDerecha.MouseEnter += (se, a) => AbrirPendientes(true);
            expDerecha.MouseLeave += (se, a) => AbrirPendientes(false);
        }

        #endregion Eventos

        private void AbrirCategorias(bool b)
        {
            expLeft.IsExpanded = b;
            if (expLeft.IsExpanded)
                btnExpanderLeft.Foreground = colorDorado;
            else
                btnExpanderLeft.Foreground = colorNeutro;
        }

        private void AbrirPendientes(bool b)
        {
            expDerecha.IsExpanded = b;
        }

        private void AbrirCategoriasToogle()
        {
            expLeft.IsExpanded ^= true;
            if (expLeft.IsExpanded)
                btnExpanderLeft.Foreground = colorDorado;
            else
                btnExpanderLeft.Foreground = colorNeutro;
        }
    }
}
