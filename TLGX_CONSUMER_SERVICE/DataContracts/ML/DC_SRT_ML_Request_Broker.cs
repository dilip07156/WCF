using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SRT_ML_Request_Broker
    {
        [DataMember]
        public string Mode { get; set; }
        [DataMember]
        public string BatchId { get; set; }
        [DataMember]
        public string Transaction { get; set; }
        [DataMember]
        public List<DC_HotelRoomTypeMappingRequest> HotelRoomTypeMappingRequests { get; set; }
    }
    [DataContract]
    public class DC_HotelRoomTypeMappingRequest
    {
        [DataMember]
        public string AccommodationId { get; set; }
        [DataMember]
        public string TLGXCommonHotelId { get; set; }
        [DataMember]
        public List<DC_SupplierData> SupplierData { get; set; }
    }
    [DataContract]
    public class DC_SupplierData
    {
        [DataMember]
        public string SupplierId { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public List<DC_SupplierRoomType> SupplierRoomTypes { get; set; }
    }
    [DataContract]
    public class DC_SupplierRoomType
    {
        [DataMember]
        public string MapId { get; set; }
        [DataMember]
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        [DataMember]
        public string SupplierRoomId { get; set; }
        [DataMember]
        public string SupplierRoomTypeCode { get; set; }
        [DataMember]
        public string SupplierRoomName { get; set; }
        [DataMember]
        public string TXRoomName { get; set; }
        [DataMember]
        public string SupplierRoomCategory { get; set; }
        [DataMember]
        public string SupplierRoomCategoryId { get; set; }
        [DataMember]
        public string MaxAdults { get; set; }
        [DataMember]
        public string MaxChild { get; set; }
        [DataMember]
        public string MaxInfants { get; set; }
        [DataMember]
        public string MaxGuestOccupancy { get; set; }
        [DataMember]
        public string Quantity { get; set; }
        [DataMember]
        public string RatePlan { get; set; }
        [DataMember]
        public string RatePlanCode { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string SupplierProductId { get; set; }
        [DataMember]
        public string TxStrippedName { get; set; }
        [DataMember]
        public string TxReorderedName { get; set; }
        [DataMember]
        public string MappingStatus { get; set; }
        [DataMember]
        public string AccommodationRoomInfoId { get; set; }
        [DataMember]
        public string RoomSize { get; set; }
        [DataMember]
        public string BathRoomType { get; set; }
        [DataMember]
        public string RoomViewCode { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public string FloorNumber { get; set; }
        [DataMember]
        public string Amenities { get; set; }
        [DataMember]
        public string RoomLocationCode { get; set; }
        [DataMember]
        public string ChildAge { get; set; }
        [DataMember]
        public string ExtraBed { get; set; }
        [DataMember]
        public string Bedrooms { get; set; }
        [DataMember]
        public string Smoking { get; set; }
        [DataMember]
        public string BedType { get; set; }
        [DataMember]
        public string MinGuestOccupancy { get; set; }
        [DataMember]
        public string PromotionalVendorCode { get; set; }
        [DataMember]
        public string BeddingConfig { get; set; }
        [DataMember]
        public List<DC_SupplierRoomExtractedAttribute> SupplierRoomExtractedAttributes { get; set; }
    }
    [DataContract]
    public class DC_SupplierRoomExtractedAttribute
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    

    

  
   
}
