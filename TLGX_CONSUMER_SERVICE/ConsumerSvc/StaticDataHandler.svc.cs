using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataContracts;
using DataContracts.UploadStaticData;
using OperationContracts;
using BusinessLayer;
using DataContracts.STG;
using DataContracts.Masters;
using DataContracts.Mapping;

namespace ConsumerSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StaticDataHandler" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select StaticDataHandler.svc or StaticDataHandler.svc.cs at the Solution Explorer and start debugging.
    public class StaticDataHandler : IStaticDataHandler
    {
        public List<DC_SupplierImportFileDetails> GetStaticDataFileDetail(DC_SupplierImportFileDetails_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataFileDetail(obj);
            }
        }

        public List<DC_SupplierImportAttributes> GetStaticDataMappingAttributes(DC_SupplierImportAttributes_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributes(obj);
            }
        }

        public List<DC_SupplierImportAttributeValues> GetStaticDataMappingAttributeValues(DC_SupplierImportAttributeValues_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetStaticDataMappingAttributeValues(obj);
            }
        }

        public DC_Message UpdateStaticDataFileDetailStatus(DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.UpdateStaticDataFileDetailStatus(obj);
            }
        }

        public DC_Message AddStaticDataUploadErrorLog(DC_SupplierImportFile_ErrorLog obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadErrorLog(obj);
            }
        }

        public DC_Message AddSTGCountryData(List<DC_stg_SupplierCountryMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGCountryData(obj);
            }
        }

        public IList<DC_Supplier> GetSupplier(DC_Supplier_Search_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.GetSupplier(RQ);
            }
        }

        public DC_Message AddSTGCityData(List<DC_stg_SupplierCityMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGCityData(obj);
            }
        }

        public DC_Message AddSTGProductData(List<DC_stg_SupplierProductMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGProductData(obj);
            }
        }

        public List<DC_stg_SupplierCountryMapping> GetSTGCountryData(DC_stg_SupplierCountryMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGCountryData(obj);
            }
        }

        public List<DC_CountryMapping> GetMappingCountryData(DC_CountryMappingRQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCountryMapping(obj);
            }
        }

        public bool UpdateCountryMapping(List<DC_CountryMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCountryMapping(CM);
            }
        }

        public bool UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCountryMappingStatus(obj);
            }
        }

        public List<DC_stg_SupplierCityMapping> GetSTGCityData(DC_stg_SupplierCityMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGCityData(obj);
            }
        }

        public List<DC_CityMapping> GetMappingCityData(DC_CityMapping_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCityMapping(obj);
            }
        }

        public bool UpdateCityMapping(List<DC_CityMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCityMapping(CM);
            }
        }

        public bool UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCityMappingStatus(obj);
            }
        }

        public List<DC_stg_SupplierProductMapping> GetSTGHotelData(DC_stg_SupplierProductMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGHotelData(obj);
            }
        }

        public List<DC_Accomodation_ProductMapping> GetMappingHotelData(DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                //return objBL.GetProductSupplierMappingSearch(obj);
                return objBL.GetMappingHotelData(obj);
            }
        }

        public bool UpdateHotelMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateAccomodationProductMapping(CM);
            }
        }

        public bool UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateHotelMappingStatus(obj);
            }
        }

        public bool AddSTGMappingTableIDs(DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.AddSTGMappingTableIDs(obj);
            }
        }

        public bool UpdateRoomTypeMappingStatus(DC_MappingMatch obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateRoomTypeMappingStatus(obj);
            }
        }

        public bool HotelMappingMatch(DC_Supplier sup)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.HotelMappingMatch(sup);
            }
        }

        public int GetSTGMappingIDTableCount(DC_SupplierImportFileDetails file)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetSTGMappingIDTableCount(file);
            }
        }

        public bool HotelTTFUTelephone(DC_Supplier sup)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.HotelTTFUTelephone(sup);
            }
        }

        public bool RoomTypeMappingMatch(DC_Supplier sup)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.RoomTypeMappingMatch(sup);
            }
        }

        public bool CountryMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.CountryMappingMatch(obj);
            }
        }

        public List<DC_SupplierRoomType_TTFU_RQ> GetRoomTypeMapping_For_TTFU(DataContracts.Masters.DC_Supplier obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetRoomTypeMapping_For_TTFU(obj);
            }
        }

        public bool CityMappingMatch(DC_Supplier sup)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.CityMappingMatch(sup);
            }
        }

        public List<DC_Keyword> DataHandler_Keyword_Get()
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                return objBL.SearchKeyword(null);
            }
        }

        public void DataHandler_Keyword_Update_NoOfHits(List<DC_keyword_alias> NoOfHits)
        {
            using (BL_Masters objBL = new BL_Masters())
            {
                objBL.DataHandler_Keyword_Update_NoOfHits(NoOfHits);
            }
        }
        
        public void DataHandler_RoomName_Attributes_Update(DC_SupplierRoomName_Details SRNDetails)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                objBL.DataHandler_RoomName_Attributes_Update(SRNDetails);
            }
        }

        public List<DC_stg_SupplierHotelRoomMapping> GetSTGRoomTypeData(DC_stg_SupplierHotelRoomMapping_RQ obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.GetSTGRoomTypeData(obj);
            }
        }

        public DC_Message AddSTGRoomTypeData(List<DC_stg_SupplierHotelRoomMapping> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddSTGRoomTypeData(obj);
            }
        }

        public DataContracts.DC_Message DataHandler_AccomodationSupplierRoomTypeMapping_TTFU(List<DataContracts.Mapping.DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            if (Acco_RoomTypeMap_Ids == null)
            {
                return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Parameter Is Null" };
            }
            else
            {
                using (BL_Mapping objBL = new BL_Mapping())
                {
                    return objBL.AccomodationSupplierRoomTypeMapping_TTFUALL(Acco_RoomTypeMap_Ids);
                }
            }
        }

        public DC_Message AddStaticDataUploadProcessLog(DC_SupplierImportFile_Progress obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadProcessLog(obj);
            }
        }

        public DC_Message AddStaticDataUploadVerboseLog(DC_SupplierImportFile_VerboseLog obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadVerboseLog(obj);
            }
        }

        public DC_Message AddStaticDataUploadStatistics(DC_SupplierImportFile_Statistics obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataUploadStatistics(obj);
            }
        }

        public bool DeleteSTGMappingTableIDs(string File_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.DeleteSTGMappingTableIDs(File_Id);
            }
        }

        public DC_Message ApplyKeyword(DC_keywordApply_RQ RQ)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.ApplyKeyword(RQ);
            }
        }

        public string[] GetMappingHotelDataForTTFU(DC_Supplier obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMappingHotelDataForTTFU(obj);
            }
        }

        public DC_SupplierImportFileDetails InsertAndGetMongoFileDetails(string SupplierId, string EntityId)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataFileDetailForMongo(SupplierId, EntityId);
            }
        }

        public List<DC_Accommodation_SupplierRoomTypeMapping_Online> RoomTypeMappingOnline_Insert(List<DC_Accommodation_SupplierRoomTypeMapping_Online> obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.RoomTypeMappingOnline_Insert(obj);
            }
        }

        public DataContracts.DC_Message AddStaticDataFileDetail(DataContracts.UploadStaticData.DC_SupplierImportFileDetails obj)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.AddStaticDataFileDetail(obj);
            }
        }

        public int Get_STG_Record_Count(string SupplierImportFile_Id, string Entity)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.Get_STG_Record_Count(SupplierImportFile_Id, Entity);
            }
        }

        public DataContracts.DC_Message DeDupe_EntityMapping_FromSTG(string SupplierImportFile_Id, string Entity)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.DeDupe_EntityMapping_FromSTG(SupplierImportFile_Id, Entity);
            }
        }

        public DataContracts.DC_Message STG_Cleanup(string SupplierImportFile_Id, string Entity)
        {
            using (BL_UploadStaticData objBL = new BL_UploadStaticData())
            {
                return objBL.STG_Cleanup(SupplierImportFile_Id, Entity);
            }
        }
    }
}
