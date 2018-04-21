using System;

namespace posk.Entities
{
    class ProductEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int barCode { get; set; }
        public string fastCode { get; set; }
        public double buyPrice { get; set; }
        public int cellPrice { get; set; }
        public int points { get; set; }
        public string image { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int supplierId { get; set; }

        public void LoadEnt(object[] array)
        {
            id = Convert.ToInt32(array[0].ToString());
            name = array[1].ToString();
            barCode = Convert.ToInt32(array[2].ToString());
            fastCode = array[3].ToString();
            buyPrice = Convert.ToDouble(array[4].ToString());
            cellPrice = Convert.ToInt32(array[5].ToString());
            points = Convert.ToInt32(array[6].ToString());
            image = array[7].ToString();
            categoryId = Convert.ToInt32(array[8].ToString());
            subCategoryId = Convert.ToInt32(array[9].ToString());
            supplierId = Convert.ToInt32(array[10].ToString());
        }
    }

}