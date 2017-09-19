using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_MasterAttributeMapping
    {
        System.Guid _MasterAttributeMapping_Id;
        System.Guid _Supplier_Id;
        string _Supplier_Name;
        string _Supplier_Code;
        System.Guid _SystemMasterAttribute_Id;
        string _SystemMasterAttribute;
        string _SupplierMasterAttribute;
        string _Status;
        bool _IsActive;
        string _Create_User;
        System.DateTime _Create_Date;
        string _Edit_User;
        Nullable<System.DateTime> _Edit_Date;

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
        public Guid Supplier_Id
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
        public Guid SystemMasterAttribute_Id
        {
            get
            {
                return _SystemMasterAttribute_Id;
            }

            set
            {
                _SystemMasterAttribute_Id = value;
            }
        }

        [DataMember]
        public string SupplierMasterAttribute
        {
            get
            {
                return _SupplierMasterAttribute;
            }

            set
            {
                _SupplierMasterAttribute = value;
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
        public DateTime Create_Date
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
        public string SystemMasterAttribute
        {
            get
            {
                return _SystemMasterAttribute;
            }

            set
            {
                _SystemMasterAttribute = value;
            }
        }

        [DataMember]
        public string Supplier_Name
        {
            get
            {
                return _Supplier_Name;
            }

            set
            {
                _Supplier_Name = value;
            }
        }

        [DataMember]
        public string Supplier_Code
        {
            get
            {
                return _Supplier_Code;
            }

            set
            {
                _Supplier_Code = value;
            }
        }
    }

    [DataContract]
    public class DC_MasterAttributeMapping_RQ
    {
        System.Guid? _Supplier_Id;
        System.Guid? _MasterAttributeType_Id;
        int _PageNo;
        int _PageSize;

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
        public Guid? MasterAttributeType_Id
        {
            get
            {
                return _MasterAttributeType_Id;
            }

            set
            {
                _MasterAttributeType_Id = value;
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
    }

    [DataContract]
    public class DC_MasterAttributeMapping_RS
    {
        System.Guid _MasterAttributeMapping_Id;
        string _Supplier_Name;
        string _Supplier_Code;
        string _Supplier_Attribute_Type;
        string _System_Attribute_Type;
        string _Status;
        bool _IsActive;
        int _TotalRecords;

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
        public string Supplier_Name
        {
            get
            {
                return _Supplier_Name;
            }

            set
            {
                _Supplier_Name = value;
            }
        }

        [DataMember]
        public string Supplier_Code
        {
            get
            {
                return _Supplier_Code;
            }

            set
            {
                _Supplier_Code = value;
            }
        }

        [DataMember]
        public string Supplier_Attribute_Type
        {
            get
            {
                return _Supplier_Attribute_Type;
            }

            set
            {
                _Supplier_Attribute_Type = value;
            }
        }

        [DataMember]
        public string System_Attribute_Type
        {
            get
            {
                return _System_Attribute_Type;
            }

            set
            {
                _System_Attribute_Type = value;
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
    public class DC_MasterAttributeMappingAdd_RS
    {
        [DataMember]
        public DC_Message Message { get; set; }
        [DataMember]
        public Guid? AttributeMapping_Id { get; set; }
    }
}
