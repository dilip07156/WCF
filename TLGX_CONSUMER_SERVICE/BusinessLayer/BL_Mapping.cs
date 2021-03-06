﻿using System;
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

        public DataContracts.DC_Message Accomodation_Room_TTFUALL(string Accommodation_Room_Id)
        {
            if(Guid.TryParse(Accommodation_Room_Id, out Guid gAccommodation_Room_Id))
            {
                using (DL_Mapping objDL = new DL_Mapping())
                {
                    return objDL.Accomodation_Room_TTFUALL(gAccommodation_Room_Id);
                }
            }
            else
            {
                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning, StatusMessage = "Invalid Room Id" };
            }
        }

        public DataContracts.DC_Message UpdateAccomodationSupplierRoomTypeMappingValues(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMapping_Values> obj)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.UpdateAccomodationSupplierRoomTypeMappingValues(obj);
            }
        }

        public DataContracts.DC_Message UpdateAccomodationSupplierRoomTypeMapping_TrainingFlag(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            using (DL_Mapping objDL = new DL_Mapping())
            {
                return objDL.UpdateAccomodationSupplierRoomTypeMapping_TrainingFlag(obj);
            }
        }

        public IList<DataContracts.Mapping.DC_SupplierRoomTypeAttributes> GetAttributesForAccomodationSupplierRoomTypeMapping(string SupplierRoomtypeMappingID)
        {
            Guid gSupplierRoomtypeMappingID;

            if (!Guid.TryParse(SupplierRoomtypeMappingID, out gSupplierRoomtypeMappingID))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            else
            {
                using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
                {
                    return objBL.GetAttributesForAccomodationSupplierRoomTypeMapping(gSupplierRoomtypeMappingID);
                }
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


        public bool UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
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
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(string SupplierID, string PriorityId, string ProductCategory, string isMDM)
        {
            Guid gSupplier_Id;
            int iPriorityId;
            bool isMDMNew;
            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            if (!int.TryParse(PriorityId, out iPriorityId))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            if (!bool.TryParse(isMDM, out isMDMNew))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetMappingStatistics(gSupplier_Id, iPriorityId, ProductCategory, isMDMNew);
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

        #region Supplier Data Export
        public List<DataContracts.Mapping.DC_SupplierExportDataReport> GetSupplierDataForExport(string AccoPriority, string SupplierID, string IsMdmDataOnly, string SuppPriority)
        {
            int accoPriority;
            if (!int.TryParse(AccoPriority, out accoPriority))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }
            Guid gSupplier_Id;
            if (!Guid.TryParse(SupplierID, out gSupplier_Id))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            bool bIsMdmDataOnly;
            if (!bool.TryParse(IsMdmDataOnly, out bIsMdmDataOnly))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            int suppPriority;
            if (!int.TryParse(SuppPriority, out suppPriority))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DL_Mapping objBL = new DL_Mapping())
            {
                return objBL.GetSupplierDataForExport(accoPriority, gSupplier_Id, bIsMdmDataOnly, suppPriority);
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


        #region NewDashBoardReport
        public List<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS> GetNewDashboardReport_CountryWise(DC_NewDashBoardReport_RQ RQ)
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetNewDashboardReport_CountryWise(RQ);
            }

        }
        public List<DataContracts.Mapping.DC_NewDashBoardReportCountry_RS> GetHotelMappingReport_CityWise(DC_NewDashBoardReport_RQ RQ)
        {

            using (DataLayer.DL_Mapping objBL = new DataLayer.DL_Mapping())
            {
                return objBL.GetHotelMappingReport_CityWise(RQ);
            }

        }
        #endregion NewDashBoardReport

        #region EzeegoHotelVs
        public List<DataContracts.Mapping.DC_EzeegoHotelVsSupplierHotelMappingReport> EzeegoHotelVsSupplierHotelMappingReport(DataContracts.Mapping.DC_EzeegoHotelVsSupplierHotelMappingReport_RQ RQ)
        {
            //DC_EzeegoHotelVsSupplierHotelMappingReport_RQ rq = new DC_EzeegoHotelVsSupplierHotelMappingReport_RQ();
            //rq.Region = RQ.Select();
            using (DL_Mapping objBL = new DL_Mapping())
            {
                return objBL.EzeegoHotelVsSupplierHotelMappingReport(RQ);
            }
        }
        #endregion

        //GAURAV-TMAP-645
        public IList<DataContracts.Mapping.DC_SupplierAccoMappingExportDataReport> AccomodationMappingReport(DC_SupplerVSupplier_Report_RQ dC_SupplerVSupplier_Report_RQ)
        {

            using (DL_Mapping objBL = new DL_Mapping())
            {
                return objBL.AccomodationMappingReport(dC_SupplerVSupplier_Report_RQ);
            }
        }

        #region HotelMapping
        public List<DataContracts.Mapping.DC_HotelMappingReport_RS> HotelMappingReport(DataContracts.Mapping.DC_EzeegoHotelVsSupplierHotelMappingReport_RQ RQ)
        {
            using (DL_Mapping objBL = new DL_Mapping())
            {
                return objBL.HotelMappingReport(RQ);
            }
        }
        #endregion

        #region Reset Supplier Room Type Mapping        
        //GAURAV_TMAP_746
        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_Reset(List<DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            using (DL_Mapping objBL = new DL_Mapping())
            {
                return objBL.AccomodationSupplierRoomTypeMapping_Reset(Acco_RoomTypeMap_Ids);
            }
        }
        #endregion  
    }
}
