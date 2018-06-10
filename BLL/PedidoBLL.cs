using posk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace posk.BLL
{
    static class PedidoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<pedido> ObtenerTodos()
        {
            return db.pedidos.AsNoTracking().Include("pedidos_productos").Include("usuario").Include("mesa").ToList();
        }

        public static void Eliminar(int pedidoId)
        {
            pedido ped = db.pedidos.Where(x => x.id == pedidoId).FirstOrDefault();
            if (ped != null)
                db.pedidos.Remove(ped);
            db.SaveChanges();
        }
        public static pedido ObtenerPorMesa(string codigo)
        {
            return db.pedidos.AsNoTracking().Include("pedidos_productos").Include("usuario").Include("mesa").Where(x => x.mesa.codigo == codigo).FirstOrDefault();
        }

        public static void EstablecerMesa(int pedidoId, int mesaId)
        {
            /*
            pedido ped = db.pedidos.AsNoTracking().Where(x => x.id == pedidoId).FirstOrDefault();
            if (ped != null)
                ped.mesa_id = mesaId;
            db.SaveChanges();
            */
        }

        public static void ActualizarPedido(int pedidoId, int usuarioId, int mesaId)
        {
            pedido ped = new pedido();
            ped = db.pedidos.Where(x => x.id == pedidoId).FirstOrDefault();
            ped.mesa_id = mesaId;
            ped.usuario_id = usuarioId;
            ped.mensaje = "Actualizado";
            db.SaveChanges();
            
            /*
            pedido nuevoPedido = new pedido();
            if (ped != null)
            {
                nuevoPedido.fecha = ped.fecha;
                nuevoPedido.mensaje = ped.mensaje;
                nuevoPedido.usuario_id = usuarioId;
                nuevoPedido.mesa_id = mesaId;
                asd;
                nuevoPedido.pagado = ped.pagado;
                db.pedidos.Add(nuevoPedido);
                PedidosProductosBLL.Actualizar(ped.id, nuevoPedido.id);
                db.pedidos.Attach(ped);
                db.Entry(ped).State = EntityState.Modified;
                db.pedidos.Remove(ped);
                db.SaveChanges();
            }
            */
        }

        public static void EliminarVariosPorMesa(List<pedido> listaPedidos)
        {
            listaPedidos.ForEach(p => Eliminar(p.id));
            db.SaveChanges();
        }

        public static pedido ObtenerPedido(int pedidoId)
        {
            return db.pedidos.Include("usuario").Where(x => x.id == pedidoId).FirstOrDefault();
        }

        public static pedido Obtener(int mesaID)
        {
            return db.pedidos.Where(x => x.mesa_id == mesaID).FirstOrDefault();
        }

        public static List<pedido> ObtenerPedidosPorMesa(int? mesaId)
        {
            return db.pedidos.AsNoTracking().Where(x => x.mesa_id == mesaId).ToList();
        }

        public static pedido Crear(usuario usuario, DateTime fecha, string mensaje, int mesaID)
        {
            pedido pedido = new pedido() { fecha = fecha, mensaje = mensaje, usuario_id = usuario.id, mesa_id = mesaID };
            db.pedidos.Add(pedido);
            db.SaveChanges();
            return pedido;
        }

        public static List<pedido> ObtenerProductosPedido(int mesaID)
        {
            return db.pedidos.Include("pedidos_productos").Where(x => x.mesa_id == mesaID).ToList();
        }

        public static bool MesaLibre(int mesaID)
        {
            if (db.pedidos.Where(x => x.mesa_id == mesaID && x.pagado == false).ToList().Count > 0)
                return false;
            else
                return true;
        }
    }
}
