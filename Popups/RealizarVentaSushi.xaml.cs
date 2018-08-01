using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using posk.BLL;
using System.Windows.Media;
using System.Windows.Controls;
using posk.Partials;
using posk.Models;
using posk.Popup;
using posk.Globals;

namespace posk.Popups
{
    public partial class RealizarVentaSushi : Window
    {
        public bool bCerrado = false;
        public int MontoTotalSinPropina { get; set; }
        public int MontoTotalConPropina { get; set; }
        public int Propina { get; set; }
        public bool bPropinaIncluida { get; set; }
        public bool bTxtPropinaVacio { get; set; }

        public int MontoEfectivo { get; set; }
        public int MontoTransBank { get; set; }
        public int MontoJunaeb { get; set; }
        public int MontoOtro { get; set; }
        public int MontoPorPagar { get; set; }

        private cliente clienteEncontrado { get; set; }
        private string ServirLlevarStr { get; set; }

        public string verde = "#FF0A7562";
        public string gris = "#FFC9C9C9";
        public string blanco = "#fff";

        public event EventHandler<DeliveryInfo> AlVender;
        public event EventHandler<DeliveryInfo> AlCerrar;

        public RealizarVentaSushi(int montoTotalSinPropina, int puntos)
        {
            InitializeComponent();
            this.MontoTotalSinPropina = montoTotalSinPropina;

            ServirLlevarBLL.ObtenerTodo().ForEach(x =>
            {
                var itemOpcion = new ItemEscoger() { Nombre = x.nombre.ToUpper() };
                if (x.orden == 0)
                {
                    itemOpcion.Escoger(true);
                    ServirLlevarStr = x.nombre.ToUpper();
                }
                else
                {
                    itemOpcion.Escoger(false);
                }

                itemOpcion.btnOpcion.Click += (se, a) =>
                {
                    spServirLlevar.Children.OfType<ItemEscoger>().ToList().ForEach(y => y.Escoger(false));
                    itemOpcion.Escoger(true);
                    ServirLlevarStr = x.nombre.ToUpper();
                };
                spServirLlevar.Children.Add(itemOpcion);
            });

            bPropinaIncluida = false;
            tooglePropina.IsChecked = false;
            // if (montoTotalSinPropina == 0) return;

            Propina = (montoTotalSinPropina * 10) / 100;
            MontoTotalConPropina = montoTotalSinPropina + Propina;
            MontoTotalSinPropina = montoTotalSinPropina;

            lbTotal.Content = $"{MontoTotalSinPropina}";
            PropinaToogle(false, Propina);

            tooglePropina.Checked += (se, a) => PropinaToogle(true, Propina);
            tooglePropina.Unchecked += (se, a) => PropinaToogle(false, Propina);
            tooglePropina.Click += (se, a) =>
            {
                spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp => imp.Limpiar());
                CalcularMontos();
            };

            txtPropina.GotFocus += (se, a) => txtPropina.Text = "";
            txtPropina.MouseDown += (se, a) => txtPropina.Text = "";

            List<cliente> listaClientes = ClienteBLL.ObtenerTodo();

            btnCerrar.Click += (se, a) =>
            {
                AlCerrar?.Invoke(this, null);
            };

            btnLimpiarCliente.Click += (se, a) =>
            {
                txtBuscarPorTelefono.Text = "";
                txtNombre.Text = "";
                cbDirecciones.ItemsSource = null;
                clienteEncontrado = null;
                txtBuscarPorTelefono.Focus();
                btnVerCompras.IsEnabled = false;
                btnCrearDireccion.IsEnabled = false;
            };

            lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";

            txtBuscarPorTelefono.GotFocus += CursorAlFinalAlObtenerFoco;
            txtNombre.GotFocus += CursorAlFinalAlObtenerFoco;
            txtDireccion.GotFocus += CursorAlFinalAlObtenerFoco;

            expCrearDireccion.Expanded += (se, a) => expVerCompras.IsExpanded = false;
            expVerCompras.Expanded += (se, a) => expCrearDireccion.IsExpanded = false;

            btnAgregarDireccion.Click += (se, a) =>
            {
                cliente c;

                if (clienteEncontrado != null)
                {
                    c = clienteEncontrado;
                }
                else
                {
                    punto pt = PuntoBLL.Crear();

                    ClienteBLL.AddClient(new cliente() { nombre = txtNombre.Text, telefono = $"{txtCodArea.Text} {txtBuscarPorTelefono.Text}", puntos_id = pt.id, pass = "123" });
                    c = ClienteBLL.ObtenerPorTelefono($"{txtCodArea.Text} {txtBuscarPorTelefono.Text}");
                    listaClientes.Add(c);
                }
                if (c != null)
                {
                    DireccionesBLL.AgregarDireccionCliente(c.id, txtDireccion.Text);
                    cbDirecciones.ItemsSource = DireccionesBLL.ObtenerPorCliente(c.id);
                    cbDirecciones.DisplayMemberPath = "nombre";
                    cbDirecciones.SelectedIndex = 0;
                    cbDirecciones.Text = txtDireccion.Text;
                    txtDireccion.Text = "";
                    expCrearDireccion.IsExpanded = false;
                    txtNombre.IsEnabled = false;
                }
            };

