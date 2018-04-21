using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class SectorImpresionBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<sector_impresion> ObtenerSectoresImpresion()
        {
            return db.sector_impresion.ToList();
        }

        public static sector_impresion Obtener(int? id)
        {
            return db.sector_impresion.AsNoTracking().Where(x => x.id == id).FirstOrDefault();
        }

        public static List<sector_impresion> ObtenerParaComboBox()
        {
            List<sector_impresion> lista = new List<sector_impresion>();
            lista.Add(new sector_impresion() { nombre = "TODOS" });
            foreach (var item in db.sector_impresion.ToList())
            {
                lista.Add(item);
            }
            return lista;
        }

        public static List<sector_impresion> ObtenerTodo()
        {
            return db.sector_impresion.AsNoTracking().ToList();
        }

        public static string ObtenerImpresoraCocina()
        {
            return db.sector_impresion.Where(x => x.nombre.ToUpper() == "COCINA").FirstOrDefault().impresora;
        }

        public static string ObtenerImpresoraBar()
        {
            return db.sector_impresion.Where(x => x.nombre.ToUpper() == "BAR").FirstOrDefault().impresora;
        }
    }
}
