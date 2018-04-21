using posk.BLL;
using posk.Entities;
using posk.Globals;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace posk.Pages.PopUp
{
    public partial class WindowChooseSubCategory : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        ItemName_TextBox tbSubCategory = new ItemName_TextBox();
        //SubCategoriaBLL subCategoryBLL = new SubCategoriaBLL();

        public WindowChooseSubCategory(ItemName_TextBox tbSubCategory)
        {
            InitializeComponent();
            this.tbSubCategory = tbSubCategory;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };

            //dgSubCategory.ItemsSource = subCategoryBLL.GetAll();
            this.tbSubCategory = tbSubCategory;
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
        }

        private void dgSubCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            if (dg.SelectedCells.Count == 1)
            {
                Models.subcategoria selection = (Models.subcategoria)dgSubCategory.SelectedItem;
                tbSubCategory.Text = selection.nombre;
                tbSubCategory._id = selection.id;
                bCerrado = true; Close();
            }
        }
    }
}
