using posk.Pages.Graficos;
using System;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageInformes : Page, IDisposable
    {
        public PageInformes()
        {
            InitializeComponent();
            // frameInformes.Content = new PageVentasEnCurso();
            frameInformes.Content = new PageGaugeTest();
            //miIngresosEnCurso.Click += (se, ev) => { frameInformes.Content = new PageVentasEnCurso(); };
            //miIngresosPeriodo.Click += (se, ev) => { frameInformes.Content = new PageVentasPeriodo(); };
            miIngresosGlobal.Click += (se, ev) => { frameInformes.Content = new PageVentasGlobal(); };
        }

        void IDisposable.Dispose() {}
    }
}
