using System;
using System.ComponentModel;
using System.Net;

namespace TaxonomiaWeb.Model
{
    public class Bmv610000 : BmvBase
    {
        private double capitalSocial;

        public double CapitalSocial
        {
            get { return capitalSocial; }
            set
            {
                if (value != capitalSocial)
                {
                    capitalSocial = value;
                    base.onPropertyChanged(this, "CapitalSocial");
                }
            }
        }
        private double primaEnEmisionDeAcciones;

        public double PrimaEnEmisionDeAcciones
        {
            get { return primaEnEmisionDeAcciones; }
            set
            {
                if (value != primaEnEmisionDeAcciones)
                {
                    primaEnEmisionDeAcciones = value;
                    base.onPropertyChanged(this, "PrimaEnEmisionDeAcciones");
                }  
            }
        }
        private double accionesEnTesoreria;

        public double AccionesEnTesoreria
        {
            get { return accionesEnTesoreria; }
            set
            {
                if (value != accionesEnTesoreria)
                {
                    accionesEnTesoreria = value;
                    base.onPropertyChanged(this, "AccionesEnTesoreria");
                } 
            }
        }
        private double utilidadesAcumuladas;

        public double UtilidadesAcumuladas
        {
            get { return utilidadesAcumuladas; }
            set
            {
                if (value != utilidadesAcumuladas)
                {
                    utilidadesAcumuladas = value;
                    base.onPropertyChanged(this, "UtilidadesAcumuladas");
                } 
            }
        }
        private double superavitDeRevaluacion;

        public double SuperavitDeRevaluacion
        {
            get { return superavitDeRevaluacion; }
            set
            {
                if (value != superavitDeRevaluacion)
                {
                    superavitDeRevaluacion = value;
                    base.onPropertyChanged(this, "SuperavitDeRevaluacion");
                } 
            }
        }
        private double efectoPorConversion;

        public double EfectoPorConversion
        {
            get { return efectoPorConversion; }
            set
            {
                if (value != efectoPorConversion)
                {
                    efectoPorConversion = value;
                    base.onPropertyChanged(this, "EfectoPorConversion");
                } 
            }
        }
        private double coberturasDeFlujosDeEfectivo;

        public double CoberturasDeFlujosDeEfectivo
        {
            get { return coberturasDeFlujosDeEfectivo; }
            set
            {
                if (value != coberturasDeFlujosDeEfectivo)
                {
                    coberturasDeFlujosDeEfectivo = value;
                    base.onPropertyChanged(this, "CoberturasDeFlujosDeEfectivo");
                } 
            }
        }
        private double utilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital;

        public double UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital
        {
            get { return utilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital; }
            set
            {
                if (value != utilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital)
                {
                    utilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital = value;
                    base.onPropertyChanged(this, "UtilidadPerdidaEnInstrumentosDeCoberturaQueCubrenInversionesEnInstrumentosDeCapital");
                } 
            }
        }
        private double variacionEnElValorTemporalDeLasOpciones;

        public double VariacionEnElValorTemporalDeLasOpciones
        {
            get { return variacionEnElValorTemporalDeLasOpciones; }
            set
            {
                if (value != variacionEnElValorTemporalDeLasOpciones)
                {
                    variacionEnElValorTemporalDeLasOpciones = value;
                    base.onPropertyChanged(this, "VariacionEnElValorTemporalDeLasOpciones");
                } 
            }
        }
        private double variacionEnElValorDeContratosAFuturo;

        public double VariacionEnElValorDeContratosAFuturo
        {
            get { return variacionEnElValorDeContratosAFuturo; }
            set
            {
                if (value != variacionEnElValorDeContratosAFuturo)
                {
                    variacionEnElValorDeContratosAFuturo = value;
                    base.onPropertyChanged(this, "VariacionEnElValorDeContratosAFuturo");
                } 
            }
        }
        private double variacionEnElValorDeMárgenesConBaseEnMonedaExtranjera;

        public double VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera
        {
            get { return variacionEnElValorDeMárgenesConBaseEnMonedaExtranjera; }
            set
            {
                if (value != variacionEnElValorDeContratosAFuturo)
                {
                    variacionEnElValorDeMárgenesConBaseEnMonedaExtranjera = value;
                    base.onPropertyChanged(this, "VariacionEnElValorDeMárgenesConBaseEnMonedaExtranjera");
                } 
            }
        }
        private double utilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta;

        public double UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta
        {
            get { return utilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta; }
            set
            {
                if (value != utilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta)
                {
                    utilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta = value;
                    base.onPropertyChanged(this, "UtilidadPerdidaPorCambiosEnValorRazonableDeActivosFinancierosDisponiblesParaLaVenta");
                }
            }
        }
        private double pagosBasadosEnAcciones;

        public double PagosBasadosEnAcciones
        {
            get { return pagosBasadosEnAcciones; }
            set
            {
                if (value != pagosBasadosEnAcciones)
                {
                    pagosBasadosEnAcciones = value;
                    base.onPropertyChanged(this, "PagosBasadosEnAcciones");
                } 
            }
        }
        private double nuevasMedicionesDePlanesDeBeneficiosDefinidos;

