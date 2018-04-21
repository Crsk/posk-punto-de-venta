using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PedidosProductosBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void Eliminar(int ppId)
        {
            db.pedidos_productos.Remove(db.pedidos_productos.Where(x => x.id == ppId).FirstOrDefault());
            db.SaveChanges();
        }

        public static void EliminarVarios(int pedidoID, List<pedidos_productos> listaPedidosProductos)
        {
            listaPedidosProductos.ForEach(item => 
            {
                pedidos_productos pp = db.pedidos_productos.Where(x => x.id == item.id).FirstOrDefault();
                db.pedidos_productos.Remove(pp);
            });
            db.SaveChanges();
        }

        public static void ActualizarVarios(int pedidoID, List<pedidos_productos> listaPedidosProductos)
        {
            listaPedidosProductos.ForEach(item =>
            {
                pedidos_productos pp = db.pedidos_productos.Where(x => x.id == item.id).FirstOrDefault();
                if (pp != null)
                {
                    pp.cantidad = item.cantidad;
                    pp.impreso_cantidad = item.cantidad;
                }
            });
            db.SaveChanges();
        }

        public static void Actualizar(int pedidoID, int nuevoPedidoId)
        {
            List<pedidos_productos> listapp = db.pedidos_productos.Where(x => x.pedido_id == pedidoID).ToList();
            listapp.ForEach(x =>
            {
                x.id = nuevoPedidoId;
            });
            db.SaveChanges();
        }

        public static pedidos_productos Crear(int pedidoID, int? pId, int? promoId, decimal? cantidad, int precio, string nota)
        {
            pedidos_productos pedido_producto = new pedidos_productos() { pedido_id = pedidoID, producto_id = pId, promo_id = promoId, cantidad = cantidad, impreso = true, impreso_cantidad = cantidad, precio = precio, nota = nota };
            if (pedido_producto != null)
            {
                db.pedidos_productos.Add(pedido_producto);
                db.SaveChanges();
            }
            return pedido_producto;
        }

        public static void AgregarCantidad(int ppID, decimal? nuevaCantidad)
        {
            pedidos_productos pp = db.pedidos_productos.Where(x => x.id == ppID).FirstOrDefault();
            pp.cantidad += nuevaCantidad;
            pp.impreso_cantidad += nuevaCantidad;
            pp.impreso = true;
            db.SaveChanges();
        }

        public static List<pedidos_productos> ObtenerTodos(int pedidoID)
        {
            return db.pedidos_productos.Include("producto").Where(x => x.pedido_id == pedidoID).ToList();
        }

        public static pedidos_productos Obtener(int id)
        {
            return db.pedidos_productos.Include("producto").Where(x => x.id == id).FirstOrDefault();
        }
    }
}
