using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaxonomiaWeb.Model;
using TaxonomiaWeb.Wcf.EntityModel;
using TaxonomiaWeb.Wcf.Util;

namespace TaxonomiaWeb.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        private BmvXblrEntities context;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public int GenerarContextos(int numTrimestre, int idAno)
        {
            int cantidadContextosGenerados = 0;
            List<Bmv105000> list = new List<Bmv105000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var listaTaxonomiaReporteDetalleAntesDeContextos = from trd in context.Taxonomia_Reporte_Detalle
                                 join tr in context.Taxonomia_Reporte on trd.Id_Taxonomia_Reporte equals tr.Id_Taxonomia_Reporte
                                 join ct in context.Cat_Trimestre on trd.Id_Trimestre equals ct.Id_Trimestre
                                 join ca in context.Cat_Ano on trd.Id_Ano equals ca.Id_Ano
                                 join c in context.Cat_Contenido on tr.Id_Contenido equals c.Id_Contenido
                                 join td in context.Cat_Taxonomia_Detalle on tr.Id_Taxonomia_Detalle equals td.Id_Taxonomia_Detalle
                                 join f in context.Cat_Tipo_Formato on td.Id_Tipo_Formato equals f.Id_Tipo_Formato
                                 join oe in context.Cat_Origen_Elemento on td.Id_Origen_Elemento equals oe.Id_Origen_Elemento
                                 join tc in context.Cat_Taxonomia_Columna on tr.Id_Taxonomia_Columna equals tc.Id_Taxonomia_Columna
                                 join vc in
                                     (from sub in context.Cat_Validacion_Contexto
                                      join p in context.Periodoes on sub.Id_Validacion_Contexto equals p.Id_Validacion_Contexto
                                      join trim in context.Cat_Trimestre on p.Id_Trimestre equals trim.Id_Trimestre
                                      where trim.Trimestre == numTrimestre && p.Id_Ano == idAno
                                      select new {Fecha_Inicio = p.Fecha_Inicio,
                                                  Fecha_Fin = p.Fecha_Fin,
                                                  Id_Validacion_Contexto = sub.Id_Validacion_Contexto,
                                                  Descripcion_Periodo = sub.Descripcion_Perido
                                      }) on tc.Id_Validacion_Contexto equals vc.Id_Validacion_Contexto
                                 join mc in context.Cat_Modelo_Clase on tc.Id_Modelo_Clase equals mc.Id_Modelo_Clase
                                 join taxdimension in context.Cat_Taxonomia_Detalle on tc.Id_Taxonomia_Detalle equals taxdimension.Id_Taxonomia_Detalle into taxdimension_group
                                 from taxdimension in taxdimension_group.DefaultIfEmpty()
                                 join taxdimensionformato in context.Cat_Tipo_Formato on taxdimension.Id_Tipo_Formato equals taxdimensionformato.Id_Tipo_Formato into taxdimensionformato_group
                                 join taxAxisPadreX in context.Cat_Taxonomia_Detalle on mc.Id_Taxonomia_Detalle_Axis_X equals taxAxisPadreX.Id_Taxonomia_Detalle into taxAxisPadreX_group
                                 from taxAxisPadreX in taxAxisPadreX_group.DefaultIfEmpty()
                                 join taxAxisPadreCabeceraX in context.Cat_Taxonomia_Detalle on taxAxisPadreX.Id_Axis_Padre equals taxAxisPadreCabeceraX.Id_Taxonomia_Detalle into taxAxisPadreCabeceraX_group
                                 join taxAxisPadreY in context.Cat_Taxonomia_Detalle on mc.Id_Taxonomia_Detalle_Axis_Y equals taxAxisPadreY.Id_Taxonomia_Detalle into taxAxisPadreY_group
                                 from taxAxisPadreY in taxAxisPadreY_group.DefaultIfEmpty()
                                 join taxAxisPadreCabeceraY in context.Cat_Taxonomia_Detalle on taxAxisPadreY.Id_Axis_Padre equals taxAxisPadreCabeceraY.Id_Taxonomia_Detalle into taxAxisPadreCabeceraY_group
                                 from taxdimensionformato in taxdimensionformato_group.DefaultIfEmpty()
                                 from taxAxisPadreCabeceraX in taxAxisPadreCabeceraX_group.DefaultIfEmpty()
                                 from taxAxisPadreCabeceraY in taxAxisPadreCabeceraY_group.DefaultIfEmpty()
                                 where ct.Id_Trimestre == numTrimestre && trd.Id_Ano == idAno
                                 orderby c.Contenido ascending
                                 select new
                                 {
                                     Ano = ca.Ano,
                                     Trimestre = ct.Trimestre,
                                     Contenido = c.Contenido,
                                     Descripcion = td.Descripcion,
                                     Formato_Campo = f.Formato,
                                     Nombre_Esquema = td.Nombre_esquema,
                                     Prefijo = oe.Prefijo,
                                     Valor = trd.Valor,
                                     Atributo = mc.Atributo,
                                     Identificador_Fila = (int?)trd.Identificador_Fila,
                                     Descripcion_Periodo = vc.Descripcion_Periodo,
                                     Fecha_Inicio = vc.Fecha_Inicio,
                                     Fecha_Fin = vc.Fecha_Fin,
                                     EsquemaX = taxAxisPadreX.Nombre_esquema,
                                     EsquemaCabeceraX = taxAxisPadreCabeceraX.Nombre_esquema,
                                     EsquemaY = taxAxisPadreY.Nombre_esquema,
                                     EsquemaCabeceraY = taxAxisPadreCabeceraY.Nombre_esquema
                                 };

                    if (listaTaxonomiaReporteDetalleAntesDeContextos != null && listaTaxonomiaReporteDetalleAntesDeContextos.Any() == true)
                    {
                        cantidadContextosGenerados = listaTaxonomiaReporteDetalleAntesDeContextos.Count();

                        foreach (var trdAntesDeContexto in listaTaxonomiaReporteDetalleAntesDeContextos)
                        {
                            string contenido = trdAntesDeContexto.Contenido;
                            switch (contenido)
                            {
                                case "105000":
                                    contenido = trdAntesDeContexto.Contenido;
                                    break;
                                case "110000":
                                    contenido = trdAntesDeContexto.Contenido;
                                    break;
                                case "210000":
                                    break;
                                case "310000":
                                    break;
                                case "410000":
                                    break;
                                case "520000":
                                    break;
                                case "610000":
                                    break;
                                case "700000":
                                    break;
                                case "700002":
                                    break;
                                case "700003":
                                    break;
                                case "800001":
                                    break;
                                case "800003":
                                    break;
                                case "800005":
                                    break;
                                case "800007":
                                    break;
                                case "800100":
                                    break;
                                case "800200":
                                    break;
                                case "800500":
                                    break;
                                case "800600":
                                    break;
                                case "813000":
                                    break;
                                case "610000 Anterior":
                                    break;
                                default:
                                    break;
                            }

                               

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }


            return cantidadContextosGenerados;
        }

        //public bool GetPeriodoContextos(List<ReporteDetalle> listBmv, string emisora, int periodo, int trimestre)
        //{
        //Conseguimos el periodo del contenido (instant or duration)
        //var taxDet = from u in context.BMV_Validacion_Contextos
        //             join vcc in context.BMV_Validacion_Contextos_Contenido on u.Id equals vcc.Id_Validacion_Contextos
        //             join vc in context.BMV_Contenido on vcc.Id_Contenido equals vc.Id_Contenido
        //             where vc.Contenido.Equals("105000")
        //             select u;
        //if (taxDet != null)
        //{
        //    foreach (var item in taxDet)
        //    {
        //        int idValidacionContexto = item.Id;
        //        int trimestreInvalido = -1;
        //        trimestreInvalido = item.Periodo_No_Requerido.HasValue ? item.Periodo_No_Requerido.Value : -1;
        //        if (trimestreInvalido > 0 && trimestreInvalido != trimestre)
        //        {
        //            int anoInicio = item.X.HasValue ? (periodo - item.X.Value) : 0;
        //            if (anoInicio > 0 && (trimestre >= 1 && trimestre <= 4))
        //            {
        //                List<DateTime> listaInicioPeriodo = getListaInicioPeriodo(anoInicio);
        //                DateTime fechaInicio;
        //                DateTime fechaFin;
        //                List<DateTime> listaFinPeriodo = null;
        //                if (item.Y.HasValue)
        //                {
        //                    listaFinPeriodo = getListaFinPeriodo(periodo - item.Y.Value);
        //                }

        //                switch (idValidacionContexto)
        //                {
        //                    case 1:
        //                    case 13:
        //                    case 16:
        //                    case 19:
        //                        //Periodo
        //                        if (listaFinPeriodo != null)
        //                        {
        //                            fechaInicio = listaInicioPeriodo.ElementAt(trimestre - 1);
        //                            fechaFin = listaFinPeriodo.ElementAt(trimestre - 1);
        //                        }
        //                        //Instante
        //                        else
        //                        {
        //                            fechaInicio = listaInicioPeriodo.ElementAt(trimestre - 1);
        //                        }
        //                        break;

        //                    case 5:
        //                    case 7:
        //                    case 10:
        //                    case 11:
        //                    case 15:
        //                    case 18:
        //                        //Periodo
        //                        if (listaFinPeriodo != null)
        //                        {
        //                            fechaInicio = listaInicioPeriodo.ElementAt(0);
        //                            fechaFin = listaFinPeriodo.ElementAt(trimestre - 1);
        //                        }
        //                        //Instante
        //                        else
        //                        {
        //                            fechaInicio = listaInicioPeriodo.ElementAt(trimestre - 1);
        //                        }
        //                        break;
        //                    case 6:
        //                        break;
        //                    case 7:
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //}


        //Type myListElementType = listBmv.GetType().GetGenericArguments().Single();


        //if (myListElementType == typeof(Bmv105000))
        //{
        //    res = true;
        //    return res;
        //}



        //  return res;

        //}
        public List<Bmv105000> GetBmv105000(int numTrimestre, int idAno)
        {
            List<Bmv105000> list = new List<Bmv105000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "105000", numTrimestre, idAno);    

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv105000 bmv = new Bmv105000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }





        public List<Bmv110000> GetBmv110000(int numTrimestre, int idAno)
        {
            List<Bmv110000> list = new List<Bmv110000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "110000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv110000 bmv = new Bmv110000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv210000> GetBmv210000(int numTrimestre, int idAno)
        {
            List<Bmv210000> list = new List<Bmv210000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "210000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv210000 bmv = new Bmv210000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv310000> GetBmv310000(int numTrimestre, int idAno)
        {
            List<Bmv310000> list = new List<Bmv310000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "310000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv310000 bmv = new Bmv310000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv410000> GetBmv410000(int numTrimestre, int idAno)
        {
            List<Bmv410000> list = new List<Bmv410000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "410000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv410000 bmv = new Bmv410000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv520000> GetBmv520000(int numTrimestre, int idAno)
        {
            List<Bmv520000> list = new List<Bmv520000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "520000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv520000 bmv = new Bmv520000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }


        public List<Bmv610000> GetBmv610000(int numTrimestre, int idAno)
        {
            List<Bmv610000> list = new List<Bmv610000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "610000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv610000 bmv = new Bmv610000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            bmv.IdAxisPadre = p.Id_Axis_Padre; 
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv610000> GetBmv610000Anterior(int numTrimestre, int idAno)
        {
            List<Bmv610000> list = new List<Bmv610000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "610000 Anterior", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv610000 bmv = new Bmv610000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            bmv.IdAxisPadre = p.Id_Axis_Padre;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<BmvDetalleSuma> GetBmvDetalleSuma(string idContenido)
        {
            List<BmvDetalleSuma> list = null;
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    // Query for all unicorns with names starting with B
                    var taxDet = from u in context.Taxonomia_Detalle_Suma
                                 join c in context.Cat_Contenido on u.Id_Contenido equals c.Id_Contenido
                                 where c.Contenido.Equals(idContenido)
                                 select new { u.Id_Taxonomia_Detalle_Hijo, u.Id_Taxonomia_Detalle_Padre, c.Contenido };
                    if (taxDet != null)
                    {
                        list = (from p in taxDet
                                select new BmvDetalleSuma
                                {
                                    IdTaxonomiaPadre = p.Id_Taxonomia_Detalle_Padre.HasValue ? p.Id_Taxonomia_Detalle_Padre.Value : 0,
                                    IdTaxonomiaHijo = p.Id_Taxonomia_Detalle_Hijo.HasValue ? p.Id_Taxonomia_Detalle_Hijo.Value : 0,
                                    Contenido = p.Contenido
                                }).ToList();
                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        private List<DateTime> getListaInicioPeriodo(int anoInicio)
        {
            List<DateTime> listPeriodoInicio = new List<DateTime>();
            //01-01-20XX
            listPeriodoInicio.Add(new DateTime(anoInicio, 1, 1));
            //01-04-20XX
            listPeriodoInicio.Add(new DateTime(anoInicio, 4, 1));
            //01-07-20XX
            listPeriodoInicio.Add(new DateTime(anoInicio, 7, 1));
            //01-10-20XX
            listPeriodoInicio.Add(new DateTime(anoInicio, 10, 1));
            return listPeriodoInicio;
        }

        private List<DateTime> getListaFinPeriodo(int anoFin)
        {
            List<DateTime> listPeriodoInicio = new List<DateTime>();
            //31-03-20XX
            listPeriodoInicio.Add(new DateTime(anoFin, 3, 31));
            //30-06-20XX
            listPeriodoInicio.Add(new DateTime(anoFin, 6, 30));
            //30-09-20XX
            listPeriodoInicio.Add(new DateTime(anoFin, 9, 30));
            //31-12-20XX
            listPeriodoInicio.Add(new DateTime(anoFin, 12, 31));
            return listPeriodoInicio;
        }


        public List<FormContenido> GetAllForms()
        {
            List<FormContenido> list = null;
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    // Query for all unicorns with names starting with B
                    context.Configuration.LazyLoadingEnabled = false;
                    var contenido = from c in context.Cat_Contenido
                                    orderby c.Contenido ascending
                                    select c;
                    if (contenido != null)
                    {
                        list = (from p in contenido
                                select new FormContenido
                                {
                                    Id = p.Id_Contenido,
                                    Descripcion = string.IsNullOrEmpty(p.Descripcion) == true ? "" : p.Descripcion.Trim(),
                                    Contenido = string.IsNullOrEmpty(p.Contenido) == true ? "" : p.Contenido.Trim(),
                                }).ToList();
                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }


        public bool SaveBmvReporte(List<ReporteDetalle> listBmv, string emisora, int idAno, int numTrimestre)
        {
            bool res = false;
            try
            {
                using (var context = new BmvXblrEntities())
                {

                    if (listBmv != null && listBmv.Any() == true)
                    {
                        DateTime dtNow = DateTime.Now;
                        context.Configuration.LazyLoadingEnabled = false;
                        var catTrimestre = (from u in context.Cat_Trimestre
                                             where u.Trimestre == numTrimestre
                                             select u).FirstOrDefault();
                        if (idAno > 0 && catTrimestre != null)
                        {
                            foreach (var bmvItem in listBmv)
                            {
                                //Quitamos los campos de sinopsis de solo lectura
                                if (string.IsNullOrEmpty(bmvItem.FormatoCampo) == false)
                                {
                                    int idReporteDetalle = bmvItem.IdReporteDetalle.HasValue ? bmvItem.IdReporteDetalle.Value : 0;
                                    //Verificamos si existen los datos en la base de datos. En este caso insertar
                                    if (idReporteDetalle == 0)
                                    {
                                        Taxonomia_Reporte_Detalle trd = new Taxonomia_Reporte_Detalle();
                                        trd.Id_Ano = idAno;
                                        trd.Id_Trimestre = catTrimestre.Id_Trimestre;
                                        trd.Id_Taxonomia_Reporte = bmvItem.IdReporte;
                                        trd.Valor = bmvItem.Valor;
                                        trd.Fecha_Guardado = dtNow;
                                        context.Entry(trd).State = System.Data.EntityState.Added;
                                    }
                                    //En caso contrario actualizamos el registro
                                    else
                                    {
                                        Taxonomia_Reporte_Detalle trd = context.Taxonomia_Reporte_Detalle.FirstOrDefault(c => c.Id_Taxonomia_Reporte_Detalle == bmvItem.IdReporteDetalle && c.Id_Ano == idAno && c.Id_Trimestre == catTrimestre.Id_Trimestre);
                                        if (trd != null)
                                        {
                                            if (bmvItem.Estado == true)
                                            {
                                                trd.Valor = bmvItem.Valor;
                                                trd.Fecha_Ultima_Actualizacion = dtNow;
                                                context.Entry(trd).State = System.Data.EntityState.Modified;
                                            }
                                            else
                                            {
                                                context.Entry(trd).State = System.Data.EntityState.Deleted;
                                            }
                                        }
                                    }
                                }
                            }
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected > 0)
                            {
                                res = true;
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        public bool SaveDinamicoBmvReporte(List<ReporteDetalle> listBmv, string emisora, int idAno, int numTrimestre)
        {
            bool res = false;
            try
            {
                using (var context = new BmvXblrEntities())
                {

                    if (listBmv != null && listBmv.Any() == true)
                    {
                        DateTime dtNow = DateTime.Now;
                        context.Configuration.LazyLoadingEnabled = false;
                        var catTrimestre = (from u in context.Cat_Trimestre
                                            where u.Trimestre == numTrimestre
                                            select u).FirstOrDefault();
                        if (idAno > 0 && catTrimestre != null)
                        {
                            foreach (var bmvItem in listBmv)
                            {
                                //Quitamos los campos de sinopsis de solo lectura
                                if (string.IsNullOrEmpty(bmvItem.FormatoCampo) == false)
                                {
                                    int idReporteDetalle = bmvItem.IdReporteDetalle.HasValue ? bmvItem.IdReporteDetalle.Value : 0;
                                    //Verificamos si existen los datos en la base de datos. En este caso insertar
                                    if (idReporteDetalle == 0)
                                    {
                                        Taxonomia_Reporte_Detalle trd = new Taxonomia_Reporte_Detalle();
                                        trd.Id_Ano = idAno;
                                        trd.Id_Trimestre = catTrimestre.Id_Trimestre;
                                        trd.Id_Taxonomia_Reporte = bmvItem.IdReporte;
                                        trd.Valor = bmvItem.Valor;
                                        trd.Identificador_Fila = bmvItem.IdentificadorFila;
                                        trd.Fecha_Guardado = dtNow;
                                        context.Entry(trd).State = System.Data.EntityState.Added;
                                    }
                                    //En caso contrario actualizamos o eliminamos el registro
                                    else
                                    {
                                        Taxonomia_Reporte_Detalle trd = context.Taxonomia_Reporte_Detalle.FirstOrDefault(c => c.Id_Taxonomia_Reporte_Detalle == bmvItem.IdReporteDetalle && c.Id_Ano == idAno && c.Id_Trimestre == catTrimestre.Id_Trimestre && bmvItem.IdentificadorFila == c.Identificador_Fila);
                                        if (trd != null) 
                                        {
                                            if(bmvItem.Estado == true)
                                            {
                                                trd.Valor = bmvItem.Valor;
                                                trd.Fecha_Ultima_Actualizacion = dtNow;
                                                context.Entry(trd).State = System.Data.EntityState.Modified;
                                            }
                                            else
                                            {
                                                context.Entry(trd).State = System.Data.EntityState.Deleted;
                                            }
                                        }
                                       
                                    }
                                }
                            }
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected > 0)
                            {
                                res = true;
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return res;
        }




        public List<Bmv813000> GetBmv813000(int numTrimestre, int idAno)
        {
            List<Bmv813000> list = new List<Bmv813000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "813000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv813000 bmv = new Bmv813000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800600> GetBmv800600(int numTrimestre, int idAno)
        {
            List<Bmv800600> list = new List<Bmv800600>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800600", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800600 bmv = new Bmv800600();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800500> GetBmv800500(int numTrimestre, int idAno)
        {
            List<Bmv800500> list = new List<Bmv800500>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800500", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800500 bmv = new Bmv800500();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800200> GetBmv800200(int numTrimestre, int idAno)
        {
            List<Bmv800200> list = new List<Bmv800200>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800200", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800200 bmv = new Bmv800200();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800100> GetBmv800100(int numTrimestre, int idAno)
        {
            List<Bmv800100> list = new List<Bmv800100>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800100", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800100 bmv = new Bmv800100();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800007> GetBmv800007(int numTrimestre, int idAno)
        {
            List<Bmv800007> list = new List<Bmv800007>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800007", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800007 bmv = new Bmv800007();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800005> GetBmv800005(int numTrimestre, int idAno)
        {
            List<Bmv800005> list = new List<Bmv800005>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetDynamicContent(context, "800005", numTrimestre, idAno);
                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800005 bmv = new Bmv800005();
                            bmv.Orden = p.Orden;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            bmv.CampoDinamico = p.Campo_Dinamico;
                            bmv.IdentificadorFila = p.Identificador_Fila;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800003> GetBmv800003(int numTrimestre, int idAno)
        {
            List<Bmv800003> list = new List<Bmv800003>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "800003", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800003 bmv = new Bmv800003();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv800001> GetBmv800001(int numTrimestre, int idAno)
        {
            List<Bmv800001> list = new List<Bmv800001>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetDynamicContent(context, "800001", numTrimestre, idAno);
                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv800001 bmv = new Bmv800001();
                            bmv.Orden = p.Orden;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            string formatoCampo = string.Empty;
                            if (string.IsNullOrEmpty(p.Formato_Campo) == false)
                            {
                                formatoCampo = p.Formato_Campo;
                                formatoCampo = formatoCampo.Trim();
                                if (formatoCampo.Equals("line_items") == true)
                                {
                                    formatoCampo = null;
                                }
                            }
                            bmv.FormatoCampo = formatoCampo;
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(formatoCampo) == true ? true : false;
                            bmv.CampoDinamico = p.Campo_Dinamico;
                            bmv.IdentificadorFila = p.Identificador_Fila;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv700003> GetBmv700003(int numTrimestre, int idAno)
        {
            List<Bmv700003> list = new List<Bmv700003>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "700003", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv700003 bmv = new Bmv700003();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv700002> GetBmv700002(int numTrimestre, int idAno)
        {
            List<Bmv700002> list = new List<Bmv700002>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "700002", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv700002 bmv = new Bmv700002();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<Bmv700000> GetBmv700000(int numTrimestre, int idAno)
        {
            List<Bmv700000> list = new List<Bmv700000>();
            try
            {
                using (var context = new BmvXblrEntities())
                {
                    var taxDet = GetContent(context, "700000", numTrimestre, idAno);

                    if (taxDet != null && taxDet.Any() == true)
                    {

                        foreach (var p in taxDet)
                        {
                            Bmv700000 bmv = new Bmv700000();
                            bmv.Orden = p.Orden != null ? p.Orden : 0;
                            bmv.Descripcion = p.Nivel_Sangria != null ? UtilBase.tabSpaces(p.Nivel_Sangria) + p.Descripcion : p.Descripcion;
                            bmv.FormatoCampo = string.IsNullOrEmpty(p.Formato_Campo) == true ? "" : p.Formato_Campo.Trim();
                            bmv.IdTaxonomiaDetalle = p.Id_Taxonomia_Detalle;
                            bmv.IdReporte = p.Id_Taxonomia_Reporte;
                            bmv.IdReporteDetalle = p.Id_Taxonomia_Reporte_Detalle;
                            bmv.AtributoColumna = string.IsNullOrEmpty(p.Atributo) == true ? "" : p.Atributo.Trim();
                            bmv.Contenido = p.Contenido;
                            bmv.Valor = string.IsNullOrEmpty(p.Valor) == true ? "" : p.Valor.Trim();
                            bmv.Lectura = string.IsNullOrEmpty(p.Formato_Campo) == true ? true : false;
                            list.Add(bmv);

                        }

                    }


                }

            }
            catch (Exception ex)
            {

            }
            return list;
        }


        public List<PeriodoTrimestre> GetAllTrimesters()
        {
            List<PeriodoTrimestre> list = new List<PeriodoTrimestre>();
            try
            {
                using (var context = new BmvXblrEntities())
                {

                        context.Configuration.LazyLoadingEnabled = false;
                        var tempTrimestre = (from u in context.Cat_Trimestre
                                       select u);
                        if(tempTrimestre != null && tempTrimestre.Any() == true)
                        {

                            foreach (var p in tempTrimestre)
                            {
                                PeriodoTrimestre item = new PeriodoTrimestre();
                                item.IdTrimestre = p.Id_Trimestre;
                                item.Descripcion = p.Descripcion;
                                item.NumTrimestre = p.Trimestre.HasValue == true ? Convert.ToInt32(p.Trimestre.Value) : 0; 
                                list.Add(item);

                            }
                        }

                    }
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        public List<PeriodoAno> GetAllYears()
        {
            List<PeriodoAno> list = new List<PeriodoAno>();
            try
            {
                using (var context = new BmvXblrEntities())
                {

                    context.Configuration.LazyLoadingEnabled = false;
                    var tempAno = (from u in context.Cat_Ano
                                         select u);
                    if (tempAno != null && tempAno.Any() == true)
                    {
                        foreach (var p in tempAno)
                        {
                            PeriodoAno item = new PeriodoAno();
                            item.IdAno = p.Id_Ano;
                            item.Ano = p.Ano.HasValue == true ? Convert.ToString(p.Ano.Value) : "";
                            list.Add(item);

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        private IEnumerable<dynamic> GetContent(BmvXblrEntities context, string contenido, int idTrimestre, int idAno)
        {
            var taxDet = from td in context.Cat_Taxonomia_Detalle
                         join f in context.Cat_Tipo_Formato on td.Id_Tipo_Formato equals f.Id_Tipo_Formato into f_group
                         join oe in context.Cat_Origen_Elemento on td.Id_Origen_Elemento equals oe.Id_Origen_Elemento into oe_group
                         join tr in context.Taxonomia_Reporte on td.Id_Taxonomia_Detalle equals tr.Id_Taxonomia_Detalle into tr_group
                         from tr in tr_group.DefaultIfEmpty()
                         join c in context.Cat_Contenido on tr.Id_Contenido equals c.Id_Contenido into c_group
                         join trd in
                             (from sub in context.Taxonomia_Reporte_Detalle
                              join trim in context.Cat_Trimestre on sub.Id_Trimestre equals trim.Id_Trimestre
                              where trim.Trimestre == idTrimestre && sub.Id_Ano == idAno select sub)
                         on tr.Id_Taxonomia_Reporte equals trd.Id_Taxonomia_Reporte into trd_group
                         join tc in context.Cat_Taxonomia_Columna on tr.Id_Taxonomia_Columna equals tc.Id_Taxonomia_Columna into tc_group
                         from tc in tc_group.DefaultIfEmpty()
                         join mc in context.Cat_Modelo_Clase on tc.Id_Modelo_Clase equals mc.Id_Modelo_Clase into mc_group
                         join tdd in context.Cat_Taxonomia_Detalle on tc.Id_Taxonomia_Detalle equals tdd.Id_Taxonomia_Detalle into tdd_group
                         from tdd in tdd_group.DefaultIfEmpty()
                         from f in f_group.DefaultIfEmpty()
                         from oe in oe_group.DefaultIfEmpty()
                         from c in c_group.DefaultIfEmpty()
                         from trd in trd_group.DefaultIfEmpty()
                         from mc in mc_group.DefaultIfEmpty()
                         where c.Contenido.Equals(contenido) && td.Orden != null && tr.Id_Taxonomia_Reporte != null
                         orderby td.Orden ascending
                         select new
                         {
                             Id_Taxonomia_Reporte_Detalle = (int?)trd.Id_Taxonomia_Reporte_Detalle,
                             Id_Taxonomia_Detalle = td.Id_Taxonomia_Detalle,
                             Id_Taxonomia_Reporte = tr.Id_Taxonomia_Reporte,
                             Descripcion = td.Descripcion,
                             Formato_Campo = f.Formato,
                             Atributo = mc.Atributo,
                             Contenido = c.Contenido,
                             Orden = td.Orden,
                             Valor = trd.Valor,
                             Nivel_Sangria = td.Nivel_Sangria,
                             Id_Axis_Padre = td.Id_Axis_Padre
                         };

            return taxDet;
        }

        private IEnumerable<dynamic> GetDynamicContent(BmvXblrEntities context, string contenido, int numTrimestre, int idAno)
        {
            var taxDet = from td in context.Cat_Taxonomia_Detalle
                         join f in context.Cat_Tipo_Formato on td.Id_Tipo_Formato equals f.Id_Tipo_Formato into f_group
                         join oe in context.Cat_Origen_Elemento on td.Id_Origen_Elemento equals oe.Id_Origen_Elemento into oe_group
                         join tr in context.Taxonomia_Reporte on td.Id_Taxonomia_Detalle equals tr.Id_Taxonomia_Detalle into tr_group
                         from tr in tr_group.DefaultIfEmpty()
                         join c in context.Cat_Contenido on tr.Id_Contenido equals c.Id_Contenido into c_group
                         join trd in
                             (from sub in context.Taxonomia_Reporte_Detalle
                              join trim in context.Cat_Trimestre on sub.Id_Trimestre equals trim.Id_Trimestre
                              where trim.Trimestre == numTrimestre && sub.Id_Ano == idAno
                              select sub)
                         on tr.Id_Taxonomia_Reporte equals trd.Id_Taxonomia_Reporte into trd_group
                         join tc in context.Cat_Taxonomia_Columna on tr.Id_Taxonomia_Columna equals tc.Id_Taxonomia_Columna into tc_group
                         from tc in tc_group.DefaultIfEmpty()
                         join mc in context.Cat_Modelo_Clase on tc.Id_Modelo_Clase equals mc.Id_Modelo_Clase into mc_group
                         join tdd in context.Cat_Taxonomia_Detalle on tc.Id_Taxonomia_Detalle equals tdd.Id_Taxonomia_Detalle into tdd_group
                         from tdd in tdd_group.DefaultIfEmpty()
                         from f in f_group.DefaultIfEmpty()
                         from oe in oe_group.DefaultIfEmpty()
                         from c in c_group.DefaultIfEmpty()
                         from trd in trd_group.DefaultIfEmpty()
                         from mc in mc_group.DefaultIfEmpty()
                         where c.Contenido.Equals(contenido) && td.Orden != null && tr.Id_Taxonomia_Reporte != null
                         orderby td.Orden ascending, trd.Identificador_Fila descending
                         select new
                         {
                             Id_Taxonomia_Reporte_Detalle = (int?)trd.Id_Taxonomia_Reporte_Detalle,
                             Id_Taxonomia_Detalle = td.Id_Taxonomia_Detalle,
                             Id_Taxonomia_Reporte = tr.Id_Taxonomia_Reporte,
                             Descripcion = td.Descripcion,
                             Formato_Campo = f.Formato,
                             Atributo = mc.Atributo,
                             Contenido = c.Contenido,
                             Orden = td.Orden,
                             Valor = trd.Valor,
                             Nivel_Sangria = td.Nivel_Sangria,
                             Campo_Dinamico = td.Campo_Dinamico.HasValue ? td.Campo_Dinamico.Value : false,
                             Identificador_Fila = trd.Identificador_Fila.HasValue ? trd.Identificador_Fila.Value : 1
                         };

            return taxDet;
        }


        public List<string> GetPeriodoSinPresentar(int numTrimestre, string contenido)
        {
            List<String> listAtributoColumna = new List<String>();
            try
            {
                using (var context = new BmvXblrEntities())
                {

                    context.Configuration.LazyLoadingEnabled = false;
                    var tempList = (from tc in context.Cat_Taxonomia_Columna
                                         join cc in context.Cat_Contenido on tc.Id_Contenido equals cc.Id_Contenido
                                         join mc in context.Cat_Modelo_Clase on tc.Id_Modelo_Clase equals mc.Id_Modelo_Clase
                                         join vc in context.Cat_Validacion_Contexto on tc.Id_Validacion_Contexto equals vc.Id_Validacion_Contexto
                                         join ps in context.Periodo_Sin_Presentar on vc.Id_Validacion_Contexto equals ps.Id_Validacion_Contexto
                                         join trim in context.Cat_Trimestre on ps.Id_Trimestre equals trim.Id_Trimestre
                                         where cc.Contenido.Equals(contenido) && trim.Trimestre == numTrimestre
                                         select mc.Atributo);
                    if (tempList != null && tempList.Any() == true)
                    {
                        listAtributoColumna =  tempList.ToList();
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return listAtributoColumna;
        }
    }
}
