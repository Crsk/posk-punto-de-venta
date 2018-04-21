using posk.Popups;
using System;
using System.Configuration;
using System.IO;

namespace posk.Globals
{
    static class PoskException
    {
        public static void Make(Exception ex, string mensajePopup)
        {
            // crear archivo de texto
            try
            {
                new NotificarError(":(", mensajePopup, ex);

                string filepath = ConfigurationManager.AppSettings["RutaLog"];

                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(filepath);

                filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(filepath))
                    File.Create(filepath).Dispose();

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("-------- Inicio --------");
                    sw.WriteLine($"Fecha: {DateTime.Now.ToLongDateString()} a las {DateTime.Now.ToLongTimeString()}");
                    sw.WriteLine($"Tipo: {ex.GetType().ToString()}");
                    sw.WriteLine($"Mensaje: {ex.GetType().Name.ToString()}");
                    sw.WriteLine($"Descripción: {ex.Message.ToString()}");
                    sw.WriteLine($"InnerEx: {ex.InnerException}");
                    sw.WriteLine($"Ruta: {ex.StackTrace.Substring(ex.StackTrace.Length - 50, 50)}");
                    sw.WriteLine($"-------- Término --------");
                    sw.WriteLine("");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
            }
        }
    }
}
