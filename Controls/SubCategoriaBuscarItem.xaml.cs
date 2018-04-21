using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class SubCategoriaBuscarItem : UserControl
    {
        private subcategoria subcategoria;
        public subcategoria SubCategoria
        {
            get { return subcategoria; }
            set
            {
                subcategoria = value;
                btnSubCategoria.Content = value.nombre;
            }
        }

        private categoria categoria;

        public categoria Categoria
        {
            get { return categoria; }
            set
            {
                categoria = value;
                btnSubCategoria.Content = value.nombre;
            }
        }

        private sectore sector;

        public sectore Sector
        {
            get { return sector; }
            set { sector = value; btnSubCategoria.Content = value.nombre; }
        }



        public SubCategoriaBuscarItem()
        {
            InitializeComponent();
        }
    }
}
