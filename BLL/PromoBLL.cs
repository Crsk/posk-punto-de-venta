using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PromoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<promocione> ObtenerTodo()
        {
            return db.promociones.ToList();
        }

        public static List<producto_promocion> ObtenerDetallePromocion(int id)
        {
            return db.producto_promocion.Where(x => x.promocion_id == id).ToList();
        }

        public static int ObtenerCantidadItems(int? promoId)
        {
            return ObtenerProductosPromocion(promoId).Count;
        }

        public static List<producto_promocion> ObtenerProductosPromocion(int? idPromocion)
        {
            return db.producto_promocion.Where(x => x.promocion_id == idPromocion).ToList();
        }

        public static void EliminarDetalle(int? promoID)
        {
            List<producto_promocion> listaDetallePromocion = db.producto_promocion.Where(x => x.promocion_id == promoID).ToList();
            foreach (producto_promocion dp in listaDetallePromocion)
            {
                db.producto_promocion.Remove(dp);
            }
            db.SaveChanges();
        }

        public static void Actualizar(int? promoID, List<producto> listaDetallePromo)
        {
            EliminarDetalle(promoID);

            listaDetallePromo.ForEach(x =>
            {
                producto_promocion dp = new producto_promocion();
                dp.producto_id = x.id;
                dp.promocion_id = promoID;
                db.producto_promocion.Add(dp);
            });
            db.SaveChanges();
        }

        public static promocione Crear(List<producto> listaProductosPromo)
        {
            promocione promo = new promocione();
            listaProductosPromo.ForEach(x =>
            {
                producto_promocion lineaPromo = new producto_promocion();
                lineaPromo.producto = x;
                lineaPromo.promocione = promo;
                db.producto_promocion.Add(lineaPromo);
            });
            db.promociones.Add(promo);
            db.SaveChanges();
            return promo;
        }
    }
}
