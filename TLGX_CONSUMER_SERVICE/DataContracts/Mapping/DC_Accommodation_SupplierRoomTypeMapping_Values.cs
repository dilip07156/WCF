using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accommodation_SupplierRoomTypeMapping_Values
    {
        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMapping_Value_Id { get; set; }
        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMapping_Id { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public string UserMappingStatus { get; set; }
        [DataMember]
        public string SystemMappingStatus { get; set; }
        [DataMember]
        public System.Guid? Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string Created_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public string Edit_SystemUser { get; set; }
        [DataMember]
        public string MatchingScore { get; set; }
        [DataMember]
        public Guid? Accommodation_Id { get; set; }
    }
}
