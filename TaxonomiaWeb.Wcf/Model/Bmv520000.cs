using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv520000 : BmvBase
    {
        private double acumuladoAnoActual;
        private double acumuladoAnoAnterior;

        public double AcumuladoAnoActual
        {
            get { return acumuladoAnoActual; }
            set
            {
                if (value != acumuladoAnoActual)
                {
                    acumuladoAnoActual = value;
                    base.onPropertyChanged(this, "AcumuladoAnoActual");
                }
            }
        }

        public double AcumuladoAnoAnterior
        {
            get { return acumuladoAnoAnterior; }
            set
            {
                if (value != acumuladoAnoAnterior)
                {
                    acumuladoAnoAnterior = value;
                    base.onPropertyChanged(this, "AcumuladoAnoAnterior");
                }
            }
        }


    }
}
