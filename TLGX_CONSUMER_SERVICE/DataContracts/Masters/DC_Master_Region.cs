using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_Master_Region
    {
        [DataMember]
        public string RegionCode { get; set; }

        [DataMember]
        public string RegionName { get; set; }
    }
}
