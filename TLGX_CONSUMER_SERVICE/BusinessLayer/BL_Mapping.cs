using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;

namespace BusinessLayer
{
    public class BL_Mapping : IDisposable
    {
        public void Dispose()
        {
        }

        #region Accomodation Product Mapping

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMappingById(string Accommodation_ProductMapping_Id)
        {
            Guid gAccomodation_Id;

            if (!Guid.TryParse(Accommodation_ProductMapping_Id, out gAccomodation_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetAccomodationProductMappingById(gAccomodation_Id);
                }
            }

        }

        public bool ShiftAccommodationMappings(DataContracts.Mapping.DC_Mapping_ShiftMapping_RQ obj)
        {
            Guid gAccommodation_From_Id;
            Guid gAccommodation_To_Id;


            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.ShiftAccommodationMappings(obj);
            }

        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(string PageNo, string PageSize, string Accomodation_Id, string Status)
        {
            int iPageNo;
            int iPageSize;
            Guid gAccomodation_Id;

            if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize) || !Guid.TryParse(Accomodation_Id, out gAccomodation_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetAccomodationProductMapping(iPageNo, iPageSize, gAccomodation_Id, Status);
                }
            }

        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetProductSupplierMappingSearch(obj);
            }
        }

        public bool UpdateAccomodationProductMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> PM)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateAccomodationProductMapping(PM);
            }
        }

        public List<DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateHotelMappingStatus(obj);
            }
        }
        #endregion

        #region Supplier Room Type Mapping
        public IList<DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping> GetAccomodationSupplierRoomTypeMapping(string PageNo, string PageSize, string Accomodation_Id, string Supplier_Id)
        {
            int iPageNo;
            int iPageSize;
            Guid gAccomodation_Id;
            Guid gSupplier_Id;

            if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize) || !Guid.TryParse(Supplier_Id, out gSupplier_Id) || !Guid.TryParse(Accomodation_Id, out gAccomodation_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetAccomodationSupplierRoomTypeMapping(iPageNo, iPageSize, gAccomodation_Id, gSupplier_Id);
                }
            }
        }

        public bool UpdateAccomodationSupplierRoomTypeMapping(DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateAccomodationSupplierRoomTypeMapping(obj);
            }
        }


        #endregion

        #region Country Mapping
        public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(DataContracts.Mapping.DC_CountryMappingRQ RQ)
        {
            //int iPageNo;
            //int iPageSize;
            //Guid gSupplier_Id;
            //Guid gCountry_Id;

            if (RQ.PageSize <= 0)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetCountryMapping(RQ);
                }
            }
        }

        public bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> CM)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateCountryMapping(CM);
            }
        }

        public List<DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateCountryMappingStatus(obj);
            }
        }


        #endregion

        #region City Mapping
        public List<DataContracts.Mapping.DC_CityMapping> GetCityMapping(DataContracts.Mapping.DC_CityMapping_RQ param)
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetCityMapping(param);
            }
        }


        public bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> CM)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateCityMapping(CM);
            }
        }
        public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateCityMappingStatus(obj);
            }
        }
        #endregion

        #region Mapping Stats
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetMappingStatistics(gSupplier_Id);
                }
            }
        }


        public List<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers()
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMappingStatisticsForSuppliers();
            }
        }
        #endregion
        #region roll_off_reports
        public List<DataContracts.Mapping.DC_RollOffReportRule> getStatisticforRuleReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.getStatisticforRuleReport(param);
            }
        }
        public List<DataContracts.Mapping.DC_RollOffReportStatus> getStatisticforStatusReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.getStatisticforStatusReport(param);
            }
        }

        #endregion
        #region rdlc reports
        public List<DataContracts.Mapping.DC_supplierwiseUnmappedReport> GetsupplierwiseUnmappedDataReport(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetsupplierwiseUnmappedDataReport(gSupplier_Id);
                }
            }
        }
        public List<DataContracts.Mapping.DC_UnmappedCountryReport> GetsupplierwiseUnmappedCountryReport(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetsupplierwiseUnmappedCountryReport(gSupplier_Id);
                }
            }
        }
        public List<DataContracts.Mapping.DC_UnmappedCityReport> GetsupplierwiseUnmappedCityReport(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetsupplierwiseUnmappedCityReport(gSupplier_Id);
                }
            }
        }
        public List<DataContracts.Mapping.DC_unmappedProductReport> GetsupplierwiseUnmappedProductReport(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetsupplierwiseUnmappedProductReport(gSupplier_Id);
                }
            }
        }
        public List<DataContracts.Mapping.DC_unmappedActivityReport> GetsupplierwiseUnmappedActivityReport(string SupplierID)
        {
            Guid gSupplier_Id;

            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetsupplierwiseUnmappedActivityReport(gSupplier_Id);
                }
            }
        }
        #endregion
        #region Master Attribute Mapping
        public List<DataContracts.Mapping.DC_MasterAttributeMapping_RS> SearchMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping_RQ RQ)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.SearchMasterAttributeMapping(RQ);
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMapping GetMasterAttributeMapping(string MasterAttributeMapping_Id)
        {
            Guid gMasterAttributeMapping_Id;

            if (!Guid.TryParse(MasterAttributeMapping_Id, out gMasterAttributeMapping_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetMasterAttributeMapping(gMasterAttributeMapping_Id);
                }
            }
        }

        public List<DataContracts.Mapping.DC_MasterAttributeValueMapping> GetMasterAttributeValueMapping(string MasterAttributeMapping_Id)
        {
            Guid gMasterAttributeMapping_Id;

            if (!Guid.TryParse(MasterAttributeMapping_Id, out gMasterAttributeMapping_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetMasterAttributeValueMapping(gMasterAttributeMapping_Id);
                }
            }
        }

        public DataContracts.DC_Message AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.AddMasterAttributeMapping(param);
            }
        }


        public DataContracts.DC_Message UpdateMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateMasterAttributeMapping(param);
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateMasterAttributeValueMapping(param);
            }
        }

        #endregion
        #region Activity Mapping
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearch(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearch(obj);
            }
        }
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearchForMapping(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearchForMapping(obj);
            }

        }
        public bool IsMappedWithSupplier(string masterActivityID, string supplierID)
        {
            Guid gmasterActivityID;
            Guid gsupplierID;

            if (!Guid.TryParse(masterActivityID, out gmasterActivityID) || !Guid.TryParse(supplierID, out gsupplierID))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.IsMappedWithSupplier(gmasterActivityID, gsupplierID);
                }
            }
        }
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetActivitySupplierProductMappingSearchForDDL(obj);
            }
        }
        public DataContracts.Mapping.DC_Acitivity_SupplierProductMapping GetActivitySupplierProductMappingById(String ActivitySupplierProductMapping_Id)
        {
            Guid gMasterAttributeMapping_Id;

            if (!Guid.TryParse(ActivitySupplierProductMapping_Id, out gMasterAttributeMapping_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetActivitySupplierProductMappingById(gMasterAttributeMapping_Id);
                }
            }
        }
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMapping(string PageNo, string PageSize, string Activity_Id, string Status)
        {
            Guid gMasterActivity_Id;

            if (!Guid.TryParse(Activity_Id, out gMasterActivity_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetActivitySupplierProductMapping(Convert.ToInt32(PageNo), Convert.ToInt32(PageSize), gMasterActivity_Id, Status);
                }
            }
        }
        public DataContracts.DC_Message UpdateActivitySupplierProductMapping(List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateActivitySupplierProductMapping(obj);
            }
        }




        #endregion
    }
}
