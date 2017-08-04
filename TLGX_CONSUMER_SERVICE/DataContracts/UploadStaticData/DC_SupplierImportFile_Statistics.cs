using System;
using System.Runtime.Serialization;


namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFile_Statistics
    {
        [DataMember]
        public System.Guid SupplierImportFile_Statistics_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }

        [DataMember]
        public string FinalStatus { get; set; }

        [DataMember]
        public int? TotalRows { get; set; }

        [DataMember]
        public int? Mapped { get; set; }

        [DataMember]
        public int? Unmapped { get; set; }

        [DataMember]
        public Nullable<DateTime> Process_Date { get; set; }

        [DataMember]
        public string Process_User { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string Supplier { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string ProcessedBy { get; set; }


    }

    [DataContract]
    public class DC_SupplierImportFile_Statistics_RQ
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
        public int? PageNo { get; set; }

        [DataMember]
        public int? PageSize { get; set; }

        [DataMember]
        public Nullable<System.DateTime> From_Date { get; set; }

        [DataMember]
        public Nullable<System.DateTime> TO_Date { get; set; }
    }
}
