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
    
    public partial class agregado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public agregado()
        {
            this.pedidos_agregados = new HashSet<pedidos_agregados>();
            this.pedidos_agregados1 = new HashSet<pedidos_agregados>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public Nullable<int> cobro_extra { get; set; }
        public int font_size { get; set; }
        public Nullable<bool> para_handroll { get; set; }
        public bool es_vegetal { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidos_agregados> pedidos_agregados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedidos_agregados> pedidos_agregados1 { get; set; }
    }
}
