using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemDcto : UserControl
    {
        private int dctoPesos;
        public int DctoPesos
        {
            get { return dctoPesos; }
            set { dctoPesos = value; lbDescuento.Content = $"Descuento: ${value}"; }
        }

        private int dctoPct;
        public int DctoPct
        {
            get { return dctoPct; }
            set { dctoPct = value; }
        }


        public ItemDcto()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            DctoPesos = 0;
            DctoPct = 0;
            lbDescuento.Content = "";
        }
    }
}
