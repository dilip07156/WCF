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
    
    public partial class Activity_Types
    {
        public System.Guid Activity_Type_Id { get; set; }
        public System.Guid Activity_Id { get; set; }
        public string ActivityType { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Create_User { get; set; }
        public string Edit_User { get; set; }
    }
}
