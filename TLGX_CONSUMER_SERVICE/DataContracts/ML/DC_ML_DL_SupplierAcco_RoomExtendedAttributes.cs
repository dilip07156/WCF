using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_SupplierAcco_RoomExtendedAttributes
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data> SupplierAccommodationRoomExtendedAttributes { get; set; }
    }
    public class DC_ML_DL_SupplierAcco_RoomExtendedAttributes_Data
    {
        public string RoomTypeMapAttributeId { get; set; }
        public string RoomTypeMapId { get; set; }
        public string SupplierRoomTypeAttribute { get; set; }
        public string SystemAttributeKeyword { get; set; }
    }
}
