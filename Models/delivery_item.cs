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
    
    public partial class delivery_item
    {
        public int id { get; set; }
        public string direccion { get; set; }
        public string nombre_cliente { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public string comentario { get; set; }
        public string incluye { get; set; }
        public Nullable<System.DateTime> fecha_entrega { get; set; }
        public Nullable<int> boleta_id { get; set; }
    
        public virtual boleta boleta { get; set; }
        public virtual cliente cliente { get; set; }
    }
}
