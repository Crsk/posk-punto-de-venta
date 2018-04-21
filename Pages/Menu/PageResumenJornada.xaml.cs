using posk.BLL;
using posk.Globals;
using posk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace posk.Pages.Menu
{
    public partial class PageResumenJornada : Page
    {

        struct ProductoCantidad
        {
            public producto Producto { get; set; }
            public decimal? Cantidad { get; set; }
            public decimal? SubTotal { get; set; }
            public decimal? Adicional { get; set; }
            public decimal? Total { get; set; }
        }
        struct PromoCantidad
        {
            public promocione Promo { get; set; }
            public decimal? Cantidad { get; set; }
        }

        public PageResumenJornada()
        {
            InitializeComponent();

            Loaded += (se, a) =>
            {
                lbInicioJornada.Content = $"Inicio jornada ({Settings.Usuario.nombre}): {JornadaBLL.UltimaJornada().fecha_apertura}";
                dgResumenVentas.DataContext = null;
                dgResumenVentasPromo.DataContext = null;
                rbInicioJornadaAnterior.IsChecked = false;
                rbInicioJornadaActual.IsChecked = false;
            };

            rbInicioJornadaAnterior.Checked += (se, a) =>
            {
                jornada j = JornadaBLL.UltimaJornadaCerrada();
                CargarContenido(j.id);
                lbInicioJornada.Content = $"Inicio jornada ({Settings.Usuario.nombre}): {j.fecha_apertura}";
            };
            rbInicioJornadaActual.Checked += (se, a) =>
            {
                jornada j = JornadaBLL.UltimaJornada();
                CargarContenido(j.id);
                lbInicioJornada.Content = $"Inicio jornada ({Settings.Usuario.nombre}): {j.fecha_apertura}";
            };
        }

        private List<PromoCantidad> ContarPromosEnLista(int jornadaId)
        {
            List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();

            VentasJornadaBLL.ObtenerTodo().ForEach(vj =>
            {
                if (vj.promo_id != null && vj.jornada_id == jornadaId)
                    listaPromosCantidad.Add(new PromoCantidad() { Promo = vj.promocione, Cantidad = vj.cantidad });
            });

            return listaPromosCantidad.OrderBy(x => x.Cantidad).Reverse().ToList();
        }

        private List<ProductoCantidad> ContarProductosEnLista(int jornadaId)
        {
            List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();

            VentasJornadaBLL.ObtenerTodo().ForEach(vj =>
            {
                if (vj.producto_id != null && vj.jornada_id == jornadaId)
                    listaProductoCantidad.Add(new ProductoCantidad() { Producto = vj.producto, Cantidad = (int)vj.cantidad, SubTotal = (int)(vj.producto.precio * vj.cantidad), Adicional = vj.cobro_extra, Total = (int)(vj.producto.precio * vj.cantidad  + vj.cobro_extra )});
            });

            return listaProductoCantidad.OrderBy(x => x.Cantidad).Reverse().ToList();
        }

        private void CargarContenido(int jornadaId)
        {
            try
            {
                List<ProductoCantidad> listaProductoCantidad = new List<ProductoCantidad>();
                List<PromoCantidad> listaPromosCantidad = new List<PromoCantidad>();

                listaProductoCantidad = ContarProductosEnLista(jornadaId);
                listaPromosCantidad = ContarPromosEnLista(jornadaId);

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
