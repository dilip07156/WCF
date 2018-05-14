using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;
using DataLayer;

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

        public string[] GetMappingHotelDataForTTFU(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objDL = new DataLayer.DL_Mapping())
            {
                return objDL.GetMappingHotelDataForTTFU(obj);
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

        //public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(string PageNo, string PageSize, string Accomodation_Id, string Status)
        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            int iPageNo;
            int iPageSize;
            Guid gAccomodation_Id;

            //if (!int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize) || !Guid.TryParse(Accomodation_Id, out gAccomodation_Id))
            //{
            //    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            //}
            //else
            //{
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetAccomodationProductMapping(obj);
            }
            //}

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

        //public List<DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DC_MappingMatch obj)
        public bool UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateHotelMappingStatus(obj);
            }
        }

        public bool AddSTGMappingTableIDs(DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.AddSTGMappingTableIDs(obj);
            }
        }
        public bool HotelMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.HotelMappingMatch(obj);
            }
        }

        public int GetSTGMappingIDTableCount(DC_SupplierImportFileDetails file)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetSTGMappingIDTableCount(file);
            }
        }

        public bool HotelTTFUTelephone(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.HotelTTFUTelephone(obj);
            }
        }
        public List<DC_Accomodation_ProductMapping> GetMappingHotelData(DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMappingHotelData(obj);
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

        public void DataHandler_RoomName_Attributes_Update(DC_SupplierRoomName_Details SRNDetails)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                objDL.DataHandler_RoomName_Attributes_Update(SRNDetails);
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> AccomodationSupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.AccomodationSupplierRoomTypeMapping_Search(obj);
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_UpdateMap(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.AccomodationSupplierRoomTypeMapping_UpdateMap(obj);
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_TTFUALL(List<DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.AccomodationSupplierRoomTypeMapping_TTFUALL(Acco_RoomTypeMap_Ids);
            }
        }



        public bool RoomTypeMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.RoomTypeMappingMatch(obj);
            }
        }

        public bool CountryMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.CountryMappingMatch(obj);
            }
        }

        public List<DC_SupplierRoomType_TTFU_RQ> GetRoomTypeMapping_For_TTFU(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetRoomTypeMapping_For_TTFU(obj);
            }
        }

        public bool UpdateRoomTypeMappingStatus(DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateRoomTypeMappingStatus(obj);
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

        //public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        public bool UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateCityMappingStatus(obj);
            }
        }

        public bool CityMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.CityMappingMatch(obj);
            }
        }
        #endregion

        #region Mapping Stats
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(string SupplierID, string PriorityId, string ProductCategory)
        {
            Guid gSupplier_Id;
            int iPriorityId;
            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            if (!int.TryParse(PriorityId, out iPriorityId))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMappingStatistics(gSupplier_Id, iPriorityId, ProductCategory);
            }

        }

        public bool DeleteSTGMappingTableIDs(string file_Id)
        {
            Guid File_Id = new Guid();

            if (Guid.TryParse(file_Id, out File_Id))
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.DeleteSTGMappingTableIDs(File_Id);
                }
            }
            else
                return false;
        }

        public List<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers(string PriorityId, string ProductCategory)
        {
            int iPriorityId;

            if (!int.TryParse(PriorityId, out iPriorityId))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMappingStatisticsForSuppliers(iPriorityId, ProductCategory);
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
        public List<DataContracts.Mapping.DC_RollOffReportUpdate> getStatisticforUpdateReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.getStatisticforUpdateReport(param);
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
        public List<DataContracts.Mapping.DC_supplierwisesummaryReport> GetsupplierwiseSummaryReport()
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetsupplierwiseSummaryReport();
            }

        }
        public List<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> GetsupplierwiseUnmappedSummaryReport(string SupplierID)
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
                    return objBL.GetsupplierwiseUnmappedSummaryReport(gSupplier_Id);
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

        public List<DataContracts.Mapping.DC_MasterAttributeValueMappingRS> GetMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping_RQ RQ)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMasterAttributeValueMapping(RQ);
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMappingAdd_RS AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
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

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(List<DataContracts.Mapping.DC_MasterAttributeValueMapping> param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.UpdateMasterAttributeValueMapping(param);
            }
        }
        public DataContracts.DC_Message DeleteMasterAttributeValueMapping(DC_SupplierAttributeValues_RQ param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.DeleteMasterAttributeValueMapping(param);
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
        #region hotel report
        public List<DataContracts.Mapping.DC_newHotelsReport> getNewHotelsAddedReport(DataContracts.Mapping.DC_RollOFParams param)
        {
            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.getNewHotelsAddedReport(param);
            }
        }
        #endregion
        #region velocity Dashboard
        public List<DataContracts.Mapping.DC_VelocityMappingStats> GetVelocityDashboard(DataContracts.Mapping.DC_VelocityReport parm)
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetVelocityDashboard(parm);
            }

        }
        #endregion

        #region HotelListLinkedToCityCodeAndSupplierCode
        public IList<DataContracts.Mapping.DC_HotelListByCityCode> GetHotelListByCityCode(DataContracts.Mapping.DC_HotelListByCityCode_RQ param)
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetHotelListByCityCode(param);
            }
        }
        #endregion

        #region - ML Suggestion
        public IList<DataContracts.DC_SRT_ML_Response_Syntactic> GetRTM_ML_Suggestions_Syntactic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.GetRTM_ML_Suggestions_Syntactic(Guid.Parse(Accomodation_SupplierRoomTypeMapping_Id));
            }
        }
        public IList<DataContracts.DC_SRT_ML_Response_Semantic> GetRTM_ML_Suggestions_Semantic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.GetRTM_ML_Suggestions_Semantic(Guid.Parse(Accomodation_SupplierRoomTypeMapping_Id));
            }
        }
        public IList<DataContracts.DC_SRT_ML_Response_Supervised_Semantic> GetRTM_ML_Suggestions_Supervised_Semantic(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.GetRTM_ML_Suggestions_Supervised_Semantic(Guid.Parse(Accomodation_SupplierRoomTypeMapping_Id));
            }
        }
        public DataContracts.DC_SRT_ML_Response GetRTM_ML_Suggestions(string Accomodation_SupplierRoomTypeMapping_Id)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.GetRTM_ML_Suggestions(Guid.Parse(Accomodation_SupplierRoomTypeMapping_Id));
            }
        }
        #endregion
    }
}
