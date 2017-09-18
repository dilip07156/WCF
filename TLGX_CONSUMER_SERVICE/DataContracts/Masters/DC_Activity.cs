using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Masters
{
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
}
