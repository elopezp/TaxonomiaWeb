//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxonomiaWeb.Wcf.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cat_Ano
    {
        public Cat_Ano()
        {
            this.Periodoes = new HashSet<Periodo>();
            this.Taxonomia_Reporte_Detalle = new HashSet<Taxonomia_Reporte_Detalle>();
        }
    
        public int Id_Ano { get; set; }
        public Nullable<int> Ano { get; set; }
    
        public virtual ICollection<Periodo> Periodoes { get; set; }
        public virtual ICollection<Taxonomia_Reporte_Detalle> Taxonomia_Reporte_Detalle { get; set; }
    }
}
