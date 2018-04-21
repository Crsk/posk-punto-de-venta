using posk.BLL;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace posk.Globals
{
    public static class InformeJornada
    {
        public static void EnviarInformeJornada()
        {
            string nombreNegocio = DatosNegocioBLL.ObtenerNombreNegocio();
            string correoPrimarioStr = DatosNegocioBLL.ObtenerCorreoPrimario();
            string correoSecundarioStr = DatosNegocioBLL.ObtenerCorreoSecundario();

            if (!String.IsNullOrEmpty(correoPrimarioStr))
                EnviarCorreo(new MailAddress(correoPrimarioStr, $"Informe {nombreNegocio}"));
            if (!String.IsNullOrEmpty(correoSecundarioStr))
                EnviarCorreo(new MailAddress(correoSecundarioStr, $"Informe {nombreNegocio}"));
        }

        private static void EnviarCorreo(MailAddress correo)
        {
            try
            {
                string nombreNegocio = DatosNegocioBLL.ObtenerNombreNegocio();

                var fromAddress = new MailAddress("poskerror@gmail.com", nombreNegocio);
                // TODO implementar variable CorreoDestinatario
                // var toAddress = new MailAddress($"{ Settings.CorreoDestinatario }", "Informe Posk");
                const string fromPassword = "MyErrorPass.12";
                string subject = $"Informe jornada { nombreNegocio }";

                var jornadaActual = JornadaBLL.UltimaJornada();
                DateTime? aperturaJornada = jornadaActual.fecha_apertura;
                int ingresosJornada = BoletaBLL.ObtenerIngresosTotal(aperturaJornada, DateTime.Now);

                var mermas = MermaBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var devoluciones = DevolucionBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var gastos = GastoBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var cajaInicial = JornadaBLL.ObtenerCajaInicialJornadaActual();
                var ventas = BoletaBLL.ObtenerIngresosTotal(aperturaJornada, DateTime.Now);

                string gastosString = "";

                GastoBLL.ObtenerGastosPeriodo(aperturaJornada, DateTime.Now).ForEach(gasto =>
                {
                    gastosString += $"\n${gasto.monto} a las {gasto.fecha.Value.ToShortTimeString()} \t\t{gasto.detalle}";
                });

                string body =
                    $"Informe jornada\n" +
                    $"Desde: { aperturaJornada }\n" +
                    $"Hasta: { DateTime.Now }\n" +
                    $"Usuario que inició jornada: { jornadaActual.usuario.nombre }\n" +
                    $"Usuario que terminó jornada: { Settings.Usuario.nombre }\n" +
                    $"Ventas realizadas: { BoletaBLL.CantidadBoletasPorPeriodo(aperturaJornada, DateTime.Now) }\n\n" +

                    $"Mermas: { mermas }\n" +
                    $"Devoluciones: { devoluciones }\n" +
                    $"Gastos: { gastos }\n" +
                    $"Caja inicial: { cajaInicial }\n\n" +

                    $"Desglose gastos: { gastosString }\n\n" +

                    $"Ventas: { ventas }";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, correo)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch
            {
            }
        }
    }
}
