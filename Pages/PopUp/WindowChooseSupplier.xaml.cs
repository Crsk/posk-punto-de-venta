using System.Windows;
using System.Windows.Controls;
using posk.Entities;
using posk.Globals;
using System.Data;
using System;
using posk.BLL;

namespace posk.Pages.PopUp
{
    public partial class WindowChooseSupplier : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        ItemName_TextBox tbSupplier = new ItemName_TextBox();

        public WindowChooseSupplier(ItemName_TextBox tbSupplier)
        {
            InitializeComponent();
            this.tbSupplier = tbSupplier;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };

            dgSupplier.ItemsSource = ProveedorBLL.GetAll();
            this.tbSupplier = tbSupplier;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
        }

        private void dgProveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            if (dg.SelectedCells.Count == 1)
            {
                Models.proveedore selection = (Models.proveedore)dgSupplier.SelectedItem;
                tbSupplier.Text = selection.nombre;
                tbSupplier._id = selection.id;
                bCerrado = true; Close();
            }
        }
    }
}
