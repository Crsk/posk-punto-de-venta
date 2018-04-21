using posk.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace posk.BLL
{
    static class StockMovimientoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int productoId, DateTime fecha, int jornadaId, decimal entrada, decimal salida)
        {
            db.bodega_movimiento.Add(new bodega_movimiento()
            {
                producto_id = productoId,
                fecha = fecha,
                jornada_id = jornadaId,
                entrada = entrada,
                salida = salida
            });
            db.SaveChanges();
        }
    }
}
