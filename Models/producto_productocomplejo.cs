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
    
    public partial class producto_productocomplejo
    {
        public int id { get; set; }
        public Nullable<int> productocomplejo_id { get; set; }
        public Nullable<int> producto_id { get; set; }
        public Nullable<decimal> cantidad { get; set; }
    
        public virtual productoscomplejo productoscomplejo { get; set; }
        public virtual producto producto { get; set; }
    }
}
