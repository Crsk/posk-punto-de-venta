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
    
    public partial class boleta_mediopago
    {
        public int id { get; set; }
        public Nullable<int> boleta_id { get; set; }
        public Nullable<int> mediopago_id { get; set; }
        public int ingreso { get; set; }
        public Nullable<int> vendedor_id { get; set; }
    
        public virtual boleta boleta { get; set; }
        public virtual medio_pago medio_pago { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
