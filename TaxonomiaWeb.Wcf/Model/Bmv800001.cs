using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv800001 : BmvBase
    {
        private string institucion;

        public string Institucion
        {
            get { return institucion; }
            set
            {
                if (value != institucion)
                {
                    institucion = value;
                    base.onPropertyChanged(this, "Institucion");
                } 
            }
        }

        private bool institucionExtranjera;

        public bool InstitucionExtranjera
        {
            get { return institucionExtranjera; }
            set
            {
                if (value != institucionExtranjera)
                {
                    institucionExtranjera = value;
                    base.onPropertyChanged(this, "InstitucionExtranjera");
                }
            }
        }

        private string fechaDeFirmaContrato;

        public string FechaDeFirmaContrato
        {
            get { return fechaDeFirmaContrato; }
            set
            {
                if (value != fechaDeFirmaContrato)
                {
                    fechaDeFirmaContrato = value;
                    base.onPropertyChanged(this, "FechaDeFirmaContrato");
                }
            }
        }

        private string fechaDeVencimiento;

        public string FechaDeVencimiento
        {
            get { return fechaDeVencimiento; }
            set
            {
                if (value != fechaDeVencimiento)
                {
                    fechaDeVencimiento = value;
                    base.onPropertyChanged(this, "FechaDeVencimiento");
                }
            }
        }

        private double tasaDeInteresYOSobreTasa;

        public double TasaDeInteresYOSobreTasa
        {
            get { return tasaDeInteresYOSobreTasa; }
            set
            {
                if (value != tasaDeInteresYOSobreTasa)
                {
                    tasaDeInteresYOSobreTasa = value;
                    base.onPropertyChanged(this, "TasaDeInteresYOSobreTasa");
                }
            }
        }

        private double monedaNacionalAnoActual;

        public double MonedaNacionalAnoActual
        {
            get { return monedaNacionalAnoActual; }
            set
            {
                if (value != monedaNacionalAnoActual)
                {
                    monedaNacionalAnoActual = value;
                    base.onPropertyChanged(this, "MonedaNacionalAnoActual");
                }
            }
        }

        private double monedaNacionalHasta1Ano;

        public double MonedaNacionalHasta1Ano
        {
            get { return monedaNacionalHasta1Ano; }
            set
            {
                if (value != monedaNacionalHasta1Ano)
                {
                    monedaNacionalHasta1Ano = value;
                    base.onPropertyChanged(this, "MonedaNacionalHasta1Ano");
                }
            }
        }

        private double monedaNacionalHasta2Anos;

        public double MonedaNacionalHasta2Anos
        {
            get { return monedaNacionalHasta2Anos; }
            set
            {
                if (value != monedaNacionalHasta2Anos)
                {
                    monedaNacionalHasta2Anos = value;
                    base.onPropertyChanged(this, "MonedaNacionalHasta2Anos");
                }
            }
        }

        private double monedaNacionalHasta3Anos;

        public double MonedaNacionalHasta3Anos
        {
            get { return monedaNacionalHasta3Anos; }
            set
            {
                if (value != monedaNacionalHasta3Anos)
                {
                    monedaNacionalHasta3Anos = value;
                    base.onPropertyChanged(this, "MonedaNacionalHasta3Anos");
                }
            }
        }

        private double monedaNacionalHasta4Anos;

        public double MonedaNacionalHasta4Anos
        {
            get { return monedaNacionalHasta4Anos; }
            set
            {
                if (value != monedaNacionalHasta4Anos)
                {
                    monedaNacionalHasta4Anos = value;
                    base.onPropertyChanged(this, "MonedaNacionalHasta4Anos");
                }
            }
        }

        private double monedaNacionalHasta5AnosOMas;

        public double MonedaNacionalHasta5AnosOMas
        {
            get { return monedaNacionalHasta5AnosOMas; }
            set
            {
                if (value != monedaNacionalHasta5AnosOMas)
                {
                    monedaNacionalHasta5AnosOMas = value;
                    base.onPropertyChanged(this, "MonedaNacionalHasta5AnosOMas");
                }
            }
        }

        private double monedaExtranjeraAnoActual;

        public double MonedaExtranjeraAnoActual
        {
            get { return monedaExtranjeraAnoActual; }
            set
            {
                if (value != monedaExtranjeraAnoActual)
                {
                    monedaExtranjeraAnoActual = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraAnoActual");
                }
            }
        }

        private double monedaExtranjeraHasta1Ano;

        public double MonedaExtranjeraHasta1Ano
        {
            get { return monedaExtranjeraHasta1Ano; }
            set
            {
                if (value != monedaExtranjeraHasta1Ano)
                {
                    monedaExtranjeraHasta1Ano = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraHasta1Ano");
                }
            }
        }

        private double monedaExtranjeraHasta2Anos;

        public double MonedaExtranjeraHasta2Anos
        {
            get { return monedaExtranjeraHasta2Anos; }
            set
            {
                if (value != monedaExtranjeraHasta2Anos)
                {
                    monedaExtranjeraHasta2Anos = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraHasta2Anos");
                }
            }
        }

        private double monedaExtranjeraHasta3Anos;

        public double MonedaExtranjeraHasta3Anos
        {
            get { return monedaExtranjeraHasta3Anos; }
            set
            {
                if (value != monedaExtranjeraHasta3Anos)
                {
                    monedaExtranjeraHasta3Anos = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraHasta3Anos");
                }
            }
        }

        private double monedaExtranjeraHasta4Anos;

        public double MonedaExtranjeraHasta4Anos
        {
            get { return monedaExtranjeraHasta4Anos; }
            set
            {
                if (value != monedaExtranjeraHasta4Anos)
                {
                    monedaExtranjeraHasta4Anos = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraHasta4Anos");
                }
            }
        }

        private double monedaExtranjeraHasta5AnosOMas;

        public double MonedaExtranjeraHasta5AnosOMas
        {
            get { return monedaExtranjeraHasta5AnosOMas; }
            set
            {
                if (value != monedaExtranjeraHasta5AnosOMas)
                {
                    monedaExtranjeraHasta5AnosOMas = value;
                    base.onPropertyChanged(this, "MonedaExtranjeraHasta5AnosOMas");
                }
            }
        }


    }
}
