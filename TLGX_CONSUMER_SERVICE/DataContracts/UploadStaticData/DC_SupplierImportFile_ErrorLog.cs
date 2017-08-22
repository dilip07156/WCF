using System;
using System.Runtime.Serialization;

namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFile_ErrorLog
    {
        [DataMember]
        public System.Guid SupplierImportFile_ErrorLog_Id { get; set; }

        [DataMember]
        public System.Guid SupplierImportFile_Id { get; set; }

        [DataMember]
        public int? ErrorCode { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public string ErrorType { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Error_DATE { get; set; }

        [DataMember]
        public string ErrorMessage_UI { get; set; }

        [DataMember]
        public string Error_USER { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }

    [DataContract]
    public class DC_SupplierImportFile_ErrorLog_RQ
    {
        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_ErrorLog_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorType { get; set; }

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
        public int? PageSize { get; set; }

    }
}
