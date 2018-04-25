using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SRT_ML_Response
    {
        [DataMember]
        public string supplier_id { get; set; }
        [DataMember]
        public string product_id { get; set; }
        [DataMember]
        public string matching_string { get; set; }
        [DataMember]
        public Dictionary<string, string> matches { get; set; }
        //[DataMember]
        //public List<Dictionary<string, string>> system_room_ids { get; set; }
    }
}
