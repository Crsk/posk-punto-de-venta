using posk.Models;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class CategoriaSectorBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int categoriaID, int sectorID)
        {
            db.categoria_sector.Add(new categoria_sector() { categoria_id = categoriaID, sector_id = sectorID });
            db.SaveChanges();
        }

        public static void Eliminar(int catID)
        {
            db.categoria_sector.Remove(db.categoria_sector.Where(x => x.categoria_id == catID).FirstOrDefault());
            db.SaveChanges();
        }

        public static List<categoria> ObtenerCategorias(int sectorID)
        {
            List<categoria> listaCategorias = new List<categoria>();
            db.categoria_sector.AsNoTracking().Where(cs => cs.sector_id == sectorID).ToList().ForEach(cs2 =>
                listaCategorias.Add(db.categorias.AsNoTracking().Where(c => c.id == cs2.categoria_id).FirstOrDefault()));
            return listaCategorias;
        }

        public static List<categoria> ObtenerCategorias()
        {
            List<categoria> listaCategorias = new List<categoria>();
            db.categoria_sector.ToList().ForEach(cs2 =>
                listaCategorias.Add(db.categorias.Where(c => c.id == cs2.categoria_id).FirstOrDefault()));
            return listaCategorias;
        }

        public static sectore ObtenerSector(int categoriaID)
        {
            return db.categoria_sector.Where(x => x.categoria_id == categoriaID).FirstOrDefault().sectore;
        }
    }
}
