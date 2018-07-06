using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace posk.Popups
{
    public partial class ArmarProductoPopup : Window
    {
        public bool bCerrado = false;
        public envoltura Envoltura { get; set; }
        public int Posicion { get; set; }
        public agregado PaltaCebollin { get; set; }

        public producto Producto { get; set; }

        public event EventHandler<ItemVenta> AlIngresarProductoArmado;

        public ArmarProductoPopup(producto p)
        {
            InitializeComponent();
            Producto = p;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            lbTitulo.Content = $"ARMAR {p.nombre}".ToUpper();
            Posicion = 1;
            btnAtras.Click += (se2, a2) =>
            {
                if (p.es_handroll == true)
                {
                    if (Posicion == 2)
                    {
                        Posicion = 1;
                        expItemsDos.IsExpanded = false;
                        expItemsUno.IsExpanded = true;
                        PaltaCebollin = null;
                        Envoltura = null;
                        wrapItemsDos.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(ag => ag.Cantidad = 0);
                        ActualizarLabelSeleccion();
                    }
                    if (Posicion == 3)
                    {
                        Posicion = 2;
                        expItemsTres.IsExpanded = false;
                        expItemsDos.IsExpanded = true;
                        wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(ag => ag.Cantidad = 0);
                        ActualizarLabelSeleccion();
                    }
                }
                if (p.es_superhandroll == true)
                {
                    if (Posicion == 3)
                    {
                        Posicion = 1;
                        Envoltura = null;
                        PaltaCebollin = null;
                        expItemsTres.IsExpanded = false;
                        expItemsUno.IsExpanded = true;
                        wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(ag => ag.Cantidad = 0);
                        ActualizarLabelSeleccion();
                    }
                }
                else
                {
                    if (Posicion == 3)
                    {
                        Posicion = 1;
                        Envoltura = null;
                        PaltaCebollin = null;
                        expItemsTres.IsExpanded = false;
                        expItemsUno.IsExpanded = true;
                        wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(ag => ag.Cantidad = 0);
                        ActualizarLabelSeleccion();
                    }
                }
                wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().ToList().ForEach(x => x.lbCantidad.Content = "0");

            };

            AgregadoBLL.ObtenerPaltaCebollin().ForEach(agr =>
            {
                ItemAgregadoHandroll iah = new ItemAgregadoHandroll(true) { Agregado = agr };
                iah.btnAgregado.Click += (se, a) =>
                {
                    Posicion = 3;
                    expItemsDos.IsExpanded = false;
                    expItemsTres.IsExpanded = true;
                    PaltaCebollin = agr;
                    ActualizarLabelSeleccion();
                };
                wrapItemsDos.Children.Add(iah);
            });
            if (p.es_handroll == true)
            {
                EnvolturaBLL.ObtenerTodasParaHandroll().ForEach(env =>
                {
                    ItemEnvoltura ie = new ItemEnvoltura() { Envoltura = env };
                    ie.btnEnvoltura.Click += (se2, a2) =>
                    {
                        Envoltura = env;
                        if (p.es_handroll == true)
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsDos.IsExpanded = true;
                        }
                        if (p.es_superhandroll == true)
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsTres.IsExpanded = true;
                        }
                        Posicion = 2;
                        ActualizarLabelSeleccion();
                    };
                    wrapItemsUno.Children.Add(ie);
                });
            }
            else if (p.es_superhandroll == true)
            {
                EnvolturaBLL.ObtenerTodasParaSuperHandroll().ForEach(env =>
                {
                    ItemEnvoltura ie = new ItemEnvoltura() { Envoltura = env };
                    ie.btnEnvoltura.Click += (se2, a2) =>
                    {
                        Envoltura = env;
                        if (p.es_handroll == true)
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsDos.IsExpanded = true;
                        }
                        if (p.es_superhandroll == true)
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsTres.IsExpanded = true;
                        }
                        Posicion = 3;
                        ActualizarLabelSeleccion();
                    };
                    wrapItemsUno.Children.Add(ie);
                });
            }
            else // rollo carta por ejemplo
            {
                EnvolturaBLL.ObtenerTodas().ForEach(env =>
                {
                    ItemEnvoltura ie = new ItemEnvoltura() { Envoltura = env };
                    ie.btnEnvoltura.Click += (se2, a2) =>
                    {
                        Envoltura = env;
                        if (p.es_handroll == true)
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsDos.IsExpanded = true;
                        }
                        else
                        {
                            expItemsUno.IsExpanded = false;
                            expItemsTres.IsExpanded = true;
                        }
                        Posicion = 3;
                        ActualizarLabelSeleccion();
                    };
                    wrapItemsUno.Children.Add(ie);
                });
            }

            btnIngresar.Click += (se, a) =>
            {
                var listaAgregados = wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().Where(x => x.Cantidad > 0).ToList();
                if (PaltaCebollin != null)
                    listaAgregados.Insert(0, new ItemAgregadoHandroll() { Agregado = PaltaCebollin, Cantidad = 1 });

                ItemVenta iv = new ItemVenta() { Producto = p, listaAgregadosSushi = listaAgregados, Envoltura = Envoltura, PaltaCebollin = PaltaCebollin };

                AlIngresarProductoArmado.Invoke(this, iv);
                bCerrado = true;
                Close();
            };

            AgregadoBLL.ObtenerTodos().ForEach(agr =>
            {
                ItemAgregadoHandroll iah = new ItemAgregadoHandroll() { Agregado = agr };
                iah.btnAgregado.Click += (se2, a2) =>
                {
                    ActualizarLabelSeleccion();
                };
                iah.btnQuitarUnidad.Click += (se2, a2) =>
                {
                    ActualizarLabelSeleccion();
                };
                wrapItemsTres.Children.Add(iah);
            });
            expItemsUno.IsExpanded = true;
        }

        private void ActualizarLabelSeleccion()
        {
            var listaAgregados = wrapItemsTres.Children.OfType<ItemAgregadoHandroll>().Where(x => x.Cantidad > 0).ToList();
            string agregadosTemp = "";

            if (listaAgregados.Count >= 1)
                btnIngresar.IsEnabled = true;
            else
                btnIngresar.IsEnabled = false;

            listaAgregados.ForEach(agTemp =>
            {
                if (agTemp.Cantidad == 1)
                    agregadosTemp += $"{agTemp.txtNombre.Text}, ";
                if (agTemp.Cantidad > 1)
                    agregadosTemp += $"{agTemp.txtNombre.Text} x{agTemp.Cantidad}, ";

            });
            if (agregadosTemp != "")
                agregadosTemp = agregadosTemp.Substring(0, agregadosTemp.Length - 2);
            if (PaltaCebollin != null)
                lbSeleccion.Text = $"[{Envoltura?.nombre}] {PaltaCebollin.nombre} + {agregadosTemp}".ToUpper();
            else
                lbSeleccion.Text = $"[{Envoltura?.nombre}] {agregadosTemp}".ToUpper();
        }

        private void LimpiarSeleccion(int[] sectores)
        {

        }
    }
}
