using posk.BLL;
using posk.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemEscogerMesa : UserControl
    {
        private List<mesa> mesa;

        public List<mesa> Mesa
        {
            get { return mesa; }
            set
            {
                mesa = value;
                cbMesas.ItemsSource = value;
                cbMesas.DisplayMemberPath = "codigo";
            }
        }

        public ItemEscogerMesa()
        {
            InitializeComponent();
        }
    }
}
