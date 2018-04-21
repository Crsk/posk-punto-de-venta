using posk.Globals;
using posk.Models;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class MateriaPrimaBLL
    {
        private static PoskDB6 db = new PoskDB6();


        public static materiasprima Crear(string nombre, int unidadMedidaID)
        {
            materiasprima mp = new materiasprima();
            try
            {
                mp = new materiasprima() { nombre = nombre, unidad_medida_id = unidadMedidaID };
                db.materiasprimas.Add(mp);
                db.SaveChanges();
                StockmpBLL.Crear(mp.id, 0, 0, 0);
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, $"Asegúrate de que \"{nombre}\" no existe antes de crearlo");
            }
            return mp;
        }

        public static List<materiasprima> ObtenerTodo()
        {
            return db.materiasprimas.Include("unidades_medida").ToList();
        }

        public static void Eliminar(int mpID)
        {
            db.materiasprimas.Remove(db.materiasprimas.Where(x => x.id == mpID).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
