using posk.Models;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class CategoriaSubcategoriaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int categoriaID, int subcategoriaID)
        {
            db.categoria_subcategoria.Add(new categoria_subcategoria() { categoria_id = categoriaID, subcategoria_id = subcategoriaID });
            db.SaveChanges();
        }

        public static void Eliminar(int scatID)
        {
            db.categoria_subcategoria.Remove(db.categoria_subcategoria.Where(x => x.subcategoria_id == scatID).FirstOrDefault());
            db.SaveChanges();
        }

        public static List<subcategoria> ObtenerSubcategorias(int categoriaID)
        {
            List<subcategoria> listaSubcategorias = new List<subcategoria>();
            db.categoria_subcategoria.Where(cs => cs.categoria_id == categoriaID).ToList().ForEach(cs2 =>
                listaSubcategorias.Add(db.subcategorias.Where(c => c.id == cs2.subcategoria_id).FirstOrDefault()));
            return listaSubcategorias;
        }

        public static categoria ObtenerCategoria(int? subcategoriaID)
        {
            return db.categoria_subcategoria.Where(x => x.subcategoria_id == subcategoriaID).FirstOrDefault().categoria;
        }
    }
}
