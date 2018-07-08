using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{

    public partial class ItemOpcion : UserControl
    {
        public opcionale Opcion { get; set; }

        public event EventHandler AlSeleccionar;

        public ItemOpcion()
        {
            InitializeComponent();

            Loaded += (se, a) => 
            {
                if (Opcion != null)
                    lbNombre.Content = Opcion.nombre;
            };
            btnOpcion.Click += (se, a) => AlSeleccionar?.Invoke(this, null);
        }
    }
}
