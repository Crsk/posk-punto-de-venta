﻿using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using posk.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using posk.BLL;
using System.Windows.Media;
using System.Windows.Controls;
using posk.Partials;

namespace posk.Popups
{
    public partial class RealizarVentaSushi : Window
    {
        public bool bCerrado = false;
        public int MontoTotalSinPropina { get; set; }
        public int MontoTotalConPropina { get; set; }
        public int Propina { get; set; }
        public bool bPropinaIncluida { get; set; }
        public bool bTxtPropinaVacio { get; set; }

        public int MontoEfectivo { get; set; }
        public int MontoTransBank { get; set; }
        public int MontoJunaeb { get; set; }
        public int MontoOtro { get; set; }

        public string verde = "#FF0A7562";
        public string gris = "#FFC9C9C9";
        public string blanco = "#fff";

        public event EventHandler<DeliveryInfo> AlVender;

        public RealizarVentaSushi(int montoTotalSinPropina)
        {
            InitializeComponent();
            this.MontoTotalSinPropina = montoTotalSinPropina;

            bPropinaIncluida = true;
            if (montoTotalSinPropina == 0) return;

            Propina = (montoTotalSinPropina * 10) / 100;
            MontoTotalConPropina = montoTotalSinPropina + Propina;
            MontoTotalSinPropina = montoTotalSinPropina;

            lbTotal.Content = $"{MontoTotalConPropina}";

            tooglePropina.Checked += (se, a) => PropinaToogle(true, Propina);
            tooglePropina.Unchecked += (se, a) => PropinaToogle(false, Propina);
            tooglePropina.Click += (se, a) =>
            {
                spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp => imp.Limpiar());
                CalcularMontos();
            };

            txtPropina.GotFocus += (se, a) => txtPropina.Text = "";
            txtPropina.MouseDown += (se, a) => txtPropina.Text = "";

            Loaded += (se, a) =>
            {
                txtPropina.Text = $"{Propina}";

                txtPropina.TextChanged += (se2, a2) =>
                {
                    try
                    {
                        spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp => imp.Limpiar());
                        if (txtPropina.Text == "")
                        {
                            PropinaToogle(false, 0);
                            bTxtPropinaVacio = true;
                        }
                        if (txtPropina.Text != "")
                        {
                            bTxtPropinaVacio = false;
                            PropinaToogle(true, Convert.ToInt32(txtPropina.Text));
                        }
                    }
                    catch
                    {
                        txtPropina.Text = "";
                    }
                    CalcularMontos();
                };

