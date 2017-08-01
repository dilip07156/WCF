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
    public class DC_MappingStats
    {
        Guid _SupplierId;
        string _SupplierName;
        string _NextRun;
        List<DC_MappingStatsFor> _MappingStatsFor;

        [DataMember]
        public Guid SupplierId
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
        public List<DC_MappingStatsFor> MappingStatsFor
        {
            get
            {
                return _MappingStatsFor;
            }

            set
            {
                _MappingStatsFor = value;
            }
        }
        [DataMember]
        public string NextRun
        {
            get
            {
                return _NextRun;
            }

            set
            {
                _NextRun = value;
            }
        }
    }

    [DataContract]
    public class DC_MappingStatsFor
    {
        string _MappingFor;
        decimal _MappedPercentage;
        string _ProgressCss;
        List<DC_MappingData> _MappingData;

        [DataMember]
        public string MappingFor
        {
            get
            {
                return _MappingFor;
            }

            set
            {
                _MappingFor = value;
            }
        }

        [DataMember]
        public decimal MappedPercentage
        {
            get
            {
                return _MappedPercentage;
            }

            set
            {
                _MappedPercentage = value;
            }
        }

        [DataMember]
        public List<DC_MappingData> MappingData
        {
            get
            {
                return _MappingData;
            }

            set
            {
                _MappingData = value;
            }
        }

        [DataMember]
        public string ProgressCss
        {
            get
            {
                return _ProgressCss;
            }

            set
            {
                _ProgressCss = value;
            }
        }
    }

    [DataContract]
    public class DC_MappingData
    {
        string _Status;
        int _TotalCount;

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
        public int TotalCount
        {
            get
            {
                return _TotalCount;
            }

            set
            {
                _TotalCount = value;
            }
        }
    }

    [DataContract]
    public class DC_MappingStatsForSuppliers
    {
        //Guid _SupplierId;
        string _SupplierName;
        string _MappinFor;
        int _totalcount;
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
        public string Mappingfor
        {
            get
            {
                return _MappinFor;
            }

            set
            {
                _MappinFor = value;
            }
        }
        [DataMember]
        public int totalcount
        {
            get
            {
                return _totalcount;
            }

            set
            {
                _totalcount = value;
            }
        }
        //[DataMember]
        //public Guid SupplierId
        //{
        //    get
        //    {
        //        return _SupplierId;
        //    }

        //    set
        //    {
        //        _SupplierId = value;
        //    }
        //}
    }
    #region roll_off_reports
    [DataContract]
    public class DC_RollOffReportRule
    {

        int _hotelid;
        string _hotelname;
        string _ruleName;
        string _description;
        string _internal_Flag;
        string _lastupdatedBy;
        string _lastupdateDate;

        [DataMember]
        public int Hotelid
        {
            get
            {
                return _hotelid;
            }

            set
            {
                _hotelid = value;
            }
        }
        [DataMember]
        public string Hotelname
        {
            get
            {
                return _hotelname;
            }

            set
            {
                _hotelname = value;
            }
        }
        [DataMember]
        public string RuleName
        {
            get
            {
                return _ruleName;
            }

            set
            {
                _ruleName = value;
            }
        }
        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }
        [DataMember]
        public string Internal_Flag
        {
            get
            {
                return _internal_Flag;
            }

            set
            {
                _internal_Flag = value;
            }
        }
        [DataMember]
        public string LastupdatedBy
        {
            get
            {
                return _lastupdatedBy;
            }

            set
            {
                _lastupdatedBy = value;
            }
        }
        [DataMember]
        public string LastupdateDate
        {
            get
            {
                return _lastupdateDate;
            }

            set
            {
                _lastupdateDate = value;
            }
        }
    }
    [DataContract]
    public class DC_RollOffReportStatus
    {
        int _hotelid;
        string _hotelname;
        string _companymarket;
        string _status;
        string _validfrom;
        string _validto;
        string _reason;
        string _lastupdatedBy;
        string _lastupdateDate;
        [DataMember]
        public int Hotelid
        {
            get
            {
                return _hotelid;
            }

            set
            {
                _hotelid = value;
            }
        }
        [DataMember]
        public string Hotelname
        {
            get
            {
                return _hotelname;
            }

            set
            {
                _hotelname = value;
            }
        }
        [DataMember]
        public string Companymarket
        {
            get
            {
                return _companymarket;
            }

            set
            {
                _companymarket = value;
            }
        }
        [DataMember]
        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }
        [DataMember]
        public string Validfrom
        {
            get
            {
                return _validfrom;
            }

            set
            {
                _validfrom = value;
            }
        }
        [DataMember]
        public string Validto
        {
            get
            {
                return _validto;
            }

            set
            {
                _validto = value;
            }
        }
        [DataMember]
        public string Reason
        {
            get
            {
                return _reason;
            }

            set
            {
                _reason = value;
            }
        }
        [DataMember]
        public string LastupdatedBy
        {
            get
            {
                return _lastupdatedBy;
            }

            set
            {
                _lastupdatedBy = value;
            }
        }
        [DataMember]
        public string LastupdateDate
        {
            get
            {
                return _lastupdateDate;
            }

            set
            {
                _lastupdateDate = value;
            }
        }
    }
    [DataContract]
    public class DC_RollOffReportUpdate
    {
        int _hotelid;
        string _hotelname;
        string _hotelupdate;
        string _descriptionsource;
        string _internal_Flag;
        string _validfrom;
        string _validto;
        string _lastupdatedBy;
        string _lastupdateDate;
        [DataMember]
        public int Hotelid
        {
            get
            {
                return _hotelid;
            }

            set
            {
                _hotelid = value;
            }
        }
        [DataMember]
        public string Hotelname
        {
            get
            {
                return _hotelname;
            }

            set
            {
                _hotelname = value;
            }
        }
        [DataMember]
        public string Hotelupdate
        {
            get
            {
                return _hotelupdate;
            }

            set
            {
                _hotelupdate = value;
            }
        }
        [DataMember]
        public string Descriptionsource
        {
            get
            {
                return _descriptionsource;
            }

            set
            {
                _descriptionsource = value;
            }
        }
        [DataMember]
        public string Internal_Flag
        {
            get
            {
                return _internal_Flag;
            }

            set
            {
                _internal_Flag = value;
            }
        }
        [DataMember]
        public string Validfrom
        {
            get
            {
                return _validfrom;
            }

            set
            {
                _validfrom = value;
            }
        }
        [DataMember]
        public string Validto
        {
            get
            {
                return _validto;
            }

            set
            {
                _validto = value;
            }
        }
        [DataMember]
        public string LastupdatedBy
        {
            get
            {
                return _lastupdatedBy;
            }

            set
            {
                _lastupdatedBy = value;
            }
        }
        [DataMember]
        public string LastupdateDate
        {
            get
            {
                return _lastupdateDate;
            }

            set
            {
                _lastupdateDate = value;
            }
        }
    }
    [DataContract]
    public class DC_RollOFParams
    {
        Guid _supplierID;
        string _fromdate;
        string _todate;
        [DataMember]
        public string Fromdate
        {
            get
            {
                return _fromdate;
            }

            set
            {
                _fromdate = value;
            }
        }
        [DataMember]
        public string ToDate
        {
            get
            {
                return _todate;
            }

            set
            {
                _todate = value;
            }
        }
        [DataMember]
        public Guid SupplierID
        {
            get
            {
                return _supplierID;
            }

            set
            {
                _supplierID = value;
            }
        }
    }
    #endregion
    #region rdlc reports
    [DataContract]
    public class DC_supplierwiseUnmappedReport
    {
        [DataMember]
        public Guid _SupplierId { get; set; }
        [DataMember]
        public List<DC_UnmappedCountryReport> Unmappedcountry { get; set; }
        [DataMember]
        public List<DC_UnmappedCityReport> Unmappedcity { get; set; }
        [DataMember]
        public List<DC_unmappedProductReport> Unmappedproduct { get; set; }
        [DataMember]
        public List<DC_unmappedActivityReport> Unmappedactivity { get; set; }

    }
    [DataContract]
    public class DC_UnmappedCountryReport
    {

        string _countrycode;
        string _contryname;
        [DataMember]
        public string Countrycode
        {
            get
            {
                return _countrycode;
            }

            set
            {
                _countrycode = value;
            }
        }
        [DataMember]
        public string Contryname
        {
            get
            {
                return _contryname;
            }

            set
            {
                _contryname = value;
            }
        }
    }
    [DataContract]
    public class DC_UnmappedCityReport
    {
        string _countrycode;
        string _countryname;
        string _citycode;
        string _cityname;
        [DataMember]
        public string Countrycode
        {
            get
            {
                return _countrycode;
            }

            set
            {
                _countrycode = value;
            }
        }
        [DataMember]
        public string Countryname
        {
            get
            {
                return _countryname;
            }

            set
            {
                _countryname = value;
            }
        }
        [DataMember]
        public string Citycode
        {
            get
            {
                return _citycode;
            }

            set
            {
                _citycode = value;
            }
        }
        [DataMember]
        public string Cityname
        {
            get
            {
                return _cityname;
            }

            set
            {
                _cityname = value;
            }
        }
    }
    [DataContract]
    public class DC_unmappedProductReport
    {
        string _hotelname;
        string _country;
        string _city;
        string _address;
        string _supplierName;
        string _supplierHotelId;
        [DataMember]
        public string Hotelname
        {
            get
            {
                return _hotelname;
            }

            set
            {
                _hotelname = value;
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
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }
        [DataMember]
        public string SupplierName
        {
            get
            {
                return _supplierName;
            }

            set
            {
                _supplierName = value;
            }
        }
        [DataMember]
        public string SupplierHotelId
        {
            get
            {
                return _supplierHotelId;
            }

            set
            {
                _supplierHotelId = value;
            }
        }
    }
    [DataContract]
    public class DC_unmappedActivityReport
    {
        string _activityname;
        string _country;
        string _city;
        string _address;
        string _supplierName;
        string _supplierActivityId;
        [DataMember]
        public string Activityname
        {
            get
            {
                return _activityname;
            }

            set
            {
                _activityname = value;
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
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }
        [DataMember]
        public string SupplierName
        {
            get
            {
                return _supplierName;
            }

            set
            {
                _supplierName = value;
            }
        }
        [DataMember]
        public string SupplierActivityId
        {
            get
            {
                return _supplierActivityId;
            }

            set
            {
                _supplierActivityId = value;
            }
        }
    }
    [DataContract]
    public class DC_supplierwisesummaryReport
    {
        int _city;
        int _product;
        int _hotelrooom;
        int _activity;
        int _country;
        string _suppliername;
        [DataMember]
        public string Suppliername
        {
            get
            {
                return _suppliername;
            }

            set
            {
                _suppliername = value;
            }
        }
        [DataMember]
        public int City
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
        public int Product
        {
            get
            {
                return _product;
            }

            set
            {
                _product = value;
            }
        }
        [DataMember]
        public int Hotelrooom
        {
            get
            {
                return _hotelrooom;
            }

            set
            {
                _hotelrooom = value;
            }
        }
        [DataMember]
        public int Activity
        {
            get
            {
                return _activity;
            }

            set
            {
                _activity = value;
            }
        }
        [DataMember]
        public int Country
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
    }
    [DataContract]
    public class DC_supplierwiseunmappedsummaryReport
    {
        string _suppliername;
        string _countryname;
        string _cityname;
        int _noofproducts;
        [DataMember]
        public string Suppliername
        {
            get
            {
                return _suppliername;
            }

            set
            {
                _suppliername = value;
            }
        }
        [DataMember]
        public string Countryname
        {
            get
            {
                return _countryname;
            }

            set
            {
                _countryname = value;
            }
        }
        [DataMember]
        public string Cityname
        {
            get
            {
                return _cityname;
            }

            set
            {
                _cityname = value;
            }
        }
        [DataMember]
        public int Noofproducts
        {
            get
            {
                return _noofproducts;
            }

            set
            {
                _noofproducts = value;
            }
        }
    }
    #endregion
    #region hotel report
    [DataContract]
    public class DC_newHotelsReport
    {
        int _hotelid;
        string _hotelname;
        string _country;
        string _city;
        string _createdate;
        string _createdby;
        [DataMember]
        public int Hotelid
        {
            get
            {
                return _hotelid;
            }

            set
            {
                _hotelid = value;
            }
        }
        [DataMember]
        public string Hotelname
        {
            get
            {
                return _hotelname;
            }

            set
            {
                _hotelname = value;
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
        public string Createdate
        {
            get
            {
                return _createdate;
            }

            set
            {
                _createdate = value;
            }
        }
        [DataMember]
        public string Createdby
        {
            get
            {
                return _createdby;
            }

            set
            {
                _createdby = value;
            }
        }
    }
    #endregion
    #region velocity dash
    [DataContract]
    public class DC_VelocityMappingStats
    {
        Guid _SupplierId;
        string _SupplierName;
        List<DC_VelocityMappingStatsFor> _MappingStatsFor;
        [DataMember]
        public Guid SupplierId
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
        public List<DC_VelocityMappingStatsFor> MappingStatsFor
        {
            get
            {
                return _MappingStatsFor;
            }

            set
            {
                _MappingStatsFor = value;
            }
        }
    }
    [DataContract]
    public class DC_VelocityMappingdata
    {
        string _username;
        int _totalcount;
        int _sequence;
        [DataMember]
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }
        [DataMember]
        public int Totalcount
        {
            get
            {
                return _totalcount;
            }

            set
            {
                _totalcount = value;
            }
        }
        [DataMember]
        public int Sequence
        {
            get
            {
                return _sequence;
            }

            set
            {
                _sequence = value;
            }
        }
    }
    [DataContract]
    public class DC_VelocityMappingStatsFor
    {
         string _MappingFor;
        int _unmappeddata;
        List<DC_VelocityMappingdata> _MappingData;
        [DataMember]
        public string MappingFor
        {
            get
            {
                return _MappingFor;
            }

            set
            {
                _MappingFor = value;
            }
        }
        [DataMember]
        public List<DC_VelocityMappingdata> MappingData
        {
            get
            {
                return _MappingData;
            }

            set
            {
                _MappingData = value;
            }
        }
        [DataMember]
        public int Unmappeddata
        {
            get
            {
                return _unmappeddata;
            }

            set
            {
                _unmappeddata = value;
            }
        }
    }
}
#endregion