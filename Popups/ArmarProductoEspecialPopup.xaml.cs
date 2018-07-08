using posk.BLL;
using posk.Controls;
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
using System.Windows.Shapes;

namespace posk.Popups
{
    public partial class ArmarProductoEspecialPopup : Window
    {
        public bool bCerrado { get; set; }
        private opcionale _opcion { get; set; }
        public event EventHandler<ItemVenta> AlTerminarArmado;
        private int posicion { get; set; }
        private producto _producto { get; set; }

        public ArmarProductoEspecialPopup(producto p)
        {
            InitializeComponent();
            _producto = p;
            posicion = 1;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += (se, a) => { if (!bCerrado) Close(); };
            if (p.tipo_producto_id != null)
                CargarOpciones((int) p.tipo_producto_id);

            btnIngresar.Click += BtnIngresar_Click;

            btnAtras.Click += BtnAtras_Click;
        }

        private void BtnAtras_Click(object sender, RoutedEventArgs e)
        {
            if (posicion == 1)
            {
                bCerrado = true;
                Close();
            }
            else if (posicion == 2)
            {
                posicion = 1;
                expIngredientes.IsExpanded = false;
                expOpciones.IsExpanded = true;
                wrapIngredientes.Children.OfType<ItemIngrediente>().ToList().ForEach(ie => ie.Cantidad = 0);
            }
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            var listaIngredientesEscogidos = wrapIngredientes.Children.OfType<ItemIngrediente>().ToList().Where(ing => ing.Cantidad > 0).ToList();
            ItemVenta iv = new ItemVenta() { Producto = _producto, listaIngredientes = listaIngredientesEscogidos, Opcion = _opcion };
            AlTerminarArmado?.Invoke(this, iv); // el producto armado con sus ingredientes se integra a la sección de venta
            bCerrado = true;
            Close();
        }



        private void CargarOpciones(int tipoProductoId)
        {
            wrapOpciones.Children.Clear();
            TipoProductoOpcionBLL.ObtenerOpciones(tipoProductoId).ForEach(opcion =>
            {
                ItemOpcion io = new ItemOpcion() { Opcion = opcion };
                io.AlSeleccionar += (se, a) =>
                {
                    _opcion = opcion;
                    CargarIngredientes(_opcion.id);
                };
                wrapOpciones.Children.Add(io);
            });
            expOpciones.IsExpanded = true;
            posicion = 1;
        }

        private void CargarIngredientes(int opcionId)
        {
            wrapIngredientes.Children.Clear();
            OpcionIngredienteBLL.ObtenerIngredientes(opcionId).ForEach(ingrediente =>
            {
                ItemIngrediente ie = new ItemIngrediente() { Ingrediente = ingrediente };
                ie.AlCambiarEstado += (se, a) => HabilitarBotonIngresar();
                expOpciones.IsExpanded = false;
                expIngredientes.IsExpanded = true;
                wrapIngredientes.Children.Add(ie);
            });
            posicion = 2;
        }

        private void HabilitarBotonIngresar()
        {
            int contadorIngredientes = 0;
            wrapIngredientes.Children.OfType<ItemIngrediente>().ToList().ForEach(ie => contadorIngredientes += ie.Cantidad);
            if (contadorIngredientes > 0)
                btnIngresar.IsEnabled = true;
            else
                btnIngresar.IsEnabled = false;
        }
    }
}
