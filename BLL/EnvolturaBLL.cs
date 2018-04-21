using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class EnvolturaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<envoltura> ObtenerTodas()
        {
            return db.envolturas.ToList();
        }

        public static List<envoltura> ObtenerTodasParaHandroll()
        {
            return db.envolturas.Where(x => x.para_handroll == true).ToList();
        }

        public static List<envoltura> ObtenerTodasParaSuperHandroll()
        {
            return db.envolturas.Where(x => x.para_superhandroll == true).ToList();
        }
    }
}
