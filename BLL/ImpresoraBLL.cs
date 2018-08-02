using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class ImpresoraBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static void EstablecerSectorImpresoras(string sectorUno, string impresoraUno, string sectorDos, string impresoraDos, string sectorTres, string impresoraTres, string sectorCuatro, string impresoraCuatro, string sectorCinco, string impresoraCinco)
        {
            sector_impresion siUno = db.sector_impresion.Where(x => x.nombre == sectorUno).FirstOrDefault();
            siUno.impresora = impresoraUno;
            sector_impresion siDos = db.sector_impresion.Where(x => x.nombre == sectorUno).FirstOrDefault();
            siDos.impresora = impresoraDos;
            sector_impresion siTres = db.sector_impresion.Where(x => x.nombre == sectorUno).FirstOrDefault();
            siTres.impresora = impresoraTres;
            sector_impresion siCuatro = db.sector_impresion.Where(x => x.nombre == sectorUno).FirstOrDefault();
            siCuatro.impresora = impresoraCuatro;
            sector_impresion siCinco = db.sector_impresion.Where(x => x.nombre == sectorUno).FirstOrDefault();
            siCinco.impresora = impresoraCinco;

            db.SaveChanges();
        }

        public static void EstablecerSectorImpresora(string sector, string impresora)
        {
            sector_impresion si = db.sector_impresion.Where(x => x.nombre == sector).FirstOrDefault();
            si.impresora = impresora;
            db.SaveChanges();
        }

    }
}
