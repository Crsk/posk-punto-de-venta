using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class DctoPopup : Window
    {
        public bool bCerrado = false;
        public event EventHandler AlAplicarDcto;
        public event EventHandler AlActualizarDescuento;
        public int TotalConDcto { get; set; }
        public int DctoPesos { get; set; }
        public int DctoPct { get; set; }

        public DctoPopup(ItemDcto itemDcto, int totalOriginal)
        {
            InitializeComponent();

            Deactivated += (se, a) => { if (!bCerrado) Close(); };

            DctoPct = (itemDcto.DctoPesos * 100) / totalOriginal;
            DctoPesos = itemDcto.DctoPesos;

            Loaded += (se, a) =>
            {
                DctoPct = (itemDcto.DctoPesos * 100) / totalOriginal;
                DctoPesos = itemDcto.DctoPesos;
                itemDcto.DctoPct = (itemDcto.DctoPesos * 100) / totalOriginal;
                txtPesos.Text = itemDcto?.DctoPesos + "";
                txtPct.Text = itemDcto?.DctoPct + "";
            };
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lbTotalOriginal.Content = $"Total original: ${totalOriginal - DctoPesos}";
            btnAceptar.Click += (se, a) =>
            {
                try
                {
                    if (txtPesos?.Text != "")
                        DctoPesos = Convert.ToInt32(txtPesos?.Text);
                    if (txtPct?.Text != "")
                        DctoPct = Convert.ToInt32(txtPct?.Text);
                    AlActualizarDescuento.Invoke(this, null);
                }
                catch (Exception ex)
                {
                    Globals.PoskException.Make(ex, "Error al convertir descuento");
                }
                AlAplicarDcto.Invoke(this, null);
            };

            txtPesos.GotFocus += (se, a) => 
            {
                txtPesos.Text = "";
                txtPct.Text = "";
            };

            txtPesos.TextChanged += (se, a) =>
            {
                try
                {
                    if (txtPesos.Text != "")
                    {
                        int dctoPesosTemp = Convert.ToInt32(txtPesos?.Text);
                        TotalConDcto = totalOriginal - Convert.ToInt32(txtPesos.Text);
                        lbTotalConDctoValor.Content = $"{TotalConDcto - DctoPesos}";
                        txtPct.Text = $"{dctoPesosTemp * 100 / totalOriginal}";
                    }
                }
                catch (Exception ex)
                {
                    Globals.PoskException.Make(ex, "Error al calcular descuento");
                }
            };

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
