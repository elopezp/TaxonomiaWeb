using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class Bmv813000 : BmvBase
    {
        private string trimestreActual;

        public string TrimestreActual
        {
            
            get { return trimestreActual; }
            set
            {
                if (value != trimestreActual)
                {
                    trimestreActual = value;
                    base.onPropertyChanged(this, "TrimestreActual");
                }
            }
        }

    }
}