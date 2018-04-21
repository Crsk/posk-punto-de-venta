using posk.BLL;
using posk.Globals;
using posk.Models;
using posk.Pages.PopUp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace posk.Components
{
    public partial class PageAdministrarProducto : UserControl, IDisposable
    {
        public string Modo { get; set; }
        string pathImageProductsFromConfig;
        // item nuevo producto
        StackPanel spNuevo;
        Button btnNuevo, btnEliminar;
        Label lbNuevo;
        //int productSelectedId = 0;
        decimal stockTmp;
        int iva = Convert.ToInt32(ConfigurationManager.AppSettings["IVA"]);
        bool bEliminarCreado = false, bNuevoCreado = false;

        public PageAdministrarProducto(string modo) // PRODUCTO, CLIENTE, USUARIO
        {
            InitializeComponent();
            pathImageProductsFromConfig = ConfigurationManager.AppSettings["RutaImagenProducto"];
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
                Background = new SolidColorBrush(Color.FromRgb(33, 33, 33)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(33, 33, 33)),
                Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240))
            };
            btnEliminar.Click += (se, ev) =>
            {
                DB.Delete(txtBarCode.Text);
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
                btnEditarCrear.Content = "CREAR";
                if (!spProductItems2.Children.Contains(txtCost))
                    spProductItems2.Children.Insert(0, txtCost);
                if (spProductItems1.Children.Contains(spStock))
                    spProductItems1.Children.Remove(spStock);
                if (!spProductItems1.Children.Contains(txtStockInicial))
                    spProductItems1.Children.Insert(2, txtStockInicial);
                RefreshScreen();
            };

            if (!bNuevoCreado)
            {
                wrapProductos.Children.Add(spNuevo);
                bNuevoCreado = true;
            }

            btnCategory.Click += (se, ev) => { var wc = new WindowChooseCategory(txtCategory); wc.Show(); };
            btnSubCategory.Click += (se, ev) => { var wsc = new WindowChooseSubCategory(txtSubCategory); wsc.Show(); };
            btnProveedor.Click += (se, ev) => { var wp = new WindowChooseSupplier(txtSupplier); wp.Show(); };
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
            txtSubCategory.Clear();
            txtSupplier.Clear();
        }

        private void txtImagen_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void btnBuscaImagen_Click(object sender, RoutedEventArgs e)
        {
            var wi = new WindowChooseOrTakeImage("products", imgProducto, txtNombre.Text);
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
            DataTable dt = new DataTable();


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


                listaProductos = DB.GetProducts(txtBuscar.Text);

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
                            img = new Image() { Source = new BitmapImage(new Uri("http://www.lacasadelasnavajas.com/img/cms/recursos/producto-exclusivo.png")) };
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
                            btnEditarCrear.Content = "ACTUALIZAR";
                            imgProducto._id = productItemFromSearchWrap.Id;
                            imgProducto._name = productItemFromSearchWrap.ProductName + ".jpg";
                            txtCategory._id = productItemFromSearchWrap.CategoryId;
                            txtSubCategory._id = productItemFromSearchWrap.SubCategoryId;
                            txtSupplier._id = productItemFromSearchWrap.SupplierId;

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
                                image.UriSource = new Uri(pathImageProductsFromConfig + productItemFromSearchWrap.Image);
                                image.EndInit();
                                imgProducto.Source = image;
                            }
                            catch (Exception)
                            {
                                imgProducto.Source = null;
                            }
                            imgProducto.ToolTip = productItemFromSearchWrap.Image;

                            txtSubCategory.Text = DB.GetSubCategoryName(Convert.ToInt32(p.sub_categoria_id));
                            txtSupplier.Text = DB.GetSupplierName(Convert.ToInt32(p.proveedor_id));
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
                var ws = new WindowInfoStock(imgProducto._id);
                ws.Show();
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
            categoria category = new categoria() { id = txtCategory._id, nombre = txtCategory.Text };
            subcategoria subCategory = new subcategoria() { id = txtSubCategory._id, nombre = txtSubCategory.Text };
            proveedore supplier = new proveedore() { id = txtSupplier._id, nombre = txtSupplier.Text };

            producto p = new producto();
            p.nombre = txtNombre.Text;
            p.codigo_barras = txtBarCode.Text;
            p.precio = txtPrice.Text.Trim() != "" ? Convert.ToInt32(txtPrice.Text) : 0;
            p.puntos_cantidad = txtPoints.Text.Trim() != "" ? Convert.ToInt32(txtPoints.Text) : 0;
            p.imagen = imgProducto._name;
            p.sub_categoria_id = txtSubCategory._id;
            p.proveedor_id = txtSupplier._id;

            if (btnEditarCrear.Content.Equals("CREAR"))
            {
                //DB.Insert<producto>("products", p);
                ProductoBLL.Create(p);
                string msg = CompraBLL.AddStockByProduct(txtNombre.Text, p.id, Convert.ToDecimal(txtCost.Text), Convert.ToDecimal(txtStockInicial.Text));
                MessageBox.Show(msg);
            }
            else
            {
                DB.UpdateProduct(p);

                // Stock
                decimal newStock = Convert.ToDecimal(txtStock.Text);
                if (newStock == stockTmp)
                {
                    MessageBox.Show("Stock igual");
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
    }
}
