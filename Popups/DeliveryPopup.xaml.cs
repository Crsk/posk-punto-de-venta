﻿using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Popups
{
    public partial class DeliveryPopup : Window
    {
        public bool bCerrado = false;
        public event EventHandler AlEntregar;

        public DeliveryPopup(delivery_item di)
        {
            InitializeComponent();

            cbMedioPago.ItemsSource = MedioPagoBLL.ObtenerTodos();
            cbMedioPago.DisplayMemberPath = "nombre";
            cbMedioPago.Text = di.medio_pago?.nombre;
            cbMedioPago.SelectionChanged += (se, a) =>
            {
                int medioPagoId = (cbMedioPago.SelectedItem as medio_pago).id;
                DeliveryItemBLL.CambiarMedioDePago(di.id, medioPagoId);
                BoletaMediopagoBLL.ActualizarMedioDePago(di.boleta.id, medioPagoId);
                if (medioPagoId == 1 && di.paga_con != 0)
                    txtPagaConMonto.Text = $" (Monto: ${di.paga_con}, Vuelto: ${di.vuelto})";
                else
                    txtPagaConMonto.Text = "";
            };

            Loaded += (se, a) =>
            {
                lbTotal.Content += di.boleta.total.ToString();
                lbNumeroBoleta.Content = $"#{di.boleta?.numero_boleta}";
                lbNombreCliente.Content = $"{di.nombre_cliente}";
                txtAdicional.Text = $"{di.incluye}";
                txtServirLlevar.Text = di.servir == true ? "SERVIR" : "LLEVAR";
                if ((cbMedioPago.SelectedItem as medio_pago)?.id == 1 && di.paga_con != 0)
                    txtPagaConMonto.Text = $" (Monto: ${di.paga_con}, Vuelto: ${di.vuelto})";
                else
                    txtPagaConMonto.Text = "";

                if (di.fecha_entrega != null)
                {
                    lbFecha.Content = $"{di.fecha_entrega.Value.ToShortDateString()} a las {di.fecha_entrega.Value.ToShortTimeString()}";
                }

                DetalleBoletaBLL.ObtenerPorBoletaId(di.boleta?.id).ForEach(x =>
                {
                    spDetalleBoleta.Children.Add(new Label() { Content = $"{x.producto.nombre} x{x.cantidad}", Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)) });
                });
            };
            btnAceptar.Click += (se, a) =>
            {
                DeliveryItemBLL.Entregar(di.id);
                AlEntregar?.Invoke(this, null);
            };

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
        }

        public void Cerrar()
        {
            bCerrado = true;
            Close();
        }
    }
}
