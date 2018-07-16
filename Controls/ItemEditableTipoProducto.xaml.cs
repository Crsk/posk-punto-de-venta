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
    public partial class ItemEditableTipoProducto : UserControl
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; txtNombre.Text = value; }
        }

        private int limiteIngr;

        public int LimiteIngr
        {
            get { return limiteIngr; }
            set { limiteIngr = value; txtLimiteIngr.Text = value + ""; }
        }

        public event EventHandler AlEliminar;
        public event EventHandler AlEditar;
        public event EventHandler AlGuardar;

        public ItemEditableTipoProducto()
        {
            InitializeComponent();
            btnBorrar.Click += (se, a) => AlEliminar?.Invoke(this, null);
            btnEditar.Click += (se, a) => AlEditar?.Invoke(this, null);
            btnGuardar.Click += (se, a) => AlGuardar?.Invoke(this, null);
            Loaded += (se, a) => MostrarBotonEditar();
        }

        public void MostrarBotonEditar()
        {
            spContenido.Children.Clear();
            spContenido.Children.Add(btnBorrar);
            spContenido.Children.Add(txtNombre);
            spContenido.Children.Add(txtLimiteIngr);
            spContenido.Children.Add(btnEditar);
            txtNombre.IsReadOnly = true;
            txtLimiteIngr.IsReadOnly = true;
            txtNombre.IsEnabled = false;
            txtLimiteIngr.IsEnabled = false;
        }

        public void MostrarBotonGuardar()
        {
            spContenido.Children.Clear();
            spContenido.Children.Add(btnBorrar);
            spContenido.Children.Add(txtNombre);
            spContenido.Children.Add(txtLimiteIngr);
            spContenido.Children.Add(btnGuardar);
            txtNombre.IsReadOnly = false;
            txtLimiteIngr.IsReadOnly = false;
            txtNombre.IsEnabled = true;
            txtLimiteIngr.IsEnabled = true;
        }
    }
}
