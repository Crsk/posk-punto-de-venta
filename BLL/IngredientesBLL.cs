using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class IngredientesBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(string nombre, int precio)
        {
            db.ingredientes.Add(new ingrediente() { nombre = nombre, precio = precio });
            db.SaveChanges();
        }

        public static List<ingrediente> ObtenerTodo()
        {
            return db.ingredientes.ToList();
        }

        public static void Eliminar(int id)
        {
            db.ingredientes.Remove(db.ingredientes.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int Id, string nombre, int precio)
        {
            var ingrediente = db.ingredientes.Where(x => x.id == Id).FirstOrDefault();
            ingrediente.nombre = nombre;
            ingrediente.precio = precio;
            db.SaveChanges();
        }
    }
}
