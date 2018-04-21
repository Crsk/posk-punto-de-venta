using System;

namespace posk.Entities
{
    class SubCategoryEntity
    {
        public int id { get; set; }
        public string name { get; set; }

        public void LoadEnt(object[] array)
        {
            id = Convert.ToInt32(array[0].ToString());
            name = array[1].ToString();
        }
    }
}
