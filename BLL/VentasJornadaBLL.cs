using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class VentasJornadaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void Agregar(int jid, int detalleBoletaId, string opcion, int cantidad, int cobroExtra)
        {
            db.ventas_jornada.Add(new ventas_jornada() { jornada_id = jid, detalle_boleta_id = detalleBoletaId, opcion = opcion == null ? "" : opcion, cantidad = cantidad, cobro_extra = cobroExtra, fecha = DateTime.Now });
            db.SaveChanges();
        }

        public static List<ventas_jornada> ObtenerTodo()
        {
            return db.ventas_jornada.Include("detalle_boleta").ToList();
        }

        public static List<ventas_jornada> ObtenerVentasJornada(int jornadaId)
        {
            return db.ventas_jornada.Include("detalle_boleta").Where(x => x.jornada_id == jornadaId).ToList();
        }

        public static List<ventas_jornada> ObtenerVentasRangoFechas(DateTime desde, DateTime hasta)
        {
            return db.ventas_jornada.Include("detalle_boleta").Where(x => x.fecha > desde && x.fecha < hasta).ToList();
        }

        public static List<ventas_jornada> ObtenerDistintos()
        {
            return db.ventas_jornada.Include("detalle_boleta").GroupBy(p => p.opcion).Select(grp => grp.FirstOrDefault()).ToList();
        }
    }
}
