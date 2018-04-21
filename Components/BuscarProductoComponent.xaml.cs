using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using posk.Globals;
using System.Configuration;
using System.Data;
using posk.BLL;
using posk.Models;
using posk.Pages.PopUp;
using System.Windows.Threading;
using System.ComponentModel;
using posk.Components;
using posk.Popup;
using System.Windows.Media.Animation;

namespace posk.Components
{
    public partial class BuscarProductoComponent : UserControl, INotifyPropertyChanged
    {


        #region Variables Globales

        public int VentaActual_descuentoGlobal { get; set; }

        public bool bDescuentoGlobalActivo { get; set; }

        public bool bProductosFavToggle { get; set; }

        public struct ProductInfoCB
        {
            public int _id { get; set; }
            public string _name { get; set; }
            public int _price { get; set; }
            public int _descuento { get; set; }
            public int _descuentoPct { get; set; }
            public int _totalPorElCualSeAplicaDescuentoGlobal { get; set; }
        }
        List<ProductInfoCB> listaDescuentos = new List<ProductInfoCB>();

        private int ventaActual_descuento;
        public int VentaActual_descuento
        {
            get { return ventaActual_descuento; }
            set { ventaActual_descuento = value; OnPropertyChanged("VentaActual_descuento"); }
        }


        private int ventaActual_envasesPrestados;
        public int VentaActual_envasesPrestados
        {
            get { return ventaActual_envasesPrestados; }
            set { ventaActual_envasesPrestados = value; OnPropertyChanged("VentaActual_envasesPrestados"); }
        }

        private int ventaActual_totalSinDcto = 0;

        public int VentaActual_totalSinDcto
        {
            get { return ventaActual_totalSinDcto; }
            set { ventaActual_totalSinDcto = value; OnPropertyChanged("VentaActual_totalSinDcto"); }
        }

        private int ventaActual_total = 0;
        public int VentaActual_total
        {
            get { return ventaActual_total; }
            set { ventaActual_total = value; OnPropertyChanged("VentaActual_total"); }
        }

        private int ventaActual_puntos = 0;
        public int VentaActual_puntos
        {
            get { return ventaActual_puntos; }
            set { ventaActual_puntos = value; OnPropertyChanged("VentaActual_puntos"); }
        }

        Label lbUser = new Label();

        // bools mayor que
        //bool bFiltroMayorQue;

        // bools categoria
        bool bFiltroComestible;
        //bool bFiltroBebestible;
        bool bFiltroHabilitar;
        bool bModulos;
        //List<ItemProduct> listaProductosBoleta = new List<ItemProduct>();

        List<ItemVenta> listaItemsVenta = new List<ItemVenta>();

        bool bClient_isEditEnabled = false;
        bool bClient_founded = false;
        bool bClient_isFavPressed = false;

        int envasesQueTrajo = 0, envasesQueLleva = 0, envasesQueDebe = 0, envasesQueDebera = 0;
        //bool bIsCreated_lbInfo = false;

        string ventaActual_mesa = "";
        cliente ventaActual_cliente = new cliente();

        //int ventaActual_descuentoGlobalEnPesos = 0
        int ventaActual_descuentoGlobalEnPct = 0;

        #endregion Variables Globales

        #region Constructor
        List<TextBox> ListaItemsTeclado;
        List<TextBox> ListaItemsTecladoNumerico;
        ItemTeclado teclado;
        ItemTecladoNumerico ItemTecladoNumerico;
        public static MessageBox mb { get; set; }
        public string Modo { get; set; }

        public BuscarProductoComponent(string modo) // VENDER, COMPRAR, DEVOLVER, PEDIDO
        {
            InitializeComponent();
            Modo = modo;

            switch (modo)
            {
                case "VENDER":
                    lbInfo.Content = "VENTA";
                    borderPendiente.Child = spPendiente;
                    borderPagaVueltoTotal.Child = spPagaVueltoTotal;
                    break;
                case "COMPRAR":
                    lbInfo.Content = $"COMPRA";
                    borderPendiente.Child = null;
                    borderPagaVueltoTotal.Child = null;
                    break;
                case "DEVOLVER":
                    lbInfo.Content = $"DEVOLUCIÓN";
                    borderPendiente.Child = null;
                    borderPagaVueltoTotal.Child = null;
                    break;
                case "ENVIAR PEDIDO":
                    lbInfo.Content = $"PEDIDO";
                    borderPendiente.Child = null;
                    borderPagaVueltoTotal.Child = null;
                    break;
                default:
                    lbInfo.Content = "";
                    break;
            }

            btnAccionPrincipal.Content = modo;

            ListaItemsTeclado = new List<TextBox>() { txtBuscar, txtClient_rut, txtClient_name, txtClient_contact, txtClient_comment };
            teclado = new ItemTeclado(ListaItemsTeclado);
            borderTeclado.Child = teclado;

            ListaItemsTecladoNumerico = new List<TextBox>() { txtPagaCon, txtDescuentoGlobalPct, txtDescuentoGlobalPesos };
            ItemTecladoNumerico = new ItemTecladoNumerico(ListaItemsTecladoNumerico);
            borderTecladoNumerico.Child = ItemTecladoNumerico;

            HabilitarDeshabilitarBotonesVenta();

            lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Where(x => x.archivado == false).ToList().Count}";

            bProductosFavToggle = false;
            MostrarProductosFav(false);

            Events();
            spAsociacionDescuento.Children.Clear();
            lbUser = new Label()
            {
                Content = $"Vendedor: {Settings.NombreUsuario}",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(8, 0, 8, 0)
            };
            txtTotalVenta.IsReadOnly = true;
            txtVuelto.IsReadOnly = true;
            lbVIP.Content = "";
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); //in Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
            spAsociacionCliente.Children.Clear();
            spAsociacionDescuento.Children.Clear();
            spAsociacionMesa.Children.Clear();


            //lbAsociacionDescuento.Content = "";
            lbAsociacionMesa.Content = "";
            lbAsociacionCliente1.Content = "";
            lbAsociacionCliente3.Content = "";

            DataContext = this;
            txtScan.Focus();

