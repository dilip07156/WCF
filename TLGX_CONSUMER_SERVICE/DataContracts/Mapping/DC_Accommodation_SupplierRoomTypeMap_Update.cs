using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accommodation_SupplierRoomTypeMap_Update
    {
        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMapping_Id { get; set; }
        [DataMember]
        public Guid? Accommodation_Id { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public System.Guid? Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public List<DC_SupplierRoomTypeAttributes> RoomTypeAttributes { get; set; }
        [DataMember]
        public string Edit_User { get; set; }


    }
}
