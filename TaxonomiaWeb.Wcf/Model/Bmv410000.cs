using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv410000 : BmvBase
    {
        private double trimestreActual;
        private double trimestreAnoAnterior;
        private double acumuladoAnoActual;
        private double acumuladoAnoAnterior;

        public double TrimestreActual
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

        public double TrimestreAnoAnterior
        {
            get { return trimestreAnoAnterior; }
            set
            {
                if (value != trimestreAnoAnterior)
                {
                    trimestreAnoAnterior = value;
                    base.onPropertyChanged(this, "TrimestreAnoAnterior");
                }
            }
        }

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
