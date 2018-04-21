using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PedidosAgregadosBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int pedidoProductoId, int? agregadoUnoId, int? agregadoDosId)
        {
            if (agregadoUnoId == 0) agregadoUnoId = null;
            if (agregadoDosId == 0) agregadoDosId = null;
            db.pedidos_agregados.Add(new pedidos_agregados() { pedidos_productos_id = pedidoProductoId, agregado_uno_id = agregadoUnoId, agregado_dos_id = agregadoDosId });
            db.SaveChanges();
        }

        //public static List<agregado> ObtenerAgregados(int pedidoProductoID)
        //{
        //    List<pedidos_agregados> listaPA = db.pedidos_agregados.Where(x => x.pedidos_productos_id == pedidoProductoID).ToList();
        //    List<agregado> listaAgregados = new List<agregado>();
        //    foreach (pedidos_agregados pa in listaPA)
        //    {
        //        agregado a = db.agregados.Where(x => x.id == pa.agregado_id).FirstOrDefault();
        //        listaAgregados.Add(a);
        //    }
        //    return listaAgregados;
        //}

        public static pedidos_agregados Obtener(int pedidoProductoID)
        {
            return db.pedidos_agregados.Where(x => x.pedidos_productos_id == pedidoProductoID).FirstOrDefault();
        }

        public static List<pedidos_agregados> ObtenerTodos(int pedidoID)
        {
            return db.pedidos_agregados.Where(x => x.pedidos_productos_id == pedidoID).ToList();
        }
    }
}
