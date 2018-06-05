using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_ProposedMapping
    {
        [DataMember]
        public string AccommodationRoomInfoId { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public double Score { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    [DataContract]
    public class DC_RS_SupplierRoomType
    {
        [DataMember]
        public string SupplierRoomId { get; set; }
        [DataMember]
        public string SupplierString { get; set; }
        [DataMember]
        public string MapId { get; set; }
        [DataMember]
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        [DataMember]
        public List<DC_ProposedMapping> ProposedMappings { get; set; }
    }

    [DataContract]
    public class DC_RS_SupplierData
    {
        [DataMember]
        public string SupplierId { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public List<DC_RS_SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    [DataContract]
    public class DC_HotelRoomTypeMappingResponse
    {
        [DataMember]
        public string AccommodationId { get; set; }
        [DataMember]
        public string TLGXCommonHotelId { get; set; }
        [DataMember]
        public List<DC_RS_SupplierData> SupplierData { get; set; }
    }

    [DataContract]
    public class DC_SRT_ML_Response_Broker
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_HotelRoomTypeMappingResponse> HotelRoomTypeMappingResponses { get; set; }
    }
}
