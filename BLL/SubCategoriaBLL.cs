using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace posk.BLL
{
    static class SubCategoriaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<subcategoria> ObtenerTodo()
        {
            return db.subcategorias.AsNoTracking().ToList();
        }

        public static Func<string, List<subcategoria>> ObtenerTodo_ = x =>
            db.subcategorias.Where(sc => sc.nombre.Equals(x)).ToList();

        public static subcategoria Crear(string nombre)
        {
            subcategoria subcat = new subcategoria() { nombre = nombre };
            db.subcategorias.Add(subcat);
            db.SaveChanges();
            return subcat;
        }

        public static Func<string, subcategoria> Crear_ = x =>
        {
            subcategoria subcat = new subcategoria() { nombre = x };
            db.subcategorias.Add(subcat);
            db.SaveChanges();
            return subcat;
        };

        public static void Actualizar(int id, string nuevoNombre)
        {
            subcategoria sc = db.subcategorias.Where(x => x.id == id).FirstOrDefault();
            sc.nombre = nuevoNombre;
            db.SaveChanges();
        }

        public static void Eliminar(int id)
        {
            try
            {
                db.subcategorias.Remove(db.subcategorias.Where(x => x.id == id).FirstOrDefault());
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //public static List<subcategoria> ObtenerPorCategoria(int categoriaID)
        //{
        //    return db.subcategorias.Where(x => x.categoria_id == categoriaID).ToList();
        //}

        public static subcategoria Obtener(string nombre)
        {
            return db.subcategorias.Where(x => x.nombre == nombre).FirstOrDefault();
        }

        public static List<producto> ObtenerProductos(int subCategoriaID)
        {
            return db.productos.Where(p => p.sub_categoria_id == subCategoriaID).ToList();
        }
    }
}
