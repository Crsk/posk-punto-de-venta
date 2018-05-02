using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace posk.Popups
{
    public partial class InfoVentaPopup : UserControl
    {
        public string UsuarioIniciadorJornadaAnterior { get; set; }
        public string UsuarioIniciadorJornadaActual { get; set; }

        private DateTime inicioJornadaAnterior;
        public DateTime InicioJornadaAnterior
        {
            get { return inicioJornadaAnterior; }
            set { inicioJornadaAnterior = value; lbInfoInicioJornadaAnterior.Content = $"{ value.ToShortDateString() }  { value.ToShortTimeString() } ({ UsuarioIniciadorJornadaAnterior })"; }
        }

        private DateTime inicioJornadaActual;
        public DateTime InicioJornadaActual
        {
            get { return inicioJornadaActual; }
            set { inicioJornadaActual = value; lbInfoInicioJornadaActual.Content = $"{ value.ToShortDateString() }  { value.ToShortTimeString() } ({ UsuarioIniciadorJornadaActual })"; }
        }

        public int Mermas { get; set; }
        public int Devoluciones { get; set; }
        public int Gastos { get; set; }
        public int CajaInicial { get; set; }
        public int VentasEfectivo { get; set; }
        public int VentasTransBank { get; set; }
        public int VentasJunaeb { get; set; }
        public int VentasOtro { get; set; }
        public int Propinas { get; set; }

        public int Caja { get; set; }

        private void RecalcularCaja()
        {
            lbInfoVentas.Content = $"{ -Devoluciones - Gastos + VentasEfectivo + VentasTransBank + VentasJunaeb + VentasOtro - Propinas }";
            lbInfoVentas.ToolTip = "-Devoluciones - Gastos + VentasEfectivo + VentasTransBank + VentasJunaeb + VentasOtro - Propinas";

            lbInfoCaja.Content = $"{ -Devoluciones - Gastos + CajaInicial + VentasEfectivo - Propinas }";
            lbInfoCaja.ToolTip = "-Devoluciones - Gastos + CajaInicial + VentasEfectivo - Propinas";
        }

        public InfoVentaPopup()
        {
            InitializeComponent();

            ItemTecladoNumerico teclado = new ItemTecladoNumerico(new List<TextBox>() { txtCajaInicial });
            borderTeclado.Child = teclado;

            Loaded += (se, a) =>
            {
                if (GlobalSettings.RadioButtonJornadaAnteriorSeleccionado)
                    rbInicioJornadaAnterior.IsChecked = true;
                else if (GlobalSettings.RadioButtonJornadaActualSeleccionado)
                    rbInicioJornadaActual.IsChecked = true;
            };

            rbInicioJornadaAnterior.Checked += (se, a) =>
            {
                GlobalSettings.RadioButtonJornadaAnteriorSeleccionado = true;
                GlobalSettings.RadioButtonJornadaActualSeleccionado = false;
                CalcularMontos();
            };
            rbInicioJornadaActual.Checked += (se, a) =>
            {
                GlobalSettings.RadioButtonJornadaAnteriorSeleccionado = false;
                GlobalSettings.RadioButtonJornadaActualSeleccionado = true;
                CalcularMontos();
            };

            txtCajaInicial.GotMouseCapture += (se, a) => { expDetalle.IsExpanded = false; };

            btnVerGastos.Click += (se, a) =>
            {
                expDetalle.IsExpanded = true;
                spDetalle.Children.Clear();
                if (rbInicioJornadaActual.IsChecked == true)
                {
                    spDetalle.Children.Add(new Label { Content = $"Gastos desde {inicioJornadaActual}", FontSize = 18 });

                    GastoBLL.ObtenerGastosPeriodo(inicioJornadaActual, DateTime.Now).ForEach(gasto =>
                    {
                        spDetalle.Children.Add(new Label { Content = $"{gasto.fecha}        ${gasto.monto}        {gasto.detalle}", FontSize = 18 });
                    });
                }
                else if (rbInicioJornadaAnterior.IsChecked == true)
                {
                    spDetalle.Children.Add(new Label { Content = $"Gastos desde {inicioJornadaAnterior}", FontSize = 18 });
                    GastoBLL.ObtenerGastosPeriodo(inicioJornadaAnterior, DateTime.Now).ForEach(gasto =>
                    {
                        spDetalle.Children.Add(new Label { Content = $"{gasto.fecha}        ${gasto.monto}        {gasto.detalle}", FontSize = 18 });
                    });
                }
            };

            btnImprimirGastos.Click += (se, a) =>
            {
                CrearTicket ticket = new CrearTicket();
                if (rbInicioJornadaActual.IsChecked == true)
                {
                    ticket.TextoCentro($"GASTOS DESDE {inicioJornadaActual}".ToUpper());
                    ticket.TextoIzquierda($"");
                    GastoBLL.ObtenerGastosPeriodo(inicioJornadaActual, DateTime.Now).ForEach(gasto =>
                    {
                        ticket.TextoCentro($"{gasto.fecha}");
                        ticket.TextoExtremos(gasto.detalle, gasto.monto.ToString());
                        ticket.TextoIzquierda($"");
                    });
                }
                else if (rbInicioJornadaAnterior.IsChecked == true)
                {
                    ticket.TextoCentro($"GASTOS DESDE {inicioJornadaAnterior}".ToUpper());
                    ticket.TextoIzquierda("");
                    GastoBLL.ObtenerGastosPeriodo(inicioJornadaAnterior, DateTime.Now).ForEach(gasto =>
                    {
                        ticket.TextoCentro($"{gasto.fecha}");
                        ticket.TextoExtremos(gasto.detalle, gasto.monto.ToString());
                        ticket.TextoIzquierda($"");
                    });
                }
                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings.ImpresoraBar, "Gastos");
            };

            btnGuardarCajainicial.Click += (se, a) =>
            {
                try
                {
                    var caja = Convert.ToInt32(txtCajaInicial.Text);
                    JornadaBLL.EstablecerCajaInicial(caja);
                    new Notification("LISTO");
                    teclado.ExpTecladoNumerico.IsExpanded = false;
                    CajaInicial = caja;
                    RecalcularCaja();
                }
                catch
                {
                    new Notification(":(", "Intenta denuevo", Notification.Type.Danger, 3);
                }
            };
            /*
            btnVerVentas.Click += (se, a) =>
            {
                expDetalle.IsExpanded = true;
                spDetalle.Children.Clear();

                if (rbInicioJornadaActual.IsChecked == true)
                {
                    Label titulo = new Label() { Content = $"Ventas de la jornada ({BoletaBLL.CantidadBoletasPorPeriodo(inicioJornadaActual, DateTime.Now)}):", HorizontalAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 20 };
                    //titulo.MouseLeftButtonUp += (se2, a2) => expDetalle.IsExpanded = false;
                    spDetalle.Children.Add(titulo);
                    BoletaBLL.ObtenerUltimasBoletasPorPeriodo(inicioJornadaActual, DateTime.Now).ForEach(boleta =>
                    {
                        var ib = new ItemBoleta() { Id = boleta.id, Total = boleta.total.ToString(), Fecha = $"{boleta.fecha.ToShortDateString()} {boleta.fecha.ToLongTimeString()}", Usuario = boleta.usuario?.nombre };
                        ib.btnBoleta.Click += (se2, a2) =>
                        {
                            string detalleBoleta = $"Boleta ID: {boleta.id}    {boleta.fecha.ToShortDateString()} {boleta.fecha.ToLongTimeString()}    {boleta.usuario?.nombre} \n\n";
                            LineaDetalleBLL.Get(boleta.id).ForEach(detalle =>
                            {
                                if (detalle.promocione != null)
                                    detalleBoleta += $"${detalle.promocione?.precio * detalle.cantidad}    x{detalle.cantidad}    [{detalle.promocione?.nombre}]  (promoción)\n";
                                else
                                    detalleBoleta += $"${detalle.producto?.precio * detalle.cantidad}    x{detalle.cantidad}    [{detalle.producto?.nombre}]\n";
                            });
                            detalleBoleta += $"\nTotal: ${boleta.total}";
                            MessageBox.Show(detalleBoleta);
                        };
                        spDetalle.Children.Add(ib);
                    });
                }
                else if (rbInicioJornadaAnterior.IsChecked == true)
                {
                    Label titulo = new Label() { Content = $"Ventas de la jornada ({BoletaBLL.CantidadBoletasPorPeriodo(inicioJornadaAnterior, DateTime.Now)}):", HorizontalAlignment = System.Windows.HorizontalAlignment.Center, FontSize = 20 };
                    //titulo.MouseLeftButtonUp += (se2, a2) => expDetalle.IsExpanded = false;
                    spDetalle.Children.Add(titulo);
                    BoletaBLL.ObtenerUltimasBoletasPorPeriodo(inicioJornadaAnterior, DateTime.Now).ForEach(boleta =>
                    {
                        var ib = new ItemBoleta() { Id = boleta.id, Total = boleta.total.ToString(), Fecha = $"{boleta.fecha.ToShortDateString()} {boleta.fecha.ToLongTimeString()}", Usuario = boleta.usuario?.nombre };
                        ib.btnBoleta.Click += (se2, a2) =>
                        {
                            string detalleBoleta = $"Boleta ID: {boleta.id}    {boleta.fecha.ToShortDateString()} {boleta.fecha.ToLongTimeString()}    {boleta.usuario?.nombre} \n\n";
                            LineaDetalleBLL.Get(boleta.id).ForEach(detalle =>
                            {
                                if (detalle.promocione != null)
                                    detalleBoleta += $"${detalle.promocione?.precio * detalle.cantidad}    x{detalle.cantidad}    [{detalle.promocione?.nombre}]  (promoción)\n";
                                else
                                    detalleBoleta += $"${detalle.producto?.precio * detalle.cantidad}    x{detalle.cantidad}    [{detalle.producto?.nombre}]\n";
                            });
                            detalleBoleta += $"\nTotal: ${boleta.total}";
                            MessageBox.Show(detalleBoleta);
                        };
                        spDetalle.Children.Add(ib);
                    });
                }

            };*/
        }

        private void CalcularMontos()
        {
            try
            {
                Limpiar();

                if (rbInicioJornadaAnterior.IsChecked == true)
                {
                    Mermas = MermaBLL.ObtenerValor(InicioJornadaAnterior, DateTime.Now);
                    Devoluciones = DevolucionBLL.ObtenerValor(InicioJornadaAnterior, DateTime.Now);
                    Gastos = GastoBLL.ObtenerValor(InicioJornadaAnterior, DateTime.Now);
                    CajaInicial = JornadaBLL.ObtenerCajaInicialJornadaAnterior();

                    VentasEfectivo = BoletaBLL.ObtenerIngresosEfectivo(InicioJornadaAnterior, DateTime.Now);
                    VentasTransBank = BoletaBLL.ObtenerIngresosTransBank(InicioJornadaAnterior, DateTime.Now);
                    VentasJunaeb = BoletaBLL.ObtenerIngresosJunaeb(InicioJornadaAnterior, DateTime.Now);
                    VentasOtro = BoletaBLL.ObtenerIngresosOtro(InicioJornadaAnterior, DateTime.Now);
                    Propinas = BoletaBLL.ObtenerPropinas(InicioJornadaAnterior, DateTime.Now);

                    btnGuardarCajainicial.IsEnabled = false;
                    txtCajaInicial.IsEnabled = false;

                    lbInfoMermas.Content = Mermas;
                    lbInfoDevoluciones.Content = Devoluciones;
                    lbInfoGastos.Content = Gastos;
                    txtCajaInicial.Text = CajaInicial + "";
                    /*
                    lbInfoVentas.Content = $"{Ventas - Devoluciones}";
                    if (Devoluciones != 0)
                        lbInfoVentas2.Content = $" ({Ventas} - {Devoluciones})";
                    */
                    lbInfoVentasEfectivo.Content = $"{VentasEfectivo}";
                    lbInfoVentasJunaeb.Content = $"{VentasJunaeb}";
                    lbInfoVentasTransBank.Content = $"{VentasTransBank}";
                    lbInfoVentasOtro.Content = $"{VentasOtro}";
                    lbInfoPropinas.Content = $"{Propinas}";

                    RecalcularCaja();
                }
                else if (rbInicioJornadaActual.IsChecked == true)
                {
                    Mermas = MermaBLL.ObtenerValor(InicioJornadaActual, DateTime.Now);
                    Devoluciones = DevolucionBLL.ObtenerValor(InicioJornadaActual, DateTime.Now);
                    Gastos = GastoBLL.ObtenerValor(InicioJornadaActual, DateTime.Now);
                    CajaInicial = JornadaBLL.ObtenerCajaInicialJornadaActual();
                    VentasEfectivo = BoletaBLL.ObtenerIngresosEfectivo(InicioJornadaActual, DateTime.Now);
                    VentasTransBank = BoletaBLL.ObtenerIngresosTransBank(InicioJornadaActual, DateTime.Now);
                    VentasJunaeb = BoletaBLL.ObtenerIngresosJunaeb(InicioJornadaActual, DateTime.Now);
                    VentasOtro = BoletaBLL.ObtenerIngresosOtro(InicioJornadaActual, DateTime.Now);
                    Propinas = BoletaBLL.ObtenerPropinas(InicioJornadaActual, DateTime.Now);

                    btnGuardarCajainicial.IsEnabled = true;
                    txtCajaInicial.IsEnabled = true;

                    lbInfoMermas.Content = Mermas;
                    lbInfoDevoluciones.Content = Devoluciones;
                    lbInfoGastos.Content = Gastos;
                    txtCajaInicial.Text = CajaInicial + "";
                    /*
                    lbInfoVentas.Content = $"{Ventas - Devoluciones}";
                    if (Devoluciones != 0)
                        lbInfoVentas2.Content = $" ({Ventas} - {Devoluciones})";
                    */
                    lbInfoVentasEfectivo.Content = $"{VentasEfectivo}";
                    lbInfoVentasJunaeb.Content = $"{VentasJunaeb}";
                    lbInfoVentasTransBank.Content = $"{VentasTransBank}";
                    lbInfoVentasOtro.Content = $"{VentasOtro}";
                    lbInfoPropinas.Content = $"{Propinas}";
                    RecalcularCaja();
                }

                //lbInfoInicioJornadaAnterior.Content = $"{InicioJornadaAnterior.ToShortDateString()} {InicioJornadaAnterior.ToShortTimeString()}";
                //lbInfoInicioJornadaActual.Content = $"{InicioJornadaActual.ToShortDateString()} {InicioJornadaActual.ToShortTimeString()}";
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL CALCULAR INFORMACION DE JORNADA");
            }
        }

        private void Limpiar()
        {
            Mermas = 0;

            Devoluciones = 0;
            Gastos = 0;

            //CajaInicial = 0;
            VentasEfectivo = 0;
            VentasTransBank = 0;
            VentasJunaeb = 0;
            VentasOtro = 0;
            Propinas = 0;
            Caja = 0;

            lbInfoMermas.Content = "";

            lbInfoDevoluciones.Content = "";
            lbInfoGastos.Content = "";

            lbInfoVentasEfectivo.Content = "";
            lbInfoVentasTransBank.Content = "";
            lbInfoVentasJunaeb.Content = "";
            lbInfoVentasOtro.Content = "";
            lbInfoPropinas.Content = "";
            lbInfoVentas.Content = "";
        }
    }
}
