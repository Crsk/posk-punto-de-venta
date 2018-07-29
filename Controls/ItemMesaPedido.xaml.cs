using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemMesaPedido : UserControl
    {
        public mesa Mesa { get; set; }
        public event EventHandler AlClickear;
        public string verde = "#FF12A466";
        public string rojo = "#c0392b";

        public ItemMesaPedido()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                Colorear();
                btnMesa.Click += (se2, a2) =>
                {
                    AlClickear?.Invoke(this, null);
                };
            };
        }

        public void Colorear()
        {
            var bc = new BrushConverter();
            lbMesa.Content = Mesa.codigo;
            if (Mesa?.libre == true)
            {
                // lbEstado.Content = "Libre";
                // btnMesa.Background = new SolidColorBrush(Color.FromRgb(5, 91, 37));
                btnMesa.Background = (Brush)bc.ConvertFrom(verde);
            }
            else
            {
                // lbEstado.Content = "Ocupada";
                // btnMesa.Background = new SolidColorBrush(Color.FromRgb(91, 5, 5));
                btnMesa.Background = (Brush)bc.ConvertFrom(rojo);
            }
        }
    }
}
