using posk.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace posk.BLL
{
    static class DB
    {
        static PoskDB6 db = new PoskDB6();
        static IObjectContextAdapter ctx = ((IObjectContextAdapter)db).ObjectContext;

        public static void SaveChanges()
        {
            db.SaveChanges();
        }

        // Generic methods
        public static void Insert<T>(string tableName, T obj)
        {
            ctx.ObjectContext.AddObject(tableName, obj);
            db.SaveChanges();
        }
        // Generic methods


        //public static int? GetIncomesOfTheDay()
        //{
        //    List<detail_lines> incomes = db.detail_lines.Where( x => x.ammount != 0 || x.discount != 0).ToList();
        //    int? totalIncomes = 0;
        //    foreach (var item in incomes)
        //    {
        //        if (item.ammount != 0)
        //            totalIncomes += item.ammount;
        //        if (item.discount != 0)
        //        {
        //            decimal discount = Math.Round(item.discount, 0);
        //            totalIncomes += Convert.ToInt32(item.discount);
        //        }
        //    }
        //    return totalIncomes;
        //}

        public static punto GetPoints(cliente client)
        {
            return db.puntos.Where(x => x.id == client.puntos_id).FirstOrDefault();
        }
        
        
        // Product methods
        public static List<producto> GetProducts(string filtro)
        {
            if (filtro.ToLower() == "todo" || filtro.ToLower() == "*")
                return db.productos.ToList();
            else
                return db.productos.Where(x => x.nombre.Contains(filtro)).ToList();
        }

        public static void UpdateProduct(producto newProduct)
        {
            producto original = db.productos.Where(x => x.codigo_barras == newProduct.codigo_barras).FirstOrDefault();
            if (original != null)
            {
                original.nombre = newProduct.nombre;
                original.precio = newProduct.precio;
                original.puntos_cantidad = newProduct.puntos_cantidad;
                original.imagen = newProduct.imagen;
                original.sub_categoria_id = newProduct.sub_categoria_id;
                db.SaveChanges();
            }
        }

        public static void Delete(string barCode)
        {
            producto p = db.productos.Where(x => x.codigo_barras == barCode).FirstOrDefault();
            db.productos.Remove(p);
            db.SaveChanges();
        }
        // Product methods



        // Purchase methods
        //public static List<product_purchase> GetPurchase(int purchaseId)
        //{
        //    List<purchases> list = db.purchases.Where(x => x.product_id == id).ToList();
        //    list = (from x in list orderby x.date select x).ToList();
        //    return list;
        //}

        //public static decimal GetStock(int id)
        //{
        //    List<purchases> purchasesFiltered = db.purchases.Where(x => x.product_id == id).ToList();
        //    decimal stock = 0;
        //    foreach (purchases purch in purchasesFiltered)
        //    {
        //        stock += purch.quantity;
        //    }
        //    return stock;
        //}

        //public static void DeletePurchase(int id)
        //{
        //    purchases p = db.purchases.Where(x => x.id == id).FirstOrDefault();
        //    if (p != null)
        //    {
        //        db.purchases.Remove(p);
        //        db.SaveChanges();
        //    }
        //    //else
        //    //    return "error, no existe la compra";
        //}

        //public static void AddPurchase(int productId, string instigator, decimal unitaryCost, decimal quantityToAdd, DateTime date)
        //{
        //    purchases purch = new purchases() { product_id = productId, instigator = instigator, unitary_cost = unitaryCost, quantity = quantityToAdd, date = date };
        //    db.purchases.Add(purch);
        //    db.SaveChanges();
        //}

        //public static string AddStock(string productName, int productId, decimal unitaryCost, string instigator, decimal quantityToAdd)
        //{
        //    purchases purch = new purchases() { product_id = productId, instigator = instigator, unitary_cost = unitaryCost, quantity = quantityToAdd, date = DateTime.Now };
        //    string msg = "SE AGREGÓ UNA COMPRA";
        //    msg += "\n-Producto: " + productName;
        //    msg += "\n-Cantidad: " + quantityToAdd;
        //    msg += "\n-Fecha: " + DateTime.Now;
        //    db.purchases.Add(purch);
        //    db.SaveChanges();
        //    return msg;
        //}

        //public static string ReduceStock(int productId, decimal quantityToReduce)
        //{
        //    List<purchases> purchasesFiltered = db.purchases.Where(x => x.product_id == productId).ToList();

        //    decimal fifoPurchasedQuantity;
        //    decimal reducedQuantity = 0;
        //    int deletedPurchases = 0;

        //    // making fifo
        //    purchasesFiltered = (from x in purchasesFiltered orderby x.date select x).ToList();
        //    foreach (purchases purch in purchasesFiltered)
        //    {
        //        // obtengo la cantidad de la primera compra, le reduzco la cantidad que haya quitado y la guardo fuera del ciclo for
        //        fifoPurchasedQuantity = purch.quantity;

        //        // si el que la cantidad a reducir es menor a la cantidad existente de la primera compra, reducir directamente
        //        if (quantityToReduce < fifoPurchasedQuantity)
        //        {
        //            purch.quantity -= quantityToReduce;
        //            db.SaveChanges();
        //            return "Se redujo el stock -> " + quantityToReduce + " - se eliminaron " + deletedPurchases + " compras";
        //        }
        //        // si es que la cantidad a reducir es igual a la cantidad existente en la primera compra, eliminar la compra
        //        else if (quantityToReduce == fifoPurchasedQuantity)
        //        {
        //            db.purchases.Remove(purch);
        //            db.SaveChanges();
        //            deletedPurchases++;
        //            return "Se redujo el stock -> " + quantityToReduce + " - se eliminaron " + deletedPurchases + " compras";
        //        }
        //        // si es que la catidad a reducir es mayor a la cantidad existente en la primera compra, eliminar la compra y seguir iterando...
        //        else
        //        {
        //            db.purchases.Remove(purch);
        //            reducedQuantity += purch.quantity;
        //            quantityToReduce -= purch.quantity;
        //            deletedPurchases++;
        //            continue;
        //        }
        //    }
        //    db.SaveChanges();
        //    return "Se redujo el stock -> " + reducedQuantity + " - se eliminaron " + deletedPurchases + " compras";
        //}
        // Purchase methods



        // Client methods
        public static List<cliente> GetClients(string filtro)
        {
            if (filtro.ToLower() == "todo" || filtro.ToLower() == "*")
                return db.clientes.ToList();
            else
                return db.clientes.Where(x => x.nombre.Contains(filtro)).ToList();
        }

        public static cliente GetClient(string rut)
        {
            return db.clientes.Where(x => x.rut == rut).FirstOrDefault();
        }

        public static void AddClient(cliente c)
        {
            db.clientes.Add(c);
            db.SaveChanges();
        }

        public static void VipToggle(cliente c)
        {
            c.favorito ^= true;
            db.SaveChanges();
        }

        public static void UpdateClient(string rut, cliente newClient)
        {
            cliente c = db.clientes.Where(x => x.rut == rut).FirstOrDefault();
            c = newClient;
            db.SaveChanges();
        }
        // Client methods



        // Point methods
        public static void UpdatePoints(punto p, int ammount)
        {
            p.puntos_activos = ammount;
            p.fecha_expiracion = DateTime.Today.AddDays(30);
            db.SaveChanges();
        }
        // Point methods


        // User Methods
        public static string GetUserName(int id)
        {
            usuario u = db.usuarios.Where(x => x.id == id).FirstOrDefault();
            if (u != null)
                return u.nombre;
            else
                return "";
        }

        // Category methods
        public static string GetCategoryName(int id)
        {
            categoria c = db.categorias.Where(x => x.id == id).FirstOrDefault();
            if (c != null)
                return c.nombre;
            else
                return "";
        }

        // SubCategory methods
        public static string GetSubCategoryName(int id)
        {
            subcategoria sc = db.subcategorias.Where(x => x.id == id).FirstOrDefault();
            if (sc != null)
                return sc.nombre;
            else
                return "";
        }
    }
}
