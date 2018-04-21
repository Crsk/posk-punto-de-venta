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
    public partial class ItemSeleccionarPendienteParaVenta : UserControl
    {
        private bool seleccionado;

        public bool Seleccionado
        {
            get { return seleccionado; }
            set { seleccionado = value; }
        }

        public pedidos_productos PedidoProducto { get; set; }
        public pedidos_agregados PedidoAgregado { get; set; }
        public pedidos_preparaciones PedidoPreparacion { get; set; }
        public producto Producto { get; set; }
        public promocione Promo { get; set; }

        public agregado AgregadoUno { get; set; }
        public agregado AgregadoDos { get; set; }
        public preparacione Preparacion { get; set; }

        public int Precio { get; set; }

        public ItemSeleccionarPendienteParaVenta()
        {
            InitializeComponent();

            Loaded += (se, a) => 
            {
                if (Producto != null)
                {
                    lbNombre.Text = $"x{PedidoProducto.cantidad}   ${Precio}   {Producto?.nombre}";
                    PedidoProducto.producto = Producto;
                }
                else if (Promo != null)
                    lbNombre.Text = $"{Promo?.nombre}";
                checkSeleccion.IsChecked = Seleccionado == true ? true : false;
                if (PedidoProducto != null)
                {
                    PedidoAgregado = PedidosAgregadosBLL.Obtener(PedidoProducto.id);
                    PedidoPreparacion = PedidosPreparacionesBLL.Obtener(PedidoProducto.id);
                    AgregadoUno = PedidoAgregado?.agregado;
                    AgregadoDos = PedidoAgregado?.agregado1;
                    Preparacion = PedidoPreparacion?.preparacione;

                    if (AgregadoUno != null) lbNombre.Text += $" con {AgregadoUno.nombre}";
                    if (AgregadoDos != null) lbNombre.Text += $" y {AgregadoDos.nombre}";
                    if (Preparacion != null) lbNombre.Text += $" - {Preparacion.nombre}";
                }
            };

            checkSeleccion.Checked += (se, a) => 
            {
                Seleccionado = true;
            };
            checkSeleccion.Unchecked += (se, a) =>
            {
                Seleccionado = false;
            };
        }
    }
}
