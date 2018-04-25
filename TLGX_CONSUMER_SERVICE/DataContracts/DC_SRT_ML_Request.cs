using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SRT_ML_Request
    {
        [DataMember]
        public List<DC_SRT_ML_supplier_data> supplier_data { get; set; }
        [DataMember]
        public List<DC_SRT_ML_system_room_categories> system_room_categories { get; set; }
        [DataMember]
        public List<string> skip_words { get; set; }
        [DataMember]
        public string semantic_mode { get; set; } = "disabled";
    }
    [DataContract]
    public class DC_SRT_ML_supplier_data
    {
        [DataMember]
        public string matching_string { get; set; }
        [DataMember]
        public string supplier_id { get; set; }
        [DataMember]
        public string product_id { get; set; }
    }
    [DataContract]
    public class DC_SRT_ML_system_room_categories
    {
        [DataMember]
        public string system_room_name { get; set; }
        [DataMember]
        public string system_room_id { get; set; }
    }
}
