using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class DeliveryItemBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int boleta_id, DateTime? fechaEntrega = null, DateTime fechaPedido = new DateTime(), string direccion = "", string nombreCliente = "", int? cliente_id = null, string comentario = "", string incluye = "", bool bServir = true, int pagaCon = 0, string vuelto = "", int medioPagoId = 1)
        {

            db.delivery_item.Add(new delivery_item()
            {
                boleta_id = boleta_id,
                fecha_entrega = fechaEntrega,
                fecha_pedido = fechaPedido,
                direccion = direccion,
                nombre_cliente = nombreCliente,
                cliente_id = cliente_id,
                comentario = comentario,
                incluye = incluye,
                servir = bServir,
                paga_con = pagaCon,
                vuelto = vuelto == null ? "" : vuelto,
                medio_pago_id = medioPagoId
            });
            db.SaveChanges();
        }

        public static List<delivery_item> ObtenerPendientesDeEntrega()
        {
            return db.delivery_item.Include("boleta").Include("cliente").Include("medio_pago").Where(x => x.fecha_entrega == null).OrderBy(x=> x.boleta.numero_boleta).ToList();
        }

        public static void Entregar(int id)
        {
            delivery_item di = db.delivery_item.Where(x => x.id == id).FirstOrDefault();
            if (di != null)
            {
                di.fecha_entrega = DateTime.Now;
            }
            db.SaveChanges();
        }

        public static void CambiarMedioDePago(int diId, int medioPagoId)
        {
            delivery_item di = db.delivery_item.Where(x => x.id == diId).FirstOrDefault();
            di.medio_pago_id = medioPagoId;
            db.SaveChanges();
        }
    }
}
