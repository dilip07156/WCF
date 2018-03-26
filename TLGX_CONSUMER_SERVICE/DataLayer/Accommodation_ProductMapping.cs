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
    
    public partial class Accommodation_ProductMapping
    {
        public System.Guid Accommodation_ProductMapping_Id { get; set; }
        public Nullable<System.Guid> Accommodation_Id { get; set; }
        public Nullable<System.Guid> Supplier_Id { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierProductReference { get; set; }
        public string ProductName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string PostCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_User { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Street4 { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Nullable<int> MapId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Legacy_Htl_ID { get; set; }
        public string address { get; set; }
        public string address_tx { get; set; }
        public string TelephoneNumber_tx { get; set; }
        public string StarRating { get; set; }
        public string Google_Place_Id { get; set; }
        public Nullable<System.Guid> Country_Id { get; set; }
        public Nullable<System.Guid> City_Id { get; set; }
        public Nullable<System.Guid> stg_AccoMapping_Id { get; set; }
        public string TLGXProductCode { get; set; }
        public Nullable<bool> IsPentaho { get; set; }
        public Nullable<int> MatchedBy { get; set; }
        public string HotelName_Tx { get; set; }
        public string Latitude_Tx { get; set; }
        public string Longitude_Tx { get; set; }
        public string MatchedByString { get; set; }
        public Nullable<System.Guid> SupplierImportFile_Id { get; set; }
        public Nullable<int> Batch { get; set; }
        public Nullable<System.Guid> ReRun_SupplierImportFile_Id { get; set; }
        public Nullable<int> ReRun_Batch { get; set; }
        public string ProductType { get; set; }
    }
}
