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
    
    public partial class Activity_Flavour
    {
        public System.Guid Activity_Flavour_Id { get; set; }
        public Nullable<System.Guid> Activity_Id { get; set; }
        public string ProductName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCategorySubType { get; set; }
        public string ProductType { get; set; }
        public Nullable<int> Legacy_Product_ID { get; set; }
        public string CommonProductNameSubType_Id { get; set; }
        public string CompanyProductNameSubType_Id { get; set; }
        public string FinanceControlId { get; set; }
        public string ProductNameSubType { get; set; }
        public string PlaceOfEvent { get; set; }
        public string StartingPoint { get; set; }
        public string EndingPoint { get; set; }
        public string USP { get; set; }
        public Nullable<bool> IsPickUpDropDefined { get; set; }
    }
}
