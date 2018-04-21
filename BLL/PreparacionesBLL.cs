using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PreparacionesBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<preparacione> ObtenerTodos()
        {
            return db.preparaciones.ToList();
        }

        public static preparacione Obtener(int? id)
        {
            return db.preparaciones.Where(x => x.id == id).FirstOrDefault();
        }
    }
}