            btnVerCompras.Click += (se, a) => MostrarCompras();

            btnCrearDireccion.Click += (se, a) =>
            {
                expCrearDireccion.IsExpanded ^= true;
                if (expCrearDireccion.IsExpanded == true)
                    txtDireccion.Focus();
            };

            txtNombre.TextChanged += (se, a) => HabilitarFormularioCliente();

            txtBuscarPorTelefono.TextChanged += (se, a) =>
            {
                cliente cliPorTelefono = listaClientes.Where(x => x.telefono.Replace(" ", "") == (txtCodArea.Text + txtBuscarPorTelefono.Text).Replace(" ", "")).FirstOrDefault();

                if (cliPorTelefono != null)
                {
                    clienteEncontrado = cliPorTelefono;
                    txtNombre.Text = cliPorTelefono.nombre;
                    cbDirecciones.ItemsSource = DireccionesBLL.ObtenerPorCliente(cliPorTelefono.id);
                    cbDirecciones.DisplayMemberPath = "nombre";
                    cbDirecciones.SelectedIndex = 0;
                }
                else
                {
                    clienteEncontrado = null;
                    txtNombre.Clear();
                    cbDirecciones.ItemsSource = null;
                }

                HabilitarFormularioCliente();
            };

            Loaded += (se, a) =>
            {
                txtPropina.Text = $"{Propina}";

                MostrarPagaCon(true);

                txtPropina.TextChanged += (se2, a2) =>
                {
                    try
                    {
                        spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp => imp.Limpiar());
                        if (txtPropina.Text == "")
                        {
                            PropinaToogle(false, 0);
                            bTxtPropinaVacio = true;
                        }
                        if (txtPropina.Text != "")
                        {
                            bTxtPropinaVacio = false;
                            PropinaToogle(true, Convert.ToInt32(txtPropina.Text));
                        }
                    }
                    catch
                    {
                        txtPropina.Text = "";
                    }
                    CalcularMontos();
                };

                SalsaBLL.ObtenerTodas().ForEach(salsa =>
                {
                    var itemSalsa = new ItemSalsa() { Salsa = salsa };
                    itemSalsa.btnAgregado.Click += (se2, a2) => HabilitarBotonVender();
                    itemSalsa.btnQuitarUnidad.Click += (se2, a2) => HabilitarBotonVender();
                    wrapSalsas.Children.Add(itemSalsa);
                });

                MedioPagoBLL.ObtenerTodos().ForEach(mp =>
                {
                    ItemMedioPagoCalcular impc = new ItemMedioPagoCalcular() { MedioPago = mp };

                    if (impc.MedioPago.nombre.ToLower().Equals("efectivo"))
                    {
                        impc.btnMedioPago.Click += (se2, a2) => MostrarPagaCon(true);
                        impc.txtMonto.TabIndex = 1;
                        impc.txtMonto.Text = $"{MontoTotalSinPropina}";
                        MontoEfectivo = MontoTotalSinPropina;
                        CalcularMontos();
                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoEfectivo = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoEfectivo = 0;
                                HabilitarBotonVender();

                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoEfectivo = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("trans bank"))
                    {
                        impc.btnMedioPago.Click += (se2, a2) => MostrarPagaCon(false);
                        impc.txtMonto.TabIndex = 2;
                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoTransBank = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoTransBank = 0;
                                HabilitarBotonVender();

                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoTransBank = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("junaeb"))
                    {
                        impc.btnMedioPago.Click += (se2, a2) => MostrarPagaCon(false);
                        impc.txtMonto.TabIndex = 3;

                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoJunaeb = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoJunaeb = 0;
                                HabilitarBotonVender();

                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoJunaeb = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("otro"))
                    {
                        impc.btnMedioPago.Click += (se2, a2) => MostrarPagaCon(false);
                        impc.txtMonto.TabIndex = 4;

                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoOtro = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoOtro = 0;
                                HabilitarBotonVender();
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoOtro = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("por pagar"))
                    {
                        impc.btnMedioPago.Click += (se2, a2) => MostrarPagaCon(false);
                        impc.txtMonto.TabIndex = 5;

                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoPorPagar = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoPorPagar = 0;
                                HabilitarBotonVender();
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoPorPagar = 0;
                            }
                            CalcularMontos();
                        };
                    }

