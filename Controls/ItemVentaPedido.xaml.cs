using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemVentaPedido : UserControl, IDisposable
    {
        public event EventHandler AlEliminar;

        public int PedidoProductoID { get; set; }
        public decimal? Cantidad { get; set; }
        private producto producto;

        public producto Producto
        {
            get { return producto; }
            set { producto = value; lbDetalle.Content = value.nombre; }
        }

        private promocione promocione;

        public promocione Promo
        {
            get { return promocione; }
            set { promocione = value; lbDetalle.Content = value.nombre; }
        }

        public ItemVentaPedido()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                lbDetalle.Content = Producto.nombre.ToUpper();
                lbCantidad.Content = $"x{Cantidad}";
                if (Cantidad % 1 == 0)
                    lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            };
            btnAgregarUnidad.Click += (se, a) => AgregarCantidad(1);
            btnQuitarUnidad.Click += (se, a) =>
            {
                AgregarCantidad(-1);
                if (Cantidad == 0)
                    AlEliminar(this, null);
            };
        }

        public void AsignarCantidad(decimal? cantidad)
        {
            Cantidad = cantidad;
            if (Cantidad % 1 == 0)
                lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            else
                lbCantidad.Content = Cantidad;
        }

        public void AgregarCantidad(decimal? cantidad)
        {
            Cantidad += cantidad;
            if (Cantidad % 1 == 0)
                lbCantidad.Content = $"x{Convert.ToInt32(Cantidad)}";
            else
                lbCantidad.Content = Cantidad;
        }

        void IDisposable.Dispose()
        {
        }
    }
}
