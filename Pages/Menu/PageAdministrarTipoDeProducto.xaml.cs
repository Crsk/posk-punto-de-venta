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
    public partial class PageAdministrarTipoDeProducto : Page
    {
        public PageAdministrarTipoDeProducto()
        {
            InitializeComponent();
            Loaded += PageAdministrarTipoDeProducto_Loaded;
            btnAgregarTipoDeProducto.Click += BtnAgregarTipoDeProducto_Click;
            btnAgregarOpcion.Click += BtnAgregarOpcion_Click;
            btnAgregarIngrediente.Click += BtnAgregarIngrediente_Click;
        }

        #region agregar click

        private void BtnAgregarTipoDeProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crear("TIPO_PRODUCTO", txtTipoProductoNombre.Text, Convert.ToInt32(txtTipoProductoLimiteIngr.Text));
                CargarTipoDeProducto();
            }
            catch (Exception ex)
            {
                // envía un correo del problema al programador
                PoskException.Make(ex, "Error al ingresar tipo de producto");
            }
        }
        private void BtnAgregarOpcion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crear("OPCION", txtOpcionesNombre.Text, Convert.ToInt32(txtOpcionesPrecio.Text));
                CargarOpciones();
            }
            catch (Exception ex)
            {
                // envía un correo del problema al programador
                PoskException.Make(ex, "Error al ingresar tipo de producto");
            }
        }
        private void BtnAgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Crear("INGREDIENTE", txtIngredientesNombre.Text, Convert.ToInt32(txtIngredientesPrecio.Text));
                CargarIngredientes();
            }
            catch (Exception ex)
            {
                // envía un correo del problema al programador
                PoskException.Make(ex, "Error al ingresar tipo de producto");
            }
        }
        #endregion agregar click

        #region cargar
        private void CargarTipoDeProducto()
        {
            spTipoProducto.Children.Clear();
            TipoProductoBLL.ObtenerTodo().ForEach(tp =>
            {
                var ie = new ItemEditableTipoProducto() { Id = tp.id, Tipo = "TIPO_PRODUCTO", Nombre = tp.nombre, LimiteIngr = tp.limite_ingr };
                ie.AlEliminar += (se, a) => EliminarTipoProducto(ie);
                ie.AlEditar += (se, a) => ie.MostrarBotonGuardar();
                ie.AlGuardar += (se, a) => ActualizarTipoProducto(ie);
                spTipoProducto.Children.Add(ie);
            });
        }

        private void CargarOpciones()
        {
            spOpciones.Children.Clear();
            OpcionesBLL.ObtenerTodo().ForEach(tp =>
            {
                var ie = new ItemEditable() { Id = tp.id, Tipo = "OPCION", Nombre = tp.nombre, Precio = tp.precio };
                ie.AlEliminar += (se, a) => Eliminar(ie);
                ie.AlEditar += (se, a) => ie.MostrarBotonGuardar();
                ie.AlGuardar += (se, a) => Actualizar(ie);
                spOpciones.Children.Add(ie);
            });
        }

        private void CargarIngredientes()
        {
            spIngredientes.Children.Clear();
            IngredientesBLL.ObtenerTodo().ForEach(tp =>
            {
                var ie = new ItemEditable() { Id = tp.id, Tipo = "INGREDIENTE", Nombre = tp.nombre, Precio = tp.precio };
                ie.AlEliminar += (se, a) => Eliminar(ie);
                ie.AlEditar += (se, a) => ie.MostrarBotonGuardar();
                ie.AlGuardar += (se, a) => Actualizar(ie);
                spIngredientes.Children.Add(ie);
            });
        }
        #endregion cargar

        #region CRUD
        private void Crear(string tipo, string nombre, int valor)
        {
            switch (tipo)
            {
                case "TIPO_PRODUCTO":
                    TipoProductoBLL.Ingresar(nombre, valor);
                    new Notification("Ingresado");
                    LimpiarCampos();
                    break;
                case "OPCION":
                    OpcionesBLL.Ingresar(nombre, valor);
                    new Notification("Ingresado");
                    LimpiarCampos();
                    break;
                case "INGREDIENTE":
                    IngredientesBLL.Ingresar(nombre, valor);
                    new Notification("Ingresado");
                    LimpiarCampos();
                    break;
                default:
                    break;
            }
        }
        private void EliminarTipoProducto(ItemEditableTipoProducto ie)
        {
            try
            {
                TipoProductoBLL.Eliminar(ie.Id);
                CargarTipoDeProducto();
                new Notification("Borrado");
            }
            catch (Exception ex)
            {
                // envía un correo del problema al programador
                PoskException.Make(ex, "Error al Borrar tipo producto");
            }
        }
        private void Eliminar(ItemEditable ie)
        {
            switch (ie.Tipo)
            {
                case "OPCION":
                    try
                    {
                        OpcionesBLL.Eliminar(ie.Id);
                        CargarOpciones();
                        new Notification("Borrado");
                    }
                    catch (Exception ex)
                    {
                        // envía un correo del problema al programador
                        PoskException.Make(ex, "Error al borrar opción");
                    }
                    break;
                case "INGREDIENTE":
                    try
                    {
                        IngredientesBLL.Eliminar(ie.Id);
                        CargarIngredientes();
                        new Notification("Borrado");
                    }
                    catch (Exception ex)
                    {
                        // envía un correo del problema al programador
                        PoskException.Make(ex, "Error al borrar ingrediente");
                    }
                    break;
                default:
                    break;
            }
        }
        private void ActualizarTipoProducto(ItemEditableTipoProducto ie)
        {
            try
            {
                TipoProductoBLL.Actualizar(ie.Id, ie.txtNombre.Text, Convert.ToInt32(ie.txtLimiteIngr.Text));
                ie.MostrarBotonEditar();
                new Notification("Actualizado");
            }
            catch (Exception ex)
            {
                // envía un correo del problema al programador
                PoskException.Make(ex, "Error al actualizar tipo producto");
            }
        }
        private void Actualizar(ItemEditable ie)
        {
            switch (ie.Tipo)
            {
                case "OPCION":
                    try
                    {
                        OpcionesBLL.Actualizar(ie.Id, ie.txtNombre.Text, Convert.ToInt32(ie.txtPrecio.Text));
                        ie.MostrarBotonEditar();
                        new Notification("Actualizado");
                    }
                    catch (Exception ex)
                    {
                        // envía un correo del problema al programador
                        PoskException.Make(ex, "Error al actualizar opción");
                    }
                    break;
                case "INGREDIENTE":
                    try
                    {
                        IngredientesBLL.Actualizar(ie.Id, ie.txtNombre.Text, Convert.ToInt32(ie.txtPrecio.Text));
                        ie.MostrarBotonEditar();
                        new Notification("Actualizado");
                    }
                    catch (Exception ex)
                    {
                        // envía un correo del problema al programador
                        PoskException.Make(ex, "Error al actualizar opción");
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion CRUD

        private void LimpiarCampos()
        {
            txtTipoProductoNombre.Clear();
            txtTipoProductoLimiteIngr.Clear();
            txtOpcionesNombre.Clear();
            txtOpcionesPrecio.Clear();
            txtIngredientesNombre.Clear();
            txtIngredientesPrecio.Clear();
        }

        private void PageAdministrarTipoDeProducto_Loaded(object sender, RoutedEventArgs e)
        {
            CargarTipoDeProducto();
            CargarOpciones();
            CargarIngredientes();
        }
    }
}
