using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class ProductoPromocionBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static producto_promocion Crear(int? productoID, int? promoID)
        {
            producto_promocion pp = new producto_promocion() { promocion_id = promoID, producto_id = productoID };
            db.producto_promocion.Add(pp);
            db.SaveChanges();
            return pp;
        }

        public static List<producto> ObtenerProductos(int? promocionID)
        {
            List<producto> listaProductos = new List<producto>();
            try
            {
                db.producto_promocion.Include("producto").Where(pp => pp.promocion_id == promocionID).ToList().ForEach(pp2 =>
                    listaProductos.Add(db.productos.Where(c => c.id == pp2.producto.id).FirstOrDefault()));
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al agregar item a promo");
            }
            return listaProductos;
        }

        public static void Eliminar(int productoID)
        {
            db.producto_promocion.Remove(db.producto_promocion.Where(x => x.producto_id == productoID).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
