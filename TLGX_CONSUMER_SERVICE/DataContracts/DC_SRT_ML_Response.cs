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
        public string Supplier_Id { get; set; }
        [DataMember]
        public string Accommodation_Id { get; set; }
        [DataMember]
        public string matching_string { get; set; }
        [DataMember]
        public List<DC_SRT_ML_Match> matches { get; set; }
        [DataMember]
        public List<DC_SRT_ML_AccommodationRoomInfo> AccommodationRoomInfo_Id { get; set; }
    }

    [DataContract]
    public class DC_SRT_ML_Match
    {
        [DataMember]
        public string matched_string { get; set; }
        [DataMember]
        public int score { get; set; }
    }
    [DataContract]
    public class DC_SRT_ML_AccommodationRoomInfo
    {
        [DataMember]
        public string AccommodationRoomInfo_Id { get; set; }
        [DataMember]
        public string system_room_name { get; set; }
    }

}
