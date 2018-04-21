using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    public struct BodegaInventario
    {
        public producto Producto { get; set; }
        public decimal Stock { get; set; }
        public decimal Bodega { get; set; }

        public decimal NoBodega
        {
            get
            {
                if (Stock > Bodega) return (decimal)(Stock - Bodega);
                else return 0;
            }
        }
    }

    static class ItemBodegaInventarioBLL
    {
        private static PoskDB6 db = new PoskDB6();

        public static List<BodegaInventario> ObtenerInventario()
        {
            List<BodegaInventario> lista = new List<BodegaInventario>();

            List<producto> listaProductos = ProductoBLL.ObtenerTodo();
            foreach (producto p in listaProductos)
            {
                if (p.solo_compra == true || p.solo_venta == true) continue;

                BodegaInventario bi = new BodegaInventario()
                {
                    Producto = p,
                    Stock = StockBLL.ObtenerStock(p.id),
                    Bodega = BodegaBLL.ObtenerStock(p.id),
                };
                lista.Add(bi);
            }
            return lista;
        }

    }
}
