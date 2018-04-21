using System.Windows.Controls;

namespace posk.Controls
{
    public partial class LoginItem : UserControl
    {
        public string Pass { get; set; }
        public int ID { get; set; }

        public LoginItem()
        {
            InitializeComponent();
        }
    }
}
