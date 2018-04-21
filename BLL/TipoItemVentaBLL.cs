using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class TipoItemVentaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<tipo_itemventa> ObtenerTodo()
        {
            return db.tipo_itemventa.AsNoTracking().ToList();
        }

        public static tipo_itemventa Obtener(int? id)
        {
            return db.tipo_itemventa.AsNoTracking().Where(x => x.id == id).FirstOrDefault();
        }
    }
}
