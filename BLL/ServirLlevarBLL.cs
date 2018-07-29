using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class ServirLlevarBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<servir_llevar> ObtenerTodo()
        {
            return db.servir_llevar.ToList();
        }
    }
}
