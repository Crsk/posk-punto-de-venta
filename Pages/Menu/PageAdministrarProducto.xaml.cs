using System;
using System.Windows;
using System.Windows.Controls;
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
using posk.Popup;
using System.Windows.Media.Animation;

namespace posk.Pages.Menu
{
    public partial class PageAdministrarProducto : Page, IDisposable
    {
        string RutaImagenProducto;

        // item nuevo producto
        StackPanel spNuevo;
        Button btnNuevo, btnEliminar;
        Label lbNuevo;
        //int productSelectedId = 0;
        decimal stockTmp;
        int iva = Convert.ToInt32(ConfigurationManager.AppSettings["IVA"]);
        bool bEliminarCreado = false, bNuevoCreado = false;
        ItemTeclado teclado;
        ItemTecladoNumerico tecladoNumerico;

        public PageAdministrarProducto()
        {
            InitializeComponent();
            spItemsPromo.Visibility = Visibility.Hidden;
            btnEscogerProductosPromo.Visibility = Visibility.Hidden;
            dpProducto.IsEnabled = false;
            teclado = new ItemTeclado(new List<TextBox>() { txtBuscar, txtNombre });
            borderTeclado.Child = teclado;
            tecladoNumerico = new ItemTecladoNumerico(new List<TextBox>() { txtBarCode, txtStockInicial, txtCost, txtPrice });
            borderTecladoNumerico.Child = tecladoNumerico;

            RutaImagenProducto = ConfigurationManager.AppSettings["RutaImagenProducto"];
            stockTmp = 0;

            spProductItems1.Children.Remove(spStock);


            spPrice.Children.Remove(btnPriceInfo);
            txtPrice.Width = 150;
            bool bIsPriceInfoCreated = false;

            txtCost.TextChanged += (se, ev) =>
            {
                txtPrice.Text = "";
            };

            txtPrice.TextChanged += (se, ev) =>
            {
                try
                {
                    if (txtPrice.Text != "" && txtCost.Text != "")
                    {
                        decimal costDecimal = Convert.ToDecimal(txtCost.Text);
                        decimal priceDecimal = Convert.ToDecimal(txtPrice.Text);

                        int cost = Convert.ToInt32(costDecimal);
                        int price = Convert.ToInt32(priceDecimal);

                        int lenghtOfPrice = price.ToString().Length;

                        int tenPctOfPrice = price * 10 / 100;
                        string pointsStr = tenPctOfPrice.ToString().Substring(0, 1) + "0";

                        switch (lenghtOfPrice)
                        {
                            case 1:
                                pointsStr = "0";
                                break;
                            case 2:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 1);
                                break;
                            case 3:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 2);
                                break;
                            case 4:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 2) + "0";
                                break;
                            case 5:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 2) + "00";
                                break;
                            case 6:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 2) + "000";
                                break;
                            case 7:
                                pointsStr = tenPctOfPrice.ToString().Substring(0, 2) + "0000";
                                break;
                            default:
                                pointsStr = "0";
                                break;
                        }


