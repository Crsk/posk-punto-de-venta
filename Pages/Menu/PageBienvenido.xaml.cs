using posk.BLL;
using posk.Globals;
using posk.Popup;
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

namespace posk.Pages.Menu
{
    public partial class PageBienvenido : Page
    {
        public static event EventHandler AlIniciarJornadaDesdeAdmin;

        public PageBienvenido()
        {
            InitializeComponent();
            lbBienvenida.Content = $"Hola {Settings.NombreUsuario}";
            
            Loaded += (se, a) => Events();
            MainWindow.AlIniciarJornada += (se, a) =>
            {
                expTerminarJornada.IsExpanded = true;
                expIniciarJornada.IsExpanded = false;
            };
        }

        private void Events()
        {

            if (JornadaBLL.UltimaJornadaTerminada())
            {
                expTerminarJornada.IsExpanded = false;
                expIniciarJornada.IsExpanded = true;
            }
            else
            {
                expTerminarJornada.IsExpanded = true;
                expIniciarJornada.IsExpanded = false;
            }

            txtMensajeJornadaEspecial.TextChanged += (se, ev) =>
            {
                if (txtMensajeJornadaEspecial.Text != "")
                    checkJornadaEspecial.IsChecked = true;
                else
                    checkJornadaEspecial.IsChecked = false;
            };

            btnComenzarJornada.Click += (se, ev) =>
            {
                AlIniciarJornadaDesdeAdmin.Invoke(this, null);
                JornadaBLL.CrearJornadaSiNoExiste(txtMensajeJornadaEspecial.Text);
                expTerminarJornada.IsExpanded = true;
                expIniciarJornada.IsExpanded = false;
                new Notification("JORNADA INICIADA");
            };

            btnCerrarJornada.Click += (se, ev) =>
            {
                JornadaBLL.TerminarJornadaSiEstaIniciada(DateTime.Now, 0, txtMensajeJornadaEspecial.Text);
                expTerminarJornada.IsExpanded = false;
                expIniciarJornada.IsExpanded = true;
                new Notification("JORNADA TERMINADA");
            };
        }

    }
}
