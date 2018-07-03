using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_SupplierAcco_Room
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_SupplierAcco_Room_Data> SupplierAccommodationRoomData { get; set; }
    }
    public class DC_ML_DL_SupplierAcco_Room_Data
    {
        public string AccommodationSupplierRoomTypeMappingId { get; set; }
        public string AccommodationId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierRoomId { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomName { get; set; }
        public string TXRoomName { get; set; }
        public string SupplierRoomCategory { get; set; }
        public string SupplierRoomCategoryId { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string EditDate { get; set; }
        public string EditUser { get; set; }
        public int? MaxAdults { get; set; }
        public int? MaxChild { get; set; }
        public int? MaxInfants { get; set; }
        public int? MaxGuestOccupancy { get; set; }
        public int? Quantity { get; set; }
        public string RatePlan { get; set; }
        public string RatePlanCode { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProductId { get; set; }
        public string TxStrippedName { get; set; }
        public string TxReorderedName { get; set; }
        public string MappingStatus { get; set; }
        public int? MapId { get; set; }
        public string AccommodationRoomInfoId { get; set; }
        public string RoomSize { get; set; }
        public string BathRoomType { get; set; }
        public string RoomViewCode { get; set; }
        public string FloorName { get; set; }
        public int? FloorNumber { get; set; }
        public string Amenities { get; set; }
        public string RoomLocationCode { get; set; }
        public int? ChildAge { get; set; }
        public string ExtraBed { get; set; }
        public string Bedrooms { get; set; }
        public string Smoking { get; set; }
        public string BedTypeCode { get; set; }
        public int? MinGuestOccupancy { get; set; }
        public string PromotionalVendorCode { get; set; }
        public string BeddingConfig { get; set; }

    }
}
