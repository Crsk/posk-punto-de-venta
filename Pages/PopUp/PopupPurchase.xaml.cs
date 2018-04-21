using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace posk.Pages.PopUp
{
    public partial class PopupPurchase : Window, INotifyPropertyChanged
    {
        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CalcularTotales()
        {
            subTotal = 0;
            foreach (PurchaseLineControl item in spPurchases.Children)
            {
                if (item.TotalNeto.Trim() != "")
                    subTotal += Convert.ToDecimal(item.TotalNeto);
            }

            iva = subTotal * (DatosNegocioBLL.IvaPctDecimal__0_iva());
            total = subTotal + iva;

            subTotal = decimal.Round(subTotal, 2, MidpointRounding.AwayFromZero);
            iva = decimal.Round(iva, 2, MidpointRounding.AwayFromZero);
            total = decimal.Round(total, 2, MidpointRounding.AwayFromZero);

            SubTotal = subTotal;
            IVA = iva;
            Total = total;
        }
        #endregion Methods

        #region Variables Globales
        decimal ivaPctDecimal__1_iva;

        private decimal subTotal;
        public decimal SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; OnPropertyChanged("SubTotal"); }
        }

        private decimal iva;
        public decimal IVA
        {
            get { return iva; }
            set { iva = value;  OnPropertyChanged("IVA"); }
        }

        private decimal ila;
        public decimal ILA
        {
            get { return ila; }
            set { ila = value; OnPropertyChanged("ILA"); }
        }

        private decimal descuento;
        public decimal Descuento
        {
            get { return descuento; }
            set { descuento = value; OnPropertyChanged("Descuento"); }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged("Total"); }
        }


        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        StackPanel spProductList;
        #endregion Variables Globales

        #region Init
        public PopupPurchase(StackPanel spProductList)
        {
            InitializeComponent();
            ivaPctDecimal__1_iva = DatosNegocioBLL.IvaPctDecimal__1_iva();
            this.spProductList = spProductList;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
            Start();
            DataContext = this;
        }
        private void Start()
        {
            foreach (ItemVenta item in spProductList.Children)
            {
                PurchaseLineControl plc = new PurchaseLineControl()
                {
                    Id = item.ProductoID,
                    Quantity = (int)item.Cantidad,
                    ProductoDesc = $"x{item.Cantidad}    {item.Producto.nombre}",
                    ToolTip = $"Precio de venta ${item.Producto.precio} C/U"
                };

                plc.txtTotalBruto.GotFocus += (se, ev) =>
                {
                    if (plc.txtTotalBruto.Text == "0")
                        plc.txtTotalBruto.Text = "";
                };
                plc.txtTotalBruto.MouseDown += (se, ev) =>
                {
                    if (plc.txtTotalBruto.Text == "0")
                        plc.txtTotalBruto.Text = "";
                };
                plc.txtTotalBruto.TextChanged += (se, ev) =>
                {
                    if (escape)
                        return;
                    escape = true;
                    TextBox tb = (TextBox)se;
                    if (tb.Text.Trim() != "")
                    {
                        decimal costoTotalBruto = Convert.ToDecimal(tb.Text);
                        Math.Round(costoTotalBruto, 1);
                        plc.txtCostoUnitarioNeto.Text = $"{decimal.Round(((costoTotalBruto / (ivaPctDecimal__1_iva)) / plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtCostoUnitarioBruto.Text = $"{decimal.Round(((costoTotalBruto) / plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtTotalNeto.Text = $"{decimal.Round(((costoTotalBruto / (ivaPctDecimal__1_iva))), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        CalcularTotales();
                    }
                    else
                    {
                        plc.txtCostoUnitarioNeto.Text = "0";
                        plc.txtCostoUnitarioBruto.Text = "0";
                        plc.txtTotalNeto.Text = "0";
                        tb.SelectionStart = 1;
                        CalcularTotales();
                    }
                    escape = false;
                };

                plc.txtTotalNeto.GotFocus += (se, ev) =>
                {
                    if (plc.txtTotalNeto.Text == "0")
                        plc.txtTotalNeto.Text = "";
                };
                plc.txtTotalNeto.MouseDown += (se, ev) =>
                {
                    if (plc.txtTotalNeto.Text == "0")
                        plc.txtTotalNeto.Text = "";
                };
                plc.txtTotalNeto.TextChanged += (se, ev) =>
                {
                    if (escape)
                        return;
                    escape = true;
                    TextBox tb = (TextBox)se;
                    if (tb.Text.Trim() != "")
                    {
                        decimal costoTotalNeto = Convert.ToDecimal(tb.Text);
                        Math.Round(costoTotalNeto, 1);
                        plc.txtCostoUnitarioNeto.Text = $"{decimal.Round((costoTotalNeto / plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtCostoUnitarioBruto.Text = $"{decimal.Round(((costoTotalNeto * (ivaPctDecimal__1_iva)) / plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtTotalBruto.Text = $"{decimal.Round(((costoTotalNeto * (ivaPctDecimal__1_iva)) * plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        CalcularTotales();
                    }
                    else
                    {
                        plc.txtCostoUnitarioNeto.Text = "0";
                        plc.txtCostoUnitarioBruto.Text = "0";
                        plc.txtTotalBruto.Text = "0";
                        tb.SelectionStart = 1;
                        CalcularTotales();
                    }
                    escape = false;
                };

                plc.txtCostoUnitarioBruto.GotFocus += (se, ev) =>
                {
                    if (plc.txtCostoUnitarioBruto.Text == "0")
                        plc.txtCostoUnitarioBruto.Text = "";
                };
                plc.txtCostoUnitarioBruto.MouseDown += (se, ev) =>
                {
                    if (plc.txtCostoUnitarioBruto.Text == "0")
                        plc.txtCostoUnitarioBruto.Text = "";
                };
                plc.txtCostoUnitarioBruto.TextChanged += (se, ev) =>
                {
                    if (escape)
                        return;
                    escape = true;
                    TextBox tb = (TextBox)se;
                    if (tb.Text.Trim() != "")
                    {
                        decimal costoUnitarioBruto = Convert.ToDecimal(tb.Text);
                        Math.Round(costoUnitarioBruto, 1);
                        plc.txtTotalBruto.Text = $"{decimal.Round((costoUnitarioBruto * plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtTotalNeto.Text = $"{decimal.Round(((costoUnitarioBruto / (ivaPctDecimal__1_iva)) * plc.Quantity) , 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtCostoUnitarioNeto.Text = $"{decimal.Round((costoUnitarioBruto / (ivaPctDecimal__1_iva)), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        CalcularTotales();
                    }
                    else
                    {
                        plc.txtTotalBruto.Text = "0";
                        plc.txtTotalNeto.Text = "0";
                        plc.txtCostoUnitarioNeto.Text = "0";
                        tb.SelectionStart = 1;
                        CalcularTotales();
                    }
                    escape = false;
                };

                plc.txtCostoUnitarioNeto.GotFocus += (se, ev) =>
                {
                    if (plc.txtCostoUnitarioNeto.Text == "0")
                        plc.txtCostoUnitarioNeto.Text = "";
                };
                plc.txtCostoUnitarioNeto.MouseDown += (se, ev) =>
                {
                    if (plc.txtCostoUnitarioNeto.Text == "0")
                        plc.txtCostoUnitarioNeto.Text = "";
                };
                plc.txtCostoUnitarioNeto.TextChanged += (se, ev) =>
                {
                    if (escape)
                        return;
                    escape = true;
                    TextBox tb = (TextBox)se;
                    if (tb.Text.Trim() != "")
                    {
                        decimal costoUnitarioNeto = Convert.ToDecimal(tb.Text);
                        Math.Round(costoUnitarioNeto, 1);
                        plc.txtTotalBruto.Text = $"{decimal.Round((costoUnitarioNeto * (ivaPctDecimal__1_iva) * plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtTotalNeto.Text = $"{decimal.Round((costoUnitarioNeto * plc.Quantity), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        plc.txtCostoUnitarioBruto.Text = $"{decimal.Round((costoUnitarioNeto * (ivaPctDecimal__1_iva)), 2, MidpointRounding.AwayFromZero).ToString("F")}";
                        CalcularTotales();
                    }
                    else
                    {
                        plc.txtTotalBruto.Text = "0";
                        plc.txtTotalNeto.Text = "0";
                        plc.txtCostoUnitarioBruto.Text = "0";
                        tb.SelectionStart = 1;
                        CalcularTotales();
                    }
                    escape = false;
                };

                spPurchases.Children.Add(plc);
            }
            btnCrearCompra.Click += (se, ev) =>
            {
                compra c = CompraBLL.CreatePurchase(DateTime.Now, Settings.Usuario.id);

                foreach (PurchaseLineControl plc in spPurchases.Children)
                {
                    CompraProductoBLL.Create(c, plc.Id, Convert.ToDecimal(plc.txtCostoUnitarioNeto.Text), plc.Quantity);
                }
            };
        }
        bool escape = false;
        #endregion Init
    }
}
