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
    
    public partial class SupplierImportFileDetail
    {
        public System.Guid SupplierImportFile_Id { get; set; }
        public System.Guid Supplier_Id { get; set; }
        public string Entity { get; set; }
        public string OriginalFilePath { get; set; }
        public string SavedFilePath { get; set; }
        public string ArchiveFilePath { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public Nullable<System.DateTime> PROCESS_DATE { get; set; }
        public string PROCESS_USER { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Mode { get; set; }
    }
}
