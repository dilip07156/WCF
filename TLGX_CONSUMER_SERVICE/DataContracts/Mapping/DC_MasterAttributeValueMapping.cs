using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_MasterAttributeValueMapping
    {
        System.Guid? _MasterAttributeValueMapping_Id;
        System.Guid _MasterAttributeMapping_Id;
        System.Guid _SystemMasterAttributeValue_Id;
        string _SystemMasterAttributeValue;
        string _SupplierMasterAttributeValue;
        bool? _IsActive;
        string _Create_User;
        System.DateTime? _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;
        int _TotalRecords;
        [DataMember]
        public Guid? MasterAttributeValueMapping_Id
        {
            get
            {
                return _MasterAttributeValueMapping_Id;
            }

            set
            {
                _MasterAttributeValueMapping_Id = value;
            }
        }

        [DataMember]
        public Guid MasterAttributeMapping_Id
        {
            get
            {
                return _MasterAttributeMapping_Id;
            }

            set
            {
                _MasterAttributeMapping_Id = value;
            }
        }

        [DataMember]
        public Guid SystemMasterAttributeValue_Id
        {
            get
            {
                return _SystemMasterAttributeValue_Id;
            }

            set
            {
                _SystemMasterAttributeValue_Id = value;
            }
        }

        [DataMember]
        public string SupplierMasterAttributeValue
        {
            get
            {
                return _SupplierMasterAttributeValue;
            }

            set
            {
                _SupplierMasterAttributeValue = value;
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

        [DataMember]
        public string SystemMasterAttributeValue
        {
            get
            {
                return _SystemMasterAttributeValue;
            }

            set
            {
                _SystemMasterAttributeValue = value;
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
        
    }
    [DataContract]
    public class DC_MasterAttributeValueMappingRS
    {
        System.Guid _MasterAttributeMapping_Id;
        System.Guid _SystemMasterAttributeValue_Id;
        string _SystemMasterAttributeValue;
        
        int _TotalRecords;
        List<DC_SupplierAttributeValues> _SupplierAttributeValues;

        [DataMember]
        public Guid MasterAttributeMapping_Id
        {
            get
            {
                return _MasterAttributeMapping_Id;
            }

            set
            {
                _MasterAttributeMapping_Id = value;
            }
        }

        [DataMember]
        public Guid SystemMasterAttributeValue_Id
        {
            get
            {
                return _SystemMasterAttributeValue_Id;
            }

            set
            {
                _SystemMasterAttributeValue_Id = value;
            }
        }

        [DataMember]
        public string SystemMasterAttributeValue
        {
            get
            {
                return _SystemMasterAttributeValue;
            }

            set
            {
                _SystemMasterAttributeValue = value;
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
        public List<DC_SupplierAttributeValues> SupplierAttributeValues
        {
            get
            {
                return _SupplierAttributeValues;
            }

            set
            {
                _SupplierAttributeValues = value;
            }
        }
    }

    [DataContract]
    public class DC_MasterAttributeValueMapping_RQ
    {
        System.Guid? _MasterAttributeMapping_Id;
        System.Guid? _SystemMasterAttributeValue_Id;
        string _SystemMasterAttributeValue;
        int _PageNo;
        int _PageSize;

        [DataMember]
        public Guid? MasterAttributeMapping_Id
        {
            get
            {
                return _MasterAttributeMapping_Id;
            }

            set
            {
                _MasterAttributeMapping_Id = value;
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
        public string SystemMasterAttributeValue
        {
            get
            {
                return _SystemMasterAttributeValue;
            }

            set
            {
                _SystemMasterAttributeValue = value;
            }
        }

        [DataMember]
        public Guid? SystemMasterAttributeValue_Id
        {
            get
            {
                return _SystemMasterAttributeValue_Id;
            }

            set
            {
                _SystemMasterAttributeValue_Id = value;
            }
        }
    }
    [DataContract]
    public class DC_SupplierAttributeValues
    {
        System.Guid? _MasterAttributeValueMapping_Id;
        string _SupplierMasterAttributeValue;
        bool? _IsActive;
        string _Create_User;
        System.DateTime? _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;

        [DataMember]
        public Guid? MasterAttributeValueMapping_Id
        {
            get
            {
                return _MasterAttributeValueMapping_Id;
            }

            set
            {
                _MasterAttributeValueMapping_Id = value;
            }
        }

        [DataMember]
        public string SupplierMasterAttributeValue
        {
            get
            {
                return _SupplierMasterAttributeValue;
            }

            set
            {
                _SupplierMasterAttributeValue = value;
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
}
