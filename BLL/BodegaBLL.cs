using posk.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace posk.BLL
{
    static class BodegaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void CrearActualizar(int productoId, decimal stock, bool bSaleDeStock)
        {
            bodega_stock bs = db.bodega_stock.Where(x => x.producto_id == productoId).FirstOrDefault();
            if (bs != null)
            {
                if (bSaleDeStock) bs.stock -= stock;
                else bs.stock += stock;
            }
            else
            {
                db.bodega_stock.Add(new bodega_stock() { producto_id = productoId, stock = stock });
            }
            db.SaveChanges();
        }

        public static void BorrarTodo()
        {
            db.bodega_stock.ToList().ForEach(x => db.bodega_stock.Remove(x));
            db.SaveChanges();
        }

        public static decimal ObtenerStock(int productoId)
        {
            return db.bodega_stock.AsNoTracking().Where(x => x.producto_id == productoId).FirstOrDefault()?.stock ?? 0;
        }
    }
}
