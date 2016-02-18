using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class BmvBase : INotifyPropertyChanged
    {
        private int? idReporte;
        private int? idReporteDetalle;
        private int idTaxonomiaDetalle;
        private string contenido;
        private string formatoCampo;
        private string descripcion;
        private int orden;
        private string atributoColumna;
        private string valor;
        private bool lectura;
        private bool campoDinamico;
        private int? identificadorFila;
        private int? idAxisPadre;

        public int? IdReporte
        {
            get { return idReporte; }
            set
            {
                if (value != idReporte)
                {
                    idReporte = value;
                    onPropertyChanged(this, "IdReporte");
                }
            }
        }

        public int? IdReporteDetalle
        {
            get { return idReporteDetalle; }
            set
            {
                if (value != idReporteDetalle)
                {
                    idReporteDetalle = value;
                    onPropertyChanged(this, "IdReporteDetalle");
                }
            }
        }

        public int IdTaxonomiaDetalle
        {
            get { return idTaxonomiaDetalle; }
            set
            {
                if (value != idTaxonomiaDetalle)
                {
                    idTaxonomiaDetalle = value;
                    onPropertyChanged(this, "IdTaxonomiaDetalle");
                }
            }
        }

        public string Contenido
        {
            get { return contenido; }
            set
            {
                if (value != contenido)
                {
                    contenido = value;
                    onPropertyChanged(this, "Contenido");
                }
            }
        }

        public string FormatoCampo
        {
            get { return formatoCampo; }
            set
            {
                if (value != formatoCampo)
                {
                    formatoCampo = value;
                    onPropertyChanged(this, "FormatoCampo");
                }
            }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                if (value != descripcion)
                {
                    descripcion = value;
                    onPropertyChanged(this, "Descripcion");
                }
            }
        }

        public int Orden
        {
            get { return orden; }
            set
            {
                if (orden != value)
                {
                    orden = value;
                    onPropertyChanged(this, "Orden");
                }
            }
        }

        public string AtributoColumna
        {
            get { return atributoColumna; }
            set
            {
                if (value != atributoColumna)
                {
                    atributoColumna = value;
                    onPropertyChanged(this, "AtributoColumna");
                }
            }
        }

        public string Valor
        {
            get { return valor; }
            set
            {
                if (value != valor)
                {
                    valor = value;
                    onPropertyChanged(this, "Valor");
                }
            }
        }

        public bool Lectura
        {
            get { return lectura; }
            set
            {
                if (value != lectura)
                {
                    lectura = value;
                    onPropertyChanged(this, "Lectura");
                }
            }
        }

        public bool CampoDinamico
        {
            get { return campoDinamico; }
            set
            {
                if (value != campoDinamico)
                {
                    campoDinamico = value;
                    onPropertyChanged(this, "CampoDinamico");
                }
            }
        }


        public int? IdentificadorFila
        {
            get { return identificadorFila; }
            set
            {
                if (value != identificadorFila)
                {
                    identificadorFila = value;
                    onPropertyChanged(this, "IdentificadorFila");
                }
            }
        }

        //Propiedad que sirve para detectar si es un miembro como campo. Como en el caso del 610000.
        public int? IdAxisPadre
        {
            get { return idAxisPadre; }
            set
            {
                if (value != idAxisPadre)
                {
                    idAxisPadre = value;
                    onPropertyChanged(this, "IdAxisPadre");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
