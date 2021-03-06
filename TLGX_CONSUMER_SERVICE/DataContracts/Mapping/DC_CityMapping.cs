﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_CityMapping
    {
        System.Guid _CityMapping_Id;
        Nullable<System.Guid> _Country_Id;
        Nullable<System.Guid> _City_Id;
        string _CityName;
        string _CityCode;
        Nullable<System.Guid> _Supplier_Id;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        Nullable<int> _MapID;
        string _Status;
        int _TotalRecords;
        string _SupplierName;
        string _CountryCode;
        string _CountryName;
        string _MasterCountryCode;
        string _MasterStateName;
        string _MasterStateCode;
        string _MasterCountryName;
        string _MasterCityCode;
        string _Master_CityName;
        Nullable<Guid> _Master_CityId;
        string _Remarks;
        string _StateCode;
        string _StateName;
        string _StateNameWithCode;


        [DataMember]
        public Guid ReRunSupplierImporrtFile_Id { get; set; }

        [DataMember]
        public int ReRunBatch { get; set; }

        [DataMember]
        public Guid SupplierImporrtFile_Id { get; set; }

        [DataMember]
        public int Batch { get; set; }

        [DataMember]
        public List<DC_CityMapping_EntityCount> EntityTypeFlag { get; set; }

        [DataMember]
        public Int32? MatchedBy { get; set; }
        [DataMember]
        public string ListedService { get; set; }

        [DataMember]
        public Nullable<System.Guid> stg_City_Id { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public string oldCityName { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public Guid CityMapping_Id
        {
            get
            {
                return _CityMapping_Id;
            }

            set
            {
                _CityMapping_Id = value;
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
        public Guid? City_Id
        {
            get
            {
                return _City_Id;
            }

            set
            {
                _City_Id = value;
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
        public string MasterCountryCode
        {
            get
            {
                return _MasterCountryCode;
            }

            set
            {
                _MasterCountryCode = value;
            }
        }

        [DataMember]
        public string MasterCountryName
        {
            get
            {
                return _MasterCountryName;
            }

            set
            {
                _MasterCountryName = value;
            }
        }

        [DataMember]
        public string MasterCityCode
        {
            get
            {
                return _MasterCityCode;
            }

            set
            {
                _MasterCityCode = value;
            }
        }

        [DataMember]
        public string Master_CityName
        {
            get
            {
                return _Master_CityName;
            }

            set
            {
                _Master_CityName = value;
            }
        }
        [DataMember]
        public Guid? Master_City_id    
        {
            get
            {
                return _Master_CityId;
            }

            set
            {
                _Master_CityId = value;
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
        public string MasterStateName
        {
            get
            {
                return _MasterStateName;
            }

            set
            {
                _MasterStateName = value;
            }
        }

        [DataMember]
        public string StateNameWithCode
        {
            get
            {
                return _StateNameWithCode;
            }

            set
            {
                _StateNameWithCode = value;
            }
        }
        [DataMember]
        public string MasterStateCode
        {
            get
            {
                return _MasterStateCode;
            }

            set
            {
                _MasterStateCode = value;
            }
        }
    }

    [DataContract]
    public class DC_CityMapping_RQ
    {
        Nullable<System.Guid> _Supplier_Id;
        Nullable<System.Guid> _Country_Id;
        Nullable<System.Guid> _City_Id;
        string _CityName;
        string _CityCode;
        string _Status;
        int _PageNo;
        int _PageSize;
        string _SortBy;
        //string _SortOrder;
        string _SupplierCountryName;
        string _SupplierCityName;
        string _StatusExcept;
        string _ResultSet;
        bool _IsExact;
        string _StateNameWithCode;


        [DataMember]
        public Nullable<System.Guid> CityMapping_Id { get; set; }
        [DataMember]
        public string SupplierCityCode { get; set; }

        [DataMember]
        public string EntityType { get; set; }
        [DataMember]
        public string ListedService { get; set; }

        [DataMember]
        public string CalledFromTLGX { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

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
        public Guid? City_Id
        {
            get
            {
                return _City_Id;
            }

            set
            {
                _City_Id = value;
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

        //[DataMember]
        //public string SortOrder
        //{
        //    get
        //    {
        //        return _SortOrder;
        //    }

        //    set
        //    {
        //        _SortOrder = value;
        //    }
        //}

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
        public string ResultSet
        {
            get
            {
                return _ResultSet;
            }

            set
            {
                _ResultSet = value;
            }
        }

        [DataMember]
        public bool IsExact
        {
            get
            {
                return _IsExact;
            }

            set
            {
                _IsExact = value;
            }
        }

        [DataMember]
        public string StateNameWithCode
        {
            get
            {
                return _StateNameWithCode;
            }

            set
            {
                _StateNameWithCode = value;
            }
        }
    }

    [DataContract]
    public class DC_CityMapping_EntityCount
    {
        [DataMember]
        public System.Guid EntityCityMapping_Id { get; set; }
        [DataMember]
        public System.Guid? CityMapping_Id { get; set; }
        [DataMember]
        public string EntityType { get; set; }
        [DataMember]
        public int? Count { get; set; }
    }
    [DataContract]
    public class DC_HotelListByCityCode_RQ
    {
        [DataMember]
        public string CityMapping_Id { get; set; }
        [DataMember]
        public string GoFor { get; set; }
        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int PageSize { get; set; }

    }

    [DataContract]
    public class DC_HotelListByCityCode
    {
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string CityMapping_Id { get; set; }
        [DataMember]
        public string GoFor { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }

    }


}
