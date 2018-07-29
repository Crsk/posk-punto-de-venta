using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Controls
{
    public partial class ItemEscoger : UserControl
    {
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; txtNombre.Text = value.ToUpper(); }
        }

        public string verdeStr = "#FF0A7562";
        public string grisStr = "#FFC9C9C9";
        public string grisClaroStr = "#FFE1E1E1";
        public string grisOscuroStr = "#FF7C7C7C";
        public string blancoStr = "#fff";

        public Color Verde { get; set; }
        public Color Gris { get; set; }
        public Color Blanco { get; set; }
        public Color GrisClaro { get; set; }
        public Color GrisOscuro { get; set; }

        public ItemEscoger()
        {
            InitializeComponent();
            Verde = (Color)ColorConverter.ConvertFromString(verdeStr);
            Gris = (Color)ColorConverter.ConvertFromString(grisStr);
            Blanco = (Color)ColorConverter.ConvertFromString(blancoStr);
            GrisClaro = (Color)ColorConverter.ConvertFromString(grisClaroStr);
            GrisOscuro = (Color)ColorConverter.ConvertFromString(grisOscuroStr);
        }

        public void Escoger(bool b)
        {
            if (b)
            {
                btnOpcion.Background = new SolidColorBrush(Verde);
                txtNombre.Foreground = new SolidColorBrush(Blanco);
            }
            else
            {
                btnOpcion.Background = new SolidColorBrush(GrisClaro);
                txtNombre.Foreground = new SolidColorBrush(GrisOscuro);
            }
        }
    }
}
