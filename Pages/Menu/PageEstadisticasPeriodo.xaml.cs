using posk.BLL;
using posk.Controls;
using posk.Globals;
using System;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageEstadisticasPeriodo : Page
    {
        public PageEstadisticasPeriodo()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                cbJornadas.ItemsSource = JornadaBLL.ObtenerTodas();
                cbJornadas.DisplayMemberPath = "NombreMostrar";
                cbJornadas.SelectedIndex = -1;
            };

            cbJornadas.SelectionChanged += (se, a) =>
            {
                try
                {
                    expDetalle.IsExpanded = false;
                    if (cbJornadas.SelectedIndex != -1)
                    {
                        JornadaDetalle jd = (JornadaDetalle)cbJornadas.SelectedItem;
                        DateTime? fechaInicio = jd.FechaInicio;
                        DateTime? fechaCierre = jd.FechaCierre;

                        int? devolucion = DevolucionBLL.ObtenerValor(fechaInicio, fechaCierre);
                        int? merma = MermaBLL.ObtenerValor(fechaInicio, fechaCierre);
                        int? gasto = GastoBLL.ObtenerValor(fechaInicio, fechaCierre);
                        int? venta = BoletaBLL.ObtenerIngresosTotal(fechaInicio, fechaCierre);

                        tbDevolucionValor.Text = $"{devolucion}";
                        tbDevolucionUnidades.Text = $"{DevolucionBLL.ObtenerCantidadItemsDevueltos(fechaInicio, fechaCierre)}";

                        tbMermaValor.Text = $"{merma}";
                        tbMermaUnidades.Text = $"{MermaBLL.ObtenerCantidadItemsMermados(fechaInicio, fechaCierre)}";

                        tbGastoValor.Text = $"{gasto}";
                        tbGastoUnidades.Text = $"{GastoBLL.ObtenerCantidadItemsGasto(fechaInicio, fechaCierre)}";

                        tbVentaValor.Text = $"{venta}";
                        tbVentasUnidades.Text = $"{BoletaBLL.ObtenerCantidadItemsVendidos(fechaInicio, fechaCierre)}";

                        tbResultado.Text = $"{venta - devolucion - merma - gasto}";
                        tbCaja.Text = $"{venta - devolucion - gasto}";
                    }
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL CARGAR JORNADA");
                }
            };

            btnCerrarDetalle.Click += (se, a) =>
            {
                expDetalle.IsExpanded = false;
            };

            btnVerDetalleVenta.Click += (se, a) =>
            {
                try
                {
                    if (cbJornadas.SelectedIndex != -1)
                    {
                        expDetalle.IsExpanded = true;
                        lbDetalle.Content = "Detalle venta";
                        JornadaDetalle jd = (JornadaDetalle)cbJornadas.SelectedItem;
                        DateTime? fechaInicio = jd.FechaInicio;
                        DateTime? fechaCierre = jd.FechaCierre;
                        spDetalle.Children.Clear();
                        BoletaBLL.ObtenerUltimasBoletasPorPeriodo(fechaInicio, fechaCierre).ForEach(boleta =>
                        {
                            ItemBoletaFactura ibf = new ItemBoletaFactura() { NumeroBoleta = boleta.numero_boleta, Total = boleta.total, Cliente = boleta.usuario.nombre, Fecha = boleta.fecha };
                            DetalleBoletaBLL.ObtenerPorBoletaId(boleta.id).ForEach(detalle => 
                            {
                                ibf.spDetalle.Children.Add(new Label() { Content = $"{detalle.producto?.precio*detalle.cantidad}{detalle.promocione?.precio*detalle.cantidad} [{detalle.producto?.nombre}{detalle.promocione?.nombre}] x{detalle.cantidad}" });
                            });
                            spDetalle.Children.Add(ibf);
                        });
                    }
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL VER DETALLE VENTA");
                }
            };

            btnVerDetalleDevolucion.Click += (se, a) =>
            {
                try
                {
                    expDetalle.IsExpanded = true;
                    lbDetalle.Content = "Detalle devolución";
                    JornadaDetalle jd = (JornadaDetalle)cbJornadas.SelectedItem;
                    DateTime? fechaInicio = jd.FechaInicio;
                    DateTime? fechaCierre = jd.FechaCierre;
                    spDetalle.Children.Clear();
                    DevolucionBLL.ObtenerPorPeriodo(fechaInicio, fechaCierre).ForEach(devolucion =>
                    {
                        spDetalle.Children.Add(new Label() { Content = $"{devolucion.producto.nombre} x{devolucion.cantidad}" });
                    });
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL VER DETALLE DEVOLUCIÓN");
                }
            };

            btnVerDetalleMerma.Click += (se, a) =>
            {
                try
                {
                    expDetalle.IsExpanded = true;
                    lbDetalle.Content = "Detalle merma";
                    JornadaDetalle jd = (JornadaDetalle)cbJornadas.SelectedItem;
                    DateTime? fechaInicio = jd.FechaInicio;
                    DateTime? fechaCierre = jd.FechaCierre;
                    spDetalle.Children.Clear();
                    MermaBLL.ObtenerPorPeriodo(fechaInicio, fechaCierre).ForEach(merma =>
                    {
                        spDetalle.Children.Add(new Label() { Content = $"{merma.producto.nombre} x{merma.cantidad}" });
                    });
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "ERROR AL VER DETALLE DEVOLUCIÓN");
                }
            };
        }
    }
}
