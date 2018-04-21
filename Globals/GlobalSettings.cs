using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.Globals
{
    static class GlobalSettings
    {
        public struct ProductoCantidad
        {
            public producto Producto { get; set; }
            public decimal Cantidad { get; set; }
        }

        public static List<ProductoCantidad> ListaCantidadVendida { get; set; }

        public static bool RadioButtonJornadaAnteriorSeleccionado { get; set; }
        public static bool RadioButtonJornadaActualSeleccionado { get; set; }

        public enum ModoEnum { RESTAURANT, RUA, KUPAL, NORMAL, SUSHI }
        public static string Modo { get; set; }

        public static bool? UsarTecladoTactilIntegrado { get; set; }

        public static void EstablecerModo(Enum modo)
        {
            Modo = modo.ToString().ToUpper();
        }
    }
}
