﻿using System;
using System.Runtime.Serialization;


namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFile_Progress
    {
        [DataMember]
        public System.Guid SupplierImportFileProgress_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }

        [DataMember]
        public string Step { get; set; }

        [DataMember]
        public Nullable<int> PercentageValue { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
