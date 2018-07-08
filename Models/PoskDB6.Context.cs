﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace posk.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PoskDB6 : DbContext
    {
        public PoskDB6()
            : base("name=PoskDB6")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bodega_movimiento> bodega_movimiento { get; set; }
        public virtual DbSet<bodega_stock> bodega_stock { get; set; }
        public virtual DbSet<cantidadesvendida> cantidadesvendidas { get; set; }
        public virtual DbSet<cantidadesvendidas_jornada> cantidadesvendidas_jornada { get; set; }
        public virtual DbSet<categoria_sector> categoria_sector { get; set; }
        public virtual DbSet<categoria_subcategoria> categoria_subcategoria { get; set; }
        public virtual DbSet<categoria> categorias { get; set; }
        public virtual DbSet<compra> compras { get; set; }
        public virtual DbSet<config> configs { get; set; }
        public virtual DbSet<detalle_boleta> detalle_boleta { get; set; }
        public virtual DbSet<deuda> deudas { get; set; }
        public virtual DbSet<devolucion> devolucions { get; set; }
        public virtual DbSet<fiado> fiados { get; set; }
        public virtual DbSet<gasto> gastos { get; set; }
        public virtual DbSet<ingreso> ingresos { get; set; }
        public virtual DbSet<jornada> jornadas { get; set; }
        public virtual DbSet<lineas_fiado> lineas_fiado { get; set; }
        public virtual DbSet<materiasprima> materiasprimas { get; set; }
        public virtual DbSet<merma> mermas { get; set; }
        public virtual DbSet<mesa> mesas { get; set; }
        public virtual DbSet<pedidos_agregados> pedidos_agregados { get; set; }
        public virtual DbSet<pedidos_preparaciones> pedidos_preparaciones { get; set; }
        public virtual DbSet<pendiente> pendientes { get; set; }
        public virtual DbSet<prestamo_envases> prestamo_envases { get; set; }
        public virtual DbSet<producto_compra> producto_compra { get; set; }
        public virtual DbSet<producto_materiaprima> producto_materiaprima { get; set; }
        public virtual DbSet<producto_preparacion> producto_preparacion { get; set; }
        public virtual DbSet<producto_productocomplejo> producto_productocomplejo { get; set; }
        public virtual DbSet<producto_promocion> producto_promocion { get; set; }
        public virtual DbSet<productoscomplejo> productoscomplejos { get; set; }
        public virtual DbSet<promocione> promociones { get; set; }
        public virtual DbSet<proveedore> proveedores { get; set; }
        public virtual DbSet<punto> puntos { get; set; }
        public virtual DbSet<registro> registros { get; set; }
        public virtual DbSet<reserva> reservas { get; set; }
        public virtual DbSet<sectormesa> sectormesas { get; set; }
        public virtual DbSet<stock_mp> stock_mp { get; set; }
        public virtual DbSet<stock_pr> stock_pr { get; set; }
        public virtual DbSet<subcategoria> subcategorias { get; set; }
        public virtual DbSet<sync> syncs { get; set; }
        public virtual DbSet<unidades_medida> unidades_medida { get; set; }
        public virtual DbSet<usuario> usuarios { get; set; }
        public virtual DbSet<datos_negocio> datos_negocio { get; set; }
        public virtual DbSet<sectore> sectores { get; set; }
        public virtual DbSet<sector_impresion> sector_impresion { get; set; }
        public virtual DbSet<tipo_itemventa> tipo_itemventa { get; set; }
        public virtual DbSet<preparacione> preparaciones { get; set; }
        public virtual DbSet<ventas_jornada> ventas_jornada { get; set; }
        public virtual DbSet<pedidos_productos> pedidos_productos { get; set; }
        public virtual DbSet<pedido> pedidos { get; set; }
        public virtual DbSet<medio_pago> medio_pago { get; set; }
        public virtual DbSet<salsa> salsas { get; set; }
        public virtual DbSet<boleta> boletas { get; set; }
        public virtual DbSet<boleta_mediopago> boleta_mediopago { get; set; }
        public virtual DbSet<agregado> agregados { get; set; }
        public virtual DbSet<producto> productos { get; set; }
        public virtual DbSet<cliente> clientes { get; set; }
        public virtual DbSet<delivery_item> delivery_item { get; set; }
        public virtual DbSet<envoltura> envolturas { get; set; }
        public virtual DbSet<ingrediente> ingredientes { get; set; }
        public virtual DbSet<opcionale> opcionales { get; set; }
        public virtual DbSet<opcionales_ingredientes> opcionales_ingredientes { get; set; }
        public virtual DbSet<tipo_producto> tipo_producto { get; set; }
        public virtual DbSet<tipo_producto_opcionales> tipo_producto_opcionales { get; set; }
    }
}
