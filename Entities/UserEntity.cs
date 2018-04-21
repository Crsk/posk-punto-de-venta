using System;

namespace posk.Globals
{
    class UserEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        //public string pass { get; set; }
        public bool isAdmin { get; set; }
        public int configId { get; set; }

        public void LoadUserEnt(object[] array)
        {
            id = Convert.ToInt32(array[0].ToString());
            name = array[1].ToString();
            //pass = array[2].ToString();
            isAdmin = Convert.ToBoolean(array[3]);
            configId = Convert.ToInt32(array[4]);
        }
    }
}