                SalsaBLL.ObtenerTodas().ForEach(salsa => wrapSalsas.Children.Add(new ItemSalsa() { Salsa = salsa }));
                MedioPagoBLL.ObtenerTodos().ForEach(mp =>
                {
                    ItemMedioPagoCalcular impc = new ItemMedioPagoCalcular() { MedioPago = mp };

                    if (impc.MedioPago.nombre.ToLower().Equals("efectivo"))
                    {
                        impc.txtMonto.TabIndex = 1;
                        impc.txtMonto.Text = $"{MontoTotalConPropina}";
                        MontoEfectivo = MontoTotalConPropina;
                        CalcularMontos();
                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoEfectivo = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoEfectivo = 0;
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoEfectivo = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("trans bank"))
                    {
                        impc.txtMonto.TabIndex = 2;
                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoTransBank = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoTransBank = 0;
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoTransBank = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("junaeb"))
                    {
                        impc.txtMonto.TabIndex = 3;

                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoJunaeb = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoJunaeb = 0;
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoJunaeb = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    if (impc.MedioPago.nombre.ToLower().Equals("otro"))
                    {
                        impc.txtMonto.TabIndex = 4;

                        impc.txtMonto.TextChanged += (se2, a2) =>
                        {
                            try
                            {
                                if (impc.txtMonto.Text != "")
                                    MontoOtro = Convert.ToInt32(impc.txtMonto.Text);
                                else
                                    MontoOtro = 0;
                            }
                            catch
                            {
                                impc.txtMonto.Text = "";
                                MontoOtro = 0;
                            }
                            CalcularMontos();
                        };
                    }
                    //impc.txtMonto.TextChanged += (se2, a2) => CalcularMontos();
                    impc.btnMedioPago.Click += (se2, a2) =>
                    {
                        spMedioPago.Children.OfType<ItemMedioPagoCalcular>().ToList().ForEach(imp =>
                        {
                            if (imp.btnMedioPago.Content == impc.btnMedioPago.Content)
                            {
                                imp.UsadoEstilo();
                                imp.txtMonto.Text = tooglePropina.IsChecked == true ? $"{MontoTotalConPropina}" : $"{montoTotalSinPropina}";
                            }
                            else
                                imp.Limpiar();
                        });

                        CalcularMontos();
                    };
                    spMedioPago.Children.Add(impc);
                });
            };

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            btnAceptar.Click += (se, a) =>
            {
                try
                {
                    ItemMedioPagoCalcular impcEfectivo = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Efectivo")).FirstOrDefault();
                    int efectivo = Convert.ToInt32(String.IsNullOrEmpty(impcEfectivo.txtMonto.Text) == true ? "0" : impcEfectivo.txtMonto.Text);

                    ItemMedioPagoCalcular impcTransBank = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Trans Bank")).FirstOrDefault();
                    int transBank = Convert.ToInt32(String.IsNullOrEmpty(impcTransBank.txtMonto.Text) == true ? "0" : impcTransBank.txtMonto.Text);

                    ItemMedioPagoCalcular impcJunaeb = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Junaeb")).FirstOrDefault();
                    int junaeb = Convert.ToInt32(String.IsNullOrEmpty(impcJunaeb.txtMonto.Text) == true ? "0" : impcJunaeb.txtMonto.Text);

                    ItemMedioPagoCalcular impcOtro = spMedioPago.Children.OfType<ItemMedioPagoCalcular>().Where(x => x.btnMedioPago.Content.Equals("Otro")).FirstOrDefault();
                    int otro = Convert.ToInt32(String.IsNullOrEmpty(impcOtro.txtMonto.Text) == true ? "0" : impcOtro.txtMonto.Text);

                    int propina = tooglePropina.IsChecked == true ? Convert.ToInt32(txtPropina.Text) : 0;

                    string incluyeStr = "";
                    string incluyeStrUnaLinea = "";

                    wrapSalsas.Children.OfType<ItemSalsa>().ToList().ForEach(salsa =>
                    {
                        incluyeStr += $"{salsa.txtNombre.Text} x{salsa.Cantidad}\n";
                    });

                    wrapSalsas.Children.OfType<ItemSalsa>().ToList().ForEach(salsa =>
                    {
                        incluyeStrUnaLinea += $"{salsa.txtNombre.Text} x{salsa.Cantidad}, ";
                    });
                    if (incluyeStrUnaLinea != "")
                        incluyeStrUnaLinea = incluyeStrUnaLinea.Substring(0, incluyeStrUnaLinea.Length - 2);

                    DeliveryInfo di = new DeliveryInfo()
                    {
                        Efectivo = efectivo,
                        TransBank = transBank,
                        Junaeb = junaeb,
                        Otro = otro,
                        Propina = propina,
                        NombreCliente = txtNombreCliente.Text,
                        Incluye = incluyeStr,
                        IncluyeStrUnaLinea = incluyeStrUnaLinea
                    };

                    AlVender.Invoke(this, di);
                }
                catch (Exception ex)
                {
                    Globals.PoskException.Make(ex, "Error al vender");
                }
            };

            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
        }

        private void CalcularMontos()
        {
            int _propina = 0;
            int _montoTotal = 0;
            bool bIncluyeProp = tooglePropina.IsChecked == true ? true : false;

            if (bIncluyeProp)
            {
                try
                {
                    if (!txtPropina.Text.Equals(""))
                        _propina = Convert.ToInt32(txtPropina.Text);
                }
                catch
                {
                }
            }
            _montoTotal = Convert.ToInt32(lbTotal.Content);

            if (MontoEfectivo + MontoTransBank + MontoJunaeb + MontoOtro == _montoTotal)
                btnAceptar.IsEnabled = true;
            else
                btnAceptar.IsEnabled = false;
        }

        private void PropinaToogle(bool bPropina, int propina)
        {
            var bc = new BrushConverter();
            Propina = propina;
            MontoTotalConPropina = MontoTotalSinPropina + Propina;
            MontoTotalSinPropina = MontoTotalConPropina - Propina;

            if (bPropina)
            {
                bPropinaIncluida = bPropina;
                tooglePropina.Background = (Brush)bc.ConvertFrom(verde);
                tooglePropina.Foreground = (Brush)bc.ConvertFrom(blanco);
                lbTotal.Content = $"{MontoTotalConPropina}";
                tooglePropina.IsChecked = true;
                if (bTxtPropinaVacio)
                {
                    bTxtPropinaVacio = false;
                    txtPropina.Text = $"{(MontoTotalSinPropina * 10) / 100}";
                    CalcularMontos();
                }
            }
            else
            {
                bPropinaIncluida = bPropina;
                tooglePropina.Background = (Brush)bc.ConvertFrom(gris);
                tooglePropina.Foreground = (Brush)bc.ConvertFrom(blanco);
                lbTotal.Content = $"{MontoTotalSinPropina}";
                if (Propina == 0)
                    txtPropina.Text = "";
                else
                    txtPropina.Text = $"{Propina}";
                tooglePropina.IsChecked = false;
            }
        }

        private void ValidarNumero(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
