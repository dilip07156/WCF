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
    
    public partial class Activity_Descriptions
    {
        public System.Guid Activity_Description_Id { get; set; }
        public Nullable<System.Guid> Activity_Id { get; set; }
        public Nullable<int> Legacy_Product_ID { get; set; }
        public string Description { get; set; }
        public string Language_Code { get; set; }
        public string DescriptionFor { get; set; }
        public string DescriptionType { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Source { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Edit_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public Nullable<System.Guid> Activity_Flavour_Id { get; set; }
    }
}
