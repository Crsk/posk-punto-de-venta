using System;

namespace posk.Entities
{
    class SupplierEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string image { get; set; }

        public void LoadEnt(object[] array)
        {
            id = Convert.ToInt32(array[0].ToString());
            name = array[1].ToString();
            email = array[2].ToString();
            phoneNumber = array[3].ToString();
            image = array[4].ToString();
        }
    }
}
