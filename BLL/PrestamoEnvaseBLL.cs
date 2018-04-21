using posk.Models;
using posk.Partials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace posk.BLL
{
    static class PrestamoEnvaseBLL
    {
        static PoskDB6 db = new PoskDB6();
        public static List<prestamo_envases> GetByClientRut(string clienteRut)
        {
            cliente c = db.clientes.Where(x => x.rut == clienteRut).FirstOrDefault();
            if (c != null)
                return db.prestamo_envases.Where(x => x.cliente_id == c.id).ToList();
            else return new List<prestamo_envases>();
        }

        public static List<PrestamoEnvaseUserDetails> GetPrestamoEnvaseDetails(string clientRut)
        {
            cliente c = db.clientes.Where(x => x.rut == clientRut).FirstOrDefault();
            var listaPrestamos = new List<prestamo_envases>();
            var listaPrestamoDetails = new List<PrestamoEnvaseUserDetails>();
            if (c != null)
            {
                listaPrestamos = db.prestamo_envases.Where(x => x.cliente_id == c.id).ToList();
            }

            foreach (var item in listaPrestamos)
            {
                PrestamoEnvaseUserDetails ped = new PrestamoEnvaseUserDetails()
                {
                    prestamoID = item.id,
                    date = item.fecha,
                    envases = item.envases,
                    userName = item.cliente.nombre
                };
                listaPrestamoDetails.Add(ped);
            }
            return listaPrestamoDetails;
        }

        public static void Delete(int id)
        {
            prestamo_envases pe = db.prestamo_envases.Where(x => x.id == id).FirstOrDefault();
            db.prestamo_envases.Remove(pe);
            db.SaveChanges();
        }

        public static int CuantoDebeCliente(string rutCliente)
        {
            cliente c = db.clientes.Where(x => x.rut == rutCliente).FirstOrDefault();
            if (c != null)
            {
                List<prestamo_envases> peList = db.prestamo_envases.Where(x => x.cliente_id == c.id).ToList();
                int envases = 0;

                foreach (var item in peList)
                {
                    envases += item.envases;
                }
                return envases;
            }
            else
                return 0;
        }

    }
}