                        if (price > cost)
                        {
                            txtPoints.Text = pointsStr;
                            if (!bIsPriceInfoCreated)
                            {
                                spPrice.Children.Add(btnPriceInfo);
                                txtPrice.Width = 105;
                                bIsPriceInfoCreated = true;
                            }
                        }
                        else
                        {
                            txtPoints.Text = "";
                            if (bIsPriceInfoCreated)
                            {
                                spPrice.Children.Remove(btnPriceInfo);
                                txtPrice.Width = 150;
                                bIsPriceInfoCreated = false;
                            }
                        }
                    }
                    else
                    {
                        txtPoints.Text = "";
                        if (bIsPriceInfoCreated)
                        {
                            spPrice.Children.Remove(btnPriceInfo);
                            txtPrice.Width = 150;
                            bIsPriceInfoCreated = false;
                        }
                    }
                }
                catch
                {
                    txtCost.Text = "";
                    txtPrice.Text = "";
                    txtPoints.Text = "";
                    spPrice.Children.Remove(btnPriceInfo);
                    txtPrice.Width = 150;
                    bIsPriceInfoCreated = false;
                }
            };
            btnPriceInfo.Click += (se, ev) =>
            {
                decimal costDecimal = Convert.ToDecimal(txtCost.Text);
                decimal priceDecimal = Convert.ToDecimal(txtPrice.Text);

                int cost = Convert.ToInt32(costDecimal);
                int price = Convert.ToInt32(priceDecimal);

                WindowPriceInfo wpi = new WindowPriceInfo(cost, price); wpi.Show();
            };

            //if (txtPrecioCompra.Text != "")
            //    precioCompra = Convert.ToDouble(txtPrecioCompra.Text);
            //if (txtPrecioVenta.Text != "")
            //    precioVenta = Validations.ToInt(txtPrecioVenta.Text);
            //if (txtGananciaPct.Text != "")
            //    gananciaPct = Validations.ToInt(txtGananciaPct.Text);
            //if (txtGanancia.Text != "")
            //    ganancia = Convert.ToDouble(txtGanancia.Text);

            //txtGanancia.TextChanged += (se, ev) =>
            //{
            //    txtPrecioVenta.Text = (precioCompra + ganancia) + "";
            //};

            //txtPrecioVenta.TextChanged += (se, ev) =>
            //{
            //    txtGanancia.Text = (precioVenta - precioCompra) + "";
            //    txtPrecioVenta.Text = (precioCompra + ganancia) + "";
            //};

            btnNuevo = new Button()
            {
                Name = "btnNuevoProducto",
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
                    ProductoBLL.Borrar(imgProducto._id);
                    MessageBox.Show("Producto borrado");
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
                RefreshScreen();
                dpProducto.IsEnabled = true;
                btnEditarCrear.Content = "CREAR";
                if (!spProductItems2.Children.Contains(txtCost))
                    spProductItems2.Children.Insert(0, txtCost);
                if (spProductItems1.Children.Contains(spStock))
                    spProductItems1.Children.Remove(spStock);
                if (!spProductItems1.Children.Contains(txtStockInicial))
                    spProductItems1.Children.Insert(2, txtStockInicial);
                try
                {
                    imgProducto.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + "default.jpg"));
                    imgProducto._name = "default.jpg";
                }
                catch (Exception)
                {
                    imgProducto.Source = null;
                }
                spItemsPromo.Background = null;
            };

            if (!bNuevoCreado)
            {
                wrapProductos.Children.Add(spNuevo);
                bNuevoCreado = true;
            }

            btnSeleccionarCategoria.Click += (se, ev) =>
            {
                var cp = new CategoriaPopup();
                expPopup.Content = cp;
                MostrarOverlay(true);
                cp.OnSelect += (se2, a) =>
                {
                    txtCategory.Text = a;
                    MostrarOverlay(false);
                    if (a.ToLower().Equals("promocion") || a.ToLower().Equals("promoción") || a.ToLower().Equals("promo"))
                    {
                        spItemsPromo.Visibility = Visibility.Visible;
                        btnEscogerProductosPromo.Visibility = Visibility.Visible;
                        spPromo.Background = new SolidColorBrush(Color.FromRgb(193, 193, 193));

                        spProveedor.Height = 0;
                        spProveedor.Visibility = Visibility.Hidden;
                        txtCost.Height = 0;
                        txtCost.Visibility = Visibility.Hidden;
                        btnPriceInfo.Visibility = Visibility.Hidden;
                        txtStockInicial.Visibility = Visibility.Hidden;
                        spStock.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        spItemsPromo.Visibility = Visibility.Hidden;
                        spItemsPromo.Children.Clear();
                        btnEscogerProductosPromo.Visibility = Visibility.Hidden;
                        spPromo.Background = null;

                        spProveedor.Height = 54;
                        spProveedor.Visibility = Visibility.Visible;
                        txtCost.Height = 54;
                        txtCost.Visibility = Visibility.Visible;
                        btnPriceInfo.Visibility = Visibility.Visible;
                        txtStockInicial.Visibility = Visibility.Visible;
                        spStock.Visibility = Visibility.Visible;
                    }
                };
            };

            btnEscogerProductosPromo.Click += (se, a) =>
            {
                spItemsPromo.Children.Clear();
                var pp = new PromoPopup();
                expPopup.Content = pp;
                MostrarOverlay(true);
                pp.OnSelect += (se2, a2) =>
                {
                    a2.ForEach(x =>
                    {
                        var pi = new PromoItem_xs();
                        pi.txtNombreProducto.Content = x.txtNombreProducto.Content;
                        pi.Producto = x.Producto;
                        pi.btnEliminar.Click += (se3, a3) => spItemsPromo.Children.Remove(pi);
                        spItemsPromo.Children.Add(pi);
                    });
                    MostrarOverlay(false);
                };
            };

            btnProveedor.Click += (se, ev) =>
            {
                var pp = new ProveedorPopup();
                expPopup.Content = pp;
                MostrarOverlay(true);
                pp.OnSelect += (se2, a) =>
                {
                    txtSupplier.Text = a;
                    MostrarOverlay(false);
                };
            };
            overlay.MouseLeftButtonUp += (se, a) => MostrarOverlay(false);
        }

        private void RefreshScreen()
        {

            string temp = txtBuscar.Text;
            txtBuscar.Text = "";
            txtBuscar.Text = temp;

            imgProducto.Source = null;
            if (bEliminarCreado)
            {
                spBtnCrearEliminar.Children.Remove(btnEliminar);
                bEliminarCreado = false;
            }
            //txtImage.Clear();
            imgProducto._id = 0;
            imgProducto._name = "";
            txtNombre.Clear();
            txtBarCode.Clear();
            txtStock.Clear();
            txtCost.Clear();
            txtPrice.Clear();
            txtPoints.Clear();
            txtCategory.Clear();
            txtSupplier.Clear();
            spItemsPromo.Children.Clear();

            btnEscogerProductosPromo.Visibility = Visibility.Hidden;
            spPromo.Background = null;

            spProveedor.Height = 54;
            spProveedor.Visibility = Visibility.Visible;
            txtCost.Height = 54;
            txtCost.Visibility = Visibility.Visible;
            btnPriceInfo.Visibility = Visibility.Visible;
            txtStockInicial.Visibility = Visibility.Visible;
            spStock.Visibility = Visibility.Visible;
        }

        private void MostrarOverlay(bool b)
        {
            if (b)
            {
                overlay.Visibility = Visibility.Visible;
                DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.7));
                overlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = true;
            }
            else
            {
                teclado.ExpTeclado.IsExpanded = false;
                overlay.Visibility = Visibility.Hidden;
                DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.7));
                overlay.BeginAnimation(OpacityProperty, animation);
                expPopup.IsExpanded = false;
            }
        }

        private void txtImagen_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void btnBuscaImagen_Click(object sender, RoutedEventArgs e)
        {
            var wi = new WindowChooseOrTakeImage(ConfigurationManager.AppSettings["RutaImagenProducto"], imgProducto, txtNombre.Text);
            wi.Show();
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
            List<Models.producto> listaProductos = new List<Models.producto>();

            wrapProductos.Children.Clear();
            wrapProductos.Children.Add(spNuevo);
            bNuevoCreado = true;

            if (txtBuscar.Text == "" || txtBuscar.Text.Contains("'") || txtBuscar.Text.Contains("\""))
            {
                if (!bNuevoCreado)
                {
                    wrapProductos.Children.Add(spNuevo);
                    bNuevoCreado = true;
                }
            }
            else
            {
                if (!bNuevoCreado)
                {
                    wrapProductos.Children.Add(spNuevo);
                    bNuevoCreado = true;
                }

                //listaProductos = await productDB.GetByCoincidence(txtBuscar.Text);


                listaProductos = ProductoBLL.ObtenerCoincidencias(txtBuscar.Text).listaProductos;


                if (listaProductos != null)
                {
                    foreach (Models.producto p in listaProductos)
                    {

                        StackPanel sp = new StackPanel();
                        Border border = new Border();
                        border.Child = sp;
                        //                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(100,100,100));
                        //                    border.BorderThickness = new Thickness(1);
                        Label lb = new Label();
                        lb.Width = 160;
                        lb.Height = 24;
                        lb.Margin = new Thickness(4, 0, 4, 0);
                        lb.Content = p.nombre;
                        lb.VerticalAlignment = VerticalAlignment.Top;
                        lb.VerticalContentAlignment = VerticalAlignment.Top;
                        lb.HorizontalContentAlignment = HorizontalAlignment.Center;

                        Image img = new Image();

                        try
                        {
                            img = new Image() { Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + p.imagen)) };
                        }
                        catch (Exception)
                        {
                            img = new Image() { Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + "default.jpg")) };
                        }


                        ItemProduct productItemFromSearchWrap = new ItemProduct()
                        {
                            Id = p.id,
                            ProductName = p.nombre,
                            BarCode = p.codigo_barras,
                            //FastCode = p.fast_code,
                            //BuyPrice = Convert.ToInt32(p.buy_price),
                            Ammount = Convert.ToInt32(p.precio),
                            Points = Convert.ToInt32(p.puntos_cantidad),
                            Image = p.imagen,
                            SubCategoryId = Convert.ToInt32(p.sub_categoria_id),
                            SupplierId = Convert.ToInt32(p.proveedor_id),

                            BorderThickness = new Thickness(0),
                            Height = 120,
                            Width = 160,
                            Margin = new Thickness(4, 4, 4, 0),
                            VerticalContentAlignment = VerticalAlignment.Center,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            Content = img
                        };
                        try
                        {
                            //productItemFromSearchWrap.promo_id = (int)p.promocion_id;
                        }
                        catch
                        {
                            productItemFromSearchWrap.promo_id = 0;
                        }

                        if (productItemFromSearchWrap.Content == null)
                            productItemFromSearchWrap.Content = img;

                        sp.Children.Add(productItemFromSearchWrap);
                        sp.Children.Add(lb);

                        productItemFromSearchWrap.Click += (se2, ev2) =>
                        {
                            if (!bEliminarCreado)
                            {
                                spBtnCrearEliminar.Children.Add(btnEliminar);
                                bEliminarCreado = true;
                            }

                            dpProducto.IsEnabled = true;
                            btnEditarCrear.Content = "ACTUALIZAR";
                            imgProducto._id = productItemFromSearchWrap.Id;
                            imgProducto._name = productItemFromSearchWrap.ProductName + ".jpg";
                            txtCategory._id = productItemFromSearchWrap.CategoryId;
                            txtSupplier._id = productItemFromSearchWrap.SupplierId;
                            txtCategory._promoID = productItemFromSearchWrap.promo_id;

                            productItemFromSearchWrap.Stock = CompraBLL.GetStockByProductId(productItemFromSearchWrap.Id);
                            stockTmp = productItemFromSearchWrap.Stock;

                            if (spProductItems1.Children.Contains(txtStockInicial))
                                spProductItems1.Children.Remove(txtStockInicial);
                            if (!spProductItems1.Children.Contains(spStock))
                                spProductItems1.Children.Insert(2, spStock);
                            if (spProductItems2.Children.Contains(txtCost))
                                spProductItems2.Children.Remove(txtCost);

                            txtNombre.Text = productItemFromSearchWrap.ProductName;
                            txtBarCode.Text = productItemFromSearchWrap.BarCode + "";
                            txtStock.Text = productItemFromSearchWrap.Stock + "";
                            txtCost.Text = productItemFromSearchWrap.Cost + "";
                            txtPrice.Text = productItemFromSearchWrap.Ammount + "";
                            txtPoints.Text = productItemFromSearchWrap.Points + "";
                            //txtImage.Text = productItemFromSearchWrap.Image;



                            try
                            {
                                BitmapImage image = new BitmapImage();
                                image.BeginInit();
                                image.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + productItemFromSearchWrap.Image);
                                image.EndInit();
                                imgProducto.Source = image;
                            }
                            catch (Exception)
                            {
                                imgProducto.Source = null;
                            }
                            imgProducto.ToolTip = productItemFromSearchWrap.Image;

                            txtSupplier.Text = DB.GetSupplierName(Convert.ToInt32(p.proveedor_id));
                            if (txtCategory.Text.ToLower().Equals("promo") || txtCategory.Text.ToLower().Equals("promoción") || txtCategory.Text.ToLower().Equals("promocion"))
                            {
                                spItemsPromo.Children.Clear();
                                PromoBLL.ObtenerDetallePromocion(txtCategory._id).ForEach(x =>
                                {
                                    if (x != null)
                                    {
                                        PromoItem_xs pixs = new PromoItem_xs();
                                        pixs.Producto = x.producto;
                                        pixs.txtNombreProducto.Content = x.producto.nombre;
                                        pixs.btnEliminar.Click += (se, a) => spItemsPromo.Children.Remove(pixs);
                                        spItemsPromo.Children.Add(pixs);
                                    }
                                });

                                spItemsPromo.Visibility = Visibility.Visible;
                                btnEscogerProductosPromo.Visibility = Visibility.Visible;
                                spPromo.Background = new SolidColorBrush(Color.FromRgb(193, 193, 193));

                                spProveedor.Height = 0;
                                spProveedor.Visibility = Visibility.Hidden;
                                txtCost.Height = 0;
                                txtCost.Visibility = Visibility.Hidden;
                                btnPriceInfo.Visibility = Visibility.Hidden;
                                txtStockInicial.Visibility = Visibility.Hidden;
                                spStock.Visibility = Visibility.Hidden;

                                //List<detalle_promocion> productosPromo = PromoBLL.ObtenerProductosPromocion(p.promocion_id);
                                //if (productosPromo != null)
                                //{
                                //    productosPromo.ForEach(x =>
                                //    {
                                //        producto pr = ProductoBLL.ObtenerPorID(x.producto_id);
                                //        var pi = new PromoItem_xs();
                                //        pi.txtNombreProducto.Content = pr.nombre;
                                //        pi.Producto = pr;
                                //        pi.btnEliminar.Click += (se3, a3) => spItemsPromo.Children.Remove(pi);
                                //        spItemsPromo.Children.Add(pi);
                                //    });
                                //}
                                //else
                                    //MessageBox.Show("productosPromo es null");
                            }
                            else
                            {
                                spItemsPromo.Visibility = Visibility.Hidden;
                                spItemsPromo.Children.Clear();
                                btnEscogerProductosPromo.Visibility = Visibility.Hidden;
                                spPromo.Background = null;

                                spProveedor.Height = 54;
                                spProveedor.Visibility = Visibility.Visible;
                                txtCost.Height = 54;
                                txtCost.Visibility = Visibility.Visible;
                                btnPriceInfo.Visibility = Visibility.Visible;
                                txtStockInicial.Visibility = Visibility.Visible;
                                spStock.Visibility = Visibility.Visible;
                            }
                        };
                        wrapProductos.Children.Add(border);
                    }
                }
                else
                {
                    wrapProductos.Children.Clear();
                    if (!bNuevoCreado)
                    {
                        wrapProductos.Children.Add(spNuevo);
                        bNuevoCreado = true;
                    }
                }
            }
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            if (imgProducto._id != 0)
            {
                try
                {
                    var ws = new WindowInfoStock(imgProducto._id);
                    ws.Show();
                }
                catch (Exception)
                {
                }
            }
        }

        private void txtNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNombre.Text == "")
                btnBuscaImagen.IsEnabled = false;
            else
                btnBuscaImagen.IsEnabled = true;

            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenProducto"] + imgProducto._name);
                image.EndInit();
                imgProducto.Source = image;
            }
            catch (Exception)
            {
                imgProducto.Source = null;
            }
            imgProducto.ToolTip = imgProducto._name;
        }

        private void btnEditarCrear_Click(object sender, RoutedEventArgs e)
        {
            categoria cat = CategoriaBLL.Obtener(txtCategory.Text); // unique
            proveedore proveedor = ProveedorBLL.Obtener(txtSupplier.Text); // tomo cualquiera sin importar cuantos hayan

            var listaProductosPromo = new List<producto>();
            foreach (PromoItem_xs promoItem in spItemsPromo.Children)
            {
                listaProductosPromo.Add(promoItem.Producto);
            }

            producto p = new producto();
            p.nombre = txtNombre.Text;
            p.codigo_barras = txtBarCode.Text;
            p.precio = txtPrice.Text.Trim() != "" ? Convert.ToInt32(txtPrice.Text) : 0;
            p.puntos_cantidad = txtPoints.Text.Trim() != "" ? Convert.ToInt32(txtPoints.Text) : 0;
            // TODO - corregir, al editar el nombre borra la imagen
            p.imagen = imgProducto._name;
            if (proveedor != null)
                p.proveedor_id = proveedor.id;
            p.proveedor_id = null;


            //if (txtCategory._promoID == 0)
            //    p.promocion_id = null;
            //else
            //    p.promocion_id = txtCategory._promoID;

            if (btnEditarCrear.Content.Equals("CREAR"))
            {
                try
                {
                    if (txtCategory.Text.ToLower().Equals("promo") || txtCategory.Text.ToLower().Equals("promocion") || txtCategory.Text.ToLower().Equals("promoción"))
                    {
                        if (spItemsPromo.Children.Count == 0)
                        {
                            MessageBox.Show("Debes escoger los productos de la promo");
                            return;
                        }
                    }
                    ProductoBLL.Create(p);
                    string msg = CompraBLL.AddStockByProduct(txtNombre.Text, p.id, Convert.ToDecimal(txtCost.Text), Convert.ToDecimal(txtStockInicial.Text));
                    MessageBox.Show(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else // editar
            {
                try
                {

                    if (txtCategory.Text.ToLower().Equals("promo") || txtCategory.Text.ToLower().Equals("promocion") || txtCategory.Text.ToLower().Equals("promoción"))
                    {
                        //PromoBLL.Actualizar(p.promocion_id, listaProductosPromo);

                        ProductoBLL.Actualizar(p);
                        MessageBox.Show("Promoción actualizada");
                    }
                    else
                    {
                        ProductoBLL.Actualizar(p);

                        // Stock
                        // stockTmp = variable global que guarda el stock del producto seleccionado
                        decimal newStock = Convert.ToDecimal(txtStock.Text);
                        if (newStock == stockTmp)
                        {
                            MessageBox.Show("Listo!");
                            Console.WriteLine("Stock igual");
                        }
                        else if (newStock < stockTmp)
                        {
                            decimal dif = (stockTmp - newStock);
                            //string msg = DB.ReduceStock(imgProducto._id, dif);
                            //MessageBox.Show(msg);
                        }
                        else
                        {
                            decimal dif = (newStock - stockTmp);
                            string msg = CompraBLL.AddStockByProduct(txtNombre.Text, imgProducto._id, Convert.ToDecimal(txtCost.Text), dif);
                            MessageBox.Show(msg);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            RefreshScreen();

            /*
             * TODO, para mañana que ahora tengo sueño... (hecho, TODO - revisar)
             al mostrar un producto, guardar su stock en una variable
             al presionar el boton editar, calcular la diferencia de stock
             con tal de detectar si se agregó, se quitó o está igual
             si se agregó, crear una compra con el valor calculado
             si se quitó, quitar stock mediante fifo

            si estoy presionando el botón pero su content es "crear"
            verificar si existe el codigo de barras ingresado
            si no existe, se crea un producto y se crea una compra con el stock ingresado
            si existe, el evento text changed del codigo de barras lo va a detectar y va a desplegar la info y cambiar el content del botón a "editar"
            y al editarlo, se va a crear una compra o una reducción de stock segun la diferencia correspondiente al stock actual y el ingresado
            y además se va a actualizar la información del producto

            // purchases purchase = new purchases() { products = p, cost = Convert.ToDecimal(txtBuyPrice.Text), date = DateTime.Now, quantity = Convert.ToInt32(txtCantidad.Text) };
             */

        }

        void IDisposable.Dispose()
        {
        }
        //private void txtImagen_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    // bad use of try-catch but...
        //    try
        //    {
        //        BitmapImage image = new BitmapImage();
        //        image.BeginInit();
        //        image.UriSource = new Uri(pathImageProductsFromConfig + txtImage.Text);
        //        image.EndInit();
        //        imgProducto.Source = image;
        //    }
        //    catch (Exception)
        //    {
        //        imgProducto.Source = null;
        //    }
        //}
    }
}
