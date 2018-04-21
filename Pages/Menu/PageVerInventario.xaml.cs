using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Popup;
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

namespace posk.Pages.Menu
{
    public partial class PageVerInventario : Page
    {
        private List<TextBox> listaTecladoNum;
        public PageVerInventario()
        {
            InitializeComponent();
            borderInventario.IsEnabled = false;
            btnEnviar.IsEnabled = false;
            listaTecladoNum = new List<TextBox>();

            Loaded += CargarItemsInventario;
            rbSale.Checked += HabilitarModificacionInventario;
            rbDevolver.Checked += HabilitarModificacionInventario;
            btnEnviar.Click += Enviar;
        }

        private void Enviar(object sender, RoutedEventArgs e)
        {
            try
            {
                bool bSaleStock = rbSale.IsChecked == true ? true : false;
                spInventarioItems.Children.OfType<ItemInventario>().ToList().ForEach(item =>
                {
                    if (!String.IsNullOrEmpty(item.txtCantidad.Text))
                        BodegaBLL.CrearActualizar(item.Producto.id, Convert.ToDecimal(item.txtCantidad.Text), bSaleStock);
                });
                new Notification("LISTO");

                dgInventario.DataContext = null;
                dgInventario.DataContext = ItemBodegaInventarioBLL.ObtenerInventario();
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL MODIFICAR INVENTARIO");
            }
            Limpiar();
        }

        private void Limpiar()
        {
            rbSale.IsChecked = false;
            rbDevolver.IsChecked = false;
            spInventarioItems.Children.OfType<ItemInventario>().ToList().ForEach(item => { item.txtCantidad.Clear(); });
            borderInventario.IsEnabled = false;
            btnEnviar.IsEnabled = false;
        }

        private void CargarItemsInventario(object sender, RoutedEventArgs e)
        {
            dgInventario.DataContext = null;
            dgInventario.DataContext = ItemBodegaInventarioBLL.ObtenerInventario();

            ProductoBLL.ObtenerTodo().ForEach(producto => 
            {
                var itemInventario = new ItemInventario() { Producto = producto };
                listaTecladoNum.Add(itemInventario.txtCantidad);
                spInventarioItems.Children.Add(itemInventario);
            });

            ItemTecladoNumerico tecladoNum = new ItemTecladoNumerico(listaTecladoNum);
            borderTecladoNumerico.Child = tecladoNum;
            tecladoNum.ExpTecladoNumerico.IsExpanded = true;
        }

        private void HabilitarModificacionInventario(object sender, RoutedEventArgs e)
        {
            borderInventario.IsEnabled = true;
            btnEnviar.IsEnabled = true;
        }
    }
}
