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
    
    public partial class detalle_boleta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public detalle_boleta()
        {
            this.ventas_jornada = new HashSet<ventas_jornada>();
        }
    
        public int id { get; set; }
        public Nullable<int> monto { get; set; }
        public decimal cantidad { get; set; }
        public decimal descuento { get; set; }
        public Nullable<int> producto_id { get; set; }
        public int boleta_id { get; set; }
        public Nullable<int> promocion_id { get; set; }
    
        public virtual boleta boleta { get; set; }
        public virtual producto producto { get; set; }
        public virtual promocione promocione { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ventas_jornada> ventas_jornada { get; set; }
    }
}
