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
    
    public partial class cantidadesvendidas_jornada
    {
        public int id { get; set; }
        public Nullable<int> cantidadesvendidas_id { get; set; }
        public Nullable<int> jornada_id { get; set; }
    
        public virtual cantidadesvendida cantidadesvendida { get; set; }
        public virtual jornada jornada { get; set; }
    }
}
