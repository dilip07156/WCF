using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accommodation_SupplierRoomTypeMap_SearchRS
    {
        [DataMember]
        public Guid ReRunSupplierImporrtFile_Id { get; set; }

        [DataMember]
        public int ReRunBatch { get; set; }

        [DataMember]
        public Guid SupplierImporrtFile_Id { get; set; }

        [DataMember]
        public int Batch { get; set; }

        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMapping_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Accommodation_Id { get; set; }

        [DataMember]
        public string CommonProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public int NumberOfRooms { get; set; }

        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string SupplierRoomId { get; set; }

        [DataMember]
        public string SupplierRoomTypeCode { get; set; }

        [DataMember]
        public string SupplierRoomName { get; set; }

        [DataMember]
        public string TX_RoomName { get; set; }

        [DataMember]
        public string SupplierRoomCategory { get; set; }

        [DataMember]
        public string SupplierRoomCategoryId { get; set; }

        [DataMember]
        public Nullable<int> MaxAdults { get; set; }

        [DataMember]
        public Nullable<int> MaxChild { get; set; }

        [DataMember]
        public Nullable<int> MaxInfants { get; set; }

        [DataMember]
        public Nullable<int> MaxGuestOccupancy { get; set; }

        [DataMember]
        public Nullable<int> Quantity { get; set; }

        [DataMember]
        public string RatePlan { get; set; }

        [DataMember]
        public string RatePlanCode { get; set; }

        [DataMember]
        public string SupplierProductName { get; set; }

        [DataMember]
        public string SupplierProductId { get; set; }

        [DataMember]
        public string Tx_StrippedName { get; set; }

        [DataMember]
        public string Tx_ReorderedName { get; set; }

        [DataMember]
        public string MappingStatus { get; set; }

        [DataMember]
        public Nullable<int> MapId { get; set; }

        [DataMember]
        public Nullable<System.Guid> Accommodation_RoomInfo_Id { get; set; }

        [DataMember]
        public string Accommodation_RoomInfo_Name { get; set; }

        [DataMember]
        public string Accommodation_RoomInfo_Category { get; set; }

        [DataMember]
        public List<DC_SupplierRoomTypeAttributes> RoomTypeAttributes { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string OldSupplierRoomName { get; set; }

        [DataMember]
        public Nullable<System.Guid> stg_SupplierHotelRoomMapping_Id { get; set; }

        [DataMember]
        public string RoomDescription { get; set; }

        [DataMember]
        public Nullable<System.Guid> Oldstg_SupplierHotelRoomMapping_Id { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public string RoomSize { get; set; }

        [DataMember]
        public string BathRoomType { get; set; }

        [DataMember]
        public string RoomViewCode { get; set; }

        [DataMember]
        public string FloorName { get; set; }

        [DataMember]
        public int? FloorNumber { get; set; }

        [DataMember]
        public string SupplierProvider { get; set; }

        [DataMember]
        public string Amenities { get; set; }

        [DataMember]
        public string RoomLocationCode { get; set; }

        [DataMember]
        public int? ChildAge { get; set; }

        [DataMember]
        public string ExtraBed { get; set; }

        [DataMember]
        public string Bedrooms { get; set; }

        [DataMember]
        public string Smoking { get; set; }

        [DataMember]
        public string BedTypeCode { get; set; }

        [DataMember]
        public int? MinGuestOccupancy { get; set; }

        [DataMember]
        public string PromotionalVendorCode { get; set; }

        [DataMember]
        public string BeddingConfig { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string Score { get; set; }

        [DataMember]
        public string Edit_User { get; set; }

        [DataMember]
        public DateTime? Edit_Date { get; set; }
    }

    [DataContract]
    public class DC_SupplierRoomTypeAttributes
    {
        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMapAttribute_Id { get; set; }
        [DataMember]
        public System.Guid Accommodation_SupplierRoomTypeMap_Id { get; set; }
        [DataMember]
        public string SupplierRoomTypeAttribute { get; set; }
        [DataMember]
        public string SystemAttributeKeyword { get; set; }
        [DataMember]
        public System.Guid SystemAttributeKeyword_Id { get; set; }
        [DataMember]
        public string IconClass { get; set; }
    }

    [DataContract]
    public class DC_SupplierRoomInfo_ForSuggestion
    {
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public Guid Accommodation_RoomInfo_Id { get; set; }
        [DataMember]
        public string Accommodation_RoomInfo_Name { get; set; }
    }


}
