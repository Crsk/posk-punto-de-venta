using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace posk.Controls
{
    public partial class ItemPagaCon : UserControl
    {
        public int Total { get; set; }

        private int pagaCon;

        public string Vuelto { get; set; }

        public int PagaCon
        {
            get { return pagaCon; }
            set
            {
                pagaCon = value;
                if (value >= Total)
                {
                    Vuelto = $"{value - Total}";
                    txtVuelto.Text = Vuelto;
                }
                else
                {
                    txtVuelto.Text = "'Paga Con' es incorrecto";
                    Vuelto = "No Denifido";
                }
            }
        }

        public ItemPagaCon()
        {
            InitializeComponent();
        }

        private void ValidarNumero(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
