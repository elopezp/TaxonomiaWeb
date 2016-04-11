using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TaxonomiaWeb.Utils
{
    public class AppConsts
    {
        public const string COL_DESCRIPCION = "Descripcion";
        public const string COL_TRIMESTREACTUAL = "TrimestreActual";
        public const string COL_CIERREEJERCICIOANTERIOR = "CierreEjercicioAnterior";
        public const string COL_INICIOEJERCICIOANTERIOR = "InicioEjercicioAnterior";
        public const string COL_TRIMESTREANOANTERIOR = "TrimestreAnoAnterior";
        public const string COL_ACUMULADOANOACTUAL = "AcumuladoAnoActual";
        public const string COL_ACUMULADOANOANTERIOR = "AcumuladoAnoAnterior";
        public const string COL_CAPITALSOCIAL = "CapitalSocial";
        public const string COL_PRIMAENEMISIONDEACCIONES = "PrimaEnEmisionDeAcciones";
        public const string COL_ACCIONESENTESORERIA = "AccionesEnTesoreria";
        public const string COL_UTILIDADESACUMULADAS = "UtilidadesAcumuladas";
        public const string COL_SUPERAVITDEREVALUACION = "SuperavitDeRevaluacion";
        public const string COL_EFECTOPORCONVERSION = "EfectoPorConversion";
        public const string COL_COBERTURASDEFLUJOSDEEFECTIVO = "CoberturasDeFlujosDeEfectivo";
        public const string COL_UTILIDADPERDIDAENINSTRUMENTOSDECOBERTURAQUECUBRENINVERSIONESENINSTRUMENTOSDECAPITAL = "UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital";
        public const string COL_VARIACIONENELVALORTEMPORALDELASOPCIONES = "VariacionEnElValorTemporalDeLasOpciones";
        public const string COL_VARIACIONENELVALORDECONTRATOSAFUTURO = "VariacionEnElValorDeContratosAFuturo";
        public const string COL_VARIACIONENELVALORDEMÁRGENESCONBASEENMONEDAEXTRANJERA = "VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera";
        public const string COL_UTILIDADPERDIDAPORCAMBIOSENVALORRAZONABLEDEACTIVOSFINANCIEROSDISPONIBLESPARALAVENTA = "UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta";
        public const string COL_PAGOSBASADOSENACCIONES = "PagosBasadosEnAcciones";
        public const string COL_NUEVASMEDICIONESDEPLANESDEBENEFICIOSDEFINIDOS = "NuevasMedicionesDePlanesDeBeneficiosDefinidos";
        public const string COL_IMPORTESRECONOCIDOSENOTRORESULTADOINTEGRALYACUMULADOSENELCAPITALCONTABLERELATIVOSAACTIVOSNOCORRIENTESOGRUPOSDEACTIVOSPARASUDISPOSICIONMANTENIDOSPARALAVENTA = "ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta";
        public const string COL_UTILIDADPERDIDAPORINVERSIONESENINSTRUMENTOSDECAPITAL = "UtilidadPerdidaPorInversionesEnInstrumentosDeCapital";
        public const string COL_RESERVAPARACAMBIOSENELVALORRAZONABLEDEPASIVOSFINANCIEROSATRIBUIBLESACAMBIOSENELRIESGODECREDITODEPASIVO = "ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo";
        public const string COL_RESERVAPARACATASTROFES = "ReservaParaCatastrofes";
        public const string COL_RESERVAPARAESTABILIZACION = "ReservaParaEstabilizacion";
        public const string COL_RESERVADECOMPONENTESDEPARTICIPACIONDISCRECIONAL = "ReservaDeComponentesDeParticipacionDiscrecional";
        public const string COL_OTROSRESULTADOSINTEGRALES = "OtrosResultadosIntegrales";
        public const string COL_OTROSRESULTADOSINTEGRALESACUMULADOS = "OtrosResultadosIntegralesAcumulados";
        public const string COL_CAPITALCONTABLEDELAPARTICIPACIONCONTROLADORA = "CapitalContableDeLaParticipacionControladora";
        public const string COL_PARTICIPACIONNOCONTROLADORA = "ParticipacionNoControladora";
        public const string COL_CAPITALCONTABLE = "CapitalContable";
        public const string COL_ANOACTUAL = "AnoActual";
        public const string COL_ANOANTERIOR = "AnoAnterior";
        public const string COL_INSTITUCION = "Institucion";
        public const string COL_INSTITUCIONEXTRANJERASINO = "InstitucionExtranjeraSiNo";
        public const string COL_FECHADEFIRMACONTRATO = "FechaDeFirmaContrato";
        public const string COL_FECHADEVENCIMIENTO = "FechaDeVencimiento";
        public const string COL_TASADEINTERESYOSOBRETASA = "TasaDeInteresYOSobreTasa";
        public const string COL_MONEDANACIONALANOACTUAL = "MonedaNacionalAnoActual";
        public const string COL_MONEDANACIONALHASTA1ANO = "MonedaNacionalHasta1Ano";
        public const string COL_MONEDANACIONALHASTA2ANOS = "MonedaNacionalHasta2Anos";
        public const string COL_MONEDANACIONALHASTA3ANOS = "MonedaNacionalHasta3Anos";
        public const string COL_MONEDANACIONALHASTA4ANOS = "MonedaNacionalHasta4Anos";
        public const string COL_MONEDANACIONALHASTA5ANOSOMAS = "MonedaNacionalHasta5AnosOMas";
        public const string COL_MONEDAEXTRANJERAANOACTUAL = "MonedaExtranjeraAnoActual";
        public const string COL_MONEDAEXTRANJERAHASTA1ANO = "MonedaExtranjeraHasta1Ano";
        public const string COL_MONEDAEXTRANJERAHASTA2ANOS = "MonedaExtranjeraHasta2Anos";
        public const string COL_MONEDAEXTRANJERAHASTA3ANOS = "MonedaExtranjeraHasta3Anos";
        public const string COL_MONEDAEXTRANJERAHASTA4ANOS = "MonedaExtranjeraHasta4Anos";
        public const string COL_MONEDAEXTRANJERAHASTA5ANOSOMAS = "MonedaExtranjeraHasta5AnosOMas";
        public const string COL_DOLARES = "Dolares";
        public const string COL_DOLARESCONTRAVALORPESOS = "DolaresContravalorPesos";
        public const string COL_OTRASMONEDASCONTRAVALORDOLARES = "OtrasMonedasContravalorDolares";
        public const string COL_OTRASMONEDASCONTRAVALORPESOS = "OtrasMonedasContravalorPesos";
        public const string COL_TOTALDEPESOS = "TotalDePesos";
        public const string COL_PRINCIPALESMARCAS = "PrincipalesMarcas";
        public const string COL_PRINCIPALESPRODUCTOSOLINEADEPRODUCTOS = "PrincipalesProductosOLineaDeProductos";
        public const string COL_INGRESOSNACIONALES = "IngresosNacionales";
        public const string COL_INGRESOSPOREXPORTACION = "IngresosPorExportacion";
        public const string COL_INGRESOSDESUBSIDIARIASENELEXTRANJERO = "IngresosDeSubsidiariasEnElExtranjero";
        public const string COL_INGRESOSTOTALES = "IngresosTotales";


        public const string FORMAT_TEXTBLOCK = "text_block";
        public const string FORMAT_TEXT = "text";
        public const string FORMAT_YYMMDD = "yyyy-mm-dd";
        public const string FORMAT_X = "X";
        public const string FORMAT_X_NEGATIVE = "(X)";
        public const string FORMAT_XXX = "X.XX";
        public const string FORMAT_SHARES = "shares";
        public const string FORMAT_SUM = "+";
        public const string FORMAT_TABLE = "table";
        public const string FORMAT_AXISEXPLICITMEMBER = "axis_explicit_member";
        public const string FORMAT_AXIS_IMPLICIT_MEMBER = "axis_implicit_member";
        public const string FORMAT_MEMBER = "member";
        public const string FORMAT_LINE_ITEMS = "line_items";
        public const string FORMAT_BOOLEAN = "boolean";

        public const int MAXWIDTH_COL_DESCRIPCION = 600;

    }
}
