using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System;
using DataContracts;

namespace OperationContracts
{
    [ServiceContract]
    public interface IStaticDataHandler
    {
        #region AddFileDetails
        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/MongoFileDetails/InsertAndGet/{SupplierId}/{EntityId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.UploadStaticData.DC_SupplierImportFileDetails InsertAndGetMongoFileDetails(string SupplierId, string EntityId);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/UploadStaticData/FileDetails/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj);

        #endregion AddFileDetails

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

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Statistics/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message AddStaticDataUploadStatistics(DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/Statistics/Delete/{File_Id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteSTGMappingTableIDs(string File_Id);

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

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/STG/RecordCount/Get/{SupplierImportFile_Id}/{Entity}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int Get_STG_Record_Count(string SupplierImportFile_Id, string Entity);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/STG/Cleanup/{SupplierImportFile_Id}/{Entity}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message STG_Cleanup(string SupplierImportFile_Id, string Entity);

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
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Country/Match", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool CountryMappingMatch(DataContracts.Masters.DC_Supplier obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Country/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);


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

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/GetSTGMappingTableIDCount", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int GetSTGMappingIDTableCount(DataContracts.UploadStaticData.DC_SupplierImportFileDetails file);


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetMappingHotelData(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/GetForTTFU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string[] GetMappingHotelDataForTTFU(DataContracts.Masters.DC_Supplier obj);

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
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/TTFU_Telephone", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool HotelTTFUTelephone(DataContracts.Masters.DC_Supplier sup);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/Hotel/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateHotelMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);
        //List<DataContracts.Mapping.DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/RoomType/Match", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool RoomTypeMappingMatch(DataContracts.Masters.DC_Supplier sup);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/RoomType/GetForTTFU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_SupplierRoomType_TTFU_RQ> GetRoomTypeMapping_For_TTFU(DataContracts.Masters.DC_Supplier obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/RoomType/UpdateStatus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool UpdateRoomTypeMappingStatus(DataContracts.Mapping.DC_MappingMatch obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/AddSTGMappingTable", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddSTGMappingTableIDs(DataContracts.Mapping.DC_MappingMatch obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Mapping/RoomType/Insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMapping_Online> RoomTypeMappingOnline_Insert(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMapping_Online> obj);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "DataHandler/Mapping/DeDupe/{SupplierImportFile_Id}/{Entity}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message DeDupe_EntityMapping_FromSTG(string SupplierImportFile_Id, string Entity);

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


        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "DataHandler/Keyword/Apply", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DataContracts.DC_Message ApplyKeyword(DataContracts.Masters.DC_keywordApply_RQ RQ);
        #endregion

        #region Task Checker Activities

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "SchedulerServices/TaskCheker/GetUnprocessed", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_UnprocessedData> getTaskGeneratorFiles();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "SchedulerServices/TaskExecuter/GetExecutableData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_UnprocessedExecuterData> getExecutableTasks();

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "GET", UriTemplate = "SchedulerServices/TaskLogger/GetLoggerData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_LoggerData> getLoggerTasks();

        #endregion

        #region CRUD Declarations Supplier_Scheduled_Task

        /// <summary>
        /// Comment for All Links inside the given CRUD Ops Region
        /// </summary>
        /// <param name="C reate">  SchedulerServices/ScheduledTask/Add    </param>
        /// <param name="R ead"  >  SchedulerServices/ScheduledTask/Get    </param>
        /// <param name="U pdate">  SchedulerServices/ScheduledTask/Update </param>
        /// <param name="D elete">  SchedulerServices/ScheduledTask/Delete </param>
        /// <returns></returns>

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Add_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_SchedulerServicesTasks> Get_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Update_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Delete_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        #endregion

        #region CRUD Declarations Supplier_Scheduled_Task_Log

        /// <summary>
        /// Comment for All Links inside the given CRUD Ops Region
        /// </summary>
        /// <param name="C reate">  SchedulerServices/ScheduledLog/Add    </param>
        /// <param name="R ead"  >  SchedulerServices/ScheduledLog/Get    </param>
        /// <param name="U pdate">  SchedulerServices/ScheduledLog/Update </param>
        /// <param name="D elete">  SchedulerServices/ScheduledLog/Delete </param>
        /// <returns></returns>

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Add_Scheduled_Tasklog(DC_SchedulerServicesLogs RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_SchedulerServicesLogs> Get_Scheduled_Logs(DC_SchedulerServicesLogs RqDC_SchedulerServices);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Update_Scheduled_Logs(DC_SchedulerServicesLogs RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Delete_Scheduled_Logs(DC_SchedulerServicesLogs RQ);

        #endregion
    }
}
