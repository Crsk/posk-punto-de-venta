using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.Partials
{
    public class DeliveryInfo
    {
        public int Efectivo { get; set; }
        public int TransBank { get; set; }
        public int Junaeb { get; set; }
        public int Otro { get; set; }
        public int Propina { get; set; }
        public string NombreCliente { get; set; }
        public string ServirLlevar { get; set; }
        public string MensajeTicket { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string MensajeDeliveryUno { get; set; }
        public string MensajeDeliveryDos { get; set; }
        public string Incluye { get; set; }
        public string IncluyeStrUnaLinea { get; set; }
        public int PagaCon { get; set; }
        public string Vuelto { get; set; }

    }
}
