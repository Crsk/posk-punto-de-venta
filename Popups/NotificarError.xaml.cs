using posk.Popup;
using System;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace posk.Popups
{
    public partial class NotificarError : Window
    {
        public NotificarError(string titulo, string descripcion, Exception ex)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            lbMensaje.Text = titulo.ToUpper();
            tbMensajeSecundario.Text = descripcion;
            Topmost = true;

            btnNada.Click += (se, a) => Close();
            btnEnviarCorreo.Click += (se, a) => 
            {
                // enviar correo
                try
                {
                    var fromAddress = new MailAddress("poskerror@gmail.com", "Posk error");
                    var toAddress = new MailAddress("crsk@mail.com", "Christopher Kiessling (Soporte Posk)");
                    const string fromPassword = "MyErrorPass.12";
                    const string subject = "POSK ERROR (RuaBar)";
                    string body = descripcion + "\n\n" + ex.ToString();

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }
                    Close();

                    new Notification("Enviado");
                }
                catch
                {
                    new Notification("NO SE PUDO ENVIAR", "", Notification.Type.Danger);
                    Close();
                }
            };

            Show();
        }
    }
}
