using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataContracts
{
    [DataContract]
    public class DC_ML_DataTransfer
    {
    }
    [DataContract]
    public class DC_ML_Acco_Data
    {
        [DataMember]
        public Guid AccommodationId { get; set; }
        [DataMember]
        public int? TLGXHotelId { get; set; }
        [DataMember]
        public string AccommodationName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public string Edituser { get; set; }
    }

    [DataContract]
    public class DC_ML_MasterAccoRoomFacility_Data
    {
        [DataMember]
        public Guid Accommodation_RoomFacility_Id { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string AmenityType { get; set; }
        [DataMember]
        public string AmenityName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_user { get; set; }
    }

    [DataContract]
    public class DC_ML_MasterAccoRoomInfo_Data
    {
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public int? Legacy_Htl_Id { get; set; }
        [DataMember]
        public string RoomId { get; set; }
        [DataMember]
        public string RoomView { get; set; }
        [DataMember]
        public int? NoOfRooms { get; set; }
        [DataMember]
        public string RoomName { get; set; }
        [DataMember]
        public int? NoOfInterconnectingRooms { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string RoomSize { get; set; }
        [DataMember]
        public string RoomDecor { get; set; }
        [DataMember]
        public bool? Smoking { get; set; }
        [DataMember]
        public string FloorName { get; set; }
        [DataMember]
        public string FloorNumber { get; set; }
        [DataMember]
        public bool? MysteryRoom { get; set; }
        [DataMember]
        public string BathRoomType { get; set; }
        [DataMember]
        public string BedType { get; set; }
        [DataMember]
        public string CompanyRoomCategory { get; set; }
        [DataMember]
        public string RoomCategory { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string RoomInfo_TX { get; set; }
        public string TLGXAccoRoomId { get; set; }
    }

    [DataContract]
    public class DC_ML_SupplierAcco_RoomExtendedAttributes_Data
    {
        [DataMember]
        public Guid RoomTypeMapAttribute_Id { get; set; }
        [DataMember]
        public Guid RoomTypeMap_Id { get; set; }
        [DataMember]
        public string SupplierRoomTypeAttribute { get; set; }
        [DataMember]
        public string SystemAttributeKeyword { get; set; }
    }

    [DataContract]
    public class DC_ML_SupplierAcco_Data
    {
        [DataMember]
        public Guid Accommodation_ProductMapping_Id { get; set; }
        [DataMember]
        public Guid? Accommodation_Id { get; set; }
        [DataMember]
        public string SupplierProductReference { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public Guid SupplierId { get; set; }
        [DataMember]
        public int? TLGXHotelId { get; set; }
        [DataMember]
        public string AccommodationName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string CreateUser { get; set; }
        [DataMember]
        public DateTime? EditDate { get; set; }
        [DataMember]
        public string Edituser { get; set; }
    }

    [DataContract]
    public class DC_ML_SupplierAcco_Room_Data
    {
        public Guid Accommodation_SupplierRoomTypeMapping_Id { get; set; }
        public Guid? Accommodation_Id { get; set; }
        public Guid Supplier_Id { get; set; }
        public string SupplierName { get; set; }
        public string SupplierRoomId { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomName { get; set; }
        public string TX_RoomName { get; set; }
        public string SupplierRoomCategory { get; set; }
        public string SupplierRoomCategoryId { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Edit_Date { get; set; }
        public string Edit_User { get; set; }
        public int? MaxAdults { get; set; }
        public int? MaxChild { get; set; }
        public int? MaxInfants { get; set; }
        public int? MaxGuestOccupancy { get; set; }
        public int? Quantity { get; set; }
        public string RatePlan { get; set; }
        public string RatePlanCode { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProductId { get; set; }
        public string Tx_StrippedName { get; set; }
        public string Tx_ReorderedName { get; set; }
        public string MappingStatus { get; set; }
        public int? MapId { get; set; }
        public Guid? Accommodation_RoomInfo_Id { get; set; }
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

    [DataContract]
    public class DC_ML_RoomTypeMatch_Data
    {
        public Guid AccommodationSupplierRoomTypeMappingId { get; set; }
        public Guid? AccommodationId { get; set; }
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierRoomId { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomName { get; set; }
        public string TXRoomName { get; set; }
        public string SupplierRoomView { get; set; }
        public string SupplierRoomBedType { get; set; }
        public string SupplierRoomBedTypeCode { get; set; }
        public string SupplierRoomSmoking { get; set; }
        public string SupplierRoomCategory { get; set; }
        public string SupplierRoomCategoryId { get; set; }
        public DateTime? SupplierRoomCreateDate { get; set; }
        public string SupplierRoomCreateUser { get; set; }
        public DateTime? SupplierRoomEditDate { get; set; }
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
        public Guid? AccommodationRoomInfoId { get; set; }
        public string SupplierRoomRoomDescription { get; set; }
        public string SupplierRoomRoomSize { get; set; }
        public int? TLGXCommonHotelId { get; set; }
        public string AccoRoomId { get; set; }
        public string AccoRoomView { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AccoNoOfRooms { get; set; }
        public string AccoRoomName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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
        public DateTime? AccoCreateDate { get; set; }
        public string AccoCreateUser { get; set; }
        public DateTime? AccoEditDate { get; set; }
        public string AccoEditUser { get; set; }
        public string SimilarityIndicator { get; set; }
        public float SimilarityScore { get; set; }
        public string TLGXAccoRoomId { get; set; }

    }
}
