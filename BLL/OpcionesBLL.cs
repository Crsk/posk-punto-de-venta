using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class OpcionesBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(string nombre, int precio)
        {
            db.opcionales.Add(new opcionale() { nombre = nombre, precio = precio });
            db.SaveChanges();
        }

        public static List<opcionale> ObtenerTodo()
        {
            return db.opcionales.ToList();
        }

        public static void Eliminar(int id)
        {
            db.opcionales.Remove(db.opcionales.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int Id, string nombre, int precio)
        {
            var opcional = db.opcionales.Where(x => x.id == Id).FirstOrDefault();
            opcional.nombre = nombre;
            opcional.precio = precio;
            db.SaveChanges();
        }
    }
}
