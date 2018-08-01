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
                string subject = $"Informe jornada { nombreNegocio }".ToUpper();

                var jornadaActual = JornadaBLL.UltimaJornada();
                DateTime? aperturaJornada = jornadaActual.fecha_apertura;
                int ingresosJornada = BoletaBLL.ObtenerIngresosTotal(aperturaJornada, DateTime.Now);

                var mermas = MermaBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var devoluciones = DevolucionBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var gastos = GastoBLL.ObtenerValor(aperturaJornada, DateTime.Now);
                var cajaInicial = JornadaBLL.ObtenerCajaInicialJornadaActual();
                var ventasTotal = BoletaBLL.ObtenerIngresosTotal(aperturaJornada, DateTime.Now);
                var ingresosEfectivo = BoletaBLL.ObtenerIngresosEfectivo(aperturaJornada, DateTime.Now);
                var ingresosTransBank = BoletaBLL.ObtenerIngresosTransBank(aperturaJornada, DateTime.Now);
                var ingresosJunaeb = BoletaBLL.ObtenerIngresosJunaeb(aperturaJornada, DateTime.Now);
                var ingresosOtro = BoletaBLL.ObtenerIngresosOtro(aperturaJornada, DateTime.Now);
                var ingresosPropina = BoletaBLL.ObtenerPropinas(aperturaJornada, DateTime.Now);


                string body =
                    "<table>" +
                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Informe de Jornada" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Desde" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{aperturaJornada}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Hasta" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{DateTime.Now}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Usuario inicio" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{jornadaActual.usuario.nombre}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Usuario término" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{Settings.Usuario.nombre}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Número de ventas" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{BoletaBLL.CantidadBoletasPorPeriodo(aperturaJornada, DateTime.Now)}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d61804;'>" +
                            "Gastos" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d61804;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d61804;'>" +
                            $"${gastos}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            "Mermas" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            $"${mermas}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            "Devoluciones" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            $"${devoluciones}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            "Caja inicial" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            $"${cajaInicial}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            "Propinas TransBank" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #d68504;'>" +
                            $"${ingresosPropina}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            "Ventas Efectivo" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            $"${ingresosEfectivo}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            "Ventas TransBank" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            $"${ingresosTransBank}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            "Ventas Junaeb" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            $"${ingresosJunaeb}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            "Ventas Otro" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #048762;'>" +
                            $"${ingresosOtro}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 18px; color: #048762;'>" +
                            "Ventas Total" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 18px; color: #048762;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 18px; color: #048762;'>" +
                            $"${ventasTotal}" +
                        "</td>" +
                    "</tr>" +

                "</table>";

                List<gasto> listaGastos = GastoBLL.ObtenerGastosPeriodo(aperturaJornada, DateTime.Now);

                if (listaGastos.Count > 0)
                {
                    body += "</br><p style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>Lista de gastos: </p>";
                    body += "<table>";
                    listaGastos.ForEach(gasto =>
                    {
                        body +=
                        "<tr>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                $"{gasto.detalle} ({gasto.fecha.Value.ToShortTimeString()} hrs.)" +
                            "</td>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                ":" +
                            "</td>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                $"${gasto.monto}" +
                            "</td>" +
                        "</tr>";
                    });
                    body += "</table>";
                }



                jornada j = JornadaBLL.UltimaJornada();
                List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
                // List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();
                //listaPromosCantidad = ContarPromosEnLista(j.id);
                listaProductoCantidad = ContarProductosEnLista(j.id);

                body += "</br><p style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>Resúmen de ventas: </p><table>";
                listaProductoCantidad.ForEach(pc =>
                {
                    bool bTieneOpcion = pc.Opcion == "" ? false : true;

                    body +=
                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Item venta" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{pc.Producto.nombre}" +
                        "</td>" +
                    "</tr>";

                    if (bTieneOpcion == true)
                    {
                        body +=
                        "<tr>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                "Opción" +
                            "</td>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                ":" +
                            "</td>" +
                            "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                                $"{pc.Opcion}" +
                            "</td>" +
                        "</tr>";
                    }

                    body +=
                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Cantidad" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{pc.Cantidad}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "SubTotal" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{pc.SubTotal}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Adicional" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{pc.Adicional}" +
                        "</td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            "Total" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            ":" +
                        "</td>" +
                        "<td style='font-family: Roboto, Helvetica, sans-serif; font-size: 14px; color: #515151;'>" +
                            $"{pc.Total}" +
                        "</td>" +
                    "</tr><tr><td></br></td></tr>";
                });

                body += "</table>";


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
                    Body = body,
                    IsBodyHtml = true
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
