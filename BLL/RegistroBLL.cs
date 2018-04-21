using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class RegistroBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void CreateLog(DateTime _date, string _type, int _user_id, string _detail)
        {
            var registro = new registro()
            {
                fecha = _date,
                tipo = _type,
                usuario_id = _user_id,
                detalle = _detail
            };
            db.registros.Add(registro);
            db.SaveChanges();
        }

        public static List<LogItem> GetLogItems()
        {
            var listaLogs = db.registros.ToList();
            listaLogs = (from x in listaLogs orderby x.fecha descending select x).ToList();
            if (listaLogs.Count > 5)
            {
                int count = 0;
                foreach (var item in listaLogs)
                {
                    if (count > 4)
                        db.registros.Remove(item);
                    count++;
                }
                db.SaveChanges();
            }

            var listaLogsAchicada = db.registros.ToList();
            var listaLogItems = new List<LogItem>();
            foreach (var item in listaLogsAchicada)
            {
                var li = new LogItem();
                li.id = item.id;
                li.fecha = item.fecha;
                li.usuario = item.usuario;
                li.usuario_id = item.usuario_id;
                li.tipo = item.tipo;
                li.detalle = item.detalle;
                listaLogItems.Add(li);
            }
            db.SaveChanges();
            return listaLogItems;
        }
    }
}
