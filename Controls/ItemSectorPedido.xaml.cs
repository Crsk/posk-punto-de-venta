using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemSectorPedido : UserControl
    {
        private sectormesa sectorMesa;
        public sectormesa SectorMesa
        {
            get { return sectorMesa; }
            set { sectorMesa = value; lbSector.Content = value.nombre; }
        }

        public event EventHandler AlClickear;

        public ItemSectorPedido()
        {
            InitializeComponent();
            btnSector.Click += (se, a) => AlClickear?.Invoke(this, null);
        }
    }
}
