using posk.BLL;
using posk.Controls;
using posk.Popup;
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

namespace posk.Pages.Menu
{
    public partial class PageGasto : Page
    {
        public PageGasto()
        {
            InitializeComponent();

            ItemTeclado teclado = new ItemTeclado(new List<TextBox>() { txtDetalle });
            borderTeclado.Child = teclado;

            ItemTecladoNumerico tecladoNumerico = new ItemTecladoNumerico(new List<TextBox>() { txtMonto });
            borderTecladoNumerico.Child = tecladoNumerico;

            txtMonto.GotFocus += (se, a) => teclado.expTeclado.IsExpanded = false;
            txtDetalle.GotFocus += (se, a) => tecladoNumerico.expTecladoNum.IsExpanded = false;

            btnIngresar.Click += (se, a) =>
            {
                GastoBLL.Ingresar(txtDetalle.Text, Convert.ToDecimal(txtMonto.Text), DateTime.Now);
                new Notification("LISTO");
                txtDetalle.Clear();
                txtMonto.Clear();
            };
        }
    }
}
