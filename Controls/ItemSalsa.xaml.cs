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
    public partial class ItemSalsa : UserControl
    {
        public salsa Salsa { get; set; }
        public int CobroExtra { get; set; }
        public int Cantidad { get; set; }

        public ItemSalsa()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                txtNombre.Text = Salsa.nombre.ToUpper();
                Cantidad = 0;
                lbCantidad.Content = Cantidad;
            };
            btnAgregado.Click += (se, a) => lbCantidad.Content = $"{++Cantidad}";
            btnQuitarUnidad.Click += (se, a) =>
            {
                if (Cantidad >= 1)
                    lbCantidad.Content = $"{--Cantidad}";
            };
        }
    }
}
