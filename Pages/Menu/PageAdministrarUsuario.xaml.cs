using posk.Components;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;
using posk.Globals;
using System.Windows.Media;
using System.Collections.Generic;
using System.Data;
using posk.Pages.PopUp;
using posk.BLL;
using posk.Models;
using posk.Controls;
using System.Windows.Media.Animation;
using posk.Popup;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarUsuario : Page, IDisposable
    {
        // item nuevo producto
        StackPanel spNuevo;
        Button btnNuevo, btnEliminar;
        Label lbNuevo;
        //int productSelectedId = 0;

        int iva = Convert.ToInt32(ConfigurationManager.AppSettings["IVA"]);
        bool bEliminarCreado = false, bNuevoCreado = false;

        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                gridOverlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
            }
            else
            {
                gridOverlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                gridOverlay.BeginAnimation(OpacityProperty, animation);
            }
        }

        public PageAdministrarUsuario()
        {
            InitializeComponent();
            // borderTeclado.Child = new ItemTeclado(new List<TextBox>() { txtBuscar, txtNombre, txtNombreUsuario, txtPass });
            gridOverlay.MouseLeftButtonDown += (se, e) => MostrarOverlay(false);
            btnBuscaImagen.Click += (se, e) =>
            {
                try
                {
                    expPopup.IsExpanded = true;
                    var wi = new WindowChooseOrTakeImage(ConfigurationManager.AppSettings["RutaImagenUsuario"], imgUsuario, txtNombre.Text);
                    wi.Show();
                }
                catch (Exception)
                {
                }
                //wi.OnFinish += (se2, a) => MostrarOverlay(false);

                // Frame framePopup = new Frame();
                // var windowFoto = new WindowChooseOrTakeImage(ConfigurationManager.AppSettings["RutaImagenUsuario"], imgUsuario, txtNombreUsuario.Text);
                // HostWindowInFrame(framePopup, windowFoto);
                // expPopup.Content = framePopup;
            };

            btnNuevo = new Button()
            {
                Name = "btnNuevoUsuario",
                Content = "+",
                Height = 120,
                Width = 160,
                Margin = new Thickness(4, 4, 4, 0),
                Background = new SolidColorBrush(Color.FromRgb(214, 214, 214)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(214, 214, 214))
            };

            btnEliminar = new Button()
            {
                Name = "btnEliminar",
                Content = "ELIMINAR",
                Width = 120,
                Margin = new Thickness(8),
                Background = new SolidColorBrush(Color.FromRgb(2, 2, 2)),
                BorderBrush = null,
                Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240))
            };
            btnEliminar.Click += (se, ev) =>
            {
                try
                {
                    UsuarioBLL.Borrar(imgUsuario._id);
                    MessageBox.Show("Usuario borrado");
                }
                catch (Exception)
                {
                    MessageBox.Show("No se pudo borrar", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                RefreshScreen();
            };

            lbNuevo = new Label()
            {
                Content = "NUEVO",
                Width = 100,
                Height = 24,
                Margin = new Thickness(4, 0, 4, 0),
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            spNuevo = new StackPanel();
            spNuevo.Children.Add(btnNuevo);
            spNuevo.Children.Add(lbNuevo);
            btnNuevo.Click += (se2, ev2) =>
            {
                dpUsuario.IsEnabled = true;
                btnEditarCrear.Content = "CREAR";
                RefreshScreen();
                try
                {
                    imgUsuario.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenUsuario"] + "default.jpg"));
                    imgUsuario._name = "default.jpg";
                }
                catch (Exception)
                {
                    imgUsuario.Source = null;
                }
            };

            if (!bNuevoCreado)
            {
                wrapItems.Children.Add(spNuevo);
                bNuevoCreado = true;
            }
        }

        public void HostWindowInFrame(Frame fraContainer, Window win)
        {
            object tmp = win.Content;
            win.Content = null;
            fraContainer.Content = new ContentControl() { Content = tmp };
        }

        private void RefreshScreen()
        {

            string temp = txtBuscar.Text;
            txtBuscar.Text = "";
            txtBuscar.Text = temp;

            imgUsuario.Source = null;
            if (bEliminarCreado)
            {
                spBtnCrearEliminar.Children.Remove(btnEliminar);
                bEliminarCreado = false;
            }
            //txtImage.Clear();
            imgUsuario._id = 0;
            imgUsuario._name = "";
            txtPass.Clear();
            txtNombre.Clear();
            rbGarzon.IsChecked = true;
            rbCajero.IsChecked = false;
            rbAdministrador.IsChecked = false;
            checkFavorito.IsChecked = true;
        }

        private void txtImagen_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void txtNombre_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
        }

        private void txtNombre_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.A) { char x = (char)e.Key; x = 'r'; }
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<usuario> listaUsuarios = new List<usuario>();

                wrapItems.Children.Clear();
                wrapItems.Children.Add(spNuevo);
                bNuevoCreado = true;

                if (txtBuscar.Text == "" || txtBuscar.Text.Contains("'") || txtBuscar.Text.Contains("\""))
                {
                    if (!bNuevoCreado)
                    {
                        wrapItems.Children.Add(spNuevo);
                        bNuevoCreado = true;
                    }
                }
                else
                {
                    if (!bNuevoCreado)
                    {
                        wrapItems.Children.Add(spNuevo);
                        bNuevoCreado = true;
                    }

                    listaUsuarios = UsuarioBLL.GetUsers(txtBuscar.Text);

                    if (listaUsuarios != null)
                    {
                        foreach (usuario u in listaUsuarios)
                        {
                            ItemUsuario ItemFromSearchWrap = new ItemUsuario()
                            {
                                ID = u.id,
                                Nombre = u.nombre,
                                Pass = u.pass,
                                Tipo = u.tipo,
                                Favorito = u.favorito == false ? false : true,
                                Imagen = u.imagen
                            };
                            if (u.favorito == null)
                                ItemFromSearchWrap.Favorito = false;

                            ItemFromSearchWrap.BotonUsuario.Click += (se2, ev2) =>
                            {
                                if (!bEliminarCreado)
                                {
                                    spBtnCrearEliminar.Children.Add(btnEliminar);
                                    bEliminarCreado = true;
                                }
                                dpUsuario.IsEnabled = true;
                                btnEditarCrear.Content = "ACTUALIZAR";
                                try
                                {
                                    imgUsuario._id = ItemFromSearchWrap.ID;
                                    imgUsuario._name = ItemFromSearchWrap.Imagen;
                                    imgUsuario.ToolTip = ItemFromSearchWrap.RutaImagen;
                                    imgUsuario.Source = new BitmapImage(new Uri(ItemFromSearchWrap.RutaImagen));
                                }
                                catch
                                {
                                }

                                txtNombre.Text = ItemFromSearchWrap.Nombre;
                                txtPass.Password = ItemFromSearchWrap.Pass;
                                checkFavorito.IsChecked = ItemFromSearchWrap.Favorito == true ? true : false;

                                if (ItemFromSearchWrap.Tipo == "A")
                                    rbAdministrador.IsChecked = true;
                                else if (ItemFromSearchWrap.Tipo == "C")
                                    rbCajero.IsChecked = true;
                                else
                                    rbGarzon.IsChecked = true;
                            };
                            wrapItems.Children.Add(ItemFromSearchWrap);
                        }
                    }
                    else
                    {
                        wrapItems.Children.Clear();
                        if (!bNuevoCreado)
                        {
                            wrapItems.Children.Add(spNuevo);
                            bNuevoCreado = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNombre.Text == "")
                btnBuscaImagen.IsEnabled = false;
            else
                btnBuscaImagen.IsEnabled = true;
        }

        private void btnEditarCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                usuario u = new usuario();
                u.nombre = txtNombre.Text;
                u.pass = txtPass.Password;
                u.config_id = 1;
                u.imagen = imgUsuario._name;

                if (rbAdministrador.IsChecked == true)
                    u.tipo = "A";
                else if (rbCajero.IsChecked == true)
                    u.tipo = "C";
                else
                    u.tipo = "G";

                if (checkFavorito.IsChecked == true)
                    u.favorito = true;
                else
                    u.favorito = false;

                u.imagen = imgUsuario._name;
             
                if (btnEditarCrear.Content.Equals("CREAR"))
                {
                    //DB.Insert<producto>("products", p);
                    UsuarioBLL.Crear(u);
                    MessageBox.Show("Creado");
                }
                else
                    UsuarioBLL.Actualizar(imgUsuario._id, u);
                RefreshScreen();
            }
            catch (Exception)
            {
            }
        }

        void IDisposable.Dispose()
        {
        }
    }
}
