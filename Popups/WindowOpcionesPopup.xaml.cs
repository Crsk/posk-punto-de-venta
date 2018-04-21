using posk.Controls;
using System.Collections.Generic;
using System.Windows;

namespace posk.Popup
{
    public partial class WindowOpcionesPopup : Window
    {
        public WindowOpcionesPopup(List<ItemAccion> lista, string titulo)
        {
            InitializeComponent();
            lbTitulo.Content = titulo;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lista.ForEach(x =>
            {
                spAccion.Children.Add(x);
                x.btnAccion.Click += (se, a) => this.Close();
            });
        }
    }
}
