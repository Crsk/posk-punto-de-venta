using posk.BLL;
using posk.Components;
using posk.Globals;
using posk.Models;
using posk.Pages.Graficos;
using posk.Pages.Menu;
using posk.Popup;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace posk
{
    public partial class MainWindow : Window
    {
        Label lbInternetStatus = new Label();
        public static Frame MainFrame;
        public static Button BtnMenu;
        public static ImageBrush FotoUsuario;
        public static Label LbNombreUsuario, LbRol;

        public static PageVerInventario PageVerInventario { get; set; }
        public static PageInicio PageInicio { get; set; }
        public static PageVenta PageVenta { get; set; }
        public static PageMesas PageMesas { get; set; }
        public static PageCompra PageCompra { get; set; }
        public static PageDevolucion PageDevolucion { get; set; }
        public static PageMerma PageMerma { get; set; }
        public static PageGasto PageGasto { get; set; }
        public static PageBruno PageBruno { get; set; }
        public static PageConfig PageOpciones { get; set; }

        public static PageResumenJornada PageResumenJornada { get; set; }

        public static PageEstadisticasPeriodo PageEstPeriodo { get; set; }
        public static PageEstadisticasGlobalIngresos PageEstGlobalIngresos { get; set; }
        public static PageEstadisticasPorFecha PageEstFecha { get; set; }
        public static PageEstadisticasUsuario PageEstUsuario { get; set; }
        public static PageEstadisticasStock PageEstStock { get; set; }

        public static PageAdministrarProductoDos PageAdmProducto { get; set; }
        public static PageAdministrarMateriaPrima PageAdmMateriaPrima { get; set; }
        public static PageAdministrarUsuario PageAdmUsuario { get; set; }
        public static PageAdministrarCliente PageAdmCliente { get; set; }
        // public static PageAdministrarCliente PageAdm { get; set; }
        public static PageAdministrarBoleta PageAdmBoleta { get; set; }
        public static PageAdministrarSectoresCategorias PageAdmSectoresCategorias { get; set; }
        public static PageAdministrarRelaciones PageAdmRelaciones { get; set; }
        public static PageAdministrarPromociones PageAdmPromociones { get; set; }
        public static PageAdministrarStock PageAdmStock { get; set; }
        public static PageAdministrarTipoDeProducto PageAdmTipoProducto { get; set; }
        public static PageRelacionarTipoProducto PageRelTipoProducto { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            MainFrame = menuFrame;
            BtnMenu = btnMenu;
            FotoUsuario = fotoUsuario;
            LbNombreUsuario = lbNombreUsuario;
            LbRol = lbRol;

            if (DatosNegocioBLL.PagoInmediato())
                MostrarItemMesas(false);
            else
                MostrarItemMesas(true);

            GlobalSettings.Modo = DatosNegocioBLL.ObtenerModo();
            GlobalSettings.UsarTecladoTactilIntegrado = DatosNegocioBLL.ObtenerConfiguracionTeclado();

            InitEvents();
            btnMenu.IsEnabled = false;
            btnMenu.Click += (se, ev) => { AbrirMenu(true); };
            gridOverlay.MouseLeftButtonDown += (se, e) => { AbrirMenu(false); };

            if (Properties.Settings.Default.AppName.Equals("posk"))
            {
                PagePrimerInicio ppi = new PagePrimerInicio();
                menuFrame.Content = ppi;
                ppi.OnFirstInit += (se, a) => menuFrame.Content = new PageInicio();
            }
            else
                menuFrame.Content = new PageInicio();

            PageMesas.AlEscogerMesa += (se, a) => AbrirSeccionVenta(a.Usuario, a.Mesa);
        }

        private void MostrarItemMesas(bool b)
        {
            if (b)
            {
                miMesas.Height = 54;
                miMesas.Visibility = Visibility.Visible;
            }
            else
            {
                miMesas.Height = 0;
                miMesas.Visibility = Visibility.Hidden;
            }
        }

        private void TerminarJornada()
        {
            if (!JornadaBLL.JornadaAbierta()) return; //si ya está cerrada: return
            if (MessageBox.Show("¿Cerrar jornada?", "", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            JornadaBLL.TerminarJornadaSiEstaIniciada(DateTime.Now, 0, "");
            AbrirMenu(false);
            Logout();
            new Notification("JORNADA TERMINADA");

            InformeJornada.EnviarInformeJornada();
        }

        #region metodos

        private void AbrirMenu(bool b)
        {
            if (b)
            {
                gridOverlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
                expMenu.IsExpanded = true;

                expMenuGlobalExtension.IsExpanded = false;
                expMenuJornadaExtension.IsExpanded = false;
                expMenuStockExtension.IsExpanded = false;
                expMenuAdministrarExtension.IsExpanded = false;
                expMenuAdministrarExtension.IsExpanded = false;
            }
            else
            {
                gridOverlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
                expMenu.IsExpanded = false;
                expMenuAdministrarExtension.IsExpanded = false;
            }
        }

        private void SetMenuItemColor(MenuItem mi)
        {
            AbrirMenu(false);
            miVerInventario.Background = null;
            miInicio.Background = null;
            miVenta.Background = null;
            miMesas.Background = null;
            miCompra.Background = null;
            miDevolucion.Background = null;
            miMerma.Background = null;
            miGasto.Background = null;
            miOpciones.Background = null;
            miJornadaVentas.Background = null;
            miJornadaResumen.Background = null;

            miAdministrarProducto.Background = null;
            miAdministrarMateriaPrima.Background = null;
            miAdministrarUsuario.Background = null;
            miAdministrarCliente.Background = null;
            miAdministrarBoletas.Background = null;
            miAdministrarSectoresCategorias.Background = null;
            miAdministrarRelaciones.Background = null;
            miAdministrarPromociones.Background = null;
            miAdministrarStock.Background = null;
            miAdministrarTipoDeProducto.Background = null;
            miAdministrarRelacionesTipoDeProducto.Background = null;

            miEstadisticas.Background = null;
            miAdministrar.Background = null;
            miJornada.Background = null;
            miGlobal.Background = null;

            miEstadisticasGlobalIngresos.Background = null;
            miEstadisticasFecha.Background = null;
            miEstadisticasUsuario.Background = null;
            miEstadisticasCliente.Background = null;
            miEstadisticasStock.Background = null;
            if (mi == miInicio) return;
            mi.Background = new SolidColorBrush(Color.FromArgb(80, 0, 100, 255));
        }

        private void Logout()
        {
            if (PageInicio != null)
                menuFrame.Content = PageInicio;
            else
            {
                PageInicio = new PageInicio();
                menuFrame.Content = PageInicio;
            }
        }

        private void CerrarJornada()
        {
            if (PageVerInventario != null)
                menuFrame.Content = PageVerInventario;
            else
            {
                PageVerInventario = new PageVerInventario();
                menuFrame.Content = PageVerInventario;
            }
            SetMenuItemColor(miVerInventario);
        }

        private void AbrirSeccionVenta(usuario u, mesa m)
        {
            if (PageVenta != null)
                menuFrame.Content = PageVenta;
            else
            {
                PageVenta = new PageVenta(u, m);
                menuFrame.Content = PageVenta;
            }
            PrincipalComponent.CargarGarzonMesa(u, m);
            SetMenuItemColor(miVenta);
        }

        #endregion metodos

        #region eventos
        private void InitEvents()
        {
            btnCerrar.Click += (se, a) =>
            {
                TerminarJornada();
                Close();
            };

            #region paginas principales

            miTerminarJornada.Click += (se, a) =>
            {
                TerminarJornada();
            };

            miVerInventario.Click += (se, a) =>
            {
                if (PageVerInventario != null)
                    menuFrame.Content = PageVerInventario;
                else
                {
                    PageVerInventario = new PageVerInventario();
                    menuFrame.Content = PageVerInventario;
                }
                SetMenuItemColor(miVerInventario);
            };

            miInicio.Click += (se, a) =>
            {
                Logout();
                SetMenuItemColor(miInicio);
            };

            miVenta.Click += (se, a) =>
            {
                AbrirSeccionVenta(null, null);
            };
            miMesas.Click += (se, a) =>
            {
                if (PageMesas != null)
                    menuFrame.Content = PageMesas;
                else
                {
                    PageMesas = new PageMesas(Settings.Usuario.tipo);
                    menuFrame.Content = PageMesas;
                }
                SetMenuItemColor(miMesas);
            };
            miCompra.Click += (se, a) =>
            {
                if (PageCompra != null)
                    menuFrame.Content = PageCompra;
                else
                {
                    PageCompra = new PageCompra();
                    menuFrame.Content = PageCompra;
                }
                SetMenuItemColor(miCompra);
            };
            miDevolucion.Click += (se, a) =>
            {
                if (PageDevolucion != null)
                    menuFrame.Content = PageDevolucion;
                else
                {
                    PageDevolucion = new PageDevolucion();
                    menuFrame.Content = PageDevolucion;
                }
                SetMenuItemColor(miDevolucion);
            };
            miMerma.Click += (se, a) =>
            {
                if (PageMerma != null)
                    menuFrame.Content = PageMerma;
                else
                {
                    PageMerma = new PageMerma();
                    menuFrame.Content = PageMerma;
                }
                SetMenuItemColor(miMerma);
            };
            miGasto.Click += (se, a) =>
            {
                if (PageGasto == null)
                    PageGasto = new PageGasto();

                menuFrame.Content = PageGasto;
                SetMenuItemColor(miGasto);
            };

            miJornadaResumen.Click += (se, a) =>
            {
                if (PageResumenJornada == null)
                    PageResumenJornada = new PageResumenJornada();

                menuFrame.Content = PageResumenJornada;
                SetMenuItemColor(miJornadaResumen);
            };

            miOpciones.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                }
                else
                {
                    if (PageOpciones != null)
                        menuFrame.Content = PageOpciones;
                    else
                    {
                        PageOpciones = new PageConfig();
                        menuFrame.Content = PageOpciones;
                    }
                    SetMenuItemColor(miOpciones);
                }
            };
            #endregion paginas principales

            #region paginas estadistica
            miEstadisticasJornada.Click += (se, a) =>
            {
                if (PageEstPeriodo != null)
                    menuFrame.Content = PageEstPeriodo;
                else
                {
                    PageEstPeriodo = new PageEstadisticasPeriodo();
                    menuFrame.Content = PageEstPeriodo;
                }
                SetMenuItemColor(miEstadisticasJornada);
            };
            miEstadisticasGlobalIngresos.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageEstGlobalIngresos != null)
                    menuFrame.Content = PageEstGlobalIngresos;
                else
                {
                    PageEstGlobalIngresos = new PageEstadisticasGlobalIngresos();
                    menuFrame.Content = PageEstGlobalIngresos;
                }
                SetMenuItemColor(miEstadisticasGlobalIngresos);
            };
            miEstadisticasFecha.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageEstFecha != null)
                    menuFrame.Content = PageEstFecha;
                else
                {
                    PageEstFecha = new PageEstadisticasPorFecha();
                    menuFrame.Content = PageEstFecha;
                }
                SetMenuItemColor(miEstadisticasFecha);
            };

            miEstadisticasUsuario.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageEstUsuario != null)
                    menuFrame.Content = PageEstUsuario;
                else
                {
                    PageEstUsuario = new PageEstadisticasUsuario();
                    menuFrame.Content = PageEstUsuario;
                }
                SetMenuItemColor(miEstadisticasUsuario);
            };

            miEstadisticasStock.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageEstStock != null)
                    menuFrame.Content = PageEstStock;
                else
                {
                    PageEstStock = new PageEstadisticasStock();
                    menuFrame.Content = PageEstStock;
                }
                SetMenuItemColor(miEstadisticasStock);
            };
            #endregion paginas estadistica

            #region paginas administrar
            miAdministrarProducto.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmProducto != null)
                    menuFrame.Content = PageAdmProducto;
                else
                {
                    PageAdmProducto = new PageAdministrarProductoDos();
                    menuFrame.Content = PageAdmProducto;
                }
                SetMenuItemColor(miAdministrarProducto);
            };
            miAdministrarMateriaPrima.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmMateriaPrima != null)
                    menuFrame.Content = PageAdmMateriaPrima;
                else
                {
                    PageAdmMateriaPrima = new PageAdministrarMateriaPrima();
                    menuFrame.Content = PageAdmMateriaPrima;
                }
                SetMenuItemColor(miAdministrarMateriaPrima);
            };
            miAdministrarUsuario.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmUsuario != null)
                    menuFrame.Content = PageAdmUsuario;
                else
                {
                    PageAdmUsuario = new PageAdministrarUsuario();
                    menuFrame.Content = PageAdmUsuario;
                }
                SetMenuItemColor(miAdministrarUsuario);
            };

            miAdministrarCliente.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmCliente != null)
                    menuFrame.Content = PageAdmCliente;
                else
                {
                    PageAdmCliente = new PageAdministrarCliente();
                    menuFrame.Content = PageAdmCliente;
                }
                SetMenuItemColor(miAdministrarCliente);
            };

            miAdministrarBoletas.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmBoleta != null)
                    menuFrame.Content = PageAdmBoleta;
                else
                {
                    PageAdmBoleta = new PageAdministrarBoleta();
                    menuFrame.Content = PageAdmBoleta;
                }
                SetMenuItemColor(miAdministrarBoletas);
            };

            miAdministrarSectoresCategorias.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmSectoresCategorias != null)
                    menuFrame.Content = PageAdmSectoresCategorias;
                else
                {
                    PageAdmSectoresCategorias = new PageAdministrarSectoresCategorias();
                    menuFrame.Content = PageAdmSectoresCategorias;
                }
                SetMenuItemColor(miAdministrarSectoresCategorias);
            };
            miAdministrarRelaciones.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmRelaciones != null)
                    menuFrame.Content = PageAdmRelaciones;
                else
                {
                    PageAdmRelaciones = new PageAdministrarRelaciones();
                    menuFrame.Content = PageAdmRelaciones;
                }
                SetMenuItemColor(miAdministrarRelaciones);
            };
            miAdministrarPromociones.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmPromociones != null)
                    menuFrame.Content = PageAdmPromociones;
                else
                {
                    PageAdmPromociones = new PageAdministrarPromociones();
                    menuFrame.Content = PageAdmPromociones;
                }
                SetMenuItemColor(miAdministrarPromociones);
            };
            miAdministrarStock.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmStock != null)
                    menuFrame.Content = PageAdmStock;
                else
                {
                    PageAdmStock = new PageAdministrarStock();
                    menuFrame.Content = PageAdmStock;
                }
                SetMenuItemColor(miAdministrarStock);
            };
            miAdministrarTipoDeProducto.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageAdmTipoProducto != null)
                    menuFrame.Content = PageAdmTipoProducto;
                else
                {
                    PageAdmTipoProducto = new PageAdministrarTipoDeProducto();
                    menuFrame.Content = PageAdmTipoProducto;
                }
                SetMenuItemColor(miAdministrarTipoDeProducto);
            };

            miAdministrarRelacionesTipoDeProducto.Click += (se, a) =>
            {
                if (!Settings.Usuario.tipo.ToLower().Equals("a"))
                {
                    new Notification("NO ESTAS AUTORIZADO", "", Notification.Type.Warning, 4);
                    return;
                }
                if (PageRelTipoProducto != null)
                    menuFrame.Content = PageRelTipoProducto;
                else
                {
                    PageRelTipoProducto = new PageRelacionarTipoProducto();
                    menuFrame.Content = PageRelTipoProducto;
                }
                SetMenuItemColor(miAdministrarRelacionesTipoDeProducto);
            };
            #endregion paginas administrar

            #region menu expandible
            miJornada.Click += (se, ev) =>
            {
                expMenuJornadaExtension.IsExpanded ^= true;
            };

            miStock.Click += (se, ev) =>
            {
                expMenuStockExtension.IsExpanded ^= true;
            };

            miAdministrar.Click += (se, ev) =>
            {
                expMenuAdministrarExtension.IsExpanded ^= true;
            };

            miEstadisticas.Click += (se, ev) =>
            {
                expMenuAdministrarExtensionEstadisticas.IsExpanded ^= true;
            };

            miGlobal.Click += (se, ev) =>
            {
                expMenuGlobalExtension.IsExpanded ^= true;
            };
            #endregion menu expandible

            expMenuGlobalExtension.Expanded += (se, a) =>
            {
                miGlobal.Background = new SolidColorBrush(Color.FromArgb(80, 0, 160, 255));
                miJornada.Background = null;
                miAdministrar.Background = null;
                miEstadisticas.Background = null;
                miStock.Background = null;
                expMenuJornadaExtension.IsExpanded = false;
                expMenuAdministrarExtension.IsExpanded = false;
                expMenuAdministrarExtensionEstadisticas.IsExpanded = false;
                expMenuStockExtension.IsExpanded = false;
            };
            expMenuGlobalExtension.Collapsed += (se, a) => miGlobal.Background = null;

            expMenuJornadaExtension.Expanded += (se, a) =>
            {
                miJornada.Background = new SolidColorBrush(Color.FromArgb(80, 0, 160, 255));
                miAdministrar.Background = null;
                miEstadisticas.Background = null;
                miStock.Background = null;
                miGlobal.Background = null;
                expMenuAdministrarExtension.IsExpanded = false;
                expMenuAdministrarExtensionEstadisticas.IsExpanded = false;
                expMenuStockExtension.IsExpanded = false;
                expMenuGlobalExtension.IsExpanded = false;
            };
            expMenuStockExtension.Collapsed += (se, a) => miJornada.Background = null;

            expMenuStockExtension.Expanded += (se, a) =>
            {
                miStock.Background = new SolidColorBrush(Color.FromArgb(80, 0, 160, 255));
                miJornada.Background = null;
                miAdministrar.Background = null;
                miEstadisticas.Background = null;
                miGlobal.Background = null;
                expMenuAdministrarExtension.IsExpanded = false;
                expMenuAdministrarExtensionEstadisticas.IsExpanded = false;
                expMenuJornadaExtension.IsExpanded = false;
                expMenuGlobalExtension.IsExpanded = false;
            };
            expMenuStockExtension.Collapsed += (se, a) => miStock.Background = null;

            expMenuAdministrarExtensionEstadisticas.Expanded += (se, a) =>
            {
                miJornada.Background = null;
                miStock.Background = null;
                miEstadisticas.Background = new SolidColorBrush(Color.FromArgb(80, 0, 160, 255));
                miAdministrar.Background = null;
                miGlobal.Background = null;
                expMenuAdministrarExtension.IsExpanded = false;
                expMenuStockExtension.IsExpanded = false;
                expMenuJornadaExtension.IsExpanded = false;
                expMenuGlobalExtension.IsExpanded = false;
            };
            expMenuAdministrarExtensionEstadisticas.Collapsed += (se, a) => miEstadisticas.Background = null;

            expMenuAdministrarExtension.Expanded += (se, a) =>
            {
                miJornada.Background = null;
                miStock.Background = null;
                miEstadisticas.Background = null;
                miGlobal.Background = null;
                miAdministrar.Background = new SolidColorBrush(Color.FromArgb(80, 0, 160, 255));
                expMenuAdministrarExtensionEstadisticas.IsExpanded = false;
                expMenuStockExtension.IsExpanded = false;
                expMenuJornadaExtension.IsExpanded = false;
                expMenuGlobalExtension.IsExpanded = false;
            };
            expMenuAdministrarExtension.Collapsed += (se, a) => miAdministrar.Background = null;
        }
        protected override void OnClosed(EventArgs e) { base.OnClosed(e); Application.Current.Shutdown(); }
        #endregion eventos

    }
}
