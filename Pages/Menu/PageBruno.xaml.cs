using posk.BLL;
using posk.Controls;
using posk.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageBruno : Page
    {
        public PageBruno()
        {
            InitializeComponent();

            Loaded += (se, a) => 
            {
                spPedidos.Children.Add(new ItemPendienteMesa());
                spPedidos.Children.Add(new ItemPendienteMesa());
                spPedidos.Children.Add(new ItemPendienteMesa());
                spPedidos.Children.Add(new ItemPendienteMesa());
                spPedidos.Children.Add(new ItemPendienteMesa());
            };
        }
    }
}