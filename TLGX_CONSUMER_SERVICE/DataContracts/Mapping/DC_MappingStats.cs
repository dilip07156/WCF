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

    public class DC_RollOFParams
    {
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
    }
}
