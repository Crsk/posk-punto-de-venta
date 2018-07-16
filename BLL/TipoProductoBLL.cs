using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class TipoProductoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(string nombre, int limiteIngr)
        {
            db.tipo_producto.Add(new tipo_producto() { nombre = nombre, limite_ingr = limiteIngr });
            db.SaveChanges();
        }

        public static List<tipo_producto> ObtenerTodo()
        {
            return db.tipo_producto.ToList();
        }

        public static void Eliminar(int id)
        {
            db.tipo_producto.Remove(db.tipo_producto.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int Id, string nombre, int limiteIngr)
        {
            var tipoProducto = db.tipo_producto.Where(x => x.id == Id).FirstOrDefault();
            tipoProducto.nombre = nombre;
            tipoProducto.limite_ingr = limiteIngr;
            db.SaveChanges();
        }

        public static void MostrarOpciones(int tipoProductoId, bool b)
        {
            var tipoProducto = db.tipo_producto.Where(x => x.id == tipoProductoId).FirstOrDefault();
            tipoProducto.mostrar_opciones = b;
            db.SaveChanges();
        }
    }
}
