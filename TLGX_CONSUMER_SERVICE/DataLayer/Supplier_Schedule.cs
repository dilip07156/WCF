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
    
    public partial class Supplier_Schedule
    {
        public System.Guid SupplierScheduleID { get; set; }
        public System.Guid Supplier_ID { get; set; }
        public bool ISXMLSupplier { get; set; }
        public Nullable<bool> ISUpdateFrequence { get; set; }
        public string FrequencyTypeCode { get; set; }
        public Nullable<int> Recur_No { get; set; }
        public Nullable<int> MonthOfYear { get; set; }
        public string DayOfWeek { get; set; }
        public Nullable<int> DateOfMonth { get; set; }
        public Nullable<int> WeekOfMonth { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_User { get; set; }
    }
}
