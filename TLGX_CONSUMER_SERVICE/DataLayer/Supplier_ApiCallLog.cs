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
    
    public partial class Supplier_ApiCallLog
    {
        public System.Guid SupplierApiCallLog_Id { get; set; }
        public Nullable<System.Guid> SupplierApiLocation_Id { get; set; }
        public Nullable<System.Guid> PentahoCall_Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string CalledBy { get; set; }
        public Nullable<System.DateTime> CalledDate { get; set; }
    }
}
