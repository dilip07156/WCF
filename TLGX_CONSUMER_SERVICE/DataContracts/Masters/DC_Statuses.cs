using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_Statuses
    {
        [DataMember]
        public Guid Status_ID { get; set; }
        [DataMember]
        public string Status_Name { get; set; }
        [DataMember]
        public string Status_Short { get; set; }
        [DataMember]
        public DateTime? CREATE_DATE { get; set; }
        [DataMember]
        public string CREATE_USER { get; set; }
        [DataMember]
        public DateTime? UPDATE_DATE { get; set; }
        [DataMember]
        public string UPDATE_USER { get; set; }
    }
}
