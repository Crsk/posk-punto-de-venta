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
    
    public partial class deuda
    {
        public int id { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<bool> saldada { get; set; }
        public Nullable<int> producto_id { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public string comentario { get; set; }
    
        public virtual cliente cliente { get; set; }
        public virtual producto producto { get; set; }
    }
}
