using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace posk.Popup
{
    public partial class Notification : Window
    {
        public enum Type { Successful, Warning, Danger };
        public enum Size { Default, Sm, Md, Lg };
        public enum Position { Top, Center };
        private static SolidColorBrush colorVerde;
        private static SolidColorBrush colorAmarillo;
        private static SolidColorBrush colorRojo;
        DispatcherTimer dtClockTime;


        public Notification(string mensaje, string mensajeSecundario = "", Type type = Type.Successful, int duracion = 2, Size size = Size.Default, Position position = Position.Center )
        {
            bool alwaysShow = false;
            colorVerde = new SolidColorBrush(Color.FromRgb(5, 91, 37));
            colorAmarillo = new SolidColorBrush(Color.FromRgb(201, 127, 0));
            colorRojo = new SolidColorBrush(Color.FromRgb(168, 33, 0));
            InitializeComponent();

            btnCerrar.Click += (se, a) => Cerrar();


            switch (position)
            {
                case Position.Top:
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    break;
                case Position.Center:
                    WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
                default:
                    WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
            }
            //WindowStartupLocation = WindowStartupLocation.Manual;

            lbMensaje.Text = mensaje.ToUpper();
            tbMensajeSecundario.Text = mensajeSecundario;

            Opacity = 0;
            Topmost = true;

            switch (size)
            {
                case Size.Sm:
                    lbMensaje.FontSize = 16;
                    break;
                case Size.Md:
                    lbMensaje.FontSize = 26;
                    break;
                case Size.Lg:
                    lbMensaje.FontSize = 36;
                    break;
                default:
                    lbMensaje.FontSize = 26;
                    break;
            }

            switch (type)
            {
                case Type.Successful:
                    borderFondo.Background = colorVerde;
                    break;
                case Type.Warning:
                    borderFondo.Background = colorAmarillo;
                    break;
                case Type.Danger:
                    alwaysShow = true;
                    borderFondo.Background = colorRojo;
                    break;
                default:
                    borderFondo.Background = colorVerde;
                    break;
            }

            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            BeginAnimation(OpacityProperty, fadeIn);

            dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            int i = 0;
            dtClockTime.Tick += (se, a) =>
            {
                i++;
                if (i >= duracion)
                {
                    if (!alwaysShow)
                    {
                        DoubleAnimation fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                        BeginAnimation(OpacityProperty, fadeOut);
                        if (i >= duracion + 0.1)
                            Cerrar();
                    }
                }
            };
            dtClockTime.Start();
            Show();
        }

        private void Cerrar()
        {
            dtClockTime.Stop();
            Close();
        }
    }
}
