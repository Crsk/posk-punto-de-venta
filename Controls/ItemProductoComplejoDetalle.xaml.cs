using posk.BLL;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemProductoComplejoDetalle : UserControl
    {
        public producto Producto { get; set; }
        public materiasprima MateriaPrima { get; set; }
        public decimal Cantidad { get; set; }
        public unidades_medida UnidadMedida { get; set; }

        public ItemProductoComplejoDetalle()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                txtCantidad.Text = $"{Cantidad}";
                lbProducto.Content = $"{Producto?.nombre}";
                cbUnidadMedada.ItemsSource = UnidadMedidaVentaBLL.Obtener(MateriaPrima.id);
                cbUnidadMedada.SelectedIndex = 0;
            };
        }
    }
}
