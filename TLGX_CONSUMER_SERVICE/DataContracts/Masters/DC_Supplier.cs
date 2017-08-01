using System;
using System.Runtime.Serialization;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_Supplier
    {
        Guid? _Supplier_Id;
        string _Name;
        string _Code;
        DateTime? _Create_Date;
        string _Create_User;
        DateTime? _Edit_Date;
        string _Edit_User;
        string _SupplierType;
        string _SupplierOwner;
        string _StatusCode;
        string _ProductCategory_ID;
        string _CategorySubType_ID;
        string _ProductCategory;
        string _CategorySubType;
        int _TotalRecords;
        Guid? _File_Id;

        [DataMember]
        public int? CurrentBatch { get; set; }
        [DataMember]
        public int? TotalBatch { get; set; }

        #region GetterSetter
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
        public string SupplierType
        {
            get
            {
                return _SupplierType;
            }

            set
            {
                _SupplierType = value;
            }
        }
        [DataMember]
        public string SupplierOwner
        {
            get
            {
                return _SupplierOwner;
            }

            set
            {
                _SupplierOwner = value;
            }
        }
        [DataMember]
        public string StatusCode
        {
            get
            {
                return _StatusCode;
            }

            set
            {
                _StatusCode = value;
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
        public string ProductCategory_ID
        {
            get
            {
                return _ProductCategory_ID;
            }

            set
            {
                _ProductCategory_ID = value;
            }
        }
        [DataMember]
        public string CategorySubType_ID
        {
            get
            {
                return _CategorySubType_ID;
            }

            set
            {
                _CategorySubType_ID = value;
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
        public string CategorySubType
        {
            get
            {
                return _CategorySubType;
            }

            set
            {
                _CategorySubType = value;
            }
        }
        [DataMember]
        public Guid? File_Id
        {
            get
            {
                return _File_Id;
            }

            set
            {
                _File_Id = value;
            }
        }
        #endregion
    }


    [DataContract]
    public class DC_Supplier_Search_RQ
    {
        Guid? _Supplier_Id;
        string _Name;
        string _Code;
        string _ProductCategory_ID;
        string _SupplierType;
        string _CategorySubType_ID;
        string _StatusCode;
        int? _PageNo;
        int? _PageSize;


        #region GetterSetter
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
        public string ProductCategory_ID
        {
            get
            {
                return _ProductCategory_ID;
            }

            set
            {
                _ProductCategory_ID = value;
            }
        }
        [DataMember]
        public string CategorySubType_ID
        {
            get
            {
                return _CategorySubType_ID;
            }

            set
            {
                _CategorySubType_ID = value;
            }
        }
        [DataMember]
        public string StatusCode
        {
            get
            {
                return _StatusCode;
            }

            set
            {
                _StatusCode = value;
            }
        }
        [DataMember]
        public int? PageNo
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
        public int? PageSize
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
        public string SupplierType
        {
            get
            {
                return _SupplierType;
            }

            set
            {
                _SupplierType = value;
            }
        }
        #endregion
    }


    [DataContract]
    public class DC_Supplier_DDL
    {
        [DataMember]
        public Guid Supplier_Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
