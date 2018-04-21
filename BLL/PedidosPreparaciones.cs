using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PedidosPreparaciones
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int ppId, int preparacionId)
        {
            db.pedidos_preparaciones.Add(new pedidos_preparaciones() { pedidos_productos_id = ppId, preparacion_id = preparacionId });
            db.SaveChanges();
        }
    }
}
