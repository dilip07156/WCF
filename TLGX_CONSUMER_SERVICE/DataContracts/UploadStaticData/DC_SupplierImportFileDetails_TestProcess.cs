using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.UploadStaticData
{
    [DataContract]
    public class DC_SupplierImportFileDetails_TestProcess
    {
        [DataMember]
        public System.Guid SupplierImportFile_Id { get; set; }

        [DataMember]
        public System.Guid Supplier_Id { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public string SavedFilePath { get; set; }

        [DataMember]
        public string STATUS { get; set; }

        [DataMember]
        public string PROCESS_USER { get; set; }

        [DataMember]
        public int No_Of_Records_ToProcess { get; set; }

        [DataMember]
        public DataSet Data { get; set; }
    }
}
