using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class FiadoBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static fiado CreateFiado(DateTime date, int total, cliente client, int userID)
        {
            fiado f = new fiado()
            {
                fecha = date, 
                total = total,
                usuario_id = userID
            };
            if (client != null)
                f.cliente_id = client.id;
            else
                f.cliente_id = null;

            db.fiados.Add(f);
            db.SaveChanges();

            return f;
        }

        public static void AddFiadoLine(lineas_fiado fl)
        {
            db.lineas_fiado.Add(fl);
            db.SaveChanges();
        }
    }
}
