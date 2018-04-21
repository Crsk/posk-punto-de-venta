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

namespace posk.Globals
{
    public partial class VentaDetailLineControl : UserControl
    {
        public string Desc
        {
            get { return lbDesc.Content.ToString(); }
            set { lbDesc.Content = value; }
        }
        public int Valor
        {
            get { return Convert.ToInt32(lbValor.Content); }
            set { lbValor.Content = value; }
        }

        public VentaDetailLineControl()
        {
            InitializeComponent();
        }
    }
}
