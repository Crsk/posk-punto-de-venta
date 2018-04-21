using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class SalsaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<salsa> ObtenerTodas()
        {
            return db.salsas.ToList();
        }
    }
}
