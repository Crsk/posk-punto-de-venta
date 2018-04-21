using System;
using System.Collections.Generic;
using System.Linq;
using posk.Models;

namespace posk.BLL
{
    static class AgregadoBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<agregado> ObtenerTodos()
        {
            return db.agregados.ToList();
        }

        public static agregado Obtener(int? id)
        {
            return db.agregados.Where(x => x.id == id).FirstOrDefault();
        }

        public static List<agregado> ObtenerPaltaCebollin()
        {
            return db.agregados.Where(x => x.nombre.ToLower().Equals("palta") || x.nombre.ToLower().Equals("cebollin")).ToList();
        }
    }
}
