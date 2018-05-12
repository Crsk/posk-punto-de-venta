using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace posk.BLL
{
    static class CompraBLL
    {
        static PoskDB6 db = new PoskDB6();

        public static List<compra> ObtenerTodas()
        {
            return db.compras.AsNoTracking().Include("usuario").ToList();
        }

        public static void BorrarTodo()
        {
            db.compras.ToList().ForEach(x => db.compras.Remove(x));
            db.SaveChanges();
        }

        public static compra CreatePurchase(DateTime date, int usuarioID)
        {
            compra p = new compra() { usuario_id = usuarioID, fecha = date };
            db.compras.Add(p);
            db.SaveChanges();
            return p;
        }

        public static compra ObtainById(int purchaseId)
        {
            compra p = db.compras.Where(x => x.id == purchaseId).FirstOrDefault();
            return p;
        }

        public static void DeleteById(int id)
        {
            // testear on cascade
            compra p = db.compras.Where(x => x.id == id).FirstOrDefault();
            if (p != null)
            {
                db.compras.Remove(p);
                db.SaveChanges();
            }
            //else
            //    return "error, no existe la compra";
        }

        // sum of all purchases stock of specific product
        public static decimal GetStockByProductId(int id)
        {
            List<producto_compra> purchasesFiltered = db.producto_compra.Where(x => x.producto_id == id).ToList();
            decimal stock = 0;
            foreach (producto_compra pp in purchasesFiltered)
            {
                stock += pp.cantidad_disponible;
            }
            return stock;
        }

        /// <summary>
        /// Funcionalidad quitada,
        /// permitía agregar stock a un producto a momento de su creación
        /// para simplicidad, ahora solo se puede agregar stock desde la sección agregar stock
        /// </summary>
        /// <returns></returns>
        public static string AddStockByProduct(string productName, int productId, decimal unitaryCost, decimal quantityToAdd)
        {
            compra purchase = CreatePurchase(DateTime.Now, Settings.Usuario.id);
            producto_compra pp = new producto_compra() { compra_id = purchase.id, producto_id = productId, cantidad_disponible = quantityToAdd, cantidad_compra = quantityToAdd, costo_unitario = unitaryCost };
            stock_pr skpr = db.stock_pr.Where(x => x.producto_id == productId).FirstOrDefault();
            if (skpr == null)
            {
                db.stock_pr.Add(new stock_pr() { producto_id = productId, entrada = quantityToAdd, salida = 0, ajuste = 0 });
            }
            else
            {
                skpr.entrada += quantityToAdd;
            }
            db.SaveChanges();

            string msg = "SE AGREGÓ UNA COMPRA";
            msg += "\n-Producto: " + productName;
            msg += "\n-Cantidad: " + quantityToAdd;
            msg += "\n-Fecha: " + DateTime.Now;
            db.producto_compra.Add(pp);
            db.SaveChanges();
            return msg;
        }

        public static void AumentarStock(int productoId, decimal cantidadAgregar)
        {
            List<producto_compra> ppList = db.producto_compra.Where(x => x.producto_id == productoId).ToList();
            ppList = (from x in ppList orderby x.compra.fecha select x).ToList();
            producto_compra cp = ppList.LastOrDefault();
            cp.cantidad_disponible += cantidadAgregar;

            db.stock_pr.Where(x => x.producto_id == productoId).FirstOrDefault().salida -= cantidadAgregar;

            db.SaveChanges();
        }

        public static void ReducirCrearSMP(producto_materiaprima pmp, decimal cantidad)
        {
            decimal cantidadReducir = 0;
            if (pmp.cantidad == 0) cantidadReducir = cantidad;
            else cantidadReducir = (decimal)pmp.cantidad * cantidad;

            stock_mp smp = StockmpBLL.Obtener(pmp.materiaprima_id);
            if (smp != null)
                StockmpBLL.Reducir(smp.id, cantidadReducir);
            else
                StockmpBLL.Crear(pmp.materiaprima_id, 0, cantidadReducir, 0);
        }

        public static void AumentarCrearSMP(producto_materiaprima pmp, decimal cantidad)
        {
            decimal cantidadAumentar = 0;
            if (pmp.cantidad == 0) cantidadAumentar = cantidad;
            else cantidadAumentar = (decimal)pmp.cantidad * cantidad;

            stock_mp smp = StockmpBLL.Obtener(pmp.materiaprima_id);
            if (smp != null)
                StockmpBLL.Aumentar(smp.id, cantidadAumentar);
            else
                StockmpBLL.Crear(pmp.materiaprima_id, cantidadAumentar, 0, 0);
        }

        /*
         * Reduce stock tomando las compras realizadas en el pasado 
         * utilizando método FIFO, es decir que toma la última venta y le reduce stock
         * cuando esta compra se queda sin productos, busca la siguiente compra que se realizó
         * para reducir stock desde allí
         */
        public static string ReduceStockByProduct(int? productId, int? promoId, decimal quantityToReduce)
        {
            // TODO - reducir materia prima asociada al producto

            // si promoId no es null, obtengo la promo
            if (promoId != null)
            {
                // obtengo una lista de los productos de esa promo
                ProductoPromocionBLL.ObtenerProductos(promoId).ForEach(x =>
                {
                    // reduzco stock mediante FIFO de esa lista de productos
                    ReduceStockByProduct(x.id, null, quantityToReduce);
                });
            }
            try
            {
                if (productId != null)
                {
                    stock_pr spr = db.stock_pr.Where(x => x.producto_id == productId).FirstOrDefault();
                    if (spr != null)
                    {
                        spr.salida += quantityToReduce;
                        db.SaveChanges();
                    }
                    else
                    {
                        spr = new stock_pr() { producto_id = productId, entrada = 0, salida = quantityToReduce, ajuste = 0 };
                        db.stock_pr.Add(spr);
                        db.SaveChanges();
                    }
                }

                // TEST
                // reduce stock de materia prima que contiene un producto
                try
                {
                    // 1. producto: c, mp: limon 50 ml - 2. producto: c, mp: cerveza 200 ml

                    if (productId != null)
                    {
                        ProductoMateriaPrimaBLL.ObtenerTodo((int)productId).ForEach(pmp => ReducirCrearSMP(pmp, quantityToReduce));
                    }
                }
                catch (Exception ex)
                {
                    PoskException.Make(ex, "Error al reducir stock de materiaprima");
                }
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "Error al reducir stock de item venta");
            }

            List<producto_compra> ppList = db.producto_compra.Where(x => x.producto_id == productId && x.activa == true).ToList();

            decimal fifoPurchasedQuantity;
            decimal reducedQuantity = 0;
            int deletedPurchases = 1;

            ppList = (from x in ppList orderby x.compra.fecha select x).ToList();

            foreach (producto_compra pp in ppList)
            {
                if (pp.activa == true)
                {
                    // obtengo la cantidad de la primera compra, le reduzco la cantidad que haya quitado y la guardo fuera del ciclo for
                    fifoPurchasedQuantity = pp.cantidad_disponible;

                    // si el que la cantidad a reducir es menor a la cantidad existente de la primera compra, reducir directamente
                    if (quantityToReduce < fifoPurchasedQuantity)
                    {
                        pp.cantidad_disponible -= quantityToReduce;
                        db.SaveChanges();
                        return "Se redujo el stock -> " + quantityToReduce + " - en " + deletedPurchases + " compra";
                    }
                    // si es que la cantidad a reducir es igual a la cantidad existente en la primera compra, eliminar la compra
                    else if (quantityToReduce == fifoPurchasedQuantity)
                    {
                        //db.producto_compra.Remove(pp);
                        pp.activa = false;
                        pp.cantidad_disponible = 0;
                        db.SaveChanges();
                        deletedPurchases++;
                        return "Se redujo el stock -> " + quantityToReduce + " - en " + deletedPurchases + " compras";
                    }
                    // si es que la catidad a reducir es mayor a la cantidad existente en la primera compra, eliminar la compra y seguir iterando...
                    else
                    {
                        //db.producto_compra.Remove(pp);
                        pp.activa = false;
                        reducedQuantity += pp.cantidad_disponible;
                        quantityToReduce -= pp.cantidad_disponible;
                        pp.cantidad_disponible = 0;
                        deletedPurchases++;
                        continue;
                    }
                }
            }
            db.SaveChanges();
            return "Se redujo el stock -> " + reducedQuantity + " - en " + deletedPurchases + " compras";
        }
    }
}
