using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class UnidadMedidaVentaBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<unidades_medida> ObtenerTodo()
        {
            return db.unidades_medida.ToList();
        }

        public static List<string> Obtener(int mpID)
        {
            List<string> listaUnidadesMedida = new List<string>();
            materiasprima mp = db.materiasprimas.Include("unidades_medida").Where(x => x.id == mpID).FirstOrDefault();
            unidades_medida um = mp.unidades_medida;
            switch (um.nombre.ToUpper())
            {
                case "UN":
                    return new List<string>() { "UN" };
                case "GR":
                    return new List<string>() { "GR", "KG" };
                case "ML":
                    return new List<string>() { "ML", "LT" };
                default:
                    return new List<string>() { "UN" };
            }
        }
    }
}
