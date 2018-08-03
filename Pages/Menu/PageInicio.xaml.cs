using posk.BLL;
using posk.Components;
using posk.Controls;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace posk.Pages.Menu
{
    public partial class PageInicio : Page, IDisposable
    {
        public List<TextBox> ListaTB { get; set; }
        public static event EventHandler AlIniciarSesionComoCajero;

        public PageInicio()
        {
            InitializeComponent();

            btnCerrar.Click += (se, a) => Application.Current.Shutdown();

            Loaded += (se, a) =>
            {
                MainWindow.BtnMenu.Visibility = Visibility.Hidden;
                MainWindow.BtnMenu.IsEnabled = false;
                btnLogin.Click += (se2, a2) =>
                {
                    expLogin.IsExpanded ^= true;
                    if (expLogin.IsExpanded)
                    {
                        spUsuarios.Children.Clear();
                        Init();
                    }
                };


                try
                {
                    imageLogo.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["RutaImagenLocal"] + DatosNegocioBLL.ObtenerImagenUrl()));
                }
                catch
                {
                    imageLogo.Source = null;
                }
            };
        }

        void IDisposable.Dispose()
        {

        }

        public void Init()
        {

            UsuarioBLL.GetAllUsers().ForEach(x =>
            {
                LoginItem li = new LoginItem();
                li.ID = x.id;
                li.nombre.Content = x.nombre;
                try
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(ConfigurationManager.AppSettings["RutaImagenUsuario"] + x.imagen);
                    image.EndInit();
                    li.foto.Source = image;
                }
                catch
                {
                    li.foto.Source = null;
                }
                try
                {
                    li.code1.MouseLeftButtonUp += (se, arg) =>
                    {
                        li.Pass += "1";
                        IntentarLogin(li);
                    };
                    li.code2.MouseLeftButtonUp += (se, arg) =>
                    {
                        li.Pass += "2";
                        IntentarLogin(li);
                    };
                    li.code3.MouseLeftButtonUp += (se, arg) =>
                    {
                        li.Pass += "3";
                        IntentarLogin(li);
                    };
                    li.code4.MouseLeftButtonUp += (se, arg) =>
                    {
                        li.Pass += "4";
                        IntentarLogin(li);
                    };
                }
                catch
                {
                    li.Pass = "";
                }
                //if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.RUA.ToString()) && x.tipo.ToUpper().Equals("G")) return;
                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()) && x.tipo.ToUpper().Equals("G")) return;
                if (GlobalSettings.Modo.Equals(GlobalSettings.ModoEnum.KUPAL.ToString()) && x.tipo.ToUpper().Equals("C")) return;
                spUsuarios.Children.Add(li);
            });



            /*
            ListaTB = new List<TextBox>() { txtUsuario, txtPass };
            ItemTeclado it = new ItemTeclado(ListaTB);
            it.VerticalAlignment = VerticalAlignment.Bottom;
            borderTeclado.Child = it;

            txtUsuario.GotFocus += (se, ev) => { it.expTeclado.IsExpanded = true; };
            txtUsuario.LostFocus += (se, ev) => { it.expTeclado.IsExpanded = false; };
            txtPass.GotFocus += (se, ev) => { it.expTeclado.IsExpanded = true; };
            txtPass.LostFocus += (se, ev) => { it.expTeclado.IsExpanded = false; };

            btnIngresar.Click += (se, ev) =>
            {
                usuario u = UsuarioBLL.Login(txtUsuario.Text, txtPass.Text);
                if (u != null)
                {
                    Settings.Nombre = u.nombre;
                    Settings.NombreUsuario = u.nombre_usuario;
                    Settings.Foto = u.imagen;
                    Settings.Tipo = u.tipo;
                    BitmapImage avatar = null;
                    try
                    {
                        avatar = new BitmapImage(new Uri($@"D:\posk\img\usuarios\{u.imagen}"));
                    }
                    catch
                    {
                        avatar = new BitmapImage(new Uri($@"D:\posk\img\usuarios\nullavatar.jpg"));
                    }
                    foto.ImageSource = avatar;
                    lbNombreUsuario.Content = u.nombre_usuario;
                    switch (Settings.Tipo)
                    {
                        case "A":
                            lbRol.Content = "Administrador";
                            frame.Content = new PageBienvenido();
                            break;
                        case "C":
                            lbRol.Content = "Cajero";
                            frame.Content = new PageVenta();
                            break;
                        case "G":
                            lbRol.Content = "Garzón";
                            frame.Content = new PageMesas();
                            break;
                    }
                    btnMenu.IsEnabled = true;
                }
                else
                    lbCapsLockInfo.Content = "INCORRECTO";
            };
            */
        }
        private void IntentarLogin(LoginItem li)
        {
            if (li.Pass.Contains(UsuarioBLL.ObtenerPass(li.ID)))
            {
                li.Pass = "";
                usuario u = UsuarioBLL.ObtenerUsuario(li.ID);
                if (u != null)
                {
                    Settings.ImpresoraCocina = SectorImpresionBLL.ObtenerImpresoraCocina();
                    Settings.ImpresoraBar = SectorImpresionBLL.ObtenerImpresoraBar();
                    Settings.Usuario = u;
                    Settings.Nombre = u.nombre;
                    Settings.NombreUsuario = u.nombre_usuario;
                    Settings.Foto = u.imagen;
                    Settings.Tipo = u.tipo;
                    Settings.NombreDelNegocio = DatosNegocioBLL.ObtenerNombreNegocio();
                    //EnviarCorreo.Enviar($"{u.nombre} ha iniciado sesión\n{DateTime.Now}");
                    BitmapImage avatar = null;
                    try
                    {
                        avatar = new BitmapImage(new Uri($@"{ConfigurationManager.AppSettings["RutaImagenUsuario"]}\{u.imagen}"));
                    }
                    catch
                    {
                        avatar = new BitmapImage(new Uri($@"{ConfigurationManager.AppSettings["RutaImagenUsuario"]}\nullavatar.jpg"));
                    }
                    MainWindow.FotoUsuario.ImageSource = avatar;
                    MainWindow.LbNombreUsuario.Content = u.nombre_usuario;
                    switch (Settings.Tipo)
                    {
                        case "A":
                            MainWindow.LbRol.Content = "Administrador";
                            MainWindow.MainFrame.Content = new PageBienvenido();
                            break;
                        case "C":
                            MainWindow.LbRol.Content = "Cajero";
                            AlIniciarSesionComoCajero.Invoke(this, null);
                            MainWindow.MainFrame.Content = new PrincipalComponent("VENTA");
                            break;
                        case "G":
                            MainWindow.LbRol.Content = "Garzón";
                            //MainWindow.MainFrame.Content = new PageMesas();
                            MainWindow.MainFrame.Content = new PrincipalComponent("VENTA");
                            //AlIniciarSesion.Invoke(this, "G");
                            break;
                        case "R":
                            MainWindow.LbRol.Content = "Cajero Restaurant";
                            JornadaBLL.CrearJornadaSiNoExiste();
                            //MainWindow.MainFrame.Content = new PageMesas();
                            break;
                    }
                    MainWindow.BtnMenu.Visibility = Visibility.Visible;
                    MainWindow.BtnMenu.IsEnabled = true;
                }
            }
        }
    }
}
