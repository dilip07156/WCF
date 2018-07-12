using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_SupplierRoomName_AttributeList
    {
        [DataMember]
        public string SupplierRoomTypeAttribute { get; set; }
        [DataMember]
        public string SystemAttributeKeyword { get; set; }
        [DataMember]
        public Guid SystemAttributeKeywordID { get; set; }
    }

    [DataContract]
    public class DC_SupplierRoomName_Details
    {
        [DataMember]
        public Guid RoomTypeMap_Id { get; set; }
        [DataMember]
        public string SupplierRoomName { get; set; }
        [DataMember]
        public string SupplierRoomDescription { get; set; }
        [DataMember]
        public string TX_SupplierRoomName { get; set; }
        [DataMember]
        public string TX_SupplierRoomName_Stripped { get; set; }
        [DataMember]
        public string TX_SupplierRoomName_Stripped_ReOrdered { get; set; }
        [DataMember]
        public List<DC_SupplierRoomName_AttributeList> AttributeList { get; set; }
    }
}
