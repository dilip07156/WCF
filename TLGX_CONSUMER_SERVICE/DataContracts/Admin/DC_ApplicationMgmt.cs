using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_ApplicationMgmt
    {
        [DataMember]
        public Guid ApplicationId { get; set; }
        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
    }
}
