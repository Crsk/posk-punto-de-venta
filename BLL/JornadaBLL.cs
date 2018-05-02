using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    public struct JornadaDetalle
    {
        public int ID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }
        public bool? Especial { get; set; }
        public string Mensaje { get; set; }
        public int? Ingresos { get; set; }
        public usuario Usuario { get; set; }
        public string NombreMostrar
        {
            get
            {
                string mostrar = "";
                if (FechaInicio != null && FechaCierre != null)
                    mostrar = $"{ID} [DEL {FechaInicio.Value.ToLongDateString()} {FechaInicio.Value.ToLongTimeString()}    AL    {FechaCierre.Value.ToLongDateString()} {FechaCierre.Value.ToLongTimeString()}]    {Mensaje}";
                return mostrar;
            }
        }
    }

    static class JornadaBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<JornadaDetalle> ObtenerTodas()
        {
            List<JornadaDetalle> listaJornadaDetalle = new List<JornadaDetalle>();
            List<jornada> listaJornada = db.jornadas.Include("usuario").OrderBy(x => x.id).ToList();
            listaJornada.Reverse();
            foreach (jornada j in listaJornada)
            {
                listaJornadaDetalle.Add(new JornadaDetalle()
                {
                    ID = j.id,
                    FechaInicio = j.fecha_apertura,
                    FechaCierre = j.fecha_cierre,
                    Especial = j.especial,
                    Mensaje = j.mensaje,
                    Ingresos = j.ingresos,
                    Usuario = j.usuario
                });
            }
            return listaJornadaDetalle;
        }

        public static void CrearJornadaSiNoExiste(string mensaje = "")
        {
            if (!JornadaAbierta())
            {
                bool bEspecial = mensaje == "" ? false : true;
                //TerminarJornadaSiEstaIniciada(UltimaJornada(), DateTime.Now, false, 0, "");
                db.jornadas.Add(new jornada() { fecha_apertura = DateTime.Now, fecha_cierre = null, especial = bEspecial, ingresos = 0, mensaje = mensaje, usuario_id = Settings.Usuario.id });
                db.boletas.OrderBy(x => x.id).ToList().LastOrDefault().numero_boleta = 0;
                db.SaveChanges();
            }
        }

        public static bool JornadaAbierta()
        {
            if (UltimaJornada()?.fecha_cierre == null)
                return true;
            else
                return false;
        }

        public static void TerminarJornadaSiEstaIniciada(DateTime fechaCierre, int ingresos, string mensaje)
        {
            if (JornadaAbierta())
            {
                var bEspecial = mensaje == "" ? false : true;
                var j = UltimaJornada();
                j.fecha_cierre = fechaCierre;
                j.especial = bEspecial;
                j.ingresos = ingresos;
                j.mensaje = mensaje;
                db.boletas.OrderBy(x => x.id).ToList().LastOrDefault().numero_boleta = 0;
                db.SaveChanges();
            }
        }

        public static jornada UltimaJornada()
        {
            return db.jornadas.Include("usuario").OrderBy(x => x.id).ToList().LastOrDefault();
        }
        public static jornada UltimaJornadaCerrada()
        {
            return db.jornadas.Include("usuario").Where(x => x.fecha_cierre != null).OrderBy(x => x.id).ToList().LastOrDefault();
        }

        public static void EstablecerCajaInicial(int monto)
        {
            try
            {
                jornada j = UltimaJornada();
                if (j == null) return;
                j.caja_inicial_efectivo = monto;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL GUARDAR CAJA INICIAL");
            }
        }

        public static int ObtenerCajaInicialJornadaActual()
        {
            if (UltimaJornada() == null) return 0;
            return UltimaJornada().caja_inicial_efectivo;
        }

        public static int ObtenerCajaInicialJornadaAnterior()
        {
            if (UltimaJornadaCerrada() == null) return 0;
            return UltimaJornadaCerrada().caja_inicial_efectivo;
        }

        /// <summary>
        /// retorna true si la última jornada no tiene fecha de cierre
        /// </summary>
        /// <returns></returns>
        public static bool UltimaJornadaTerminada()
        {
            if (UltimaJornada()?.fecha_cierre == null)
                return false;
            else
                return true;
        }

        //public static int ObtenerIngresosJornadaActual()
        //{
        //}
    }
}
