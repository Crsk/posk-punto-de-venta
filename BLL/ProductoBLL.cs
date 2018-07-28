using posk.Models;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class ProductoBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<producto> ObtenerTodo()
        {
            return db.productos.Include("subcategoria").AsNoTracking().OrderBy(x => x.z_index).ToList();
        }

        public static bool NombreExiste(string nombre)
        {
            producto p = db.productos.Where(x => x.nombre.Equals(nombre)).FirstOrDefault();
            if (p != null)
                return true;
            else
                return false;
        }

        public static void Crear(producto p)
        {
            try
            {
                p.nombre.ToUpper();
                db.productos.Add(p);
                db.SaveChanges();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627) // UNIQUE Exception
                {
                    new Notification("ITEM VENTA YA EXISTE", "Intenta con otro nombre", Notification.Type.Danger);
                }
            }
        }

        public static producto ObtenerPorID(int? id)
        {
            return db.productos.AsNoTracking().Where(x => x.id == id).FirstOrDefault();
        }

        public static producto ObtenerPorCodigo(string codigo)
        {
            return db.productos.Where(x => x.codigo_barras == codigo).FirstOrDefault();
        }

        //public static List<producto> ObtenerNoPromos()
        //{
        //    return db.productos.AsNoTracking().Where(x => x.promocion_id == null || x.promocion_id == 0).ToList();
        //}

        //public static List<producto> ObtenerPromos()
        //{
        //    return db.productos.AsNoTracking().Where(x => x.promocion_id != null || x.promocion_id != 0).ToList();
        //}

        public static (List<producto> listaProductos, List<promocione> listaPromos, bool bSubCategoria) ObtenerCoincidencias(string str)
        {
            if (str.ToUpper().Equals("TODO") || str.Equals("*"))
                return (db.productos.ToList(), db.promociones.ToList(), false);
            else
            {
                subcategoria sc = db.subcategorias.Where(x => x.nombre == str).FirstOrDefault();
                if (sc != null)
                {
                    return (db.productos.Where(x => x.sub_categoria_id == sc.id).OrderBy(x => x.z_index).ToList(),
                        db.promociones.Where(x => x.subcategoria_id == sc.id).ToList(), true);
                }
                else
                {
                    return (db.productos.Where(x => x.nombre.Contains(str)).OrderBy(x => x.z_index).ToList(),
                        db.promociones.Where(x => x.nombre.Contains(str)).ToList(), false);
                }
            }
        }

        public static (List<producto> listaProductos, List<promocione> listaPromos, bool bSubCategoria) ObtenerPromociones()
        {
            return (db.productos.AsNoTracking().ToList(), db.promociones.AsNoTracking().ToList(), true);
        }

        public static producto GetProduct(int? id)
        {
            return db.productos.AsNoTracking().Where(x => x.id == id).FirstOrDefault();
        }

        public static void Create(producto p)
        {
            db.productos.Add(p);
            db.SaveChanges();
        }

        public static (List<producto>, List<promocione>, bool) GetFavs()
        {
            return (db.productos.AsNoTracking().Where(x => x.favorito == true).OrderBy(x => x.z_index).ToList(), db.promociones.AsNoTracking().Where(x => x.favorito == true).ToList(), false);
        }

        public static (List<producto>, List<promocione>, bool) GetPromos()
        {
            return (null, db.promociones.AsNoTracking().ToList(), false);
        }

        public static producto GetProductById(int? id)
        {
            return db.productos.AsNoTracking().Where(x => x.id == id).FirstOrDefault();
        }

        public static void Insert(producto p)
        {
            producto newProduct = new producto()
            {
                nombre = p.nombre,
                codigo_barras = p.codigo_barras,
                precio = p.precio,
                puntos_cantidad = p.puntos_cantidad,
                imagen = p.imagen
            };

            db.productos.Add(newProduct);
            db.SaveChanges();
        }

        public static void Actualizar(producto pNuevo)
        {
            producto pViejo = db.productos.Where(x => x.id == pNuevo.id).FirstOrDefault();
            pViejo.nombre = pNuevo.nombre;
            pViejo.codigo_barras = pNuevo.codigo_barras;
            pViejo.precio = pNuevo.precio;
            pViejo.retornable = pNuevo.retornable;
            pViejo.favorito = pNuevo.favorito;
            pViejo.puntos_cantidad = pNuevo.puntos_cantidad;
            pViejo.imagen = pNuevo.imagen;
            pViejo.sub_categoria_id = pNuevo.sub_categoria_id;
            pViejo.solo_venta = pNuevo.solo_venta;
            pViejo.solo_compra = pNuevo.solo_compra;
            pViejo.sector_impresion_id = pNuevo.sector_impresion_id;
            pViejo.z_index = pNuevo.z_index;
            db.SaveChanges();
        }

        public static void Borrar(int? id)
        {
            db.productos.Remove(db.productos.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
