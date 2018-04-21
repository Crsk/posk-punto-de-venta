using posk.Models;
using System;

namespace posk.BLL
{
    static class PuntoBLL
    {
        static PoskDB6 db = new PoskDB6();

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
    }
}
