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
    
    public partial class Taxonomia_Reporte
    {
        public Taxonomia_Reporte()
        {
            this.Taxonomia_Reporte_Detalle = new HashSet<Taxonomia_Reporte_Detalle>();
        }
    
        public int Id_Taxonomia_Reporte { get; set; }
        public Nullable<int> Id_Taxonomia_Detalle { get; set; }
        public Nullable<int> Id_Taxonomia_Columna { get; set; }
        public Nullable<int> Id_Contenido { get; set; }
        public Nullable<int> Id_Validacion_Contexto { get; set; }
    
        public virtual ICollection<Taxonomia_Reporte_Detalle> Taxonomia_Reporte_Detalle { get; set; }
        public virtual Cat_Taxonomia_Columna Cat_Taxonomia_Columna { get; set; }
        public virtual Cat_Contenido Cat_Contenido { get; set; }
        public virtual Cat_Taxonomia_Detalle Cat_Taxonomia_Detalle { get; set; }
        public virtual Cat_Validacion_Contexto Cat_Validacion_Contexto { get; set; }
    }
}
