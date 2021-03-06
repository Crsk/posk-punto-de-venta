﻿using posk.BLL;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace posk.Pages.Menu
{
    public partial class PageEstadisticasRangoFechas : Page
    {
        struct ProductoCantidad
        {
            public producto Producto { get; set; }
            public string Opcion { get; set; }
            public int Cantidad { get; set; }
            public decimal? SubTotal { get; set; }
            public decimal? Adicional { get; set; }
            public decimal? Total { get; set; }

            public void AumentarCantidad(int cantidad)
            {
                Cantidad += cantidad;
            }
        }
        struct PromoCantidad
        {
            public promocione Promo { get; set; }
            public decimal? Cantidad { get; set; }
        }

        public PageEstadisticasRangoFechas()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                //lbInicioJornada.Content = $"Inicio jornada ({Settings.Usuario.nombre}): {JornadaBLL.UltimaJornada().fecha_apertura}";
                dgResumenVentas.DataContext = null;
                dgResumenVentasPromo.DataContext = null;
            };

            btnVer.Click += (se, a) =>
            {
                CargarContenido();
                // lbInicioJornada.Content = $"({Settings.Usuario.nombre}) Inicio: {j.fecha_apertura}";
            };
        }

        private List<PromoCantidad> ContarPromosEnLista(int jornadaId)
        {
            List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();

            VentasJornadaBLL.ObtenerTodo().ForEach(vj =>
            {
                if (vj.detalle_boleta.promocion_id != null && vj.jornada_id == jornadaId)
                    listaPromosCantidad.Add(new PromoCantidad() { Promo = vj.detalle_boleta.promocione, Cantidad = vj.cantidad });
            });

            return listaPromosCantidad.OrderBy(x => x.Cantidad).Reverse().ToList();
        }

        private List<ProductoCantidad> ContarProductosEnLista(DateTime desde, DateTime hasta)
        {
            List<ventas_jornada> listaVentasRangoFechas = VentasJornadaBLL.ObtenerVentasRangoFechas(desde, hasta);
            List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
            List<ProductoCantidad> listaProductoCantidadAgrupado = new List<ProductoCantidad>();
            listaVentasRangoFechas.ForEach(vj => AgregarProductoCantidad(listaProductoCantidad, vj));
            foreach (var vj in listaVentasRangoFechas)
            {
                ProductoCantidad pcExistente = listaProductoCantidadAgrupado.Where(x => x.Producto?.id == vj?.detalle_boleta?.producto?.id && x.Opcion == vj?.opcion).FirstOrDefault();
                if (pcExistente.Producto != null && vj?.opcion == pcExistente.Opcion)
                    AgregarProductoCantidadExistente(listaProductoCantidadAgrupado, vj, pcExistente);
                else
                    AgregarProductoCantidad(listaProductoCantidadAgrupado, vj);
            }
            return listaProductoCantidadAgrupado.OrderBy(x => x.Cantidad).OrderBy(x => x.Producto?.nombre).OrderBy(x => x.Opcion).Reverse().ToList();
        }

        private void AgregarProductoCantidadExistente(List<ProductoCantidad> listaProductoCantidad, ventas_jornada vj, ProductoCantidad pcExistente)
        {
            listaProductoCantidad.Remove(pcExistente);
            listaProductoCantidad.Add(new ProductoCantidad()
            {
                Producto = vj.detalle_boleta?.producto,
                Cantidad = pcExistente.Cantidad + vj.cantidad,
                Opcion = vj.opcion,
                Adicional = vj.cobro_extra + pcExistente.Adicional,
                SubTotal = vj.detalle_boleta?.producto?.precio * (vj.cantidad + pcExistente.Cantidad),
                Total = vj.detalle_boleta?.producto?.precio * (vj.cantidad + pcExistente.Cantidad) + vj?.cobro_extra
            });
        }

        private void AgregarProductoCantidad(List<ProductoCantidad> listaProductoCantidad, ventas_jornada vj)
        {
            listaProductoCantidad.Add(new ProductoCantidad()
            {
                Producto = vj?.detalle_boleta?.producto,
                Cantidad = vj.cantidad,
                Opcion = vj?.opcion,
                Adicional = vj?.cobro_extra,
                SubTotal = vj?.detalle_boleta?.producto?.precio * vj.cantidad,
                Total = vj?.detalle_boleta?.producto?.precio * vj.cantidad + vj?.cobro_extra
            });
        }

        private void CargarContenido()
        {
            try
            {
                List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
                List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();

                DateTime desde = Convert.ToDateTime($"{fechaDesde.Text} {tiempoDesde.Text}");
                DateTime hasta = Convert.ToDateTime($"{fechaHasta.Text} {tiempoHasta.Text}");

                listaProductoCantidad = ContarProductosEnLista(desde, hasta);
                //listaPromosCantidad = ContarPromosEnLista(jornadaId);

                dgResumenVentas.DataContext = null;
                dgResumenVentas.DataContext = listaProductoCantidad;

                dgResumenVentasPromo.DataContext = null;
                dgResumenVentasPromo.DataContext = listaPromosCantidad;
            }
            catch (Exception ex)
            {
                PoskException.Make(ex, "ERROR AL CARGAR RESUMEN JORNADA");
            }
        }

        
    }
}
