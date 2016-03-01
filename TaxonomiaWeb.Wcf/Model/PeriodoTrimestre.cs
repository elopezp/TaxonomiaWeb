using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class PeriodoTrimestre : INotifyPropertyChanged
    {
        private int idTrimestre;

        public int IdTrimestre
        {

            get { return idTrimestre; }
            set
            {
                if (value != idTrimestre)
                {
                    idTrimestre = value;
                    onPropertyChanged(this, "");
                }
            }
        }

        private string descripcion;

        public string Descripcion
        {

            get { return descripcion; }
            set
            {
                if (value != descripcion)
                {
                    descripcion = value;
                    onPropertyChanged(this, "");
                }
            }
        }

        private int numTrimestre;

        public int NumTrimestre
        {

            get { return numTrimestre; }
            set
            {
                if (value != numTrimestre)
                {
                    numTrimestre = value;
                    onPropertyChanged(this, "");
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