                    //impc.txtMonto.TextChanged += (se2, a2) => CalcularMontos();
                    impc.btnMedioPago.Click += (se2, a2) =>
                    {
                        spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp =>
                        {
                            if (imp.btnMedioPago.Content == impc.btnMedioPago.Content)
                            {
                                imp.UsadoEstilo();
                                imp.txtMonto.Text = tooglePropina.IsChecked == true ? $"{MontoTotalConPropina}" : $"{montoTotalSinPropina}";
                            }
                            else
                                imp.Limpiar();
                        });

                        CalcularMontos();
                    };
                    spMedioPago.Children.Add(impc);
                });

                txtBuscarPorTelefono.Focus();
            };

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            btnAceptar.Click += (se, a) =>
            {
                try
                {
                    ItemMedioPagoCalcular impcEfectivo = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Efectivo")).FirstOrDefault();
                    int efectivo = Convert.ToInt32(String.IsNullOrEmpty(impcEfectivo.txtMonto.Text) == true ? "0" : impcEfectivo.txtMonto.Text);

                    ItemMedioPagoCalcular impcTransBank = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Trans Bank")).FirstOrDefault();
                    int transBank = Convert.ToInt32(String.IsNullOrEmpty(impcTransBank.txtMonto.Text) == true ? "0" : impcTransBank.txtMonto.Text);

                    ItemMedioPagoCalcular impcJunaeb = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Junaeb")).FirstOrDefault();
                    int junaeb = Convert.ToInt32(String.IsNullOrEmpty(impcJunaeb.txtMonto.Text) == true ? "0" : impcJunaeb.txtMonto.Text);

                    ItemMedioPagoCalcular impcOtro = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Otro")).FirstOrDefault();
                    int otro = Convert.ToInt32(String.IsNullOrEmpty(impcOtro.txtMonto.Text) == true ? "0" : impcOtro.txtMonto.Text);

                    int propina = tooglePropina.IsChecked == true ? Convert.ToInt32(txtPropina.Text) : 0;

                    var itemPagaCon = gridPagaCon.Children.OfType<ItemPagaCon>().FirstOrDefault();
                    int pagaCon = itemPagaCon == null ? 0 : itemPagaCon.PagaCon;
                    string vuelto = itemPagaCon?.Vuelto;

                    string incluyeStr = "";
                    string incluyeStrUnaLinea = "";

                    wrapSalsas.Children.OfType<ItemSalsa>().ToList().ForEach(salsa =>
                    {
                        if (salsa.Cantidad != 0)
                            incluyeStr += $"{salsa.txtNombre.Text} x{salsa.Cantidad}\n";
                    });

                    wrapSalsas.Children.OfType<ItemSalsa>().ToList().ForEach(salsa =>
                    {
                        if (salsa.Cantidad != 0)
                            incluyeStrUnaLinea += $"{salsa.txtNombre.Text} x{salsa.Cantidad}, ";
                    });
                    if (incluyeStrUnaLinea != "")
                        incluyeStrUnaLinea = incluyeStrUnaLinea.Substring(0, incluyeStrUnaLinea.Length - 2);

                    DeliveryInfo di = new DeliveryInfo()
                    {
                        Efectivo = efectivo,
                        TransBank = transBank,
                        Junaeb = junaeb,
                        Otro = otro,
                        Propina = propina,
                        NombreCliente = txtNombre.Text,
                        ServirLlevar = ServirLlevarStr,
                        MensajeTicket = txtMensajeTicket.Text,
                        Telefono = $"{txtCodArea.Text} {txtBuscarPorTelefono.Text}",
                        Direccion = cbDirecciones.Text,
                        MensajeDeliveryUno = txtMensajeDeliveryUno.Text,
                        Incluye = incluyeStr,
                        IncluyeStrUnaLinea = incluyeStrUnaLinea,
                        PagaCon = pagaCon,
                        Vuelto = vuelto
                    };

                    string nombre = txtBuscarPorTelefono.Text;
                    string nombredos = nombre + "asd";

                    AlVender.Invoke(this, di);
                }
                catch (Exception ex)
                {
                    Globals.PoskException.Make(ex, "Error al vender");
                }
            };

            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
        }

        private void MostrarCompras()
        {
            if (clienteEncontrado != null)
            {
                try
                {
                    int total = 0;
                    spComprasCliente.Children.Clear();
                    BoletaBLL.ObtenerPorCliente(clienteEncontrado.id).ForEach(boleta =>
                    {
                        total += (int)boleta.total;
                        spComprasCliente.Children.Add(new Label()
                        {
                            Content = $"El {boleta.fecha.ToShortDateString()} a las {boleta.fecha.Hour}:{boleta.fecha.Minute} - TOTAL: ${boleta.total}",
                            Foreground = new SolidColorBrush(Color.FromRgb(12, 12, 12))
                        });
                    });
                    expVerCompras.IsExpanded ^= true;
                    labelTotalComprasCliente.Content = $"Total ${total}";
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al obtener compras cliente");
                }
            }
            else
            {
                spComprasCliente.Children.Clear();
                expVerCompras.IsExpanded = false;
            }
        }

        private void CursorAlFinalAlObtenerFoco(object sender, RoutedEventArgs e)
        {
            if (txtBuscarPorTelefono.Text.Length != 0)
            {
                TextBox tb = (TextBox)sender;
                tb.CaretIndex = tb.Text.Length;
            }
        }

        private void HabilitarFormularioCliente()
        {
            btnVerCompras.IsEnabled = false;
            btnCrearDireccion.IsEnabled = false;

            if (clienteEncontrado != null)
            {
                txtNombre.IsEnabled = false;
                btnCrearDireccion.IsEnabled = true;
                btnVerCompras.IsEnabled = true;
            }
            else
            {
                txtNombre.IsEnabled = true;
                btnVerCompras.IsEnabled = true;
                if (txtNombre.Text != "" && txtBuscarPorTelefono.Text.Length == 8)
                {
                    btnCrearDireccion.IsEnabled = true;
                    btnVerCompras.IsEnabled = true;
                }
                else
                {
                    btnCrearDireccion.IsEnabled = false;
                    btnVerCompras.IsEnabled = false;
                }
            }
        }

        private void HabilitarBotonVender()
        {
            int contador = 0;
            wrapSalsas.Children.OfType<ItemSalsa>().ToList().ForEach(x => contador += x.Cantidad);

            if (CalcularMontos() && contador != 0)
                btnAceptar.IsEnabled = true;
            else
                btnAceptar.IsEnabled = false;

        }

        private void MostrarPagaCon(bool b)
        {
            gridPagaCon.Children.Clear();

            if (!b) return;

            int montoCobrar = tooglePropina.IsChecked == true ? MontoTotalConPropina : MontoTotalSinPropina;
            var itemPagaCon = new ItemPagaCon() { Total = montoCobrar };
            itemPagaCon.txtPagaCon.TextChanged += (se2, a2) =>
            {
                try
                {
                    // usa RegEx para aceptar solo valores numéricos
                    itemPagaCon.PagaCon = Convert.ToInt32(itemPagaCon.txtPagaCon.Text);
                }
                catch (Exception)
                {

                }
            };

            gridPagaCon.Children.Add(itemPagaCon);
        }

        private bool CalcularMontos()
        {
            int _propina = 0;
            int _montoTotal = 0;
            bool bIncluyeProp = tooglePropina.IsChecked == true ? true : false;

            if (bIncluyeProp)
            {
                try
                {
                    if (!txtPropina.Text.Equals(""))
                        _propina = Convert.ToInt32(txtPropina.Text);
                }
                catch
                {

                }
            }
            _montoTotal = Convert.ToInt32(lbTotal.Content);

            if (MontoEfectivo + MontoTransBank + MontoJunaeb + MontoOtro + MontoPorPagar == _montoTotal)
                return true;
            else
                return false;
        }

        private void PropinaToogle(bool bPropina, int propina)
        {
            var bc = new BrushConverter();
            Propina = propina;
            MontoTotalConPropina = MontoTotalSinPropina + Propina;
            MontoTotalSinPropina = MontoTotalConPropina - Propina;

            if (bPropina)
            {
                bPropinaIncluida = bPropina;
                tooglePropina.Background = (Brush)bc.ConvertFrom(verde);
                tooglePropina.Foreground = (Brush)bc.ConvertFrom(blanco);
                lbTotal.Content = $"{MontoTotalConPropina}";
                tooglePropina.IsChecked = true;
                if (bTxtPropinaVacio)
                {
                    bTxtPropinaVacio = false;
                    txtPropina.Text = $"{(MontoTotalSinPropina * 10) / 100}";
                    CalcularMontos();
                }
            }
            else
            {
                bPropinaIncluida = bPropina;
                tooglePropina.Background = (Brush)bc.ConvertFrom(gris);
                tooglePropina.Foreground = (Brush)bc.ConvertFrom(blanco);
                lbTotal.Content = $"{MontoTotalSinPropina}";
                if (Propina == 0)
                    txtPropina.Text = "";
                else
                    txtPropina.Text = $"{Propina}";
                tooglePropina.IsChecked = false;
            }
        }

        private void ValidarNumero(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
