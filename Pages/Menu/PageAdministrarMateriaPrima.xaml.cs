using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using posk.Popup;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarMateriaPrima : Page
    {
        public PageAdministrarMateriaPrima()
        {
            InitializeComponent();
            CargarMateriasPrimas();

            cbUnidadMedida.ItemsSource = UnidadMedidaVentaBLL.ObtenerTodo();
            cbUnidadMedida.DisplayMemberPath = "nombre";

            btnAgregar.Click += (se, a) =>
            {
                materiasprima mp = MateriaPrimaBLL.Crear(txtNombre.Text, (cbUnidadMedida.SelectedItem as unidades_medida).id);
                CargarMateriasPrimas();
            };
        }

        private void CargarMateriasPrimas()
        {
            spMateriaPrima.Children.Clear();
            MateriaPrimaBLL.ObtenerTodo().ForEach(mp =>
            {
                var itemLineaMateriaPrima = new ItemLineaMateriaPrima() { MateriaPrima = mp };
                itemLineaMateriaPrima.btnEliminar.Click += (se, a) =>
                {
                    MateriaPrimaBLL.Eliminar(mp.id);
                    CargarMateriasPrimas();
                };
                // TODO - programar botones editar y guardar
                spMateriaPrima.Children.Add(itemLineaMateriaPrima);
            });
        }
    }
}
