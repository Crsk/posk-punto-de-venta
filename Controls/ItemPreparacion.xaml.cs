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
    public partial class ItemPreparacion : UserControl
    {
        private preparacione preparacione;
        public bool Seleccionado { get; set; }

        public preparacione Preparacion
        {
            get { return preparacione; }
            set { preparacione = value; btnPreparacion.Content = value.nombre.ToUpper(); }
        }

        public ItemPreparacion()
        {
            InitializeComponent();

            Loaded += (se, a) => 
            {
                btnPreparacion.Content = Preparacion.nombre;
                btnPreparacion.FontSize = Preparacion.font_size;
            };
        }
    }
}
