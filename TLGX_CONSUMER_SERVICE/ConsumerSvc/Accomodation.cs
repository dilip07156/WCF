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
        #region Acco Search
        public IList<DC_Accomodation> GetAccomodationList(string PageNo, string PageSize)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation> searchResults = new List<DC_Accomodation>();
                searchResults = objBL.GetAccomodationList(int.Parse(PageNo), int.Parse(PageSize));

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }

        public IList<string> GetAccomodationNames(DC_Accomodation_Search_RQ RQ)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<string> searchResults = new List<string>();
                searchResults = objBL.GetAccomodationNames(RQ);

                //if (searchResults.Count == 0)
                //{
                //    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                //}

                return searchResults;
            }
        }

        public DC_Accomodation GetAccomodationShortInfo(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                DC_Accomodation searchResults = new DC_Accomodation();
                searchResults = objBL.GetAccomodationShortInfo(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public DC_Accomodation GetAccomodationDetails(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                DC_Accomodation searchResults = new DC_Accomodation();
                searchResults = objBL.GetAccomodationDetails(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        
        public IList<DC_Accomodation> GetAccomodationListForMissingAttributeReports(DC_Accomodation_Search_RQ RQ)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation> searchResults = new List<DC_Accomodation>();
                searchResults = objBL.GetAccomodationListForMissingAttributeReports(RQ);

                if (searchResults.Count == 0)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }

                return searchResults;
            }
        }
        public IList<DC_Accomodation_Search_RS> AccomodationSearch(DC_Accomodation_Search_RQ Accomodation_Request)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_Search_RS> searchResults = new List<DC_Accomodation_Search_RS>();
                searchResults = objBL.AccomodationSearch(Accomodation_Request);

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public IList<DC_Accomodation_AutoComplete_RS> AccomodationSearchAutoComplete(DC_Accomodation_AutoComplete_RQ Accomodation_Request)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_AutoComplete_RS> searchResults = new List<DC_Accomodation_AutoComplete_RS>();
                searchResults = objBL.AccomodationSearchAutoComplete(Accomodation_Request);

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        
        #endregion

        #region Accomodation Info
        public List<DC_Accomodation> GetAccomodationInfo(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation> searchResults = new List<DC_Accomodation>();
                searchResults = objBL.GetAccomodationInfo(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        
        public List<DC_AccomodationBasic> GetAccomodationBasicInfo(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_AccomodationBasic> searchResults = new List<DC_AccomodationBasic>();
                searchResults = objBL.GetAccomodationBasicInfo(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public List<DataContracts.DC_Accommodation_RoomInfo> GetRoomDetailsByWithPagging(DC_Accommodation_RoomInfo_RQ RQ)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.GetRoomDetailsByWithPagging(RQ);
            }
        }
        public bool UpdateAccomodationGoogleInfo(DC_Accomodation AccomodationInfo)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                bool results = objBL.UpdateAccomodationGoogleInfo(AccomodationInfo);
                return results;
            }
        }

        public bool UpdateAccomodationInfo(DC_Accomodation AccomodationInfo)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                bool results = objBL.UpdateAccomodationInfo(AccomodationInfo);
                return results;
            }
        }

        public bool AddAccomodationInfo(DC_Accomodation AccomodationInfo)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                bool results = objBL.AddAccomodationInfo(AccomodationInfo);
                return results;
            }
        }
        #endregion

        #region Accomodation Contact
        public IList<DC_Accommodation_Contact> GetAccomodationContacts(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_Contact> searchResults = new List<DC_Accommodation_Contact>();
                searchResults = objBL.GetAccomodationContacts(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationContacts(AC);
            }
        }

        public bool AddAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationContacts(AC);
            }
        }
        #endregion

        #region Accomodation Descriptions
        public IList<DC_Accommodation_Descriptions> GetAccomodationDescriptions(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_Descriptions> searchResults = new List<DC_Accommodation_Descriptions>();
                searchResults = objBL.GetAccomodationDescriptions(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.UpdateAccomodationDescriptions(AD);
            }
        }

        public bool AddAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.AddAccomodationDescriptions(AD);
            }
        }
        #endregion

        #region Accomodation Facilities
        public IList<DC_Accommodation_Facility> GetAccomodationFacilities(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_Facility> searchResults = new List<DC_Accommodation_Facility>();
                searchResults = objBL.GetAccomodationFacilities(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationFacilities(AF);
            }
        }

        public bool AddAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationFacilities(AF);
            }
        }
        #endregion

        #region Accomodation HealthAndSafety
        public IList<DC_Accommodation_HealthAndSafety> GetAccomodationHealthAndSafety(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_HealthAndSafety> searchResults = new List<DC_Accommodation_HealthAndSafety>();
                searchResults = objBL.GetAccomodationHealthAndSafety(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationHealthAndSafety(HS);
            }
        }

        public bool UpdateAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            using (BusinessLayer.BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationHealthAndSafety(HS);
            }
        }
        #endregion

        #region Accomodation Hotel Updates
        public IList<DC_Accommodation_HotelUpdates> GetAccomodationHotelUpdates(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_HotelUpdates> searchResults = new List<DC_Accommodation_HotelUpdates>();
                searchResults = objBL.GetAccomodationHotelUpdates(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationHotelUpdates(HU);
            }
        }

        public bool UpdateAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationHotelUpdates(HU);
            }
        }
        #endregion

        #region Accomodation Media
        public IList<DC_Accommodation_Media> GetAccomodationMedia(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_Media> searchResults = new List<DC_Accommodation_Media>();
                searchResults = objBL.GetAccomodationMedia(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationMedia(AM);
            }
        }

        public bool UpdateAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationMedia(AM);
            }
        }
        public bool UpdateAccomodationDetailStatus(DataContracts.DC_Accomodation_UpdateStatus_RQ RQ)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationDetailStatus(RQ);
            }
        }

        public bool CheckMediaPositionDuplicateforAccommodation(string Accomodation_Id, string mediaposition, string Accommodation_Media_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                Guid Acco_md_id = Guid.Empty;
                if (Accommodation_Media_Id != "0")
                    Acco_md_id = Guid.Parse(Accommodation_Media_Id);

                return objBL.CheckMediaPositionDuplicateforAccommodation(Guid.Parse(Accomodation_Id), Convert.ToInt32(mediaposition), Acco_md_id);
            }
        }

        #endregion

        #region Accomodation Media Attributes
        public IList<DataContracts.DC_Accomodation_Media_Attributes> GetAccomodationMediaAttributes(string Accomodation_Media_Id, string DataKey_Id, string PageNo, string PageSize)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.GetAccomodationMediaAttributes(Accomodation_Media_Id, DataKey_Id, PageNo, PageSize);
            }
        }

        public bool AddAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.AddAccomodationMediaAttributes(AM);
            }
        }

        public bool UpdateAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.UpdateAccomodationMediaAttributes(AM);
            }
        }
        #endregion

        #region Accomodation Near By Places
        public IList<DC_Accommodation_NearbyPlaces> GetAccomodationNearbyPlaces(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_NearbyPlaces> searchResults = new List<DC_Accommodation_NearbyPlaces>();
                searchResults = objBL.GetAccomodationNearbyPlaces(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public IList<DC_Accommodation_NearbyPlaces> GetNearbyPlacesDetailsWithPaging(string Accomodation_Id, string DataKey_Id, string pageSize, string pageindex)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_NearbyPlaces> searchResults = new List<DC_Accommodation_NearbyPlaces>();
                searchResults = objBL.GetNearbyPlacesDetailsWithPaging(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id),Convert.ToInt32(pageSize),Convert.ToInt32(pageindex));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationNearbyPlaces(NP);
            }
        }

        public bool UpdateAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationNearbyPlaces(NP);
            }
        }

        public DC_Message AddUpldatePlaces(DC_GooglePlaceNearByWithAccoID objPlaces)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddUpldatePlaces(objPlaces);
            }
        }

        #endregion

        #region Accomodation Pax Occupancy
        public IList<DC_Accommodation_PaxOccupancy> GetAccomodationPaxOccupancy(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_PaxOccupancy> searchResults = new List<DC_Accommodation_PaxOccupancy>();
                searchResults = objBL.GetAccomodationPaxOccupancy(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationPaxOccupancy(PO);
            }
        }

        public bool UpdateAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationPaxOccupancy(PO);
            }
        }
        #endregion

        #region Accomodation RoomInfo
        public IList<DC_Accommodation_RoomInfo> GetAccomodationRoomInfo(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_RoomInfo> searchResults = new List<DC_Accommodation_RoomInfo>();
                searchResults = objBL.GetAccomodationRoomInfo(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public IList<DataContracts.DC_Accomodation_Category_DDL> GetAccomodationRoomInfo_RoomCategory(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_Category_DDL> searchResults = new List<DC_Accomodation_Category_DDL>();
                searchResults = objBL.GetAccomodationRoomInfo_RoomCategory(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public IList<DataContracts.DC_Accomodation_Category_DDL_WithExtraDetails> GetAccomodationRoomInfo_RoomCategoryWithDetails(string Accomodation_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_Category_DDL_WithExtraDetails> searchResults = new List<DC_Accomodation_Category_DDL_WithExtraDetails>();
                searchResults = objBL.GetAccomodationRoomInfo_RoomCategoryWithDetails(Guid.Parse(Accomodation_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }
        public IList<string> GetRoomCategoryMaster(DataContracts.DC_RoomCategoryMaster_RQ RC)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                return objBL.GetRoomCategoryMaster(RC);
            }
        }
        public bool AddAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationRoomInfo(RI);
            }
        }

        public bool UpdateAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationRoomInfo(RI);
            }
        }
        public DataContracts.DC_Message CopyAccomodationInfo(DataContracts.DC_Accomodation_CopyRoomDef RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.CopyAccomodationInfo(RI);
            }
        }
        #endregion

        #region Accomodation Room Facilities
        public IList<DC_Accomodation_RoomFacilities> GetAccomodationRoomFacilities(string Accomodation_Id, string Accommodation_RoomInfo_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_RoomFacilities> searchResults = new List<DC_Accomodation_RoomFacilities>();
                searchResults = objBL.GetAccomodationRoomFacilities(Guid.Parse(Accomodation_Id), Guid.Parse(Accommodation_RoomInfo_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationRoomFacilities(RF);
            }
        }

        public bool UpdateAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationRoomFacilities(RF);
            }
        }
        #endregion

        #region Accomodation Route Info
        public IList<DC_Accommodation_RouteInfo> GetAccomodationRouteInfo(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_RouteInfo> searchResults = new List<DC_Accommodation_RouteInfo>();
                searchResults = objBL.GetAccomodationRouteInfo(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationRouteInfo(RI);
            }
        }

        public bool UpdateAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationRouteInfo(RI);
            }
        }
        #endregion

        #region Accomodation Rule Info
        public IList<DC_Accommodation_RuleInfo> GetAccomodationRuleInfo(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_RuleInfo> searchResults = new List<DC_Accommodation_RuleInfo>();
                searchResults = objBL.GetAccomodationRuleInfo(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool AddAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationRuleInfo(RI);
            }
        }

        public bool UpdateAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationRuleInfo(RI);
            }
        }
        #endregion

        #region Accomodation Status
        public IList<DC_Accommodation_Status> GetAccomodationStatus(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accommodation_Status> searchResults = new List<DC_Accommodation_Status>();
                searchResults = objBL.GetAccomodationStatus(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationStatus(AS);
            }
        }

        public bool AddAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationStatus(AS);
            }
        }
        #endregion

        #region Accomodation Dynamic Attributes
        public IList<DataContracts.Masters.DC_DynamicAttributes> GetAccomodationDynamicAttributes(string Accomodation_Id, string SubObject_Id, string DataKey_Id)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                List<DataContracts.Masters.DC_DynamicAttributes> searchResults = new List<DataContracts.Masters.DC_DynamicAttributes>();

                DataContracts.Masters.DC_DynamicAttributes RQ = new DataContracts.Masters.DC_DynamicAttributes();
                RQ.DynamicAttribute_Id = Guid.Parse(DataKey_Id);
                RQ.ObjectSubElement_Id = Guid.Parse(SubObject_Id);
                RQ.Object_Id = Guid.Parse(Accomodation_Id);

                return obj.GetDynamicAttributes(RQ);
            }
        }

        public bool UpdateAccomodationDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes DA)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.UpdateDynamicAttributes(DA);
            }
        }

        public bool AddAccomodationDynamicAttributes(DataContracts.Masters.DC_DynamicAttributes DA)
        {
            using (BusinessLayer.BL_Masters obj = new BL_Masters())
            {
                return obj.AddDynamicAttributes(DA);
            }
        }
        #endregion

        #region Accomodation ClassificationAttributes
        public IList<DataContracts.DC_Accomodation_ClassificationAttributes> GetAccomodationClassificationAttributes(string Accomodation_Id, string DataKey_Id)
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                List<DC_Accomodation_ClassificationAttributes> searchResults = new List<DC_Accomodation_ClassificationAttributes>();
                searchResults = objBL.GetAccomodationClassificationAttributes(Guid.Parse(Accomodation_Id), Guid.Parse(DataKey_Id));

                if (searchResults == null)
                {
                    throw new WebFaultException<string>("No records found.", System.Net.HttpStatusCode.NoContent);
                }
                return searchResults;
            }
        }

        public bool UpdateAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.UpdateAccomodationClassificationAttributes(CA);
            }
        }

        public bool AddAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            using (BL_Accomodation obj = new BL_Accomodation())
            {
                return obj.AddAccomodationClassificationAttributes(CA);
            }
        }
        #endregion

        #region UpdateAccoTxInfo
        public void UpdateAccomodationTxInfo()
        {
            using (BL_Accomodation objBL = new BL_Accomodation())
            {
                objBL.UpdateAccomodationTxInfo();
            }
        }

        #endregion
    }
}