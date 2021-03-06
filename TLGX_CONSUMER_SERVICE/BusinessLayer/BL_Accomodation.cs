﻿using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Accomodation : IDisposable
    {
        public void Dispose()
        { }

        #region Accomodation Search
        public List<DataContracts.DC_Accomodation> GetAccomodationList(int PageNo, int PageSize)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationList(PageNo, PageSize);
            }
        }
        public List<string> GetAccomodationNames(DataContracts.DC_Accomodation_Search_RQ RQ)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationNames(RQ);
            }
        }

        public DataContracts.DC_Accomodation GetAccomodationShortInfo(Guid Accomodation_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationShortInfo(Accomodation_Id);
            }
        }

        public DataContracts.DC_Accomodation GetAccomodationDetails(Guid Accomodation_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationDetails(Accomodation_Id);
            }
        }

        public List<DataContracts.DC_Accomodation> GetAccomodationListForMissingAttributeReports(DataContracts.DC_Accomodation_Search_RQ RQ)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationListForMissingAttributeReports(RQ);
            }
        }
        public List<DataContracts.DC_Accomodation_Search_RS> AccomodationSearch(DataContracts.DC_Accomodation_Search_RQ RQ)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AccomodationSearch(RQ);
            }
        }
        public List<DataContracts.DC_Accomodation_AutoComplete_RS> AccomodationSearchAutoComplete(DataContracts.DC_Accomodation_AutoComplete_RQ RQ)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AccomodationSearchAutoComplete(RQ);
            }
        }


        #endregion

        #region Accomodation Info
        public List<DataContracts.DC_Accomodation> GetAccomodationInfo(Guid Accomodation_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationInfo(Accomodation_Id);
            }
        }
        public List<DataContracts.DC_AccomodationBasic> GetAccomodationBasicInfo(Guid Accomodation_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationBasicInfo(Accomodation_Id);
            }
        }

        public List<DataContracts.DC_Accommodation_RoomInfo> GetRoomDetailsByWithPagging(DC_Accommodation_RoomInfo_RQ RQ)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetRoomDetailsByWithPagging(RQ);
            }

        }
        public bool UpdateAccomodationGoogleInfo(DataContracts.DC_Accomodation AccomodationInfo)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationGoogleInfo(AccomodationInfo);
            }
        }

        public bool UpdateAccomodationInfo(DataContracts.DC_Accomodation AccomodationInfo)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationInfo(AccomodationInfo);
            }
        }

        public bool AddAccomodationInfo(DataContracts.DC_Accomodation AccomodationInfo)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationInfo(AccomodationInfo);
            }
        }
        public bool UpdateAccomodationDetailStatus(DataContracts.DC_Accomodation_UpdateStatus_RQ AS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationDetailStatus(AS);
            }
        }
        #endregion

        #region Accomodation Contact
        public List<DataContracts.DC_Accommodation_Contact> GetAccomodationContacts(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationContacts(Accomodation_Id, DataKey_Id);
            }
        }

        public bool UpdateAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationContacts(AC);
            }
        }

        public bool AddAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationContacts(AC);
            }
        }

        public DataContracts.DC_Message DeleteHotelsContactInTable(DataContracts.DC_Accommodation_Contact param)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.DeleteHotelsContactInTable(param);
            }
        }
        #endregion

        #region Accomodation Descriptions
        public List<DataContracts.DC_Accommodation_Descriptions> GetAccomodationDescriptions(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationDescriptions(Accomodation_Id, DataKey_Id);
            }

        }

        public bool UpdateAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationDescriptions(AD);
            }
        }

        public bool AddAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationDescriptions(AD);
            }
        }

        public DataContracts.DC_Message DeleteHotelsDescInTable(DataContracts.DC_Accommodation_Descriptions param)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.DeleteHotelsDescInTable(param);
            }
        }
        //public List<DataContracts.DC_Accommodation_Descriptions> GetAccomodationDescriptionsByType(Guid Accomodation_Id, string Desc_Type)
        //{
        //    using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
        //    {
        //        return obj.GetAccomodationDescriptionsByType(Accomodation_Id, Desc_Type);
        //    }
        //}
        #endregion

        #region Accomodation Facilities
        public List<DataContracts.DC_Accommodation_Facility> GetAccomodationFacilities(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationFacilities(Accomodation_Id, DataKey_Id);
            }

        }

        public bool UpdateAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationFacilities(AF);
            }
        }

        public bool AddAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationFacilities(AF);
            }
        }

        //public List<DataContracts.DC_Accommodation_Facility> GetAccomodationFacilitiesByCategory(Guid Accomodation_Id, string FacilityCategory)
        //{
        //    using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
        //    {
        //        return obj.GetAccomodationFacilitiesByCategory(Accomodation_Id, FacilityCategory);
        //    }

        //}

        public DataContracts.DC_Message DeleteHotelsFacilitiesInTable(DataContracts.DC_Accommodation_Facility param)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.DeleteHotelsFacilitiesInTable(param);
            }
        }
        #endregion

        #region Accomodation HealthAndSafety
        public List<DataContracts.DC_Accommodation_HealthAndSafety> GetAccomodationHealthAndSafety(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationHealthAndSafety(Accomodation_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationHealthAndSafety(HS);
            }
        }

        public bool UpdateAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationHealthAndSafety(HS);
            }
        }
        #endregion

        #region Accomodation Hotel Updates
        public List<DataContracts.DC_Accommodation_HotelUpdates> GetAccomodationHotelUpdates(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationHotelUpdates(Accomodation_Id, DataKey_Id);
            }
        }
        public bool AddAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationHotelUpdates(HU);
            }
        }

        public bool UpdateAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationHotelUpdates(HU);
            }
        }
        #endregion

        #region Accomodation Media
        public List<DataContracts.DC_Accommodation_Media> GetAccomodationMedia(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationMedia(Accomodation_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationMedia(AM);
            }
        }

        public bool UpdateAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationMedia(AM);
            }
        }

        public bool CheckMediaPositionDuplicateforAccommodation(Guid Accomodation_Id, int mediaposition, Guid Accommodation_Media_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.CheckMediaPositionDuplicateforAccommodation(Accomodation_Id, mediaposition, Accommodation_Media_Id);
            }
        }
        #endregion

        #region Accomodation Media Attributes
        public List<DataContracts.DC_Accomodation_Media_Attributes> GetAccomodationMediaAttributes(string Accomodation_Media_Id, string DataKey_Id, string PageNo, string PageSize)
        {
            Guid gAccomodation_Media_Id;
            Guid gDataKey_Id;
            int iPageNo;
            int iPageSize;

            if (!Guid.TryParse(Accomodation_Media_Id, out gAccomodation_Media_Id) || !Guid.TryParse(DataKey_Id, out gDataKey_Id) || !int.TryParse(PageNo, out iPageNo) || !int.TryParse(PageSize, out iPageSize))
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationMediaAttributes(gAccomodation_Media_Id, gDataKey_Id, iPageNo, iPageSize);
            }
        }

        public bool AddAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            if (AM.Accomodation_Media_Id == null)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationMediaAttributes(AM);
            }
        }

        public bool UpdateAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            if (AM.Accomodation_Media_Attributes_Id == null || AM.Accomodation_Media_Id == null)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Invalid Request", ErrorStatusCode = System.Net.HttpStatusCode.BadRequest });
            }

            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationMediaAttributes(AM);
            }
        }
        #endregion

        #region Accomodation NearByPlaces
        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetAccomodationNearbyPlaces(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationNearbyPlaces(Accomodation_Id, DataKey_Id);
            }
        }
        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetNearbyPlacesDetailsWithPaging(Guid Accomodation_Id, Guid DataKey_Id, Int32 pageSize, Int32 pageindex)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetNearbyPlacesDetailsWithPaging(Accomodation_Id, DataKey_Id, pageSize, pageindex);
            }
        }

        public bool AddAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationNearbyPlaces(NP);
            }
        }

        public bool UpdateAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationNearbyPlaces(NP);
            }
        }

        public DC_Message AddUpldatePlaces(DC_GooglePlaceNearByWithAccoID objplaces)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddUpldatePlaces(objplaces);
            }
        }

        #endregion

        #region Accomodation Pax Occupancy
        public List<DataContracts.DC_Accommodation_PaxOccupancy> GetAccomodationPaxOccupancy(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationPaxOccupancy(Accomodation_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationPaxOccupancy(PO);
            }
        }

        public bool UpdateAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationPaxOccupancy(PO);
            }
        }
        #endregion

        #region Accomodation RoomInfo
        public List<DataContracts.DC_Accommodation_RoomInfo> GetAccomodationRoomInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomInfo(Accomodation_Id, DataKey_Id);
            }
        }
        public List<DataContracts.DC_Accomodation_Category_DDL> GetAccomodationRoomInfo_RoomCategory(Guid Accomodation_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomInfo_RoomCategory(Accomodation_Id);
            }

        }
        public List<DataContracts.DC_Accomodation_Category_DDL_WithExtraDetails> GetAccomodationRoomInfo_RoomCategoryWithDetails(Guid Accomodation_Id, Guid acco_SupplierRoomTypeMapping_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomInfo_RoomCategoryWithDetails(Accomodation_Id, acco_SupplierRoomTypeMapping_Id);
            }

        }
        public List<string> GetRoomCategoryMaster(DataContracts.DC_RoomCategoryMaster_RQ RC)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetRoomCategoryMaster(RC);
            }

        }

        public bool AddAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationRoomInfo(RI);
            }
        }

        public bool UpdateAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationRoomInfo(RI);
            }
        }
        public DataContracts.DC_Message CopyAccomodationInfo(DataContracts.DC_Accomodation_CopyRoomDef RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.CopyAccomodationInfo(RI);
            }
        }

        public List<DataContracts.DC_Accommodation_RoomInfo> GetAccomodationRoomInfobyRoomId(Guid Accomodation_Id, string Room_id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomInfobyRoomId(Accomodation_Id, Room_id);
            }
        }
        #endregion

        #region Accomodation Room Facilities
        public List<DataContracts.DC_Accomodation_RoomFacilities> GetAccomodationRoomFacilities(Guid Accomodation_Id, Guid Accommodation_RoomInfo_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomFacilities(Accomodation_Id, Accommodation_RoomInfo_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationRoomFacilities(RF);
            }
        }

        public bool UpdateAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationRoomFacilities(RF);
            }
        }
        #endregion

        #region Accomodation Route Info
        public List<DataContracts.DC_Accommodation_RouteInfo> GetAccomodationRouteInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRouteInfo(Accomodation_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationRouteInfo(RI);
            }
        }

        public bool UpdateAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationRouteInfo(RI);
            }
        }
        #endregion

        #region Accomodation Rule Info
        public List<DataContracts.DC_Accommodation_RuleInfo> GetAccomodationRuleInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRuleInfo(Accomodation_Id, DataKey_Id);
            }
        }

        public bool AddAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationRuleInfo(RI);
            }
        }

        public bool UpdateAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationRuleInfo(RI);
            }
        }
        #endregion

        #region Accomodation Status
        public List<DataContracts.DC_Accommodation_Status> GetAccomodationStatus(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationStatus(Accomodation_Id, DataKey_Id);
            }
        }

        public bool UpdateAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationStatus(AS);
            }
        }

        public bool AddAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationStatus(AS);
            }
        }

        public DataContracts.DC_Message DeleteHotelsStatusInTable(DataContracts.DC_Accommodation_Status param)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.DeleteHotelsStatusInTable(param);
            }
        }
        #endregion

        #region Accomodation ClassificationAttributes
        public List<DataContracts.DC_Accomodation_ClassificationAttributes> GetAccomodationClassificationAttributes(Guid Accomodation_Id, Guid DataKey_Id)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationClassificationAttributes(Accomodation_Id, DataKey_Id);
            }
        }

        public bool UpdateAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateAccomodationClassificationAttributes(CA);
            }
        }

        public bool AddAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddAccomodationClassificationAttributes(CA);
            }
        }
        #endregion

        #region UpdateAccoTxInfo
        public void UpdateAccomodationTxInfo()
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                obj.UpdateAccomodationTxInfo();
            }
        }

        #endregion

        #region KAFKA
        public bool AddLstAccomodationDescriptions(List<DataContracts.DC_Accommodation_Descriptions> AD)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstAccomodationDescriptions(AD);
            }
        }

        public bool AddLstAccomodationFacilities(List<DataContracts.DC_Accommodation_Facility> AF)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstAccomodationFacilities(AF);
            }
        }

        public bool AddLstAccomodationStatus(List<DataContracts.DC_Accommodation_Status> AS)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstAccomodationStatus(AS);
            }
        }

        public bool AddLstAccomodationContacts(List<DataContracts.DC_Accommodation_Contact> AC)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstAccomodationContacts(AC);
            }
        }

        public List<DataContracts.DC_Accommodation_RoomInfo> GetAccomodationRoomInfobyAccoID(Guid Accomodation_Id, string TLGXID = null)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.GetAccomodationRoomInfobyAccoID(Accomodation_Id, (TLGXID != null ? TLGXID.Replace("ACCOID-", "") : null));
            }
        }

        public bool AddLstAccomodationRoomInfo(List<DataContracts.DC_Accommodation_RoomInfo> RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstAccomodationRoomInfo(RI);
            }
        }

        public bool UpdateLstAccomodationRoomInfo(List<DataContracts.DC_Accommodation_RoomInfo> RI)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.UpdateLstAccomodationRoomInfo(RI);
            }
        }

        public bool AddLstRoomAmenities(List<DataContracts.DC_Accomodation_Roomamenities> RA)
        {
            using (DataLayer.DL_Accomodation obj = new DataLayer.DL_Accomodation())
            {
                return obj.AddLstRoomAmenities(RA);
            }
        }
        #endregion KAFKA
    }
}
