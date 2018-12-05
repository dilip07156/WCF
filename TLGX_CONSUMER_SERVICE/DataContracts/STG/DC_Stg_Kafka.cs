using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.STG
{
    [DataContract]
    public class DC_Stg_Kafka
    {

        [DataMember]
        public System.Guid Row_Id { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Topic { get; set; }

        [DataMember]
        public string PayLoad { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Offset { get; set; }

        [DataMember]
        public string Partion { get; set; }

        [DataMember]
        public DateTime? TimeStamp { get; set; }

        [DataMember]
        public string TopicPartion { get; set; }

        [DataMember]
        public string TopicPartionOffset { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public DateTime? Create_Date { get; set; }

        [DataMember]
        public string Process_User { get; set; }

        [DataMember]
        public DateTime? Process_Date { get; set; }

    }
}
