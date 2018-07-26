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
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public Guid ReRunSupplierImporrtFile_Id { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string SystemProductType { get; set; }
        [DataMember]
        public string OldProductType { get; set; }

        [DataMember]
        public int ReRunBatch { get; set; }

        [DataMember]
        public Guid SupplierImporrtFile_Id { get; set; }

        [DataMember]
        public int Batch { get; set; }

        [DataMember]
        public string SystemTelephone { get; set; }


        [DataMember]
        public string SystemLocation { get; set; }

        [DataMember]
        public string SystemLatitude { get; set; }

        [DataMember]
        public string SystemLongitude { get; set; }

        [DataMember]
        public string OldLatitude { get; set; }


        [DataMember]
        public string OldLongitude { get; set; }

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
        public Guid Accommodation_ProductMapping_Id { get; set; }

        [DataMember]
        public Guid? Accommodation_Id { get; set; }


        [DataMember]
        public Guid? Supplier_Id { get; set; }


        [DataMember]
        public string SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string SupplierProductReference { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Street2 { get; set; }
        [DataMember]
        public string Street3 { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string CityCode { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string TelephoneNumber { get; set; }
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
        public bool IsActive { get; set; }
        [DataMember]
        public string Street4 { get; set; }
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string StateCode { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string SystemCountryName { get; set; }

        [DataMember]
        public string SystemStateName { get; set; }

        [DataMember]
        public string SystemCityName { get; set; }

        [DataMember]
        public string SystemProductName { get; set; }

        [DataMember]
        public string SystemProductCode { get; set; }


        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public string FullAddress { get; set; }

        [DataMember]
        public string Address_tx { get; set; }

        [DataMember]
        public string TelephoneNumber_tx { get; set; }

        [DataMember]
        public int? MapId { get; set; }
        [DataMember]
        public string mstAcco_Id { get; set; }

        [DataMember]
        public string mstHotelName { get; set; }

        [DataMember]
        public string SystemFullAddress { get; set; }

        [DataMember]
        public string Google_Place_Id { get; set; }
        [DataMember]
        public string Location { get; set; }
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
        public string SupplierProductCode { get; set; }

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

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public Guid? City_Id { get; set; }
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
