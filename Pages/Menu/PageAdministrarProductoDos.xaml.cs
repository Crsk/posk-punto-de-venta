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
    public partial class PageAdministrarProductoDos : Page
    {
        #region Global
        private ItemCalcularTotal itemCalcularTotal;
        private ItemTeclado teclado;
        private ItemTeclado teclado_mp;
        public bool bProductosFavToggle { get; set; }
        private SolidColorBrush colorDorado;
        private SolidColorBrush colorNeutro;
        private bool bFavorito;

        private ItemProducto itemProductoSeleccionado;
        private ItemAgregado itemAgregadoUno;
        private ItemAgregado itemAgregadoDos;

        private List<ItemMoneda> listaItemsMoneda { get; set; }


        #endregion

        #region Constructor
        public PageAdministrarProductoDos()
        {
            InitializeComponent();

            #region Inicializar otros componentes
            itemCalcularTotal = new ItemCalcularTotal();
            itemProductoSeleccionado = new ItemProducto();
            itemAgregadoUno = new ItemAgregado();
            itemAgregadoDos = new ItemAgregado();
            colorDorado = new SolidColorBrush(Color.FromRgb(255, 150, 0));
            colorNeutro = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            btnExpanderLeft.Foreground = colorDorado;
            expLeft.IsExpanded = true;
            gridBotonesPrincipales.Children.Remove(btnEliminarProducto);


            teclado = new ItemTeclado(new List<TextBox>() { txtBuscar, txtNombre, txtPrecioVenta, txtPuntos, txtCodigo });
            teclado_mp = new ItemTeclado(new List<TextBox>() { txtCantidadMateriaPrima });
            borderTeclado.Child = teclado_mp;

            borderTeclado_dos.Child = teclado;

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

        private void EstablecerFormulario()
        {
            spIzquierda.Children.Clear();
            spDerecha.Children.Clear();
            spOpcionales.Children.Clear();
            if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()))
            {
                spIzquierda.Children.Add(borderNombre);
                spIzquierda.Children.Add(borderCodigo);
                spIzquierda.Children.Add(borderPrecioVenta);
                spIzquierda.Children.Add(borderPuntos);
                spIzquierda.Children.Add(borderFav);
                spIzquierda.Children.Add(borderCompraVenta);
                spIzquierda.Children.Add(borderVenta);
                spIzquierda.Children.Add(borderCompra);

                spDerecha.Children.Add(borderSector);
                spDerecha.Children.Add(borderCategoria);
                spDerecha.Children.Add(borderSubcategoria);
            }
            else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RESTAURANT.ToString()))
            {
                spIzquierda.Children.Add(borderNombre);
                spIzquierda.Children.Add(borderCodigo);
                spIzquierda.Children.Add(borderPrecioVenta);
                spIzquierda.Children.Add(borderPuntos);
                spIzquierda.Children.Add(borderFav);
                spIzquierda.Children.Add(borderCompraVenta);
                spIzquierda.Children.Add(borderVenta);
                spIzquierda.Children.Add(borderCompra);

                spDerecha.Children.Add(borderSector);
                spDerecha.Children.Add(borderCategoria);
                spDerecha.Children.Add(borderSubcategoria);
                spDerecha.Children.Add(borderAgregado);
                spDerecha.Children.Add(borderPreparadoEspecial);
                spDerecha.Children.Add(borderTipoItemVenta);
                spDerecha.Children.Add(borderSectorImpresion);
                spDerecha.Children.Add(borderZindex);
            }
            else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.SUSHI.ToString()))
            {
                spIzquierda.Children.Add(borderNombre);
                spIzquierda.Children.Add(borderCodigo);
                spIzquierda.Children.Add(borderPrecioVenta);
                spIzquierda.Children.Add(borderPuntos);
                spIzquierda.Children.Add(borderFav);
                spIzquierda.Children.Add(borderCompraVenta);
                spIzquierda.Children.Add(borderVenta);
                spIzquierda.Children.Add(borderCompra);

                spDerecha.Children.Add(borderSector);
                spDerecha.Children.Add(borderCategoria);
                spDerecha.Children.Add(borderSubcategoria);
                spDerecha.Children.Add(borderTipoProducto);
                spDerecha.Children.Add(borderAgregado);
                spDerecha.Children.Add(borderPreparadoEspecial);
                spDerecha.Children.Add(borderTipoItemVenta);
                spDerecha.Children.Add(borderSectorImpresion);
                spDerecha.Children.Add(borderZindex);
            }
            else if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()))
            {
                expDerecha.IsExpanded = false;
                spIzquierda.Children.Add(borderNombre);
                spIzquierda.Children.Add(borderCodigo);
                spIzquierda.Children.Add(borderPrecioVenta);
                spIzquierda.Children.Add(borderFav);
                spIzquierda.Children.Add(borderCompraVenta);
                spIzquierda.Children.Add(borderVenta);
                spIzquierda.Children.Add(borderCompra);

                spDerecha.Children.Add(borderSector);
                spDerecha.Children.Add(borderCategoria);
                spDerecha.Children.Add(borderSubcategoria);
            }
            else
                new Notification("Cambia de modo en opciones");
        }

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

        /// <summary>
        /// Crea productos que se muestran como resultado para seleccionar, con tal de ingresarlos a sección VENTA.
        /// bSubCategoria es un booleano que retorna true cuando el texto ingresado coincide con el nombre de una subcategoria
        /// </summary>
        /// <param name="tupla"></param>
        private void MostrarProductos((List<producto> listaProductos, List<promocione> listaPromociones, bool bSubCategoria) tupla)
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

                wrapProductos.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                wrapProductos.Children.Clear();
                txtBuscar.Foreground = colorNeutro;

                if (tupla.listaProductos != null || tupla.listaPromociones != null)
                {
                    if (tupla.bSubCategoria)
                    {
                        txtBuscar.Foreground = colorDorado;
                    }
                    else
                    {
                        txtBuscar.Foreground = colorNeutro;
                    }
                    //tupla.listaPromociones?.ForEach(promo => CrearMostrarItemPromo(promo));
                    tupla.listaProductos?.ForEach(p => CrearMostrarItemProducto(p));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CrearMostrarItemProducto(producto p)
        {
            var ip = new ItemProducto() { Producto = p, SeccionAdministrarProducto = true };

            ip.btnProducto.Click += (se, a) =>
            {
                itemIdMasNombre.Nombre = $"{ip.Producto.nombre}";
                itemIdMasNombre.ID = ip.Producto.id;
                btnCrearEditar.Content = "EDITAR";
                if (!gridBotonesPrincipales.Children.Contains(btnEliminarProducto))
                    gridBotonesPrincipales.Children.Add(btnEliminarProducto);

                // cargar foto del producto
                try
                {
                    BitmapImage imageFromDialog = new BitmapImage();
                    imageFromDialog.BeginInit();
                    imageFromDialog.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + p.imagen);
                    imageFromDialog.EndInit();
                    itemFoto.imagen.Source = imageFromDialog;
                    itemFoto.NombreFoto = p.imagen;
                }
                catch
                {
                    itemFoto.imagen.Source = null;
                }

                txtNombre.Text = p.nombre;
                txtCodigo.Text = p.codigo_barras;
                txtPrecioVenta.Text = $"{p.precio}";
                txtPuntos.Text = $"{p.puntos_cantidad}";

                checkFavorito.IsChecked = p.favorito == true ? true : false;
                rbCompraVenta.IsChecked = p.solo_venta != true && p.solo_compra != true ? true : false;
                rbSoloVenta.IsChecked = p.solo_venta == true ? true : false;
                rbSoloCompra.IsChecked = p.solo_compra == true ? true : false;
                checkRetornable.IsChecked = p.retornable == true ? true : false;
                checkContieneAgregado.IsChecked = p.contiene_agregado == true ? true : false;
                checkPreparadoEspecial.IsChecked = p.preparado_especial == true ? true : false;
                txtZindex.Text = $"{p.z_index}";

                try
                {
                    sector_impresion si = SectorImpresionBLL.Obtener(p.sector_impresion_id);
                    cbSectorImpresion.ItemsSource = SectorImpresionBLL.ObtenerTodo();
                    cbSectorImpresion.DisplayMemberPath = "nombre";
                    cbSectorImpresion.Text = p.sector_impresion.nombre;

                    tipo_itemventa tiv = TipoItemVentaBLL.Obtener(p.id);
                    cbTipoItemVenta.ItemsSource = TipoItemVentaBLL.ObtenerTodo();
                    cbTipoItemVenta.DisplayMemberPath = "nombre";
                    cbTipoItemVenta.Text = p.tipo_itemventa.nombre;
                    //cbTipoItemVenta.SelectedItem = cbTipoItemVenta.ItemsSource.OfType<tipo_itemventa>().Where(x => x.nombre.ToUpper() == $"{p.tipo_itemventa.nombre}").FirstOrDefault();
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "error al cargar cb");
                    cbSectorImpresion.Text = "";
                    cbTipoItemVenta.Text = "";
                }

                if (p.tipo_producto_id != null)
                {
                    cbTipoProducto.ItemsSource = TipoProductoBLL.ObtenerTodo();
                    cbTipoProducto.DisplayMemberPath = "nombre";
                    cbTipoProducto.Text = $"{p.tipo_producto?.nombre}";
                }
                else
                {
                    cbTipoProducto.ItemsSource = null;
                }

                if (p.subcategoria != null)
                {
                    // Cargar ComboBox al escoger item venta
                    try
                    {
                        categoria cat = CategoriaSubcategoriaBLL.ObtenerCategoria(p.sub_categoria_id);
                        sectore sec = CategoriaSectorBLL.ObtenerSector(cat.id);

                        cbCategoria.ItemsSource = CategoriaSectorBLL.ObtenerCategorias(sec.id);
                        cbCategoria.DisplayMemberPath = "nombre";
                        cbCategoria.Text = cat.nombre;

                        cbSector.ItemsSource = SectorBLL.ObtenerTodo();
                        cbSector.DisplayMemberPath = "nombre";
                        cbSector.Text = sec.nombre;

                        cbSubCategoria.ItemsSource = CategoriaSubcategoriaBLL.ObtenerSubcategorias(cat.id);
                        cbSubCategoria.DisplayMemberPath = "nombre";
                        cbSubCategoria.Text = $"{p.subcategoria.nombre}";
                    }
                    catch (Exception)
                    {
                        cbCategoria.ItemsSource = null;
                        cbSubCategoria.ItemsSource = null;
                    }
                }
                else
                {
                    cbCategoria.ItemsSource = null;
                    cbSubCategoria.ItemsSource = null;
                }

                cbProductoParaAgregarMP.Text = p.nombre;
            };
            wrapProductos.Children.Add(ip);
        }

        /// <summary>
        /// Crea un expandible por cada sector, luego crea un expandible por cada categoria de cada sector
        /// Luego crea elemento subcategoría que al clickear, muestra los elementos que coinciden con la subcategoría
        /// </summary>
        private void CrearMenuBusquedaPorCategoria()
        {
            spCategorias.Children.Clear();
            SectorBLL.ObtenerTodo().ForEach(s =>
            {
                spCategorias.Children.Add(new ItemBuscarPorSector() { Sector = s });
            });

            spCategorias.Children.OfType<ItemBuscarPorSector>().ToList().ForEach(ibs =>
            {
                ibs.spItems.Children.Clear();
                CategoriaSectorBLL.ObtenerCategorias(ibs.Sector.id).ForEach(c =>
                {
                    ibs.spItems.Children.Add(new ItemBuscarPorCategoria() { Categoria = c });
                });

                ibs.spItems.Children.OfType<ItemBuscarPorCategoria>().ToList().ForEach(ibc =>
                {
                    ibc.spItems.Children.Clear();
                    CategoriaSubcategoriaBLL.ObtenerSubcategorias(ibc.Categoria.id).ForEach(sc =>
                    {
                        var scbi = new SubCategoriaBuscarItem() { SubCategoria = sc };
                        scbi.btnSubCategoria.Click += (se2, a2) =>
                        {
                            txtBuscar.Text = $"{sc.nombre}";
                            //MostrarPedidos(SubCategoriaBLL.ObtenerProductos(sc.id));
                            bFavorito = false;
                            btnFavorito.Foreground = colorNeutro;
                        };
                        ibc.spItems.Children.Add(scbi);
                    });
                });
            });
        }

        private void LimpiarTodo()
        {
            itemIdMasNombre.Nombre = "ITEM VENTA";
            itemIdMasNombre.ID = null;
            btnCrearEditar.Content = "CREAR";
            gridBotonesPrincipales.Children.Remove(btnEliminarProducto);
            bFavorito = false;
            iconFavorito.Foreground = colorNeutro;

            //cbMateriaPrima.SelectedIndex = -1;
            //cbProductoParaAgregarMP.SelectedIndex = -1;
            txtCantidadMateriaPrima.Clear();

            //txtBuscar.Clear();
            itemFoto.imagen.Source = null;
            itemFoto.NombreFoto = "";
            txtNombre.Clear();
            txtCodigo.Clear();
            txtPrecioVenta.Clear();
            txtPuntos.Clear();
            checkFavorito.IsChecked = false;
            rbSoloVenta.IsChecked = false;
            rbSoloCompra.IsChecked = false;
            cbSector.SelectedIndex = -1;
            cbCategoria.ItemsSource = null;
            cbSubCategoria.ItemsSource = null;
            cbTipoProducto.ItemsSource = null;
            checkRetornable.IsChecked = false;
            checkContieneAgregado.IsChecked = false;
            cbSectorImpresion.SelectedIndex = -1;
            cbTipoItemVenta.SelectedIndex = -1;
            txtZindex.Clear();
        }

        private void CargarMateriasPrimas()
        {
            try
            {
                spMateriaPrima.Children.Clear();
                producto p = cbProductoParaAgregarMP?.SelectedItem as producto;
                if (p != null)
                {
                    ProductoMateriaPrimaBLL.ObtenerTodo(p.id).ForEach(pmp =>
                    {
                        var ilmpc = new ItemLineaMateriaPrimaContenida() { ProductoMateriaPrima = pmp };
                        spMateriaPrima.Children.Add(ilmpc);

                        ilmpc.btnEliminar.Click += (se2, a2) =>
                        {
                            ProductoMateriaPrimaBLL.Eliminar(pmp.id);
                            CargarMateriasPrimas();
                        };
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}");
            }
        }

        #endregion Metodos

        #region Eventos

        private void InitEvents()
        {
            btnMP.Click += (se, a) => expDerecha.IsExpanded ^= true;

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
                        var wi = new WindowTomarFoto(ConfigurationManager.AppSettings["RutaImagenProducto"], itemFoto, txtNombre.Text);
                        wi.Show();
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al cargar la cámara");
                    }
                }
            };

            cbMateriaPrima.SelectionChanged += (se, a) =>
            {
                if (cbMateriaPrima.SelectedItem != null)
                    lbUnidadMedidaMP.Content = $"{(cbMateriaPrima.SelectedItem as materiasprima).unidades_medida.nombre}";
            };

            cbProductoParaAgregarMP.SelectionChanged += (se, a) =>
            {
                CargarMateriasPrimas();
            };

            btnAgregarMateriaPrima.Click += (se, a) =>
            {
                try
                {
                    materiasprima mp = cbMateriaPrima.SelectedItem as materiasprima;
                    producto p = cbProductoParaAgregarMP.SelectedItem as producto;
                    producto_materiaprima pmp = new producto_materiaprima() { materiaprima_id = mp?.id, producto_id = p?.id, cantidad = Convert.ToDecimal(txtCantidadMateriaPrima.Text) };
                    ProductoMateriaPrimaBLL.Crear(pmp);
                    var ilmpc = new ItemLineaMateriaPrimaContenida() { ProductoMateriaPrima = ProductoMateriaPrimaBLL.Obtener(pmp.id) };
                    spMateriaPrima.Children.Add(ilmpc);
                    txtCantidadMateriaPrima.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex}");
                }
            };

            btnNuevoItemVenta.Click += (se, a) =>
            {
                btnCrearEditar.Content = "CREAR";
                gridBotonesPrincipales.Children.Remove(btnEliminarProducto);
                itemIdMasNombre.ID = null;
                itemIdMasNombre.Nombre = "ITEM VENTA";
                LimpiarTodo();
            };

            btnEliminarProducto.Click += (se, a) =>
            {
                ProductoBLL.Borrar(itemIdMasNombre.ID);
                new Notification("BORRADO", $"{itemIdMasNombre.Nombre}");
                LimpiarTodo();
            };

            btnCrearEditar.Click += (se, a) =>
            {
                // Crear item antes de ingresarlo a la BD
                producto p = new producto();
                try
                {
                    p = new producto()
                    {
                        nombre = txtNombre.Text,
                        codigo_barras = txtCodigo.Text,
                        precio = Convert.ToInt32(txtPrecioVenta.Text),
                        retornable = checkRetornable.IsChecked,
                        favorito = checkFavorito.IsChecked,
                        puntos_cantidad = 0,
                        imagen = itemFoto.NombreFoto,
                        sub_categoria_id = (cbSubCategoria.SelectedItem as subcategoria).id,
                        tipo_producto_id = (cbTipoProducto.SelectedItem as tipo_producto).id,
                        proveedor_id = null,
                        contiene_agregado = checkContieneAgregado.IsChecked,
                        solo_venta = rbSoloVenta.IsChecked,
                        solo_compra = rbSoloCompra.IsChecked,
                        preparado_especial = checkPreparadoEspecial.IsChecked,
                        tipo_itemventa_id = (cbTipoItemVenta.SelectedItem as tipo_itemventa).id,
                        sector_impresion_id = (cbSectorImpresion.SelectedItem as sector_impresion).id,
                        z_index = Convert.ToInt32(txtZindex.Text)
                    };
                    if (ProductoBLL.NombreExiste(p?.nombre) && btnCrearEditar.Content.Equals("CREAR"))
                    {
                        new Notification($"{p?.nombre.ToUpper()} YA EXISTE", "Intenta con otro nombre o edita el existente", Notification.Type.Danger);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al crear item antes de ingresarlo a la DB");
                    return;
                }

                if (btnCrearEditar.Content.Equals("CREAR"))
                {
                    // Crear item venta
                    try
                    {
                        if (p != null)
                            ProductoBLL.Crear(p);

                        cbProductoParaAgregarMP.ItemsSource = ProductoBLL.ObtenerTodo();
                        cbProductoParaAgregarMP.DisplayMemberPath = "nombre";
                        cbProductoParaAgregarMP.Text = p.nombre;
                        new Notification("LISTO!");
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al crear item venta");
                    }
                }
                else
                {
                    // Editar item venta
                    try
                    {
                        if (p != null)
                        {
                            p.id = (int)itemIdMasNombre.ID;
                            ProductoBLL.Actualizar(p);
                            new Notification("LISTO!", "Editaste un item venta");
                        }
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error al editar item venta");
                    }
                }
                LimpiarTodo();
            };

            Loaded += (se, a) =>
            {
                EstablecerFormulario();

                expLeft.IsExpanded = false;
                // Cargar Sección Administrar Item
                try
                {
                    cbProductoParaAgregarMP.ItemsSource = ProductoBLL.ObtenerTodo();
                    cbProductoParaAgregarMP.DisplayMemberPath = "nombre";

                    cbMateriaPrima.ItemsSource = MateriaPrimaBLL.ObtenerTodo();
                    cbMateriaPrima.DisplayMemberPath = "nombre";

                    cbSector.ItemsSource = SectorBLL.ObtenerTodo();
                    cbSector.DisplayMemberPath = "nombre";

                    cbTipoItemVenta.ItemsSource = TipoItemVentaBLL.ObtenerTodo();
                    cbTipoItemVenta.DisplayMemberPath = "nombre";

                    cbSectorImpresion.ItemsSource = SectorImpresionBLL.ObtenerTodo();
                    cbSectorImpresion.DisplayMemberPath = "nombre";


                    LimpiarTodo();

                    cbTipoProducto.ItemsSource = TipoProductoBLL.ObtenerTodo();
                    cbTipoProducto.DisplayMemberPath = "nombre";

                    try
                    {
                        txtPuntos.Text = "0";
                        txtZindex.Text = "0";
                        cbTipoItemVenta.Text = "plato fondo";
                        cbTipoItemVenta.Visibility = Visibility.Hidden;
                    }
                    catch
                    {
                    }

                    // cargar ComboBox al cambiar selección
                    try
                    {
                        cbSector.SelectionChanged += (se2, a2) =>
                        {
                            if (cbSector.SelectedItem != null)
                            {
                                cbCategoria.ItemsSource = CategoriaSectorBLL.ObtenerCategorias((cbSector.SelectedItem as sectore).id);
                                cbCategoria.DisplayMemberPath = "nombre";
                                cbCategoria.SelectionChanged += (se3, a3) =>
                                {
                                    if (cbCategoria.SelectedItem != null)
                                    {
                                        cbSubCategoria.ItemsSource = CategoriaSubcategoriaBLL.ObtenerSubcategorias((cbCategoria.SelectedItem as categoria).id);
                                        cbSubCategoria.DisplayMemberPath = "nombre";
                                    }
                                    else
                                        cbSubCategoria.ItemsSource = null;
                                };
                            }
                            //else
                            //    cbSector.ItemsSource = null;
                        };
                    }
                    catch (Exception ex)
                    {
                        PoskException.Make(ex, "Error con los ComboBox al cambiar selección");
                        cbCategoria.ItemsSource = null;
                        cbSubCategoria.ItemsSource = null;
                    }

                    string tipoUsuario = "";
                    if (Settings.Usuario?.tipo.ToUpper() == "G")
                        tipoUsuario = "Garzón";
                    else if (Settings.Usuario?.tipo.ToUpper() == "C")
                        tipoUsuario = "Cajero(a)";
                    else if (Settings.Usuario?.tipo.ToUpper() == "A")
                        tipoUsuario = "Admin";
                    lbUsuario.Content = $"{tipoUsuario}: {Settings.Usuario?.nombre}";

                    CrearMenuBusquedaPorCategoria();
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al cargar sección Administrar Item");
                }
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
                    MostrarProductos(ProductoBLL.ObtenerCoincidencias(txtBuscar.Text));
                else
                {
                    wrapProductos.Children.Clear();
                    txtBuscar.Foreground = colorNeutro;
                }
            };

            btnExpanderLeft.Click += (se, a) =>
            {
                expLeft.IsExpanded ^= true;
                if (expLeft.IsExpanded)
                    btnExpanderLeft.Foreground = colorDorado;
                else
                    btnExpanderLeft.Foreground = colorNeutro;
            };

            btnExpanderBottom.Click += (se, a) =>
                expBottom.IsExpanded ^= true;
            expBottom.Expanded += (se, a) =>
                btnExpanderBottom.Foreground = colorDorado;
            expBottom.Collapsed += (se, a) =>
                btnExpanderBottom.Foreground = colorNeutro;

            btnFavorito.Click += (se, a) =>
            {
                if (!bFavorito)
                {
                    iconFavorito.Foreground = colorDorado;
                    // descomentar fav
                    MostrarProductos((ProductoBLL.GetFavs()));
                    txtBuscar.Text = "FAVORITOS";
                    txtBuscar.Foreground = colorDorado;
                    bFavorito = true;
                }
                else
                {
                    txtBuscar.Text = "";
                    iconFavorito.Foreground = colorNeutro;
                    txtBuscar.Foreground = colorNeutro;
                    wrapProductos.Children.Clear();
                    bFavorito = false;
                }
            };

            overlay.MouseLeftButtonDown += (se, e) => MostrarOverlay(false);

            btnCerraTecladoMoneda.Click += (se, a) => expTecladoPagar.IsExpanded = false;
        }

        #endregion Eventos
    }
}
