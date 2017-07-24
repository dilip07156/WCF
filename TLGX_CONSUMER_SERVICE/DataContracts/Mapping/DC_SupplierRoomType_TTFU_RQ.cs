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
        public Guid Acco_RoomTypeMap_Id { get; set; }
        public string Edit_User { get; set; }
    }
}
