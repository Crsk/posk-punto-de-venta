using System.Windows.Controls;

namespace posk.Globals
{
    class ItemProduct : Button
    {
        public int IdProductFromThisTicket { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public decimal Stock { get; set; }
        public decimal Cost { get; set; }
        public bool? Retornable { get; set; }
        public bool? Promo { get; set; }
        public bool? MasVendido { get; set; }
        public int UnidadesPromo { get; set; }
        public int Ammount { get; set; }
        public int Points { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SupplierId { get; set; }
        public int Price { get; set; }
        public int promo_id { get; set; }

        // needed to store quantity and show it at sales page
        public int Quantity { get; set; }
        public int Discount { get; set; }

    }
}