        public double NuevasMedicionesDePlanesDeBeneficiosDefinidos
        {
            get { return nuevasMedicionesDePlanesDeBeneficiosDefinidos; }
            set
            {
                if (value != nuevasMedicionesDePlanesDeBeneficiosDefinidos)
                {
                    nuevasMedicionesDePlanesDeBeneficiosDefinidos = value;
                    base.onPropertyChanged(this, "NuevasMedicionesDePlanesDeBeneficiosDefinidos");
                } 
            }
        }
        private double importesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta;

        public double ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta
        {
            get { return importesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta; }
            set
            {
                if (value != importesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta)
                {
                    importesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta = value;
                    base.onPropertyChanged(this, "ImportesReconocidosEnOtroResultadoIntegralYAcumuladosEnElCapitalContableRelativosAActivosNoCorrientesOGruposDeActivosParaSuDisposicionMantenidosParaLaVenta");
                } 
            }
        }
        private double utilidadPerdidaPorInversionesEnInstrumentosDeCapital;

        public double UtilidadPerdidaPorInversionesEnInstrumentosDeCapital
        {
            get { return utilidadPerdidaPorInversionesEnInstrumentosDeCapital; }
            set
            {
                if (value != utilidadPerdidaPorInversionesEnInstrumentosDeCapital)
                {
                    utilidadPerdidaPorInversionesEnInstrumentosDeCapital = value;
                    base.onPropertyChanged(this, "UtilidadPerdidaPorInversionesEnInstrumentosDeCapital");
                } 
            }
        }
        private double reservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo;

        public double ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo
        {
            get { return reservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo; }
            set
            {
                if (value != reservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo)
                {
                    reservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo = value;
                    base.onPropertyChanged(this, "ReservaParaCambiosEnElValorRazonableDePasivosFinancierosAtribuiblesACambiosEnElRiesgoDeCreditoDePasivo");
                } 
            }
        }
        private double reservaParaCatastrofes;

        public double ReservaParaCatastrofes
        {
            get { return reservaParaCatastrofes; }
            set
            {
                if (value != reservaParaCatastrofes)
                {
                    reservaParaCatastrofes = value;
                    base.onPropertyChanged(this, "ReservaParaCatastrofes");
                } 
            }
        }
        private double reservaParaEstabilizacion;

        public double ReservaParaEstabilizacion
        {
            get { return reservaParaEstabilizacion; }
            set
            {
                if (value != reservaParaEstabilizacion)
                {
                    reservaParaEstabilizacion = value;
                    base.onPropertyChanged(this, "ReservaParaEstabilizacion");
                } 
            }
        }
        private double reservaDeComponentesDeParticipacionDiscrecional;

        public double ReservaDeComponentesDeParticipacionDiscrecional
        {
            get { return reservaDeComponentesDeParticipacionDiscrecional; }
            set
            {
                if (value != reservaDeComponentesDeParticipacionDiscrecional)
                {
                    reservaDeComponentesDeParticipacionDiscrecional = value;
                    base.onPropertyChanged(this, "ReservaDeComponentesDeParticipacionDiscrecional");
                } 
            }
        }
        private double otrosResultadosIntegrales;

        public double OtrosResultadosIntegrales
        {
            get { return otrosResultadosIntegrales; }
            set
            {
                if (value != otrosResultadosIntegrales)
                {
                    otrosResultadosIntegrales = value;
                    base.onPropertyChanged(this, "OtrosResultadosIntegrales");
                } 
            }
        }
        private double otrosResultadosIntegralesAcumulados;

        public double OtrosResultadosIntegralesAcumulados
        {
            get { return otrosResultadosIntegralesAcumulados; }
            set
            {
                if (value != otrosResultadosIntegralesAcumulados)
                {
                    otrosResultadosIntegralesAcumulados = value;
                    base.onPropertyChanged(this, "OtrosResultadosIntegralesAcumulados");
                } 
            }
        }
        private double capitalContableDeLaParticipacionControladora;

        public double CapitalContableDeLaParticipacionControladora
        {
            get { return capitalContableDeLaParticipacionControladora; }
            set
            {
                if (value != capitalContableDeLaParticipacionControladora)
                {
                    capitalContableDeLaParticipacionControladora = value;
                    base.onPropertyChanged(this, "CapitalContableDeLaParticipacionControladora");
                } 
            }
        }
        private double participacionNoControladora;

        public double ParticipacionNoControladora
        {
            get { return participacionNoControladora; }
            set
            {
                if (value != participacionNoControladora)
                {
                    participacionNoControladora = value;
                    base.onPropertyChanged(this, "ParticipacionNoControladora");
                } 
            }
        }
        private double capitalContable;

        public double CapitalContable
        {
            get { return capitalContable; }
            set
            {
                if (value != capitalContable)
                {
                    capitalContable = value;
                    base.onPropertyChanged(this, "CapitalContable");
                } 
            }
        }



    }
}
