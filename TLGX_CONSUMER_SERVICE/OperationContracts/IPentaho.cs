using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts;

namespace OperationContracts
{
    [ServiceContract]
    public interface IPentaho
    {
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Pentaho/SupplierApi/Call/{ApiLocationId}/{CalledBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Pentaho_SupplierApi_Call(string ApiLocationId, string CalledBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Pentaho/SupplierApi/ViewDetails/{PentahoCallId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Pentaho.DC_PentahoTransStatus_TransStatus Pentaho_SupplierApiCall_ViewDetails(string PentahoCallId);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Pentaho/SupplierApi/GetId/{SupplierId}/{EntityId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string Pentaho_SupplierApiLocationId_Get(string SupplierId, string EntityId);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Pentaho/SupplierApi/RemoveCall/{PentahoCallId}/{CalledBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Pentaho_SupplierApiCall_Remove(string PentahoCallId, string CalledBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Pentaho/SupplierApi/CallList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Pentaho.DC_PentahoApiCallLogDetails> Pentaho_SupplierApiCall_List();
    }
}
