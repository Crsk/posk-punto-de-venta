using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemAccion : UserControl
    {
        private string accion;

        public string Accion
        {
            get { return accion; }
            set { accion = value; btnAccion.Content = value.ToUpper(); }
        }

        public ItemAccion()
        {
            InitializeComponent();
        }
    }
}
