using System;

namespace posk.Globals
{
    class Validations
    {
        // elimina los caracteres que no sean numericos de un texto y lo convierte a entero
        public static int ToInt(string input)
        {
            try
            {

                string numeroStr = "";
                int numero = 0;
                foreach (char item in input)
                {
                    if (item > 47 && item < 58)
                        numeroStr += item;
                }
                if (numeroStr != "")
                    numero = Convert.ToInt32(numeroStr);
                return numero;
            }
            catch
            {
                return 0;
            }
        }

        // averigua si todos los caracteres de un texto son numéricos
        public static bool IsInt(string texto)
        {
            foreach (char item in texto)
            {
                if (item > 47 && item < 58)
                    return true;
            }
            return false;
        }

        //public static char JustText(string input)
        //{
        //    char validChar = char.E;
        //    foreach (char item in input)
        //    {
        //        if (item >= 65 && item <= 90 || item >= 97 && item <= 122 || item == 164 || item == 165)
        //            validChar += item;
        //    }
        //    if (validChar != "")
        //        return validChar;
        //    else
        //        return "";
        //}

        //public static int JustInt(string input)
        //{

        //}

        //// allows int and flotating point numbers
        //public static float JustNumbers(string input)
        //{

        //}

        //// allows numbers, dash, dot and k
        //public static string JustRut(string input)
        //{

        //}
    }
}
