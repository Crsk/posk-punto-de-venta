using posk.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using posk.Models;

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




                jornada j = JornadaBLL.UltimaJornada();
                List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
                // List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();
                //listaPromosCantidad = ContarPromosEnLista(j.id);
                listaProductoCantidad = ContarProductosEnLista(j.id);

                body += "\n\n---------------\n";
                listaProductoCantidad.ForEach(pc =>
                {
                    body += $"{pc.Producto.nombre}\nOPCIÓN: {pc.Opcion}\nCANTIDAD: {pc.Cantidad}\nSUB TOTAL: {pc.SubTotal}\nADICIONAL: {pc.Adicional}\nTOTAL: {pc.Total}\n---------------\n";
                });







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














        struct ProductoCantidad
        {
            public producto Producto { get; set; }
            public string Opcion { get; set; }
            public int Cantidad { get; set; }
            public decimal? SubTotal { get; set; }
            public decimal? Adicional { get; set; }
            public decimal? Total { get; set; }

            public void AumentarCantidad(int cantidad)
            {
                Cantidad += cantidad;
            }
        }
        struct PromoCantidad
        {
            public promocione Promo { get; set; }
            public decimal? Cantidad { get; set; }
        }





        /*
        private static List<PromoCantidad> ContarPromosEnLista(int jornadaId)
        {
            List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();

            VentasJornadaBLL.ObtenerTodo().ForEach(vj =>
            {
                if (vj.detalle_boleta.promocion_id != null && vj.jornada_id == jornadaId)
                    listaPromosCantidad.Add(new PromoCantidad() { Promo = vj.detalle_boleta.promocione, Cantidad = vj.cantidad });
            });

            return listaPromosCantidad.OrderBy(x => x.Cantidad).Reverse().ToList();
        }
        */
        private static List<ProductoCantidad> ContarProductosEnLista(int jornadaId)
        {
            List<ventas_jornada> listaVentasJornada = VentasJornadaBLL.ObtenerVentasJornada(jornadaId);
            List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
            List<ProductoCantidad> listaProductoCantidadAgrupado = new List<ProductoCantidad>();
            listaVentasJornada.ForEach(vj => AgregarProductoCantidad(listaProductoCantidad, vj));
            foreach (var vj in listaVentasJornada)
            {
                ProductoCantidad pcExistente = listaProductoCantidadAgrupado.Where(x => x.Producto.id == vj.detalle_boleta.producto.id && x.Opcion == vj.opcion).FirstOrDefault();
                if (pcExistente.Producto != null && vj.opcion == pcExistente.Opcion)
                    AgregarProductoCantidadExistente(listaProductoCantidadAgrupado, vj, pcExistente);
                else
                    AgregarProductoCantidad(listaProductoCantidadAgrupado, vj);
            }
            return listaProductoCantidadAgrupado.OrderBy(x => x.Cantidad).OrderBy(x => x.Producto.nombre).OrderBy(x => x.Opcion).Reverse().ToList();
        }

        private static void AgregarProductoCantidadExistente(List<ProductoCantidad> listaProductoCantidad, ventas_jornada vj, ProductoCantidad pcExistente)
        {
            listaProductoCantidad.Remove(pcExistente);
            listaProductoCantidad.Add(new ProductoCantidad()
            {
                Producto = vj.detalle_boleta.producto,
                Cantidad = pcExistente.Cantidad + vj.cantidad,
                Opcion = vj.opcion,
                Adicional = vj.cobro_extra + pcExistente.Adicional,
                SubTotal = vj.detalle_boleta.producto.precio * (vj.cantidad + pcExistente.Cantidad),
                Total = vj.detalle_boleta.producto.precio * (vj.cantidad + pcExistente.Cantidad) + vj.cobro_extra
            });
        }

        private static void AgregarProductoCantidad(List<ProductoCantidad> listaProductoCantidad, ventas_jornada vj)
        {
            listaProductoCantidad.Add(new ProductoCantidad()
            {
                Producto = vj.detalle_boleta.producto,
                Cantidad = vj.cantidad,
                Opcion = vj.opcion,
                Adicional = vj.cobro_extra,
                SubTotal = vj.detalle_boleta.producto.precio * vj.cantidad,
                Total = vj.detalle_boleta.producto.precio * vj.cantidad + vj.cobro_extra
            });
        }
    }
}
