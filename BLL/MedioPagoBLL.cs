using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class MedioPagoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<medio_pago> ObtenerTodos()
        {
            return db.medio_pago.ToList();
        }
    }
}
