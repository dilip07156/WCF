using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accomodation_ProductMapping
    {
        System.Guid _Accommodation_ProductMapping_Id;
        Nullable<System.Guid> _Accommodation_Id;
        Nullable<System.Guid> _Supplier_Id;
        string _SupplierId;
        string _SupplierName;
        string _SupplierProductReference;
        string _ProductName;
        string _Street;
        string _Street2;
        string _Street3;
        string _CountryName;
        string _CountryCode;
        string _CityName;
        string _CityCode;
        string _PostCode;
        string _TelephoneNumber;
        string _Status;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        bool _IsActive;
        string _Street4;
        string _StateName;
        string _StateCode;
        string _Latitude;
        string _Longitude;
        string _Email;
        string _Fax;
        string _Website;
        int _TotalRecords;
        string _ProductId;
        string _SystemCountryName;
        string _SystemStateName;
        string _SystemCityName;
        string _SystemFullAddress;
        string _SystemProductName;
        string _SystemProductCode;

        string _Remarks;
        string _FullAddress;
        string _address_tx;
        string _TelephoneNumber_tx;
        int? _MapId;
        string _mstAcco_Id;
        string _mstHotelName;
        string _Google_Place_Id;
        string _Location;

        [DataMember]
        public string SystemTelephone { get; set; }


        [DataMember]
        public string SystemLocation { get; set; }

        [DataMember]
        public string SystemLatitude { get; set; }

        [DataMember]
        public string SystemLongitude { get; set; }

        [DataMember]
        public string Latitude_Tx { get; set; }


        [DataMember]
        public string Longitude_Tx { get; set; }

        [DataMember]
        public string HotelName_Tx { get; set; }

        [DataMember]
        public Int32? MatchedBy { get; set; }
        [DataMember]
        public string MatchedByString { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public Nullable<System.Guid> Country_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> City_Id { get; set; }

        [DataMember]
        public string oldProductName { get; set; }

        [DataMember]
        public string StarRating { get; set; }


        [DataMember]
        public Nullable<System.Guid> stg_AccoMapping_Id { get; set; }

        [DataMember]
        public Guid Accommodation_ProductMapping_Id
        {
            get
            {
                return _Accommodation_ProductMapping_Id;
            }

            set
            {
                _Accommodation_ProductMapping_Id = value;
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
        public Guid? Supplier_Id
        {
            get
            {
                return _Supplier_Id;
            }

            set
            {
                _Supplier_Id = value;
            }
        }

        [DataMember]
        public string SupplierId
        {
            get
            {
                return _SupplierId;
            }

            set
            {
                _SupplierId = value;
            }
        }

        [DataMember]
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }

            set
            {
                _SupplierName = value;
            }
        }

        [DataMember]
        public string SupplierProductReference
        {
            get
            {
                return _SupplierProductReference;
            }

            set
            {
                _SupplierProductReference = value;
            }
        }

        [DataMember]
        public string ProductName
        {
            get
            {
                return _ProductName;
            }

            set
            {
                _ProductName = value;
            }
        }

        [DataMember]
        public string Street
        {
            get
            {
                return _Street;
            }

            set
            {
                _Street = value;
            }
        }

        [DataMember]
        public string Street2
        {
            get
            {
                return _Street2;
            }

            set
            {
                _Street2 = value;
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
        public string CountryName
        {
            get
            {
                return _CountryName;
            }

            set
            {
                _CountryName = value;
            }
        }

        [DataMember]
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                _CountryCode = value;
            }
        }

        [DataMember]
        public string CityName
        {
            get
            {
                return _CityName;
            }

            set
            {
                _CityName = value;
            }
        }

        [DataMember]
        public string CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                _CityCode = value;
            }
        }

        [DataMember]
        public string PostCode
        {
            get
            {
                return _PostCode;
            }

            set
            {
                _PostCode = value;
            }
        }

        [DataMember]
        public string TelephoneNumber
        {
            get
            {
                return _TelephoneNumber;
            }

            set
            {
                _TelephoneNumber = value;
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
        public string StateName
        {
            get
            {
                return _StateName;
            }

            set
            {
                _StateName = value;
            }
        }

        [DataMember]
        public string StateCode
        {
            get
            {
                return _StateCode;
            }

            set
            {
                _StateCode = value;
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
        public string Website
        {
            get
            {
                return _Website;
            }

            set
            {
                _Website = value;
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
        public string ProductId
        {
            get
            {
                return _ProductId;
            }

            set
            {
                _ProductId = value;
            }
        }

        [DataMember]
        public string SystemCountryName
        {
            get
            {
                return _SystemCountryName;
            }

            set
            {
                _SystemCountryName = value;
            }
        }
        [DataMember]
        public string SystemStateName
        {
            get
            {
                return _SystemStateName;
            }

            set
            {
                _SystemStateName = value;
            }
        }

        [DataMember]
        public string SystemCityName
        {
            get
            {
                return _SystemCityName;
            }

            set
            {
                _SystemCityName = value;
            }
        }

        [DataMember]
        public string SystemProductName
        {
            get
            {
                return _SystemProductName;
            }

            set
            {
                _SystemProductName = value;
            }
        }

        [DataMember]
        public string SystemProductCode
        {
            get
            {
                return _SystemProductCode;
            }

            set
            {
                _SystemProductCode = value;
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
        public string Address_tx
        {
            get
            {
                return _address_tx;
            }

            set
            {
                _address_tx = value;
            }
        }
        [DataMember]
        public string TelephoneNumber_tx
        {
            get
            {
                return _TelephoneNumber_tx;
            }

            set
            {
                _TelephoneNumber_tx = value;
            }
        }
        [DataMember]
        public int? MapId
        {
            get
            {
                return _MapId;
            }

            set
            {
                _MapId = value;
            }
        }
        [DataMember]
        public string mstAcco_Id
        {
            get
            {
                return _mstAcco_Id;
            }

            set
            {
                _mstAcco_Id = value;
            }
        }
        [DataMember]
        public string mstHotelName
        {
            get
            {
                return _mstHotelName;
            }

            set
            {
                _mstHotelName = value;
            }
        }
        [DataMember]
        public string SystemFullAddress
        {
            get
            {
                return _SystemFullAddress;
            }

            set
            {
                _SystemFullAddress = value;
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
        public string TLGXProductCode { get; set; }
    }

    [DataContract]
    public class DC_Mapping_ProductSupplier_Search_RQ
    {
        string _SupplierId;
        string _SupplierName;
        string _ProductName;
        string _Street;
        string _Street2;
        string _Street3;
        string _CountryName;
        string _CountryCode;
        string _CityName;
        string _CityCode;
        string _PostCode;
        string _TelephoneNumber;
        string _Status;
        int _PageNo;
        int _PageSize;
        string _Brand;
        string _Chain;
        string _SupplierCountryName;
        string _SupplierCityName;
        string _SupplierProductName;
        string _StatusExcept;
        string _source;
        string _address_tx;
        string _TelephoneNumber_tx;



        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public string Via { get; set; }
        [DataMember]
        public string HotelName_TX { get; set; }


        [DataMember]
        public Nullable<System.Guid> Country_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Accommodation_Id { get; set; }

        [DataMember]
        public int? MatchedBy { get; set; }

        [DataMember]
        public string CalledFromTLGX { get; set; }


        [DataMember]
        public string StarRating { get; set; }

        [DataMember]
        public string SupplierId
        {
            get
            {
                return _SupplierId;
            }

            set
            {
                _SupplierId = value;
            }
        }

        [DataMember]
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }

            set
            {
                _SupplierName = value;
            }
        }

        [DataMember]
        public string ProductName
        {
            get
            {
                return _ProductName;
            }

            set
            {
                _ProductName = value;
            }
        }

        [DataMember]
        public string Street
        {
            get
            {
                return _Street;
            }

            set
            {
                _Street = value;
            }
        }

        [DataMember]
        public string Street2
        {
            get
            {
                return _Street2;
            }

            set
            {
                _Street2 = value;
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
        public string CountryName
        {
            get
            {
                return _CountryName;
            }

            set
            {
                _CountryName = value;
            }
        }

        [DataMember]
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                _CountryCode = value;
            }
        }

        [DataMember]
        public string CityName
        {
            get
            {
                return _CityName;
            }

            set
            {
                _CityName = value;
            }
        }

        [DataMember]
        public string CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                _CityCode = value;
            }
        }

        [DataMember]
        public string PostCode
        {
            get
            {
                return _PostCode;
            }

            set
            {
                _PostCode = value;
            }
        }

        [DataMember]
        public string TelephoneNumber
        {
            get
            {
                return _TelephoneNumber;
            }

            set
            {
                _TelephoneNumber = value;
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

        [DataMember]
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
        public string SupplierCountryName
        {
            get
            {
                return _SupplierCountryName;
            }

            set
            {
                _SupplierCountryName = value;
            }
        }

        [DataMember]
        public string SupplierCityName
        {
            get
            {
                return _SupplierCityName;
            }

            set
            {
                _SupplierCityName = value;
            }
        }

        [DataMember]
        public string SupplierProductName
        {
            get
            {
                return _SupplierProductName;
            }

            set
            {
                _SupplierProductName = value;
            }
        }

        [DataMember]
        public string StatusExcept
        {
            get
            {
                return _StatusExcept;
            }

            set
            {
                _StatusExcept = value;
            }
        }

        [DataMember]
        public string Source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
            }
        }
        [DataMember]
        public string Address_tx
        {
            get
            {
                return _address_tx;
            }

            set
            {
                _address_tx = value;
            }
        }
        [DataMember]
        public string TelephoneNumber_tx
        {
            get
            {
                return _TelephoneNumber_tx;
            }

            set
            {
                _TelephoneNumber_tx = value;
            }
        }
    }

    [DataContract]
    public class DC_Mapping_ShiftMapping_RQ
    {
        System.Guid _Accommodation_From_Id;
        System.Guid _Accommodation_To_Id;
        System.Guid _Accommodation_Id;
        string _Remarks;
        DateTime _Edit_Date;
        string _Edit_User;

        [DataMember]
        public Guid Accommodation_From_Id
        {
            get
            {
                return _Accommodation_From_Id;
            }

            set
            {
                _Accommodation_From_Id = value;
            }
        }

        [DataMember]
        public Guid Accommodation_To_Id
        {
            get
            {
                return _Accommodation_To_Id;
            }

            set
            {
                _Accommodation_To_Id = value;
            }
        }

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
}
