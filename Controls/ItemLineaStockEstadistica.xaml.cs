using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemLineaStockEstadistica : UserControl
    {
        public string Item { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string Ajuste { get; set; }

        public ItemLineaStockEstadistica()
        {
            InitializeComponent();
            Loaded += (se, a) => 
            {
                lbItem.Content = Item;
                lbEntrada.Content = Entrada;
                lbSalida.Content = Salida;
                lbAjuste.Content = Ajuste;
            };
        }
    }
}
