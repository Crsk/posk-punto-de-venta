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
    public partial class PagePrimerInicio : Page
    {
        public event EventHandler OnFirstInit;
        public PagePrimerInicio()
        {
            InitializeComponent();
            btnValidar.Click += (se, a) =>
            {
                if (txtUsuario.Text.Equals("asd") && txtPass.Password.Equals("MyTempPass.12"))
                {
                    posk.Properties.Settings.Default.AppName = "Posk";
                    posk.Properties.Settings.Default.Save();
                    OnFirstInit(this, null);
                }
            };
        }
    }
}
