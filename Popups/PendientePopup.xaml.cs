using posk.BLL;
using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Popup
{
    public partial class PendientePopup : UserControl, IDisposable
    {
        public List<ItemPendiente> listaPendientesSeleccionados = new List<ItemPendiente>();
        private Label lbVerEntregasNumero;
        private EventHandler despendientizarEvent;
        public event EventHandler DespendientizarEvent
        {
            add { if (despendientizarEvent == null) despendientizarEvent += value; }
            remove { despendientizarEvent -= value; }
        }

        public PendientePopup(Label lbVerEntregasNumero)
        {
            InitializeComponent();
            spPendientesArchivados.Visibility = Visibility.Hidden;
            spPendientesArchivados.Height = 0;
            spPendientes.Visibility = Visibility.Visible;
            spPendientes.Height = 3000;

            this.lbVerEntregasNumero = lbVerEntregasNumero;

            MostrarPendientes();
            DeseleccionarPendientes();
            btnArchivo.Content = NombrarBotonArchivo();
            lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
            Events();
        }

        void IDisposable.Dispose()
        {
        }

        #region Metodos
        private string NombrarBotonIngresarPendiente()
        {
            if (listaPendientesSeleccionados.Count > 0)
            {
                btnIngresarPendientesAVenta.IsEnabled = true;
                return $"INGRESAR ({listaPendientesSeleccionados.Count})";
            }
            else
            {
                btnIngresarPendientesAVenta.IsEnabled = false;
                return "";
            }
        }

        private string NombrarBotonArchivo()
        {
            if (spPendientesArchivados.Height == 0)
            {
                lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                lbPendientes.Content = "PENDIENTES";
                dpPendientes.Background = new SolidColorBrush(Color.FromRgb(12, 12, 12));
                if (listaPendientesSeleccionados.Count > 0)
                    return $"ARCHIVAR ({listaPendientesSeleccionados.Count})";
                else
                    return "VER ARCHIVADOS";
            }
            else
            {
                lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                lbPendientes.Content = "PENDIENTES ARCHIVADOS";
                dpPendientes.Background = new SolidColorBrush(Color.FromRgb(2, 2, 2));
                if (listaPendientesSeleccionados.Count > 0)
                    return $"DESARCHIVAR ({listaPendientesSeleccionados.Count})";
                else
                    return "VOLVER";
            }
        }

        private void MostrarPendientes()
        {
            spPendientesArchivados.Children.Clear();
            spPendientes.Children.Clear();
            /*
            List<pendiente> listaPendientes = PendienteBLL.GetAll();

            foreach (var pend in listaPendientes)
            {
                ItemPendiente ip = new ItemPendiente()
                {
                    ID = pend.id,
                    BarCode = pend.producto.codigo_barras,
                    Detalle = pend.producto.nombre,
                    ValorUnitario = Convert.ToInt32(pend.producto.precio),
                    RutaImagenProducto = pend.producto.imagen,
                    RutaImagenUsuario = pend.usuario.imagen,
                    Nombre = pend.usuario.nombre,
                    Usuario = pend.usuario.nombre_usuario,
                    Fecha = pend.fecha,
                    Archivado = pend.archivado
                };
                ip.BtnIncluirExcluir.Click += (se2, ev2) =>
                {
                    if (ip.container.Background == null)
                    {
                        ip.container.Background = new SolidColorBrush(Color.FromRgb(34, 139, 34));
                        ip.bSeleccionado = true;
                        listaPendientesSeleccionados.Add(ip);
                    }
                    else
                    {
                        ip.container.Background = null;
                        ip.bSeleccionado = false;
                        listaPendientesSeleccionados.Remove(ip);
                    }
                    btnArchivo.Content = NombrarBotonArchivo();
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                };
                if (ip.Archivado == true)
                    spPendientesArchivados.Children.Add(ip);
                else
                    spPendientes.Children.Add(ip);
            }
            */
        }

        private void DeseleccionarPendientes()
        {
            foreach (ItemPendiente ip in listaPendientesSeleccionados)
            {
                ip.container.Background = null;
                //ip.bSeleccionado = false;
            }
            listaPendientesSeleccionados.Clear();
        }


        #endregion Metodos

        #region Eventos
        private void Events()
        {
            btnIngresarPendientesAVenta.Click += (se, ev) =>
            {
                despendientizarEvent?.Invoke(this, null);
            };

            btnArchivo.Click += (se, ev) =>
            {
                if (btnArchivo.Content.Equals("VER ARCHIVADOS"))
                {
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                    lbPendientes.Content = "PENDIENTES";
                    dpPendientes.Background = new SolidColorBrush(Color.FromRgb(12, 12, 12));
                    spPendientesArchivados.Visibility = Visibility.Visible;
                    spPendientesArchivados.Height = 3000;
                    spPendientes.Visibility = Visibility.Hidden;
                    spPendientes.Height = 0;
                    btnArchivo.Content = NombrarBotonArchivo();
                }
                else if (btnArchivo.Content.ToString().StartsWith("ARCHIVAR"))
                {
                    // Update pendientes seleccionados a estado achivado
                    //foreach (ItemPendiente ip in listaPendientesSeleccionados)
                    //{
                    //    PendienteBLL.Archivar(ip.ID);
                    //}

                    // update visual
                    MostrarPendientes();
                    DeseleccionarPendientes();
                    btnArchivo.Content = "VER ARCHIVADOS";
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                    lbPendientes.Content = "PENDIENTES";
                    dpPendientes.Background = new SolidColorBrush(Color.FromRgb(12, 12, 12));
                    lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Where(x => x.archivado == false).ToList().Count}";
                }
                else if (btnArchivo.Content.ToString().StartsWith("DESARCHIVAR"))
                {
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                    lbPendientes.Content = "PENDIENTES";
                    dpPendientes.Background = new SolidColorBrush(Color.FromRgb(12, 12, 12));
                    // Update pendientes seleccionados a estado desachivado
                    //foreach (ItemPendiente ip in listaPendientesSeleccionados)
                    //{
                    //    PendienteBLL.Desarchivar(ip.ID);
                    //}

                    spPendientesArchivados.Visibility = Visibility.Hidden;
                    spPendientesArchivados.Height = 0;
                    spPendientes.Visibility = Visibility.Visible;
                    spPendientes.Height = 3000;
                    MostrarPendientes();
                    DeseleccionarPendientes();
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                    btnArchivo.Content = "VER ARCHIVADOS";
                    lbVerEntregasNumero.Content = $"{PendienteBLL.GetAll().Where(x => x.archivado == false).ToList().Count}";
                }
                else
                {
                    lbBotonIngresarPendiente.Content = NombrarBotonIngresarPendiente();
                    lbPendientes.Content = "PENDIENTES";
                    dpPendientes.Background = new SolidColorBrush(Color.FromRgb(12, 12, 12));
                    spPendientesArchivados.Visibility = Visibility.Hidden;
                    spPendientesArchivados.Height = 0;
                    spPendientes.Visibility = Visibility.Visible;
                    spPendientes.Height = 3000;
                    DeseleccionarPendientes();
                    btnArchivo.Content = "VER ARCHIVADOS";
                }
                // btnArchivo.Content = btnArchivo.Content.ToString().Contains("ARCHIVAR") || btnArchivo.Content.Equals("VER ARCHIVADOS") ? "VOLVER" : NombrarBotonArchivo(listaPendientesSeleccionados);
            };
        }
    }
}
#endregion Eventos