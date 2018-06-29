using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Masters;
using DataContracts;

namespace OperationContracts
{
    [ServiceContract]
    public interface IRefreshDistributionData
    {

        #region distributor refresh Country
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/CountryMaster/{Country_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncCountryMaster(string country_id, string CreatedBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/CountryMapping/{Country_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncCountryMapping(string country_id, string CreatedBy);

        #endregion

        #region distributor refresh City
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/CityMaster/{City_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncCityMaster(string city_id, string CreatedBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/CityMapping/{City_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncCityMapping(string city_id, string CreatedBy);

        #endregion

        #region Hotel
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/HotelMapping/{Hotel_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncHotelMapping(string hotel_id, string CreatedBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/HotelMappingLite/{Hotel_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncHotelMappingLite(string hotel_id, string CreatedBy);

        #endregion

        #region Activity
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/ActivityMapping/{Activity_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncActivityMapping(string activity_id, string CreatedBy);

        #endregion

        #region Supplier
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/SupplierMaster/{Supplier_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncSupplierMaster(string supplier_id, string CreatedBy);

        #endregion

        

        #region RefreshDistributionLog
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Get/RefreshDistributionLog", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_RefreshDistributionDataLog> GetRefreshDistributionLog();
        #endregion

        #region RefreshDistributionLog
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Get/RefreshDistributionHotelLog", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_RefreshDistributionDataLog> GetRefreshStaticHotelLog();
        #endregion

        #region port
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/PortMaster/{Port_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncPortMaster(string port_id, string CreatedBy);
        #endregion

        #region state
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/StateMaster/{State_Id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncStateMaster(string state_id, string CreatedBy);
        #endregion

        #region Supplier Data
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/SupplierStaticDetails", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_SupplierEntity> LoadSupplierData();
        #endregion

        #region Supplier static entity
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Load/SupplierMaster/{log_id}/{supplier_id}/{CreatedBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message SyncSupplierStaticHotel(string log_id ,string supplier_id, string CreatedBy);
        #endregion


    }


}
