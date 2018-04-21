using posk.BLL;
using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemPromo : UserControl
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int? Precio { get; set; }
        public string Imagen { get; set; }
        public subcategoria SubCategoria { get; set; }
        public bool? Favorito { get; set; }

        public ItemPromo()
        {
            InitializeComponent();
            txtNombre.IsEnabled = false;
            txtPrecio.IsEnabled = false;
            cbSubCategoria.IsEnabled = false;
            checkFav.IsEnabled = false;

            Loaded += (se, a) =>
            {
                txtNombre.Text = Nombre.ToUpper();
                txtPrecio.Text = $"{Precio}";
                cbSubCategoria.ItemsSource = SubCategoriaBLL.ObtenerTodo();
                cbSubCategoria.DisplayMemberPath = "nombre";
                cbSubCategoria.Text = SubCategoria?.nombre;
                checkFav.IsChecked = Favorito == true ? true : false;
            };
        }
    }
}
