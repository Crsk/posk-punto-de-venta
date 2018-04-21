using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using posk.Popup;
using System;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarRelaciones : Page
    {
        private sectore sectorSeleccionado;
        private categoria categoriaSeleccionada;

        public PageAdministrarRelaciones()
        {
            InitializeComponent();

            Loaded += (se, a) => Reestablecer();

            // evento click agregar relaciones
            try
            {
                btnAgregarCategoriaSector.Click += (se, a) =>
                {
                    categoria cat = cbCategoriasSeccionCategorias.SelectedItem as categoria;
                    if (sectorSeleccionado != null)
                        CategoriaSectorBLL.Crear(cat.id, sectorSeleccionado.id);
                    else
                    {
                        new Notification("SELECCIONA UN SECTOR", "Antes de ingresar una categoría", Notification.Type.Warning, 10);
                        return;
                    }
                    CargarCategoriasSector(sectorSeleccionado);
                };
                btnAgregarCategoriaSubcategoria.Click += (se, a) =>
                {
                    subcategoria scat = cbSubcategoriasSeccionSubcategorias.SelectedItem as subcategoria;
                    if (categoriaSeleccionada != null)
                        CategoriaSubcategoriaBLL.Crear(categoriaSeleccionada.id, scat.id);
                    else
                    {
                        new Notification("SELECCIONA UNA CATEGORÍA", "Antes de ingresar una subcategoría", Notification.Type.Warning, 10);
                        return;
                    }
                    CargarSubcategoriasCategoria(categoriaSeleccionada);
                };
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al agregar relación");
            }
        }

        #region Cargar relaciones

        private void CargarSectores()
        {
            cbCategoriasSeccionCategorias.IsEnabled = false;
            btnAgregarCategoriaSector.IsEnabled = false;
            lbSectorSeleccionado.Content = "";

            cbSubcategoriasSeccionSubcategorias.IsEnabled = false;
            btnAgregarCategoriaSubcategoria.IsEnabled = false;
            lbCategoriaSeleccionada.Content = "";

            spSectores.Children.Clear();
            SectorBLL.ObtenerTodo().ForEach(sec =>
            {
                var irs = new ItemRelacion() { Nombre = sec.nombre, Eliminable = false, Sector = sec, Seleccionable = sec.seleccionable };
                irs.spItem.Children.Remove(irs.btnEliminar);
                irs.btnVer.Click += (se_sec, a_sec) => CargarCategoriasSector(sec);
                irs.AlCambiarEstadoSeleccionable += (se, a) =>
                {
                    if (irs.Seleccionable == true)
                    {
                        cbSubcategoriasSeccionSubcategorias.IsEnabled = false;
                        btnAgregarCategoriaSubcategoria.IsEnabled = false;
                        lbCategoriaSeleccionada.Content = "";
                        spSubCategorias.Children.Clear();
                    }
                };
                spSectores.Children.Add(irs);
            });

        }

        private void CargarCategoriasSector(sectore sec)
        {
            cbCategoriasSeccionCategorias.IsEnabled = true;
            btnAgregarCategoriaSector.IsEnabled = true;
            lbSectorSeleccionado.Content = $"{sec.nombre}";

            cbSubcategoriasSeccionSubcategorias.IsEnabled = false;
            btnAgregarCategoriaSubcategoria.IsEnabled = false;
            lbCategoriaSeleccionada.Content = "";

            sectorSeleccionado = sec;
            spCategorias.Children.Clear();
            spSubCategorias.Children.Clear();
            CategoriaSectorBLL.ObtenerCategorias(sec.id).ForEach(cat =>
            {
                var irc = new ItemRelacion() { Nombre = cat.nombre, Eliminable = true, Categoria = cat };
                irc.btnVer.Click += (se_cat, a_cat) => CargarSubcategoriasCategoria(cat);
                irc.btnEliminar.Click += (se_cat, a_cat) => EliminarCategoriaSector(cat.id);
                irc.AlCambiarEstadoSeleccionable += (se, a) =>
                {
                    if (irc.Seleccionable == true)
                    {
                        cbCategoriasSeccionCategorias.IsEnabled = false;
                        //cbSubcategoriasSeccionSubcategorias.IsEnabled = false;
                        btnAgregarCategoriaSector.IsEnabled = false;
                        //btnAgregarCategoriaSubcategoria.IsEnabled = false;
                        lbSectorSeleccionado.Content = "";
                        //lbCategoriaSeleccionada.Content = "";
                        spCategorias.Children.Clear();
                        //spSubCategorias.Children.Clear();
                    }
                };
                spCategorias.Children.Add(irc);
            });
        }

        private void CargarSubcategoriasCategoria(categoria cat)
        {
            cbSubcategoriasSeccionSubcategorias.IsEnabled = true;
            btnAgregarCategoriaSubcategoria.IsEnabled = true;
            lbCategoriaSeleccionada.Content = $"{sectorSeleccionado.nombre} -> {cat.nombre}";

            categoriaSeleccionada = cat;
            spSubCategorias.Children.Clear();
            CategoriaSubcategoriaBLL.ObtenerSubcategorias(cat.id).ForEach(scat =>
            {
                var irsc = new ItemRelacion() { Nombre = scat.nombre, Eliminable = true, Seleccionable = true };
                irsc.spItem.Children.Remove(irsc.btnSeleccionable);
                irsc.btnEliminar.Click += (se_scat, a_scat) => EliminarSubcategoriaCategoria(scat.id);
                spSubCategorias.Children.Add(irsc);
            });
        }

        #endregion

        #region Eliminar relación
        private void EliminarCategoriaSector(int catID)
        {
            CategoriaSectorBLL.Eliminar(catID);
            CargarCategoriasSector(sectorSeleccionado);
        }

        private void EliminarSubcategoriaCategoria(int scatID)
        {
            CategoriaSubcategoriaBLL.Eliminar(scatID);
            CargarSubcategoriasCategoria(categoriaSeleccionada);
        }
        #endregion

        private void Reestablecer()
        {
            spCategorias.Children.Clear();
            spSubCategorias.Children.Clear();

            cbCategoriasSeccionCategorias.ItemsSource = CategoriaBLL.ObtenerTodo();
            cbCategoriasSeccionCategorias.DisplayMemberPath = "nombre";
            cbCategoriasSeccionCategorias.SelectedItem = -1;

            cbSubcategoriasSeccionSubcategorias.ItemsSource = SubCategoriaBLL.ObtenerTodo();
            cbSubcategoriasSeccionSubcategorias.DisplayMemberPath = "nombre";
            cbSubcategoriasSeccionSubcategorias.SelectedItem = -1;

            lbSectorSeleccionado.Content = "";
            lbCategoriaSeleccionada.Content = "";

            CargarSectores();
        }
    }
}
