using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace posk.Controls
{
    public partial class ItemMedioPagoCalcular : UserControl
    {
        public medio_pago MedioPago { get; set; }
        public bool bUsado { get; set; }

        public ItemMedioPagoCalcular()
        {
            InitializeComponent();



            AddHandler(PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent,
                new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent,
                new RoutedEventHandler(SelectAllText), true);



            Loaded += (se, a) =>
            {
                btnMedioPago.Content = $"{MedioPago.nombre}";
            };

            btnMedioPago.Click += (se, a) =>
            {
                if (bUsado)
                {
                    NoUsadoEstilo();
                    txtMonto.Text = "";
                }
            };

            txtMonto.TextChanged += (se, a) =>
            {
                if (txtMonto.Text == "")
                    bUsado = false;
                else
                    bUsado = true;
                EstablecerUso();
            };
        }

        private void EstablecerUso()
        {
            if (bUsado)
                UsadoEstilo();
            else
                NoUsadoEstilo();
        }

        private void ValidarNumero(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void UsadoEstilo()
        {
            var bc = new BrushConverter();
            btnMedioPago.Background = (Brush)bc.ConvertFrom("#FF0A7562");
            btnMedioPago.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff"));
            bUsado = true;
        }

        public void NoUsadoEstilo()
        {
            btnMedioPago.Background = new SolidColorBrush(Color.FromRgb(225, 225, 225));
            btnMedioPago.Foreground = new SolidColorBrush(Color.FromRgb(124, 124, 124));
            bUsado = false;
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        public void Limpiar()
        {
            NoUsadoEstilo();
            txtMonto.Clear();
        }
    }
}
