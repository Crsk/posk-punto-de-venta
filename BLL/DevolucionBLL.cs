using System;
using posk.Models;
using System.Collections.Generic;
using System.Linq;
using posk.Globals;

namespace posk.BLL
{
    static class DevolucionBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void Agregar(int? productoId, int? promoId, decimal? cantidad, int monto)
        {
            try
            {
                if (productoId != null)
                {
                    db.devolucions.Add(new devolucion() { producto_id = (int)productoId, cantidad = (decimal)cantidad, fecha = DateTime.Now, monto = monto });
                    CompraBLL.AumentarStock((int)productoId, (decimal)cantidad);
                }
                else if (promoId != null)
                {
                    int cantidadItemEnPromo = PromoBLL.ObtenerCantidadItems(promoId);
                    ProductoPromocionBLL.ObtenerProductos(promoId).ForEach(productoEnPromocion =>
                    {
                        db.devolucions.Add(new devolucion() { producto_id = productoEnPromocion.id, cantidad = (decimal)cantidad, fecha = DateTime.Now, monto = monto/cantidadItemEnPromo });
                        CompraBLL.AumentarStock(productoEnPromocion.id, (decimal)cantidad);
                    });
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL DEVOLVER");
            }
        }

        public static List<devolucion> ObtenerPorPeriodo(DateTime? inicio, DateTime? fin)
        {
            return db.devolucions.Include("producto").Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
        }

        public static int ObtenerCantidadItemsDevueltos(DateTime? inicio, DateTime? fin)
        {
            decimal cantidad = 0;
            db.devolucions.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList().ForEach(dev => cantidad += dev.cantidad);
            return Convert.ToInt32(cantidad);
        }

        public static int ObtenerValor(DateTime? inicio, DateTime? fin)
        {
            decimal? totales = 0;
            List<devolucion> listaDevolucionesPeriodo = db.devolucions.Include("producto").Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
            foreach (devolucion item in listaDevolucionesPeriodo)
            {
                if (item.monto != null)
                    totales += item.monto;
            }
            return Convert.ToInt32(totales);
        }
    }
}
