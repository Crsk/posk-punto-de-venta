using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemBuscarPorSector : UserControl
    {
        public sectore Sector { get; set; }

        public ItemBuscarPorSector()
        {
            InitializeComponent();

            Loaded += (se, a) => 
                expSector.Header = Sector?.nombre;
        }
    }
}
