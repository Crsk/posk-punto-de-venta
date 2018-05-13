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

        public static void Crear(int boleta_id, DateTime? fechaentrega = null, string direccion = "", string nombreCliente = "", int? cliente_id = null, string comentario = "", string incluye = "", bool bServir = true)
        {
            db.delivery_item.Add(new delivery_item()
            {
                boleta_id = boleta_id,
                fecha_entrega = fechaentrega,
                direccion = direccion,
                nombre_cliente = nombreCliente,
                cliente_id = cliente_id,
                comentario = comentario,
                incluye = incluye,
                servir = bServir
            });
            db.SaveChanges();
        }

        public static List<delivery_item> ObtenerPendientesDeEntrega()
        {
            return db.delivery_item.Include("boleta").Include("cliente").Where(x => x.fecha_entrega == null).ToList();
        }

        public static void Entregar(int id)
        {
            delivery_item di = db.delivery_item.Where(x => x.id == id).FirstOrDefault();
            di.fecha_entrega = DateTime.Now;
            db.SaveChanges();
        }
    }
}
