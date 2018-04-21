using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarPromociones : Page
    {
        private ItemTeclado teclado;
        private List<TextBox> listaItemsTeclado;
        private int ultimaPromoID;

        public PageAdministrarPromociones()
        {
            InitializeComponent();
            ultimaPromoID = 0;
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevaPromocion);
            listaItemsTeclado.Add(txtNuevaPromocionPrecio);


            teclado = new ItemTeclado(listaItemsTeclado);

            CargarPromociones();

            cbProductos.ItemsSource = ProductoBLL.ObtenerTodo();
            cbProductos.DisplayMemberPath = "nombre";

            cbProductos.ItemsSource = ProductoBLL.ObtenerTodo();
            cbProductos.DisplayMemberPath = "nombre";

            cbSubCategoria.ItemsSource = SubCategoriaBLL.ObtenerTodo();
            cbSubCategoria.DisplayMemberPath = "nombre";

            borderTeclado.Child = teclado;

            try
            {
                Loaded += (se1, a1) =>
                {
                    btnAgregarPromocion.Click += (se, a) => AgregarPromocion(txtNuevaPromocion.Text, Convert.ToInt32(txtNuevaPromocionPrecio.Text));
                    btnAgregarDetallePromo.Click += (se, a) => AgregarDetallePromo(cbProductos.SelectedItem as producto);
                };
            }
            catch
            {
                MessageBox.Show("Algo salió mal");
            }
        }

        private void CargarDetallePromocion(int promocionID)
        {
            spDetallePromocion.Children.Clear();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevaPromocion);
            listaItemsTeclado.Add(txtNuevaPromocionPrecio);

            // lista de productos por promocion
            ProductoPromocionBLL.ObtenerProductos(promocionID).ForEach(x =>
            {
                var ipd = new ItemPromoDetalle() { ID = x.id, Nombre = x.nombre, Precio = x.precio };

                spDetallePromocion.Children.Add(ipd);

                listaItemsTeclado.Add(ipd.txtNombre);

                ipd.btnEliminar.Click += (se, a) =>
                {
                    ProductoPromocionBLL.Eliminar(x.id);
                    CargarDetallePromocion(promocionID);
                };
            });
            borderTeclado.Child = new ItemTeclado(listaItemsTeclado);
        }

        private void CargarPromociones()
        {
            spPromociones.Children.Clear();
            listaItemsTeclado = new List<TextBox>();
            listaItemsTeclado.Add(txtNuevaPromocion);
            listaItemsTeclado.Add(txtNuevaPromocionPrecio);

            PromocionBLL.ObtenerTodas().ForEach(x =>
            {
                var ip = new ItemPromo() { ID = x.id, Nombre = x.nombre, Precio = x.precio, Imagen = x.imagen, SubCategoria = x.subcategoria, Favorito = x.favorito };

                ip.spItem.Children.Remove(ip.btnGuardar);
                ip.spItem.Children.Remove(ip.btnAgregar);
                spPromociones.Children.Add(ip);

                listaItemsTeclado.Add(ip.txtNombre);

                ip.btnEliminar.Click += (se, a) =>
                {
                    PromocionBLL.Eliminar(x.id);
                    CargarPromociones();
                };
                ip.btnEditar.Click += (se, a) =>
                {
                    ip.txtNombre.IsEnabled = true;
                    ip.txtPrecio.IsEnabled = true;
                    ip.cbSubCategoria.IsEnabled = true;
                    ip.checkFav.IsEnabled = true;
                    ip.spItem.Children.Clear();
                    ip.spItem.Children.Add(ip.btnEliminar);
                    ip.spItem.Children.Add(ip.borderItemCantidad);
                    ip.spItem.Children.Add(ip.borderItem);
                    ip.spItem.Children.Add(ip.borderSubCategoria);
                    ip.spItem.Children.Add(ip.borderCheckFav);

                    ip.spItem.Children.Add(ip.btnGuardar);
                };
                ip.btnGuardar.Click += (se, a) =>
                {
                    try
                    {
                        bool fav = ip.checkFav.IsChecked == true ? true : false;
                        PromocionBLL.Actualizar(x.id, ip.txtNombre.Text, Convert.ToInt32(ip.txtPrecio.Text), (ip.cbSubCategoria.SelectedItem as subcategoria).id, fav);
                        ip.txtNombre.IsEnabled = false;
                        ip.txtPrecio.IsEnabled = false;
                        ip.cbSubCategoria.IsEnabled = false;
                        ip.checkFav.IsEnabled = false;
                        ip.spItem.Children.Remove(ip.btnGuardar);
                        ip.spItem.Children.Add(ip.btnEditar);
                        ip.spItem.Children.Add(ip.btnVer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error: " + ex);
                    }
                };
                ip.btnVer.Click += (se, a) =>
                {
                    try
                    {
                        ultimaPromoID = ip.ID;
                        CargarDetallePromocion(ultimaPromoID);
                        lbDetallePromo.Content = $"{ip.Nombre.ToUpper()}";
                        try
                        {
                            imagenPromo.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + ip.Imagen));
                        }
                        catch (Exception)
                        {
                            imagenPromo.Source = null;
                        }
                    }
                    catch
                    {
                        spDetallePromocion.Children.Clear();
                    }
                };
            });
            borderTeclado.Child = new ItemTeclado(listaItemsTeclado);
        }

        private void AgregarDetallePromo(producto produ)
        {
            if (ultimaPromoID != 0)
            {
                producto_promocion pp = ProductoPromocionBLL.Crear(produ?.id, ultimaPromoID);

                CargarDetallePromocion(ultimaPromoID);
                teclado.expTeclado.IsExpanded = false;
            }
        }

        private void AgregarPromocion(string nombre, int precio)
        {
            bool fav = checkFav.IsChecked == true ? true : false;

            PromocionBLL.Crear(nombre, precio, (cbSubCategoria.SelectedItem as subcategoria).id, fav);
            CargarPromociones();
            txtNuevaPromocion.Clear();
            teclado.expTeclado.IsExpanded = false;

        }
    }
}
