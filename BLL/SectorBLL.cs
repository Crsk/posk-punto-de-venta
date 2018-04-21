using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class SectorBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(string nombre)
        {
            db.sectores.Add(new sectore() { nombre = nombre });
            db.SaveChanges();
        }

        public static List<sectore> ObtenerTodo()
        {
            return db.sectores.AsNoTracking().ToList();
        }

        public static void EstablecerSeleccionable(int sectorID, bool b)
        {
            sectore sec = db.sectores.Where(x => x.id == sectorID).FirstOrDefault();
            sec.seleccionable = b;
            db.SaveChanges();
        }

        public static void Eliminar(int id)
        {
            db.sectores.Remove(db.sectores.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int id, string nuevoNombre)
        {
            sectore sec = db.sectores.Where(x => x.id == id).FirstOrDefault();
            sec.nombre = nuevoNombre;
            db.SaveChanges();
        }
    }
}
