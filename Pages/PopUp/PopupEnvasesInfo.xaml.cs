using posk.BLL;
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

namespace posk.Pages.PopUp
{
    public partial class PopupEnvasesInfo : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        string clientRut;
        public PopupEnvasesInfo(string clientRut)
        {
            InitializeComponent();
            this.clientRut = clientRut;
            List<PrestamoEnvaseUserDetails> listaPrestamoDetails = PrestamoEnvaseBLL.GetPrestamoEnvaseDetails(clientRut);

            dgPrestamos.ItemsSource = listaPrestamoDetails;

            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {
            int id = (dgPrestamos.SelectedItem as PrestamoEnvaseUserDetails).prestamoID;
            PrestamoEnvaseBLL.Delete(id);
            dgPrestamos.ItemsSource = PrestamoEnvaseBLL.GetPrestamoEnvaseDetails(clientRut);
        }
    }
}
