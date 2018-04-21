using posk.Models;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemEnvoltura : UserControl
    {
        public envoltura Envoltura { get; set; }

        public ItemEnvoltura()
        {
            InitializeComponent();

            Loaded += (se, a) => btnEnvoltura.Content = $"{Envoltura?.nombre}";
        }
    }
}
