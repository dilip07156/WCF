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
    public class DC_CountryMapping
    {
        System.Guid _CountryMapping_Id;
        Nullable<System.Guid> _Country_Id;
        Nullable<System.Guid> _Supplier_Id;
        string _SupplierName;
        string _CountryName;
        string _CountryCode;
        string _Status;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        Nullable<int> _MapID;
        string _Name;
        string _Code;
        int _TotalRecord;
        string _Remarks;
        string _MasterCountry_Id;
        string _MasterNameWithCode;

        [DataMember]
        public string OldCountryCode { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public Nullable<System.Guid> stg_Country_Id { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public Guid CountryMapping_Id
        {
            get
            {
                return _CountryMapping_Id;
            }

            set
            {
                _CountryMapping_Id = value;
            }
        }

        [DataMember]
        public Guid? Country_Id
        {
            get
            {
                return _Country_Id;
            }

            set
            {
                _Country_Id = value;
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
        public int? MapID
        {
            get
            {
                return _MapID;
            }

            set
            {
                _MapID = value;
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
        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                _Code = value;
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
        public string MasterCountry_Id
        {
            get
            {
                return _MasterCountry_Id;
            }

            set
            {
                _MasterCountry_Id = value;
            }
        }
        [DataMember]
        public string MasterNameWithCode
        {
            get
            {
                return _MasterNameWithCode;
            }

            set
            {
                _MasterNameWithCode = value;
            }
        }
    }

    [DataContract]
    public class DC_CountryMappingRQ
    {
        Guid? _Supplier_Id;
        Guid? _Country_Id;
        string _Status;
        string _SupplierCountryName;
        int _PageNo;
        int _PageSize;
        string _SortBy;
        string _StatusExcept;
        string _SystemCountryName;


        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

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
        public Guid? Country_Id
        {
            get
            {
                return _Country_Id;
            }

            set
            {
                _Country_Id = value;
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
        public string SortBy
        {
            get
            {
                return _SortBy;
            }

            set
            {
                _SortBy = value;
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
    }

    [DataContract]
    public class DC_MappingMatch
    {
        [DataMember]
        public List<DC_CountryMapping> lstCountryMapping { get; set; }


        [DataMember]
        public List<DC_CityMapping> lstCityMapping { get; set; }

        [DataMember]
        public List<DC_Accomodation_ProductMapping> lstHotelMapping { get; set; }

        [DataMember]
        public DataContracts.Masters.DC_Supplier SupplierDetail { get; set; }


        [DataMember]
        public List<DataContracts.UploadStaticData.DC_SupplierImportAttributeValues> lstConfigs { get; set; }

        [DataMember]
        public int TotalPriorities { get; set; }

        [DataMember]
        public int CurrentPriority { get; set; }

        [DataMember]
        public Nullable<Int32> TotalBatch { get; set; }

        [DataMember]
        public Nullable<Int32> CurrentBatch { get; set; }

        [DataMember]
        public Nullable<Guid> File_Id { get; set; }

        [DataMember]
        public bool IsBatched { get; set; }

        [DataMember]
        public string FileMode { get; set; }

    }
}
