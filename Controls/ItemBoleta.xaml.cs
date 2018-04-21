using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemBoleta : UserControl
    {
        private string total;

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; lbId.Content = $"Boleta ID: {value}"; }
        }

        public string Total
        {
            get { return total; }
            set { total = value; lbTotal.Content = $"Total: ${value}"; }
        }

        private string fecha;

        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; lbFecha.Content = value; }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; lbUsuario.Content = value; }
        }

        public ItemBoleta()
        {
            InitializeComponent();
        }
    }
}
