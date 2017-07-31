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
    

}
