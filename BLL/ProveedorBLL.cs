using posk.Models;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class ProveedorBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<proveedore> GetAll()
        {
            return db.proveedores.ToList();
        }

        public static proveedore Obtener(string nombre)
        {
            return db.proveedores.Where(x => x.nombre == nombre).FirstOrDefault();
        }

        public static void Actualizar(int id, string proveedor, string representante, string contacto)
        {
            proveedore p = db.proveedores.Where(x => x.id == id).FirstOrDefault();
            p.nombre = proveedor;
            p.representante = representante;
            p.contacto = contacto;
            db.SaveChanges();
        }

        public static void Eliminar(int id)
        {
            db.proveedores.Remove(db.proveedores.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Crear(string proveedor, string representante, string contacto)
        {
            db.proveedores.Add(new proveedore() { nombre = proveedor, representante = representante, contacto = contacto });
            db.SaveChanges();
        }
    }
}
