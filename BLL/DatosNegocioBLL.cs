using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace posk.BLL
{
    static class DatosNegocioBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static void SetDescripcionNegocio(string nombre, string mensaje, string direccion, string logo)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.nombre = nombre;
            dn.mensaje = mensaje;
            dn.direccion = direccion;
            dn.logo = logo;
            db.SaveChanges();
        }

        public static string ObtenerImagenUrl()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            return dn.logo;
        }

        public static void EstablecerModo(string modo)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.modo = modo;
            db.SaveChanges();
        }

        public static void GuardarConfiguracionTeclado(bool? b)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.teclado_tactil_integrado = b;
            db.SaveChanges();
        }

        public static bool? ObtenerConfiguracionTeclado()
        {
            return db.datos_negocio.FirstOrDefault().teclado_tactil_integrado;
        }

        public static string ObtenerModo()
        {
            return db.datos_negocio.FirstOrDefault().modo;
        }

        public static int? ObtenerIva()
        {
            return db.datos_negocio.FirstOrDefault().iva;
        }

        public static string ObtenerCorreoPrimario()
        {
            return db.datos_negocio.FirstOrDefault().correo_primario;
        }

        public static string ObtenerCorreoSecundario()
        {
            return db.datos_negocio.FirstOrDefault().correo_secundario;
        }

        public static string ObtenerNombreNegocio()
        {
            return db.datos_negocio.FirstOrDefault().nombre;
        }

        public static void SetJornada(string inicioJornadaDia, string terminoJornadaDia, TimeSpan inicioJornadaHora, TimeSpan terminoJornadaHora)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.inicio_jornada_dia = inicioJornadaDia;
            dn.termino_jornada_dia = terminoJornadaDia;
            dn.inicio_jornada_hora = inicioJornadaHora;
            dn.termino_jornada_hora = terminoJornadaHora;
            db.SaveChanges();
        }

        public static void SetIvaPct(int ivaPct)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.iva = ivaPct;
            db.SaveChanges();
        }

        public static int IvaPct()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            return (int) dn.iva;
        }

        public static decimal IvaPctDecimal__0_iva()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            int ivaPct = (int)dn.iva;
            decimal ivaPctDecimal = 0.19M;
            switch (ivaPct)
            {
                case 10:
                    ivaPctDecimal = 0.10M;
                    break;
                case 11:
                    ivaPctDecimal = 0.11M;
                    break;
                case 12:
                    ivaPctDecimal = 0.12M;
                    break;
                case 13:
                    ivaPctDecimal = 0.13M;
                    break;
                case 14:
                    ivaPctDecimal = 0.14M;
                    break;
                case 15:
                    ivaPctDecimal = 0.15M;
                    break;
                case 16:
                    ivaPctDecimal = 0.16M;
                    break;
                case 17:
                    ivaPctDecimal = 0.17M;
                    break;
                case 18:
                    ivaPctDecimal = 0.18M;
                    break;
                case 19:
                    ivaPctDecimal = 0.19M;
                    break;
                case 20:
                    ivaPctDecimal = 0.20M;
                    break;
                case 21:
                    ivaPctDecimal = 0.21M;
                    break;
                case 22:
                    ivaPctDecimal = 0.22M;
                    break;
                case 23:
                    ivaPctDecimal = 0.23M;
                    break;
                case 24:
                    ivaPctDecimal = 0.24M;
                    break;
                case 25:
                    ivaPctDecimal = 0.25M;
                    break;
                case 26:
                    ivaPctDecimal = 0.26M;
                    break;
                case 27:
                    ivaPctDecimal = 0.27M;
                    break;
                case 28:
                    ivaPctDecimal = 0.28M;
                    break;
                case 29:
                    ivaPctDecimal = 0.29M;
                    break;
                case 30:
                    ivaPctDecimal = 0.30M;
                    break;
                case 31:
                    ivaPctDecimal = 0.31M;
                    break;
                case 32:
                    ivaPctDecimal = 0.32M;
                    break;
                case 33:
                    ivaPctDecimal = 0.33M;
                    break;
                case 34:
                    ivaPctDecimal = 0.34M;
                    break;
                case 35:
                    ivaPctDecimal = 0.35M;
                    break;
            }
            return ivaPctDecimal;
        }

        public static decimal IvaPctDecimal__1_iva()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            int ivaPct = (int)dn.iva;
            decimal ivaPctDecimal = 1.19M;
            switch (ivaPct)
            {
                case 10:
                    ivaPctDecimal = 1.10M;
                    break;
                case 11:
                    ivaPctDecimal = 1.11M;
                    break;
                case 12:
                    ivaPctDecimal = 1.12M;
                    break;
                case 13:
                    ivaPctDecimal = 1.13M;
                    break;
                case 14:
                    ivaPctDecimal = 1.14M;
                    break;
                case 15:
                    ivaPctDecimal = 1.15M;
                    break;
                case 16:
                    ivaPctDecimal = 1.16M;
                    break;
                case 17:
                    ivaPctDecimal = 1.17M;
                    break;
                case 18:
                    ivaPctDecimal = 1.18M;
                    break;
                case 19:
                    ivaPctDecimal = 1.19M;
                    break;
                case 20:
                    ivaPctDecimal = 1.20M;
                    break;
                case 21:
                    ivaPctDecimal = 1.21M;
                    break;
                case 22:
                    ivaPctDecimal = 1.22M;
                    break;
                case 23:
                    ivaPctDecimal = 1.23M;
                    break;
                case 24:
                    ivaPctDecimal = 1.24M;
                    break;
                case 25:
                    ivaPctDecimal = 1.25M;
                    break;
                case 26:
                    ivaPctDecimal = 1.26M;
                    break;
                case 27:
                    ivaPctDecimal = 1.27M;
                    break;
                case 28:
                    ivaPctDecimal = 1.28M;
                    break;
                case 29:
                    ivaPctDecimal = 1.29M;
                    break;
                case 30:
                    ivaPctDecimal = 1.30M;
                    break;
                case 31:
                    ivaPctDecimal = 1.31M;
                    break;
                case 32:
                    ivaPctDecimal = 1.32M;
                    break;
                case 33:
                    ivaPctDecimal = 1.33M;
                    break;
                case 34:
                    ivaPctDecimal = 1.34M;
                    break;
                case 35:
                    ivaPctDecimal = 1.35M;
                    break;
            }
            return ivaPctDecimal;
        }

        public static datos_negocio GetDatosNegocio()
        {
            return db.datos_negocio.FirstOrDefault();
        }

        public static TimeSpan GetHoraInicioJornada()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            return TimeSpan.Parse(dn.inicio_jornada_hora.ToString());
        }
        public static TimeSpan GetHoraTerminoJornada()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            return TimeSpan.Parse(dn.termino_jornada_hora.ToString());
        }

        public static void GuardarCorreos(string correoPrimario, string correoSecundario)
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            dn.correo_primario = correoPrimario;
            dn.correo_secundario = correoSecundario;
            db.SaveChanges();

        }

        public static bool JornadeDeUnDia()
        {
            datos_negocio dn = db.datos_negocio.FirstOrDefault();
            string dia1 = dn.inicio_jornada_dia;
            string dia2 = dn.termino_jornada_dia;

            if (dia1 == dia2)
                return true;
            else
                return false;
        }
    }
}
