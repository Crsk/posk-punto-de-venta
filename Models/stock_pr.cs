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
    
    public partial class stock_pr
    {
        public int id { get; set; }
        public Nullable<int> producto_id { get; set; }
        public decimal entrada { get; set; }
        public decimal salida { get; set; }
        public decimal ajuste { get; set; }
    
        public virtual producto producto { get; set; }
    }
}
