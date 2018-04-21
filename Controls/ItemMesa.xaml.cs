using posk.Models;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemMesa : UserControl
    {
        private bool? libre;
        public mesa Mesa { get; set; }

        public bool? Libre
        {
            get { return libre; }
            set
            {
                libre = value;
                switch (libre)
                {
                    case true:
                        btnMesa.Background = new SolidColorBrush(Color.FromRgb(39, 174, 96));
                        break;
                    case false:
                        btnMesa.Background = new SolidColorBrush(Color.FromRgb(192, 57, 43));
                        break;
                    default:
                        break;
                }
            }
        }

        public ItemMesa()
        {
            InitializeComponent();
            Loaded += (se, a) =>
            {
                if (Mesa.items != 0)
                    tbItems.Text = Mesa.items+"";
                else
                    tbItems.Text = "";
                if (Mesa.usuario_id != null)
                    tbUsuario.Text = Mesa.usuario.nombre;
                else
                    tbUsuario.Text = "";
                tbMesa.Text = Mesa.codigo;

                if (Mesa?.libre == true) libre = true;
                else libre = false;
            };
        }
    }
}
