using System;
using System.Runtime.Serialization;


namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFile_VerboseLog
    {
        [DataMember]
        public System.Guid SupplierImportFile_VerboseLog_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }

        [DataMember]
        public string Step { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Nullable<System.DateTime> TimeStamp { get; set; }
    }
}