            if (txtTotalVenta.Text == "0")
            {
                txtTotalVenta.Text = "";
                txtPagaCon.Text = "";
                txtVuelto.Text = "";
            }
            ReciclarVenta();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!bIsCreated_lbInfo)
            //{
            spInfo.Children.Remove(lbUser);
            //lbUser.Content = $"Vendedor: {Settings.UserName}";
            lbUser.Content = $"Vendedor: Christopher Kiessling";
            spInfo.Children.Insert(0, lbUser);
            //bIsCreated_lbInfo = true;
            //}
        }


        #endregion Constructor

        #region Methods

        #region Global
        //private void ShowPopup(bool b)
        //{
        //    expPopup.Content = null;
        //    if (b)
        //    {
        //        expPopup.Visibility = Visibility.Visible;
        //        expPopup.IsExpanded = true;
        //        gridOverlay.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        expPopup.Visibility = Visibility.Hidden;
        //        expPopup.IsExpanded = false;
        //        gridOverlay.Visibility = Visibility.Hidden;
        //    }
        //}

        private void AgregarCantidadAItemVentaExistente(ItemVenta iv)
        {
            // ConfigurarInfoEnvases(iv);
            iv.AgregarCantidad(1);
            ActualizarToolTipDescuento();
            AgregarCantidadAItemVentaCalcularValores(iv);

            //HabilitarDeshabilitarBilletes();
        }

        private void RecalcularValores()
        {
            //listaItemsVenta.ForEach(x =>
            //{
            //    VentaActual_total = x.PrecioUnitario;
            //}
            //);

        }
        #endregion Global

        #region Filtro
        private void expFiltros_Expanded(object sender, RoutedEventArgs e)
        {
            //expFiltros.Width = 900;
            //expFiltros.Height = 600;
        }

        private void expFiltros_Collapsed(object sender, RoutedEventArgs e)
        {
            //expFiltros.Height = 40;
            //expFiltros.Width = 200;
        }
        private void CerrarExpFiltros_Click(object sender, RoutedEventArgs e)
        {
            //expFiltros.IsExpanded = false;
        }

        private void btnAbrirFiltros_Click(object sender, RoutedEventArgs e)
        {
            //expFiltros.IsExpanded = true;
        }
        private void btnHabilitarDeshabilitarFiltros_Click(object sender, RoutedEventArgs e)
        {
            Button btnFiltro = (Button)sender;
            FiltroToggle(bFiltroHabilitar, btnFiltro);
            //FiltroToggle(bFiltroHabilitar, btnAbrirFiltros);
            bFiltroHabilitar ^= true;
        }
        private void filtroComestible_Click(object sender, RoutedEventArgs e)
        {
            Button btnFiltro = (Button)sender;
            FiltroToggle(bFiltroComestible, btnFiltro);
            bFiltroComestible ^= true;
        }

        private void FiltroToggle(bool bFiltro, Button btnFiltro)
        {
            if (!bFiltro)
            {
                btnFiltro.Background = new SolidColorBrush(Color.FromRgb(10, 104, 158));
                btnFiltro.BorderBrush = new SolidColorBrush(Color.FromRgb(10, 104, 158));
                btnFiltro.Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 220));
            }
            else
            {
                btnFiltro.Background = new SolidColorBrush(Color.FromRgb(2, 2, 2));
                btnFiltro.BorderBrush = new SolidColorBrush(Color.FromRgb(2, 2, 2));
                btnFiltro.Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            }
        }
        #endregion Filtro

        #region Overlay
        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                gridOverlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = true;
            }
            else
            {
                teclado.ExpTeclado.IsExpanded = false;
                expTecladoPopup.IsExpanded = false;
                gridOverlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = false;
            }
        }
        #endregion Overlay

        #region Cliente
        // sucede al ingresar un rut, false = encuentra cliente, true = no encuentra
        private void Client_canCreate(bool value)
        {
            var black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            if (value)
            {
                btnClient_add.IsEnabled = true;
                btnClient_Edit.IsEnabled = false;
                btnClient_fav.Foreground = black;

                txtClient_rut.IsEnabled = true;
                btnClient_searchImage.IsEnabled = true;

                btnClient_Edit.Foreground = black;
                txtClient_name.IsReadOnly = false;
                txtClient_name.Foreground = black;
                txtClient_contact.IsReadOnly = false;
                txtClient_contact.Foreground = black;
                txtClient_points.IsReadOnly = false;
                txtClient_points.Foreground = black;
                txtClient_comment.IsReadOnly = false;
                txtClient_comment.Foreground = black;
            }
            // client already exists so can keep searching (editing txtClient_rut) but can't edit fields
            else
            {
                btnClient_add.IsEnabled = false;
                btnClient_Edit.IsEnabled = true;

                txtClient_rut.IsEnabled = true;
                btnClient_searchImage.IsEnabled = false;
                btnClient_Edit.Foreground = black;
                txtClient_name.IsReadOnly = true;
                txtClient_name.Foreground = black;
                txtClient_contact.IsReadOnly = true;
                txtClient_contact.Foreground = black;
                txtClient_points.IsReadOnly = true;
                txtClient_points.Foreground = black;
                txtClient_comment.IsReadOnly = true;
                txtClient_comment.Foreground = black;
            }
        }

        // sucede al presionar el boton toggle de edicion
        private void Client_canEdit(bool value)
        {
            var green = new SolidColorBrush(Color.FromRgb(24, 102, 59));
            var black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            if (value)
            {
                txtClient_rut.IsEnabled = false;
                btnClient_searchImage.IsEnabled = true;

                btnClient_Edit.Foreground = green;
                txtClient_name.IsReadOnly = false;
                txtClient_name.Foreground = green;
                txtClient_contact.IsReadOnly = false;
                txtClient_contact.Foreground = green;
                txtClient_points.IsReadOnly = false;
                txtClient_points.Foreground = green;
                txtClient_comment.IsReadOnly = false;
                txtClient_comment.Foreground = green;
            }
            else
            {
                txtClient_rut.IsEnabled = true;
                btnClient_searchImage.IsEnabled = false;

                btnClient_Edit.Foreground = black;
                txtClient_name.IsReadOnly = true;
                txtClient_name.Foreground = black;
                txtClient_contact.IsReadOnly = true;
                txtClient_contact.Foreground = black;
                txtClient_points.IsReadOnly = true;
                txtClient_points.Foreground = black;
                txtClient_comment.IsReadOnly = true;
                txtClient_comment.Foreground = black;
            }
        }

        private void ActualizarEnvasesInfo()
        {
            envasesQueDebera = envasesQueDebe + envasesQueLleva - envasesQueTrajo;
            // TODO - usar INotifyPropertyChanged para actualizar el valor
            VentaActual_envasesPrestados = envasesQueDebera;
            lbEnvasesQueLleva.Content = $"LLEVA {envasesQueLleva} ENVASES";
            lbEnvasesQueTrajo.Content = $"{envasesQueTrajo}";
            lbEnvasesQueDebera.Content = $"DEBERÁ {envasesQueDebera} ENVASES";

            if (envasesQueDebera == 1)
                lbEnvasesQueDebera.Content = $"DEBERÁ 1 ENVASE";
            if (envasesQueDebera == 0)
                lbEnvasesQueDebera.Content = $"NO DEBERÁ ENVASE";
            if (envasesQueDebera == -1)
                lbEnvasesQueDebera.Content = $"TENDRÁ 1 ENVASE DE SOBRA";
            if (envasesQueDebera < -1)
                lbEnvasesQueDebera.Content = $"TENDRÁ {Math.Abs(envasesQueDebera)} ENVASES DE SOBRA";

            if (envasesQueTrajo == 0 && envasesQueLleva == 0 && envasesQueDebe == 1)
                lbEnvasesQueDebera.Content = $"DEBE {envasesQueDebe} ENVASE";
            if (envasesQueTrajo == 0 && envasesQueLleva == 0 && envasesQueDebe > 1)
                lbEnvasesQueDebera.Content = $"DEBE {envasesQueDebe} ENVASES";

            if (envasesQueTrajo == 0 && envasesQueLleva == 0 && envasesQueDebe == -1)
                lbEnvasesQueDebera.Content = $"TIENE 1 ENVASE DE SOBRA";
            if (envasesQueTrajo == 0 && envasesQueLleva == 0 && envasesQueDebe < -1)
                lbEnvasesQueDebera.Content = $"DEBE {Math.Abs(envasesQueDebe)} ENVASES DE SOBRA";
        }
        #endregion Cliente

        #region Venta

        //private void HabilitarDeshabilitarBilletes()
        //{
        //    if (VentaActual_total > 1000)
        //        btn1000.Opacity = 0.2;
        //    else
        //        btn1000.Opacity = 1;
        //    if (VentaActual_total > 2000)
        //        btn2000.Opacity = 0.2;
        //    else
        //        btn2000.Opacity = 1;
        //    if (VentaActual_total > 5000)
        //        btn5000.Opacity = 0.2;
        //    else
        //        btn5000.Opacity = 1;
        //    if (VentaActual_total > 10000)
        //        btn10000.Opacity = 0.2;
        //    else
        //        btn10000.Opacity = 1;
        //    if (VentaActual_total > 20000)
        //        btn20000.Opacity = 0.2;
        //    else
        //        btn20000.Opacity = 1;
        //}

            /*
        private ItemVenta CreateItemVentaFromProduct(producto p)
        {
            //ItemVenta iv = new ItemVenta()
            //{
            //    ID = p.id,
            //    Barcode = p.codigo_barras,
            //    PrecioUnitario = Convert.ToInt32(p.precio),
            //    Cantidad = 0,
            //    Detalle = p.nombre,
            //    ImagePath = p.imagen,
            //    Puntos = Convert.ToInt32(p.puntos_cantidad),
            //    UnidadesPromo = p.unidades_promo,
            //    Retornable = p.retornable
            //};
            //iv.AgregarCantidad(1);

            //if (iv != null)
            //{
            //    iv.BtnAgregarUnidad.Click += (se, ev) => { AgregarCantidadAItemVentaExistente(iv); };
            //    iv.BtnQuitarUnidad.Click += (se, ev) => { QuitarCantidadAItemVentaExistente(iv); };
            //}

            //return iv;
        }
        */
        private ItemProducto CreateItemProductFromProduct(producto p)
        {
            ItemProducto ip = new ItemProducto()
            {
                Producto = p
            };
            string retornable = "", promo = "";
            if (ip.Producto.retornable == true) retornable = "Retornable";
            else retornable = "Desechable";
            //if (ip.Producto.promo == true) promo = $"Promo: si, {ip.Producto.unidades_promo} unidades";
            //else promo = "Promo: no";
            ip.ToolTip = $"Nombre: {ip.Producto.nombre}\nPrecio: {ip.Producto.precio}\nPutnos: {ip.Producto.puntos_cantidad}\n{retornable}\nStock disponible: {CompraBLL.GetStockByProductId(p.id)}\n{promo}";
            return ip;
        }

        private void CrearItemVenta(ItemVenta iv)
        {
            listaItemsVenta.Add(iv);
            spVentaItems.Children.Add(iv);
            ActualizarToolTipDescuento();
            AgregarCantidadAItemVentaCalcularValores(iv);
            /*
            iv.BtnCantidad.Click += (se, ev) =>
            {
                using (AgregarCantidadPopup acp = new AgregarCantidadPopup(iv))
                {
                    //ShowPopup(true);
                    MostrarOverlay(true);
                    expPopup.Content = acp;
                    acp.OnAgregarCantidad += (se2, ev2) =>
                    {
                        for (int i = 0; i < ev2; i++)
                        {
                            AgregarCantidadAItemVentaExistente(iv);
                        }
                    };
                    acp.OnQuitarCantidad += (se2, ev2) =>
                    {
                        for (int i = 0; i < ev2; i++)
                        {
                            QuitarCantidadAItemVentaExistente(iv);
                        }
                    };
                    acp.OnFinish += (se2, a) => MostrarOverlay(false);
                }
            };
            //HabilitarDeshabilitarBilletes();
            */
        }

        private void QuitarCantidadAItemVentaExistente(ItemVenta iv)
        {
            // ConfigurarInfoEnvases(iv);
            //iv.QuitarCantidad(1);
            if (iv.Cantidad == 0)
            {
                listaItemsVenta.Remove(iv);
                spVentaItems.Children.Remove(iv);
            }
            ActualizarToolTipDescuento();
            QuitarCantidadAItemVentaCalcularValores(iv);

            if (listaItemsVenta.Count == 0)
                ReciclarVenta();
        }
        private void AgregarCantidadAItemVentaCalcularValores(ItemVenta iv)
        {
            //VentaActual_puntos += iv.Puntos;
            //VentaActual_totalSinDcto += 1 * iv.PrecioUnitario; // TODO - cambiar 1 por cantidad

            if (ventaActual_descuentoGlobalEnPct != 0)
                VentaActual_descuento = ventaActual_descuentoGlobalEnPct * VentaActual_totalSinDcto / 100;

            VentaActual_total = VentaActual_totalSinDcto - VentaActual_descuento;
            txtPagaCon.Text = "" + VentaActual_total;

            int pagaCon = Validations.ToInt(txtPagaCon.Text);
            txtVuelto.Text = "" + (pagaCon - VentaActual_total);
            if (txtPagaCon.Text == "")
                txtVuelto.Text = "";
        }
        private void QuitarCantidadAItemVentaCalcularValores(ItemVenta iv)
        {
            //VentaActual_puntos -= iv.Puntos;
            //VentaActual_totalSinDcto -= 1 * iv.PrecioUnitario; // TODO - cambiar 1 por cantidad

            if (ventaActual_descuentoGlobalEnPct != 0)
                VentaActual_descuento = ventaActual_descuentoGlobalEnPct * VentaActual_totalSinDcto / 100;

            VentaActual_total = VentaActual_totalSinDcto - VentaActual_descuento;
            txtPagaCon.Text = "" + VentaActual_total;

            int pagaCon = Validations.ToInt(txtPagaCon.Text);
            txtVuelto.Text = "" + (pagaCon - VentaActual_total);
            if (txtPagaCon.Text == "")
                txtVuelto.Text = "";
        }
        private void ConfigurarInfoEnvases(ItemVenta iv)
        {
            int envasesAgregados = Convert.ToInt32(lbEnvasesQueTrajo.Content);
            //if (iv.Retornable == true)
            //{
            //    int envasesPorAgregar = iv.UnidadesPromo;
            //    envasesQueLleva += envasesPorAgregar;
            //    envasesQueTrajo = envasesQueLleva;
            //    envasesQueDebe = PrestamoEnvaseBLL.CuantoDebeCliente(txtClient_rut.Text);
            //    envasesQueDebera = envasesQueDebe + envasesQueLleva - envasesQueTrajo;
            //    ActualizarEnvasesInfo();
            //}
        }
        private void MostrarProductos(List<producto> listaProductosBusqueda)
        {
            wrapEscogerProductos.Children.Clear();
            foreach (producto p in listaProductosBusqueda)
            {
                ItemProducto ip = CreateItemProductFromProduct(p);
                wrapEscogerProductos.Children.Add(ip);

                ip.BotonProducto.Click += (se2, ev2) =>
                {
                    //ItemVenta itemVentaExistente = listaItemsVenta.Where(x => x.Barcode == ip.Producto.codigo_barras).FirstOrDefault(); // busco si existe el producto en la venta

                    //if (itemVentaExistente != null)
                    //{
                    //    AgregarCantidadAItemVentaExistente(itemVentaExistente);
                    //}
                    //else
                    //{
                    //    ItemVenta iv = CreateItemVentaFromProduct(p);
                    //    CrearItemVenta(iv);
                    //    HabilitarDeshabilitarBotonesVenta();
                    //}
                    //expBuscar.IsExpanded = false;
                    teclado.ExpTeclado.IsExpanded = false;
                    ItemTecladoNumerico.expTecladoNum.IsExpanded = false;
                    ItemTecladoNumerico.expBilletes.IsExpanded = false;
                    //gridOverlay.Visibility = Visibility.Hidden;
                };
            }
        }

        private void MostrarProductosFav(bool b)
        {
            if (b)
            {
                wrapEscogerProductos.Children.Clear();
                ////List<producto> listaMasVendidos = ProductoBLL.GetFavs();
                ////MostrarProductos(listaMasVendidos);
                btnBusquedaProductos_fav.Foreground = new SolidColorBrush(Color.FromRgb(255, 150, 0)); // dorado
            }
            else
            {
                wrapEscogerProductos.Children.Clear();
                btnBusquedaProductos_fav.Foreground = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            }
        }


        private void ReciclarVenta()
        {
            listaItemsVenta.Clear();
            spVentaItems.Children.Clear();
            VentaActual_descuento = 0;
            VentaActual_descuentoGlobal = 0;
            ventaActual_descuentoGlobalEnPct = 0;
            VentaActual_envasesPrestados = 0;
            VentaActual_puntos = 0;
            VentaActual_totalSinDcto = 0;
            VentaActual_total = 0;
            ventaActual_cliente = null;
            ventaActual_mesa = null;
            txtPagaCon.Clear();
            txtTotalVenta.Clear();
            teclado.expTeclado.IsExpanded = false;
            ItemTecladoNumerico.expTecladoNum.IsExpanded = false;
            ItemTecladoNumerico.expBilletes.IsExpanded = false;
            HabilitarDeshabilitarBotonesVenta();
            MostrarOverlay(false);
        }
        private void ReCalcularVuelto()
        {
            int pagaCon = Validations.ToInt(txtPagaCon.Text);
            txtVuelto.Text = "" + (pagaCon - VentaActual_total);
            if (txtPagaCon.Text == "")
                txtVuelto.Text = "";
        }
        private void ActualizarToolTipDescuento()
        {

            if (ventaActual_descuentoGlobalEnPct != 0)
                spAsociacionDescuentoInfo.ToolTip = $"Dcto: {ventaActual_descuentoGlobalEnPct}% de ${VentaActual_totalSinDcto}";
            else
                spAsociacionDescuentoInfo.ToolTip = $"Dcto: ${VentaActual_descuento} (Total sin dcto: ${VentaActual_totalSinDcto})";

            //int i = 1;
            //lbAsociacionDescuento2.ToolTip = "";

            //foreach (var item in listaProductosEnDescuento)
            //{
            //    if (item._descuentoPct != 0)
            //    {
            //        if (i == listaProductosEnDescuento.Count)
            //            lbAsociacionDescuento2.ToolTip += $"Dcto global: ${VentaActual_descuentoGlobal} (${item._descuentoPct}% sobre ${VentaActual_totalSinDcto})";
            //        else
            //            lbAsociacionDescuento2.ToolTip += $"Dcto global: ${VentaActual_descuentoGlobal} (${item._descuentoPct}% sobre ${VentaActual_totalSinDcto})\n";
            //    }
            //    else
            //    {
            //        if (i == listaProductosEnDescuento.Count)
            //            lbAsociacionDescuento2.ToolTip += $"Dcto global: ${item._descuento}";
            //        else
            //            lbAsociacionDescuento2.ToolTip += $"Dcto global: ${item._descuento}\n";
            //    }
            //    i++;
            //}
        }


        private Boolean HayItemsEnVenta()
        {
            return listaItemsVenta.Count() == 0 ? false : true;
        }
        private void HabilitarDeshabilitarBotonesVenta()
        {
            if (HayItemsEnVenta())
            {
                btnAccionPrincipal.IsEnabled = true;
                btnPendiente.IsEnabled = true;
            }
            else
            {
                btnAccionPrincipal.IsEnabled = false;
                btnPendiente.IsEnabled = false;
            }
        }
        #endregion Venta

        #region Pendiente



        #endregion Pendiente

        #region Undefined
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void spPendientes_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void btnSiguienteFoco_Click(object sender, RoutedEventArgs e)
        {
        }
        private int GetTotalOfSell()
        {
            int total = 0;
            foreach (ItemProduct item in spVentaItems.Children)
            {
                total += item.Ammount * item.Quantity;
            }
            return total;
        }



        #endregion Undefined
        #endregion Methods

        #region Events
        private void Events()
        {
            Loaded += (se, a) =>
            {
                spCategorias.Children.Clear();
                ItemBuscarPorSector cmbiCarta = new ItemBuscarPorSector();
                //cmbiCarta.expC.Header = "CARTA";
                CategoriaBLL.ObtenerParaComboBox().ForEach(x =>
                {
                    var cbi = new SubCategoriaBuscarItem();
                    cbi.btnSubCategoria.Content = x.nombre.ToUpper();
                    cmbiCarta.spItems.Children.Add(cbi);
                    cbi.btnSubCategoria.Click += (se2, a2) =>
                    {
                        if (cbi.btnSubCategoria.Content.Equals("TODO"))
                            MostrarProductos(ProductoBLL.ObtenerTodo());
                        //else
                            //MostrarProductos(ProductoBLL.ObtenerCoincidencias(cbi.btnSubCategoria.Content.ToString()));
                    };
                });
                spCategorias.Children.Add(cmbiCarta);

                ItemBuscarPorSector cmbiMenu = new ItemBuscarPorSector();
                //cmbiMenu.expCartaMenu.Header = "MENÚ";
                CategoriaBLL.ObtenerParaComboBox().ForEach(x =>
                {
                    var cbi = new SubCategoriaBuscarItem();
                    cbi.btnSubCategoria.Content = x.nombre.ToUpper();
                    cmbiMenu.spItems.Children.Add(cbi);
                    cbi.btnSubCategoria.Click += (se2, a2) =>
                    {
                        if (cbi.btnSubCategoria.Content.Equals("TODO"))
                            MostrarProductos(ProductoBLL.ObtenerTodo());
                        //else
                        //    MostrarProductos(ProductoBLL.ObtenerCoincidencias(cbi.btnSubCategoria.Content.ToString()));
                    };
                });
                spCategorias.Children.Add(cmbiMenu);
            };

            #region Producto
            btnBuscarLupa.Click += (se, a) =>
            {
                expBuscarLupa.IsExpanded ^= true;
                if (expBuscarLupa.IsExpanded)
                {
                    teclado.ExpTeclado.IsExpanded = true;
                    btnBuscarLupa.Foreground = new SolidColorBrush(Color.FromRgb(255, 150, 0)); // dorado
                    teclado.lastFocusControl = txtBuscar;
                    txtBuscar.Focus();
                }
                else
                {
                    btnBuscarLupa.Foreground = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    teclado.ExpTeclado.IsExpanded = false;
                }
            };
            //btnTecladoToggle.Click += (se, ev) => { ItemTeclado.ExpTeclado.IsExpanded ^= true; };
            txtBuscar.TextChanged += (se, ev) =>
            {
                if (txtBuscar.Text.Contains("'") || txtBuscar.Text.Contains("\""))
                    wrapEscogerProductos.Children.Clear();
                else if (txtBuscar.Text == "")
                {
                    MostrarProductosFav(true);
                    bProductosFavToggle = true;
                }
                else
                {
                    MostrarProductosFav(false);
                    bProductosFavToggle = false;
                    List<producto> listaProductosBusqueda = (ProductoBLL.ObtenerCoincidencias(txtBuscar.Text)).listaProductos;
                    if (listaProductosBusqueda != null)
                        MostrarProductos(listaProductosBusqueda);
                    else
                        wrapEscogerProductos.Children.Clear();
                }
            };
            #endregion Producto

            #region Puntos
            btnPointsInfo.Click += (se, ev) =>
            {
                cliente client = DB.GetClient(txtClient_rut.Text);
                punto points = DB.GetPoints(client);
                if (points != null)
                {
                    var wpi = new WindowPointsInfo(points);
                    wpi.Show();
                }
            };
            #endregion Puntos

            #region Cliente
            expCliente.Expanded += (se, ev) =>
            {
                expDescuento.IsExpanded = false;
                txtClient_rut.Focus();
            };
            btnClientRut_busquedaAvanzada.Click += (se, ev) =>
            {
                PopupCliente_busquedaAvanzada pba = new PopupCliente_busquedaAvanzada(txtClient_rut);
                pba.Show();
            };
            btnClient_fav.Click += (se, ev) =>
            {
                var favColorTrue = new SolidColorBrush(Color.FromRgb(255, 116, 0));
                var favColorFalse = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                cliente c = DB.GetClient(txtClient_rut.Text);

                if (bClient_founded)
                {
                    DB.VipToggle(c);
                    if (c.favorito == true)
                    {
                        btnClient_fav.Foreground = favColorTrue;
                        //lbVIP.Content = "FAV";
                    }
                    else
                    {
                        btnClient_fav.Foreground = favColorFalse;
                        //lbVIP.Content = "";
                    }
                }
                else
                {
                    bClient_isFavPressed ^= true;
                    if (bClient_isFavPressed)
                    {
                        btnClient_fav.Foreground = favColorTrue;
                    }
                    else
                    {
                        btnClient_fav.Foreground = favColorFalse;
                    }
                }
            };
            btnClient_add.Click += (se, ev) =>
            {
                cliente creatingClient = new cliente()
                {
                    rut = txtClient_rut.Text,
                    nombre = txtClient_name.Text,
                    pass = txtClient_rut.Text,
                    imagen = imgClient._name,
                    favorito = bClient_isFavPressed == true ? true : false,
                    contacto = txtClient_contact.Text,
                    punto = new punto() { puntos_activos = Convert.ToInt32(txtClient_points.Text), fecha_expiracion = DateTime.Now.AddDays(30) },
                    comentario = txtClient_comment.Text
                };

                DB.AddClient(creatingClient);
                txtClient_rut.Text = "";
                txtClient_rut.Text = creatingClient.rut;
            };
            btnClient_Edit.Click += (se, ev) =>
            {
                bClient_isEditEnabled ^= true;

                if (bClient_isEditEnabled)
                {
                    Client_canEdit(true);
                }
                else
                {
                    Client_canEdit(false);
                    cliente currentClient = DB.GetClient(txtClient_rut.Text);

                    cliente newClient = currentClient;
                    newClient.imagen = imgClient._name;
                    newClient.nombre = txtClient_name.Text;
                    newClient.contacto = txtClient_contact.Text;
                    newClient.punto = currentClient.punto;
                    newClient.comentario = txtClient_comment.Text;

                    DB.UpdateClient(txtClient_rut.Text, newClient);
                    DB.UpdatePoints(currentClient.punto, Convert.ToInt32(txtClient_points.Text));
                    currentClient.punto.puntos_activos = Convert.ToInt32(txtClient_points.Text);
                }
            };
            btnClient_searchImage.Click += (se, ev) =>
            {
                var wi = new WindowChooseOrTakeImage("clients", imgClient, txtClient_rut.Text);
                wi.Show();
            };
            btnAsociarCliente.Click += (se, ev) =>
            {
                cliente cliente = DB.GetClient(txtClient_rut.Text);
                if (cliente != null)
                {
                    ventaActual_cliente = cliente;
                    //VentaActual_puntos = GetPointsOfSell();
                    spAsociacionCliente.Children.Clear();
                    lbAsociacionCliente1.Content = $"Cliente {cliente.rut} (acumula";
                    // le doy content por binding
                    //lbAsociacionCliente2.Content = $"{VentaActual_puntos}";
                    lbAsociacionCliente3.Content = $"pts)";
                    spAsociacionCliente.Children.Add(lbAsociacionCliente1);
                    spAsociacionCliente.Children.Add(txtAsociacionCliente2);
                    spAsociacionCliente.Children.Add(lbAsociacionCliente3);

                    spAsociacionCliente.Children.Add(btnDesasociarCliente);
                    expAsociacion.IsExpanded ^= true;
                    FiltroToggle(bModulos, btnModulos);
                    bModulos ^= true;
                }
            };
            txtClient_rut.TextChanged += (se, ev) =>
            {
                var disabled = new SolidColorBrush(Color.FromRgb(33, 33, 33));
                var enabled = new SolidColorBrush(Color.FromRgb(33, 33, 33));
                var vipColorTrue = new SolidColorBrush(Color.FromRgb(255, 116, 0));
                var vipColorFalse = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                cliente c = DB.GetClient(txtClient_rut.Text);
                if (c != null)
                {
                    bClient_founded = true;
                    Client_canCreate(false);

                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenCliente"] + c.imagen);
                        image.EndInit();
                        imgClient.Source = image;
                        imgClient._name = c.imagen;

                    }
                    catch (Exception)
                    {
                        imgClient.Source = null;
                    }

                    btnClient_image.ToolTip = c.imagen;
                    txtClient_name.Text = c.nombre;
                    txtClient_contact.Text = c.contacto;
                    txtClient_points.Text = c.punto.puntos_activos + "";
                    txtClient_comment.Text = c.comentario;
                    btnClient_fav.Foreground = c.favorito == true ? vipColorTrue : vipColorFalse;
                    envasesQueDebe = PrestamoEnvaseBLL.CuantoDebeCliente(txtClient_rut.Text);
                    envasesQueTrajo = Convert.ToInt32(lbEnvasesQueTrajo.Content);
                    ActualizarEnvasesInfo();
                    //lbVIP.Content = c.fav == true ? "FAV" : "";
                }
                else
                {
                    // variable para configuracion exclusiva del botón tpggle vip y saber si estoy creando o editando un cliente
                    bClient_founded = false;
                    Client_canCreate(true);
                    btnClient_image.ToolTip = "";
                    txtClient_name.Clear();
                    txtClient_contact.Clear();
                    txtClient_points.Clear();
                    txtClient_comment.Clear();
                    imgClient.Source = null;
                    lbEnvasesQueDebera.Content = "";
                    lbVIP.Content = "";
                    // TODO - validar rut
                }
            };
            #endregion Cliente

            #region Envases
            expEnvases.Expanded += (se, ev) =>
            {
                expEnvases.Background = new SolidColorBrush(Color.FromRgb(203, 203, 203));
            };
            expEnvases.Collapsed += (se, ev) =>
            {
                expEnvases.Background = new SolidColorBrush(Color.FromRgb(214, 214, 214));
            };
            btnEnvasesMas.Click += (se, ev) =>
            {
                envasesQueTrajo++;
                envasesQueDebera = envasesQueDebe + envasesQueLleva - envasesQueTrajo;
                ActualizarEnvasesInfo();
            };
            btnEnvasesMenos.Click += (se, ev) =>
            {
                envasesQueTrajo--;
                if (envasesQueTrajo < 0)
                    envasesQueTrajo = 0;
                // envasesQueDebera = envasesQueDebe + envasesQueLleva - envasesQueTrajo;
                ActualizarEnvasesInfo();
            };
            btnInfoEnvases.Click += (se, ev) =>
            {
                List<prestamo_envases> listaPrestamos = PrestamoEnvaseBLL.GetByClientRut(txtClient_rut.Text);
                PopupEnvasesInfo pei = new PopupEnvasesInfo(txtClient_rut.Text);
                pei.Show();
            };
            #endregion Envases

            #region Descuento
            expDescuento.Expanded += (se, ev) =>
            {
                expCliente.IsExpanded = false;
                //expDescuentoGlobal.IsExpanded = true;
            };


            txtDescuentoGlobalPesos.GotFocus += (se, ev) =>
            {
                txtDescuentoGlobalPct.Text = "";
            };
            txtDescuentoGlobalPct.GotFocus += (se, ev) =>
            {
                txtDescuentoGlobalPesos.Text = "";
            };
            btnAplicarQuitarDescuentoGlobal.Click += (se, ev) =>
            {
                if (!bDescuentoGlobalActivo)
                {
                    ProductInfoCB picb = new ProductInfoCB();

                    if (txtDescuentoGlobalPct.Text.Trim() != "")
                    {
                        ventaActual_descuentoGlobalEnPct = Convert.ToInt32(txtDescuentoGlobalPct.Text);
                        VentaActual_descuento = ventaActual_descuentoGlobalEnPct * VentaActual_totalSinDcto / 100;
                        picb._descuentoPct = ventaActual_descuentoGlobalEnPct;
                        btnAplicarQuitarDescuentoGlobal.Content = $"QUITAR DCTO ({picb._descuentoPct}%)";
                    }
                    if (txtDescuentoGlobalPesos.Text.Trim() != "")
                    {
                        VentaActual_descuento = Convert.ToInt32(txtDescuentoGlobalPesos.Text);
                        ventaActual_descuentoGlobalEnPct = 0;
                        btnAplicarQuitarDescuentoGlobal.Content = $"QUITAR DCTO (${VentaActual_descuento})";
                    }
                    ActualizarToolTipDescuento();
                    VentaActual_total = VentaActual_totalSinDcto - VentaActual_descuento;
                    txtPagaCon.Text = "" + VentaActual_total;
                    spAsociacionDescuento.Children.Clear();
                    spAsociacionDescuento.Children.Add(spAsociacionDescuentoInfo);
                    spAsociacionDescuento.Children.Add(btnDesasociarDescuento);

                    expAsociacion.IsExpanded ^= true;
                    FiltroToggle(bModulos, btnModulos);
                    bModulos ^= true;

                    ReCalcularVuelto();
                    if (txtTotalVenta.Text == "0")
                    {
                        txtTotalVenta.Text = "";
                        txtPagaCon.Text = "";
                        txtVuelto.Text = "";
                    }
                    bDescuentoGlobalActivo = true;


                }
                else
                {
                    if (!btnAplicarQuitarDescuentoGlobal.Content.Equals("APLICAR"))
                    {
                        spAsociacionDescuento.Children.Clear();
                        btnAplicarQuitarDescuentoGlobal.Content = "APLICAR";
                        ventaActual_descuentoGlobalEnPct = 0;
                        VentaActual_descuento = 0;
                        txtDescuentoGlobalPct.Text = "";
                        txtDescuentoGlobalPesos.Text = "";
                        VentaActual_total = VentaActual_totalSinDcto;
                        txtPagaCon.Text = "" + VentaActual_total;

                        //spAsociacionDescuento.Children.Clear();
                        ActualizarToolTipDescuento();
                        //VentaActual_descuento = 0;

                        ReCalcularVuelto();
                        if (txtTotalVenta.Text == "0")
                        {
                            txtTotalVenta.Text = "";
                            txtPagaCon.Text = "";
                            txtVuelto.Text = "";
                        }

                        bDescuentoGlobalActivo = false;
                    }
                }
            };
            #endregion Descuento

            #region BuscarProducto

            btnEscogerCategoria.Click += (se, a) => 
            {
                expCategorias.IsExpanded ^= true;
            };

            btnVerVentas.Click += (se, ev) =>
            {
                using (var ultimasBoletasPopup = new UltimasBoletasPopup())
                {
                    expPopup.Content = ultimasBoletasPopup;
                }
                MostrarOverlay(true);
            };

            btnLimpiarBusqueda.Click += (se, ev) =>
            {
                txtBuscar.Clear();
                wrapEscogerProductos.Children.Clear();
                //cbCategorias.SelectedItem = null;

                MostrarProductosFav(true);
                expBuscarLupa.IsExpanded = false;
                btnBuscarLupa.Foreground = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                teclado.ExpTeclado.IsExpanded = false;
            };

            btnReciclar.Click += (se, ev) =>
            {
                ReciclarVenta();
            };

            btnBusquedaProductos_fav.Click += (se, ev) =>
            {
                if (!bProductosFavToggle)
                {
                    MostrarProductosFav(true);
                    bProductosFavToggle = true;
                }
                else
                {
                    MostrarProductosFav(false);
                    bProductosFavToggle = false;
                }
            };

            txtPagaCon.GotFocus += (se, ev) =>
            {
                txtPagaCon.Text = "";
                ItemTecladoNumerico.expTecladoNum.IsExpanded = true;
                ItemTecladoNumerico.expBilletes.IsExpanded = true;
                //HabilitarDeshabilitarBilletes();

            };
            txtPagaCon.GotMouseCapture += (se, ev) =>
            {
                txtPagaCon.Text = "";
                ItemTecladoNumerico.expTecladoNum.IsExpanded = true;
                ItemTecladoNumerico.expBilletes.IsExpanded = true;
                //HabilitarDeshabilitarBilletes();
            };
            txtPagaCon.TextChanged += (se, ev) =>
            {
                ReCalcularVuelto();
            };
            txtScan.KeyDown += (se, ev) =>
            {
                if (ev.Key == System.Windows.Input.Key.Enter)
                {
                    //producto p = ProductoBLL.GetProduct(txtScan.Text);

                    //ItemProduct ip = new ItemProduct()
                    //{
                    //    ProductName = p.name,
                    //    Quantity = 1,
                    //    Price = Convert.ToInt32(p.price),
                    //    Ammount = Convert.ToInt32(p.price),
                    //    Retornable = p.retornable,
                    //    Promo = p.promo,
                    //    UnidadesPromo = p.unidades_promo,
                    //    Points = Convert.ToInt32(p.points),
                    //    ToolTip = p.name + "    $" + p.price + " C/U",
                    //    Height = 32,
                    //    Margin = new Thickness(4),
                    //    BorderThickness = new Thickness(0),
                    //    Background = new SolidColorBrush(Color.FromRgb(0, 105, 60)),
                    //    Foreground = new SolidColorBrush(Color.FromRgb(214, 214, 214)),
                    //    VerticalContentAlignment = VerticalAlignment.Center,
                    //    HorizontalContentAlignment = HorizontalAlignment.Right
                    //};
                    //ip.Content = $"{ip.ProductName}   x{ip.Quantity}   ${ip.Price}";
                    //AgregarItemVenta(spVentaItems, ip);

                    //Style style = Application.Current.FindResource("MyButtonStyle") as Style;
                    //ip.Style = style;
                }
            };

            btnAccionPrincipal.Click += (se, ev) =>
            {
                switch (Modo)
                {
                    #region Vender
                    case "VENDER":
                        if (listaItemsVenta.Count > 0)
                        {
                            boleta ts;
                            //int pointsOfSell = GetPointsOfSell();
                            cliente cliente = DB.GetClient(txtClient_rut.Text);
                            if (cliente != null)
                            {
                                punto points = DB.GetPoints(cliente);
                                PuntoBLL.Update(points, VentaActual_puntos);
                                //ts = BoletaBLL.Set(cliente.id, Settings.UserID, VentaActual_puntos, ventaActual_total);
                            }
                            //else
                                //ts = BoletaBLL.Set(0, Settings.UserID, 0, ventaActual_total);

                            //foreach (ItemVenta item in listaItemsVenta)
                            //{
                            //    detalle_boleta dl = new detalle_boleta()
                            //    {
                            //        producto_id = item.ID,
                            //        monto = item.Total,
                            //        cantidad = item.Cantidad,
                            //        descuento = 0,
                            //        boleta = ts
                            //    };
                            //    DB.AddDetailLine(dl);

                            //    CompraBLL.ReduceStockByProduct(item.ID, item.Cantidad);
                            //}

                            // TODO - avisar cuando queda poco stock

                            using (BoletaVentaComponent bvc = new BoletaVentaComponent(listaItemsVenta, txtClient_rut))
                            {
                                expPopup.Content = bvc;
                            }

                            MostrarOverlay(true);
                            ReciclarVenta();

                            #region estoyebriodespuesveoesto
                            /*

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
                                                            lbClienteMsg2.Content = $"Acabas de acumular {VentaActual_puntos} puntos";
                                                            lbClienteMsg3.Content = $"Tienes {cliente.punto.puntos_activos + VentaActual_puntos} puntos, expiran el {DateTime.Now.Date.AddDays(30).ToString("dd/MM/yyyy")}";
                                                        }
                                                        else
                                                        {
                                                            spClienteMsg.Children.Clear();
                                                        }


                                                        foreach (ItemVenta item in listaItemsVenta)
                                                        {
                                                            VentaDetailLineControl plc = new VentaDetailLineControl()
                                                            {
                                                                Desc = $"{item.Detalle}          x{item.Cantidad}",
                                                                Valor = item.Cantidad * item.PrecioUnitario,
                                                                ToolTip = $"${item.PrecioUnitario} C/U"
                                                            };
                                                            spDetalleBoleta.Children.Add(plc);
                                                        }


                                                     */

                            #endregion estoyebriodespuesveoesto

                            //List<TextBox> listaTxtLimpiar = new List<TextBox>();
                            //listaTxtLimpiar.Add(txtClient_rut);
                            //listaTxtLimpiar.Add(txtTotalVenta);
                            //listaTxtLimpiar.Add(txtPagaCon);
                            //listaTxtLimpiar.Add(txtVuelto);
                            //listaTxtLimpiar.Add(txtDescuentoGlobalPct);
                            //listaTxtLimpiar.Add(txtDescuentoGlobalPesos);


                            //PopupRealizarVenta prv = new PopupRealizarVenta(txtClient_rut.Text, VentaActual_puntos, VentaActual_total, spVentaItems, listaTxtLimpiar);
                            //prv.Show();
                            //listaItemsVenta.Clear();
                        }
                        else
                            MessageBox.Show("No hay productos para vender");
                        break;
                    #endregion Vender

                    #region Comprar
                    case "COMPRAR":
                        using (ComprarPopup cp = new ComprarPopup(spVentaItems))
                        {
                            expPopup.Content = cp;
                            cp.AlComprar += (se2, e) => ReciclarVenta();
                        }
                        MostrarOverlay(true);
                        break;
                    #endregion Comprar

                    #region Merma
                    //////case "MERMAR":
                    //////    listaItemsVenta.ForEach(x =>
                    //////    {
                    //////        CompraBLL.ReduceStockByProduct(x.ID, x.Cantidad);
                    //////        MermaBLL.Agregar(x.ID, x.Cantidad);
                    //////    });
                    //////    ReciclarVenta();
                    //////    MessageBox.Show("Listo");
                    //////    break;
                    #endregion Merma

                    #region Devolver
                    //////case "DEVOLVER":
                    //////    listaItemsVenta.ForEach(x =>
                    //////    {
                    //////        CompraBLL.Devolver(x.ID, x.Cantidad);
                    //////        DevolucionBLL.Agregar(x.ID, x.Cantidad);
                    //////    });
                    //////    ReciclarVenta();
                    //////    MessageBox.Show("Listo");
                    //////    break;
                    #endregion Devolver

                    #region Pedido
                    case "ENVIAR PEDIDO":
                        ReciclarVenta();
                        MessageBox.Show("Listo");
                        break;
                        #endregion Pedido

                }
            };

            #endregion BuscarProducto

            #region GridOverlay
            gridOverlay.MouseLeftButtonDown += (se, e) => MostrarOverlay(false);
            #endregion GidOverlay

            #region Pendiente


            btnVerPendientes.Click += (se, ev) =>
            {
                using (PendientePopup pp = new PendientePopup(lbVerEntregasNumero))
                {
                    //ShowPopup(true);
                    MostrarOverlay(true);
                    expPopup.Content = pp;
                    pp.DespendientizarEvent += (se2, ev2) =>
                    {
                        List<ItemPendiente> listaItemsPendienteSeleccionados = new List<ItemPendiente>();
                        //foreach (ItemPendiente item in pp.spPendientes.Children)
                        //{
                        //    if (item.bSeleccionado)
                        //        listaItemsPendienteSeleccionados.Add(item);
                        //}
                        //foreach (ItemPendiente item in pp.spPendientesArchivados.Children)
                        //{
                        //    if (item.bSeleccionado)
                        //        listaItemsPendienteSeleccionados.Add(item);
                        //}

                        //foreach (ItemPendiente ip in listaItemsPendienteSeleccionados)
                        //{
                        //    //ItemVenta itemVentaExistente = listaItemsVenta.Where(x => x.Barcode == ip.BarCode).FirstOrDefault(); // busco si existe el producto en la venta
                        //    producto p = ProductoBLL.GetProduct(ip.BarCode);

                        //    //if (itemVentaExistente != null)
                        //    //{
                        //    //    AgregarCantidadAItemVentaExistente(itemVentaExistente);
                        //    //}
                        //    //else
                        //    //{
                        //    //    //ItemVenta iv = CreateItemVentaFromProduct(p);
                        //    //    CrearItemVenta(iv);
                        //    //}
                        //    PendienteBLL.Delete(ip.ID);
                        //}
                        lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Where(x => x.archivado == false).ToList().Count}";
                        HabilitarDeshabilitarBotonesVenta();
                        MostrarOverlay(false);
                    };
                }
            };
            /*
            btnPendiente.Click += (se, ev) =>
            {
                using (UsuarioPendientePopup cpp = new UsuarioPendientePopup(listaItemsVenta))
                {
                    //ShowPopup(true);
                    MostrarOverlay(true);
                    expPopup.Content = cpp;
                    cpp.ReciclarEvent += (se2, ev2) =>
                    {
                        ReciclarVenta();
                        //gridOverlay.Visibility = Visibility.Hidden;
                        MostrarOverlay(false);
                        lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Where(x => x.archivado == false).ToList().Count}";
                        HabilitarDeshabilitarBotonesVenta();
                    };
                }
            };
            */
            //txtEscogerClienteEntregar.TextChanged += (se, ev) =>
            //{
            //    List<clients> listaClientes = new List<clients>();
            //    if (txtEscogerClienteEntregar.Text == "" || txtEscogerClienteEntregar.Text.Contains("'") || txtEscogerClienteEntregar.Text.Contains("\""))
            //    {
            //        wrapEscogerCliente.Children.Clear();
            //    }
            //    else
            //    {
            //        wrapEscogerCliente.Children.Clear();
            //        listaClientes = DB.GetClients(txtEscogerClienteEntregar.Text);
            //        if (listaClientes != null)
            //        {
            //            foreach (clients c in listaClientes)
            //            {
            //                ItemCliente ic = CreateItemClientFromClient(c);
            //                wrapEscogerCliente.Children.Add(ic);

            //                ic.BotonCliente.Click += (se2, ev2) =>
            //                {

            //                };
            //            };
            //        }
            //    }
            //};

            #endregion Pendiente

            #region Modulos
            btnModulos.Click += (se, ev) =>
            {
                //Expander expTecladoCopy = new Expander();
                //expTecladoCopy.Content = expTeclado.Content;
                //Style style = Application.Current.FindResource("ExpanderStyleTeclado") as Style;
                //expTecladoCopy.Style = style;
                //expTecladoCopy.Background = new SolidColorBrush(Color.FromRgb(2, 2, 2));

                //gridOverlay.Visibility = Visibility.Visible;

                //borderTecladoPopup.Child = expTecladoCopy;
                //expTecladoCopy.IsExpanded = true;
                expAsociacion.IsExpanded ^= true;
                //Button btnFiltro = (Button)se;
                ////toggle color
                //FiltroToggle(bModulos, btnFiltro);
                //bModulos ^= true;

                //if (btnAsociarVenta.Content.Equals("-"))
                //    btnAsociarVenta.Content = "AV";
                //else
                //    btnAsociarVenta.Content = "-";
            };
            btnDesasociarMesa.Click += (se, ev) =>
            {
                //spAsociacionMesa.Children.Clear();
                ventaActual_mesa = "";

            };

            btnDesasociarDescuento.Click += (se, ev) =>
            {
                listaDescuentos.Clear();
                ventaActual_descuentoGlobalEnPct = 0;
                VentaActual_descuento = 0;
                //lbAsociacionDescuento.Content = "";
                VentaActual_total = VentaActual_totalSinDcto;
                txtPagaCon.Text = "" + VentaActual_total;
                txtDescuentoGlobalPct.Text = "";
                txtDescuentoGlobalPesos.Text = "";
                spAsociacionDescuento.Children.Clear();
                btnAplicarQuitarDescuentoGlobal.Content = "APLICAR";
                bDescuentoGlobalActivo = false;

                VentaActual_descuento = 0;

                ReCalcularVuelto();
                if (txtTotalVenta.Text == "0")
                {
                    txtTotalVenta.Text = "";
                    txtPagaCon.Text = "";
                    txtVuelto.Text = "";
                }
            };

            btnDesasociarCliente.Click += (se, ev) =>
            {
                spAsociacionCliente.Children.Clear();
                ventaActual_cliente = null;
            };
            #endregion Modulos

            #region Cantidad
            /*
            txtCantidad.LostMouseCapture += (se, ev) =>
            {
                expTecladoNum.HorizontalContentAlignment = HorizontalAlignment.Left;
                expTecladoNum.IsExpanded = false;
                expBilletes.IsExpanded = false;
                expTecladoNum.IsExpanded = true;
                txtCantidad.Text = "1";
            };
            txtCantidad.GotFocus += (se, ev) =>
            {
                expTecladoNum.HorizontalContentAlignment = HorizontalAlignment.Left;
                expTecladoNum.IsExpanded = false;
                expBilletes.IsExpanded = false;
                expTecladoNum.IsExpanded = true;
                txtCantidad.Text = "";
            };
            txtCantidad.LostFocus += (se, ev) =>
            {
                if (txtCantidad.Text.Trim() == "")
                    txtCantidad.Text = "1";
            };
            */
            #endregion Cantidad

            #region Otros
            ItemTecladoNumerico.expBilletes.Collapsed += (se, ev) => { expInfo.IsExpanded = true; };
            ItemTecladoNumerico.expBilletes.Expanded += (se, ev) => { expInfo.IsExpanded = false; };



            #endregion Otros
        }
        #endregion Events


    }
}
