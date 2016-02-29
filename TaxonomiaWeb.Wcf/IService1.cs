using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaxonomiaWeb.Model;

namespace TaxonomiaWeb.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        List<Bmv813000> GetBmv813000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800600> GetBmv800600(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800500> GetBmv800500(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800200> GetBmv800200(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800100> GetBmv800100(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800007> GetBmv800007(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800005> GetBmv800005(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800003> GetBmv800003(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv800001> GetBmv800001(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv700003> GetBmv700003(int idTrimestre, int idAno);
        
        [OperationContract]
        List<Bmv700002> GetBmv700002(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv700000> GetBmv700000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv610000> GetBmv610000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv610000> GetBmv610000Anterior(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv520000> GetBmv520000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv410000> GetBmv410000(int idTrimestre, int idAno);
        
        [OperationContract]
        List<Bmv310000> GetBmv310000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv210000> GetBmv210000(int idTrimestre, int idAno);

        [OperationContract]
        List<Bmv105000> GetBmv105000(int idTrimestre,int idAno);

        [OperationContract]
        List<Bmv110000> GetBmv110000(int idTrimestre, int idAno);

        [OperationContract]
        List<FormContenido> GetAllForms();

        [OperationContract]
        List<BmvDetalleSuma> GetBmvDetalleSuma(string idContenido);

        [OperationContract]
        bool SaveBmvReporte(List<ReporteDetalle> listBmv, string emisora, int periodo, int trimestre);

        [OperationContract]
        bool SaveDinamicoBmvReporte(List<ReporteDetalle> listBmv, string emisora, int periodo, int trimestre);

        [OperationContract]
        List<PeriodoTrimestre> GetAllTrimesters();

        [OperationContract]
        List<PeriodoAno> GetAllYears();

        [OperationContract]
        int GenerarContextos(int idTrimestre, int idAno);

        [OperationContract]
        List<String> GetPeriodoSinPresentar(int idTrimestre, string contenido);

        // TODO: Add your service operations here
    }

}
