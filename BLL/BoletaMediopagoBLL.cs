using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class BoletaMediopagoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void Crear(int boleta_id, int mediopago_id, int ingreso, int vendedor_id)
        {
            db.boleta_mediopago.Add(new boleta_mediopago()
            {
                boleta_id = boleta_id,
                mediopago_id = mediopago_id,
                ingreso = ingreso,
                vendedor_id = vendedor_id
            });

            db.SaveChanges();
        }

        public static boleta_mediopago ObtenerPorBoleta(int boletaId)
        {
            return db.boleta_mediopago.Where(x => x.boleta_id == boletaId).FirstOrDefault();
        }

        public static int ObtenerMonto(DateTime? inicio, DateTime? fin, int mediopago_id)
        {
            int? total = 0;
            if (mediopago_id == 4)
                db.boleta_mediopago.Include("boleta").Where(x => x.boleta.fecha >= inicio && x.boleta.fecha <= fin && (x.mediopago_id == mediopago_id || x.mediopago_id == null)).ToList().ForEach(bmp => total += bmp.ingreso);
            else
                db.boleta_mediopago.Include("boleta").Where(x => x.boleta.fecha >= inicio && x.boleta.fecha <= fin && x.mediopago_id == mediopago_id).ToList().ForEach(bmp => total += bmp.ingreso);
            return Convert.ToInt32(total);
        }

        public static int ObtenerPropinas(DateTime? inicio, DateTime? fin)
        {
            int total = 0;
            db.boleta_mediopago.Include("boleta").Where(x => x.boleta.fecha >= inicio && x.boleta.fecha <= fin).ToList().ForEach(bmp => total += bmp.boleta.propina);
            return total;
        }

        public static List<boleta_mediopago> ObtenerTodo()
        {
            return db.boleta_mediopago.ToList();
        }

        public static void ActualizarMedioDePago(int boletaId, int medioPagoId)
        {
            boleta_mediopago bmp = ObtenerPorBoleta(boletaId);
            bmp.mediopago_id = medioPagoId;
            db.SaveChanges();
        }
    }
}
