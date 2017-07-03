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
        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(string PageNo, string PageSize, string Accomodation_Id, string Status)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetAccomodationProductMapping(PageNo, PageSize, Accomodation_Id, Status);
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
        public IList<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(string SupplierID)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMappingStatistics(SupplierID);
            }
        }
        public IList<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers()
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMappingStatisticsForSuppliers();
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

        public List<DataContracts.Mapping.DC_MasterAttributeValueMapping> GetMasterAttributeValueMapping(string MasterAttributeMapping_Id)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.GetMasterAttributeValueMapping(MasterAttributeMapping_Id);
            }
        }

        public DataContracts.DC_Message AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
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

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping param)
        {
            using (BL_Mapping objBL = new BL_Mapping())
            {
                return objBL.UpdateMasterAttributeValueMapping(param);
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
    }
}