using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class UsuarioBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<usuario> GetAllUsers()
        {
            return db.usuarios.ToList();
        }

        public static List<usuario> GetUsers(string coincidencia)
        {
            if (coincidencia.ToUpper().Equals("TODO") || coincidencia.ToUpper().Equals("*"))
                return db.usuarios.AsNoTracking().ToList();
            else
                return db.usuarios.AsNoTracking().Where(x => x.nombre.Contains(coincidencia) || x.nombre_usuario.Contains(coincidencia)).ToList();
        }

        public static List<usuario> ObtenerGarzones()
        {
            return db.usuarios.AsNoTracking().Where(x => x.tipo.ToLower() == "g").ToList();
        }

        public static string ObtenerPass(int usuarioID)
        {
            usuario u = db.usuarios.Where(x => x.id == usuarioID).FirstOrDefault();
            return u.pass;
        }

        public static usuario ObtenerUsuario(int usuarioID)
        {
            return db.usuarios.Where(x => x.id == usuarioID).FirstOrDefault();
        }

        public static void Crear(usuario u)
        {
            u.nombre.ToUpper();
            db.usuarios.Add(u);
            db.SaveChanges();
        }

        public static List<usuario> GetFavs()
        {
            return db.usuarios.Where(x => x.favorito == true).ToList();
        }

        public static void Actualizar(int usuarioNuevoId, usuario uNuevo)
        {
            usuario uViejo = db.usuarios.Where(x => x.id == usuarioNuevoId).FirstOrDefault();
            uViejo.nombre = uNuevo.nombre;
            uViejo.nombre_usuario = uNuevo.nombre_usuario;
            uViejo.pass = uNuevo.pass;
            uViejo.tipo = uNuevo.tipo;
            uViejo.imagen = uNuevo.imagen;
            uViejo.favorito = uNuevo.favorito;
            db.SaveChanges();
        }

        public static usuario Login(string userName, string pass)
        {
            usuario u = new usuario();
            u = db.usuarios.Where(x => x.nombre_usuario == userName && x.pass == pass).FirstOrDefault();
            if (u != null)
                return u;
            else
                return null;
        }

        public static string GetUserName(int? userID)
        {
            usuario u = db.usuarios.Where(x => x.id == userID).FirstOrDefault();
            if (u != null)
                return u.nombre;
            else
                return null;
        }

        public static void Borrar(int id)
        {
            db.usuarios.Remove(db.usuarios.Where(x => x.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
