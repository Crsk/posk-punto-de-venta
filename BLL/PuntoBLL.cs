using posk.Models;
using System;
using System.Linq;

namespace posk.BLL
{
    static class PuntoBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static punto Crear()
        {
            punto p = new punto() { puntos_activos = 100, puntos_expirados = 0, fecha_expiracion = DateTime.Now.AddDays(30) };
            db.puntos.Add(p);
            db.SaveChanges();
            return p;
        }

        public static void Update(punto points, int amount)
        {
            // nunca hacer un update a un objeto encontrado por id con linq
            // por que?? no me acuerdo!!!!

            // points p = db.points.Where(x => x.id == pointsId).FirstOrDefault();

            if (points.fecha_expiracion > DateTime.Now)
                points.puntos_activos += amount;
            else
                points.puntos_activos = amount;
            points.fecha_expiracion = DateTime.Now.AddDays(30);
            db.SaveChanges();
        }

        public static void Sumar(int? puntosId, int cantidad)
        {
            if (puntosId != null)
            {
                punto p = db.puntos.Where(x => x.id == puntosId).FirstOrDefault();
                p.puntos_activos += cantidad;
                p.fecha_expiracion = DateTime.Now.AddDays(30);
                db.SaveChanges();
            }
        }
    }
}
