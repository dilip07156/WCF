using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.FileTransfer
{
    [DataContract]
    public class DC_FileData
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public byte[] BufferData { get; set; }
        [DataMember]
        public long FilePostition { get; set; }
    }
}
