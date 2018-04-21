using posk.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace posk.Globals
{
    static class Settings
    {

        public static bool bAyer { get; set; }
        public static bool bHoy { get; set; }
        public static DateTime? IngresosDesde { get; set; }


        // Program settings
        public static bool bAperturaDeCajaRealizada { get; set; }

        public static string LogoString { get; set; }
        public static string NombreDelNegocio { get; set; }
        public static string DescripcionDelNegocio { get; set; }
        public static string DireccionDelNegocio { get; set; }

        // enviar correo
        public static string CorreoDestinatario { get; set; }

        public static string Slogan { get; set; }
        public static int IVA { get; set; }


        // DataBase Connection
        public static string ConnectionString { get; set; }
        public static List<string> ListaConecciones { get; set; }

        // User
        public static usuario Usuario { get; set; }
        public static int UserID { get; set; }
        public static string Nombre { get; set; }
        public static string NombreUsuario { get; set; }
        public static string Foto { get; set; }
        public static string Tipo { get; set; }

        // Impresoras
        public static string ImpresoraCocina { get; set; }
        public static string ImpresoraBar { get; set; }

       // User Settings
        public static int UserSettingsID { get; set; }
        public static string Color { get; set; }
        public static char FontSize { get; set; }
        public static bool UsarImgProductos { get; set; }
        public static bool UsarImgProveedores { get; set; }
        public static bool UsarImgCategorias { get; set; }
        public static bool UsarImgSubCategorias { get; set; }

        public static bool bMenuIsAlreadyCreated { get; set; }
        public static bool bLoggedIn { get; set; }
    }
}
