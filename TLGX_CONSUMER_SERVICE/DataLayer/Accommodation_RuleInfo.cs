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
    
    public partial class Accommodation_RuleInfo
    {
        public System.Guid Accommodation_RuleInfo_Id { get; set; }
        public Nullable<System.Guid> Accommodation_Id { get; set; }
        public Nullable<int> Legacy_Htl_Id { get; set; }
        public string RuleType { get; set; }
        public string Description { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Edit_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsInternal { get; set; }
    }
}
