using posk.Models;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class SectorMesaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<sectormesa> ObtenerTodo()
        {
            return db.sectormesas.ToList();
        }
    }
}
