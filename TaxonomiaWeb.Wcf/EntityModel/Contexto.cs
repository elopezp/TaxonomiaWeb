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
    
    public partial class Contexto
    {
        public Contexto()
        {
            this.Taxonomia_Reporte_Detalle = new HashSet<Taxonomia_Reporte_Detalle>();
        }
    
        public int Id_Contexto { get; set; }
        public Nullable<int> Id_Periodo { get; set; }
        public string Descripcion { get; set; }
    
        public virtual Periodo Periodo { get; set; }
        public virtual ICollection<Taxonomia_Reporte_Detalle> Taxonomia_Reporte_Detalle { get; set; }
    }
}
