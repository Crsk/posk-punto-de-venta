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

        private string ServirLlevarStr { get; set; }

        public string verde = "#FF0A7562";
        public string gris = "#FFC9C9C9";
        public string blanco = "#fff";

        private bool bEscribiendoEnCliente;

        public event EventHandler<DeliveryInfo> AlVender;
        public event EventHandler<DeliveryInfo> AlCerrar;

        public RealizarVentaSushi(int montoTotalSinPropina, int puntos)
        {
            InitializeComponent();
            this.MontoTotalSinPropina = montoTotalSinPropina;

            ServirLlevarStr = "SERVIR";
            bEscribiendoEnCliente = false;

            var bc = new BrushConverter();
            btnServir.Background = (Brush)bc.ConvertFrom(verde);
            btnServir.Foreground = (Brush)bc.ConvertFrom(blanco);
            btnLlevar.Background = (Brush)bc.ConvertFrom(gris);
            btnLlevar.Foreground = (Brush)bc.ConvertFrom(blanco);

            btnServir.Click += (se, a) =>
            {
                ServirLlevarStr = "SERVIR";
                btnServir.Background = (Brush)bc.ConvertFrom(verde);
                btnServir.Foreground = (Brush)bc.ConvertFrom(blanco);
                btnLlevar.Background = (Brush)bc.ConvertFrom(gris);
                btnLlevar.Foreground = (Brush)bc.ConvertFrom(blanco);
            };

            btnLlevar.Click += (se, a) =>
            {
                ServirLlevarStr = "LLEVAR";
                btnLlevar.Background = (Brush)bc.ConvertFrom(verde);
                btnLlevar.Foreground = (Brush)bc.ConvertFrom(blanco);
                btnServir.Background = (Brush)bc.ConvertFrom(gris);
                btnServir.Foreground = (Brush)bc.ConvertFrom(blanco);
            };

            bPropinaIncluida = false;
            tooglePropina.IsChecked = false;
            if (montoTotalSinPropina == 0) return;

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

            cbClientes.ItemsSource = listaClientes;
            cbClientes.DisplayMemberPath = "nombre";
            cbClientes.SelectionChanged += (se, a) =>
            {
                try
                {
                    cliente cli = cbClientes.SelectedItem as cliente;
                    lbClienteEncontrado.Content = $"{cli?.nombre} acumula {puntos} y quedará con {cli?.punto?.puntos_activos + puntos} puntos";
                    if (!bEscribiendoEnCliente)
                    {
                        txtBuscarCliente.Text = cli.nombre;
                        txtNombre.Text = cli.nombre;
                        txtTelefono.Text = cli.telefono;
                        txtDireccion.Text = cli.direccion;
                    }
                }
                catch
                {
                    lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";
                    cbClientes.SelectedIndex = -1;
                }
            };

            btnLimpiarCliente.Click += (se, a) =>
            {
                cbClientes.SelectedIndex = -1;
                txtBuscarCliente.Text = "";
                txtBuscarCliente.Focus();
            };

            lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";
            txtBuscarCliente.TextChanged += (se, a) =>
            {
                if (cbClientes.SelectedIndex != -1)
                {
                    lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";
                    cbClientes.ItemsSource = listaClientes;
                    cbClientes.DisplayMemberPath = "nombre";
                    cbClientes.SelectedIndex = -1;
                    cbClientes.Text = "";
                    txtNombre.Text = "";
                    txtTelefono.Text = "";
                    txtDireccion.Text = "";
                }

                bEscribiendoEnCliente = true;
                // cliente cli_test = listaClientes.Where(x => x.nombre.ToUpper() == txtBuscarCliente.Text.ToUpper()).FirstOrDefault();
                cliente cliPorRut = listaClientes.Where(x => x.rut == txtBuscarCliente.Text).FirstOrDefault();
                cliente cliPorTelefono = listaClientes.Where(x => x.telefono == txtBuscarCliente.Text).FirstOrDefault();
                List<cliente> listaClientesEncontrados = listaClientes.Where(x => x.rut.Contains(txtBuscarCliente.Text) || x.telefono.Contains(txtBuscarCliente.Text) || x.direccion.ToUpper().Contains(txtBuscarCliente.Text.ToUpper()) || x.nombre.ToUpper().Contains(txtBuscarCliente.Text.ToUpper())).ToList();

                if (listaClientesEncontrados != null)
                {
                    cbClientes.ItemsSource = listaClientesEncontrados;
                    cbClientes.DisplayMemberPath = "nombre";

                    if (listaClientesEncontrados.Count == 1)
                    {
                        //cliente cliEncontrado = listaClientesEncontrados.FirstOrDefault() as cliente;
                        //cbClientes.Text = cliEncontrado.nombre;
                        //lbClienteEncontrado.Content = $"{cliEncontrado.nombre} acumula {puntos} y queda con {cliEncontrado.punto.puntos_activos + puntos} puntos";

                    }
                    //else
                    //cbClientes.SelectedIndex = -1;
                }
                else // resetear
                {
                    lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";
                    cbClientes.ItemsSource = listaClientes;
                    cbClientes.DisplayMemberPath = "nombre";
                    cbClientes.SelectedIndex = -1;
                    cbClientes.Text = "";
                    txtNombre.Text = "";
                    txtTelefono.Text = "";
                    txtDireccion.Text = "";
                }
                if (cliPorRut != null)
                {
                    cbClientes.Text = cliPorRut.nombre;
                    txtNombre.Text = cliPorRut.nombre;
                    txtTelefono.Text = cliPorRut.telefono;
                    txtDireccion.Text = cliPorRut.direccion;
                }
                else if (cliPorTelefono != null)
                {
                    cbClientes.Text = cliPorTelefono.nombre;
                    txtNombre.Text = cliPorTelefono.nombre;
                    txtTelefono.Text = cliPorTelefono.telefono;
                    txtDireccion.Text = cliPorTelefono.direccion;
                }
                else
                {
                    /*
                    lbClienteEncontrado.Content = $"(Acumularía {puntos} puntos)";
                    cbClientes.ItemsSource = listaClientes;
                    cbClientes.DisplayMemberPath = "nombre";
                    cbClientes.SelectedIndex = -1;
                    */
                }
                bEscribiendoEnCliente = false;
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

                SalsaBLL.ObtenerTodas().ForEach(salsa => wrapSalsas.Children.Add(new ItemSalsa() { Salsa = salsa }));
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
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoOtro = 0;
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
                        Telefono = txtTelefono.Text,
                        Direccion = txtDireccion.Text,
                        MensajeDeliveryUno = txtMensajeDeliveryUno.Text,
                        MensajeDeliveryDos = txtMensajeDeliverDos.Text,
                        Incluye = incluyeStr,
                        IncluyeStrUnaLinea = incluyeStrUnaLinea,
                        PagaCon = pagaCon,
                        Vuelto = vuelto
                    };

                    string nombre = txtBuscarCliente.Text;
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

        private void CalcularMontos()
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

            if (MontoEfectivo + MontoTransBank + MontoJunaeb + MontoOtro == _montoTotal)
                btnAceptar.IsEnabled = true;
            else
                btnAceptar.IsEnabled = false;
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
