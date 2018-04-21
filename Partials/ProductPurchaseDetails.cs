using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.Partials
{
    class ProductPurchaseDetails
    {
        public int id { get; set; }
        public DateTime? date { get; set; }
        public string instigator { get; set; }
        public decimal? unitary_cost { get; set; }
        public decimal quantity { get; set; }
    }
}
