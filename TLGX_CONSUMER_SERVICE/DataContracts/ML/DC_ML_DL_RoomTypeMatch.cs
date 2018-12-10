using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.ML
{
    public class DC_ML_DL_RoomTypeMatch
    {
        public string Mode { get; set; }
        public string BatchId { get; set; }
        public string Transaction { get; set; }
        public List<DC_ML_DL_RoomTypeMatch_Data> RoomTypeMatching { get; set; }
    }

    public class DC_ML_RoomTypeMatch_ExtractedAttributes
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class DC_ML_DL_RoomTypeMatch_Data
    {
        public string AccommodationSupplierRoomTypeMappingId { get; set; }				
        public string AccommodationId { get; set; }										
        public string SupplierId { get; set; }											
        public string SupplierName { get; set; }										
        public string SupplierRoomId { get; set; }										
        public string SupplierRoomTypeCode { get; set; }								
        public string SupplierRoomName { get; set; }									
        public string TXRoomName { get; set; }
        public string SupplierRoomView { get; set; }
        public string SupplierRoomBedType { get; set; }
        public string SupplierRoomSmoking { get; set; }
        public List<DC_ML_RoomTypeMatch_ExtractedAttributes> SupplierRoomExtractedAttributes { get; set; }
        public string SupplierRoomCategory { get; set; }								
        public string SupplierRoomCategoryId { get; set; }								
        public string SupplierRoomCreateDate { get; set; }								
        public string SupplierRoomCreateUser { get; set; }								
        public string SupplierRoomEditDate { get; set; }								
        public string SupplierRoomEditUser { get; set; }								
        public int? SupplierRoomMaxAdults { get; set; }								
        public int? SupplierRoomMaxChild { get; set; }								
        public int? SupplierRoomMaxInfants { get; set; }								
        public int? MaxGuestOccupancy { get; set; }									
        public int? SupplierRoomQuantity { get; set; }								
        public string SupplierRoomRatePlan { get; set; }								
        public string RatePlanCode { get; set; }										
        public string SupplierRoomSupplierProductName { get; set; }						
        public string SupplierRoomSupplierProductId { get; set; }						
        public string TxStrippedName { get; set; }										
        public string TxReorderedName { get; set; }										
        public string SupplierRoomMappingStatus { get; set; }							
        public long? MapId { get; set; }												
        public string AccommodationRoomInfoId { get; set; }								
        public string SupplierRoomRoomDescription { get; set; }							
        public string SupplierRoomRoomSize { get; set; }								
        public int? TLGXCommonHotelId { get; set; }									
        public string AccoRoomId { get; set; }											
        public string AccoRoomView { get; set; }										
        public int? AccoNoOfRooms { get; set; }										
        public string AccoRoomName { get; set; }										
        public int? AccoNoOfInterconnectingRooms { get; set; }						
        public string AccoDescription { get; set; }										
        public string AccoRoomSize { get; set; }										
        public string AccoRoomDecor { get; set; }										
        public bool AccoSmoking { get; set; }											
        public string AccoFloorName { get; set; }										
        public string AccoFloorNumber { get; set; }										
        public bool AccoMysteryRoom { get; set; }										
        public string AccoBathRoomType { get; set; }									
        public string AccoBedType { get; set; }											
        public string AccoCompanyRoomCategory { get; set; }								
        public string AccoRoomCategory { get; set; }									
        public string AccoCreateDate { get; set; }										
        public string AccoCreateUser { get; set; }										
        public string AccoEditDate { get; set; }										
        public string AccoEditUser { get; set; }										
        public bool SimilarityIndicator { get; set; }									
        public double SimilarityScore { get; set; }
        public string TLGXAccoRoomId { get; set; }

    }
}
