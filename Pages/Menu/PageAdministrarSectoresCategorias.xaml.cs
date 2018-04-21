using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarSectoresCategorias : Page
    {
        private ItemTeclado teclado;
        private List<TextBox> listaItemsTeclado;

        public PageAdministrarSectoresCategorias()
        {
            InitializeComponent();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevoSector);
            listaItemsTeclado.Add(txtNuevaCategoria);
            listaItemsTeclado.Add(txtNuevaSubcategoria);

            // evento click agregar sector o categoría
            try
            {
                btnAgregarSector.Click += (se, a) => AgregarSector(txtNuevoSector.Text);
                btnAgregarCategoria.Click += (se, a) => AgregarCategoria(txtNuevaCategoria.Text);
                btnAgregarSubCategoria.Click += (se, a) => AgregarSubcategoria(txtNuevaSubcategoria.Text);
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al agregar sector o categoía");
            }

            teclado = new ItemTeclado(listaItemsTeclado);

            CargarSectores();
            CargarCategorias();
            CargarSubcategorias();

            borderTeclado.Child = teclado;
        }

        private void CargarSectores()
        {
            spSectores.Children.Clear();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevoSector);
            listaItemsTeclado.Add(txtNuevaCategoria);
            listaItemsTeclado.Add(txtNuevaSubcategoria);

            SectorBLL.ObtenerTodo().ForEach(x =>
            {
                var ic = new ItemRelacion() { Nombre = x.nombre };
                ic.spItem.Children.Clear();
                ic.spItem.Children.Add(ic.btnEliminar);
                ic.spItem.Children.Add(ic.borderItem);
                ic.spItem.Children.Add(ic.btnEditar);
                spSectores.Children.Add(ic);

                listaItemsTeclado.Add(ic.txtNombre);

                ic.btnEliminar.Click += (se, a) =>
                {
                    SectorBLL.Eliminar(x.id);
                    CargarSectores();
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
                    SectorBLL.Actualizar(x.id, ic.txtNombre.Text);
                    ic.txtNombre.IsReadOnly = true;
                    ic.Nombre = ic.txtNombre.Text;
                    ic.spItem.Children.Remove(ic.btnGuardar);
                    ic.spItem.Children.Add(ic.btnEditar);
                };
            });
            borderTeclado.Child = new ItemTeclado(listaItemsTeclado);
        }
        private void AgregarSector(string nombre)
        {
            SectorBLL.Crear(nombre);
            CargarSectores();
            txtNuevoSector.Clear();
            teclado.expTeclado.IsExpanded = false;
        }

        private void CargarCategorias()
        {
            spCategorias.Children.Clear();
            listaItemsTeclado = new List<TextBox>();

            CategoriaBLL.ObtenerTodo().ForEach(x =>
            {
                var ic = new ItemRelacion() { Nombre = x.nombre };
                ic.spItem.Children.Clear();
                ic.spItem.Children.Add(ic.btnEliminar);
                ic.spItem.Children.Add(ic.borderItem);
                ic.spItem.Children.Add(ic.btnEditar);

                spCategorias.Children.Add(ic);
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
                };
            });
            borderTeclado.Child = new ItemTeclado(listaItemsTeclado);
        }
        private void AgregarCategoria(string nombre)
        {
            CategoriaBLL.Crear(nombre);
            CargarCategorias();
            txtNuevaCategoria.Clear();
            teclado.expTeclado.IsExpanded = false;
        }

        private void CargarSubcategorias()
        {
            spSubCategorias.Children.Clear();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevoSector);
            listaItemsTeclado.Add(txtNuevaCategoria);
            listaItemsTeclado.Add(txtNuevaSubcategoria);

            SubCategoriaBLL.ObtenerTodo().ForEach(x =>
            {
                var ic = new ItemRelacion() { Nombre = x.nombre };
                ic.spItem.Children.Clear();
                ic.spItem.Children.Add(ic.btnEliminar);
                ic.spItem.Children.Add(ic.borderItem);
                ic.spItem.Children.Add(ic.btnEditar);

                spSubCategorias.Children.Add(ic);

                listaItemsTeclado.Add(ic.txtNombre);

                ic.btnEliminar.Click += (se, a) =>
                {
                    SubCategoriaBLL.Eliminar(x.id);
                    CargarSubcategorias();
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
                    SubCategoriaBLL.Actualizar(x.id, ic.txtNombre.Text);
                    ic.txtNombre.IsReadOnly = true;
                    ic.Nombre = ic.txtNombre.Text;
                    ic.spItem.Children.Remove(ic.btnGuardar);
                    ic.spItem.Children.Add(ic.btnEditar);
                };
            });
            borderTeclado.Child = new ItemTeclado(listaItemsTeclado);
        }
        private void AgregarSubcategoria(string nombre)
        {
            SubCategoriaBLL.Crear(nombre);
            CargarSubcategorias();
            txtNuevaSubcategoria.Clear();
            teclado.expTeclado.IsExpanded = false;
        }

    }
}
