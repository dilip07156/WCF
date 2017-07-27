﻿using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System;

namespace OperationContracts
{
    [ServiceContract]
    public interface IStaticDataHandler
    {
        #region "Mapping Config Attributes"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Attributes/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.UploadStaticData.DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DataContracts.UploadStaticData.DC_SupplierImportAttributes_RQ obj);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/AttributeValues/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DataContracts.UploadStaticData.DC_SupplierImportAttributeValues_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/FileDetails/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.UploadStaticData.DC_SupplierImportFileDetails> GetStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/FileDetails/UpdateProcessStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message UpdateStaticDataFileDetailStatus(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj);
        #endregion

        #region "Logging"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/ErrorLog/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStaticDataUploadErrorLog(DataContracts.UploadStaticData.DC_SupplierImportFile_ErrorLog obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/FileProgress/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStaticDataUploadProcessLog(DataContracts.UploadStaticData.DC_SupplierImportFile_Progress obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/VerboseLog/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStaticDataUploadVerboseLog(DataContracts.UploadStaticData.DC_SupplierImportFile_VerboseLog obj);
        #endregion

        #region "STG Tables"

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/Country/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.STG.DC_stg_SupplierCountryMapping> GetSTGCountryData(DataContracts.STG.DC_stg_SupplierCountryMapping_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/City/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.STG.DC_stg_SupplierCityMapping> GetSTGCityData(DataContracts.STG.DC_stg_SupplierCityMapping_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/Product/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.STG.DC_stg_SupplierProductMapping> GetSTGHotelData(DataContracts.STG.DC_stg_SupplierProductMapping_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/RoomType/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> GetSTGRoomTypeData(DataContracts.STG.DC_stg_SupplierHotelRoomMapping_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/Country/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddSTGCountryData(List<DataContracts.STG.DC_stg_SupplierCountryMapping> obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/City/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddSTGCityData(List<DataContracts.STG.DC_stg_SupplierCityMapping> obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/Product/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddSTGProductData(List<DataContracts.STG.DC_stg_SupplierProductMapping> obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/STG/RoomType/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddSTGRoomTypeData(List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> obj);

        #endregion

        #region Supplier
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Supplier/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        IList<DataContracts.Masters.DC_Supplier> GetSupplier(DataContracts.Masters.DC_Supplier_Search_RQ RQ);
        #endregion

        #region "Mapping Tables"
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Country/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_CountryMapping> GetMappingCountryData(DataContracts.Mapping.DC_CountryMappingRQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Country/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> CM);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Country/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/City/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_CityMapping> GetMappingCityData(DataContracts.Mapping.DC_CityMapping_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/City/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> CM);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/City/Match", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool CityMappingMatch(DataContracts.Masters.DC_Supplier sup);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/City/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);
        //List<DataContracts.Mapping.DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);




        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetMappingHotelData(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateHotelMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> CM);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/Match", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool HotelMappingMatch(DataContracts.Masters.DC_Supplier sup);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateHotelMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);
        //List<DataContracts.Mapping.DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/RoomType/Match", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool RoomTypeMappingMatch(DataContracts.Masters.DC_Supplier sup);

        #endregion

        #region Keyword Replace and Attribute Extraction
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/Keyword/Get/AllActive", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Masters.DC_Keyword> DataHandler_Keyword_Get();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Keyword/Update/NoOfHits", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void DataHandler_Keyword_Update_NoOfHits(List<DataContracts.Masters.DC_keyword_alias> NoOfHits);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/SupplierRoomName/Attributes/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void DataHandler_RoomName_Attributes_Update(DataContracts.Mapping.DC_SupplierRoomName_Details SRNDetails);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/AccoSupplierRoomType/TTFU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DataHandler_AccomodationSupplierRoomTypeMapping_TTFU(List<DataContracts.Mapping.DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids);
        #endregion
    }
}
