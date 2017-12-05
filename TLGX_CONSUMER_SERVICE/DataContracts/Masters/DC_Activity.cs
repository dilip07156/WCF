using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Masters
{
    #region "Activity"
    [DataContract]
    public class DC_Activity
    {
        [DataMember]
        public Guid? Activity_Id { get; set; }

        [DataMember]
        public int? CommonProductID { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public int? CompanyProductID { get; set; }
        [DataMember]
        public int? FinanceProductID { get; set; }
        [DataMember]
        public string Product_Name { get; set; }
        [DataMember]
        public string Display_Name { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public Guid? Country_Id { get; set; }
        [DataMember]
        public Guid? City_Id { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string Mode_Of_Transport { get; set; }
        [DataMember]
        public int? CompanyRating { get; set; }
        [DataMember]
        public int? ProductRating { get; set; }
        [DataMember]
        public string Affiliation { get; set; }
        [DataMember]
        public bool? CompanyRecommended { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public double? Latitude { get; set; }
        [DataMember]
        public double? Longitude { get; set; }
        [DataMember]
        public string TourType { get; set; }
        [DataMember]
        public int? Parent_Legacy_Id { get; set; }
        [DataMember]
        public int? TotalRecord { get; set; }
    }
    [DataContract]
    public class DC_Activity_OldSchema
    {
        Guid _Activity_Id;
        int? _CommonProductID;
        int? _Legacy_Product_ID;
        string _Product_Name;
        string _Display_Name;
        string _Country;
        string _City;
        string _ProductType;
        string _ProductSubType;
        string _ProductCategory;
        DateTime? _Create_Date;
        string _Create_User;
        DateTime? _Edit_Date;
        string _Edit_User;
        bool? _IsActive;
        string _StartingPoint;
        string _EndingPoint;
        string _Duration;
        bool? _CompanyRecommended;
        double? _Latitude;
        double? _Longitude;
        string _ShortDescription;
        string _LongDescription;
        bool? _MealsYN;
        bool? _GuideYN;
        string _TransferYN;
        string _PhysicalLevel;
        string _Advisory;
        string _ThingsToCarry;
        string _DeparturePoint;
        string _TourType;
        int? _Parent_Legacy_Id;
        int _TotalRecord;
        [DataMember]
        public Guid Activity_Id
        {
            get
            {
                return _Activity_Id;
            }

            set
            {
                _Activity_Id = value;
            }
        }

        [DataMember]
        public int? CommonProductID
        {
            get
            {
                return _CommonProductID;
            }

            set
            {
                _CommonProductID = value;
            }
        }

        [DataMember]
        public int? Legacy_Product_ID
        {
            get
            {
                return _Legacy_Product_ID;
            }

            set
            {
                _Legacy_Product_ID = value;
            }
        }

        [DataMember]
        public string Product_Name
        {
            get
            {
                return _Product_Name;
            }

            set
            {
                _Product_Name = value;
            }
        }

        [DataMember]
        public string Display_Name
        {
            get
            {
                return _Display_Name;
            }

            set
            {
                _Display_Name = value;
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
        public string ProductType
        {
            get
            {
                return _ProductType;
            }

            set
            {
                _ProductType = value;
            }
        }

        [DataMember]
        public string ProductSubType
        {
            get
            {
                return _ProductSubType;
            }

            set
            {
                _ProductSubType = value;
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
        public string StartingPoint
        {
            get
            {
                return _StartingPoint;
            }

            set
            {
                _StartingPoint = value;
            }
        }

        [DataMember]
        public string EndingPoint
        {
            get
            {
                return _EndingPoint;
            }

            set
            {
                _EndingPoint = value;
            }
        }

        [DataMember]
        public string Duration
        {
            get
            {
                return _Duration;
            }

            set
            {
                _Duration = value;
            }
        }

        [DataMember]
        public bool? CompanyRecommended
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
        public double? Latitude
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
        public double? Longitude
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
        public string ShortDescription
        {
            get
            {
                return _ShortDescription;
            }

            set
            {
                _ShortDescription = value;
            }
        }

        [DataMember]
        public string LongDescription
        {
            get
            {
                return _LongDescription;
            }

            set
            {
                _LongDescription = value;
            }
        }

        [DataMember]
        public bool? MealsYN
        {
            get
            {
                return _MealsYN;
            }

            set
            {
                _MealsYN = value;
            }
        }

        [DataMember]
        public bool? GuideYN
        {
            get
            {
                return _GuideYN;
            }

            set
            {
                _GuideYN = value;
            }
        }

        [DataMember]
        public string TransferYN
        {
            get
            {
                return _TransferYN;
            }

            set
            {
                _TransferYN = value;
            }
        }

        [DataMember]
        public string PhysicalLevel
        {
            get
            {
                return _PhysicalLevel;
            }

            set
            {
                _PhysicalLevel = value;
            }
        }

        [DataMember]
        public string Advisory
        {
            get
            {
                return _Advisory;
            }

            set
            {
                _Advisory = value;
            }
        }

        [DataMember]
        public string ThingsToCarry
        {
            get
            {
                return _ThingsToCarry;
            }

            set
            {
                _ThingsToCarry = value;
            }
        }

        [DataMember]
        public string DeparturePoint
        {
            get
            {
                return _DeparturePoint;
            }

            set
            {
                _DeparturePoint = value;
            }
        }

        [DataMember]
        public string TourType
        {
            get
            {
                return _TourType;
            }

            set
            {
                _TourType = value;
            }
        }

        [DataMember]
        public int? Parent_Legacy_Id
        {
            get
            {
                return _Parent_Legacy_Id;
            }

            set
            {
                _Parent_Legacy_Id = value;
            }
        }

        [DataMember]
        public int TotalRecord
        {
            get
            {
                return _TotalRecord;
            }

            set
            {
                _TotalRecord = value;
            }
        }
    }

    public class DC_ActivitySearch_RS
    {
        [DataMember]
        public Guid Activity_Id { get; set; }

        [DataMember]
        public int? CommonProductID { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public int? CompanyProductID { get; set; }
        [DataMember]
        public string Product_Name { get; set; }
        [DataMember]
        public string Display_Name { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public Guid? Country_Id { get; set; }
        [DataMember]
        public Guid? City_Id { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string Mode_Of_Transport { get; set; }
        [DataMember]
        public int? CompanyRating { get; set; }
        [DataMember]
        public int? ProductRating { get; set; }
        [DataMember]
        public string Affiliation { get; set; }
        [DataMember]
        public bool? CompanyRecommended { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public double? Latitude { get; set; }
        [DataMember]
        public double? Longitude { get; set; }
        [DataMember]
        public string TourType { get; set; }
        [DataMember]
        public int? Parent_Legacy_Id { get; set; }
        [DataMember]
        public int? TotalRecord { get; set; }

        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string ProductNameSubType { get; set; }
        [DataMember]
        public string CommonProductNameSubType_Id { get; set; }

    }

    [DataContract]
    public class DC_Activity_Search_RQ
    {
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string ProductNameSubType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public string Supplier_Id { get; set; }
        [DataMember]
        public string Keyword { get; set; }

    }
    #endregion

    #region "Activity Content"
    [DataContract]
    public class DC_Activity_Content
    {
        [DataMember]
        public Guid ActivityContent_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Id { get; set; }
        [DataMember]
        public string Content_Type { get; set; }
        [DataMember]
        public string Content_Text { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public bool? IsInternal { get; set; }
        [DataMember]
        public int TotalRecord { get; set; }
    }

    [DataContract]
    public class DC_Activity_DDL
    {
        [DataMember]
        public Guid Activity_Id { get; set; }
        [DataMember]
        public string Product_Name { get; set; }


    }
    #endregion

    #region "Activity Contacts"
    [DataContract]
    public class DC_Activity_Contact
    {
        [DataMember]
        public Guid Activity_Contact_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }

        [DataMember]
        public int? Legacy_Product_ID { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string WebSiteURL { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public DateTime? Create_Date { get; set; }

        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public DateTime? Edit_Date { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
    #endregion

    #region "Activity Status"
    [DataContract]
    public class DC_Activity_Status
    {
        [DataMember]
        public Guid Activity_Status_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }

        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public DateTime? From { get; set; }

        [DataMember]
        public DateTime? To { get; set; }

        [DataMember]
        public string DeactivationReason { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string CompanyMarket { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public DateTime? Create_Date { get; set; }

        [DataMember]
        public DateTime? Edit_Date { get; set; }

    }
    #endregion

    #region Activity Media
    [DataContract]
    public class DC_Activity_Media
    {
        [DataMember]
        public Guid? Activity_Media_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public string MediaName { get; set; }
        [DataMember]
        public string MediaType { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public DateTime? ValidFrom { get; set; }
        [DataMember]
        public DateTime? ValidTo { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Media_Path { get; set; }
        [DataMember]
        public string Media_URL { get; set; }
        [DataMember]
        public int? Media_Position { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string FileFormat { get; set; }
        [DataMember]
        public string MediaID { get; set; }
        [DataMember]
        public string MediaFileMaster { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? Media_Width { get; set; }
        [DataMember]
        public int? Media_Height { get; set; }
        [DataMember]
        public string Media_Caption { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Media_Search_RQ
    {
        [DataMember]
        public Guid? Activity_Media_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string MediaName { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public DateTime? ValidFrom { get; set; }
        [DataMember]
        public DateTime? ValidTo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public string MediaType { get; set; }
    }
    #endregion

    #region inclusions
    [DataContract]
    public class DC_Activity_Inclusions
    {
        [DataMember]
        public Guid? Activity_Inclusions_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public bool? IsInclusion { get; set; }
        [DataMember]
        public string InclusionFor { get; set; }
        [DataMember]
        public bool? IsDriver { get; set; }
        [DataMember]
        public bool? IsAudioCommentary { get; set; }
        [DataMember]
        public string InclusionDescription { get; set; }
        [DataMember]
        public string InclusionName { get; set; }
        [DataMember]
        public string RestaurantStyle { get; set; }
        [DataMember]
        public string InclusionType { get; set; }
        [DataMember]
        public DateTime? InclusionFrom { get; set; }
        [DataMember]
        public DateTime? InclusionTo { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Inclusions_RQ
    {
        [DataMember]
        public Guid? Activity_Inclusions_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public bool? IsInclusion { get; set; }
        [DataMember]
        public string InclusionName { get; set; }
        [DataMember]
        public string InclusionType { get; set; }
        [DataMember]
        public DateTime? InclusionFrom { get; set; }
        [DataMember]
        public DateTime? InclusionTo { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Inclusion Details
    [DataContract]
    public class DC_Activity_InclusionsDetails
    {
        [DataMember]
        public Guid? Activity_InclusionDetails_Id { get; set; }
        [DataMember]
        public Guid? Activity_Inclusion_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string InclusionDetailFor { get; set; }
        [DataMember]
        public string GuideLanguage { get; set; }
        [DataMember]
        public string GuideLanguageCode { get; set; }
        [DataMember]
        public string InclusionDetailType { get; set; }
        [DataMember]
        public string InclusionDetailName { get; set; }
        [DataMember]
        public string InclusionDetailDescription { get; set; }
        [DataMember]
        public DateTime? InclusionDetailFrom { get; set; }
        [DataMember]
        public DateTime? InclusionDetailTo { get; set; }
        [DataMember]
        public string DaysOfWeek { get; set; }
        [DataMember]
        public string FromTime { get; set; }
        [DataMember]
        public string ToTime { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public string EditUser { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_InclusionDetails_RQ
    {
        public Guid? Activity_InclusionDetails_Id { get; set; }
        [DataMember]
        public Guid? Activity_Inclusion_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public DateTime? InclusionDetailFrom { get; set; }
        [DataMember]
        public DateTime? InclusionDetailTo { get; set; }
        [DataMember]
        public string InclusionDetailType { get; set; }
        [DataMember]
        public string InclusionDetailName { get; set; }
        [DataMember]
        public string GuideLanguage { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activiy Clasification Attributes
    [DataContract]
    public class DC_Activity_ClassificationAttributes
    {
        [DataMember]
        public Guid? Activity_ClassificationAttribute_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeValue { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public bool? InternalOnly { get; set; }
        [DataMember]
        public string AttributeSubType { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public string EditUser { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_ClassificationAttributes_RQ
    {
        [DataMember]
        public Guid? Activity_ClassificationAttribute_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeSubType { get; set; }
        [DataMember]
        public string AttributeValue { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region pickUpDrop
    [DataContract]
    public class DC_Activity_PickUpDrop
    {
        [DataMember]
        public Guid? Activity_PickUpDrop_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public Guid? Supplier_Id { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string TransferType { get; set; }
        [DataMember]
        public string VehicleType { get; set; }
        [DataMember]
        public string VehicleCategory { get; set; }
        [DataMember]
        public string VehicleName { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public bool? IsAC { get; set; }
        [DataMember]
        public string ForSupplier { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_PickUpDrop_RQ
    {
        [DataMember]
        public Guid? Activity_PickUpDrop_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public Guid? Supplier_Id { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region pickUpDrop Details
    [DataContract]
    public class DC_Activity_PickUpDropDetails
    {
        [DataMember]
        public Guid? Activity_PickUpDropDetail_Id { get; set; }
        [DataMember]
        public Guid? Activity_PickUpDrop_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public string PickUpDropType { get; set; }
        [DataMember]
        public string FromToType { get; set; }
        [DataMember]
        public string LocationType { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string AreaSearchFor { get; set; }
        [DataMember]
        public string AreaNameofPlace { get; set; }
        [DataMember]
        public Guid? Accommodation_Id { get; set; }
        [DataMember]
        public string Acco_Name { get; set; }
        [DataMember]
        public string Acco_Address { get; set; }
        [DataMember]
        public string Acco_PostalCode { get; set; }
        [DataMember]
        public string Acco_Country { get; set; }
        [DataMember]
        public string Acco_City { get; set; }
        [DataMember]
        public string Acco_State { get; set; }
        [DataMember]
        public string Acco_Area { get; set; }
        [DataMember]
        public string Acco_Location { get; set; }
        [DataMember]
        public string Acco_Telephone { get; set; }
        [DataMember]
        public string Acco_Fax { get; set; }
        [DataMember]
        public string Acco_Website { get; set; }
        [DataMember]
        public string Acco_Email { get; set; }
        [DataMember]
        public string Acco_ContactNotes { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_PickUpDropDetails_RQ
    {
        [DataMember]
        public Guid? Activity_PickUpDropDetail_Id { get; set; }
        [DataMember]
        public Guid? Activity_PickUpDrop_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_Id { get; set; }
        [DataMember]
        public string PickUpDropType { get; set; }
        [DataMember]
        public string FromToType { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string Acco_Country { get; set; }
        [DataMember]
        public string Acco_City { get; set; }
        [DataMember]
        public string Acco_State { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public Guid? Accommodation_Id { get; set; }
    }
    #endregion

    #region Activity Description
    [DataContract]
    public class DC_Activity_Descriptions
    {
        [DataMember]
        public Guid? Activity_Description_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Language_Code { get; set; }
        [DataMember]
        public string DescriptionFor { get; set; }
        [DataMember]
        public string DescriptionType { get; set; }
        [DataMember]
        public string DescriptionSubType { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Description_Name { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Descriptions_RQ
    {
        [DataMember]
        public Guid? Activity_Description_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string DescriptionFor { get; set; }
        [DataMember]
        public string DescriptionType { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity Flavour
    [DataContract]
    public class DC_Activity_Flavour
    {
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public string CommonProductNameSubType_Id { get; set; }
        [DataMember]
        public string CompanyProductNameSubType_Id { get; set; }
        [DataMember]
        public string FinanceControlId { get; set; }
        [DataMember]
        public string ProductNameSubType { get; set; }
        [DataMember]
        public string PlaceOfEvent { get; set; }
        [DataMember]
        public string StartingPoint { get; set; }
        [DataMember]
        public string EndingPoint { get; set; }
        [DataMember]
        public string USP { get; set; }
        [DataMember]
        public bool? IsPickUpDropDefined { get; set; }
        [DataMember]
        public bool? MustSeeInCountry { get; set; }
        [DataMember]
        public bool? CompanyReccom { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string CityCode { get; set; }
        [DataMember]
        public decimal? Latitude { get; set; }
        [DataMember]
        public decimal? Longitude { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string Street2 { get; set; }
        [DataMember]
        public string Street3 { get; set; }
        [DataMember]
        public string Street4 { get; set; }
        [DataMember]
        public string Street5 { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public Guid? City_Id { get; set; }
        [DataMember]
        public Guid? Country_Id { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }

    }
    [DataContract]
    public class DC_Activity_Flavour_RQ
    {
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string ProductNameSubType { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity Flavour Options
    [DataContract]
    public class DC_Activity_Flavour_Options
    {
        [DataMember]
        public Guid? Activity_FlavourOptions_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string Activity_FlavourName { get; set; }
        [DataMember]
        public string Activity_OptionName { get; set; }
        [DataMember]
        public string Activity_OptionCode { get; set; }
        [DataMember]
        public string Activity_Type { get; set; }
        [DataMember]
        public string Activity_DealText { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Flavour_Options_RQ
    {
        [DataMember]
        public Guid? Activity_FlavourOptions_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string Activity_FlavourName { get; set; }
        [DataMember]
        public string Activity_OptionName { get; set; }
        [DataMember]
        public string Activity_OptionCode { get; set; }
        [DataMember]
        public string Activity_Type { get; set; }
        [DataMember]
        public string Activity_DealText { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity Deals
    [DataContract]
    public class DC_Activity_Deals
    {
        [DataMember]
        public Guid? Activity_Deals_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string DealCode { get; set; }
        [DataMember]
        public string DealText { get; set; }
        [DataMember]
        public string DealName { get; set; }
        [DataMember]
        public string Deal_TnC { get; set; }
        [DataMember]
        public decimal? Deal_Price { get; set; }
        [DataMember]
        public string Deal_Currency { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Deals_RQ
    {
        [DataMember]
        public Guid? Activity_Deals_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string DealCode { get; set; }
        [DataMember]
        public string DealName { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity Prices
    [DataContract]
    public class DC_Activity_Prices
    {
        [DataMember]
        public Guid? Activity_Prices_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string PriceCode { get; set; }
        [DataMember]
        public string PriceBasis { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public string Price_For { get; set; }
        [DataMember]
        public string Price_Type { get; set; }
        [DataMember]
        public string Price_OptionCode { get; set; }
        [DataMember]
        public string PriceCurrency { get; set; }
        [DataMember]
        public int? Totalrecords { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
    }
    [DataContract]
    public class DC_Activity_Prices_RQ
    {
        [DataMember]
        public Guid? Activity_Prices_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Supplier Product mapping
    [DataContract]
    public class DC_Activity_SupplierProductMapping
    {
        [DataMember]
        public Guid? ActivitySupplierProductMapping_Id { get; set; }
        [DataMember]
        public Guid? Activity_ID { get; set; }
        [DataMember]
        public Guid? Supplier_ID { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SuplierProductCode { get; set; }
        [DataMember]
        public string SupplierProductType { get; set; }
        [DataMember]
        public string SupplierType { get; set; }
        [DataMember]
        public string SupplierLocationId { get; set; }
        [DataMember]
        public string SupplierLocationName { get; set; }
        [DataMember]
        public string SupplierCountryName { get; set; }
        [DataMember]
        public string SupplierCityName { get; set; }
        [DataMember]
        public string SupplierCountryCode { get; set; }
        [DataMember]
        public string SupplierCityCode { get; set; }
        [DataMember]
        public string SupplierStateName { get; set; }
        [DataMember]
        public string SupplierStateCode { get; set; }
        [DataMember]
        public string SupplierCityIATACode { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string SupplierDataLangaugeCode { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public string Conditions { get; set; }
        [DataMember]
        public string Inclusions { get; set; }
        [DataMember]
        public string Exclusions { get; set; }
        [DataMember]
        public string AdditionalInformation { get; set; }
        [DataMember]
        public string DeparturePoint { get; set; }
        [DataMember]
        public string TicketingDetails { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string DepartureDate { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public string BlockOutDateFrom { get; set; }
        [DataMember]
        public string BlockOutDateTo { get; set; }
        [DataMember]
        public string OptionTitle { get; set; }
        [DataMember]
        public string OptionCode { get; set; }
        [DataMember]
        public string OptionDescription { get; set; }
        [DataMember]
        public string TourActivityLangauageCode { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public string TourActivityLanguage { get; set; }
        [DataMember]
        public string ImgURL { get; set; }
        [DataMember]
        public string ProductValidFor { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string DayPattern { get; set; }
        [DataMember]
        public string Theme { get; set; }
        [DataMember]
        public string Distance { get; set; }
        [DataMember]
        public string SupplierTourType { get; set; }
        [DataMember]
        public string MappingStatus { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public int? MapID { get; set; }
        [DataMember]
        public decimal? Price { get; set; }
        [DataMember]
        public int? TotalActivities { get; set; }
        [DataMember]
        public string PhysicalIntensity { get; set; }
        [DataMember]
        public string PassengerNumbers { get; set; }
        [DataMember]
        public string DurationLength { get; set; }
        [DataMember]
        public string Timing { get; set; }
        [DataMember]
        public string Session { get; set; }
        [DataMember]
        public string Specials { get; set; }
        [DataMember]
        public bool? Recommonded { get; set; }
        [DataMember]
        public double? Rating { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_SupplierProductMapping_RQ
    {
        [DataMember]
        public Guid? ActivitySupplierProductMapping_Id { get; set; }
        [DataMember]
        public Guid? Activity_ID { get; set; }
        [DataMember]
        public Guid? Supplier_ID { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SuplierProductCode { get; set; }
        [DataMember]
        public string SupplierProductType { get; set; }
        [DataMember]
        public string SupplierType { get; set; }
        [DataMember]
        public string SupplierCountryName { get; set; }
        [DataMember]
        public string SupplierCityName { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Reviews And Scores
    [DataContract]
    public class DC_Activity_ReviewsAndScores
    {
        [DataMember]
        public Guid? Activity_ReviewsAndScores_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string Review_Source { get; set; }
        [DataMember]
        public string Review_Type { get; set; }
        [DataMember]
        public decimal? Review_Score { get; set; }
        [DataMember]
        public bool? IsCustomerReview { get; set; }
        [DataMember]
        public string Review_Author { get; set; }
        [DataMember]
        public DateTime? Review_PostedDate { get; set; }
        [DataMember]
        public string Review_Title { get; set; }
        [DataMember]
        public string Review_Description { get; set; }
        [DataMember]
        public string Review_Status { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? Totalrecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_ReviewsAndScores_RQ
    {
        [DataMember]
        public Guid? Activity_ReviewsAndScores_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public string Review_Title { get; set; }
        [DataMember]
        public string Review_Author { get; set; }
        [DataMember]
        public string Review_Status { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Supplier Product mapping_CA
    [DataContract]
    public class DC_Activity_SupplierProductMapping_CA
    {
        [DataMember]
        public Guid? Activity_SupplierProductMapping_CA_Id { get; set; }
        [DataMember]
        public Guid? Supplier_ID { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SuplierProductCode { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeSubType { get; set; }
        [DataMember]
        public string AttributeValue { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public int? Totalrecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_SupplierProductMapping_CA_RQ
    {
        [DataMember]
        public Guid? Activity_SupplierProductMapping_CA_Id { get; set; }
        [DataMember]
        public Guid? Supplier_ID { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string AttributeType { get; set; }
        [DataMember]
        public string AttributeSubType { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity Policy
    [DataContract]
    public class DC_Activity_Policy
    {
        [DataMember]
        public Guid? Activity_Policy_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public string Policy_Type { get; set; }
        [DataMember]
        public bool? AllowedYN { get; set; }
        [DataMember]
        public string PolicyName { get; set; }
        [DataMember]
        public string PolicyDescription { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? Totalrecords { get; set; }
    }
    [DataContract]
    public class DC_Activity_Policy_RQ
    {
        [DataMember]
        public Guid? Activity_Policy_Id { get; set; }
        [DataMember]
        public Guid? Activity_Id { get; set; }
        [DataMember]
        public Guid? Activity_Flavour_Id { get; set; }
        [DataMember]
        public int? Legacy_Product_ID { get; set; }
        [DataMember]
        public string PolicyName { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
    }
    #endregion

    #region Activity DaysOfWeek & Operating Days
    [DataContract]
    public class DC_Activity_DaysOfWeek
    {
        [DataMember]
        public Guid Activity_DaysOfWeek_ID { get; set; }
        [DataMember]
        public Guid Activity_Flavor_ID { get; set; }
        [DataMember]
        public Guid?  Activity_DaysOfOperation_Id { get; set; }
        [DataMember]
        public string SupplierFrequency { get; set; }
        [DataMember]
        public bool Mon { get; set; }
        [DataMember]
        public bool Tues { get; set; }
        [DataMember]
        public bool Wed { get; set; }
        [DataMember]
        public bool Thur { get; set; }
        [DataMember]
        public bool Fri { get; set; }
        [DataMember]
        public bool Sat { get; set; }
        [DataMember]
        public bool Sun { get; set; }
        [DataMember]
        public string SupplierStartTime { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string SupplierEndTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string SupplierDuration { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string SupplierSession { get; set; }
        [DataMember]
        public string Session { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string EditUser { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
    }

    [DataContract]
    public class DC_Activity_OperatingDays
    {
        [DataMember]
        public Guid Activity_DaysOfOperation_Id { get; set; }
        [DataMember]
        public Guid Activity_Flavor_ID { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public bool? IsOperatingDays { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public string EditUser { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public List<DC_Activity_DaysOfWeek> Activity_DaysOfWeek { get; set; }
    }
    #endregion
    
}



