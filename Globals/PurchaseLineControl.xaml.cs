using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;

namespace posk.Globals
{
    public partial class PurchaseLineControl : UserControl
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }

        public string TotalNeto
        {
            get { return txtTotalNeto.Text; }
            set { txtTotalNeto.Text = value; }
        }

        public string CostoUnitarioNeto
        {
            get { return txtCostoUnitarioNeto.Text; }
            set { txtCostoUnitarioNeto.Text = value; }
        }

        public string ProductoDesc
        {
            get { return lbProducto.Content.ToString(); }
            set { lbProducto.Content = value; }
        }

        public PurchaseLineControl()
        {
            InitializeComponent();


        }
    }
}
