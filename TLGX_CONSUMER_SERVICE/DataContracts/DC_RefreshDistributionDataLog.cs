using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts
{
    [DataContract]
    public class DC_RefreshDistributionDataLog
    {
        [DataMember]
        public string Element { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string SupplierId { get; set; }
    }
    [DataContract]
    public class DC_MogoDbSyncRQ
    {
        [DataMember]
        public string Element { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
