using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts
{
    [DataContract]
    public class DC_Accomodation
    {
        System.Guid _Accommodation_Id;
        string _CompanyName;
        int? _CompanyHotelID;
        int? _FinanceControlID;
        System.DateTime? _OnlineDate;
        System.DateTime? _OfflineDate;
        string _HotelRating;
        string _CompanyRating;
        string _HotelName;
        string _DisplayName;
        string _ProductCategory;
        string _ProductCategorySubType;
        System.DateTime? _RatingDate;
        string _TotalFloors;
        string _TotalRooms;
        string _CheckInTime;
        string _CheckOutTime;
        string _InternalRemarks;
        bool _CompanyRecommended;
        string _RecommendedFor;
        string _Hashtag;
        string _Chain;
        string _Brand;
        string _Affiliation;
        string _Reason;
        string _Remarks;
        string _CarbonFootPrint;
        string _YearBuilt;
        string _AwardsReceived;
        bool? _IsActive;
        bool _IsMysteryProduct;
        string _Create_User;
        System.DateTime? _Create_Date;
        string _Edit_User;
        System.DateTime? _Edit_Date;
        string _StreetName;
        string _StreetNumber;
        string _Street3;
        string _Street4;
        string _Street5;
        string _PostalCode;
        string _Town;
        string _Location;
        string _Area;
        string _city;
        string _country;
        string _SuburbDowntown;
        string _Latitude;
        string _Longitude;
        string _LEGACY_COUNTRY;
        string _LEGACY_CITY;
        string _Country_ISO;
        string _City_ISO;
        string _State_Name;
        string _State_ISO;
        string _LEGACY_STATE;
        int? _Legacy_HTL_ID;
        string _Google_Place_Id;
        string _fullAddress;
        string _Telephone_Tx;
        int _TotalRecords;
        List<DC_Accommodation_Contact> _Accomodation_Contact;
        List<DC_Accommodation_Descriptions> _Accommodation_Descriptions;
        List<DC_Accommodation_HotelUpdates> _Accommodation_HotelUpdates;
        List<DC_Accommodation_NearbyPlaces> _Accommodation_NearbyPlaces;
        List<DC_Accommodation_RuleInfo> _Accommodation_RuleInfo;
        List<DC_Accommodation_Facility> _Accommodation_Facility;
        List<DC_Accommodation_HealthAndSafety> _Accommodation_HealthAndSafety;
        List<DC_Accommodation_Media> _Accommodation_Media;
        List<DC_Accommodation_RoomInfo> _Accommodation_RoomInfo;
        List<DC_Accommodation_PaxOccupancy> _Accommodation_PaxOccupancy;
        List<DC_Accommodation_RouteInfo> _Accommodation_RouteInfo;
        List<DC_Accommodation_Status> _Accomodation_Status;
        List<Masters.DC_DynamicAttributes> _Accomodation_DynamicAttributes;
        List<DC_Accomodation_ClassificationAttributes> _Accomodation_ClassificationAttributes;
        int? _TotalRecords_Accomodation_Contact;
        int? _TotalRecords_Accommodation_Descriptions;
        int? _TotalRecords_Accommodation_HotelUpdates;
        int? _TotalRecords_Accommodation_NearbyPlaces;
        int? _TotalRecords_Accommodation_RuleInfo;
        int? _TotalRecords_Accommodation_Facility;
        int? _TotalRecords_Accommodation_HealthAndSafety;
        int? _TotalRecords_Accommodation_Media;
        int? _TotalRecords_Accommodation_RoomInfo;
        int? _TotalRecords_Accommodation_PaxOccupancy;
        int? _TotalRecords_Accommodation_RouteInfo;
        int? _TotalRecords_Accomodation_Status;
        int? _TotalRecords_Accomodation_ClassificationAttributes;
        string _Room_Amenities;
        string _Null_Columns;


        [DataMember]
        public string Latitude_Tx { get; set; }


        [DataMember]
        public string Longitude_Tx { get; set; }

        [DataMember]
        public Nullable<bool> InsertFrom { get; set; }

        [DataMember]
        public Nullable<System.Guid> Country_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> City_Id { get; set; }

        [DataMember]
        public Guid Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                _CompanyName = value;
            }
        }

        [DataMember]
        public int? CompanyHotelID
        {
            get
            {
                return _CompanyHotelID;
            }

            set
            {
                _CompanyHotelID = value;
            }
        }

        [DataMember]
        public int? FinanceControlID
        {
            get
            {
                return _FinanceControlID;
            }

            set
            {
                _FinanceControlID = value;
            }
        }

        [DataMember]
        public DateTime? OnlineDate
        {
            get
            {
                return _OnlineDate;
            }

            set
            {
                _OnlineDate = value;
            }
        }

        [DataMember]
        public DateTime? OfflineDate
        {
            get
            {
                return _OfflineDate;
            }

            set
            {
                _OfflineDate = value;
            }
        }

        [DataMember]
        public string HotelRating
        {
            get
            {
                return _HotelRating;
            }

            set
            {
                _HotelRating = value;
            }
        }

        [DataMember]
        public string CompanyRating
        {
            get
            {
                return _CompanyRating;
            }

            set
            {
                _CompanyRating = value;
            }
        }

        [DataMember]
        public string HotelName
        {
            get
            {
                return _HotelName;
            }

            set
            {
                _HotelName = value;
            }
        }

        [DataMember]
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }

            set
            {
                _DisplayName = value;
            }
        }

        [DataMember]
        public string ProductCategory
        {
            get
            {
                return _ProductCategory;
            }

            set
            {
                _ProductCategory = value;
            }
        }

        [DataMember]
        public string ProductCategorySubType
        {
            get
            {
                return _ProductCategorySubType;
            }

            set
            {
                _ProductCategorySubType = value;
            }
        }

        [DataMember]
        public DateTime? RatingDate
        {
            get
            {
                return _RatingDate;
            }

            set
            {
                _RatingDate = value;
            }
        }

        [DataMember]
        public string TotalFloors
        {
            get
            {
                return _TotalFloors;
            }

            set
            {
                _TotalFloors = value;
            }
        }

        [DataMember]
        public string TotalRooms
        {
            get
            {
                return _TotalRooms;
            }

            set
            {
                _TotalRooms = value;
            }
        }

        [DataMember]
        public string CheckInTime
        {
            get
            {
                return _CheckInTime;
            }

            set
            {
                _CheckInTime = value;
            }
        }

        [DataMember]
        public string CheckOutTime
        {
            get
            {
                return _CheckOutTime;
            }

            set
            {
                _CheckOutTime = value;
            }
        }

        [DataMember]
        public string InternalRemarks
        {
            get
            {
                return _InternalRemarks;
            }

            set
            {
                _InternalRemarks = value;
            }
        }

        [DataMember]
        public bool CompanyRecommended
        {
            get
            {
                return _CompanyRecommended;
            }

            set
            {
                _CompanyRecommended = value;
            }
        }

        [DataMember]
        public string RecommendedFor
        {
            get
            {
                return _RecommendedFor;
            }

            set
            {
                _RecommendedFor = value;
            }
        }

        [DataMember]
        public string Hashtag
        {
            get
            {
                return _Hashtag;
            }

            set
            {
                _Hashtag = value;
            }
        }

        [DataMember]
        public string Chain
        {
            get
            {
                return _Chain;
            }

            set
            {
                _Chain = value;
            }
        }

        [DataMember]
        public string Brand
        {
            get
            {
                return _Brand;
            }

            set
            {
                _Brand = value;
            }
        }

        [DataMember]
        public string Affiliation
        {
            get
            {
                return _Affiliation;
            }

            set
            {
                _Affiliation = value;
            }
        }

        [DataMember]
        public string Reason
        {
            get
            {
                return _Reason;
            }

            set
            {
                _Reason = value;
            }
        }

        [DataMember]
        public string Remarks
        {
            get
            {
                return _Remarks;
            }

            set
            {
                _Remarks = value;
            }
        }

        [DataMember]
        public string CarbonFootPrint
        {
            get
            {
                return _CarbonFootPrint;
            }

            set
            {
                _CarbonFootPrint = value;
            }
        }

        [DataMember]
        public string YearBuilt
        {
            get
            {
                return _YearBuilt;
            }

            set
            {
                _YearBuilt = value;
            }
        }

        [DataMember]
        public string AwardsReceived
        {
            get
            {
                return _AwardsReceived;
            }

            set
            {
                _AwardsReceived = value;
            }
        }

        [DataMember]
        public bool? IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsMysteryProduct
        {
            get
            {
                return _IsMysteryProduct;
            }

            set
            {
                _IsMysteryProduct = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string StreetName
        {
            get
            {
                return _StreetName;
            }

            set
            {
                _StreetName = value;
            }
        }

        [DataMember]
        public string StreetNumber
        {
            get
            {
                return _StreetNumber;
            }

            set
            {
                _StreetNumber = value;
            }
        }

        [DataMember]
        public string Street3
        {
            get
            {
                return _Street3;
            }

            set
            {
                _Street3 = value;
            }
        }

        [DataMember]
        public string Street4
        {
            get
            {
                return _Street4;
            }

            set
            {
                _Street4 = value;
            }
        }

        [DataMember]
        public string Street5
        {
            get
            {
                return _Street5;
            }

            set
            {
                _Street5 = value;
            }
        }

        [DataMember]
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }

            set
            {
                _PostalCode = value;
            }
        }

        [DataMember]
        public string Town
        {
            get
            {
                return _Town;
            }

            set
            {
                _Town = value;
            }
        }

        [DataMember]
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                _Location = value;
            }
        }

        [DataMember]
        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                _city = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return _country;
            }

            set
            {
                _country = value;
            }
        }

        [DataMember]
        public string SuburbDowntown
        {
            get
            {
                return _SuburbDowntown;
            }

            set
            {
                _SuburbDowntown = value;
            }
        }

        [DataMember]
        public string Latitude
        {
            get
            {
                return _Latitude;
            }

            set
            {
                _Latitude = value;
            }
        }

        [DataMember]
        public string Longitude
        {
            get
            {
                return _Longitude;
            }

            set
            {
                _Longitude = value;
            }
        }

        [DataMember]
        public string LEGACY_COUNTRY
        {
            get
            {
                return _LEGACY_COUNTRY;
            }

            set
            {
                _LEGACY_COUNTRY = value;
            }
        }

        [DataMember]
        public string LEGACY_CITY
        {
            get
            {
                return _LEGACY_CITY;
            }

            set
            {
                _LEGACY_CITY = value;
            }
        }

        [DataMember]
        public string Country_ISO
        {
            get
            {
                return _Country_ISO;
            }

            set
            {
                _Country_ISO = value;
            }
        }

        [DataMember]
        public string City_ISO
        {
            get
            {
                return _City_ISO;
            }

            set
            {
                _City_ISO = value;
            }
        }

        [DataMember]
        public string State_Name
        {
            get
            {
                return _State_Name;
            }

            set
            {
                _State_Name = value;
            }
        }

        [DataMember]
        public string State_ISO
        {
            get
            {
                return _State_ISO;
            }

            set
            {
                _State_ISO = value;
            }
        }

        [DataMember]
        public string LEGACY_STATE
        {
            get
            {
                return _LEGACY_STATE;
            }

            set
            {
                _LEGACY_STATE = value;
            }
        }

        [DataMember]
        public int? Legacy_HTL_ID
        {
            get
            {
                return _Legacy_HTL_ID;
            }

            set
            {
                _Legacy_HTL_ID = value;
            }
        }

        [DataMember]
        public string Google_Place_Id
        {
            get
            {
                return _Google_Place_Id;
            }

            set
            {
                _Google_Place_Id = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_Contact> Accomodation_Contact
        {
            get
            {
                return _Accomodation_Contact;
            }

            set
            {
                _Accomodation_Contact = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_Descriptions> Accommodation_Descriptions
        {
            get
            {
                return _Accommodation_Descriptions;
            }

            set
            {
                _Accommodation_Descriptions = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_HotelUpdates> Accommodation_HotelUpdates
        {
            get
            {
                return _Accommodation_HotelUpdates;
            }

            set
            {
                _Accommodation_HotelUpdates = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_NearbyPlaces> Accommodation_NearbyPlaces
        {
            get
            {
                return _Accommodation_NearbyPlaces;
            }

            set
            {
                _Accommodation_NearbyPlaces = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_RuleInfo> Accommodation_RuleInfo
        {
            get
            {
                return _Accommodation_RuleInfo;
            }

            set
            {
                _Accommodation_RuleInfo = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_Facility> Accommodation_Facility
        {
            get
            {
                return _Accommodation_Facility;
            }

            set
            {
                _Accommodation_Facility = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_HealthAndSafety> Accommodation_HealthAndSafety
        {
            get
            {
                return _Accommodation_HealthAndSafety;
            }

            set
            {
                _Accommodation_HealthAndSafety = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_Media> Accommodation_Media
        {
            get
            {
                return _Accommodation_Media;
            }

            set
            {
                _Accommodation_Media = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_RoomInfo> Accommodation_RoomInfo
        {
            get
            {
                return _Accommodation_RoomInfo;
            }

            set
            {
                _Accommodation_RoomInfo = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_PaxOccupancy> Accommodation_PaxOccupancy
        {
            get
            {
                return _Accommodation_PaxOccupancy;
            }

            set
            {
                _Accommodation_PaxOccupancy = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_RouteInfo> Accommodation_RouteInfo
        {
            get
            {
                return _Accommodation_RouteInfo;
            }

            set
            {
                _Accommodation_RouteInfo = value;
            }
        }

        [DataMember]
        public List<DC_Accommodation_Status> Accomodation_Status
        {
            get
            {
                return _Accomodation_Status;
            }

            set
            {
                _Accomodation_Status = value;
            }
        }

        [DataMember]
        public List<Masters.DC_DynamicAttributes> Accomodation_DynamicAttributes
        {
            get
            {
                return _Accomodation_DynamicAttributes;
            }

            set
            {
                _Accomodation_DynamicAttributes = value;
            }
        }

        [DataMember]
        public List<DC_Accomodation_ClassificationAttributes> Accomodation_ClassificationAttributes
        {
            get
            {
                return _Accomodation_ClassificationAttributes;
            }

            set
            {
                _Accomodation_ClassificationAttributes = value;
            }
        }
        [DataMember]
        public string FullAddress
        {
            get
            {
                return _fullAddress;
            }

            set
            {
                _fullAddress = value;
            }
        }
        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accomodation_Contact
        {
            get
            {
                return _TotalRecords_Accomodation_Contact;
            }

            set
            {
                _TotalRecords_Accomodation_Contact = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_Descriptions
        {
            get
            {
                return _TotalRecords_Accommodation_Descriptions;
            }

            set
            {
                _TotalRecords_Accommodation_Descriptions = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_HotelUpdates
        {
            get
            {
                return _TotalRecords_Accommodation_HotelUpdates;
            }

            set
            {
                _TotalRecords_Accommodation_HotelUpdates = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_NearbyPlaces
        {
            get
            {
                return _TotalRecords_Accommodation_NearbyPlaces;
            }

            set
            {
                _TotalRecords_Accommodation_NearbyPlaces = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_RuleInfo
        {
            get
            {
                return _TotalRecords_Accommodation_RuleInfo;
            }

            set
            {
                _TotalRecords_Accommodation_RuleInfo = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_Facility
        {
            get
            {
                return _TotalRecords_Accommodation_Facility;
            }

            set
            {
                _TotalRecords_Accommodation_Facility = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_HealthAndSafety
        {
            get
            {
                return _TotalRecords_Accommodation_HealthAndSafety;
            }

            set
            {
                _TotalRecords_Accommodation_HealthAndSafety = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_Media
        {
            get
            {
                return _TotalRecords_Accommodation_Media;
            }

            set
            {
                _TotalRecords_Accommodation_Media = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_RoomInfo
        {
            get
            {
                return _TotalRecords_Accommodation_RoomInfo;
            }

            set
            {
                _TotalRecords_Accommodation_RoomInfo = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_PaxOccupancy
        {
            get
            {
                return _TotalRecords_Accommodation_PaxOccupancy;
            }

            set
            {
                _TotalRecords_Accommodation_PaxOccupancy = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accommodation_RouteInfo
        {
            get
            {
                return _TotalRecords_Accommodation_RouteInfo;
            }

            set
            {
                _TotalRecords_Accommodation_RouteInfo = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accomodation_Status
        {
            get
            {
                return _TotalRecords_Accomodation_Status;
            }

            set
            {
                _TotalRecords_Accomodation_Status = value;
            }
        }
        [DataMember]
        public int? TotalRecords_Accomodation_ClassificationAttributes
        {
            get
            {
                return _TotalRecords_Accomodation_ClassificationAttributes;
            }

            set
            {
                _TotalRecords_Accomodation_ClassificationAttributes = value;
            }
        }
        [DataMember]
        public string Room_Amenities
        {
            get
            {
                return _Room_Amenities;
            }

            set
            {
                _Room_Amenities = value;
            }
        }
        [DataMember]
        public string Null_Columns
        {
            get
            {
                return _Null_Columns;
            }

            set
            {
                _Null_Columns = value;
            }
        }
    }

    [DataContract]
    public class DC_AccomodationBasic
    {
        [DataMember]
        public System.Guid Accommodation_Id { get; set; }
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string StreetName { get; set; }
        [DataMember]
        public string StreetNumber { get; set; }
        [DataMember]
        public string Street3 { get; set; }
        [DataMember]
        public string Street4 { get; set; }
        [DataMember]
        public string Street5 { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Town { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public Guid? City_id { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public Guid? Country_Id { get; set; }
        [DataMember]
        public string SuburbDowntown { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string State_Name { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string Telephone_Tx { get; set; }
        [DataMember]
        public string SystemProductType { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? CompanyHotelID { get; set; }

    }

    [DataContract]
    public class DC_Accommodation_Contact
    {
        System.Guid _Accommodation_Contact_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _Telephone;
        string _Fax;
        string _WebSiteURL;
        string _Email;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;

        [DataMember]
        public Guid Accommodation_Contact_Id
        {
            get
            {
                return _Accommodation_Contact_Id;
            }

            set
            {
                _Accommodation_Contact_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string Telephone
        {
            get
            {
                return _Telephone;
            }

            set
            {
                _Telephone = value;
            }
        }

        [DataMember]
        public string Fax
        {
            get
            {
                return _Fax;
            }

            set
            {
                _Fax = value;
            }
        }

        [DataMember]
        public string WebSiteURL
        {
            get
            {
                return _WebSiteURL;
            }

            set
            {
                _WebSiteURL = value;
            }
        }

        [DataMember]
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_Descriptions
    {
        System.Guid _Accommodation_Description_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _Description;
        string _Language_Code;
        string _DescriptionType;
        Nullable<System.DateTime> _FromDate;
        Nullable<System.DateTime> _ToDate;
        string _Source;
        Nullable<bool> _IsActive;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;

        [DataMember]
        public Guid Accommodation_Description_Id
        {
            get
            {
                return _Accommodation_Description_Id;
            }

            set
            {
                _Accommodation_Description_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Language_Code
        {
            get
            {
                return _Language_Code;
            }

            set
            {
                _Language_Code = value;
            }
        }

        [DataMember]
        public string DescriptionType
        {
            get
            {
                return _DescriptionType;
            }

            set
            {
                _DescriptionType = value;
            }
        }

        [DataMember]
        public DateTime? FromDate
        {
            get
            {
                return _FromDate;
            }

            set
            {
                _FromDate = value;
            }
        }

        [DataMember]
        public DateTime? ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                _ToDate = value;
            }
        }

        [DataMember]
        public string Source
        {
            get
            {
                return _Source;
            }

            set
            {
                _Source = value;
            }
        }

        [DataMember]
        public bool? IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_HotelUpdates
    {
        System.Guid _Accommodation_HotelUpdates_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<System.DateTime> _FromDate;
        Nullable<System.DateTime> _ToDate;
        string _Description;
        string _Source;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;
        bool _IsInternal;

        [DataMember]
        public Guid Accommodation_HotelUpdates_Id
        {
            get
            {
                return _Accommodation_HotelUpdates_Id;
            }

            set
            {
                _Accommodation_HotelUpdates_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public DateTime? FromDate
        {
            get
            {
                return _FromDate;
            }

            set
            {
                _FromDate = value;
            }
        }

        [DataMember]
        public DateTime? ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                _ToDate = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Source
        {
            get
            {
                return _Source;
            }

            set
            {
                _Source = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsInternal
        {
            get
            {
                return _IsInternal;
            }

            set
            {
                _IsInternal = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_NearbyPlaces
    {
        System.Guid _Accommodation_NearbyPlace_Id;
        Nullable<System.Guid> _Accomodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _PlaceName;
        string _DistanceFromProperty;
        string _Description;
        string _PlaceCategory;
        string _DistanceUnit;
        string _PlaceType;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;
        int? _TotalRecords;

        [DataMember]
        public Guid Accommodation_NearbyPlace_Id
        {
            get
            {
                return _Accommodation_NearbyPlace_Id;
            }

            set
            {
                _Accommodation_NearbyPlace_Id = value;
            }
        }

        [DataMember]
        public Guid? Accomodation_Id
        {
            get
            {
                return _Accomodation_Id;
            }

            set
            {
                _Accomodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string PlaceName
        {
            get
            {
                return _PlaceName;
            }

            set
            {
                _PlaceName = value;
            }
        }

        [DataMember]
        public string DistanceFromProperty
        {
            get
            {
                return _DistanceFromProperty;
            }

            set
            {
                _DistanceFromProperty = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string PlaceCategory
        {
            get
            {
                return _PlaceCategory;
            }

            set
            {
                _PlaceCategory = value;
            }
        }

        [DataMember]
        public string DistanceUnit
        {
            get
            {
                return _DistanceUnit;
            }

            set
            {
                _DistanceUnit = value;
            }
        }

        [DataMember]
        public string PlaceType
        {
            get
            {
                return _PlaceType;
            }

            set
            {
                _PlaceType = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
        [DataMember]
        public int? TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_RuleInfo
    {
        System.Guid _Accommodation_RuleInfo_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _RuleType;
        string _Description;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;
        bool _IsInternal;

        [DataMember]
        public Guid Accommodation_RuleInfo_Id
        {
            get
            {
                return _Accommodation_RuleInfo_Id;
            }

            set
            {
                _Accommodation_RuleInfo_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string RuleType
        {
            get
            {
                return _RuleType;
            }

            set
            {
                _RuleType = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsInternal
        {
            get
            {
                return _IsInternal;
            }

            set
            {
                _IsInternal = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_Facility
    {
        System.Guid _Accommodation_Facility_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _FacilityCategory;
        string _FacilityType;
        string _FacilityName;
        string _Description;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;

        [DataMember]
        public Guid Accommodation_Facility_Id
        {
            get
            {
                return _Accommodation_Facility_Id;
            }

            set
            {
                _Accommodation_Facility_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string FacilityCategory
        {
            get
            {
                return _FacilityCategory;
            }

            set
            {
                _FacilityCategory = value;
            }
        }

        [DataMember]
        public string FacilityType
        {
            get
            {
                return _FacilityType;
            }

            set
            {
                _FacilityType = value;
            }
        }

        [DataMember]
        public string FacilityName
        {
            get
            {
                return _FacilityName;
            }

            set
            {
                _FacilityName = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_HealthAndSafety
    {
        System.Guid _Accommodation_HealthAndSafety_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _Name;
        string _Category;
        string _Description;
        string _Remarks;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;

        [DataMember]
        public Guid Accommodation_HealthAndSafety_Id
        {
            get
            {
                return _Accommodation_HealthAndSafety_Id;
            }

            set
            {
                _Accommodation_HealthAndSafety_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        [DataMember]
        public string Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string Remarks
        {
            get
            {
                return _Remarks;
            }

            set
            {
                _Remarks = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_Media
    {
        System.Guid _Accommodation_Media_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _MediaName;
        string _MediaType;
        string _RoomCategory;
        Nullable<System.DateTime> _ValidFrom;
        Nullable<System.DateTime> _ValidTo;
        bool _IsActive;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Media_Path;
        string _Media_URL;
        Nullable<int> _Media_Position;
        string _Category;
        string _SubCategory;
        string _Description;
        string _FileFormat;
        string _MediaID;
        string _MediaFileMaster;
        List<DC_Accomodation_Media_Attributes> _Media_Attributes;

        [DataMember]
        public Guid Accommodation_Media_Id
        {
            get
            {
                return _Accommodation_Media_Id;
            }

            set
            {
                _Accommodation_Media_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string MediaName
        {
            get
            {
                return _MediaName;
            }

            set
            {
                _MediaName = value;
            }
        }

        [DataMember]
        public string MediaType
        {
            get
            {
                return _MediaType;
            }

            set
            {
                _MediaType = value;
            }
        }

        [DataMember]
        public string RoomCategory
        {
            get
            {
                return _RoomCategory;
            }

            set
            {
                _RoomCategory = value;
            }
        }

        [DataMember]
        public DateTime? ValidFrom
        {
            get
            {
                return _ValidFrom;
            }

            set
            {
                _ValidFrom = value;
            }
        }

        [DataMember]
        public DateTime? ValidTo
        {
            get
            {
                return _ValidTo;
            }

            set
            {
                _ValidTo = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Media_Path
        {
            get
            {
                return _Media_Path;
            }

            set
            {
                _Media_Path = value;
            }
        }

        [DataMember]
        public string Media_URL
        {
            get
            {
                return _Media_URL;
            }

            set
            {
                _Media_URL = value;
            }
        }

        [DataMember]
        public int? Media_Position
        {
            get
            {
                return _Media_Position;
            }

            set
            {
                _Media_Position = value;
            }
        }

        [DataMember]
        public string Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;
            }
        }

        [DataMember]
        public string SubCategory
        {
            get
            {
                return _SubCategory;
            }

            set
            {
                _SubCategory = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string FileFormat
        {
            get
            {
                return _FileFormat;
            }

            set
            {
                _FileFormat = value;
            }
        }

        [DataMember]
        public string MediaID
        {
            get
            {
                return _MediaID;
            }

            set
            {
                _MediaID = value;
            }
        }

        [DataMember]
        public List<DC_Accomodation_Media_Attributes> Media_Attributes
        {
            get
            {
                return _Media_Attributes;
            }

            set
            {
                _Media_Attributes = value;
            }
        }

        [DataMember]
        public string MediaFileMaster
        {
            get
            {
                return _MediaFileMaster;
            }

            set
            {
                _MediaFileMaster = value;
            }
        }
    }

    [DataContract]
    public class DC_Accomodation_Media_Attributes
    {
        System.Guid _Accomodation_Media_Attributes_Id;
        Nullable<System.Guid> _Accomodation_Media_Id;
        string _AttributeType;
        string _AttributeValue;
        bool _IsActive;
        bool _IsMediaActive;
        string _Status;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        bool _IsSystemAttribute;
        int _TotalRecords;

        [DataMember]
        public Guid Accomodation_Media_Attributes_Id
        {
            get
            {
                return _Accomodation_Media_Attributes_Id;
            }

            set
            {
                _Accomodation_Media_Attributes_Id = value;
            }
        }

        [DataMember]
        public Guid? Accomodation_Media_Id
        {
            get
            {
                return _Accomodation_Media_Id;
            }

            set
            {
                _Accomodation_Media_Id = value;
            }
        }

        [DataMember]
        public string AttributeType
        {
            get
            {
                return _AttributeType;
            }

            set
            {
                _AttributeType = value;
            }
        }

        [DataMember]
        public string AttributeValue
        {
            get
            {
                return _AttributeValue;
            }

            set
            {
                _AttributeValue = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public bool IsMediaActive
        {
            get
            {
                return _IsMediaActive;
            }

            set
            {
                _IsMediaActive = value;
            }
        }

        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }

        [DataMember]
        public bool IsSystemAttribute
        {
            get
            {
                return _IsSystemAttribute;
            }

            set
            {
                _IsSystemAttribute = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_RoomInfo
    {
        System.Guid _Accommodation_RoomInfo_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _RoomId;
        string _RoomView;
        Nullable<int> _NoOfRooms;
        string _RoomName;
        Nullable<int> _NoOfInterconnectingRooms;
        string _Description;
        string _RoomSize;
        string _RoomDecor;
        Nullable<bool> _Smoking;
        string _FloorName;
        string _FloorNumber;
        Nullable<bool> _MysteryRoom;
        string _BathRoomType;
        string _BedType;
        string _CompanyRoomCategory;
        string _RoomCategory;
        string _Category;
        string _CompanyName;
        string _AmenityTypes;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        List<DC_Accomodation_RoomFacilities> _RoomFacilities;
        bool _IsActive;
        int? _TotalRecords;

        [DataMember]
        public Guid Accommodation_RoomInfo_Id
        {
            get
            {
                return _Accommodation_RoomInfo_Id;
            }

            set
            {
                _Accommodation_RoomInfo_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string RoomId
        {
            get
            {
                return _RoomId;
            }

            set
            {
                _RoomId = value;
            }
        }

        [DataMember]
        public string RoomView
        {
            get
            {
                return _RoomView;
            }

            set
            {
                _RoomView = value;
            }
        }

        [DataMember]
        public int? NoOfRooms
        {
            get
            {
                return _NoOfRooms;
            }

            set
            {
                _NoOfRooms = value;
            }
        }

        [DataMember]
        public string RoomName
        {
            get
            {
                return _RoomName;
            }

            set
            {
                _RoomName = value;
            }
        }

        [DataMember]
        public int? NoOfInterconnectingRooms
        {
            get
            {
                return _NoOfInterconnectingRooms;
            }

            set
            {
                _NoOfInterconnectingRooms = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string RoomSize
        {
            get
            {
                return _RoomSize;
            }

            set
            {
                _RoomSize = value;
            }
        }

        [DataMember]
        public string RoomDecor
        {
            get
            {
                return _RoomDecor;
            }

            set
            {
                _RoomDecor = value;
            }
        }

        [DataMember]
        public bool? Smoking
        {
            get
            {
                return _Smoking;
            }

            set
            {
                _Smoking = value;
            }
        }

        [DataMember]
        public string FloorName
        {
            get
            {
                return _FloorName;
            }

            set
            {
                _FloorName = value;
            }
        }

        [DataMember]
        public string FloorNumber
        {
            get
            {
                return _FloorNumber;
            }

            set
            {
                _FloorNumber = value;
            }
        }

        [DataMember]
        public bool? MysteryRoom
        {
            get
            {
                return _MysteryRoom;
            }

            set
            {
                _MysteryRoom = value;
            }
        }

        [DataMember]
        public string BathRoomType
        {
            get
            {
                return _BathRoomType;
            }

            set
            {
                _BathRoomType = value;
            }
        }

        [DataMember]
        public string BedType
        {
            get
            {
                return _BedType;
            }

            set
            {
                _BedType = value;
            }
        }

        [DataMember]
        public string CompanyRoomCategory
        {
            get
            {
                return _CompanyRoomCategory;
            }

            set
            {
                _CompanyRoomCategory = value;
            }
        }

        [DataMember]
        public string RoomCategory
        {
            get
            {
                return _RoomCategory;
            }

            set
            {
                _RoomCategory = value;
            }
        }

        [DataMember]
        public string Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;
            }
        }

        [DataMember]
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                _CompanyName = value;
            }
        }

        [DataMember]
        public string AmenityTypes
        {
            get
            {
                return _AmenityTypes;
            }

            set
            {
                _AmenityTypes = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public List<DC_Accomodation_RoomFacilities> RoomFacilities
        {
            get
            {
                return _RoomFacilities;
            }

            set
            {
                _RoomFacilities = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
        [DataMember]
        public int? TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }
    }
    [DataContract]
    public class DC_Accommodation_RoomInfo_RQ
    {
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public int? PageNo;
        [DataMember]
        public int? PageSize;
    }

    [DataContract]
    public class DC_Accomodation_RoomFacilities
    {
        System.Guid _Accommodation_RoomFacility_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<System.Guid> _Accommodation_RoomInfo_Id;
        string _AmenityType;
        string _AmenityName;
        string _Description;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_user;
        bool _IsActive;
        bool _IsRoomActive;

        [DataMember]
        public Guid Accommodation_RoomFacility_Id
        {
            get
            {
                return _Accommodation_RoomFacility_Id;
            }

            set
            {
                _Accommodation_RoomFacility_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_RoomInfo_Id
        {
            get
            {
                return _Accommodation_RoomInfo_Id;
            }

            set
            {
                _Accommodation_RoomInfo_Id = value;
            }
        }

        [DataMember]
        public string AmenityType
        {
            get
            {
                return _AmenityType;
            }

            set
            {
                _AmenityType = value;
            }
        }

        [DataMember]
        public string AmenityName
        {
            get
            {
                return _AmenityName;
            }

            set
            {
                _AmenityName = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Edit_user
        {
            get
            {
                return _Edit_user;
            }

            set
            {
                _Edit_user = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsRoomActive
        {
            get
            {
                return _IsRoomActive;
            }

            set
            {
                _IsRoomActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_PaxOccupancy
    {
        System.Guid _Accommodation_PaxOccupancy_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _RoomDisplayNumber;
        Nullable<System.Guid> _Accommodation_RoomInfo_Id;
        Nullable<int> _MaxAdults;
        Nullable<int> _MaxPaxWithExtraBed;
        Nullable<int> _MaxCNB;
        Nullable<int> _MaxChild;
        Nullable<int> _MaxPax;
        string _RoomType;
        Nullable<int> _FromAgeForExtraBed;
        Nullable<int> _ToAgeForExtraBed;
        Nullable<int> _FromAgeForCNB;
        Nullable<int> _ToAgeForCNB;
        Nullable<int> _FromAgeForCIOR;
        Nullable<int> _ToAgeForCIOR;
        string _category;
        string _companyName;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;
        bool _IsRoomActive;

        [DataMember]
        public Guid Accommodation_PaxOccupancy_Id
        {
            get
            {
                return _Accommodation_PaxOccupancy_Id;
            }

            set
            {
                _Accommodation_PaxOccupancy_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string RoomDisplayNumber
        {
            get
            {
                return _RoomDisplayNumber;
            }

            set
            {
                _RoomDisplayNumber = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_RoomInfo_Id
        {
            get
            {
                return _Accommodation_RoomInfo_Id;
            }

            set
            {
                _Accommodation_RoomInfo_Id = value;
            }
        }

        [DataMember]
        public int? MaxAdults
        {
            get
            {
                return _MaxAdults;
            }

            set
            {
                _MaxAdults = value;
            }
        }

        [DataMember]
        public int? MaxPaxWithExtraBed
        {
            get
            {
                return _MaxPaxWithExtraBed;
            }

            set
            {
                _MaxPaxWithExtraBed = value;
            }
        }

        [DataMember]
        public int? MaxCNB
        {
            get
            {
                return _MaxCNB;
            }

            set
            {
                _MaxCNB = value;
            }
        }

        [DataMember]
        public int? MaxChild
        {
            get
            {
                return _MaxChild;
            }

            set
            {
                _MaxChild = value;
            }
        }

        [DataMember]
        public int? MaxPax
        {
            get
            {
                return _MaxPax;
            }

            set
            {
                _MaxPax = value;
            }
        }

        [DataMember]
        public string RoomType
        {
            get
            {
                return _RoomType;
            }

            set
            {
                _RoomType = value;
            }
        }

        [DataMember]
        public int? FromAgeForExtraBed
        {
            get
            {
                return _FromAgeForExtraBed;
            }

            set
            {
                _FromAgeForExtraBed = value;
            }
        }

        [DataMember]
        public int? ToAgeForExtraBed
        {
            get
            {
                return _ToAgeForExtraBed;
            }

            set
            {
                _ToAgeForExtraBed = value;
            }
        }

        [DataMember]
        public int? FromAgeForCNB
        {
            get
            {
                return _FromAgeForCNB;
            }

            set
            {
                _FromAgeForCNB = value;
            }
        }

        [DataMember]
        public int? ToAgeForCNB
        {
            get
            {
                return _ToAgeForCNB;
            }

            set
            {
                _ToAgeForCNB = value;
            }
        }

        [DataMember]
        public int? FromAgeForCIOR
        {
            get
            {
                return _FromAgeForCIOR;
            }

            set
            {
                _FromAgeForCIOR = value;
            }
        }

        [DataMember]
        public int? ToAgeForCIOR
        {
            get
            {
                return _ToAgeForCIOR;
            }

            set
            {
                _ToAgeForCIOR = value;
            }
        }

        [DataMember]
        public string Category
        {
            get
            {
                return _category;
            }

            set
            {
                _category = value;
            }
        }

        [DataMember]
        public string CompanyName
        {
            get
            {
                return _companyName;
            }

            set
            {
                _companyName = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public bool IsRoomActive
        {
            get
            {
                return _IsRoomActive;
            }

            set
            {
                _IsRoomActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_RouteInfo
    {
        System.Guid _Accommodation_Route_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<int> _Legacy_Htl_Id;
        string _DistanceFromProperty;
        string _ApproximateDuration;
        string _Description;
        string _DrivingDirection;
        Nullable<System.DateTime> _ValidFrom;
        Nullable<System.DateTime> _ValidTo;
        string _DistanceUnit;
        string _AccommodationCode;
        string _FromPlace;
        string _NameOfPlace;
        string _ModeOfTransport;
        string _TransportType;
        string _CompanyName;
        string _Create_User;
        Nullable<System.DateTime> _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        bool _IsActive;

        [DataMember]
        public Guid Accommodation_Route_Id
        {
            get
            {
                return _Accommodation_Route_Id;
            }

            set
            {
                _Accommodation_Route_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public int? Legacy_Htl_Id
        {
            get
            {
                return _Legacy_Htl_Id;
            }

            set
            {
                _Legacy_Htl_Id = value;
            }
        }

        [DataMember]
        public string DistanceFromProperty
        {
            get
            {
                return _DistanceFromProperty;
            }

            set
            {
                _DistanceFromProperty = value;
            }
        }

        [DataMember]
        public string ApproximateDuration
        {
            get
            {
                return _ApproximateDuration;
            }

            set
            {
                _ApproximateDuration = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        [DataMember]
        public string DrivingDirection
        {
            get
            {
                return _DrivingDirection;
            }

            set
            {
                _DrivingDirection = value;
            }
        }

        [DataMember]
        public DateTime? ValidFrom
        {
            get
            {
                return _ValidFrom;
            }

            set
            {
                _ValidFrom = value;
            }
        }

        [DataMember]
        public DateTime? ValidTo
        {
            get
            {
                return _ValidTo;
            }

            set
            {
                _ValidTo = value;
            }
        }

        [DataMember]
        public string DistanceUnit
        {
            get
            {
                return _DistanceUnit;
            }

            set
            {
                _DistanceUnit = value;
            }
        }

        [DataMember]
        public string AccommodationCode
        {
            get
            {
                return _AccommodationCode;
            }

            set
            {
                _AccommodationCode = value;
            }
        }

        [DataMember]
        public string FromPlace
        {
            get
            {
                return _FromPlace;
            }

            set
            {
                _FromPlace = value;
            }
        }

        [DataMember]
        public string NameOfPlace
        {
            get
            {
                return _NameOfPlace;
            }

            set
            {
                _NameOfPlace = value;
            }
        }

        [DataMember]
        public string ModeOfTransport
        {
            get
            {
                return _ModeOfTransport;
            }

            set
            {
                _ModeOfTransport = value;
            }
        }

        [DataMember]
        public string TransportType
        {
            get
            {
                return _TransportType;
            }

            set
            {
                _TransportType = value;
            }
        }

        [DataMember]
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                _CompanyName = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }
    }

    [DataContract]
    public class DC_Accommodation_Status
    {
        System.Guid _Accommodation_Status_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<System.DateTime> _From;
        Nullable<System.DateTime> _To;
        string _DeactivationReason;
        string _Status;
        string _CompanyMarket;
        bool _IsActive;
        string _Create_User;
        string _Edit_User;
        DateTime? _Create_Date;
        DateTime? _Edit_Date;

        [DataMember]
        public Guid Accommodation_Status_Id
        {
            get
            {
                return _Accommodation_Status_Id;
            }

            set
            {
                _Accommodation_Status_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public DateTime? From
        {
            get
            {
                return _From;
            }

            set
            {
                _From = value;
            }
        }

        [DataMember]
        public DateTime? To
        {
            get
            {
                return _To;
            }

            set
            {
                _To = value;
            }
        }

        [DataMember]
        public string DeactivationReason
        {
            get
            {
                return _DeactivationReason;
            }

            set
            {
                _DeactivationReason = value;
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public string CompanyMarket
        {
            get
            {
                return _CompanyMarket;
            }

            set
            {
                _CompanyMarket = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }
    }

    [DataContract]
    public class DC_Accomodation_Search_RQ
    {
        //string _GroupOfCompanies;
        //string _GroupCompany;
        //string _CompanyId;
        string _ProductCategory;
        string _ProductCategorySubType;
        //string _CommonHotelId;
        int? _CompanyHotelId;
        string _HotelName;
        string _Country;
        string _City;
        string _Location;
        string _Status;
        string _Chain;
        string _Brand;
        int _PageNo;
        int _PageSize;
        string _Google_Place_Id;
        string _AccomodationId;
        string _searchfrom;
        string _starrating;
        int? _TotalRecords;
        Nullable<System.DateTime> _FromDate;
        Nullable<System.DateTime> _ToDate;

        //[DataMember]
        //public string GroupOfCompanies
        //{
        //    get
        //    {
        //        return _GroupOfCompanies;
        //    }

        //    set
        //    {
        //        _GroupOfCompanies = value;
        //    }
        //}

        //[DataMember]
        //public string GroupCompany
        //{
        //    get
        //    {
        //        return _GroupCompany;
        //    }

        //    set
        //    {
        //        _GroupCompany = value;
        //    }
        //}

        //[DataMember]
        //public string CompanyId
        //{
        //    get
        //    {
        //        return _CompanyId;
        //    }

        //    set
        //    {
        //        _CompanyId = value;
        //    }
        //}

        [DataMember]
        public Nullable<bool> InsertFrom { get; set; }

        [DataMember(IsRequired = true)]
        public string ProductCategory
        {
            get
            {
                return _ProductCategory;
            }

            set
            {
                _ProductCategory = value;
            }
        }

        [DataMember(IsRequired = true)]
        public string ProductCategorySubType
        {
            get
            {
                return _ProductCategorySubType;
            }

            set
            {
                _ProductCategorySubType = value;
            }
        }

        //[DataMember]
        //public string CommonHotelId
        //{
        //    get
        //    {
        //        return _CommonHotelId;
        //    }

        //    set
        //    {
        //        _CommonHotelId = value;
        //    }
        //}

        [DataMember]
        public int? CompanyHotelId
        {
            get
            {
                return _CompanyHotelId;
            }

            set
            {
                _CompanyHotelId = value;
            }
        }

        [DataMember]
        public string HotelName
        {
            get
            {
                return _HotelName;
            }

            set
            {
                _HotelName = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                _Country = value;
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                _City = value;
            }
        }

        [DataMember]
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                _Location = value;
            }
        }

        [DataMember(IsRequired = true)]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember(IsRequired = true)]
        public int PageNo
        {
            get
            {
                return _PageNo;
            }

            set
            {
                _PageNo = value;
            }
        }

        [DataMember(IsRequired = true)]
        public int PageSize
        {
            get
            {
                return _PageSize;
            }

            set
            {
                _PageSize = value;
            }
        }

        [DataMember]
        public string Chain
        {
            get
            {
                return _Chain;
            }

            set
            {
                _Chain = value;
            }
        }

        [DataMember]
        public string Brand
        {
            get
            {
                return _Brand;
            }

            set
            {
                _Brand = value;
            }
        }
        [DataMember]
        public string Google_Place_Id
        {
            get
            {
                return _Google_Place_Id;
            }

            set
            {
                _Google_Place_Id = value;
            }
        }
        [DataMember]
        public string AccomodationId
        {
            get
            {
                return _AccomodationId;
            }

            set
            {
                _AccomodationId = value;
            }
        }
        [DataMember]
        public string Searchfrom
        {
            get
            {
                return _searchfrom;
            }

            set
            {
                _searchfrom = value;
            }
        }
        [DataMember]
        public string Starrating
        {
            get
            {
                return _starrating;
            }

            set
            {
                _starrating = value;
            }
        }
        [DataMember]
        public int? TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }
        [DataMember]
        public DateTime? FromDate
        {
            get
            {
                return _FromDate;
            }

            set
            {
                _FromDate = value;
            }
        }
        [DataMember]
        public DateTime? ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                _ToDate = value;
            }
        }
    }

    [DataContract]
    public class DC_RoomCategoryMaster_RQ
    {
        [DataMember]
        public string RoomCategory { get; set; }
    }
    [DataContract]
    public class DC_Accomodation_Search_RS
    {
        string _AccomodationId;
        string _HotelName;
        string _CompanyName;
        string _CompanyHotelId;
        string _HotelChain;
        string _HotelBrand;
        string _Country;
        string _City;
        string _Location;
        string _Status;
        string _PostalCode;
        int _TotalRecords;
        string _Google_Place_Id;
        string _FullAddress;
        int? _MapCount;
        string _HotelNameWithCode;
        string _starrating;

        [DataMember]
        public string Telephone_Tx { get; set; }


        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }


        [DataMember]
        public Nullable<bool> InsertFrom { get; set; }

        [DataMember]
        public Nullable<System.Guid> Country_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> City_Id { get; set; }

        [DataMember]
        public string Google_Place_Id
        {
            get
            {
                return _Google_Place_Id;
            }

            set
            {
                _Google_Place_Id = value;
            }
        }

        [DataMember]
        public string AccomodationId
        {
            get
            {
                return _AccomodationId;
            }

            set
            {
                _AccomodationId = value;
            }
        }

        [DataMember]
        public string HotelName
        {
            get
            {
                return _HotelName;
            }

            set
            {
                _HotelName = value;
            }
        }

        [DataMember]
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                _CompanyName = value;
            }
        }

        [DataMember]
        public string CompanyHotelId
        {
            get
            {
                return _CompanyHotelId;
            }

            set
            {
                _CompanyHotelId = value;
            }
        }

        [DataMember]
        public string HotelChain
        {
            get
            {
                return _HotelChain;
            }

            set
            {
                _HotelChain = value;
            }
        }

        [DataMember]
        public string HotelBrand
        {
            get
            {
                return _HotelBrand;
            }

            set
            {
                _HotelBrand = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                _Country = value;
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                _City = value;
            }
        }

        [DataMember]
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                _Location = value;
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }

        [DataMember]
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }

            set
            {
                _PostalCode = value;
            }
        }

        [DataMember]
        public string FullAddress
        {
            get
            {
                return _FullAddress;
            }

            set
            {
                _FullAddress = value;
            }
        }

        [DataMember]
        public int? MapCount
        {
            get
            {
                return _MapCount;
            }

            set
            {
                _MapCount = value;
            }
        }

        [DataMember]
        public string HotelNameWithCode
        {
            get
            {
                return _HotelNameWithCode;
            }

            set
            {
                _HotelNameWithCode = value;
            }
        }
        [DataMember]
        public string Starrating
        {
            get
            {
                return _starrating;
            }

            set
            {
                _starrating = value;
            }
        }
    }

    [DataContract]
    public class DC_Accomodation_ClassificationAttributes
    {
        System.Guid _Accommodation_ClassificationAttribute_Id;
        Nullable<System.Guid> _Accommodation_Id;
        string _AttributeType;
        string _AttributeValue;
        bool _IsActive;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        Nullable<bool> _InternalOnly;
        string _AttributeSubType;

        [DataMember]
        public Guid Accommodation_ClassificationAttribute_Id
        {
            get
            {
                return _Accommodation_ClassificationAttribute_Id;
            }

            set
            {
                _Accommodation_ClassificationAttribute_Id = value;
            }
        }

        [DataMember]
        public Guid? Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public string AttributeType
        {
            get
            {
                return _AttributeType;
            }

            set
            {
                _AttributeType = value;
            }
        }

        [DataMember]
        public string AttributeValue
        {
            get
            {
                return _AttributeValue;
            }

            set
            {
                _AttributeValue = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }

            set
            {
                _IsActive = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public bool? InternalOnly
        {
            get
            {
                return _InternalOnly;
            }

            set
            {
                _InternalOnly = value;
            }
        }

        [DataMember]
        public string AttributeSubType
        {
            get
            {
                return _AttributeSubType;
            }

            set
            {
                _AttributeSubType = value;
            }
        }
    }

    [DataContract]
    public class DC_Accomodation_UpdateStatus_RQ
    {
        Guid _Accommodation_Id;
        bool _Status;
        DateTime _Edit_Date;
        string _Edit_User;

        [DataMember]
        public Guid Accommodation_Id
        {
            get
            {
                return _Accommodation_Id;
            }

            set
            {
                _Accommodation_Id = value;
            }
        }

        [DataMember]
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public DateTime Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }
    }

    [DataContract]
    public class DC_Accomodation_DDL
    {
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
    [DataContract]
    public class DC_Accomodation_Category_DDL
    {
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
    }

    [DataContract]
    public class DC_Accomodation_Category_DDL_WithExtraDetails
    {
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public string BedType { get; set; }
        [DataMember]
        public string IsSomking { get; set; }
    }

    [DataContract]
    public class DC_Accomodation_CopyRoomDef
    {
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string NewRoomCategory { get; set; }
        [DataMember]
        public string Create_User { get; set; }
    }

    [DataContract]
    public class DC_Places
    {
        [DataMember]
        public Guid Place_Id { get; set; }
        [DataMember]
        public string Place_Category { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Formatted_Address { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Google_Place_Id { get; set; }
        [DataMember]
        public string Google_Rating { get; set; }
        [DataMember]
        public string Google_Reference { get; set; }
        [DataMember]
        public string Google_PlaceTypes { get; set; }
        [DataMember]
        public string Google_Icon { get; set; }
        [DataMember]
        public string Google_FullObjext { get; set; }
    }
    [DataContract]
    public class DC_Accomodation_AutoComplete_RQ
    {
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    public class DC_Accomodation_AutoComplete_RS
    {
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string StateCode { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }

}
