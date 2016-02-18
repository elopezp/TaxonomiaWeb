using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv700003 : BmvBase
    {
        private double anoActual;
        private double anoAnterior;


        public double AnoActual
        {
            get { return anoActual; }
            set
            {
                if (value != anoActual)
                {
                    anoActual = value;
                    base.onPropertyChanged(this, "AnoActual");
                }
            }
        }

        public double AnoAnterior
        {
            get { return anoAnterior; }
            set
            {
                if (value != anoAnterior)
                {
                    anoAnterior = value;
                    base.onPropertyChanged(this, "AnoAnterior");
                }
            }
        }


    }
}
