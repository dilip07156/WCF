//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Accommodation_SupplierRoomTypeMapping
    {
        public System.Guid Accommodation_SupplierRoomTypeMapping_Id { get; set; }
        public Nullable<System.Guid> Accommodation_Id { get; set; }
        public Nullable<System.Guid> Supplier_Id { get; set; }
        public string SupplierName { get; set; }
        public string SupplierRoomId { get; set; }
        public string SupplierRoomTypeCode { get; set; }
        public string SupplierRoomName { get; set; }
        public string TX_RoomName { get; set; }
        public string SupplierRoomCategory { get; set; }
        public string SupplierRoomCategoryId { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_User { get; set; }
        public Nullable<int> MaxAdults { get; set; }
        public Nullable<int> MaxChild { get; set; }
        public Nullable<int> MaxInfants { get; set; }
        public Nullable<int> MaxGuestOccupancy { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string RatePlan { get; set; }
        public string RatePlanCode { get; set; }
        public string SupplierProductName { get; set; }
        public string SupplierProductId { get; set; }
        public string Tx_StrippedName { get; set; }
        public string Tx_ReorderedName { get; set; }
        public string MappingStatus { get; set; }
        public Nullable<int> MapId { get; set; }
        public Nullable<System.Guid> Accommodation_RoomInfo_Id { get; set; }
        public Nullable<System.Guid> stg_SupplierHotelRoomMapping_Id { get; set; }
        public string RoomDescription { get; set; }
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }
        public Nullable<int> Batch { get; set; }
        public Nullable<System.Guid> ReRun_SupplierImportFile_Id { get; set; }
        public Nullable<int> ReRun_Batch { get; set; }
        public string RoomSize { get; set; }
        public string BathRoomType { get; set; }
        public string RoomViewCode { get; set; }
        public string FloorName { get; set; }
        public Nullable<int> FloorNumber { get; set; }
        public string SupplierProvider { get; set; }
        public string Amenities { get; set; }
        public string RoomLocationCode { get; set; }
        public Nullable<int> ChildAge { get; set; }
        public string ExtraBed { get; set; }
        public string Bedrooms { get; set; }
        public string Smoking { get; set; }
        public string BedTypeCode { get; set; }
        public Nullable<int> MinGuestOccupancy { get; set; }
        public string PromotionalVendorCode { get; set; }
        public string BeddingConfig { get; set; }
    }
}
