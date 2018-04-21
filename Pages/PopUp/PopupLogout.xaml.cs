using posk.Globals;
using System.Windows;

namespace posk.Pages.PopUp
{
    public partial class PopupLogout : Window
    {
        bool bCerrado = false; // necesario para cerrar la ventana al pasar a segundo plano y evitar error

        public PopupLogout()
        {
            InitializeComponent();
            this.Deactivated += (se, ev) => { if (!bCerrado) Close(); };
            btnCerrar.Click += (se, ev) => { bCerrado = true; Close(); };
            btnDejarMensaje.Click += (se, ev) => { LogoutMessage.Message = txtMessage.Text.Trim(); bCerrado = true; Close(); };
        }
    }
}
