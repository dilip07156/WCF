using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    [DataContract]
    public class DL_ML_DL_EntityStatus
    {
        [DataMember]
        public string EntityType { get; set; }
        [DataMember]
        public DateTime LastUpdate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string TotalCount { get; set; }
        [DataMember]
        public string PushedCount { get; set; }
        [DataMember]
        public decimal Per { get; set; }

    }
}
