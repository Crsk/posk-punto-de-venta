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
    
    public partial class datos_negocio
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string mensaje { get; set; }
        public string direccion { get; set; }
        public string logo { get; set; }
        public Nullable<int> iva { get; set; }
        public string inicio_jornada_dia { get; set; }
        public string termino_jornada_dia { get; set; }
        public Nullable<System.TimeSpan> inicio_jornada_hora { get; set; }
        public Nullable<System.TimeSpan> termino_jornada_hora { get; set; }
        public string modo { get; set; }
        public string correo_primario { get; set; }
        public string correo_secundario { get; set; }
        public Nullable<bool> teclado_tactil_integrado { get; set; }
    }
}
