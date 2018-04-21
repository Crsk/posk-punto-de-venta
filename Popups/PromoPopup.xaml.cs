using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace posk.Popup
{
    public partial class PromoPopup : UserControl
    {
        public event EventHandler<List<PromoItem>> OnSelect;
        private List<PromoItem> listaPromo;

        public PromoPopup()
        {
            InitializeComponent();
            listaPromo = new List<PromoItem>();

            //cbProductos.ItemsSource = ProductoBLL.ObtenerNoPromos();
            //cbProductos.DisplayMemberPath = "nombre";
            btnAgregar.Click += (se, a) =>
            {
                producto p = (producto)cbProductos.SelectedItem;
                if (p != null)
                {
                    PromoItem pi = new PromoItem();
                    pi.Producto = p;
                    pi.txtNombreProducto.Content = p.nombre;
                    spItems.Children.Add(pi);
                    pi.btnEliminar.Click += (se2, a2) => { spItems.Children.Remove(pi); listaPromo.Remove(pi); };
                    listaPromo.Add(pi);
                }
            };
            btnListo.Click += (se, a) => OnSelect(this, listaPromo);
        }
    }
}
