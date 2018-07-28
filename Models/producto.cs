//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            this.bodega_movimiento = new HashSet<bodega_movimiento>();
            this.bodega_stock = new HashSet<bodega_stock>();
            this.detalle_boleta = new HashSet<detalle_boleta>();
            this.deudas = new HashSet<deuda>();
            this.devolucions = new HashSet<devolucion>();
            this.envolturas = new HashSet<envoltura>();
            this.lineas_fiado = new HashSet<lineas_fiado>();
            this.mermas = new HashSet<merma>();
            this.pedidos_productos = new HashSet<pedidos_productos>();
            this.pendientes = new HashSet<pendiente>();
            this.producto_compra = new HashSet<producto_compra>();
            this.producto_materiaprima = new HashSet<producto_materiaprima>();
            this.producto_preparacion = new HashSet<producto_preparacion>();
            this.producto_productocomplejo = new HashSet<producto_productocomplejo>();
            this.producto_promocion = new HashSet<producto_promocion>();
            this.productoscomplejos = new HashSet<productoscomplejo>();
            this.stock_pr = new HashSet<stock_pr>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string codigo_barras { get; set; }
        public int precio { get; set; }
        public Nullable<bool> retornable { get; set; }
        public Nullable<bool> favorito { get; set; }
        public Nullable<int> puntos_cantidad { get; set; }
        public string imagen { get; set; }
        public Nullable<int> sub_categoria_id { get; set; }
        public Nullable<bool> solo_venta { get; set; }
        public Nullable<bool> solo_compra { get; set; }
        public Nullable<int> sector_impresion_id { get; set; }
        public int z_index { get; set; }
        public Nullable<int> tipo_producto_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bodega_movimiento> bodega_movimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bodega_stock> bodega_stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_boleta> detalle_boleta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deuda> deudas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<devolucion> devolucions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<envoltura> envolturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<lineas_fiado> lineas_fiado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<merma> mermas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidos_productos> pedidos_productos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pendiente> pendientes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_compra> producto_compra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_materiaprima> producto_materiaprima { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_preparacion> producto_preparacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_productocomplejo> producto_productocomplejo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<producto_promocion> producto_promocion { get; set; }
        public virtual subcategoria subcategoria { get; set; }
        public virtual tipo_producto tipo_producto { get; set; }
        public virtual sector_impresion sector_impresion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productoscomplejo> productoscomplejos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stock_pr> stock_pr { get; set; }
    }
}
