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
        private List<ItemVentaPedido> listaItemsPedidoEliminarDesdeBD;
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

            // Crea los agregados
            //var btnCerrarAgregados = new Button() { Content = "x" };
            //btnCerrarAgregados.Click += (se, a) =>
            //{
            //    expBottom.IsExpanded = false;
            //    DeseleccionarProductoAgregado();
            //};
            //wrapAgregados.Children.Add(btnCerrarAgregados);

            // Crea los item agregado
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

            AgregadoBLL.ObtenerPaltaCebollin().ForEach(item =>
            {
                var ia = new ItemAgregadoHandroll(true) { Agregado = item };
                ia.btnAgregado.Click += (se, a) =>
                {
                    itemPaltaCebollin = ia;
                    expBottomPaltaCebollin.IsExpanded = false;
                    expBottomAgregadosHandroll.IsExpanded = true;
                };
                wrapPaltaCebollin.Children.Add(ia);
            });

            // CargarEnvolturas();

            List<ItemAgregadoHandroll> listaAgregadosParaHandroll = new List<ItemAgregadoHandroll>();

            CrearAgregadosHandroll();

            btnCrearItemVentaEspecialHandroll.Click += (se, a) =>
            {
                List<ItemAgregadoHandroll> listaAgregados = wrapAgregadosHandroll.Children.OfType<ItemAgregadoHandroll>().Where(x => x.Cantidad > 0).ToList();
                if (listaAgregados.Count > 0)
                    IncluirRolloSushi(listaAgregados);
                else
                    new Notification("NO SE PUDO", "DEBES INCLUIR INGREDIENTES", Notification.Type.Warning);
            };

            // Crea los item preparado especial
            PreparacionesBLL.ObtenerTodos().ForEach(prep =>
            {
                var ip = new ItemPreparacion() { Preparacion = prep };
                ip.btnPreparacion.Click += (se, a) =>
                {
                    //if (itemAgregadoUno.Agregado == null && itemAgregadoDos.Agregado == null)
                    //itemAgregadoUno.Agregado = itemLista;
                    //else if (itemAgregadoDos.Agregado == null && itemAgregadoUno.Agregado != null)
                    //itemAgregadoDos.Agregado = itemLista;
                    //if (itemAgregadoDos.Agregado != null && itemAgregadoUno.Agregado != null && itemProductoSeleccionado.Producto != null)

                    //Incluir();
                    //var ipSeleccion = new ItemPreparacion() { Preparacion = p };
                    //ipSeleccion.btnPreparacion.Click += (se2, a2) => wrapPreparadoEspecialSeleccion.Children.Remove(se2 as ItemPreparacion);

                    // usar en sushi:
                    //listaItemsPreparadoEspecial.Add(p);
                    //IncluiraVentaPreparadoEspecial();

                    listaItemsPreparadoEspecial.Add(prep);
                    // usar en sushi
                    //var itemVentaExtendido = new ItemVentaPreparadoEspecial() { Producto = itemProductoSeleccionado.Producto, ListaItemsPreparado = listaItemsPreparadoEspecial };

                    //var itemVentaExtendido = new ItemVentaPlatoFondo() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado, Preparacion = prep };
                    var itemVentaExtendido = new ItemVenta() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado, Preparacion = prep };

                    itemVentaExtendido.AlEliminar += (se2, a2) =>
                    {
                        spVentaItems.Children.Remove(itemVentaExtendido);
                        CalcularTotal();
                    };
                    itemVentaExtendido.AlModificarCantidad += (se4, a4) => CalcularTotal();
                    itemVentaExtendido.AlModificarTotal += (se4, a4) => CalcularTotal();
                    spVentaItems.Children.Add(itemVentaExtendido);
                    DeseleccionarProductoAgregado();
                    CalcularTotal();
                    expBottomPreparadoEspecial.IsExpanded = false;
                    listaItemsPreparadoEspecial.Clear();
                };
                wrapPreparadoEspecial.Children.Add(ip);
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

        private void CrearAgregadosHandroll()
        {
            wrapAgregadosHandroll.Children.Clear();
            AgregadoBLL.ObtenerTodos().Where(a => a.para_handroll == true).ToList().ForEach(itemLista =>
            {
                var ia = new ItemAgregadoHandroll() { Agregado = itemLista };
                wrapAgregadosHandroll.Children.Add(ia);
            });
        }

        private void IncluiraVentaPreparadoEspecial()
        {
            wrapPreparadoEspecialSeleccion.Children.Clear();
            listaItemsPreparadoEspecial.ForEach(p =>
            {
                var ip = new ItemPreparacion() { Preparacion = p };
                ip.btnPreparacion.Click += (se, a) =>
                {
                    listaItemsPreparadoEspecial.Remove(p);
                    IncluiraVentaPreparadoEspecial();
                };
                wrapPreparadoEspecialSeleccion.Children.Add(ip);
            });

            var btnIncluir = new Button() { Content = "Incluir" };
            btnIncluir.Click += (se, a) =>
            {
                // usar en sushi
                // var itemVentaExtendido = new ItemVentaPreparadoEspecial() { Producto = itemProductoSeleccionado.Producto, ListaItemsPreparado = listaItemsPreparadoEspecial };
                //var itemVentaExtendido = new ItemVentaPlatoFondo() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado };
                var itemVentaExtendido = new ItemVenta() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado };
                itemVentaExtendido.AlEliminar += (se2, a2) =>
                {
                    spVentaItems.Children.Remove(itemVentaExtendido);
                    CalcularTotal();
                };
                itemVentaExtendido.AlModificarCantidad += (se4, a4) => CalcularTotal();
                itemVentaExtendido.AlModificarTotal += (se4, a4) => CalcularTotal();
                spVentaItems.Children.Add(itemVentaExtendido);
                DeseleccionarProductoAgregado();
                CalcularTotal();

                listaItemsPreparadoEspecial.Clear();
            };
            wrapPreparadoEspecialSeleccion.Children.Add(btnIncluir);
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                Sync("pedido");
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
            // descomentar fav
            MostrarPedidos((ProductoBLL.GetFavs()));
            txtBuscar.Text = "FAVORITOS";
            txtBuscar.Foreground = colorDorado;
            bFavorito = true;
            bPromo = false;

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
        /// Incluye un plato de fondo a la sección venta (cuando tiene 2 agregados)
        /// </summary>
        private void Incluir()
        {
            try
            {
                if (ProductoYaSeleccionado() && AgregadosSeleccionados() == 2)
                {
                    if (itemProductoSeleccionado.Producto.preparado_especial == true)
                    {
                        expBottom.IsExpanded = false;
                        expBottomPreparadoEspecial.IsExpanded = true;
                        return;
                    }
                    string agregadoStr = "";
                    if (itemAgregadoUno.Agregado == itemAgregadoDos.Agregado)
                        agregadoStr = $"{itemAgregadoUno.Agregado.nombre}";
                    else
                        agregadoStr = $"{itemAgregadoUno.Agregado.nombre} y {itemAgregadoDos.Agregado.nombre}";
                    MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre}", agregadoStr);

                    //var itemVentaExtendido = new ItemVentaPlatoFondo() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado };
                    var itemVentaExtendido = new ItemVenta() { Producto = itemProductoSeleccionado.Producto, AgregadoUno = itemAgregadoUno.Agregado, AgregadoDos = itemAgregadoDos.Agregado };
                    itemVentaExtendido.AlEliminar += (se, a) =>
                    {
                        spVentaItems.Children.Remove(itemVentaExtendido);
                        CalcularTotal();
                    };
                    itemVentaExtendido.AlModificarCantidad += (se4, a4) => CalcularTotal();
                    itemVentaExtendido.AlModificarTotal += (se4, a4) => CalcularTotal();
                    spVentaItems.Children.Add(itemVentaExtendido);
                    DeseleccionarProductoAgregado();
                    CalcularTotal();
                    expBottom.IsExpanded = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }


        private void IncluirRolloSushi(List<ItemAgregadoHandroll> listaAgregados, producto producto = null, envoltura envoltura = null)
        {
            try
            {
                if (ProductoYaSeleccionado())
                {
                    envoltura _envoltura = envoltura;
                    producto _producto = producto;

                    if (producto == null) _producto = itemProductoSeleccionado.Producto;

                    if (_envoltura == null)
                    {
                        // no tiene envoltura y requiere una, abrir popup para solicitar
                        if (_producto.es_envoltura == true && itemEnvoltura.Envoltura.nombre == null)
                        {
                            expBottom.IsExpanded = false;
                            expBottomPreparadoEspecial.IsExpanded = false;
                            expBottomEnvolturasHandroll.IsExpanded = true;
                        }
                        _envoltura = itemEnvoltura.Envoltura;
                    }

                    string agregadosStr = "";
                    listaAgregados.ForEach(a =>
                    {
                        if (a.Cantidad == 1)
                            agregadosStr += $"{a.Agregado.nombre}";
                        else
                            agregadosStr += $"{a.Agregado.nombre} x{a.Cantidad}, ";

                    });
                    try
                    {
                        if (agregadosStr != "")
                            agregadosStr = agregadosStr.Substring(0, agregadosStr.Length - 2);

                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "ERROR AL QUITAR ULTIMA COMA");
                    }

                    MostrarNotificacion($"{_producto.nombre}", "");



                    var itemVentaExtendido = new ItemVenta();

                    var cantidadIngredientes = 0;
                    var cobroExtra = 0;
                    listaAgregados.Where(x => x.Cantidad > 0).ToList().ForEach(a => cantidadIngredientes += a.Cantidad);

                    // cobro extra desde el quinto ingrediente
                    if (cantidadIngredientes >= 5) cobroExtra = 500 * (cantidadIngredientes - 5);

                    if (_envoltura != null)
                        itemVentaExtendido = new ItemVenta() { Producto = _producto, listaAgregadosSushi = listaAgregados, Envoltura = _envoltura, Extra = cobroExtra };
                    else
                        itemVentaExtendido = new ItemVenta() { Producto = _producto, listaAgregadosSushi = listaAgregados, Extra = cobroExtra };

                    itemVentaExtendido.AlEliminar += (se, a) =>
                    {
                        spVentaItems.Children.Remove(itemVentaExtendido);
                        CalcularTotal();
                    };
                    itemVentaExtendido.AlModificarCantidad += (se4, a4) => CalcularTotal();
                    itemVentaExtendido.AlModificarTotal += (se4, a4) => CalcularTotal();
                    spVentaItems.Children.Add(itemVentaExtendido);
                    DeseleccionarProductoAgregado();
                    CalcularTotal();
                    expBottomAgregadosHandroll.IsExpanded = false;
                    itemEnvoltura = new ItemEnvoltura();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
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
            expBottomPreparadoEspecial.IsExpanded = false;
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

                            if (Modo == "STOCK" && p.contiene_agregado == true || Modo == "STOCK" && p.solo_venta == true)
                                continue;
                            else if (p.contiene_agregado == true)
                            {
                                var ip = new ItemProducto() { Producto = p };

                                ip.AlDeseleccionar += (se3, a3) =>
                                {
                                    expBottomPreparadoEspecial.IsExpanded = false;
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
                                    if (itemProductoSeleccionado != null && itemAgregadoUno != null && itemAgregadoDos != null)
                                        Incluir();
                                    teclado.expTeclado.IsExpanded = false;
                                };

                                ip.MouseDoubleClick += (se2, a2) =>
                                {
                                    if (itemProductoSeleccionado != null)
                                        itemProductoSeleccionado.Reiniciar();

                                    ItemVentaPedido ivpExistente = spVentaItems.Children.OfType<ItemVentaPedido>().ToList().Where(ivp => ivp.Producto.id == ip.Producto.id).FirstOrDefault();

                                    if (ivpExistente != null)
                                    {
                                        ivpExistente.AgregarCantidad(1);
                                        MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre.ToUpper()}", "+1");
                                        // new Notification($"{itemProductoSeleccionado.Producto.nombre.ToUpper()}", "+1", Notification.Type.Successful, 1, Notification.Size.Sm, Notification.Position.Top);
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

                                        MostrarNotificacion($"{itemProductoSeleccionado.Producto.nombre.ToUpper()}", "");
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
                                    expBottomPreparadoEspecial.IsExpanded = false;
                                    expBottom.IsExpanded = false;
                                };

                                ip.btnProducto.Click += (se, a) =>
                                {
                                    ItemVentaPedido ivpExistente = spVentaItems.Children.OfType<ItemVentaPedido>().ToList().Where(ivp => ivp.Producto.id == ip.Producto.id).FirstOrDefault();

                                    if (ivpExistente != null)
                                    {
                                        ivpExistente.AgregarCantidad(1);
                                        MostrarNotificacion($"{ivpExistente.Producto.nombre.ToUpper()}", "+1");
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

                                        MostrarNotificacion($"{ivpNuevo.Producto.nombre.ToUpper()}", "");
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

        private void CargarEnvolturas(producto p)
        {
            wrapEnvolturasHandroll.Children.Clear();
            if (p.es_handroll == true)
            {
                foreach (envoltura env in EnvolturaBLL.ObtenerTodasParaHandroll())
                {
                    var ie = new ItemEnvoltura() { Envoltura = env };
                    ie.btnEnvoltura.Click += (se, a) =>
                    {
                        itemEnvoltura = ie;
                        expBottomEnvolturasHandroll.IsExpanded = false;
                        expBottomPaltaCebollin.IsExpanded = true;
                    };
                    wrapEnvolturasHandroll.Children.Add(ie);
                }
            }
            if (p.es_superhandroll == true)
            {
                foreach (envoltura env in EnvolturaBLL.ObtenerTodasParaSuperHandroll())
                {
                    var ie = new ItemEnvoltura() { Envoltura = env };
                    ie.btnEnvoltura.Click += (se, a) =>
                    {
                        itemEnvoltura = ie;
                        expBottomEnvolturasHandroll.IsExpanded = false;
                        expBottomAgregadosHandroll.IsExpanded = true;
                    };
                    wrapEnvolturasHandroll.Children.Add(ie);
                }
            }
        }

        private void CrearMostrarItemProducto(producto p)
        {
            if (Modo == "VENTA" && p.solo_compra == true)
                return;
            if (!GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
            {
                // que no muestre el plato con agregado en seccion venta si es que no estamos en modo restaurant
                if (Modo == "VENTA" && p.contiene_agregado == true)
                    return;
            }

            if (p.es_tabla == true)
            {
                var ip = new ItemProducto() { Producto = p };

                ip.btnProducto.Click += (se2, a2) =>
                {
                    ArmarTabla(p);
                };
                wrapProductos.Children.Add(ip);

                return;
            }

            if (p.es_handroll == true || p.es_superhandroll == true || p.es_envoltura == true)
            {
                var ip = new ItemProducto() { Producto = p };
                ip.AlSeleccionar += (se, a) =>
                {
                    ArmarProductoPopup app = new ArmarProductoPopup(p);
                    app.Deactivated += (se2, a2) =>
                    {
                        app.bCerrado = true;
                        MostrarOverlay(false);
                        ip.Reiniciar();
                    };
                    MostrarOverlay(true);
                    app.Show();
                    app.AlIngresarProductoArmado += (se2, ivArmado) =>
                    {
                        CrearItemVentaDesdeItemVenta(ivArmado);
                        MostrarOverlay(false);
                    };
                };

                wrapProductos.Children.Add(ip);

                return;
                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottomEnvolturasHandroll.IsExpanded = false;
                };
                ip.AlSeleccionar += (se3, a3) =>
                {
                    CargarEnvolturas(p);
                    expBottomEnvolturasHandroll.IsExpanded = true;
                };
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
                    CrearItemVenta(p);
                    teclado.expTeclado.IsExpanded = false;
                };
                wrapProductos.Children.Add(ip);

                return;
            }




            if (p.contiene_agregado == true && p.preparado_especial == false)
            {
                var ip = new ItemProducto() { Producto = p };

                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottomPreparadoEspecial.IsExpanded = false;
                    expBottom.IsExpanded = false;
                };
                ip.AlSeleccionar += (se3, a3) =>
                {
                    expBottom.IsExpanded = true;
                };

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
                    teclado.expTeclado.IsExpanded = false;
                };

                ip.MouseDoubleClick += (se2, a2) =>
                {
                    if (itemProductoSeleccionado != null)
                        itemProductoSeleccionado.Reiniciar();
                    CrearItemVenta(p);
                    teclado.expTeclado.IsExpanded = false;
                };
                wrapProductos.Children.Add(ip);
            }
            else if (p.contiene_agregado == true && p.preparado_especial == true)
            {
                var ip = new ItemProducto() { Producto = p };
                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottomPreparadoEspecial.IsExpanded = false;
                    expBottom.IsExpanded = false;
                };
                ip.btnProducto.Click += (se2, a2) =>
                {
                    if (itemProductoSeleccionado != ip)
                    {
                        if (itemProductoSeleccionado != null)
                            itemProductoSeleccionado.Reiniciar();
                        itemProductoSeleccionado = ip;
                    }
                    if (itemProductoSeleccionado != null && itemAgregadoUno != null && itemAgregadoDos != null)
                    {
                        expBottomPreparadoEspecial.IsExpanded = false;
                        expBottom.IsExpanded = true;
                    }
                    // Incluir();
                    teclado.expTeclado.IsExpanded = false;
                };
                wrapProductos.Children.Add(ip);
            }
            else if (p.preparado_especial == true && p.contiene_agregado == false)
            {
                var ip = new ItemProducto() { Producto = p };

                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottomPreparadoEspecial.IsExpanded = false;
                    expBottom.IsExpanded = false;
                };
                ip.AlSeleccionar += (se3, a3) => expBottomPreparadoEspecial.IsExpanded = true;

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
                    teclado.expTeclado.IsExpanded = false;
                };

                ip.MouseDoubleClick += (se2, a2) =>
                {
                    if (itemProductoSeleccionado != null)
                        itemProductoSeleccionado.Reiniciar();
                    CrearItemVenta(p);
                    teclado.expTeclado.IsExpanded = false;
                };
                wrapProductos.Children.Add(ip);
            }
            else
            {
                var ip = new ItemProducto() { Producto = p };

                ip.AlDeseleccionar += (se3, a3) =>
                {
                    expBottomPreparadoEspecial.IsExpanded = false;
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
                    MostrarNotificacion($"{ivExistente.Producto.nombre.ToUpper()}", "+" + cantidad);
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
                    MostrarNotificacion($"{ivNuevo.Producto.nombre.ToUpper()}", "");
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
            MostrarNotificacion($"{_iv.Producto.nombre.ToUpper()}", "");
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
                            p = ProductoBLL.GetProduct(itemPend.Producto.id);
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

        private void CargarPendientesRestaurant()
        {
            List<pedido> listaPedidosPorMesa = new List<pedido>();
            int? mesaAnteriorId = null;

            foreach (pedido x in PedidoBLL.ObtenerTodos().OrderBy(xs => xs.mesa_id))
            {
                if (mesaAnteriorId != null && mesaAnteriorId == x.mesa_id)
                    continue;
                if (Settings.Usuario.tipo.ToLower() == "g" && x.usuario_id != Settings.Usuario.id)
                    continue;

                mesaAnteriorId = x.mesa_id;

                listaPedidosPorMesa = PedidoBLL.ObtenerPedidosPorMesa(x.mesa_id);

                //listaPedidosPorMesa = PedidoBLL.ObtenerPedidosPorMesa(x.mesa_id);
                listaPedidosPorMesa.Add(x);

                ItemPendienteMesa ipm = new ItemPendienteMesa() { Usuario = x.usuario, Mesa = x.mesa, Pedido = x, ListaPedidos = listaPedidosPorMesa, Id = x.id };

                ipm.AlEliminar += (se, a) =>
                {
                    if (MessageBox.Show($"¿Cancelar pedido? Mesa: {ipm.Mesa.codigo}", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        //PedidoBLL.Eliminar(ipm.Pedido.id);
                        PedidoBLL.EliminarVariosPorMesa(ipm.ListaPedidos);
                        MesaBLL.LiberarMesa(ipm.Mesa.id);
                        spDerecha.Children.Clear();
                        CargarPendientesRestaurant();
                        CargarItemVerPendientes();

                        CrearTicket ticket = new CrearTicket();
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        //ticket.TextoCentro($"Cancelar pedido ID: {x.id}".ToUpper());
                        ticket.TextoCentro($"Cancelar pedido Mesa: {ipm.Mesa.codigo}".ToUpper());
                        ticket.TextoCentro($"Pedido ID: {ipm.Pedido.id}".ToUpper());
                        ticket.TextoCentro($"({ipm.Pedido.fecha})".ToUpper());
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.CortaTicket();
                        ticket.ImprimirTicket(Settings.ImpresoraBar, "Cancelacion pedido - caja"); // al cancelar pedido
                        int contadorPedidosCocina = 0;

                        ipm.ListaPedidos.ForEach(lp =>
                        {
                            foreach (pedidos_productos pp in lp.pedidos_productos)
                            {
                                if (pp.producto != null)
                                {
                                    if (pp.producto.sector_impresion.nombre.Equals("COCINA"))
                                        contadorPedidosCocina++;
                                }
                            }
                        });
                        if (contadorPedidosCocina != 0)
                            ticket.ImprimirTicket(Settings.ImpresoraCocina, "Cancelacion pedido - cocina"); // al cancelar pedido
                    }
                };
                ipm.AlClickear += (se, a) =>
                {
                    MostrarOverlay(true);

                    // popup seleccionar items del pedido
                    var sipp = new SeleccionarItemsPedidoPopup(ipm.Pedido);
                    sipp.Show();

                    sipp.Deactivated += (se2, a2) => MostrarOverlay(false);

                    List<pedidos_productos> listaPPDesdePopup = new List<pedidos_productos>();
                    sipp.AlRetornarLista += (se2, lista) => listaPPDesdePopup = lista;

                    sipp.AlActualizar += (se2, args) =>
                    {
                        // args[0] Garzón ID (int) 
                        usuario _usuario = UsuarioBLL.ObtenerUsuario(args[0]);

                        // args[1] = Mesa ID (int)
                        mesa _mesa = MesaBLL.Obtener(args[1]);

                        PedidoBLL.ActualizarPedido(ipm.Pedido.id, args[0], args[1]);
                        sipp.Close();
                        spDerecha.Children.Clear();
                        CargarPendientesRestaurant();
                        CargarItemVerPendientes();

                        //PedidoBLL.EliminarVariosPorMesa(ipm.ListaPedidos);
                        //MesaBLL.LiberarMesa(ipm.Mesa.id);
                        //spDerecha.Children.Clear();
                        //CargarPendientesRestaurant();

                        int? subTotal = 0;
                        int propinaSugerida = 0;
                        int? total = 0;
                        listaPPDesdePopup.ForEach(pp =>
                        {
                            if (pp.producto != null)
                                subTotal += pp.producto.precio;
                            else if (pp.promocione != null)
                                subTotal += pp.promocione.precio;
                        });
                        expTecladoPagar.IsExpanded = false;
                        //MostrarNotificacion("IMPRIMIENDO...", "");
                        propinaSugerida = Convert.ToInt32(subTotal * 0.10);
                        total = propinaSugerida + subTotal;

                        string header = "";
                        header += $"Reemplazar Pedido # {ipm.Pedido.id}".ToUpper();

                        GenerarTicketDesdePP(ipm.Pedido, ConvertirListaPPitemVenta(listaPPDesdePopup), "Actualizacion pedido", header, $"{ipm.Pedido.id}", $"{_usuario.nombre}", $"{_mesa.codigo}");
                    };


                    //List<pedidos_productos> listaPP = new List<pedidos_productos>();
                    sipp.AlIngresar2 += (se2, a2) =>
                    {
                        ieg.cbGarzones.Text = $"{a2[0]}";
                        iem.cbMesas.Text = $"{a2[1]}";
                    };

                    sipp.AlPedirCuenta += (se2, listaPP_Seleccionados) =>
                    {
                        int? subTotal = 0;
                        int propinaSugerida = 0;
                        int? total = 0;
                        listaPP_Seleccionados.ForEach(pp =>
                        {
                            if (pp.producto != null)
                                subTotal += pp.producto.precio;
                            else if (pp.promocione != null)
                                subTotal += pp.promocione.precio;
                        });
                        expTecladoPagar.IsExpanded = false;
                        //MostrarNotificacion("IMPRIMIENDO...", "");
                        propinaSugerida = Convert.ToInt32(subTotal * 0.10);
                        total = propinaSugerida + subTotal;

                        GenerarTicketDesdePP(x, ConvertirListaPPitemVenta(listaPP_Seleccionados));
                    };

                    sipp.AlIngresarVenta += (se2, listaPP_Seleccionados) =>
                    {
                        foreach (pedidos_productos pp in listaPP_Seleccionados)
                        {
                            var ppFromDB = PedidosProductosBLL.Obtener(pp.id);
                            if (ppFromDB.producto?.contiene_agregado == true || ppFromDB.producto?.preparado_especial == true)
                            {
                                pedidos_agregados pa = PedidosAgregadosBLL.Obtener(ppFromDB.id);
                                pedidos_preparaciones pedPrep = PedidosPreparacionesBLL.Obtener(ppFromDB.id);

                                agregado agregadoUno = AgregadoBLL.Obtener(pa?.agregado_uno_id);
                                agregado agregadoDos = AgregadoBLL.Obtener(pa?.agregado_dos_id);
                                preparacione preparacion = PreparacionesBLL.Obtener(pedPrep?.preparacion_id);

                                //var ivpPlatoFondoNuevo = new ItemVentaPlatoFondo() { Producto = ppFromDB.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos, Preparacion = preparacion };

                                var ivpPlatoFondoNuevo = new ItemVenta() { Producto = ppFromDB.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos, Preparacion = preparacion, Cantidad = (int)ppFromDB.cantidad };
                                //if (ppFromDB.precio != 0)
                                //    ivpPlatoFondoNuevo.txtTotal.Text = $"{ppFromDB.precio}";


                                ivpPlatoFondoNuevo.AlEliminar += (se3, a3) =>
                                {
                                    spVentaItems.Children.Remove(ivpPlatoFondoNuevo);
                                    CalcularTotal();
                                };
                                ivpPlatoFondoNuevo.AlModificarCantidad += (se4, a4) => CalcularTotal();
                                ivpPlatoFondoNuevo.AlModificarTotal += (se4, a4) => CalcularTotal();
                                spVentaItems.Children.Add(ivpPlatoFondoNuevo);
                            }
                            else
                            {
                                try
                                {
                                    producto prod = new producto();
                                    if (ppFromDB.precio != 0)
                                        prod = new producto() { nombre = ppFromDB.producto.nombre, precio = ppFromDB.precio, sector_impresion_id = ppFromDB.producto.sector_impresion_id, tipo_itemventa_id = ppFromDB.producto.tipo_itemventa_id };
                                    else
                                        prod = new producto() { nombre = ppFromDB.producto.nombre, precio = ppFromDB.producto.precio, sector_impresion_id = ppFromDB.producto.sector_impresion_id, tipo_itemventa_id = ppFromDB.producto.tipo_itemventa_id };


                                    var ivpNuevo = new ItemVenta() { Producto = ppFromDB.producto, Cantidad = Convert.ToInt32(ppFromDB.cantidad)/*, TotalModificado = ppFromDB.precio */};

                                    ivpNuevo.AlEliminar += (se3, a3) =>
                                    {
                                        spVentaItems.Children.Remove(ivpNuevo);
                                        CalcularTotal();
                                    };
                                    ivpNuevo.AlModificarCantidad += (se4, a4) => CalcularTotal();
                                    ivpNuevo.AlModificarTotal += (se4, a4) => CalcularTotal();
                                    spVentaItems.Children.Add(ivpNuevo);
                                }
                                catch (Exception ex)
                                {
                                    PoskException.Make(ex, "Error al convertir");
                                }
                            }
                            PedidosProductosBLL.Eliminar(ppFromDB.id);
                        }

                        int count = 0;
                        ipm.ListaPedidos.ForEach(p =>
                        {
                            PedidosProductosBLL.ObtenerTodos(p.id).ForEach(pp => count++);
                        });
                        if (count == 0)
                        {
                            //PedidoBLL.Eliminar(x.id);
                            PedidoBLL.EliminarVariosPorMesa(ipm.ListaPedidos);
                            MesaBLL.LiberarMesa(ipm.Mesa.id);
                            spDerecha.Children.Clear();
                            CargarPendientesRestaurant();
                            CargarItemVerPendientes();
                            CalcularTotal();
                        }
                        CalcularTotal();
                        sipp.Close();
                    };
                };
                spDerecha.Children.Add(ipm);
            }

            return;
            /*
            PedidoBLL.ObtenerTodos().ForEach(x =>
            {
                listaPedidosPorMesa = PedidoBLL.ObtenerPedidosPorMesa(x.mesa_id);

                //listaPedidosPorMesa = PedidoBLL.ObtenerPedidosPorMesa(x.mesa_id);
                listaPedidosPorMesa.Add(x);

                ItemPendienteMesa ipm = new ItemPendienteMesa() { Usuario = x.usuario, Mesa = x.mesa, Pedido = x, ListaPedidos = listaPedidosPorMesa };
                ipm.AlEliminar += (se, a) =>
                {
                    if (MessageBox.Show("¿Eliminar pedido?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        PedidoBLL.Eliminar(ipm.Pedido.id);
                        spDerecha.Children.Clear();
                        CargarPendientesRestaurant();
                        CargarItemVerPendientes();
                        CrearTicket ticket = new CrearTicket();
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.TextoCentro($"Cancelar pedido ID: {x.id}".ToUpper());
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.TextoIzquierda("");
                        ticket.CortaTicket();
                        ticket.ImprimirTicket("THERMAL Receipt Printer");
                    }
                };
                ipm.AlClickear += (se, a) =>
                {
                    // popup seleccionar items del pedido
                    var sipp = new SeleccionarItemsPedidoPopup(x.id, ipm.Pedido);
                    sipp.Show();

                    List<pedidos_productos> listaPP = new List<pedidos_productos>();
                    sipp.AlIngresarVenta += (se2, listaPP_Seleccionados) =>
                    {
                        foreach (pedidos_productos pp in listaPP_Seleccionados)
                        {
                            var ppFromDB = PedidosProductosBLL.Obtener(pp.id);
                            if (ppFromDB.producto?.contiene_agregado == true)
                            {
                                pedidos_agregados pa = PedidosAgregadosBLL.Obtener(ppFromDB.id);
                                agregado agregadoUno = AgregadoBLL.Obtener(pa?.agregado_uno_id);
                                agregado agregadoDos = AgregadoBLL.Obtener(pa?.agregado_dos_id);

                                //var ivpPlatoFondoNuevo = new ItemVentaPlatoFondo() { Producto = ppFromDB.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos };
                                var ivpPlatoFondoNuevo = new ItemVenta() { Producto = ppFromDB.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos };
                                ivpPlatoFondoNuevo.AlEliminar += (se3, a3) =>
                                {
                                    spVentaItems.Children.Remove(ivpPlatoFondoNuevo);
                                    CalcularTotal();
                                };
                                ivpPlatoFondoNuevo.AlModificarCantidad += (se4, a4) => CalcularTotal();
                                spVentaItems.Children.Add(ivpPlatoFondoNuevo);
                            }
                            else
                            {
                                try
                                {
                                    var ivpNuevo = new ItemVenta() { Producto = ppFromDB.producto, Cantidad = (int)ppFromDB.cantidad };
                                    ivpNuevo.AlEliminar += (se3, a3) =>
                                    {
                                        spVentaItems.Children.Remove(ivpNuevo);
                                        CalcularTotal();
                                    };
                                    ivpNuevo.AlModificarCantidad += (se4, a4) => CalcularTotal();
                                    spVentaItems.Children.Add(ivpNuevo);
                                }
                                catch (Exception ex)
                                {
                                    PoskException.Make(ex, "Error al convertir");
                                }
                            }
                            PedidosProductosBLL.Eliminar(ppFromDB.id);
                        }

                        if (PedidosProductosBLL.ObtenerTodos(x.id).Count == 0)
                        {
                            PedidoBLL.Eliminar(x.id);
                            spDerecha.Children.Clear();
                            CargarPendientesRestaurant();
                            CargarItemVerPendientes();
                            CalcularTotal();
                        }
                        CalcularTotal();
                        sipp.Close();
                    };
                };
                spDerecha.Children.Add(ipm);
            });
            */
        }

        private void CargarArriendos()
        {
            /*
            ArriendoBLL.ObtenerTodos().ForEach(arriendo => 
            {
                ItemArriendo ia = new ItemArriendo()
                {
                    Cliente = arriendo.cliente,
                    Producto = arriendo.producto,
                    Arriendo = arriendo
                };
                ia.btnPendiente.Click += (se, a) => 
                {
                    new Notification("test");
                };
                spDerecha.Children.Add(ia);
                expDerecha.IsExpanded ^= true;
            });
            */
        }

        private void CargarItemVerPendientes()
        {
            spDerecha.Children.Clear();

            ItemVerPendientes_venta itemVerPendientes = new ItemVerPendientes_venta();

            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
            {
                CargarPendientesRestaurant();
                //itemVerPendientes.lbVerEntregasNumero.Content = $"{ PedidoBLL.ObtenerTodos().Count }";
            }
            else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
            {
                CargarArriendos();
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
                //text = $"{item.Producto.nombre}\t\t${item.Producto.precio}";
                //e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new RectangleF(x, y, width, height), drawFormatLeft);
                //y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;


                e.Graphics.DrawString(item.Producto.nombre.ToString(), drawFontArial10Regular, System.Drawing.Brushes.Black, 20, 150 + count * 20);
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

                lbInfo.Content = "SECCIÓN VENTA";

                if (Settings.Usuario.tipo.ToLower().Equals("g"))
                    lbInfo.Content = "SECCIÓN PEDIDOS";

                // esconder seccion pendiete a garzón
                //if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                CargarItemVerPendientes();

                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
                    CargarDeliveryPndientesDeEntrega();

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
                    ItemUltimaVenta _iuv = new ItemUltimaVenta() { Mensaje = $"ÚLTIMA: ${_ts?.total}  EL {_ts?.fecha.ToShortDateString()}  A LAS  {_ts?.fecha.ToLongTimeString()} ", Boleta = _ts };
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
                    // cambiado a popup
                    //if (spMensaje.Children.OfType<ItemMensajeCocina>().ToList().Count == 0)
                    //    spMensaje.Children.Add(imc);

                    /*
                    if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    {
                        if (spGarzonMesa.Children.OfType<ItemEscogerGarzonSeccionVenta>().ToList().Count == 0)
                            spGarzonMesa.Children.Add(ieg);
                        if (spGarzonMesa.Children.OfType<ItemEscogerMesaSeccionVenta>().ToList().Count == 0)
                            spGarzonMesa.Children.Add(iem);
                    }
                    if (spMedioDePago.Children.OfType<ItemMedioPago>().ToList().Count == 0)
                    {
                        spMedioDePago.Children.Add(imp);
                    }
                    */
                    iegm.btnEscogerMesaGarzon.Click += (se2, a2) =>
                    {
                        var rpp = new RealizarPedidoPopup(
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == true).ToList(),
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == false).ToList(),
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList(), Settings.Usuario.tipo
                        );
                        rpp.AlEscogerMesa += (se3, mesa_usuario) =>
                        {
                            iem.cbMesas.Text = mesa_usuario.Mesa.codigo;
                            ieg.cbGarzones.Text = mesa_usuario.Usuario?.nombre;
                        };
                        rpp.Show();
                    };
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
                        dctoPopup.AlAplicarDcto += (se3, a3) =>
                        {
                            itemCalcularTotal.txtTotalVenta.Text = dctoPopup.TotalConDcto.ToString();
                            itemDcto.DctoPesos = dctoPopup.DctoPesos;
                            itemDcto.DctoPct = dctoPopup.DctoPct;
                            itemDcto.btnCerrar.Click += (se4, a4) =>
                            {
                                gridDcto.Children.Remove(itemDcto);
                                itemDcto.Reset();
                                new Notification("DCTO BORRADO", "$" + dctoPopup.DctoPesos);
                                ReCalcularTotal();
                            };
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
                                        producto prod = ProductoBLL.GetProduct(iv.Producto.id);
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
                                            puntos += (int)item.Producto.puntos_cantidad;
                                    }
                                    RealizarVentaSushi rvs = new RealizarVentaSushi(Convert.ToInt32(itemCalcularTotal.txtTotalVenta.Text), puntos);
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

                                        if (di.Efectivo != 0)
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, 1, di.Efectivo, Settings.Usuario.id);
                                        if (di.TransBank != 0)
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, 2, di.TransBank, Settings.Usuario.id);
                                        if (di.Junaeb != 0)
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, 3, di.Junaeb, Settings.Usuario.id);
                                        if (di.Otro != 0)
                                            BoletaMediopagoBLL.Crear(ultimaBoleta.id, 4, di.Otro, Settings.Usuario.id);

                                        foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                                        {
                                            if (item.Producto.precio == 0) continue;

                                            detalle_boleta dl = new detalle_boleta()
                                            {
                                                producto_id = item.Producto?.id,
                                                promocion_id = item.Promocion?.id,
                                                monto = item.ObtenerTotal(),
                                                cantidad = (int)item.Cantidad,
                                                descuento = 0,
                                                boleta_id = ultimaBoleta.id
                                            };
                                            DB.AddDetailLine(dl);

                                            // al almar una tabla creo productos cuya imagen es 'rollo tabla', los cuales no se deben descontar de stock
                                            if (item.Producto.imagen != "rollo_tabla")
                                                CompraBLL.ReduceStockByProduct(item.Producto?.id, item.Promocion?.id, (int)item.Cantidad);

                                            int? cobroExtra = 0;
                                            if (item.Producto.contiene_agregado == true)
                                            {
                                                if (item.AgregadoUno?.cobro_extra != null)
                                                    cobroExtra += item.AgregadoUno?.cobro_extra;
                                                if (item.AgregadoDos?.cobro_extra != null)
                                                    cobroExtra += item.AgregadoDos?.cobro_extra;
                                            }

                                            if (item.Producto.imagen != "rollo_tabla")
                                                VentasJornadaBLL.Agregar(JornadaBLL.UltimaJornada().id, item.Producto?.id, item.Promocion?.id, item.Cantidad, (int)cobroExtra);
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
                                        DeliveryItemBLL.Crear(ultimaBoleta.id, null, "", di.NombreCliente, null, "", di.Incluye, bServir);
                                        CargarDeliveryPndientesDeEntrega();
                                    };
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    PoskException.Make(ex, "ERROR AL MOSTRAR RV POPUP");
                                }
                            }

                            return;
                            int calcularTotal = 0;
                            if (itemCalcularTotal?.txtTotalVenta?.Text != "")
                                calcularTotal = Convert.ToInt32(itemCalcularTotal?.txtTotalVenta?.Text);

                            boleta ts = BoletaBLL.Set(0, Settings.Usuario.id, 0, calcularTotal, imp.MedioPagoSeleccionado.id);

                            foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                            {
                                if (item.Producto.precio == 0) continue;

                                detalle_boleta dl = new detalle_boleta()
                                {
                                    producto_id = item.Producto?.id,
                                    promocion_id = item.Promocion?.id,
                                    monto = item.ObtenerTotal(),
                                    cantidad = (int)item.Cantidad,
                                    descuento = 0,
                                    boleta = ts
                                };
                                DB.AddDetailLine(dl);

                                CompraBLL.ReduceStockByProduct(item.Producto?.id, item.Promocion?.id, (int)item.Cantidad);

                                int? cobroExtra = 0;
                                if (item.Producto.contiene_agregado == true)
                                {
                                    if (item.AgregadoUno?.cobro_extra != null)
                                        cobroExtra += item.AgregadoUno?.cobro_extra;
                                    if (item.AgregadoDos?.cobro_extra != null)
                                        cobroExtra += item.AgregadoDos?.cobro_extra;
                                }

                                VentasJornadaBLL.Agregar(JornadaBLL.UltimaJornada().id, item.Producto?.id, item.Promocion?.id, item.Cantidad, (int)cobroExtra);
                            }
                            int? subTotal = 0;
                            int propinaSugerida = 0;
                            int? total = 0;

                            spVentaItems.Children.OfType<ItemVenta>().ToList().ForEach(x => subTotal += x.ObtenerTotal());
                            expTecladoPagar.IsExpanded = false;
                            MostrarNotificacion("VENDIDO", "");
                            if (subTotal != null && subTotal != 0)
                            {
                                propinaSugerida = Convert.ToInt32(subTotal * 0.10);
                                total = propinaSugerida + subTotal;
                            }

                            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                                GenerarTicket(subTotal, ((usuario)ieg.cbGarzones.SelectedItem)?.nombre, ((mesa)iem.cbMesas.SelectedValue)?.codigo, "", "Ticket Caja");

                            LimpiarTodo();

                            try // mostrar última boleta
                            {
                                ItemUltimaVenta iuv = new ItemUltimaVenta() { Mensaje = $"ÚLTIMA: ${ts.total} A LAS {ts.fecha.ToLongTimeString()}", Boleta = ts };
                                borderUltimaVenta.Child = null;
                                borderUltimaVenta.Child = iuv;

                                iuv.btnBoleta.Click += (se3, a3) =>
                                {
                                    string detalleBoleta = $"Boleta ID: {iuv.Boleta.id}  GENERADA A LAS  {iuv.Boleta.fecha.ToLongTimeString()}\n\n";
                                    LineaDetalleBLL.Get(iuv.Boleta.id).ForEach(detalle =>
                                    {
                                        int detalleCantidad = 1;
                                        if (detalle.cantidad == 0) detalleCantidad = 1;
                                        if (detalle.promocione != null)
                                            detalleBoleta += $"${detalle.promocione?.precio * detalleCantidad}    x{detalleCantidad}    [{detalle.promocione?.nombre}]  (promoción)\n";
                                        else
                                            detalleBoleta += $"${detalle.producto?.precio * detalleCantidad}    x{detalleCantidad}    [{detalle.producto?.nombre}]\n";
                                    });
                                    detalleBoleta += $"\nTotal: ${iuv.Boleta.total}";
                                    MessageBox.Show(detalleBoleta);
                                };
                            }
                            catch (Exception ex)
                            {
                                PoskException.Make(ex, "ERROR AL MOSTRAR ÚLTIMA BOLETA");
                            }

                            // agregar iteracion para platos de fondo
                        }
                        catch (Exception ex)
                        {
                            PoskException.Make(ex, "ERROR AL VENDER");
                        }
                    }
                };


                ItemDejarPendiente_venta itemDejarPendiente = new ItemDejarPendiente_venta();
                itemDejarPendiente.btnPendiente.Click += (se2, a2) =>
                {
                    // deja items venta como pendiente asociado a una mesa
                    #region restaurant
                    try
                    {
                        //var rpp = new RealizarPedidoPopup(spVentaItems.Children.OfType<ItemVenta>().ToList(), spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList());
                        var rpp = new RealizarPedidoPopup(
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == true).ToList(),
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == false).ToList(),
                            spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList(), Settings.Usuario.tipo
                        );
                        rpp.Show();

                        rpp.ReciclarEvent += (se3, usuarioMesa) =>
                        {
                            spDerecha.Children.Clear();
                            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
                            {
                                CargarPendientesRestaurant();
                                CargarItemVerPendientes();

                                SyncBLL.AumentarSyncId("pedido");
                            }
                            else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
                            {
                                CargarArriendos();
                                CargarItemVerPendientes();
                            }
                            else //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
                            {
                                CargarPendientes();
                                CargarItemVerPendientes();
                            }
                            rpp.Close();
                            GenerarTicket(0, usuarioMesa[0], usuarioMesa[1], usuarioMesa[2]);
                            LimpiarTodo();
                        };
                    }
                    catch
                    {
                        MessageBox.Show("Test message");
                    }

                    if (!Settings.Usuario.tipo.ToLower().Equals("g"))
                    {
                        borderAccionIzquierda.Child = itemDejarPendiente;
                    }
                    #endregion restaurant
                };
                if (!GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
                    borderAccionIzquierda.Child = itemDejarPendiente;
            };
        }

        private void CargarDeliveryPndientesDeEntrega()
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
                    Boleta = d.boleta,
                    DeliveryItem = d
                };

                id.btnItem.Click += (se, a) =>
                {
                    DeliveryPopup dp = new DeliveryPopup(d.id, d.nombre_cliente, d.boleta.fecha, d.incluye, id.Boleta);
                    dp.Deactivated += (se2, a2) => MostrarOverlay(false);
                    dp.AlEntregar += (se2, a2) =>
                    {
                        MostrarOverlay(false);
                        dp.Cerrar();
                        CargarDeliveryPndientesDeEntrega();
                    };
                    dp.Show();
                    MostrarOverlay(true);
                };
                spDerecha.Children.Add(id);
            });
        }

        private List<ItemVenta> ConvertirListaPPitemVenta(List<pedidos_productos> listaPP)
        {
            List<ItemVenta> listaItemsVenta = new List<ItemVenta>();
            try
            {
                listaPP.ForEach(x =>
                {
                    var ppFromDB = PedidosProductosBLL.Obtener(x.id);
                    if (ppFromDB.producto?.contiene_agregado == true || ppFromDB.producto?.preparado_especial == true)
                    {
                        pedidos_agregados pa = PedidosAgregadosBLL.Obtener(ppFromDB.id);
                        pedidos_preparaciones pedPrep = PedidosPreparacionesBLL.Obtener(ppFromDB.id);

                        agregado agregadoUno = AgregadoBLL.Obtener(pa?.agregado_uno_id);
                        agregado agregadoDos = AgregadoBLL.Obtener(pa?.agregado_dos_id);
                        preparacione preparacion = PreparacionesBLL.Obtener(pedPrep?.preparacion_id);
                        listaItemsVenta.Add(new ItemVenta() { Producto = ppFromDB.producto, AgregadoUno = agregadoUno, AgregadoDos = agregadoDos, Preparacion = preparacion, Cantidad = (int)x.cantidad });
                    }
                    else
                    {
                        listaItemsVenta.Add(new ItemVenta() { Producto = ppFromDB.producto, Cantidad = (int)x.cantidad, Nota = x.nota });
                    }
                });
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL CONVERTIR LISTA");
            }
            return listaItemsVenta;
        }

        private void GenerarTicketDesdePP(pedido p, List<ItemVenta> listaIV, string docName = "Cuenta", string header = "", string pedidoId = "", string nuevoGarzonNombre = "", string nuevaMesaCodigo = "")
        {
            try
            {
                int? subTotal = 0;
                listaIV.ForEach(x =>
                {
                    subTotal += x.ObtenerTotal();
                });

                CrearTicket ticket = new CrearTicket();
                ticket.AbreCajon();

                ticket.TextoCentro($"{header}");
                ticket.TextoIzquierda(" ");
                ticket.TextoIzquierda(" ");

                bool bEsPedido = false;
                if (!string.IsNullOrEmpty(pedidoId))
                    bEsPedido = true;

                if (!bEsPedido)
                    ticket.TextoCentro($"{Settings.NombreDelNegocio}");
                else
                    ticket.TextoCentro($"TICKET COCINA");

                ticket.TextoIzquierda("");

                if (!bEsPedido)
                {
                    if (nuevaMesaCodigo != "")
                    {
                        ticket.TextoIzquierda($"MESA: {nuevaMesaCodigo}".ToUpper());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(p.mesa.codigo))
                            ticket.TextoIzquierda($"MESA: {p.mesa.codigo}".ToUpper());
                    }
                }
                else
                {
                    if (nuevaMesaCodigo != "")
                    {
                        ticket.TextoExtremos($"MESA: {nuevaMesaCodigo}".ToUpper(), $"PEDIDO ID: {p.id}".ToUpper());
                    }
                    else
                    {
                        ticket.TextoExtremos($"MESA: {p.mesa.codigo}".ToUpper(), $"PEDIDO ID: {p.id}".ToUpper());
                    }
                }

                if (nuevoGarzonNombre != "")
                {
                    ticket.TextoIzquierda($"GARZON: {nuevoGarzonNombre}".ToUpper());
                }
                else
                {
                    if (!string.IsNullOrEmpty(p.usuario.nombre))
                        ticket.TextoIzquierda($"GARZON: {p.usuario.nombre}".ToUpper());
                }

                ticket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                ticket.TextoIzquierda("");

                //if (spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList().Count != 0)
                if (listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "entrada").ToList().Count != 0)
                {
                    ticket.lineasGuion();
                    ticket.TextoCentro("ENTRADAS");
                }

                foreach (ItemVenta item in listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "entrada").ToList())
                {
                    string espaciosStr = "";
                    string espaciosValor = "";
                    switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                    string cortesiaStr = "";
                    if (item.Producto.precio == 0) cortesiaStr = " (CORTESIA)";

                    for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                        espaciosStr += ".";

                    if (item.Cantidad < 10)
                    {
                        if (bEsPedido || item.Producto.precio == 0)
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }
                    else
                    {
                        if (bEsPedido || item.Producto.precio == 0)
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }
                }

                if (listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo").ToList().Count != 0)
                {
                    ticket.TextoIzquierda("");
                    //ticket.lineasGuion();
                    ticket.TextoCentro("DETALLE");
                    //foreach (ItemVentaPlatoFondo item in spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList())
                    foreach (ItemVenta item in listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == false).ToList())
                    {
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


                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre} {item.Preparacion?.nombre}".Length; i++)
                            espaciosStr += ".";

                        int? cobroExtra = 0;
                        cobroExtra += item.AgregadoUno?.cobro_extra;
                        cobroExtra += item.AgregadoDos?.cobro_extra;
                        //if (cobroExtra == null) cobroExtra = 0;

                        if (cobroExtra != 0 && cobroExtra != null)
                        {
                            if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                            {
                                if (bEsPedido)
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                                else
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                            }
                            else
                            {
                                if (item.AgregadoUno != null && item.AgregadoDos != null)
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                    else
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                            {
                                if (bEsPedido)
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                                else
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                            }
                            else
                            {
                                if (item.AgregadoUno?.nombre != null && item.AgregadoDos?.nombre != null)
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                    else
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }

                                    }
                                }
                                else
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
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
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        }

                                    }
                                }
                            }
                        }
                    }
                    ticket.TextoIzquierda("");
                    //ticket.lineasGuion();
                    if (bEsPedido)
                    {
                        if (!string.IsNullOrEmpty(imc.txtMensaje.Text) && ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "COCINA" || ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "TODOS")
                        {
                            ticket.TextoIzquierda("");
                            ticket.TextoIzquierda($"Mensaje: {imc.txtMensaje.Text}".ToUpper());
                        }
                    }
                }


                if (listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList().Count != 0)
                {
                    bool bParaBar = false;
                    if (!string.IsNullOrEmpty(pedidoId))
                        bParaBar = true;

                    ticket.lineasGuion();
                    ticket.TextoCentro($"OTROS");


                    foreach (ItemVenta item in listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "otro" && x.Producto.precio != 0).ToList())
                    {
                        string espaciosStr = "";
                        string espaciosValor = "";
                        switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                            espaciosStr += ".";

                        if (item.Cantidad < 10)
                        {
                            if (bParaBar || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                        else
                        {
                            if (bParaBar || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                    }

                    if (listaIV.Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList().Count != 0)
                    {
                        ticket.TextoIzquierda("");
                        ticket.lineasGuion();
                    }

                    if (bParaBar)
                    {
                        if (!string.IsNullOrEmpty(imc.txtMensaje.Text) && ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "BAR" || ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "TODOS")
                        {
                            ticket.TextoIzquierda("");
                            ticket.TextoIzquierda($"Mensaje: {imc.txtMensaje.Text}".ToUpper());
                        }
                    }
                }

                if (!bEsPedido)
                {
                    ticket.TextoIzquierda("");
                    int propinaSugerida = Convert.ToInt32(subTotal * 0.10);
                    int? total = propinaSugerida + subTotal;
                    ticket.TextoIzquierda($"SubTotal:                     ${subTotal}".ToUpper());
                    ticket.TextoIzquierda($"Propina sugerida (10%):       $ {propinaSugerida}".ToUpper());
                    ticket.TextoIzquierda($"Total:                        ${total}".ToUpper());
                }
                else
                {
                    //if (!string.IsNullOrEmpty(mensajeCocina))
                    //    ticket.TextoIzquierda($"Mensaje cocina: {mensajeCocina}".ToUpper());
                }

                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings.ImpresoraBar, docName + " - BAR");

                if (bEsPedido)
                    ticket.ImprimirTicket(Settings.ImpresoraCocina, docName + " - COCINA");
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al generar el ticket");
            }

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
                    ticket.TextoExtremos($"MESA: {mesa}".ToUpper(), $"ID: {BoletaBLL.ObtenerUltimoNumeroBleta()}");
                }

                if (!string.IsNullOrEmpty(garzon))
                    ticket.TextoIzquierda($"GARZON: {garzon}".ToUpper());

                ticket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                ticket.TextoIzquierda("");

                //if (spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList().Count != 0)
                if (spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "entrada").ToList().Count != 0)
                {
                    ticket.lineasGuion();
                    ticket.TextoCentro("ENTRADAS");
                }

                foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "entrada").ToList())
                {
                    bContieneParaCocina = true;
                    if (item.Producto.sector_impresion.nombre == "COCINA")
                        bImprimirEnCocina = true;

                    string espaciosStr = "";
                    string espaciosValor = "";
                    switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                    string cortesiaStr = "";
                    if (item.Producto.precio == 0) cortesiaStr = " (CORTESIA)";

                    for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                        espaciosStr += ".";

                    if (item.Cantidad < 10)
                    {
                        if (bEsPedido || item.Producto.precio == 0)
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }
                    else
                    {
                        if (bEsPedido || item.Producto.precio == 0)
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }

                    if (!string.IsNullOrEmpty(item.txtNota.Text)) ticket.TextoIzquierda("   " + item.txtNota.Text.ToUpper());

                }

                if (spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo").ToList().Count != 0)
                {
                    //ticket.lineasGuion();
                    //ticket.TextoCentro("ENTRADAS");
                    foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == true).ToList())
                    {
                        bContieneParaCocina = true;
                        if (item.Producto.sector_impresion.nombre == "COCINA")
                            bImprimirEnCocina = true;

                        string espaciosStr = "";
                        string espaciosValor = "";
                        switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                        string cortesiaStr = "";
                        if (item.Producto.precio == 0) cortesiaStr = " (CORTESIA)";

                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                            espaciosStr += ".";

                        if (item.Cantidad < 10)
                        {
                            if (bEsPedido || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                        else
                        {
                            if (bEsPedido || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{cortesiaStr}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                        if (!string.IsNullOrEmpty(item.txtNota.Text)) ticket.TextoIzquierda("   " + item.txtNota.Text.ToUpper());
                    }
                    ticket.TextoIzquierda("");
                    //ticket.lineasGuion();
                    //ticket.TextoCentro("DETALLE");
                    //foreach (ItemVentaPlatoFondo item in spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList())
                    foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == false).ToList())
                    {
                        if (item.Producto.sector_impresion?.nombre != "NINGUNO")
                            bImprimir = true;

                        bContieneParaCocina = true;
                        if (item.Producto.sector_impresion?.nombre == "COCINA")
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


                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre} {item.Preparacion?.nombre}".Length; i++)
                            espaciosStr += ".";

                        int? cobroExtra = 0;
                        cobroExtra += item.AgregadoUno?.cobro_extra;
                        cobroExtra += item.AgregadoDos?.cobro_extra;
                        //if (cobroExtra == null) cobroExtra = 0;

                        if (cobroExtra != 0 && cobroExtra != null)
                        {
                            if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                            {
                                if (bEsPedido)
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                                else
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                            }
                            else
                            {
                                if (item.AgregadoUno != null && item.AgregadoDos != null)
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                    else
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                            {
                                if (bEsPedido)
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                                else
                                {
                                    if (item.Cantidad < 10)
                                    {
                                        ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                    else
                                    {
                                        ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre}".ToUpper());
                                    }
                                }
                            }
                            else
                            {
                                if (item.AgregadoUno?.nombre != null && item.AgregadoDos?.nombre != null)
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                    }
                                    else
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                            ticket.TextoIzquierda($"   Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                                        }

                                    }
                                }
                                else
                                {
                                    if (bEsPedido)
                                    {
                                        if (item.Cantidad < 10)
                                        {
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
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
                                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        }
                                        else
                                        {
                                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.ObtenerTotal()}".ToUpper());
                                        }

                                    }
                                }
                            }
                        }
                        if (item.ListaRollosTabla != null)
                        {
                            int count = 1;
                            item.ListaRollosTabla.ForEach(rolloTablaIV =>
                            {
                                ticket.TextoIzquierda($"");
                                ticket.TextoIzquierda($"*** ROLLO #{count}:");
                                ticket.TextoIzquierda($"ENVOLTURA: {rolloTablaIV.Envoltura.nombre}");
                                ticket.TextoIzquierda($"{rolloTablaIV.ObtenerAgregadosStr()}");

                                if (item.Envoltura != null)
                                    ticket.TextoIzquierda($"ENVOLTURA: {item.Envoltura.nombre}");
                                if (item.listaAgregadosSushi != null)
                                    ticket.TextoIzquierda($"{item?.ObtenerAgregadosStr()}");
                                if (!string.IsNullOrEmpty(item.txtNota?.Text))
                                    ticket.TextoIzquierda("   " + item.txtNota?.Text.ToUpper());
                                //ticket.TextoIzquierda("");

                                ticket.TextoIzquierda($"***");
                                count++;
                            });
                        }
                        else
                        {
                            if (item.Envoltura != null)
                                ticket.TextoIzquierda($"ENVOLTURA: {item.Envoltura.nombre}");
                            if (item.listaAgregadosSushi != null)
                                ticket.TextoIzquierda($"{item?.ObtenerAgregadosStr()}");
                            if (!string.IsNullOrEmpty(item.txtNota?.Text))
                                ticket.TextoIzquierda("   " + item.txtNota?.Text.ToUpper());
                            ticket.TextoIzquierda("");

                            if (bEsPedido == true)
                                ticket.lineasGuion();
                        }
                    }
                    ticket.TextoIzquierda("");
                    //ticket.lineasGuion();
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
                    if (di.Incluye != "")
                        ticket.TextoCentro($"{di.IncluyeStrUnaLinea}");
                    if (di.MensajeTicket != "")
                        ticket.TextoCentro($"Mensaje cocina: {di.MensajeTicket}");
                    ticket.TextoCentro("");
                }
                if (di != null && !bEsPedido)
                {
                    if (di.NombreCliente != "")
                        ticket.TextoCentro($"Cliente: {di.NombreCliente}");
                    if (di.Telefono != "")
                        ticket.TextoCentro($"Teléfono: {di.Telefono}");
                    if (di.Direccion != "")
                        ticket.TextoCentro($"Direccion: {di.Direccion}");
                    if (di.MensajeDeliveryUno != "")
                        ticket.TextoCentro($"{di.MensajeDeliveryUno}");
                    if (di.MensajeDeliveryDos != "")
                        ticket.TextoCentro($"{di.MensajeDeliveryDos}");
                }

                if (spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList().Count != 0)
                {
                    if (!string.IsNullOrEmpty(pedidoId))
                        bParaBar = true;

                    if (bParaBar)
                    {
                        if (bContieneParaCocina)
                        {
                            ticket.CortaTicket();
                            ticket.ImprimirTicket(Settings.ImpresoraCocina, "Ticket Cocina");
                            // ticket.ImprimirTicket(Settings.ImpresoraBar, "7466456");
                        }
                        ticket.linea.Clear();
                        ticket.TextoCentro($"TICKET CAJA");
                        ticket.TextoIzquierda("");
                        // ticket.TextoExtremos($"MESA: {mesa}".ToUpper(), $"PEDIDO ID: {pedidoId}".ToUpper());
                        ticket.TextoIzquierda($"MESA: {mesa}".ToUpper());
                        if (!string.IsNullOrEmpty(garzon))
                            ticket.TextoIzquierda($"GARZON: {garzon}".ToUpper());
                        ticket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                        ticket.TextoIzquierda("");
                    }
                    else
                    {
                        ticket.lineasGuion();
                        ticket.TextoCentro($"OTROS");
                    }


                    foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro" && x.Producto.precio != 0).ToList())
                    {
                        if (item.Producto.sector_impresion.nombre == "COCINA")
                            bImprimirEnCocina = true;

                        string espaciosStr = "";
                        string espaciosValor = "";
                        switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                        for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                            espaciosStr += ".";

                        if (item.Cantidad < 10)
                        {
                            if (bParaBar || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                        else
                        {
                            if (bParaBar || item.Producto.precio == 0)
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                            else
                                ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                        }
                        if (!string.IsNullOrEmpty(item.txtNota.Text)) ticket.TextoIzquierda("   " + item.txtNota.Text.ToUpper());
                    }

                    if (spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList().Count != 0)
                    {
                        ticket.TextoIzquierda("");
                        ticket.lineasGuion();
                    }

                    if (bParaBar)
                    {
                        if (!string.IsNullOrEmpty(imc.txtMensaje.Text) && (((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "BAR" || ((sector_impresion)imc.cbSectoresImpresion.SelectedItem).nombre.ToUpper() == "TODOS"))
                        {
                            ticket.TextoIzquierda("");
                            ticket.TextoIzquierda($"Mensaje: {imc.txtMensaje.Text}".ToUpper());
                        }
                    }

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

                int contadorProductosCocina = 0;
                foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == true).ToList())
                {
                    if (item.Producto.sector_impresion.nombre == "COCINA")
                        contadorProductosCocina++;
                }
                foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo" && x.Entrada == false).ToList())
                {
                    if (item.Producto.sector_impresion.nombre == "COCINA")
                        contadorProductosCocina++;
                }
                foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "otro").ToList())
                {
                    if (item.Producto.sector_impresion.nombre == "COCINA")
                        contadorProductosCocina++;
                }

                //ticket.ImprimirTicket(Settings.ImpresoraBar);

                //Imprimir.PrintText("Texto de prueba a bar", Settings.ImpresoraBar);
                //Imprimir.PrintText("Texto de prueba a cocina", Settings.ImpresoraCocina);


                if (contadorProductosCocina != 0 && bEsPedido && !bParaBar && bImprimir == true)
                    ticket.ImprimirTicket(Settings.ImpresoraCocina, "Ticket Cocina");

                //Imprimir.PrintText("Texto de prueba a cocina", Settings.ImpresoraCocina);

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

        /*
        private void GenerarTicketDesdePP(int? subTotal, string mensajeCocina, string garzon, string mesa, string pedidoId, List<pedidos_productos> listaPP)
        {
            try
            {
                CrearTicket ticket = new CrearTicket();
                ticket.AbreCajon();

                bool bParaCocina = false;
                if (!string.IsNullOrEmpty(pedidoId))
                    bParaCocina = true;

                if (!bParaCocina)
                    ticket.TextoCentro($"{Settings.NombreDelNegocio}");
                else
                    ticket.TextoCentro($"TICKET COCINA");

                ticket.TextoIzquierda("");

                if (!bParaCocina)
                {
                    if (!string.IsNullOrEmpty(mesa))
                        ticket.TextoIzquierda($"MESA: {mesa}".ToUpper());
                }
                else
                    ticket.TextoExtremos($"PEDIDO ID: {pedidoId}".ToUpper(), $"MESA: {mesa}".ToUpper());

                if (!string.IsNullOrEmpty(garzon))
                    ticket.TextoIzquierda($"GARZÓN: {garzon}".ToUpper());

                ticket.TextoExtremos("FECHA: " + DateTime.Now.ToShortDateString(), "HORA: " + DateTime.Now.ToShortTimeString());
                ticket.TextoIzquierda("");

                if (listaPP.Count != 0)
                    ticket.lineasGuion();

                foreach (pedidos_productos item in listaPP.Where(x => x.producto.contiene_agregado == true).ToList())
                {
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

                    for (int i = 0; i < 33 - $"{item.Producto?.nombre} {item.Preparacion?.nombre}".Length; i++)
                        espaciosStr += ".";

                    int? cobroExtra = 0;
                    cobroExtra += item.AgregadoUno?.cobro_extra;
                    cobroExtra += item.AgregadoDos?.cobro_extra;
                    if (cobroExtra != 0)
                    {
                        if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                        {
                            if (bParaCocina)
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} X2".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio + cobroExtra}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} X2".ToUpper());
                            }
                        }
                        else
                        {
                            if (bParaCocina)
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio + cobroExtra}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                            }
                        }
                    }
                    else
                    {
                        if (item.AgregadoUno == item.AgregadoDos && item.AgregadoUno != null)
                        {
                            if (bParaCocina)
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} X2".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} X2".ToUpper());
                            }
                        }
                        else
                        {
                            if (bParaCocina)
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                            }
                            else
                            {
                                ticket.TextoIzquierda($"\n{item.Producto?.nombre} {item.Preparacion?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio}".ToUpper());
                                ticket.TextoIzquierda($"Con {item.AgregadoUno?.nombre} y {item.AgregadoDos?.nombre}".ToUpper());
                            }
                        }

                    }
                }
                ticket.TextoIzquierda("");
                ticket.lineasGuion();
                foreach (ItemVenta item in spVentaItems.Children.OfType<ItemVenta>().ToList())
                {
                    string espaciosStr = "";
                    string espaciosValor = "";
                    switch ($"{item.Producto?.precio * item.Cantidad}".Length)
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

                    for (int i = 0; i < 33 - $"00 {item.Producto?.nombre}".Length; i++)
                        espaciosStr += ".";

                    if (item.Cantidad < 10)
                    {
                        if (bParaCocina)
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n0{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }
                    else
                    {
                        if (bParaCocina)
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}".ToUpper());
                        else
                            ticket.TextoIzquierda($"\n{item.Cantidad} {item.Producto?.nombre}{espaciosStr}${espaciosValor}{item.Producto?.precio * item.Cantidad}".ToUpper());
                    }
                }
                ticket.TextoIzquierda("");
                ticket.lineasGuion();

                if (!bParaCocina)
                {
                    ticket.TextoIzquierda("");
                    int propinaSugerida = Convert.ToInt32(subTotal * 0.10);
                    int? total = propinaSugerida + subTotal;
                    ticket.TextoIzquierda($"SubTotal:                     ${subTotal}".ToUpper());
                    ticket.TextoIzquierda($"Propina sugerida (10%):       $ {propinaSugerida}".ToUpper());
                    ticket.TextoIzquierda($"Total:                        ${total}".ToUpper());
                }
                else
                {
                    if (!string.IsNullOrEmpty(mensajeCocina))
                        ticket.TextoIzquierda($"Mensaje cocina: {mensajeCocina}".ToUpper());
                }

                ticket.CortaTicket();
                ticket.ImprimirTicket("THERMAL Receipt Printer");
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al generar el ticket");
            }
        }
        */

        private void CargarSeccionPedido()
        {
            btnVolver.Click += (se, a) => MainWindow.MainFrame.Content = new PageMesas();

            Loaded += (se, a) =>
            {
                lbInfo.Content = "SECCIÓN PEDIDO";
                //lbNombreLista.Content = "PEDIDO";

                ItemDejarMensaje_pedido itemDejarMensaje = new ItemDejarMensaje_pedido();
                borderAccionIzquierda.Child = itemDejarMensaje;

                ItemMesa_pedido itemMesa = new ItemMesa_pedido() { Mesa = mesa };
                borderAccionCentro.Child = itemMesa;

                ItemEnviar_pedido itemEnviar = new ItemEnviar_pedido();
                borderAccionDerecha.Child = itemEnviar;

                ItemMensajeCocina itemMensajeCocina = new ItemMensajeCocina();
                spContenidoSeccionLista.Children.Add(itemMensajeCocina);

                itemEnviar.btnEnviar.Click += (se2, a2) =>
                {
                    pedido ped = new pedido();
                    mesa.usuario = Settings.Usuario;
                    ped = PedidoBLL.Obtener(mesa.id);
                    if (ped == null)
                        ped = PedidoBLL.Crear(Settings.Usuario, DateTime.UtcNow, itemMensajeCocina.Mensaje, mesa.id);

                    // platos de fondo con sus agregados
                    //List<ItemVentaPlatoFondo> listaPlatosFondo = spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList();
                    List<ItemVenta> listaPlatosFondo = spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo").ToList();
                    if (listaPlatosFondo.Count != 0)
                    {
                        listaPlatosFondo.ForEach(ive =>
                        {
                            //pedidos_productos pedido_producto = PedidosProductosBLL.Crear(ped.id, ive.Producto?.id, ive.Promocion?.id, ive.Cantidad, Convert.ToInt32(ive.txtTotal.Text), );
                            //PedidosAgregadosBLL.Crear(pedido_producto.id, ive.AgregadoUno.id, ive.AgregadoDos.id);
                            //PedidosAgregadosBLL.Crear(pedido_producto, ive.AgregadoDos);
                        });
                    }


                    string listaProductosImprimir_Nuevo = "\nNUEVO:\n";
                    string listaProductosImprimir_Agregar = "\nAGREGAR:\n";
                    string listaProductosImprimir_Quitar = "\nQUITAR UNIDAD(ES):\n";
                    string listaProductosImprimir_QuitarTodasLasUnidades = "\nQUITAR TODAS LAS UNIDADES:\n";

                    List<ItemVentaPedido> listaItemsPedido = spVentaItems.Children.OfType<ItemVentaPedido>().ToList();
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
                                //PedidosProductosBLL.Crear(ped.id, ivp.Producto.id, ivp.Promo.id, ivp.Cantidad);
                                listaProductosImprimir_Nuevo += $"{ivp.Producto.nombre} x{ivp.Cantidad}\n";
                            }
                        });
                    }
                    if (spVentaItems.Children.Count == 0)
                    {
                        MesaBLL.Actualizar(mesa.id, 0, null, true);
                        // Mensaje
                        List<ItemAccion> _listaItemsAccion = new List<ItemAccion>();
                        ItemAccion _ia1 = new ItemAccion();
                        _ia1.Accion = "SALIR";
                        _ia1.btnAccion.Click += (se3, a3) => { MainWindow.MainFrame.Content = new PageInicio(); };

                        ItemAccion _ia2 = new ItemAccion();
                        _ia2.Accion = "OTRO PEDIDO";
                        _ia2.btnAccion.Click += (se3, a3) => { MainWindow.MainFrame.Content = new PageMesas(); };

                        _listaItemsAccion.Add(_ia1);
                        _listaItemsAccion.Add(_ia2);

                        MostrarAccion(_listaItemsAccion, "¡LISTO!");
                        return;
                    }

                    List<pedidos_productos> listaPedidosProductosActualizar = new List<pedidos_productos>();
                    spVentaItems.Children.OfType<ItemVentaPedido>().ToList().ForEach(x =>
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
                    //spVentaItems.Children.OfType<ItemVentaPlatoFondo>().ToList().ForEach(x =>
                    spVentaItems.Children.OfType<ItemVenta>().Where(x => x.Producto.tipo_itemventa?.nombre == "plato fondo").ToList().ForEach(x =>
                    {
                        if (x.AgregadoUno == x.AgregadoDos)
                            imprimirActualización += $"{x.Producto.nombre} con {x.AgregadoUno.nombre}\n";
                        else
                            imprimirActualización += $"{x.Producto.nombre} con {x.AgregadoUno.nombre} y {x.AgregadoDos.nombre}\n";
                        contadorItems++;
                    });
                    spVentaItems.Children.OfType<ItemVentaPedido>().ToList().ForEach(x =>
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
                    ia1.btnAccion.Click += (se3, a3) => { MainWindow.MainFrame.Content = new PageInicio(); };

                    ItemAccion ia2 = new ItemAccion();
                    ia2.Accion = "OTRO PEDIDO";
                    ia2.btnAccion.Click += (se3, a3) => { MainWindow.MainFrame.Content = new PageMesas(); };

                    listaItemsAccion.Add(ia1);
                    listaItemsAccion.Add(ia2);

                    MostrarAccion(listaItemsAccion, "¡LISTO!");
                };

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
                                CalcularTotal();

                            };
                            spVentaItems.Children.Add(ivp);
                            CalcularTotal();

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

            };
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
                            contenidoCorreo += $"{item.lbProducto.Content}\nUnitario bruto: ${item.txtCostoUnitarioBruto.Text}\nTotal bruto: ${item.txtTotalBruto.Text}\n\n";
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
            expBottomAgregadosHandroll.Expanded += (se, a) =>
            {
                CrearAgregadosHandroll();
                expAgregadoSushiSuperior.IsExpanded = true;
            };
            expBottomAgregadosHandroll.Collapsed += (se, a) => expAgregadoSushiSuperior.IsExpanded = false;

            this.KeyDown += (se, a) =>
            {
                if (a.Key == Key.Left)
                    txtCantidad.Focus();
                if (a.Key == Key.Right)
                    txtBarCode.Focus();
            };
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
                case "PEDIDO":
                    CargarSeccionPedido();
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
                    //if (bListaRescatadaDeBD == false)
                    //{
                    //    listas = ProductoBLL.ObtenerCoincidencias("*");
                    //    bListaRescatadaDeBD = true;
                    //    MostrarPedidos(listas);
                    //}
                    //else
                    //{
                    //    MostrarPedidos(listas);
                    //}
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

            /*
            btnExpanderBottom.Click += (se, a) =>
                expBottom.IsExpanded ^= true;
            expBottom.Expanded += (se, a) =>
                btnExpanderBottom.Foreground = colorDorado;
            expBottom.Collapsed += (se, a) =>
                btnExpanderBottom.Foreground = colorNeutro;
            */

            expBottomEnvolturasHandroll.Expanded += (se, a) =>
            {
                expBottomAgregadosHandroll.IsExpanded = false;
                expBottomPreparadoEspecial.IsExpanded = false;
            };
            expBottomAgregadosHandroll.Expanded += (se, a) =>
            {
                expBottomEnvolturasHandroll.IsExpanded = false;
                expBottomPreparadoEspecial.IsExpanded = false;
            };

            btnCrearItemVentaEspecialHandroll.Click += (se, a) =>
            {
                expBottomEnvolturasHandroll.IsExpanded = false;
                expBottomPreparadoEspecial.IsExpanded = false;
                expBottomAgregadosHandroll.IsExpanded = false;
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
