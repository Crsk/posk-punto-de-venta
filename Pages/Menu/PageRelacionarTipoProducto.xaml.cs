using posk.BLL;
using posk.Controls;
using posk.Models;
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
    public partial class PageRelacionarTipoProducto : Page
    {
        private int _tipoProductoId;
        private int _opcionId;

        public PageRelacionarTipoProducto()
        {
            InitializeComponent();
            Loaded += PageRelacionarTipoProducto_Loaded;
            btnAgregarOpcion.Click += BtnAgregarOpcion_Click;
            btnAgregarIngrediente.Click += BtnAgregarIngrediente_Click;
            btnAgregarIngredienteTodos.Click += BtnAgregarIngredienteTodos_Click;
            toggleMostrarOpciones.Click += ToggleMostrarOpciones_Click;
            CargarTipoProducto();
        }

        private void ToggleMostrarOpciones_Click(object sender, RoutedEventArgs e)
        {
            if (_tipoProductoId == 0)
            {
                new Notification("Acción requerida", "Selecciona un tipo de producto", Notification.Type.Warning, 6);
                return;
            }
            TipoProductoBLL.MostrarOpciones(_tipoProductoId, toggleMostrarOpciones.IsChecked == true ? true : false);
            new Notification("Listo");
        }

        private void BtnAgregarIngredienteTodos_Click(object sender, RoutedEventArgs e)
        {
            if (_opcionId == 0)
            {
                new Notification("Acción requerida", "Selecciona una opción", Notification.Type.Warning, 6);
                return;
            }
            IngredientesBLL.ObtenerTodo().ForEach(ing =>
            {
                OpcionIngredienteBLL.Ingresar(_opcionId, ing.id);
            });
            new Notification("Ingresados");
            MostrarIngredientesRelacionados(_opcionId);
        }

        private void BtnAgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            if (_opcionId == 0)
            {
                new Notification("", "Opción requerida", Notification.Type.Warning);
                return;
            }

            // relacionar ingrediente con opción seleccionada
            if (cbIngredientes.SelectedIndex == -1)
                new Notification("No es posible", "Selecciona un ingrediente", Notification.Type.Warning);
            else
            {
                OpcionIngredienteBLL.Ingresar(_opcionId, (cbIngredientes.SelectedItem as ingrediente).id);
                new Notification("Ingresado");
                MostrarIngredientesRelacionados(_opcionId);
            }
        }

        private void BtnAgregarOpcion_Click(object sender, RoutedEventArgs e)
        {
            if (_tipoProductoId == 0)
            {
                new Notification("No es posible", "Escoge un tipo de producto", Notification.Type.Warning);
                return;
            }

            // relacionar opción con tipo de producto seleccionado
            if (cbOpciones.SelectedIndex == -1)
                new Notification("Selecciona una opción", "", Notification.Type.Warning);
            else
            {
                TipoProductoOpcionBLL.Ingresar(_tipoProductoId, (cbOpciones.SelectedItem as opcionale).id);
                new Notification("Ingresado");
                MostrarOpcionesRelacionadas(_tipoProductoId);
            }
        }


        private void PageRelacionarTipoProducto_Loaded(object sender, RoutedEventArgs e)
        {
            cbOpciones.ItemsSource = OpcionesBLL.ObtenerTodo();
            cbOpciones.DisplayMemberPath = "nombre";

            cbIngredientes.ItemsSource = IngredientesBLL.ObtenerTodo();
            cbIngredientes.DisplayMemberPath = "nombre";

            CargarTipoProducto();
            spOpciones.Children.Clear();
            spIngredientes.Children.Clear();
        }

        private void CargarTipoProducto()
        {
            spTipoProducto.Children.Clear();
            TipoProductoBLL.ObtenerTodo().ForEach(tp =>
            {
                var itemRelacion = new ItemRelacionProducto() { Id = tp.id, Nombre = tp.nombre };
                itemRelacion.QuitarBotonBorrar();
                itemRelacion.AlSeleccionar += (se, a) =>
                {
                    Seleccionar("TIPO_PRODUCTO", itemRelacion);
                    MostrarOpcionesRelacionadas(tp.id);
                };
                spTipoProducto.Children.Add(itemRelacion);
            });
        }

        private void MostrarOpcionesRelacionadas(int tipoProductoId)
        {
            spOpciones.Children.Clear();
            TipoProductoOpcionBLL.ObtenerOpciones(tipoProductoId).ForEach(opcion =>
            {
                var itemRelacion = new ItemRelacionProducto() { Id = opcion.id, Precio = opcion.precio, Nombre = opcion.nombre };
                itemRelacion.AlEliminar += (se, a) =>
                {
                    TipoProductoOpcionBLL.Eliminar(tipoProductoId, opcion.id);
                    new Notification("Borrado");
                    MostrarOpcionesRelacionadas(tipoProductoId);
                };

                itemRelacion.AlSeleccionar += (se, a) =>
                {
                    Seleccionar("OPCION", itemRelacion);
                    MostrarIngredientesRelacionados(opcion.id);
                };
                spOpciones.Children.Add(itemRelacion);
            });
        }

        private void MostrarIngredientesRelacionados(int opcionId)
        {
            spIngredientes.Children.Clear();
            OpcionIngredienteBLL.ObtenerIngredientes(opcionId).ForEach(ingrediente =>
            {
                var itemRelacion = new ItemRelacionProducto() { Id = ingrediente.id, Precio = ingrediente.precio, Nombre = ingrediente.nombre };
                itemRelacion.AlEliminar += (se, a) =>
                {
                    OpcionIngredienteBLL.Eliminar(opcionId, ingrediente.id);
                    new Notification("Borrado");
                    MostrarIngredientesRelacionados(opcionId);
                };
                spIngredientes.Children.Add(itemRelacion);
            });
        }

        // desseleccionar todos los items excepto la última selección
        private void Seleccionar(string tipo, ItemRelacionProducto itemRelacion)
        {
            switch (tipo)
            {
                case "TIPO_PRODUCTO":
                    spTipoProducto.Children.OfType<ItemRelacionProducto>().ToList().ForEach(ir => ir.SeleccionarToggle(false));
                    itemRelacion.SeleccionarToggle(true);
                    _tipoProductoId = itemRelacion.Id;
                    _opcionId = 0; // olvidar opción cuando cambiamos tipo de producto
                    spIngredientes.Children.Clear(); // ocultar ingredientes previamente visualizados
                    MostrarOpcionesRelacionadas(_tipoProductoId);
                    break;
                case "OPCION":
                    spOpciones.Children.OfType<ItemRelacionProducto>().ToList().ForEach(ir => ir.SeleccionarToggle(false));
                    itemRelacion.SeleccionarToggle(true);
                    _opcionId = itemRelacion.Id;
                    MostrarIngredientesRelacionados(_opcionId);
                    break;
                default:
                    break;
            }
        }
    }
}
