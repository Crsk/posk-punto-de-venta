using System;
using System.Configuration;
using System.Windows;

namespace posk.Pages.PopUp
{
    public partial class WindowPriceInfo : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        int cost, price;

        public WindowPriceInfo(int cost, int price)
        {
            InitializeComponent();
            this.cost = cost;
            this.price = price;
            ShowDetails();
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
        }

        private void ShowDetails()
        {
            try
            {
                int precioCompraNeto;
                int precioCompraBruto;
                int precioVentaNeto;
                int precioVentaBruto;
                int utilidad;

                int ivaPct = Convert.ToInt32(ConfigurationManager.AppSettings["IVA"]);

                decimal ivaDecimal = 1.19M;

                int ivaCompra;
                int ivaVenta;
                int ivaTributario;
                if (ivaPct == 19) ivaDecimal = 1.19M;

                precioVentaBruto = price;
                precioVentaNeto = Convert.ToInt32(precioVentaBruto / ivaDecimal);
                precioCompraBruto = cost;
                precioCompraNeto = Convert.ToInt32(precioCompraBruto / ivaDecimal);
                ivaCompra = precioCompraBruto - precioCompraNeto;
                ivaVenta = precioVentaBruto - precioVentaNeto;
                ivaTributario = ivaVenta - ivaCompra;
                utilidad = precioVentaBruto - precioCompraBruto - ivaTributario;

                lbInfo.Content = "";
                lbInfo.Content += $"\n ivaPct: {ivaPct}";
                lbInfo.Content += $"\n precioCompraNeto: {precioCompraNeto}";
                lbInfo.Content += $"\n ivaCompra: {ivaCompra}";
                lbInfo.Content += $"\n precioCompraBruto: {precioCompraBruto}";
                lbInfo.Content += $"\n precioVentaNeto: {precioVentaNeto}";
                lbInfo.Content += $"\n ivaVenta: {ivaVenta}";
                lbInfo.Content += $"\n precioVentaBruto: {precioVentaBruto}";
                lbInfo.Content += $"\n ivaTributario: {ivaTributario}";
                lbInfo.Content += $"\n utilidad: {utilidad}";
            }
            catch
            {
                bCerrado = true; Close();
            }
        }
    }
}
