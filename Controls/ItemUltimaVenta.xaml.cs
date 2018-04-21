using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemUltimaVenta : UserControl
    {
        public boleta Boleta { get; set; }

        private string mensaje;
        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; lbUltimaVenta.Content = value; }
        }


        public ItemUltimaVenta()
        {
            InitializeComponent();

        }
    }
}
