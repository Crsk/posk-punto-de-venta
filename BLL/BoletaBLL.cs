using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class BoletaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public struct PuntoIngresoPorPeriodo
        {
            public DateTime Fecha { get; set; }
            public int Monto { get; set; }
        }

        public static List<boleta> ObtenerPorCliente(int clienteId)
        {
            return db.boletas.Where(x => x.cliente_id == clienteId).ToList();
        }

        public static boleta ObtenerUltima()
        {
            return db.boletas.ToList().OrderBy(x => x.id).LastOrDefault();
        }

        public static boleta Set(int client_id, int userId, int points_amount, int total, int propina, int? cliente_id = null)
        {
            int numeroUltimaBoleta = ObtenerUltimoNumeroBleta();

            boleta ts = new boleta();
            if (client_id != 0)
                ts.cliente_id = client_id;
            else
                ts.cliente_id = null;
            ts.puntos_cantidad = points_amount;
            ts.fecha = DateTime.Now;
            ts.usuario_id = userId;
            ts.total = total;
            ts.propina = propina;
            ts.numero_boleta = ++numeroUltimaBoleta;
            ts.cliente_id = cliente_id;
            db.boletas.Add(ts);
            db.SaveChanges();
            return ts;
        }

        public static int ObtenerUltimoNumeroBleta()
        {
            int numeroUltimaBoleta = 0;
            boleta ultimaboleta = db.boletas.AsNoTracking().OrderBy(x => x.id).ToList().LastOrDefault();


            if (ultimaboleta.numero_boleta == null)
                numeroUltimaBoleta = 0;
            else
                numeroUltimaBoleta = (int)ultimaboleta.numero_boleta;

            return numeroUltimaBoleta;
        }

        public static void Delete(int id)
        {
            db.boletas.Remove(db.boletas.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static List<boleta> GetLast(int cantidad)
        {
            return db.boletas.ToList();
        }

        public static List<producto> ObtenerProductosVendidosPeriodo(DateTime inicio, DateTime fin)
        {
            List<producto> listaProductos = new List<producto>();
            ObtenerUltimasBoletasPorPeriodo(inicio, fin).ForEach(boleta => 
            {
                boleta.detalle_boleta.ToList().ForEach(detalleBoleta => 
                {
                    listaProductos.Add(detalleBoleta.producto);
                });
            });
            return listaProductos;
        }

        public static List<boleta> ObtenerUltimas(int cantidad)
        {
            return db.boletas.AsNoTracking().OrderByDescending(x => x.fecha).Take(cantidad).ToList();
        }

        public static int ObtenerCantidadItemsVendidos(DateTime? fechaInicio, DateTime? fechaTermino)
        {
            decimal? contadorItems = 0;
            ObtenerUltimasBoletasPorPeriodo(fechaInicio, fechaTermino).ForEach(boleta =>
            {
                DetalleBoletaBLL.ObtenerPorBoletaId(boleta.id).ForEach(lineaBoleta =>
                {
                    if (lineaBoleta.promocione != null)
                    {
                        int contadorProductosPromo = 0;
                        lineaBoleta.promocione.producto_promocion.ToList().ForEach(pp =>
                        {
                            contadorProductosPromo++;
                        });
                        contadorItems += contadorProductosPromo * lineaBoleta.cantidad;
                    }
                    else
                        contadorItems += lineaBoleta.cantidad;
                });
            });
            return Convert.ToInt32(contadorItems);
        }

        public static int ObtenerIngresosTotal(DateTime? inicio, DateTime? fin)
        {
            int? totales = 0;
            List<boleta> listaBoletasPeriodo = db.boletas.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
            foreach (boleta item in listaBoletasPeriodo)
            {
                totales += item.total;
            }
            return Convert.ToInt32(totales);
        }

        public static int ObtenerIngresosEfectivo(DateTime? inicio, DateTime? fin)
        {
            return BoletaMediopagoBLL.ObtenerMonto(inicio, fin, 1);
        }

        public static int ObtenerIngresosTransBank(DateTime? inicio, DateTime? fin)
        {
            return BoletaMediopagoBLL.ObtenerMonto(inicio, fin, 2);
        }

        public static int ObtenerIngresosJunaeb(DateTime? inicio, DateTime? fin)
        {
            return BoletaMediopagoBLL.ObtenerMonto(inicio, fin, 3);
        }

        public static int ObtenerIngresosOtro(DateTime? inicio, DateTime? fin)
        {
            return BoletaMediopagoBLL.ObtenerMonto(inicio, fin, 4);
        }

        public static int ObtenerPropinas(DateTime? inicio, DateTime? fin)
        {
            return BoletaMediopagoBLL.ObtenerPropinas(inicio, fin);
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <returns></returns>
        public static int GetIngresosPeriodo()
        {

            TimeSpan horaApertura = DatosNegocioBLL.GetHoraInicioJornada();
            TimeSpan mediaNoche = new TimeSpan(00, 00, 01);
            DateTime dtApertura = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaApertura.Hours, horaApertura.Minutes, horaApertura.Seconds);

            // si son mas de las 00 y la jornada comienza antes de las 00, descontar un día al inicio de la jornada para calibrar fecha de apertura
            // el sistema debe guardar en una tabla temporal de la base de datos los datos de la fecha de apertura
            //if (DateTime.Now.Date.Day > dtApertura.Date.Day)
            dtApertura = dtApertura.Date.AddDays(-1);

            TimeSpan horaCierre = DatosNegocioBLL.GetHoraTerminoJornada();
            DateTime dtCierre = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaCierre.Hours, horaCierre.Minutes, horaCierre.Seconds);

            if (!DatosNegocioBLL.JornadeDeUnDia())
                dtCierre = dtCierre.Date.AddDays(1);

            List<boleta> listaBoletasPeriodo = db.boletas.Where(x => x.fecha >= dtApertura && x.fecha <= dtCierre).ToList();

            int ingresosPeriodo = 0;
            foreach (boleta b in listaBoletasPeriodo)
            {
                ingresosPeriodo += (int)b.total;
            }
            return ingresosPeriodo;
        }

        struct Ingreso
        {
            public DateTime Inicio { get; set; }
            public DateTime Fin { get; set; }
            public int Monto { get; set; }
        };

        public static int GetMayorIngresoUltimosDias(int dias)
        {
            DateTime inicio = DateTime.Now.AddDays(-dias);
            List<boleta> listaBoletas = db.boletas.Where(x => x.fecha >= inicio).ToList();
            int mayorIngreso = 0;

            List<Ingreso> ingresos = new List<Ingreso>();
            Ingreso ing = new Ingreso();
            for (int i = -dias; i <= -1; i++)
            {
                if (DatosNegocioBLL.JornadeDeUnDia())
                {
                    ing.Inicio = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraInicioJornada());
                    ing.Fin = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraTerminoJornada());
                }
                else
                {
                    ing.Inicio = DateTime.Now.AddDays(i).Date.Add(DatosNegocioBLL.GetHoraInicioJornada());
                    ing.Fin = DateTime.Now.AddDays(i + 1).Date.Add(DatosNegocioBLL.GetHoraTerminoJornada());
                }
                ing.Monto = BoletaBLL.ObtenerIngresosTotal(ing.Inicio, ing.Fin);

                if (ing.Monto > mayorIngreso) mayorIngreso = ing.Monto;

                ingresos.Add(ing);
            }
            return mayorIngreso;
        }

        public static int GetIngresos12Horas()
        {
            int totales = 0;
            DateTime menos12horas = DateTime.Now.AddHours(-12);
            List<boleta> listaBoletasUltimas12Horas = db.boletas.Where(x => x.fecha > menos12horas).ToList();
            foreach (var item in listaBoletasUltimas12Horas)
            {
                //totales += item.total;
            }
            return totales;
        }

        /// <summary>
        /// Depreated
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public static int GetIngresosPorPeriodo(DateTime inicio, DateTime fin)
        {
            int ventas = 0;
            List<boleta> listaVentas = db.boletas.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();

            foreach (var item in listaVentas)
            {
                //ventas += item.total;
            }
            return ventas;
        }

        public static List<boleta> ObtenerUltimasBoletasPorPeriodo(DateTime? inicio, DateTime? fin)
        {
            List<boleta> lista = db.boletas.Include("cliente").Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
            lista.Reverse();
            return lista;
        }

        public static boleta Ultima()
        {
            return db.boletas.LastOrDefault();
        }

        public static int CantidadBoletasPorPeriodo(DateTime? inicio, DateTime? fin)
        {
            return ObtenerUltimasBoletasPorPeriodo(inicio, fin).Count;
        }

        public static int GetIngresosPorUsuario(int userID, DateTime desde, DateTime hasta)
        {
            int ventas = 0;
            List<boleta> listaVentasPorUsuario = db.boletas.Where(x => x.usuario_id == userID && x.fecha >= desde && x.fecha <= hasta).ToList();
            foreach (var item in listaVentasPorUsuario)
            {
                //ventas += item.total;
            }
            return ventas;
        }
    }
}
