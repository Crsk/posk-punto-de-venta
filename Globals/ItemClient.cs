using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace posk.Globals
{
    class ItemClient : Button
    {
        public int Id { get; set; }
        public string Rut { get; set; }
        public string ClientName { get; set; }
        public string Contacto { get; set; }
        public int? PuntosAmmount { get; set; }
        public string Comment { get; set; }
        public bool? Vip { get; set; }
        public int EnvasesQueDebe { get; set; }
        public int CuantoHaComprado { get; set; }
    }
}
