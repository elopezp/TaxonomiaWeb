using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class BmvDetalleSuma : INotifyPropertyChanged
    {
        private string contenido;
        private int idTaxonomiaPadre;
        private int idTaxonomiaHijo;


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

        public int IdTaxonomiaPadre
        {
            get { return idTaxonomiaPadre; }
            set
            {
                if (idTaxonomiaPadre != value)
                {
                    idTaxonomiaPadre = value;
                    onPropertyChanged(this, "IdTaxonomiaPadre");
                }
            }
        }


        public int IdTaxonomiaHijo
        {
            get { return idTaxonomiaHijo; }
            set
            {
                if (idTaxonomiaHijo != value)
                {
                    idTaxonomiaHijo = value;
                    onPropertyChanged(this, "IdTaxonomiaHijo");
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