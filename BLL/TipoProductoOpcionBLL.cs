using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class TipoProductoOpcionBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Ingresar(int tipoProductoId, int opcionId)
        {
            db.tipo_producto_opcionales.Add(new tipo_producto_opcionales() { tipo_producto_id = tipoProductoId, opcional_id = opcionId });
            db.SaveChanges();
        }

        public static void Eliminar(int tipoProductoId, int opcionId)
        {
            db.tipo_producto_opcionales.Remove(db.tipo_producto_opcionales.Where(x => 
            x.tipo_producto_id == tipoProductoId && x.opcional_id == opcionId).FirstOrDefault());
            db.SaveChanges();
        }

        public static List<opcionale> ObtenerOpciones(int tipoProductoId)
        {
            List<opcionale> listaOpciones = new List<opcionale>();

            db.tipo_producto_opcionales.AsNoTracking().Where(nav => /* nav equivale a todas las posibles relacinoes entre tipo producto y opcion */
            nav.tipo_producto_id == tipoProductoId).ToList().ForEach(navFiltrado =>  /* navFiltrado equivale a todas las relacinoes del tipo producto específico */
            listaOpciones.Add(db.opcionales.AsNoTracking().Where(opcion => /* cada una de las opcinoes encontradas se intregan a la listaOpciones */
            opcion.id == navFiltrado.opcional_id).FirstOrDefault())); /* opcion equivale a la opcion relacinada a navFiltrado */

            return listaOpciones; 
        }
    }
}
