using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv210000 : BmvBase
    {
        private double trimestreActual;
        private double cierreEjercicioAnterior;
        private double inicioEjercicioAnterior;

        [DisplayName("Trimestre Actual")]
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
        [DisplayName("Cierre Ejercicio Anterior")]
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
          [DisplayName("Inicio Ejercicio Anterior")]
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
