using posk.Models;
using posk.Partials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class CompraProductoBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<producto_compra> Obtener(int compraID)
        {
            return db.producto_compra.Include("producto").AsNoTracking().Where(x => x.compra_id == compraID).ToList();
        }

        public static void BorrarTodo()
        {
            db.producto_compra.ToList().ForEach(x => db.producto_compra.Remove(x));
            db.SaveChanges();
        }

        public static void Actualizar(int compraProductoId, decimal nuevoPrecioCompraBruto)
        {
            producto_compra pc = db.producto_compra.Where(x => x.id == compraProductoId).FirstOrDefault();
            pc.costo_unitario = nuevoPrecioCompraBruto;
            db.SaveChanges();
        }

        public static producto_compra Create(compra purchase, int productId, decimal unitaryCost, decimal quantity)
        {
            try
            {
                stock_pr skpr = db.stock_pr.Where(x => x.producto_id == productId).FirstOrDefault();
                if (skpr == null)
                {
                    db.stock_pr.Add(new stock_pr() { producto_id = productId, entrada = quantity });
                }
                else
                {
                    skpr.entrada += quantity;
                }
                db.SaveChanges();
            }
            catch
            {
            }

            producto_compra pp = new producto_compra() { compra_id = purchase.id, producto_id = productId, costo_unitario = unitaryCost, cantidad_disponible = quantity, cantidad_compra = quantity, activa = true };
            db.producto_compra.Add(pp);
            db.SaveChanges();
            return pp;
        }

        public static List<producto_compra> ObtainByProductId(int productId)
        {
            List<producto_compra> list = db.producto_compra.Where(x => x.producto_id == productId).ToList();
            return list;
        }

        public static List<ProductPurchaseDetails> ObtainPurchaseDetails(int productId)
        {
            List<producto_compra> list = db.producto_compra.Where(x => x.producto_id == productId).ToList();
            List<ProductPurchaseDetails> list2 = new List<ProductPurchaseDetails>();
            foreach (var item in list)
            {
                compra purchase = db.compras.Where(x => x.id == item.compra_id).FirstOrDefault();
                ProductPurchaseDetails ppd = new ProductPurchaseDetails()
                {
                    id = item.id,
                    date = purchase.fecha,
                    instigator = purchase.usuario.nombre,
                    unitary_cost = item.costo_unitario,
                    quantity = (decimal) item.cantidad_compra
                };
                list2.Add(ppd);
            }
            return list2;
        }


        public static void Delete(int productPurchaseId)
        {
            producto_compra pp = db.producto_compra.Where(x => x.id == productPurchaseId).FirstOrDefault();
            db.producto_compra.Remove(pp);
            db.SaveChanges();
        }
    }
}
