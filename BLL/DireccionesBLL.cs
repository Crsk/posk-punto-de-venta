using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    class DireccionesBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<direccione> ObtenerPorCliente(int clienteId)
        {
            return db.direcciones.Where(x => x.cliente_id == clienteId).ToList();
        }

        public static void AgregarDireccionCliente(int clienteId, string direccion)
        {
            db.direcciones.Add(new direccione() { cliente_id = clienteId, nombre = direccion });
            db.SaveChanges();
        }
    }
}
