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

namespace posk.Controls.Accion
{
    public partial class ItemMensajeCocina : UserControl
    {
        private string mensaje;

        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value; txtMensaje.Text = value; }
        }


        public ItemMensajeCocina()
        {
            InitializeComponent();

            btnBorrarMensaje.Click += (se, a) => { txtMensaje.Clear(); };

            Loaded += (se, a) => 
            {
                //cbSectoresImpresion.DataContext = null;
                cbSectoresImpresion.ItemsSource = SectorImpresionBLL.ObtenerParaComboBox();
                cbSectoresImpresion.DisplayMemberPath = "nombre";
                cbSectoresImpresion.SelectedIndex = 0;
            };
        }
    }
}
