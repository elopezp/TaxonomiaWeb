using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv800005 : BmvBase
    {

        private string principalesMarcas;

        public string PrincipalesMarcas
        {
            get { return principalesMarcas; }
            set
            {
                if (value != principalesMarcas)
                {
                    principalesMarcas = value;
                    base.onPropertyChanged(this, "PrincipalesMarcas");
                }
            }
        }

        private string principalesProductosOLineaDeProductos;

        public string PrincipalesProductosOLineaDeProductos
        {
            get { return principalesProductosOLineaDeProductos; }
            set
            {
                if (value != principalesProductosOLineaDeProductos)
                {
                    principalesProductosOLineaDeProductos = value;
                    base.onPropertyChanged(this, "PrincipalesProductosOLineaDeProductos");
                }
            }
        }

        private double ingresosNacionales;

        public double IngresosNacionales
        {
            get { return ingresosNacionales; }
            set
            {
                if (value != ingresosNacionales)
                {
                    ingresosNacionales = value;
                    base.onPropertyChanged(this, "IngresosNacionales");
                }
            }
        }

        private double ingresosPorExportacion;

        public double IngresosPorExportacion
        {
            get { return ingresosPorExportacion; }
            set
            {
                if (value != ingresosPorExportacion)
                {
                    ingresosPorExportacion = value;
                    base.onPropertyChanged(this, "IngresosPorExportacion");
                }
            }
        }

        private double ingresosDeSubsidiariasEnElExtranjero;

        public double IngresosDeSubsidiariasEnElExtranjero
        {
            get { return ingresosDeSubsidiariasEnElExtranjero; }
            set
            {
                if (value != ingresosDeSubsidiariasEnElExtranjero)
                {
                    ingresosDeSubsidiariasEnElExtranjero = value;
                    base.onPropertyChanged(this, "IngresosDeSubsidiariasEnElExtranjero");
                }
            }
        }

        private double ingresosTotales;

        public double IngresosTotales
        {
            get { return ingresosTotales; }
            set
            {
                if (value != ingresosTotales)
                {
                    ingresosTotales = value;
                    base.onPropertyChanged(this, "IngresosTotales");
                }
            }
        }

    }
}
