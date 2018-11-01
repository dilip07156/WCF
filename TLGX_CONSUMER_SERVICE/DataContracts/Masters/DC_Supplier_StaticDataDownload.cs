using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_Supplier_StaticDataDownload
    {
        [DataMember]
        public Guid SupplierCredentialsId { get; set; }

        [DataMember]
        public Guid? SupplierId { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? TotalRecords { get; set; }

        [DataMember]
        public int? PageNo { get; set; }

        [DataMember]
        public int? PageSize { get; set; }

    }
}
