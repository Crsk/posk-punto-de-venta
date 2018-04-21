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

namespace posk.Controls
{
    public partial class ItemEscogerMesaSeccionVenta : UserControl
    {
        private List<mesa> mesas;

        public List<mesa> Mesas
        {
            get { return mesas; }
            set
            {
                mesas = value;
                cbMesas.ItemsSource = value;
                value?.Insert(0, new mesa() { codigo = "" });
                cbMesas.DisplayMemberPath = "codigo";
            }
        }


        public ItemEscogerMesaSeccionVenta()
        {
            InitializeComponent();
        }
    }
}
