using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public class DC_ProposedMapping
    {
        public string AccommodationRoomInfoId { get; set; }
        public string RoomName { get; set; }
        public double Score { get; set; }
        public string Status { get; set; }
    }

    public class DC_RS_SupplierRoomType
    {
        public string SupplierRoomId { get; set; }
        public string SupplierString { get; set; }
        public string MapId { get; set; }
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        public List<DC_ProposedMapping> ProposedMappings { get; set; }
    }

    public class DC_RS_SupplierData
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<DC_RS_SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    public class DC_HotelRoomTypeMappingResponse
    {
        public string AccommodationId { get; set; }
        public string TLGXCommonHotelId { get; set; }
        public List<DC_RS_SupplierData> SupplierData { get; set; }
    }

    public class DC_SRT_ML_Response_Broker
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_HotelRoomTypeMappingResponse> HotelRoomTypeMappingResponses { get; set; }
    }
}
