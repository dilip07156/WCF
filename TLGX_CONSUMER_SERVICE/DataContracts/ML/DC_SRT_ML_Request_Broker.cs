using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class SupplierRoomExtractedAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SupplierRoomType
    {
        public string MapId { get; set; }
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        public string SupplierRoomId { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomName { get; set; }
        public string TXRoomName { get; set; }
        public string SupplierRoomCategory { get; set; }
        public string SupplierRoomCategoryId { get; set; }
        public string MaxAdults { get; set; }
        public string MaxChild { get; set; }
        public string MaxInfants { get; set; }
        public string MaxGuestOccupancy { get; set; }
        public string Quantity { get; set; }
        public string RatePlan { get; set; }
        public string RatePlanCode { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProductId { get; set; }
        public string TxStrippedName { get; set; }
        public string TxReorderedName { get; set; }
        public string MappingStatus { get; set; }
        public string AccommodationRoomInfoId { get; set; }
        public string RoomSize { get; set; }
        public string BathRoomType { get; set; }
        public string RoomViewCode { get; set; }
        public string FloorName { get; set; }
        public string FloorNumber { get; set; }
        public string Amenities { get; set; }
        public string RoomLocationCode { get; set; }
        public string ChildAge { get; set; }
        public string ExtraBed { get; set; }
        public string Bedrooms { get; set; }
        public string Smoking { get; set; }
        public string BedType { get; set; }
        public string MinGuestOccupancy { get; set; }
        public string PromotionalVendorCode { get; set; }
        public string BeddingConfig { get; set; }
        public List<SupplierRoomExtractedAttribute> SupplierRoomExtractedAttributes { get; set; }
    }

    public class SupplierData
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<SupplierRoomType> SupplierRoomTypes { get; set; }
    }

    public class HotelRoomTypeMappingRequest
    {
        public string AccommodationId { get; set; }
        public string TLGXCommonHotelId { get; set; }
        public List<SupplierData> SupplierData { get; set; }
    }
    public class DC_SRT_ML_Request_Broker
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<HotelRoomTypeMappingRequest> HotelRoomTypeMappingRequests { get; set; }
    }
}
