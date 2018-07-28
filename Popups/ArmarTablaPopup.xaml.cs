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
        public int TotalTabla { get; set; }

        public ArmarTablaPopup(producto p)
        {
            InitializeComponent();
            ListaRollosArmados = new List<ItemVenta>();
            ListaEnvolturasSeleccionadas = new List<envoltura>();
            Producto = p;
            TotalTabla = p.precio;

            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            /*
            Loaded += (se, a) =>
            {
                lbTotal.Content = $"Total: {TotalTabla}";

                if (p.cantidad_rollos_tabla == 2)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);

                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "PALTA";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Palta";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);
                }
                if (p.cantidad_rollos_tabla == 3)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);

                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "QUESO CREMA";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);

                    var iratTres = new ItemRolloArmarTabla();
                    iratTres.EnvolturaPorDefecto = "CALIFORNIA";
                    iratTres.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratTres.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratTres);
                }
                if (p.cantidad_rollos_tabla == 4)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);

                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "QUESO CREMA";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);

                    var iratTres = new ItemRolloArmarTabla();
                    iratTres.EnvolturaPorDefecto = "CALIFORNIA";
                    iratTres.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratTres.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratTres);

                    var iratCuatro = new ItemRolloArmarTabla();
                    iratCuatro.EnvolturaPorDefecto = "NORI";
                    iratCuatro.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Nori";
                    iratCuatro.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCuatro);
                }
                if (p.cantidad_rollos_tabla == 5)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);

                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "FURAY";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);

                    var iratTres = new ItemRolloArmarTabla();
                    iratTres.EnvolturaPorDefecto = "QUESO CREMA";
                    iratTres.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratTres.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratTres);

                    var iratCuatro = new ItemRolloArmarTabla();
                    iratCuatro.EnvolturaPorDefecto = "CALIFORNIA";
                    iratCuatro.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratCuatro.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCuatro);

                    var iratCinco = new ItemRolloArmarTabla();
                    iratCinco.EnvolturaPorDefecto = "NORI";
                    iratCinco.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Nori";
                    iratCinco.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCinco);
                }
                if (p.cantidad_rollos_tabla == 6)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);

                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "FURAY";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);

                    var iratTres = new ItemRolloArmarTabla();
                    iratTres.EnvolturaPorDefecto = "QUESO CREMA";
                    iratTres.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratTres.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratTres);

                    var iratCuatro = new ItemRolloArmarTabla();
                    iratCuatro.EnvolturaPorDefecto = "NORI";
                    iratCuatro.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Nori";
                    iratCuatro.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCuatro);

                    var iratCinco = new ItemRolloArmarTabla();
                    iratCinco.EnvolturaPorDefecto = "CALIFORNIA";
                    iratCinco.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratCinco.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCinco);

                    var iratSeis = new ItemRolloArmarTabla();
                    iratSeis.EnvolturaPorDefecto = "CALIFORNIA";
                    iratSeis.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratSeis.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratSeis);
                }
                if (p.cantidad_rollos_tabla == 10)
                {
                    var iratUno = new ItemRolloArmarTabla();
                    iratUno.EnvolturaPorDefecto = "FURAY";
                    iratUno.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratUno.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratUno);
                    var iratDos = new ItemRolloArmarTabla();
                    iratDos.EnvolturaPorDefecto = "FURAY";
                    iratDos.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratDos.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDos);
                    var iratTres = new ItemRolloArmarTabla();
                    iratTres.EnvolturaPorDefecto = "FURAY";
                    iratTres.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Furay";
                    iratTres.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratTres);

                    var iratCuatro = new ItemRolloArmarTabla();
                    iratCuatro.EnvolturaPorDefecto = "QUESO CREMA";
                    iratCuatro.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratCuatro.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCuatro);
                    var iratCinco = new ItemRolloArmarTabla();
                    iratCinco.EnvolturaPorDefecto = "QUESO CREMA";
                    iratCinco.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Queso o Palta";
                    iratCinco.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratCinco);

                    var iratSeis = new ItemRolloArmarTabla();
                    iratSeis.EnvolturaPorDefecto = "CALIFORNIA";
                    iratSeis.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratSeis.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratSeis);
                    var iratSiete = new ItemRolloArmarTabla();
                    iratSiete.EnvolturaPorDefecto = "CALIFORNIA";
                    iratSiete.MensajeEnvolturaPorDefecto = "Envoltura por defecto: California";
                    iratSiete.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratSiete);

                    var iratOcho = new ItemRolloArmarTabla();
                    iratOcho.EnvolturaPorDefecto = "NORI";
                    iratOcho.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Nori";
                    iratOcho.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratOcho);
                    var iratNueve = new ItemRolloArmarTabla();
                    iratNueve.EnvolturaPorDefecto = "NORI";
                    iratNueve.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Nori";
                    iratNueve.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratNueve);


                    var iratDiez = new ItemRolloArmarTabla();
                    iratDiez.EnvolturaPorDefecto = "CIBOULETTE";
                    iratDiez.MensajeEnvolturaPorDefecto = "Envoltura por defecto: Ciboulette";
                    iratDiez.cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
                    wrapRollosTabla.Children.Add(iratDiez);
                }
            };
            */

            btnCerrar.Click += (se, a) =>
            {
                bCerrado = true;
                Close();
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

                ItemVenta tabla = new ItemVenta() { Producto = _producto, ListaRollosTabla = ListaRollosArmados, TotalIV = TotalTabla };
                AlTerminarArmado.Invoke(this, tabla);
            };
        }

        private void CbEnvolturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TotalTabla = Producto.precio;
            wrapRollosTabla.Children.OfType<ItemRolloArmarTabla>().ToList().ForEach(x => TotalTabla += x.CobroAdicional);
            lbTotal.Content = $"Total: {TotalTabla}";
        }
        
        /*
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
                    {
                        // pasar de furay a palta suma $500
                        Total = Producto.precio + 500;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, Furay -> Queso o Palta: +$500)";
                    }
                    else
                    {
                        Total = Producto.precio;
                        lbTotal.Content = $"Total: {Total}";
                    }
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
                        Total = Producto.precio + 2000;
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
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o Nori -> Furay: +$1000)";
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
                    // envoltura normal: 1 rollo furay, 1 rollo queso o palta, 1 rollo california
                    List<envoltura> listaQuesoPalta = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("palta") || x.nombre.ToLower().Equals("queso crema")).ToList();
                    List<envoltura> listaCaliforniaNori = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("california") || x.nombre.ToLower().Equals("nori")).ToList();
                    List<envoltura> listaFuray = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("furay")).ToList();
                    if (listaQuesoPalta.Count == 3) // los 3 rollos son de palta o queso
                    {
                        // pasar de calfornia o nori a queso o palta suma $1500
                        // pasar de furay a queso o palta suma $500
                        Total = Producto.precio + 2000;
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
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o Nori -> Furay: +$1000)";
                    }
                    else if (listaFuray.Count == 3)
                    {
                        // pasar de california o nori a furay y palta/queso a furay suma $1000
                        Total = Producto.precio + 1000;
                        lbTotal.Content = $"Total: {Total} (${Producto.precio}, California o Nori -> Furay: +$1000)";
                    }
                    else
                    {
                        Total = Producto.precio;
                        lbTotal.Content = $"Total: {Total}";
                    }
                }
                if (Producto.cantidad_rollos_tabla == 5)
                {
                    // envoltura normal: 1 rollo furay, 1 rollo queso/palta, 2 rollos california/nori
                    List<envoltura> listaQuesoPalta = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("palta") || x.nombre.ToLower().Equals("queso crema")).ToList();
                    List<envoltura> listaCaliforniaNori = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("california") || x.nombre.ToLower().Equals("nori")).ToList();
                    List<envoltura> listaFuray = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("furay")).ToList();

                    if (listaQuesoPalta.Count == 4) // caso 1
                    {
                        Total = Producto.precio + 3500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 3 && listaFuray.Count == 1) // caso 2
                    {
                        Total = Producto.precio + 3000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 3 && listaCaliforniaNori.Count == 1) // caso 3
                    {
                        Total = Producto.precio + 2000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 2 && listaFuray.Count == 2) // caso 4
                    {
                        Total = Producto.precio + 2500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 1 && listaFuray.Count == 2) // caso 5
                    {
                        Total = Producto.precio + 2000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaFuray.Count == 4) // caso 6
                    {
                        Total = Producto.precio + 2000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else
                    {
                        Total = Producto.precio;
                        lbTotal.Content = $"Total: {Total}";
                    }
                }
                if (Producto.cantidad_rollos_tabla == 6)
                {
                    // envoltura normal: 2 rollos furay, 1 rollo queso/palta, 2 rollos california/nori
                    List<envoltura> listaQuesoPalta = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("palta") || x.nombre.ToLower().Equals("queso crema")).ToList();
                    List<envoltura> listaCaliforniaNori = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("california") || x.nombre.ToLower().Equals("nori")).ToList();
                    List<envoltura> listaFuray = ListaEnvolturasSeleccionadas.OfType<envoltura>().Where(x => x.nombre.ToLower().Equals("furay")).ToList();

                    if (listaQuesoPalta.Count == 5) // caso 1
                    {
                        Total = Producto.precio + 4000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaFuray.Count == 5) // caso 2
                    {
                        Total = Producto.precio + 2000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaFuray.Count == 4 && listaCaliforniaNori.Count == 1) // caso 6
                    {
                        Total = Producto.precio + 1000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 4 && listaCaliforniaNori.Count == 1) // caso 8
                    {
                        Total = Producto.precio + 2500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 4 && listaFuray.Count == 1) // caso 9
                    {
                        Total = Producto.precio + 3500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaCaliforniaNori.Count == 3 && listaQuesoPalta.Count == 2) // caso 11
                    {
                        Total = Producto.precio + 500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaFuray.Count == 3 && listaQuesoPalta.Count == 2) // caso 13
                    {
                        Total = Producto.precio + 2500;
                        lbTotal.Content = $"Total: {Total}";
                    }
                    else if (listaQuesoPalta.Count == 3 && listaCaliforniaNori.Count == 2) // caso 14
                    {
                        Total = Producto.precio + 1000;
                        lbTotal.Content = $"Total: {Total}";
                    }
                }
                if (Producto.cantidad_rollos_tabla == 10)
                {

                }
            }
        }

        */

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
