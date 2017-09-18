using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_SupplierRoomType_TTFU_RQ
    {
        [DataMember]
        public Guid Acco_RoomTypeMap_Id { get; set; }
        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public Nullable<Guid> File_Id { get; set; }

        [DataMember]
        public int? CurrentBatch { get; set; }

        [DataMember]
        public int? TotalBatch { get; set; }
    }
}
