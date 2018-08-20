using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    public class DC_Accommodation_SupplierRoomTypeMapping_Online
    {
        /// <summary>
        /// Unique id of the document
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }
        /// <summary>
        /// This field specifies the type of room type mapping requests. You should always set this value to "online".
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// Please generate a unique Batch / Tracking ID to allow end to end tracebility.
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// TLGX Accommodation CommonHotelId - TLGX MDM unique identifier for a hotel property. This value can be retrived by using ProductMappingLite api.
        /// </summary>
        public string TLGXCommonHotelId { get; set; }
        /// <summary>
        /// Nakshatra mapping system's SupplierCode. This can be retreieved by using SupplierMaster api.
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// Supplier Product code for accommodation.
        /// </summary>
        public string SupplierProductId { get; set; }
        /// <summary>
        /// Supplier system room id.
        /// </summary>
        public string SupplierRoomId { get; set; }
        /// <summary>
        /// Supplier room type code. Certain suppliers use both id and code values.
        /// </summary>
        public string SupplierRoomTypeCode { get; set; }
        /// <summary>
        /// Supplier room type name.
        /// </summary>
        public string SupplierRoomName { get; set; }
        /// <summary>
        /// Supplier room category name. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategory { get; set; }
        /// <summary>
        ///  Supplier room category id. Certain suppliers use both room type and category.
        /// </summary>
        public string SupplierRoomCategoryId { get; set; }
        /// <summary>
        /// The maximum number of adults permitted in the room stay.
        /// </summary>
        public string MaxAdults { get; set; }
        /// <summary>
        /// The maximum number of children permitted in the room stay.
        /// </summary>
        public string MaxChild { get; set; }
        /// <summary>
        /// The maximum number of infants permitted in the room stay.
        /// </summary>
        public string MaxInfants { get; set; }
        /// <summary>
        /// The maximum number of total occupancy permitted in the room stay.
        /// </summary>
        public string MaxGuestOccupancy { get; set; }
        /// <summary>
        /// Quantity of suppier rooms of this type.
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Specific rate plan name for this room stay (For future use).
        /// </summary>
        public string RatePlan { get; set; }
        /// <summary>
        /// Specific rate plan code for this room stay (For future use).
        /// </summary>
        public string RatePlanCode { get; set; }
        /// <summary>
        /// The size of the room
        /// </summary>
        public string RoomSize { get; set; }
        /// <summary>
        /// The type of bathroom in the room stay.
        /// </summary>
        public string BathRoomType { get; set; }
        /// <summary>
        /// The type of room view. Accepts either code or name.
        /// </summary>
        public string RoomView { get; set; }
        /// <summary>
        /// The name of the floor if specified by the supplier.
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// The number of the floor if specified by the supplier.
        /// </summary>
        public string FloorNumber { get; set; }
        /// <summary>
        /// Amenities of the room stay if specified by the supplier (For future use).
        /// </summary>
        public string[] Amenities { get; set; }
        /// <summary>
        /// Any specific room location code if specified by the supplier.
        /// </summary>
        public string RoomLocationCode { get; set; }
        /// <summary>
        /// Maximum child age for this room stay if specified by the supplier.
        /// </summary>
        public string ChildAge { get; set; }
        /// <summary>
        /// if extra bed applicable for this room stay if specified by the supplier.
        /// </summary>
        public string ExtraBed { get; set; }
        /// <summary>
        /// The number of bedrooms for this room stay if specified by the supplier.
        /// </summary>
        public string Bedrooms { get; set; }
        /// <summary>
        /// Is Smoking permitted in this room stay if specified by the supplier.
        /// </summary>
        public string Smoking { get; set; }
        /// <summary>
        /// The type of bed for this room stay if specified by the supplier.
        /// </summary>
        public string BedType { get; set; }
        /// <summary>
        /// The minimum guest occupancy for this room stay if specified by the supplier.
        /// </summary>
        public string MinGuestOccupancy { get; set; }
        /// <summary>
        /// Any promotional vendor code for this room stay if specified by the supplier (For future use).
        /// </summary>
        public string PromotionalVendorCode { get; set; }
        /// <summary>
        /// Any specific bedding configuration for this room stay if specified by the supplier.
        /// </summary>
        public string BeddingConfig { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime? ProcessDateTime { get; set; }
        public string ProcessBatchId { get; set; }
        public int? ProcessBatchNo { get; set; }

        public string Accommodation_SupplierRoomType_Id { get; set; }
        public string Accommodation_Id { get; set; }
        public string Accommodation_RoomInfo_Id { get; set; }
        public string Status { get; set; }
        public int? SystemRoomTypeMapId { get; set; }
        public float? MatchingScore { get; set; }
        public int? SystemProductCode { get; set; }
        public string SystemRoomTypeCode { get; set; }
        public string TLGXRoomTypeCode { get; set; }

        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string RoomDescription { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProvider { get; set; }
    }
}
