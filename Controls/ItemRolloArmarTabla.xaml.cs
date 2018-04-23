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
    public partial class ItemRolloArmarTabla : UserControl
    {
        public string EnvolturaSeleccionada { get; set; }
        public string EnvolturaPorDefecto { get; set; }
        public string MensajeEnvolturaPorDefecto { get; set; }
        public int CobroAdicional { get; set; }
        public int CobroAdicionalAgregados { get; set; }

        public ItemRolloArmarTabla()
        {
            InitializeComponent();
            Loaded += (se, a) =>
            {
                lbEnvolturaDefecto.Content = MensajeEnvolturaPorDefecto;
                cbEnvolturas.ItemsSource = EnvolturaBLL.ObtenerTodas();
                cbEnvolturas.DisplayMemberPath = "nombre";
                cbEnvolturas.Text = EnvolturaPorDefecto;

                AgregadoBLL.ObtenerTodos().Where(ag => ag.para_handroll == true).ToList().ForEach(itemLista =>
                {
                    var ia = new ItemAgregadoHandroll() { Agregado = itemLista };
                    ia.btnQuitarUnidad.Click += (se2, a2) => ActualizarSeleccion();
                    ia.btnAgregado.Click += (se2, a2) => ActualizarSeleccion();
                    spAgregados.Children.Add(ia);
                });
            };

            cbEnvolturas.SelectionChanged += CbEnvolturas_SelectionChanged;
        }

        private void CbEnvolturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            envoltura envSeleccionada = cbEnvolturas.SelectedItem as envoltura;

            if (EnvolturaPorDefecto == "FURAY")
            {
                if (envSeleccionada.nombre == "QUESO CREMA" || envSeleccionada.nombre == "PALTA")
                    CobroAdicional = 500;
                else
                    CobroAdicional = 0;
            }
            else if (EnvolturaPorDefecto == "CALIFORNIA")
            {
                if (envSeleccionada.nombre == "QUESO CREMA" || envSeleccionada.nombre == "PALTA")
                    CobroAdicional = 1500;
                else
                    CobroAdicional = 0;
            }
            else if (EnvolturaPorDefecto == "PALTA" || EnvolturaPorDefecto == "QUESO CREMA")
            {

            }
            else
            {
                if (envSeleccionada.nombre == "QUESO CREMA" || envSeleccionada.nombre == "PALTA")
                    CobroAdicional = 1500;
                else if (envSeleccionada.nombre == "FURAY")
                    CobroAdicional = 1000;
                else
                    CobroAdicional = 0;
            }
            ActualizarSeleccion();
        }

        private void ActualizarSeleccion()
        {
            int contadorIngredientes = 0;
            int limiteIngredientes = 5;
            int cobroPorIngredienteExtra = 500;

            string seleccion = "";
            if (cbEnvolturas.SelectedItem != null)
                seleccion += $"Envoltura: {(cbEnvolturas.SelectedItem as envoltura).nombre} \nIngredientes: ";
            else
                seleccion += $"Envoltura: [SELECCIONA] \nIngredientes: ";

            spAgregados.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(agregado =>
            {
                contadorIngredientes += agregado.Cantidad;
                if (agregado.Cantidad == 1)
                    seleccion += $"{agregado.txtNombre.Text}, ";
                else if (agregado.Cantidad > 1)
                {
                    seleccion += $"{agregado.txtNombre.Text} x{agregado.Cantidad}, ";
                }
            });

            if (contadorIngredientes > limiteIngredientes)
            {
                CobroAdicionalAgregados = cobroPorIngredienteExtra * (contadorIngredientes - limiteIngredientes);
                //AlObtenerCobroExtraAgregados.Invoke(this, CobroAdicionalAgregados);
            }
            else
                CobroAdicionalAgregados = 0;

            if (seleccion != "")
                seleccion = seleccion.Substring(0, seleccion.Length - 2);

            if (contadorIngredientes == 0)
                lbSeleccionaMsg.Content = "SELECCIONA";
            else
            {
                lbSeleccionaMsg.Content = "";
                lbSeleccion.Content = seleccion;
            }

        }
    }
}
