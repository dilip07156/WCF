using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;

namespace OperationContracts
{
    [ServiceContract]
    public interface IMapping
    {
        #region Accomodation Product Mapping

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/Accomodation/ShiftMapping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool ShiftAccommodationMappings(DataContracts.Mapping.DC_Mapping_ShiftMapping_RQ obj);



        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccomodationProductbyId/Get/{Accommodation_ProductMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMappingById(string Accommodation_ProductMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccomodationProduct/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/ProductSupplier/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccomodationProduct/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateAccomodationProductMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> PM);
        #endregion

        #region Supplier Room Type Mapping
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccoSupplierRoomType/Get/{PageNo}/{PageSize}/{Accomodation_Id}/{Supplier_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping> GetAccomodationSupplierRoomTypeMapping(string PageNo, string PageSize, string Accomodation_Id, string Supplier_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccoSupplierRoomType/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateAccomodationSupplierRoomTypeMapping(DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccoSupplierRoomType/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> AccomodationSupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccoSupplierRoomType/UpdateMap", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_UpdateMap(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/AccoSupplierRoomType/TTFU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_TTFUALL(List<DataContracts.Mapping.DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids);

        


        #endregion

        #region Country Mapping
        //[OperationContract]
        //[FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        //[WebInvoke(Method = "GET", UriTemplate = "Mapping/Country/Get/{PageNo}/{PageSize}/{Supplier_Id}/{Country_Id}/{Sts}/{SortBy}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //IList<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(string PageNo, string PageSize, string Supplier_Id, string Country_Id, string Sts, string SortBy);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/Country/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(DataContracts.Mapping.DC_CountryMappingRQ RQ);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/Country/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> CM);
        #endregion

        #region City Mapping
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/City/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_CityMapping> GetCityMapping(DataContracts.Mapping.DC_CityMapping_RQ param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/City/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> CM);

        #endregion

        #region Master Attribute Mapping

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttribute/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_MasterAttributeMapping_RS> SearchMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/MasterAttribute/Get/{MasterAttributeMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Mapping.DC_MasterAttributeMapping GetMasterAttributeMapping(string MasterAttributeMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttributeValue/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_MasterAttributeValueMappingRS> GetMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping_RQ RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttribute/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Mapping.DC_MasterAttributeMappingAdd_RS AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttribute/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttributeValue/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateMasterAttributeValueMapping(List<DataContracts.Mapping.DC_MasterAttributeValueMapping> param);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/MasterAttributeValue/Delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeleteMasterAttributeValueMapping(DC_SupplierAttributeValues_RQ param);
        #endregion

        #region Activity Mappling

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/ActivitySupplierProductMappingById/Get/{ActivitySupplierProductMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.Mapping.DC_Acitivity_SupplierProductMapping GetActivitySupplierProductMappingById(string ActivitySupplierProductMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/ActivitySupplierProductMapping/Get/{PageNo}/{PageSize}/{Activity_Id}/{Status}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMapping(string PageNo, string PageSize, string Activity_Id, string Status);



        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/ActivitySupplierProductMapping/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearch(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/ActivitySupplierProductMapping/SearchForMapping", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearchForMapping(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/ActivityIsMappingWith/Supplier/{masterActivityID}/{supplierID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool IsMappedWithSupplier(string masterActivityID, string supplierID);
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/ActivitySupplierForDDL/Search", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/ActivitySupplierProductMapping/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateActivitySupplierProductMapping(List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> PM);
        #endregion

        #region velocity Dashboard
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/Statistics/GetVelocityDashboard", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_VelocityMappingStats> GetVelocityDashboard(DataContracts.Mapping.DC_VelocityReport parm);
        #endregion

        #region HotelListLinkedToCityCodeAndSupplierCode
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "Mapping/City/GetHotelListByCityCode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Mapping.DC_HotelListByCityCode> GetHotelListByCityCode(DataContracts.Mapping.DC_HotelListByCityCode_RQ param);
        #endregion

        #region ML_Service
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccoSupplierRoomType/GetRTM_ML_Suggestions_Syntactic/{Accomodation_SupplierRoomTypeMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_SRT_ML_Response_Syntactic> GetRTM_ML_Suggestions_Syntactic(string Accomodation_SupplierRoomTypeMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccoSupplierRoomType/GetRTM_ML_Suggestions_Semantic/{Accomodation_SupplierRoomTypeMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_SRT_ML_Response_Semantic> GetRTM_ML_Suggestions_Semantic(string Accomodation_SupplierRoomTypeMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccoSupplierRoomType/GetRTM_ML_Suggestions_Supervised_Semantic/{Accomodation_SupplierRoomTypeMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.DC_SRT_ML_Response_Supervised_Semantic> GetRTM_ML_Suggestions_Supervised_Semantic(string Accomodation_SupplierRoomTypeMapping_Id);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "Mapping/AccoSupplierRoomType/GetRTM_ML_Suggestions/{Accomodation_SupplierRoomTypeMapping_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_SRT_ML_Response GetRTM_ML_Suggestions(string Accomodation_SupplierRoomTypeMapping_Id);

        
        #endregion

    }
}

