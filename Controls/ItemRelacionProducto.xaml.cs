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
    public partial class ItemRelacionProducto : UserControl
    {
        public int Id { get; set; }

        public int Precio { get; set; }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; lbNombre.Content = $"{value} (${Precio})"; }
        }
        public bool Seleccionado { get; set; }

        public event EventHandler AlEliminar;
        public event EventHandler AlSeleccionar;

        public ItemRelacionProducto()
        {
            InitializeComponent();
            Seleccionado = false;

            btnBorrar.Click += (se, a) => AlEliminar?.Invoke(this, null);
            btnItem.Click += (se, a) =>
            {
                AlSeleccionar?.Invoke(this, null);
            };
        }

        public void SeleccionarToggle(bool b)
        {
            if (b)
            {
                lbNombre.Foreground = new SolidColorBrush(Color.FromRgb(5, 119, 85)); // texto verde (seleccionado)
            }
            else
            {
                lbNombre.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)); // texto negro (no seleccionado)
            }
        }

        public void QuitarBotonBorrar()
        {
            spItems.Children.Remove(btnBorrar);
        }
    }
}
