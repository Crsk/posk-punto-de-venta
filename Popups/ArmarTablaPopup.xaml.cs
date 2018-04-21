using posk.Controls;
using posk.Models;
using posk.Partials;
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
using System.Windows.Shapes;

namespace posk.Popups
{
    public partial class ArmarTablaPopup : Window
    {
        public bool bCerrado = false;
        public event EventHandler<ItemVenta> AlTerminarArmado;
        private List<ItemVenta> ListaRollosArmados;

        public ArmarTablaPopup(producto p)
        {
            InitializeComponent();
            ListaRollosArmados = new List<ItemVenta>();

            Deactivated += (se, ev) => { if (!bCerrado) Close(); };

            Loaded += (se, a) =>
            {
                wrapRollosTabla.Children.Add(new ItemRolloArmarTabla());
                int? cantidadRollos = p.cantidad_rollos_tabla;
                while (cantidadRollos > 1)
                {
                    wrapRollosTabla.Children.Add(new ItemRolloArmarTabla());
                    cantidadRollos--;
                }
            };


            btnListo.Click += (se, a) =>
            {
                producto _producto = p;
                wrapRollosTabla.Children.OfType<ItemRolloArmarTabla>().ToList().ForEach(irat =>
                {
                    List<ItemAgregadoHandroll> _listaItemsAgregadoHandroll = new List<ItemAgregadoHandroll>();
                    irat.spAgregados.Children.OfType<ItemAgregadoHandroll>().Where(iah => iah.Cantidad > 0).ToList().ForEach(iah => _listaItemsAgregadoHandroll.Add(iah));

                    envoltura _envoltura = new envoltura();
                    _envoltura = (irat.cbEnvolturas.SelectedItem as envoltura);


                    ListaRollosArmados.Add(new ItemVenta() { listaAgregadosSushi = _listaItemsAgregadoHandroll, Envoltura = _envoltura, Producto = _producto });
                });
                ItemVenta tabla = new ItemVenta() { Producto = _producto, ListaRollosTabla = ListaRollosArmados };
                AlTerminarArmado.Invoke(this, tabla);
            };
        }

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
