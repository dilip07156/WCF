using System;
using System.Runtime.Serialization;
namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFileDetails
    {
        [DataMember]
        public System.Guid SupplierImportFile_Id { get; set; }

        [DataMember]
        public System.Guid Supplier_Id { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string OriginalFilePath { get; set; }

        [DataMember]
        public string SavedFilePath { get; set; }

        [DataMember]
        public string ArchiveFilePath { get; set; }

        [DataMember]
        public string STATUS { get; set; }

        [DataMember]
        public Nullable<System.DateTime> CREATE_DATE { get; set; }

        [DataMember]
        public string CREATE_USER { get; set; }

        [DataMember]
        public Nullable<System.DateTime> PROCESS_DATE { get; set; }

        [DataMember]
        public string PROCESS_USER { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public string Mode { get; set; }
    }

    [DataContract]
    public class DC_SupplierImportFileDetails_RQ
    {
        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string STATUS { get; set; }

        [DataMember]
        public string StatusExcept { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public Nullable<System.DateTime> From_Date { get; set; }

        [DataMember]
        public Nullable<System.DateTime> TO_Date { get; set; }

        [DataMember]
        public string Mode { get; set; }
    }
}
