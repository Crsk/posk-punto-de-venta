using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemPromoDetalle : UserControl
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int? Precio { get; set; }

        public ItemPromoDetalle()
        {
            InitializeComponent();
            txtNombre.IsReadOnly = true;
            txtPrecio.IsReadOnly = true;
            Loaded += (se, a) => 
            {
                txtPrecio.Text = $"{Precio}";
                txtNombre.Text = $"{Nombre.ToUpper()}";
            };
        }
    }
}
