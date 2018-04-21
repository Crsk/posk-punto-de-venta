using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PedidosPreparacionesBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static pedidos_preparaciones Obtener(int? ppId)
        {
            return db.pedidos_preparaciones.Where(x => x.pedidos_productos_id == ppId).FirstOrDefault();
        }
    }
}
