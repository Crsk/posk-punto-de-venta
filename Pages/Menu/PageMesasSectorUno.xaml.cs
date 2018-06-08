using posk.BLL;
using posk.Components;
using posk.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageMesasSectorUno : Page
    {
        public PageMesasSectorUno()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                MesaBLL.ObtenerTodasPorSector(1).ForEach(m => 
                {
                    var itemMesa = new ItemMesa();
                    itemMesa.Mesa = m;
                    itemMesa.Libre = m.libre;
                    itemMesa.btnMesa.Click += (se2, a2) => { MainWindow.MainFrame.Content = new PrincipalComponent("VENTA", m); };
                    wrapMesas.Children.Add(itemMesa);
                });
            };
        }
    }
}
