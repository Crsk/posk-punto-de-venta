using posk.Models;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class PromocionBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(string nombre, int precio, int subcategoriaID, bool fav)
        {
            db.promociones.Add(new promocione() { nombre = nombre, precio = precio, subcategoria_id = subcategoriaID, favorito = fav });
            db.SaveChanges();
        }

        public static promocione ObtenerPromocion(int promoID)
        {
            return db.promociones.Where(x => x.id == promoID).FirstOrDefault();
        }

        public static List<promocione> ObtenerTodas()
        {
            return db.promociones.AsNoTracking().ToList();
        }

        public static void Eliminar(int id)
        {
            db.promociones.Remove(db.promociones.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int id, string nuevoNombre, int nuevoPrecio, int nuevaSubCategoriaID, bool? nuevoEstadoFav)
        {
            promocione promo = db.promociones.Where(x => x.id == id).FirstOrDefault();
            promo.nombre = nuevoNombre;
            promo.precio = nuevoPrecio;
            promo.subcategoria_id = nuevaSubCategoriaID;
            promo.favorito = nuevoEstadoFav == true ? true : false;
            db.SaveChanges();
        }
    }
}
