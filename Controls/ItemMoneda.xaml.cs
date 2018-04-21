using System;
using System.Configuration;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace posk.Controls
{
    public partial class ItemMoneda : UserControl
    {
        private EventHandler alSumarCantidad;
        public event EventHandler AlSumarCantidad
        {
            add { if (alSumarCantidad == null) alSumarCantidad += value; }
            remove { alSumarCantidad -= value; }
        }

        private EventHandler alRestarCantidad;
        public event EventHandler AlRestarCantidad
        {
            add { if (alRestarCantidad == null) alRestarCantidad += value; }
            remove { alRestarCantidad -= value; }
        }

        public int Monto { get; set; }
        private SolidColorBrush colorVerde;
        private SolidColorBrush colorBlanco;
        public int Valor { get; set; }
        public bool bOverlay { get; set; }
        public bool Seleccionado { get; set; }

        public ItemMoneda()
        {
            InitializeComponent();
            colorVerde = new SolidColorBrush(Color.FromRgb(6, 112, 17));
            colorBlanco = new SolidColorBrush(Color.FromRgb(250, 250, 250));
            Loaded += (se, a) => lbValor.Content = $"{Valor}";

            Loaded += (se, a) =>
            {
                MostrarValor();
            };

            btnMoneda.Click += (se, a) =>
            {
                bOverlay ^= true;
                Seleccionado = bOverlay;
                MostrarOverlay(bOverlay);
                if (bOverlay)
                    alSumarCantidad?.Invoke(this, null);
                else
                    alRestarCantidad?.Invoke(this, null);
            };
        }

        private void MostrarValor()
        {
            switch (Monto)
            {
                case 20000:
                    lbValor.Content = "20.000";
                    break;
                case 10000:
                    lbValor.Content = "10.000";
                    break;
                case 5000:
                    lbValor.Content = "5.000";
                    break;
                case 4000:
                    lbValor.Content = "4.000";
                    break;
                case 3000:
                    lbValor.Content = "3.000";
                    break;
                case 2000:
                    lbValor.Content = "2.000";
                    break;
                case 1000:
                    lbValor.Content = "1.000";
                    break;
                case 900:
                    lbValor.Content = "900";
                    break;
                case 800:
                    lbValor.Content = "800";
                    break;
                case 700:
                    lbValor.Content = "700";
                    break;
                case 600:
                    lbValor.Content = "600";
                    break;
                case 500:
                    lbValor.Content = "500";
                    break;
                case 400:
                    lbValor.Content = "400";
                    break;
                case 300:
                    lbValor.Content = "300";
                    break;
                case 200:
                    lbValor.Content = "200";
                    break;
                case 100:
                    lbValor.Content = "100";
                    break;
                case 90:
                    lbValor.Content = "90";
                    break;
                case 80:
                    lbValor.Content = "80";
                    break;
                case 70:
                    lbValor.Content = "70";
                    break;
                case 60:
                    lbValor.Content = "60";
                    break;
                case 50:
                    lbValor.Content = "50";
                    break;
                case 40:
                    lbValor.Content = "40";
                    break;
                case 30:
                    lbValor.Content = "30";
                    break;
                case 20:
                    lbValor.Content = "20";
                    break;
                case 10:
                    lbValor.Content = "10";
                    break;
                default:
                    lbValor.Content = "error";
                    break;
            }
        }

        public void Reiniciar()
        {
            Seleccionado = false;
            bOverlay = false;
            MostrarOverlay(false);
        }

        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                overlay.Visibility = System.Windows.Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                overlay.Visibility = System.Windows.Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
            }
        }
    }
}
