using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class PeriodoAno : INotifyPropertyChanged
    {
        private int idAno;

        public int IdAno
        {

            get { return idAno; }
            set
            {
                if (value != idAno)
                {
                    idAno = value;
                    onPropertyChanged(this, "");
                }
            }
        }

        private string ano;

        public string Ano
        {

            get { return ano; }
            set
            {
                if (value != ano)
                {
                    ano = value;
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