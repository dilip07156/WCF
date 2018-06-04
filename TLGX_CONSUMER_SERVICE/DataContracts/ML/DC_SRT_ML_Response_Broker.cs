using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class ProposedMapping
    {
        public string AccommodationRoomInfoId { get; set; }
        public string RoomName { get; set; }
        public double Score { get; set; }
        public string Status { get; set; }
    }

    public class RS_SupplierRoomType
    {
        public string SupplierRoomId { get; set; }
        public string SupplierString { get; set; }
        public string MapId { get; set; }
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        public List<ProposedMapping> ProposedMappings { get; set; }
    }

    public class RS_SupplierData
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    public class HotelRoomTypeMappingRespons
    {
        public string AccommodationId { get; set; }
        public string TLGXCommonHotelId { get; set; }
        public List<SupplierData> SupplierData { get; set; }
    }
    public class DC_SRT_ML_Response_Broker
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<HotelRoomTypeMappingRespons> HotelRoomTypeMappingResponses { get; set; }
    }
}
