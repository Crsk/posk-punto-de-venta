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
    
    public partial class ventas_jornada
    {
        public int id { get; set; }
        public Nullable<int> producto_id { get; set; }
        public Nullable<int> promo_id { get; set; }
        public Nullable<int> jornada_id { get; set; }
        public Nullable<decimal> cantidad { get; set; }
        public int cobro_extra { get; set; }
    
        public virtual jornada jornada { get; set; }
        public virtual promocione promocione { get; set; }
        public virtual producto producto { get; set; }
    }
}
