using posk.BLL;
using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemLineaMateriaPrima : UserControl
    {
        public materiasprima MateriaPrima { get; set; }

        public ItemLineaMateriaPrima()
        {
            InitializeComponent();
            Loaded += (se, a) => 
            {
                txtItem.Text = MateriaPrima?.nombre;
                cbUnidadMedida.ItemsSource = UnidadMedidaVentaBLL.ObtenerTodo();
                cbUnidadMedida.DisplayMemberPath = "nombre";
                cbUnidadMedida.Text = MateriaPrima?.unidades_medida?.nombre;
            };
        }
    }
}
