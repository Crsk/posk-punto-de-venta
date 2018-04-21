using posk.Globals;
using posk.Models;
using posk.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    public struct MateriaPrimaDetalle
    {
        public materiasprima MateriaPrima { get; set; }
        public decimal? Entrada { get; set; }
        public decimal? Salida { get; set; }
        public decimal? Ajuste { get; set; }
        public decimal? Disponible {
            get
            {
                if (Entrada == null) Entrada = 0;
                if (Salida == null) Salida = 0;
                if (Ajuste == null) Ajuste = 0;
                return (decimal)(Entrada - Salida + Ajuste);
            }
        }
    }


    static class StockmpBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void BorrarTodo()
        {
            db.stock_mp.ToList().ForEach(x => db.stock_mp.Remove(x));
            db.SaveChanges();
        }

        public static stock_mp Obtener(int? mpID)
        {
            return db.stock_mp.Where(x => x.materiaprima_id == mpID).FirstOrDefault();
        }

        public static void Aumentar(int smpID, decimal cantidad)
        {
            stock_mp smp = db.stock_mp.Where(x => x.id == smpID).FirstOrDefault();
            smp.entrada += cantidad;
            db.SaveChanges();
        }

        public static void Reducir(int smpID, decimal cantidad)
        {
            stock_mp smp = db.stock_mp.Where(x => x.id == smpID).FirstOrDefault();
            smp.salida += cantidad;
            db.SaveChanges();
        }

        public static stock_mp Crear(int? mpID, decimal entrada, decimal salida, decimal ajuste)
        {
            stock_mp smp = new stock_mp();
            try
            {
                smp.materiaprima_id = mpID;
                smp.entrada = entrada;
                smp.salida = salida;
                smp.ajuste = ajuste;

                db.stock_mp.Add(smp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al crear stockmp");
            }
            return smp;
        }

        public static List<stock_mp> ObtenerTodo()
        {
            return db.stock_mp.AsNoTracking().OrderBy(x => x.entrada - x.salida + x.ajuste).ToList();
        }

        /// <summary>
        /// Obtiene una lista de objeto MateriaPrimaDetalle el cual contiene propiedad Disponible como adicional
        /// </summary>
        /// <returns></returns>
        public static List<MateriaPrimaDetalle> ObtenerDetalle()
        {
            List<MateriaPrimaDetalle> listaMateriaPrimaDetalle = new List<MateriaPrimaDetalle>();
            db.stock_mp.AsNoTracking().OrderBy(x => x.entrada - x.salida + x.ajuste).ToList().ForEach(smp =>
            {
                listaMateriaPrimaDetalle.Add(new MateriaPrimaDetalle() { MateriaPrima = smp.materiasprima, Entrada = smp.entrada, Salida = smp.salida, Ajuste = smp.ajuste });
            });
            return listaMateriaPrimaDetalle;
        }

        public static void Eliminar(int stockmpID)
        {
            db.stock_mp.Remove(db.stock_mp.Where(x => x.id == stockmpID).FirstOrDefault());
            db.SaveChanges();
        }

        public static void Actualizar(int stockmpID, decimal stockNuevo)
        {
            stock_mp smp = db.stock_mp.Where(x => x.id == stockmpID).FirstOrDefault();
            decimal stockAnterior = smp.entrada - smp.salida + smp.ajuste;
            smp.ajuste += stockNuevo - stockAnterior;
            db.SaveChanges();
        }
    }
}
