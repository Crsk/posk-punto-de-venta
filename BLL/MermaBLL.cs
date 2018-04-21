using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class MermaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void Agregar(int productoId, decimal? cantidad)
        {
            db.mermas.Add(new merma() { producto_id = productoId, cantidad = (decimal)cantidad, fecha = DateTime.Now });
            // reducir stock producto
            CompraBLL.ReduceStockByProduct(productoId, null, (decimal)cantidad);

            // reducir stock_pr
            StockBLL.Reducir(productoId, (decimal)cantidad);

            db.SaveChanges();
        }

        public static int ObtenerCantidadItemsMermados(DateTime? inicio, DateTime? fin)
        {
            decimal cantidad = 0;
            db.mermas.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList().ForEach(mer => cantidad += mer.cantidad);
            return Convert.ToInt32(cantidad);
        }

        public static List<merma> ObtenerPorPeriodo(DateTime? inicio, DateTime? fin)
        {
            return db.mermas.Include("producto").Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
        }

        public static int ObtenerValor(DateTime? inicio, DateTime? fin)
        {
            decimal? totales = 0;
            List<merma> listaMermasPeriodo = db.mermas.Include("producto").Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
            foreach (merma item in listaMermasPeriodo)
            {
                totales += item.producto.precio * item.cantidad;
            }
            return Convert.ToInt32(totales);
        }
    }
}
