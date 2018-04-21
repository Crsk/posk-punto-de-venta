using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class GastoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(string detalle, decimal monto, DateTime fecha)
        {
            db.gastos.Add(new gasto() { detalle = detalle, monto = monto, fecha = fecha });
            db.SaveChanges();
        }

        public static List<gasto> ObtenerGastosPeriodo(DateTime? inicio, DateTime? termino)
        {
            return db.gastos.Where(x => inicio <= x.fecha && termino >= x.fecha).ToList();
        }

        public static int ObtenerValor(DateTime? inicio, DateTime? fin)
        {
            decimal? totales = 0;
            List<gasto> listaGastosPeriodo = db.gastos.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList();
            foreach (gasto item in listaGastosPeriodo)
                totales += item.monto;
            return Convert.ToInt32(totales);
        }

        public static int ObtenerCantidadItemsGasto(DateTime? inicio, DateTime? fin)
        {
            return db.gastos.Where(x => x.fecha >= inicio && x.fecha <= fin).ToList().Count;
        }
    }
}

