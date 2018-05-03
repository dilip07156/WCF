using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SRT_ML_Request_Syntactic
    {
        [DataMember]
        public List<DC_SRT_ML_supplier_data_Syntactic> supplier_data { get; set; }
        [DataMember]
        public List<DC_SRT_ML_system_room_categories_Syntactic> system_room_categories { get; set; }
        [DataMember]
        public List<string> skip_words { get; set; }
        [DataMember]
        public string semantic_mode { get; set; } = "disabled";
    }
    [DataContract]
    public class DC_SRT_ML_supplier_data_Syntactic
    {
        [DataMember]
        public string matching_string { get; set; }
        [DataMember]
        public string Supplier_Id { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
    }
    [DataContract]
    public class DC_SRT_ML_system_room_categories_Syntactic
    {
        [DataMember]
        public string system_room_name { get; set; }
        [DataMember]
        public Guid AccommodationRoomInfo_Id { get; set; }
    }
}
