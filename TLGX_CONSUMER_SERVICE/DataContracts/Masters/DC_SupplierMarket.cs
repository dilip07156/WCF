using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_SupplierMarket
    {
        [DataMember]
        public Guid? Supplier_Market_Id { get; set; }
        [DataMember]
        public Guid? Supplier_Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }



    }
}
