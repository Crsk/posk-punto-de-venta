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
        private List<envoltura> ListaEnvolturasSeleccionadas;
        public producto Producto { get; set; }
        public int Total { get; set; }

        public ArmarTablaPopup(producto p)
        {
            InitializeComponent();
            ListaRollosArmados = new List<ItemVenta>();
            ListaEnvolturasSeleccionadas = new List<envoltura>();
            Producto = p;
            Total = p.precio;

            Deactivated += (se, ev) => { if (!bCerrado) Close(); };

            Loaded += (se, a) =>
            {
                wrapRollosTabla.Children.Add(new ItemRolloArmarTabla());
                lbTotal.Content = $"Total: {Total}";
                int? cantidadRollos = p.cantidad_rollos_tabla;
                while (cantidadRollos > 1)
                {
                    var irat = new ItemRolloArmarTabla();
                    irat.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(irat);
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

        private void CbEnvolturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListaEnvolturasSeleccionadas.Clear();
            wrapRollosTabla.Children.OfType<ItemRolloArmarTabla>().ToList().ForEach(x => 
            {
                if ((x.cbEnvolturas.SelectedItem as envoltura) != null)
                    ListaEnvolturasSeleccionadas.Add(x.cbEnvolturas.SelectedItem as envoltura);
            });

            if (ListaEnvolturasSeleccionadas.Count != 0)
            {
                if (Producto.cantidad_rollos_tabla == 2)
                {
                    // envoltura normal: 1 rollo furay, 1 rollo palta
                    List<envoltura> listaPalta = ListaEnvolturasSeleccionadas.Where(x => x.nombre.ToLower().Equals("palta")).ToList();
                    if (listaPalta.Count == 2) // palta está repetido (el que corresponde a furay es palta, por ende suma $500)
                        Total = Producto.precio + 500;
                    lbTotal.Content = $"Total: {Total}";
                }
                if (Producto.cantidad_rollos_tabla == 3)
                {
                    // envoltura normal: 1 rollo furay, 1 rollo queso o palta, 1 rollo california
                    List<envoltura> listaQuesoPalta = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("palta") || x.nombre.ToLower().Equals("queso crema")).ToList();
                    List<envoltura> listaCaliforniaNori = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("california") || x.nombre.ToLower().Equals("nori")).ToList();
                    List<envoltura> listaFuray = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("furay")).ToList();
                    if (listaQuesoPalta.Count == 3) // los 3 rollos son de palta o queso
                    {
                        // pasar de calfornia o nori a queso o palta suma $1500
                        // pasar de furay a queso o palta suma $500
                        // total a sumar como extra es de $2500
                        Total = Producto.precio + 2500;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o Nori -> Queso o Palta: +$1500, Furay -> Queso o Palta: +$500)";
                    }
                    else if (listaQuesoPalta.Count == 2 && listaFuray.Count == 1)
                    {
                        // pasar de california o nori a palta o queso suma $1500
                        Total = Producto.precio + 1500;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o Nori -> Queso o Palta: +$1500)";
                    }

                    else if (listaQuesoPalta.Count == 2 && listaCaliforniaNori.Count == 1)
                    {
                        // pasar de furay a palta o queso suma $500
                        Total = Producto.precio + 500;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, Furay -> Queso o Palta: +$500)";
                    }
                    else if (listaFuray.Count == 2 && listaQuesoPalta.Count == 1)
                    {
                        // pasar de california o nori a furay suma $1000
                        Total = Producto.precio + 1000;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o nori -> Furay: +$1000)";
                    }
                    else if (listaFuray.Count == 3)
                    {
                        // subir de california o nori a furay y bajar de palta a furay suma $500 y resta $500? ¿queda igual?
                    }
                    else
                    {
                        Total = Producto.precio;
                        lbTotal.Content = $"Total: {Total}";
                    }
                }
                if (Producto.cantidad_rollos_tabla == 4)
                {

                }
                if (Producto.cantidad_rollos_tabla == 5)
                {

                }
                if (Producto.cantidad_rollos_tabla == 6)
                {

                }
                if (Producto.cantidad_rollos_tabla == 10)
                {

                }
            }
        }

        private void CalcularPrecio()
        {

        }

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
