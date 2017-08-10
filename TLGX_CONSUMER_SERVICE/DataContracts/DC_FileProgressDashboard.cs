using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
     public class DC_FileProgressDashboard
    {
        [DataMember]
        public List<UploadStaticData.DC_SupplierImportFile_Progress> ProgressLog { get; set; }
        [DataMember]
        public List<UploadStaticData.DC_SupplierImportFile_VerboseLog> VerboseLog { get; set; }
        [DataMember]
        public List<UploadStaticData.DC_SupplierImportFileDetails> FileDetails { get; set; }
    }
}
