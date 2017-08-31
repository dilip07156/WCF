using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Pentaho
{
    [DataContract]
    public class DC_PentahoApiCallLogDetails
    {
        [DataMember]
        public System.Guid SupplierApiCallLog_Id { get; set; }
        [DataMember]
        public Nullable<System.Guid> SupplierApiLocation_Id { get; set; }
        [DataMember]
        public string ApiPath { get; set; }
        [DataMember]
        public Nullable<System.Guid> Supplier_Id { get; set; }
        [DataMember]
        public Nullable<System.Guid> Entity_Id { get; set; }
        [DataMember]
        public string Supplier { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public Nullable<System.Guid> PentahoCall_Id { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string CalledBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CalledDate { get; set; }
    }
}
