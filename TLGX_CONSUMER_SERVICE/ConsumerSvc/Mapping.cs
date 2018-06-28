using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;
using DataContracts.Mapping;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        #region Accomodation Product Mapping

        public bool ShiftAccommodationMappings(DataContracts.Mapping.DC_Mapping_ShiftMapping_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.ShiftAccommodationMappings(obj);
            }
        }

        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMappingById(string Accommodation_ProductMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetAccomodationProductMappingById(Accommodation_ProductMapping_Id);
            }
        }
        //public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(string PageNo, string PageSize, string Accomodation_Id, string Status)
        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetAccomodationProductMapping(obj);
            }
        }

        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetProductSupplierMappingSearch(obj);
            }
        }

        public bool UpdateAccomodationProductMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> PM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateAccomodationProductMapping(PM);
            }
        }
        #endregion

        #region Supplier Room Type Mapping
        public IList<DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping> GetAccomodationSupplierRoomTypeMapping(string PageNo, string PageSize, string Accomodation_Id, string Supplier_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetAccomodationSupplierRoomTypeMapping(PageNo, PageSize, Accomodation_Id, Supplier_Id);
            }
        }

        public bool UpdateAccomodationSupplierRoomTypeMapping(DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateAccomodationSupplierRoomTypeMapping(obj);
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> AccomodationSupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.AccomodationSupplierRoomTypeMapping_Search(obj);
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_UpdateMap(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.AccomodationSupplierRoomTypeMapping_UpdateMap(obj);
            }
        }

        public DC_Message AccomodationSupplierRoomTypeMapping_TTFUALL(List<DataContracts.Mapping.DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.AccomodationSupplierRoomTypeMapping_TTFUALL(Acco_RoomTypeMap_Ids);
            }
        }




        #endregion

        #region Country Mapping
        public IList<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(DataContracts.Mapping.DC_CountryMappingRQ RQ)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCountryMapping(RQ);
            }
        }

        public bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCountryMapping(CM);
            }
        }

        #endregion

        #region City Mapping
        public IList<DataContracts.Mapping.DC_CityMapping> GetCityMapping(DataContracts.Mapping.DC_CityMapping_RQ param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetCityMapping(param);
            }
        }

        public bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> CM)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateCityMapping(CM);
            }
        }
        #endregion

        #region Mapping Stats
        public IList<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(string SupplierID, string PriorityId, string ProductCategory)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMappingStatistics(SupplierID, PriorityId, ProductCategory);
            }
        }

        public IList<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers(string PriorityId, string ProductCategory)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMappingStatisticsForSuppliers(PriorityId, ProductCategory);
            }
        }
        #endregion

        #region roll_off_reports
        public IList<DataContracts.Mapping.DC_RollOffReportRule> getStatisticforRuleReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.getStatisticforRuleReport(param);
            }
        }
        public IList<DataContracts.Mapping.DC_RollOffReportStatus> getStatisticforStatusReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.getStatisticforStatusReport(param);
            }
        }
        public IList<DataContracts.Mapping.DC_RollOffReportUpdate> getStatisticforUpdateReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.getStatisticforUpdateReport(param);
            }
        }
        #endregion

        #region rdlc reports
        public IList<DataContracts.Mapping.DC_supplierwiseUnmappedReport> GetsupplierwiseUnmappedDataReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedDataReport(SupplierID);
            }
        }
        public IList<DataContracts.Mapping.DC_UnmappedCountryReport> GetsupplierwiseUnmappedCountryReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedCountryReport(SupplierID);
            }
        }

        public IList<DataContracts.Mapping.DC_UnmappedCityReport> GetsupplierwiseUnmappedCityReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedCityReport(SupplierID);
            }
        }
        public IList<DataContracts.Mapping.DC_unmappedProductReport> GetsupplierwiseUnmappedProductReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedProductReport(SupplierID);
            }
        }
        public IList<DataContracts.Mapping.DC_unmappedActivityReport> GetsupplierwiseUnmappedActivityReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedActivityReport(SupplierID);
            }
        }
        public IList<DataContracts.Mapping.DC_supplierwisesummaryReport> GetsupplierwiseSummaryReport()
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseSummaryReport();
            }
        }
        public IList<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> GetsupplierwiseUnmappedSummaryReport(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetsupplierwiseUnmappedSummaryReport(SupplierID);
            }
        }

        #endregion

        #region Master Attribute Mapping
        public List<DataContracts.Mapping.DC_MasterAttributeMapping_RS> SearchMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping_RQ RQ)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.SearchMasterAttributeMapping(RQ);
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMapping GetMasterAttributeMapping(string MasterAttributeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMasterAttributeMapping(MasterAttributeMapping_Id);
            }
        }

        public List<DataContracts.Mapping.DC_MasterAttributeValueMappingRS> GetMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping_RQ RQ)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMasterAttributeValueMapping(RQ);
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMappingAdd_RS AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.AddMasterAttributeMapping(param);
            }
        }


        public DataContracts.DC_Message UpdateMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateMasterAttributeMapping(param);
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(List<DataContracts.Mapping.DC_MasterAttributeValueMapping> param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateMasterAttributeValueMapping(param);
            }
        }
        public DataContracts.DC_Message DeleteMasterAttributeValueMapping(DC_SupplierAttributeValues_RQ param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.DeleteMasterAttributeValueMapping(param);
            }
        }

        #endregion

        #region Activity Mapping
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearch(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearch(obj);
            }
        }
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearchForMapping(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearchForMapping(obj);
            }
        }
        public bool IsMappedWithSupplier(string masterActivityID, string supplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.IsMappedWithSupplier(masterActivityID, supplierID);
            }
        }


        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearchForDDL(obj);
            }
        }

        public DataContracts.Mapping.DC_Acitivity_SupplierProductMapping GetActivitySupplierProductMappingById(string ActivitySupplierProductMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingById(ActivitySupplierProductMapping_Id);
            }
        }

        public IList<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMapping(string PageNo, string PageSize, string Activity_Id, string Status)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetActivitySupplierProductMapping(PageNo, PageSize, Activity_Id, Status);
            }
        }

        public DataContracts.DC_Message UpdateActivitySupplierProductMapping(List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> obj)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateActivitySupplierProductMapping(obj);
            }
        }


        #endregion
        #region hotel report
        public IList<DataContracts.Mapping.DC_newHotelsReport> getNewHotelsAddedReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.getNewHotelsAddedReport(param);
            }
        }
        #endregion
        #region velocity Dashboard
        public IList<DataContracts.Mapping.DC_VelocityMappingStats> GetVelocityDashboard(DataContracts.Mapping.DC_VelocityReport parm)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetVelocityDashboard(parm);
            }
        }
        #endregion


        #region HotelListLinkedToCityCodeAndSupplierCode
        public IList<DataContracts.Mapping.DC_HotelListByCityCode> GetHotelListByCityCode(DataContracts.Mapping.DC_HotelListByCityCode_RQ param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetHotelListByCityCode(param);
            }
        }
        #endregion

        #region -ML suggestion
        public IList<DataContracts.DC_SRT_ML_Response_Syntactic> GetRTM_ML_Suggestions_Syntactic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetRTM_ML_Suggestions_Syntactic(Accomodation_SupplierRoomTypeMapping_Id);
            }
        }
        public IList<DataContracts.DC_SRT_ML_Response_Supervised_Semantic> GetRTM_ML_Suggestions_Supervised_Semantic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetRTM_ML_Suggestions_Supervised_Semantic(Accomodation_SupplierRoomTypeMapping_Id);
            }
        }
        public IList<DataContracts.DC_SRT_ML_Response_Semantic> GetRTM_ML_Suggestions_Semantic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetRTM_ML_Suggestions_Semantic(Accomodation_SupplierRoomTypeMapping_Id);
            }
        }
        public DataContracts.DC_SRT_ML_Response GetRTM_ML_Suggestions(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetRTM_ML_Suggestions(Accomodation_SupplierRoomTypeMapping_Id);
            }
        }

        #endregion

        
    }
}