//using LiveCharts;
//using LiveCharts.Wpf;
using posk.BLL;
using posk.Globals;
using posk.Popup;
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
using System.Windows.Threading;

namespace posk.Pages.Menu
{
    public partial class PageEstadisticasPorFecha : Page
    {
        //public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (InternetChecker.IsConnectedToInternet())
                {
                    lbHora.Content = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
                }
                else
                {
                    lbHora.Content = $"{DateTime.Now}";
                }
            }
            catch (Exception)
            {
                //lbInfo.Content = "ERROR: " + err.Message;
            }
        }


        public PageEstadisticasPorFecha()
        {
            InitializeComponent();

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); // Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();

            //SeriesCollection = new SeriesCollection
            //{
            //    new ColumnSeries
            //    {
            //        Title = "2015",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 }
            //    }
            //};

            ////adding series will update and animate the chart automatically
            //SeriesCollection.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            //also adding values updates and animates the chart automatically
            //SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            DataContext = this;



            Loaded += (se, a) =>
            {
                try
                {
                    dpDesde.Text = $"{DateTime.Now.ToShortDateString()}";
                    tpDesde.Text = $"{DateTime.Now.Add((new TimeSpan(-8, 0, 0))).ToShortTimeString()}";

                    dpHasta.Text = $"{DateTime.Now.ToShortDateString()}";
                    tpHasta.Text = $"{DateTime.Now.Add(new TimeSpan(0, 0, 1)).ToShortTimeString()}";
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al iniciar estadisticas por periodo");
                }
            };

            btnVer.Click += (se, a) =>
            {
                try
                {
                    DateTime desde = Convert.ToDateTime($"{dpDesde.Text} {tpDesde.Text}");
                    DateTime hasta = Convert.ToDateTime($"{dpHasta.Text} {tpHasta.Text}");
                    lbResultado.Content = $"Ingresos periodo: ${BoletaBLL.ObtenerIngresosTotal(desde, hasta)}";
                }
                catch
                {
                    new Notification(">:(", "Intenta denuevo", Notification.Type.Warning);
                }
            };

            btnGraficos.Click += (se, a) =>
            {
                //try
                //{
                //    SeriesCollection = new SeriesCollection();

                //    BoletaBLL.

                //    SeriesCollection.Add(new ColumnSeries
                //    {
                //        Title = "2016",
                //        Values = new ChartValues<double> { 11, 56, 42 }
                //    });
                //}
                //catch (Exception ex)
                //{
                //    PoskException.Make(ex, "Error al generar el gráfico");
                //}
            };
        }
    }
}
