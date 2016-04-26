using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxonomiaWeb.Model
{
    public class ReporteDetalle
    {
        private int? idReporte;
        private int? idReporteDetalle;
        private string valor;
        private int? identificadorFila;
        private bool estado;

        public int? IdReporte
        {
            get { return idReporte; }
            set
            {
                if (value != idReporte)
                {
                    idReporte = value;
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
                }
            }
        }


        public bool Estado
        {
            get { return estado; }
            set
            {
                if (value != estado)
                {
                    estado = value;
                }
            }
        }
    }
}