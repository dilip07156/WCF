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
    
    public partial class stg_SupplierActivityImageMapping
    {
        public System.Guid stg_SupplierActivityImageMapping_Id { get; set; }
        public System.Guid stg_SupplierActivityMapping_Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SuplierProductCode { get; set; }
        public string SupplierProductName { get; set; }
        public string MediaId { get; set; }
        public string FullURL { get; set; }
        public string ThumbnailURL { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string MediaDescription { get; set; }
        public string MediaDimension { get; set; }
        public string MediaCaption { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Create_User { get; set; }
        public string Edit_User { get; set; }
    }
}
