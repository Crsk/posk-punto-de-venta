using posk.BLL;
using System;
using System.Windows;

namespace posk.Popups
{
    public partial class DeliveryPopup : Window
    {
        public bool bCerrado = false;
        public event EventHandler AlEntregar;

        public DeliveryPopup(int id, string nombreCliente, DateTime? fecha, string incluye)
        {
            InitializeComponent();

            Loaded += (se, a) => 
            {
                lbCliente.Content = $"{nombreCliente}";
                txtSalsas.Text = $"{incluye}";
                lbFecha.Content = $"{fecha.Value.ToShortDateString()} a las {fecha.Value.ToShortTimeString()}";
            };
            btnAceptar.Click += (se, a) => 
            {
                DeliveryItemBLL.Entregar(id);
                AlEntregar?.Invoke(this, null);
            };

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
        }

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
