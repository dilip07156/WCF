using System;
using System.Runtime.Serialization;

namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportAttributes
    {
        [DataMember]
        public System.Guid SupplierImportAttribute_Id { get; set; }

        [DataMember]
        public System.Guid Supplier_Id { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public Nullable<System.DateTime> CREATE_DATE { get; set; }

        [DataMember]
        public string CREATE_USER { get; set; }

        [DataMember]
        public Nullable<System.DateTime> EDIT_DATE { get; set; }

        [DataMember]
        public string EDIT_USER { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string For { get; set; }
        [DataMember]
        public string STG_Table { get; set; }
        [DataMember]
        public string Master_Table { get; set; }
        [DataMember]
        public string Mapping_Table { get; set; }
    }

    [DataContract]
    public class DC_SupplierImportAttributes_RQ
    {
        [DataMember]
        public Nullable<System.Guid> SupplierImportAttribute_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string StatusExcept { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string For { get; set; }
    }
}
