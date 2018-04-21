using posk.BLL;
using posk.Controls;
using posk.Globals;
using posk.Models;
using posk.Pages.Menu;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace posk.Pages.PopUp
{
    public partial class PopupCliente_busquedaAvanzada : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error
        TextBox txtCliente_rut;
        public static TextBox txtPopupCliente = new TextBox();

        public PopupCliente_busquedaAvanzada(TextBox txtCliente_rut)
        {
            InitializeComponent();
            //txtBuscarCliente.GotMouseCapture += (se, ev) => { PageVenta.lastFocusControl = (TextBox)se; PageVenta.expTecladoRef.IsExpanded = true; };

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Top = 0;
            this.Left = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            txtPopupCliente = txtBuscarCliente;
            this.txtCliente_rut = txtCliente_rut;
            //this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            this.Deactivated += (se, ev) => { this.Topmost = true; };

            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
        }


        private void txtBuscarCliente_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /*

        private void txtBuscarCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<clients> listaClientes = new List<clients>();
            if (txtBuscarCliente.Text == "" || txtBuscarCliente.Text.Contains("'") || txtBuscarCliente.Text.Contains("\""))
            {
                wrapClientes.Children.Clear();
            }
            else
            {
                wrapClientes.Children.Clear();
                listaClientes = DB.GetClients(txtBuscarCliente.Text);
                if (listaClientes != null)
                {

                    foreach (clients c in listaClientes)
                    {
                        Image img = new Image();
                        try
                        {
                            img = new Image() { Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["ImageClientsPath"] + c.image)) };
                        }
                        catch (Exception)
                        {
                            img = new Image() { Source = new BitmapImage(new Uri("http://www.lacasadelasnavajas.com/img/cms/recursos/producto-exclusivo.png")) };
                        }

                        StackPanel sp = new StackPanel();
                        Border border = new Border();
                        border.Child = sp;
                        //                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(100,100,100));
                        //                    border.BorderThickness = new Thickness(1);
                        Label lb = new Label();
                        lb.Width = 160;
                        lb.Height = 26;
                        lb.Margin = new Thickness(4, 0, 4, 0);
                        lb.Content = c.name;
                        lb.VerticalAlignment = VerticalAlignment.Top;
                        lb.VerticalContentAlignment = VerticalAlignment.Top;
                        lb.HorizontalContentAlignment = HorizontalAlignment.Center;

                        //Image img = new Image();

                        //try
                        //{
                        //    img = new Image() { Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["ImageClientsPath"] + c.image)) };
                        //}
                        //catch (Exception)
                        //{
                        //    img = new Image() { Source = new BitmapImage(new Uri("http://www.lacasadelasnavajas.com/img/cms/recursos/producto-exclusivo.png")) };
                        //}

                        ItemClient itemBusqueda = new ItemClient()
                        {
                            Id = c.id,
                            Rut = c.rut,
                            ClientName = c.name,
                            Contacto = c.contact,
                            PuntosAmmount = c.points.points_active,
                            Comment = c.comment,
                            Vip = c.vip,
                            BorderThickness = new Thickness(0),
                            Height = 120,
                            Width = 160,
                            Margin = new Thickness(4, 4, 4, 0),
                            VerticalContentAlignment = VerticalAlignment.Center,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            Content = img
                        };
                        string vipStr = "";
                        if (itemBusqueda.Vip == true)
                            vipStr = "Si";
                        else
                            vipStr = "No";
                        itemBusqueda.ToolTip = $"Rut: {itemBusqueda.Rut}\nNombre: {itemBusqueda.ClientName}\nContacto: {itemBusqueda.Contacto}\nPuntos: {itemBusqueda.PuntosAmmount}\nVip: {vipStr}\nComentario: {itemBusqueda.Comment}";
                        if (itemBusqueda.Content == null)
                            itemBusqueda.Content = img;

                        sp.Children.Add(itemBusqueda);
                        sp.Children.Add(lb);

                        wrapClientes.Children.Add(border);

                        itemBusqueda.Click += (se, ev) =>
                        {
                            ItemClient clienteSeleccionado = (ItemClient)se;
                            txtCliente_rut.Text = clienteSeleccionado.Rut;
                            bCerrado = true; Close();
                        };
                    }
                }
            }
        }

        */
    }
}
