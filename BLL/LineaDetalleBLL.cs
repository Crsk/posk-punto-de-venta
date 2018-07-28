using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class DetalleBoletaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<detalle_boleta> ObtenerPorPeriodo(DateTime? inicio, DateTime fin)
        {
            return db.detalle_boleta.Where(x => x.boleta.fecha >= inicio && x.boleta.fecha <= fin).ToList();
        }

        public static detalle_boleta Obtener(int id)
        {
            return db.detalle_boleta.Where(x => x.id == id).FirstOrDefault();
        }

        public static int GetIngresosPorUsuario(int userID, DateTime desde, DateTime hasta)
        {
            int ventas = 0;
            // List<detail_lines> listaVentasPorUsuario = db.detail_lines.Where(x => x. == userID && x.date >= desde && x.date <= hasta).ToList();
            // foreach (var item in listaVentasPorUsuario)
            // {
            //ventas += item.total;
            // }
            return ventas;
        }

        public static void Set(producto p, boleta b, int cantidad)
        {
            detalle_boleta ld = new detalle_boleta();
            ld.producto_id = p.id;
            ld.cantidad = cantidad;
            ld.boleta_id = b.id;
            ld.monto = p.precio * cantidad;
            db.detalle_boleta.Add(ld);
            db.SaveChanges();
            ActualizarBoletaDespuesDeModificarDetalle(ld);
        }

        public static detalle_boleta Agregar(detalle_boleta dt)
        {
            detalle_boleta dl = new detalle_boleta() { producto_id = dt.producto_id, monto = dt.monto, cantidad = dt.cantidad, descuento = dt.descuento, boleta_id = dt.boleta_id, promocion_id = dt.promocion_id };
            db.detalle_boleta.Add(dt);
            db.SaveChanges();
            return dl;
        }
        public static detalle_boleta ObtenerUltima()
        {
            return db.detalle_boleta.ToList().OrderBy(x => x.id).LastOrDefault();
        }


        public static List<detalle_boleta> ObtenerPorBoletaId(int? boletaID)
        {
            return db.detalle_boleta.AsNoTracking().Include("producto").Include("boleta").Where(x => x.boleta_id == boletaID).ToList();
        }

        public static void Delete(int lineaID)
        {
            detalle_boleta ld = db.detalle_boleta.Include("boleta").Where(x => x.id == lineaID).FirstOrDefault();
            db.detalle_boleta.Remove(ld);
            db.SaveChanges();

            ActualizarBoletaDespuesDeModificarDetalle(ld);
        }

        private static void ActualizarBoletaDespuesDeModificarDetalle(detalle_boleta ld)
        {
            List<detalle_boleta> listaDetalle = new List<detalle_boleta>();
            boleta b = new boleta();
            try
            {
                listaDetalle = db.detalle_boleta.AsNoTracking().Where(x => x.boleta_id == ld.boleta.id).ToList();
                b = db.boletas.Where(x => x.id == ld.boleta.id).FirstOrDefault();
            }
            catch (Exception)
            {
                listaDetalle = db.detalle_boleta.AsNoTracking().Where(x => x.boleta_id == ld.boleta_id).ToList();
                b = db.boletas.Where(x => x.id == ld.boleta_id).FirstOrDefault();
            }
            if (listaDetalle != null)
            {
                int total = 0;
                foreach (detalle_boleta ldTemp in listaDetalle)
                {
                    total += (int)ldTemp.monto;
                }
                b.total = total;
            }
            db.SaveChanges();
        }

        public static void Update(int lineaID, int productoID, int cantidad, int monto)
        {
            detalle_boleta ld = db.detalle_boleta.Where(x => x.id == lineaID).FirstOrDefault();
            ld.producto_id = productoID;
            ld.cantidad = cantidad;
            ld.monto = monto;

            db.SaveChanges();
            ActualizarBoletaDespuesDeModificarDetalle(ld);
        }
    }
}
