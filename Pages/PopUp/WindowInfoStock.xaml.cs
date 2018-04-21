using posk.BLL;
using posk.Globals;
using posk.Models;
using posk.Partials;
using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace posk.Pages.PopUp
{
    public partial class WindowInfoStock : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        int productId;

        public WindowInfoStock(int productId)
        {
            InitializeComponent();
            this.productId = productId;
            DateTime d = System.DateTime.Now;
            dpDate.Text = d.ToString("dd/MM/yyyy");

            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
            dgPurchases.ItemsSource = CompraProductoBLL.ObtainPurchaseDetails(productId);
        }

        private void btnAddPurchase_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = dpDate.SelectedDate.Value;
            compra c = CompraBLL.CreatePurchase(selectedDate, Settings.Usuario.id);
            CompraProductoBLL.Create(c, productId, Convert.ToDecimal(txtCost.Text), Convert.ToDecimal(txtQuantity.Text));
            DateTime d = DateTime.Now;
            dpDate.Text = d.ToString("dd/MM/yyyy");
            txtCost.Clear();
            txtQuantity.Clear();
            dgPurchases.ItemsSource = CompraProductoBLL.ObtainPurchaseDetails(productId);
        }

        void DeleteRow(object sender, RoutedEventArgs e)
        {
            int id = (dgPurchases.SelectedItem as ProductPurchaseDetails).id;
            CompraProductoBLL.Delete(id);
            dgPurchases.ItemsSource = CompraProductoBLL.ObtainPurchaseDetails(productId);
        }
    }
}
