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
    public partial class ItemEscogerGarzonSeccionVenta : UserControl
    {
        private List<usuario> usuarios;

        public List<usuario> Usuarios
        {
            get { return usuarios; }
            set
            {
                usuarios = value;
                cbGarzones.ItemsSource = value;
                value?.Insert(0, new usuario() { nombre = "" });
                cbGarzones.DisplayMemberPath = "nombre";
            }
        }

        public ItemEscogerGarzonSeccionVenta()
        {
            InitializeComponent();
        }
    }
}
