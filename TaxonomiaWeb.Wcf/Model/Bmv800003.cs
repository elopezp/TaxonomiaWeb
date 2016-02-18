using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv800003 : BmvBase
    {
        private double dolares;

        public double Dolares
        {
            get { return dolares; }
            set
            {
                if (value != dolares)
                {
                    dolares = value;
                    base.onPropertyChanged(this, "Dolares");
                }
            }
        }

        private double dolaresContravalorPesos;

        public double DolaresContravalorPesos
        {
            get { return dolaresContravalorPesos; }
            set
            {
                if (value != dolaresContravalorPesos)
                {
                    dolaresContravalorPesos = value;
                    base.onPropertyChanged(this, "DolaresContravalorPesos");
                }
            }
        }

        private double otrasMonedasContravalorDolares;

        public double OtrasMonedasContravalorDolares
        {
            get { return otrasMonedasContravalorDolares; }
            set
            {
                if (value != otrasMonedasContravalorDolares)
                {
                    otrasMonedasContravalorDolares = value;
                    base.onPropertyChanged(this, "OtrasMonedasContravalorDolares");
                }
            }
        }

        private double otrasMonedasContravalorPesos;

        public double OtrasMonedasContravalorPesos
        {
            get { return otrasMonedasContravalorPesos; }
            set
            {
                if (value != otrasMonedasContravalorPesos)
                {
                    otrasMonedasContravalorPesos = value;
                    base.onPropertyChanged(this, "OtrasMonedasContravalorPesos");
                }
            }
        }

        private double totalDePesos;

        public double TotalDePesos
        {
            get { return totalDePesos; }
            set
            {
                if (value != totalDePesos)
                {
                    totalDePesos = value;
                    base.onPropertyChanged(this, "TotalDePesos");
                }
            }
        }


    }
}
