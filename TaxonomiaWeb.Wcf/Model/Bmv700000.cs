using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv700000 : BmvBase
    {
        private double trimestreActual;
        private double cierreEjercicioAnterior;
        private double inicioEjercicioAnterior;

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

        public double CierreEjercicioAnterior
        {
            get { return cierreEjercicioAnterior; }
            set
            {
                if (value != cierreEjercicioAnterior)
                {
                    cierreEjercicioAnterior = value;
                    base.onPropertyChanged(this, "CierreEjercicioAnterior");
                }
            }
        }
        public double InicioEjercicioAnterior
        {
            get { return inicioEjercicioAnterior; }
            set
            {
                if (value != inicioEjercicioAnterior)
                {
                    inicioEjercicioAnterior = value;
                    base.onPropertyChanged(this, "InicioEjercicioAnterior");
                }
            }
        }


    }
}
