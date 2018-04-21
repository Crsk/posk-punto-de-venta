using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace posk.BLL
{
    static class PendienteBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<pendiente> GetAll()
        {
            List<pendiente> listaPendientes = db.pendientes.AsNoTracking().Include("usuario").Include("producto").Include("promocione").ToList();
            return listaPendientes;
        }

        public static async void IngresarPendienteProducto(producto producto, usuario usuario, DateTime fecha)
        {
            db.pendientes.Add(new pendiente() { producto_id = producto.id, usuario_id = usuario.id, fecha = fecha, archivado = false });
            await db.SaveChangesAsync();
        }
        public static async void IngresarPendientePromo(promocione promo, usuario usuario, DateTime fecha)
        {
            db.pendientes.Add(new pendiente() { promocion_id = promo.id, usuario_id = usuario.id, fecha = fecha, archivado = false });
            await db.SaveChangesAsync();
        }
        public static void Delete(int id)
        {
            try
            {
                db.pendientes.Remove(db.pendientes.Where(x => x.id == id).FirstOrDefault());
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error, anota el código entre comillas y notifícalo al administrador: \"e1-borrarpendiente\" \n" + ex.ToString());
            }
        }

        public static void Archivar(int id)
        {
            pendiente p = db.pendientes.Where(x => x.id == id).FirstOrDefault();
            p.archivado = true;
            db.SaveChanges();
        }
        public static void Desarchivar(int id)
        {
            pendiente p = db.pendientes.Where(x => x.id == id).FirstOrDefault();
            p.archivado = false;
            db.SaveChanges();
        }
    }
}
