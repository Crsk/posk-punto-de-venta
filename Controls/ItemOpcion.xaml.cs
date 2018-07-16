using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{

    public partial class ItemOpcion : UserControl
    {
        private opcionale opcionale;

        public opcionale Opcion
        {
            get { return opcionale; }
            set
            {
                opcionale = value;
                lbNombre.Content = value.nombre;
                if (value.precio != 0)
                {
                    lbPrecio.Content = $"${value.precio}";
                    borderPrecio.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }



        public event EventHandler AlSeleccionar;

        public ItemOpcion()
        {
            InitializeComponent();
            btnOpcion.Click += (se, a) => AlSeleccionar?.Invoke(this, null);
        }
    }
}
