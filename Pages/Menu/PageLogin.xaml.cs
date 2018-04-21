using posk.Globals;
using System.Windows;
using System.Windows.Controls;
using System;
using posk.BLL;
using posk.Models;
using System.Configuration;
using posk.Pages.PopUp;

namespace posk.Pages.Menu
{
    public partial class PageLogin : Page
    {
        Expander expMenu;

        public PageLogin(Expander expMenu)
        {
            InitializeComponent();

            frameTest.Navigate(new PageInicioLogo());
            
            this.expMenu = expMenu;
            txtUser.Text = "";
            txtPass.Password = "";

            btnAdmin.Click += (se, ev) =>
            {
                txtUser.Text = "Admin";
                txtPass.Password = "123";
                checkGuardarContrasena.IsChecked = true;
                btnIngresar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };
            btnCK.Click += (se, ev) =>
            {
                txtUser.Text = "Christopher Kiessling";
                txtPass.Password = "123456789";
                checkGuardarContrasena.IsChecked = true;
                btnIngresar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            };
        }

        private void Logout()
        {
            txtUser.Clear();
            txtPass.Clear();
            checkGuardarContrasena.IsChecked = false;
            btnIngresar.Content = "LOGIN";
            Settings.NombreUsuario = null;
            Settings.Tipo = "";
            Settings.bMenuIsAlreadyCreated = false;
            Settings.bLoggedIn = false;
            lbDebug.Content = "";
            frameTest.Navigate(new PageInicioLogo());
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            expMenu.IsExpanded = false;
            if (btnIngresar.Content.Equals("LOGIN"))
            {
                if (txtPass.Password == /*posk.Properties.Settings.Default.Activado*/ "")
                {
                    Settings.bLoggedIn = true;
                    ConfigurationManager.AppSettings["LoggedUser"] = "test";
                    Settings.Tipo = "";
                    expLogin.IsExpanded = false;
                    // expMenu.IsExpanded = true;
                }
                else
                {

                    usuario user = UsuarioBLL.Login(txtUser.Text, txtPass.Password);

                    if (user != null)
                    {
                        Settings.bLoggedIn = true;
                        Settings.UserID = user.id;
                        Settings.NombreUsuario = user.nombre;
                        ConfigurationManager.AppSettings["LoggedUser"] = Settings.NombreUsuario;
                        Settings.Tipo = user.tipo;
                    }
                    else
                    {
                        lbDebug.Content = "DEBUG: NO COINCIDE";
                        return;
                    }

                    if (Settings.NombreUsuario != null)
                    {
                        Settings.bMenuIsAlreadyCreated = false;
                        btnIngresar.Content = "LOGOUT";
                        lbDebug.Content = "DEBUG: HI " + Settings.NombreUsuario + " ADMIN = " + Settings.Tipo;
                        expLogin.IsExpanded = false;
                        // expMenu.IsExpanded = true;

                        frameTest.Navigate(new PageInicioAdmin());
                    }
                }
            }
            else
            {
                PopupLogout pl = new PopupLogout();
                pl.Show();
                Logout();
            }
        }
    }
}
