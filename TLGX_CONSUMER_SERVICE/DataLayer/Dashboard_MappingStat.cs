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
    
    public partial class Dashboard_MappingStat
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> supplier_id { get; set; }
        public string SupplierName { get; set; }
        public string Status { get; set; }
        public string MappingFor { get; set; }
        public Nullable<int> totalcount { get; set; }
        public Nullable<int> SuppliersCount { get; set; }
        public Nullable<int> Batch { get; set; }
    }
}