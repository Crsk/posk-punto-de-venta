using System;
using System.Windows.Controls;
using System.Windows.Media;
using posk.Globals;
using System.Windows.Threading;
using posk.Controls;
using System.Collections.Generic;
using posk.BLL;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using posk.Popup;
using posk.Models;
using posk.Pages.Menu;
using posk.Controls.Accion;
using posk.Pages.PopUp;
using System.Configuration;
using System.Windows.Media.Imaging;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarCliente : Page
    {
        #region Global
        private ItemCalcularTotal itemCalcularTotal;
        private ItemTeclado teclado;
        public bool bClientesFavToggle { get; set; }
        private SolidColorBrush colorDorado;
        private SolidColorBrush colorNeutro;
        private bool bFavorito;

        #endregion

        #region Constructor
        public PageAdministrarCliente()
        {
            InitializeComponent();

            #region Inicializar otros componentes
            itemCalcularTotal = new ItemCalcularTotal();
            colorDorado = new SolidColorBrush(Color.FromRgb(255, 150, 0));
            colorNeutro = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            gridBotonesPrincipales.Children.Remove(btnEliminarCliente);

            teclado = new ItemTeclado(new List<TextBox>() { txtBuscar });
            borderTeclado.Child = teclado;

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1); // Hour, Minutes, Second.
            dtClockTime.Tick += dtClockTime_Tick;
            dtClockTime.Start();
            #endregion Inicializar otros componentes

            InitEvents();
        }
        private void dtClockTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (InternetChecker.IsConnectedToInternet())
                {
                    lbFecha.Content = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(46, 139, 87));
                }
                else
                {
                    lbFecha.Content = $"{DateTime.Now}";
                    internetStatus.Foreground = new SolidColorBrush(Color.FromRgb(252, 52, 52));
                }
            }
            catch (Exception)
            {
                //lbInfo.Content = "ERROR: " + err.Message;
            }
        }
        #endregion Constructor

        #region Metodos

        /// <summary>
        /// Si b es true, Muestra overlay con efeceto fadein y abre un popup el cual recibe contenido previamente.
        /// Si b es false, oculta el overlay y cierra el popup.
        /// </summary>
        /// <param name="b"></param>
        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                overlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = true;
            }
            else
            {
                teclado.ExpTeclado.IsExpanded = false;
                expTecladoPopup.IsExpanded = false;
                overlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5));
                overlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = false;
            }
        }

        private void CargarInfoBarraInferior()
        {
            // Cargar Sección Administrar Item
            try
            {
                LimpiarTodo();

                string tipoUsuario = "";
                if (Settings.Usuario?.tipo.ToUpper() == "G")
                    tipoUsuario = "Garzón";
                else if (Settings.Usuario?.tipo.ToUpper() == "C")
                    tipoUsuario = "Cajero(a)";
                else if (Settings.Usuario?.tipo.ToUpper() == "A")
                    tipoUsuario = "Admin";
                lbUsuario.Content = $"{tipoUsuario}: {Settings.Usuario?.nombre}";
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al cargar sección Administrar Item");
            }
        }

        /// <summary>
        /// Mensaje popup con una lista de botones programados (ItemAccion)
        /// </summary>
        /// <param name="listaItemsAccion"></param>
        /// <param name="titulo"></param>
        private void MostrarAccion(List<ItemAccion> listaItemsAccion, string titulo)
        {
            var wop = new WindowOpcionesPopup(listaItemsAccion, titulo);
            wop.Opacity = 0;
            wop.Topmost = true;
            wop.Show();

            DoubleAnimation fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            wop.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void MostrarClientes(List<cliente> listaClientes)
        {
            try
            {
                if (txtBuscar.Text == "FAVORITOS")
                {
                    iconFavorito.Foreground = colorDorado;
                    txtBuscar.Foreground = colorDorado;
                    bFavorito = true;
                    return;
                }
                else
                {
                    iconFavorito.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    bFavorito = false;
                }

                wrapClientes.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                wrapClientes.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                listaClientes?.ForEach(c => CrearMostrarItemCliente(c));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CrearMostrarItemCliente(cliente c)
        {
            var ic = new ItemCliente() { Cliente = c };

            ic.btnItemCliente.Click += (se, a) =>
            {
                itemIdMasNombre.Nombre = $"{ic.Cliente.nombre}";
                itemIdMasNombre.ID = ic.Cliente.id;
                btnCrearEditar.Content = "EDITAR";
                if (!gridBotonesPrincipales.Children.Contains(btnEliminarCliente))
                    gridBotonesPrincipales.Children.Add(btnEliminarCliente);

                // cargar foto del cliente
                try
                {
                    BitmapImage imageFromDialog = new BitmapImage();
                    imageFromDialog.BeginInit();
                    imageFromDialog.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenCliente"] + c.imagen);
                    imageFromDialog.EndInit();
                    itemFoto.imagen.Source = imageFromDialog;
                    itemFoto.NombreFoto = c.imagen;
                }
                catch
                {
                    itemFoto.imagen.Source = null;
                }

                txtNombre.Text = c.nombre;
                txtPuntos.Text = $"{c.punto.puntos_activos}";

                checkFavorito.IsChecked = c.favorito == true ? true : false;
            };
            wrapClientes.Children.Add(ic);
        }


        private void LimpiarTodo()
        {
            itemIdMasNombre.Nombre = "CLIENTE";
            itemIdMasNombre.ID = null;
            btnCrearEditar.Content = "CREAR";
            gridBotonesPrincipales.Children.Remove(btnEliminarCliente);
            bFavorito = false;
            iconFavorito.Foreground = colorNeutro;
            txtBuscar.Clear();
            itemFoto.imagen.Source = null;
            itemFoto.NombreFoto = "";
            txtNombre.Clear();
            txtPuntos.Clear();
            checkFavorito.IsChecked = false;
        }
        #endregion Metodos

        #region secciones

        #endregion secciones

        #region Eventos

        private void InitEvents()
        {
            itemFoto.btnImagen.Click += (se, a) =>
            {
                if (String.IsNullOrEmpty(txtNombre.Text))
                {
                    new Notification("Agrega un nombre", "antes de tomar la foto", Notification.Type.Warning);
                }
                else
                {
                    // abrir sección tomar foto
                    try
                    {
                        var wi = new WindowTomarFoto(ConfigurationManager.AppSettings["RutaImagenCliente"], itemFoto, txtNombre.Text);
                        wi.Show();
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al cargar la cámara");
                    }
                }
            };

            btnNuevoCliente.Click += (se, a) =>
            {
                btnCrearEditar.Content = "CREAR";
                gridBotonesPrincipales.Children.Remove(btnEliminarCliente);
                itemIdMasNombre.ID = null;
                itemIdMasNombre.Nombre = "CLIENTE";
                LimpiarTodo();
            };

            btnEliminarCliente.Click += (se, a) =>
            {
                ClienteBLL.Borrar(itemIdMasNombre.ID);
                new Notification("BORRADO", $"{itemIdMasNombre.Nombre}");
                LimpiarTodo();
            };

            btnCrearEditar.Click += (se, a) =>
            {
                // Crear item antes de ingresarlo a la BD
                cliente c;
                try
                {
                    c = new cliente()
                    {
                        nombre = txtNombre.Text,
                        rut = txtRut.Text,
                        contacto = txtContacto.Text,
                        favorito = checkFavorito.IsChecked,
                        imagen = itemFoto.NombreFoto,
                        puntos_id = 1,
                        comentario = "",
                        pass = "12"
                    };
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al crear item antes de ingresarlo a la DB");
                    return;
                }

                if (btnCrearEditar.Content.Equals("CREAR"))
                {
                    // Crear cliente
                    try
                    {
                        if (c != null)
                            ClienteBLL.AddClient(c);

                        new Notification("LISTO!");
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al crear cliente");
                    }
                }
                else
                {
                    // Editar cliente
                    try
                    {
                        if (c != null)
                        {
                            c.id = (int)itemIdMasNombre.ID;
                            ClienteBLL.Actualizar(c);
                            new Notification("LISTO!", "Editaste un cliente");
                        }
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al editar cliente");
                    }
                }
                LimpiarTodo();
            };

            Loaded += (se, a) =>
            {
                CargarInfoBarraInferior();
            };

            txtBuscar.GotFocus += (se, a) =>
            {
                txtBuscar.Text = "";
                iconFavorito.Foreground = colorNeutro;

                bFavorito = false;
            };

            txtBuscar.TextChanged += (se, a) =>
            {
                if (txtBuscar.Text != "")
                    MostrarClientes(ClienteBLL.GetClients(txtBuscar.Text));
                else
                {
                    wrapClientes.Children.Clear();
                    txtBuscar.Foreground = colorNeutro;
                }
            };

            btnFavorito.Click += (se, a) =>
            {
                if (!bFavorito)
                {
                    iconFavorito.Foreground = colorDorado;
                    // descomentar fav
                    MostrarClientes((ClienteBLL.GetFavs()));
                    txtBuscar.Text = "FAVORITOS";
                    txtBuscar.Foreground = colorDorado;
                    bFavorito = true;
                }
                else
                {
                    txtBuscar.Text = "";
                    iconFavorito.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    wrapClientes.Children.Clear();
                    bFavorito = false;
                }
            };

            overlay.MouseLeftButtonDown += (se, e) => MostrarOverlay(false);

            btnCerraTecladoMoneda.Click += (se, a) => expTecladoPagar.IsExpanded = false;
        }

        #endregion Eventos
    }
}
