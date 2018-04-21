using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.Globals
{
    static class RegistroActividad
    {
        public static List<RegistroActividadItem> ListaActividades { get; set; }
    }

    class RegistroActividadItem
    {
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public string Usuario { get; set; }
        public string Type { get; set; }
        public string Detalle { get; set; }
    }
}
