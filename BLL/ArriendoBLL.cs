using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class ArriendoBLL
    {
        private static PoskDB6 db = new PoskDB6();

        //public static void Crear(int productoId, int clienteId, DateTime fechaInicio, DateTime fechaTermino)
        //{
        //    db.arriendos.Add(new arriendo() { producto_id = productoId, cliente_id = clienteId, fecha_inicio = fechaInicio, fecha_termino = fechaTermino });
        //    db.SaveChanges();
        //}

        //public static List<arriendo> ObtenerTodos()
        //{
        //    return db.arriendos.Include("cliente").Include("producto").ToList();
        //}
    }
}
