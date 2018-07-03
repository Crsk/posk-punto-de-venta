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
    
    public partial class boleta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public boleta()
        {
            this.detalle_boleta = new HashSet<detalle_boleta>();
            this.boleta_mediopago = new HashSet<boleta_mediopago>();
            this.delivery_item = new HashSet<delivery_item>();
        }
    
        public int id { get; set; }
        public Nullable<int> numero_boleta { get; set; }
        public System.DateTime fecha { get; set; }
        public Nullable<int> puntos_cantidad { get; set; }
        public Nullable<int> total { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public Nullable<int> usuario_id { get; set; }
        public int propina { get; set; }
    
        public virtual usuario usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_boleta> detalle_boleta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<boleta_mediopago> boleta_mediopago { get; set; }
        public virtual cliente cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<delivery_item> delivery_item { get; set; }
    }
}
