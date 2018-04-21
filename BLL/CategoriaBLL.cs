using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace posk.BLL
{
    static class CategoriaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<categoria> ObtenerTodo()
        {
            return db.categorias.AsNoTracking().ToList();
        }

        public static void EstablecerSeleccionable(int categoriaID, bool? b)
        {
            categoria cat = db.categorias.Where(x => x.id == categoriaID).FirstOrDefault();
            cat.seleccionable = b;
            db.SaveChanges();
        }

        public static List<categoria> ObtenerParaComboBox()
        {
            List<categoria> lista = new List<categoria>();
            lista.Add(new categoria() { nombre = "TODO" });
            ObtenerTodo().ForEach(x => lista.Add(x));
            return lista;
        }

        public static categoria Obtener(string nombre)
        {
            return db.categorias.Where(x => x.nombre == nombre).FirstOrDefault();
        }

        public static void Eliminar(int id)
        {
            try
            {
                db.categorias.Remove(db.categorias.Where(x => x.id == id).FirstOrDefault());
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void Actualizar(int id, string nombre)
        {
            categoria c = db.categorias.Where(x => x.id == id).FirstOrDefault();
            c.nombre = nombre;
            db.SaveChanges();
        }

        public static categoria Crear(string nombre)
        {
            categoria cat = new categoria() { nombre = nombre, seleccionable = false };
            try
            {
                db.categorias.Add(cat);
                db.SaveChanges();
            }
            catch (DuplicateWaitObjectException ex)
            {
                MessageBox.Show("Ya existe" + ex);
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return cat;
        }
    }
}
