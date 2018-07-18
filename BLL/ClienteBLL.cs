using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class ClienteBLL
    {
        static PoskDB6 db = new PoskDB6();
        // Client methods
        public static List<cliente> GetClients(string filtro)
        {
            if (filtro.ToUpper() == "TODO" || filtro == "*")
                return db.clientes.Include("punto").ToList();
            else
                return db.clientes.Include("punto").Where(x => x.nombre.Contains(filtro)).ToList();
        }

        public static List<cliente> ObtenerTodo()
        {
            return db.clientes.Include("punto").AsNoTracking().ToList();
        }

        public static cliente ObtenerPorTelefono(string telefono)
        {
            return db.clientes.Where(x => x.telefono == telefono).FirstOrDefault();
        }

        public static void Actualizar(cliente clienteNuevo)
        {
            cliente clienteViejo = db.clientes.Where(x => x.id == clienteNuevo.id).FirstOrDefault();
            clienteViejo.nombre = clienteNuevo.nombre;
            // todo implementar
        }

        public static List<cliente> ObtenerFavoritos()
        {
            return db.clientes.Where(x => x.favorito == true).ToList();
        }

        public static void Borrar(int? id)
        {
            if (id == null) return;
            db.clientes.Remove(db.clientes.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public static cliente GetClient(string rut)
        {
            return db.clientes.Where(x => x.rut == rut).FirstOrDefault();
        }

        public static cliente GetClientById(int? id)
        {
            return db.clientes.Where(x => x.id == id).FirstOrDefault();
        }

        public static void AddClient(cliente c)
        {
            db.clientes.Add(c);
            db.SaveChanges();
        }

        public static List<cliente> GetFavs()
        {
            return db.clientes.Where(x => x.favorito == true).ToList();
        }

        public static void VipToggle(cliente c)
        {
            c.favorito ^= true;
            db.SaveChanges();
        }

        public static void UpdateClient(string rut, cliente newClient)
        {
            cliente c = db.clientes.Where(x => x.rut == rut).FirstOrDefault();
            c = newClient;
            db.SaveChanges();
        }
        // Client methods

    }
}
