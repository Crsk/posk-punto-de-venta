using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class OpcionIngredienteBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(int opcionId, int ingredienteId)
        {
            db.opcionales_ingredientes.Add(new opcionales_ingredientes() { opcional_id = opcionId, ingrediente_id = ingredienteId });
            db.SaveChanges();
        }

        public static void Eliminar(int opcionId, int ingredienteId)
        {
            db.opcionales_ingredientes.Remove(db.opcionales_ingredientes.Where(x => 
            x.opcional_id == opcionId && x.ingrediente_id == ingredienteId).FirstOrDefault());
            db.SaveChanges();
        }

        public static List<ingrediente> ObtenerIngredientes(int opcionId)
        {
            List<ingrediente> listaIngredientes = new List<ingrediente>();
            db.opcionales_ingredientes.AsNoTracking().Where(nav =>
            nav.opcional_id == opcionId).ToList().ForEach(navFiltrado =>
            listaIngredientes.Add(db.ingredientes.AsNoTracking().Where(ingrediente => 
            ingrediente.id == navFiltrado.ingrediente_id).FirstOrDefault()));
            return listaIngredientes;
        }
    }
}
