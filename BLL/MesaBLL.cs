using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class MesaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<mesa> ObtenerTodasPorSector(int sectorID)
        {
            return db.mesas.AsNoTracking().Where(m => m.sector_id == sectorID).ToList();
        }

        public static void LiberarMesa(int mesaId)
        {
            mesa mesa = db.mesas.Where(x => x.id == mesaId).FirstOrDefault();
            mesa.libre = true;
            db.SaveChanges();
        }

        public static void OcuparMesa(int mesaId)
        {
            mesa mesa = db.mesas.Where(x => x.id == mesaId).FirstOrDefault();
            mesa.libre = false;
            db.SaveChanges();
        }

        public static void Actualizar(int mesaID, int items, int? usuarioID, bool bLibre)
        {
            mesa mesa = db.mesas.Where(x => x.id == mesaID).FirstOrDefault();
            mesa.items = items;
            mesa.usuario_id = usuarioID;
            mesa.libre = bLibre;
            db.SaveChanges();
        }

        public static List<mesa> ObtenerTodas()
        {
            return db.mesas.AsNoTracking().ToList();
        }

        public static mesa Obtener(string codigo)
        {
            return db.mesas.Where(x => x.codigo == codigo).FirstOrDefault();
        }

        public static mesa Obtener(int id)
        {
            return db.mesas.Where(x => x.id == id).FirstOrDefault();
        }
    }
}
