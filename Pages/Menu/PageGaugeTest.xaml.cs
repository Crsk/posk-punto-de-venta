using posk.BLL;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageGaugeTest : Page, IDisposable
    {
        public int V { get; set; }
        public int A { get; set; }
        public int Z { get; set; }

        public PageGaugeTest()
        {
            InitializeComponent();
            V = BoletaBLL.GetIngresosPeriodo();
            A = BoletaBLL.GetMayorIngresoUltimosDias(200);
            Z = Convert.ToInt32(A * (1.5));
            DataContext = this;
        }

        void IDisposable.Dispose()
        {
        }
    }
}
