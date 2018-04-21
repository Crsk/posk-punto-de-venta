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
using System.Windows.Shapes;

namespace posk.Pages.PopUp
{
    public partial class PopupGaugeOptions : Window
    {
        public PopupGaugeOptions(int ingresos12Horas)
        {
            InitializeComponent();
            btnListo.Click += (se, ev) =>
            {
                ingresos12Horas = Convert.ToInt32(txtIngresos.Text);
                Close();
            };
        }
    }
}
