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
    public partial class ItemMedioPago : UserControl
    {
        private List<medio_pago> mediosPago;
        public medio_pago MedioPagoSeleccionado { get; set; }

        public List<medio_pago> MediosPago
        {
            get { return mediosPago; }
            set
            {
                mediosPago = value;
                cbMedioPago.ItemsSource = value;
                cbMedioPago.DisplayMemberPath = "nombre";
                cbMedioPago.Text = "Efectivo";
                MedioPagoSeleccionado = cbMedioPago.SelectedItem as medio_pago;
            }
        }

        public ItemMedioPago()
        {
            InitializeComponent();

            cbMedioPago.SelectionChanged += (se, a) => MedioPagoSeleccionado = cbMedioPago.SelectedItem as medio_pago;
        }
    }
}
