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
        public ItemRolloArmarTabla()
        {
            InitializeComponent();
            Loaded += (se, a) =>
            {
                cbEnvolturas.ItemsSource = EnvolturaBLL.ObtenerTodas();
                cbEnvolturas.DisplayMemberPath = "nombre";

                AgregadoBLL.ObtenerTodos().Where(ag => ag.para_handroll == true).ToList().ForEach(itemLista =>
                {
                    var ia = new ItemAgregadoHandroll() { Agregado = itemLista };
                    ia.btnQuitarUnidad.Click += (se2, a2) => ActualizarSeleccion();
                    ia.btnAgregado.Click += (se2, a2) => ActualizarSeleccion();
                    cbEnvolturas.SelectionChanged += (se2, a2) => ActualizarSeleccion();
                    spAgregados.Children.Add(ia);
                });
            };
        }

        private void ActualizarSeleccion()
        {
            int contadorIngredientes = 0;
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
                    seleccion += $"{agregado.txtNombre.Text} x{agregado.Cantidad}, ";
            });

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
