using posk.Entities;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using posk.Globals;
using System;
using posk.BLL;

namespace posk.Pages.PopUp
{
    public partial class WindowChooseCategory : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        ItemName_TextBox tbCategory = new ItemName_TextBox();
        //CategoriaBLL categoryBLL = new CategoriaBLL();
        public WindowChooseCategory(ItemName_TextBox tbCategory)
        {
            InitializeComponent();
            this.tbCategory = tbCategory;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };

            //dgCategory.ItemsSource = categoryBLL.GetAll();

            this.tbCategory = tbCategory;
        }

        private void dgCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            if (dg.SelectedCells.Count == 1)
            {
                Models.categoria selection = (Models.categoria)dgCategory.SelectedItem;
                tbCategory.Text = selection.nombre;
                tbCategory._id = selection.id;
                bCerrado = true; Close();
            }
        }
    }
}
