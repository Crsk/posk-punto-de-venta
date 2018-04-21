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

        public static void Agregar(int jid, int? prodId, int? promoId, decimal? cantidad, int cobroExtra)
        {
            //si existe, agregar cantidad
            ventas_jornada vj = db.ventas_jornada.Where(x => x.jornada_id == jid && x.producto_id == prodId).FirstOrDefault();
            ventas_jornada vjPromo = db.ventas_jornada.Where(x => x.jornada_id == jid && x.promo_id == promoId).FirstOrDefault();

            if (vj?.producto_id != null)
            {
                vj.cantidad += cantidad;
                vj.cobro_extra += cobroExtra;
            }
            else if (vjPromo?.promo_id != null)
            {
                vjPromo.cantidad += cantidad;
                vj.cobro_extra += cobroExtra;
            }
            else
                db.ventas_jornada.Add(new ventas_jornada() { jornada_id = jid, producto_id = prodId, promo_id = promoId, cantidad = cantidad, cobro_extra = cobroExtra });
            db.SaveChanges();
        }

        public static List<ventas_jornada> ObtenerTodo()
        {
            return db.ventas_jornada.Include("producto").Include("promocione").ToList();
        }
    }
}
