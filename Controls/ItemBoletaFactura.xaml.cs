using posk.Models;
using System;
using System.Windows.Controls;

namespace posk.Controls
{
    public partial class ItemBoletaFactura : UserControl
    {
        public int ID { get; set; }
        public Button BtnBorrar { get; set; }
        public Button BtnBoletaFactura { get; set; }
        public DateTime Fecha { get; set; }
        public int? NumeroBoleta { get; set; }
        public int? Total { get; set; }
        public string Cliente { get; set; }
        public Expander ExpDetalle { get; set; }
        public StackPanel SpDetalle { get; set; }
        public StackPanel SpContenidoPrincipal { get; set; }

        public ItemBoletaFactura()
        {
            InitializeComponent();
            ExpDetalle = expDetalle;
            SpDetalle = spDetalle;
            BtnBorrar = btnBorrar;
            BtnBoletaFactura = btnBoletaFactura;
            SpContenidoPrincipal = spContenidoPrincipal;

            Loaded += (se, ev) =>
            {
                lbTotal.Content = $"${Total}";
                lbNumeroBoleta.Content = NumeroBoleta != null ? $"N° {NumeroBoleta}" : "S/N";
                lbFecha.Content = $"{Fecha.ToShortDateString()} {Fecha.ToShortTimeString()}";
                lbCliente.Content = Cliente;
            };
            btnBoletaFactura.Click += (se, e) =>
            {
                expDetalle.IsExpanded ^= true;
            };
        }
    }
}
