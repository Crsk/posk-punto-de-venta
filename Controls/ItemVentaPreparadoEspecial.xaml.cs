using posk.BLL;
using posk.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Controls
{
    public partial class ItemVentaPreparadoEspecial : UserControl
    {
        private producto producto;
        public producto Producto
        {
            get { return producto; }
            set { producto = value; lbItemVenta.Content = value.nombre; tbTotal.Text = $"{value.precio}"; }
        }

        private List<preparacione> list;
        public List<preparacione> ListaItemsPreparado
        {
            get { return list; }
            set
            {
                list = value;
                value.ForEach(p =>
                {
                    tbAgregados.Text += $" {p.nombre.ToUpper()}";
                });
            }
        }

        public ItemVentaPreparadoEspecial()
        {
            InitializeComponent();
        }

        public int? ObtenerTotal()
        {
            return producto?.precio;
        }
    }
}
