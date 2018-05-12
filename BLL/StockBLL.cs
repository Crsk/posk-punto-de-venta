using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class StockBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public struct StockDetalle
        {
            public producto Producto { get; set; }
            public decimal? Entrada { get; set; }
            public decimal? Salida { get; set; }
            public decimal? Ajuste { get; set; }
            public bool? SoloCompra { get; set; }
            public bool? SoloVenta { get; set; }

            public decimal Disponible
            {
                get
                {
                    if (Entrada == null) Entrada = 0;
                    if (Salida == null) Salida = 0;
                    if (Ajuste == null) Ajuste = 0;
                    return (decimal)(Entrada - Salida + Ajuste);
                }
            }

        }

        public static void BorrarTodo()
        {
            db.stock_pr.ToList().ForEach(x => db.stock_pr.Remove(x));
            db.SaveChanges();
        }

        public static decimal ObtenerStock(int productoId)
        {
            stock_pr spr = db.stock_pr.AsNoTracking().Where(x => x.producto_id == productoId).FirstOrDefault();
            if (spr != null) return (spr.entrada - spr.salida + spr.ajuste);
            else return 0;
        }

        public static void Reducir(int productoId, decimal cantidad)
        {
            stock_pr spr = db.stock_pr.AsNoTracking().Where(x => x.producto_id == productoId).FirstOrDefault();
            spr.salida += cantidad;
            db.SaveChanges();
        }

        public static void Devolver(int productoId, decimal cantidad)
        {
            stock_pr spr = db.stock_pr.AsNoTracking().Where(x => x.producto_id == productoId).FirstOrDefault();
            spr.salida -= cantidad;
            db.SaveChanges();
        }

        public static void Aumentar(int productoId, decimal cantidad)
        {
            stock_pr spr = db.stock_pr.AsNoTracking().Where(x => x.producto_id == productoId).FirstOrDefault();
            spr.entrada += cantidad;
            db.SaveChanges();
        }

        public static List<stock_pr> ObtenerTodo()
        {
            return db.stock_pr.AsNoTracking().OrderBy(x => x.entrada - x.salida).ToList();
        }

        /// <summary>
        /// Obtiene detalle de stock de productos omitiendo los solo_compra
        /// </summary>
        /// <returns></returns>
        public static List<StockDetalle> ObtenerDetalle()
        {
            List<StockDetalle> listaStockDetalle = new List<StockDetalle>();
            List<stock_pr> listaSpr = db.stock_pr.AsNoTracking().OrderBy(x => x.entrada - x.salida + x.ajuste).ToList();

            foreach (stock_pr spr in listaSpr)
            {
                if (spr.producto.solo_compra == true) continue;
                listaStockDetalle.Add(new StockDetalle() { Producto = spr.producto, Entrada = spr.entrada, Salida = spr.salida, Ajuste = spr.ajuste });
            }

            return listaStockDetalle;
        }


        /// <summary>
        /// Obtiene detalle de stock de productos omitiendo los solo_venta
        /// </summary>
        /// <returns></returns>
        public static List<StockDetalle> ObtenerDetalleOmitiendoItemsSoloVenta()
        {
            List<StockDetalle> listaStockDetalle = new List<StockDetalle>();
            List<stock_pr> listaSpr = db.stock_pr.AsNoTracking().Where(x => 
                x.producto.solo_venta == false && x.producto.solo_compra == false || x.producto.solo_compra == true)
                .OrderBy(x => x.entrada - x.salida + x.ajuste).ToList();

            foreach (stock_pr spr in listaSpr)
            {
                //if (spr.producto.solo_compra == true) continue;
                listaStockDetalle.Add(new StockDetalle() { Producto = spr.producto, Entrada = spr.entrada, Salida = spr.salida, Ajuste = spr.ajuste });
            }

            return listaStockDetalle;
        }

        public static void Eliminar(int id)
        {
            db.stock_pr.Remove(db.stock_pr.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int stockID, decimal stockNuevo)
        {
            stock_pr spr = db.stock_pr.Where(x => x.id == stockID).FirstOrDefault();
            decimal stockAnterior = spr.entrada - spr.salida + spr.ajuste;
            spr.ajuste += stockNuevo - stockAnterior;
            db.SaveChanges();
        }
    }
}
