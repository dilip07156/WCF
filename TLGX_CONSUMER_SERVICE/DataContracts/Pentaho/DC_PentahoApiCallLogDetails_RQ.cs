using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Pentaho
{
    [DataContract]
    public class DC_PentahoApiCallLogDetails_RQ
    {
        [DataMember]
        public Guid? Supplier_Id { get; set; }
        [DataMember]
        public Guid? Entity_Id { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
