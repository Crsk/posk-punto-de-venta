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
    /// <summary>
    /// Interaction logic for ItemPendienteControl.xaml
    /// </summary>
    public partial class ItemPendienteControl : UserControl
    {
        public ItemPendienteControl()
        {
            InitializeComponent();

            btnItem.Click += (se, ev) =>
            {
                if (btnItem.Background == null)
                    btnItem.Background = new SolidColorBrush(Color.FromRgb(34, 139, 34));
                else
                    btnItem.Background = null;
            };
        }
    }
}
