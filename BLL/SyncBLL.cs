using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class SyncBLL
    {
        public static PoskDB6 db = new PoskDB6();

        public static int ObtenerUltimoSyncId(string nombre)
        {
            //return 0;
            return db.syncs.AsNoTracking().Where(x => x.nombre == nombre).FirstOrDefault().sync_id;
        }

        public static void AumentarSyncId(string nombre)
        {
            sync sync = db.syncs.Where(x => x.nombre == nombre).FirstOrDefault();
            if (sync != null)
                sync.sync_id += 1;
            db.SaveChanges();
        }
    }
}
