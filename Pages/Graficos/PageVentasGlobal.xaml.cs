using posk.BLL;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Pages.Graficos
{
    /// <summary>
    /// Interaction logic for PageVentasVistaGlobal.xaml
    /// </summary>
    public partial class PageVentasGlobal : Page, IDisposable
    {
        public int IngresoRangoBajo { get; set; }
        public int IngresoRangoMedioBajo { get; set; }
        public int IngresoRangoMedioAlto { get; set; }
        public int IngresoRangoAlto { get; set; }

        public int MayorIngreso { get; set; }

        public SolidColorBrush ColorNeutro { get; set; }
        public SolidColorBrush ColorRangoBajo { get; set; }
        public SolidColorBrush ColorRangoMedioBajo { get; set; }
        public SolidColorBrush ColorRangoMedioAlto { get; set; }
        public SolidColorBrush ColorRangoAlto { get; set; }
        public SolidColorBrush ColorMayorIngreso { get; set; }

        public SolidColorBrush ColorSeleccion { get; set; }

        bool bRellenoCreado;

        public DateTime InicioSeleccionado { get; set; }
        public DateTime FinSeleccionado { get; set; }

        List<Ingreso> ingresos;

        struct Ingreso
        {
            public DateTime Inicio { get; set; }
            public DateTime Fin { get; set; }
            public int Monto { get; set; }
        }

        public PageVentasGlobal()
        {
            InitializeComponent();

            //btnDetalle.Focusable = false;

            IngresoRangoBajo = 0;
            IngresoRangoAlto = 0;

            ColorNeutro = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            ColorRangoBajo = new SolidColorBrush(Color.FromRgb(140, 250, 140));
            ColorRangoMedioBajo = new SolidColorBrush(Color.FromRgb(70, 240, 70));
            ColorRangoMedioAlto = new SolidColorBrush(Color.FromRgb(0, 140, 0));
            ColorRangoAlto = new SolidColorBrush(Color.FromRgb(0, 90, 0));
            ColorMayorIngreso = new SolidColorBrush(Color.FromRgb(255, 128, 0));

            ColorSeleccion = new SolidColorBrush(Color.FromRgb(10, 30, 100));

            btnAyuda.Click += (se, ev) => { expAyuda.IsExpanded ^= true; };
            btnCompararIngresos.Click += (se, ev) => { expCompararIngresos.IsExpanded ^= true; };

            //btnDetalle.Click += (se, ev) => { expDetalle.IsExpanded ^= true; GenerarDetalle(); };

            GenerarGrafico();

            dpIngresos.SelectedDateChanged += (se, ev) =>
            {
                //Ingreso item = ingresos.Where(x => x.Inicio == ((DatePicker)se).SelectedDate.Value).FirstOrDefault();

                int monto = BoletaBLL.ObtenerIngresosTotal(dpIngresos.SelectedDate.Value, dpIngresos.SelectedDate.Value.AddDays(1));
                DateTime inicio = dpIngresos.SelectedDate.Value;
                DateTime fin = inicio.AddDays(1); // por mientras

                lbInfoIngreso.Content = $"${monto}";
                lbInfo.Content = $"De {inicio.ToLongDateString()} a las {inicio.ToShortTimeString()} hrs.\nA {fin.ToLongDateString()} a las {fin.ToShortTimeString()} hrs.";
            };

            lbRangoBajo.Content = $"Entre $0 y ${IngresoRangoBajo}";
            lbRangoMedioBajo.Content = $"Entre ${IngresoRangoBajo} y ${IngresoRangoMedioBajo}";
            lbRangoMedioAlto.Content = $"Entre ${IngresoRangoMedioBajo} y ${IngresoRangoMedioAlto}";
            lbRangoAlto.Content = $"Entre {IngresoRangoMedioAlto} y ${IngresoRangoAlto}";
            lbMayorIngreso.Content = $"Mayor ingreso ${MayorIngreso}";
        }
        public void GenerarGrafico()
        {
            wrapIngresos.Children.Clear();
            ingresos = new List<Ingreso>();

            for (int i = -369; i <= 1; i++)
            {
                Ingreso ing = new Ingreso();
                if (DatosNegocioBLL.JornadeDeUnDia())
                {
                    ing.Inicio = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraInicioJornada());
                    ing.Fin = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraTerminoJornada());
                }
                else
                {
                    ing.Inicio = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraInicioJornada());
                    ing.Fin = DateTime.Now.AddDays(i + 1).Date.Add(DatosNegocioBLL.GetHoraTerminoJornada());
                }
                ing.Monto = BoletaBLL.ObtenerIngresosTotal(ing.Inicio, ing.Fin);

                if (ing.Monto > MayorIngreso) MayorIngreso = ing.Monto;

                ingresos.Add(ing);
            }

            foreach (Ingreso item in ingresos)
            {
                IngresoRangoBajo = MayorIngreso / 4;
                IngresoRangoMedioBajo = IngresoRangoBajo * 2;
                IngresoRangoMedioAlto = IngresoRangoBajo * 3;
                IngresoRangoAlto = IngresoRangoBajo * 4;

                if (item.Monto > 0 && item.Monto < IngresoRangoBajo)
                    IngresarItemAlGrafico(item, ColorRangoBajo);
                else if (item.Monto >= IngresoRangoBajo && item.Monto < IngresoRangoMedioBajo)
                    IngresarItemAlGrafico(item, ColorRangoMedioBajo);
                else if (item.Monto >= IngresoRangoMedioBajo && item.Monto < IngresoRangoMedioAlto)
                    IngresarItemAlGrafico(item, ColorRangoMedioAlto);
                else if (item.Monto >= IngresoRangoMedioAlto && item.Monto < IngresoRangoAlto)
                    IngresarItemAlGrafico(item, ColorRangoAlto);
                else if (item.Monto == MayorIngreso && item.Monto != 0)
                    IngresarItemAlGrafico(item, ColorMayorIngreso);
                else
                    IngresarItemAlGrafico(item, ColorNeutro);
            }
        }

        private void IngresarItemAlGrafico(Ingreso item, SolidColorBrush color)
        {
            Style style = Application.Current.FindResource("BtnClear") as Style;
            Brush colorAntesSeleccion = null;

            int primerDia = (int)item.Inicio.DayOfWeek;

            if (!bRellenoCreado)
            {
                for (int i = 1; i < primerDia; i++)
                {
                    Border btnRelleno = new Border() { Height = 25, Width = 8, Margin = new Thickness(1), Background = null };
                    wrapIngresos.Children.Add(btnRelleno);
                }
                bRellenoCreado = true;
            }
            Button btn = new Button() { Height = 25, Width = 8, Margin = new Thickness(1), Background = color, Style = style, ToolTip = $"{item.Inicio.ToShortDateString()} ${item.Monto}" };
            btn.Click += (se, ev) =>
            {
                dpIngresos.SelectedDate = item.Inicio;
                lbInfoIngreso.Content = $"${item.Monto}";
                lbInfo.Content = $"De {item.Inicio.ToLongDateString()} a las {item.Inicio.ToShortTimeString()} hrs.\nA {item.Fin.ToLongDateString()} a las {item.Fin.ToShortTimeString()} hrs.";
            };
            btn.GotFocus += (se, ev) =>
            {
                //expDetalle.IsExpanded = false;
                //spDetalle.Children.Clear();
                lbCompararIngresos.Content = ObtenerDiasSimilares(item.Inicio);
                colorAntesSeleccion = btn.Background;
                btn.Background = ColorSeleccion;
                InicioSeleccionado = item.Inicio;
                FinSeleccionado = item.Fin;
            };
            btn.LostFocus += (se, ev) =>
            {
                btn.Background = colorAntesSeleccion;
                //lbInfo.Content = "";
                //lbInfoIngreso.Content = "";
                //btnDetalle.Click += (se2, ev2) => { };
            };
            wrapIngresos.Children.Add(btn);
        }

        private void GenerarDetalle()
        {
            List<boleta> boletas = BoletaBLL.ObtenerUltimasBoletasPorPeriodo(InicioSeleccionado, FinSeleccionado);
            foreach (boleta b in boletas)
            {
                //ItemBoletaFactura ibf = new ItemBoletaFactura() { Fecha = b.fecha, Total = b.total, NumeroBoleta = b.numero_boleta, Cliente = b.cliente };
                //spDetalle.Children.Add(ibf);
            }
        }

        private string ObtenerDiasSimilares(DateTime diaEscogido)
        {
            DateTime diaSemanaSimilar = new DateTime(), diaMesSimilar = new DateTime(), diaAnoSimilar = new DateTime();
            diaSemanaSimilar = diaEscogido.AddDays(-7);
            diaMesSimilar = diaEscogido.AddDays(-28);
            diaAnoSimilar = diaEscogido.AddDays(-364);

            return $"${BoletaBLL.ObtenerIngresosTotal(diaSemanaSimilar, diaSemanaSimilar.AddDays(1))}, hace una semana ({diaSemanaSimilar.ToLongDateString()})\n${BoletaBLL.ObtenerIngresosTotal(diaMesSimilar, diaMesSimilar.AddDays(1))}, hace un mes ({diaMesSimilar.ToLongDateString()})\n${BoletaBLL.ObtenerIngresosTotal(diaAnoSimilar, diaAnoSimilar.AddDays(1))}, hace un año ({diaAnoSimilar.ToLongDateString()})";
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
