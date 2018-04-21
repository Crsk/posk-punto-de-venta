using posk.Controls;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.Partials
{
    public class ProductoRollo
    {
        public List<ItemAgregadoHandroll> ListaItemsAgregadoHandroll { get; set; }
        public envoltura Envoltura { get; set; }
        public producto Producto { get; set; }
    }
}
