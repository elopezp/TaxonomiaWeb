using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv310000 : BmvBase
    {
        private string trimestreActual;
        private string trimestreAnoAnterior;
        private string acumuladoAnoActual;
        private string acumuladoAnoAnterior;

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

        public string TrimestreAnoAnterior
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

        public string AcumuladoAnoActual
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

        public string AcumuladoAnoAnterior
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
