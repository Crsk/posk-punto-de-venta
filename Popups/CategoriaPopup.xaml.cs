using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Popup
{
    public partial class CategoriaPopup : UserControl
    {
        public event EventHandler<string> OnSelect;
        private ItemTeclado teclado;
        private List<TextBox> listaItemsTeclado;

        public CategoriaPopup()
        {
            InitializeComponent();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevaCategoria);
            btnAgregar.Click += (se, a) => AgregarCategoria(txtNuevaCategoria.Text);
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            spItems.Children.Clear();
            CategoriaBLL.ObtenerTodo().ForEach(x =>
            {
                var ic = new ItemRelacion();
                ic.ID = x.id;
                ic.Nombre = x.nombre;
                ic.txtNombre.Text = x.nombre;

                ic.spItem.Children.Remove(ic.btnGuardar);
                spItems.Children.Add(ic);

                listaItemsTeclado.Add(ic.txtNombre);

                ic.btnEliminar.Click += (se, a) =>
                {
                    CategoriaBLL.Eliminar(x.id);
                    CargarCategorias();
                };
                ic.btnEditar.Click += (se, a) =>
                {
                    ic.txtNombre.IsReadOnly = false;
                    ic.spItem.Children.Clear();
                    ic.spItem.Children.Add(ic.btnEliminar);
                    ic.spItem.Children.Add(ic.borderItem);
                    ic.spItem.Children.Add(ic.btnGuardar);
                };
                ic.btnGuardar.Click += (se, a) =>
                {
                    CategoriaBLL.Actualizar(x.id, ic.txtNombre.Text);
                    ic.txtNombre.IsReadOnly = true;
                    ic.Nombre = ic.txtNombre.Text;
                    ic.spItem.Children.Remove(ic.btnGuardar);
                    ic.spItem.Children.Add(ic.btnEditar);
                    ic.spItem.Children.Add(ic.btnAgregar);
                };
                ic.btnAgregar.Click += (se, a) =>
                {
                    OnSelect(this, ic.Nombre);
                };
            });
            teclado = new ItemTeclado(listaItemsTeclado);
            borderTeclado.Child = teclado;
        }

        private void AgregarCategoria(string nombre)
        {
            CategoriaBLL.Crear(nombre);
            CargarCategorias();
            txtNuevaCategoria.Clear();
            teclado.expTeclado.IsExpanded = false;
        }
    }
}
