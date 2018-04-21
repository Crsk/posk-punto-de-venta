using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class ProductoMateriaPrimaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(producto_materiaprima pmp)
        {
            db.producto_materiaprima.Add(pmp);
            db.SaveChanges();
        }

        public static void Eliminar(int pmpId)
        {
            db.producto_materiaprima.Remove(db.producto_materiaprima.Where(x => x.id == pmpId).FirstOrDefault());
            db.SaveChanges();
        }

        public static producto_materiaprima Obtener(int pmpID)
        {
            return db.producto_materiaprima.Include("materiasprima").Include("producto").Where(x => x.id == pmpID).FirstOrDefault();
        }

        /*
        public static List<producto_materiaprima> ObtenerPMP(int productoID)
        {
            //List<materiasprima> listaMateriaprima = new List<materiasprima>();
            //listaProductoMateriaprima.ForEach(pmp =>
            //{
            //    listaMateriaprima.Add(pmp.materiasprima);
            //});

            List<producto_materiaprima> listaProductoMateriaprima = new List<producto_materiaprima>();
            listaProductoMateriaprima = db.producto_materiaprima.Include("materiasprima").Include("producto").Where(x => x.producto_id == productoID).ToList();
            return listaProductoMateriaprima;
        }
        */

        public static List<producto_materiaprima> ObtenerTodo(int productoID)
        {
            return db.producto_materiaprima.Where(x => x.producto_id == productoID).ToList();
        }
    }
}
