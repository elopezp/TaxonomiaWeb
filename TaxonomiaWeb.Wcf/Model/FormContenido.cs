using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class FormContenido
    {
        public int Id { get; set; }
        [Display(Name = "Customer ID", ShortName = "Customer ID")]
        public string Contenido { get; set; }
        public string Descripcion { get; set; }
    }
}