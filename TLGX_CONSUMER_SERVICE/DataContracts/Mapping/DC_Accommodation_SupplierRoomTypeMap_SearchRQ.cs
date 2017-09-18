using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Accommodation_SupplierRoomTypeMap_SearchRQ
    {
        [DataMember]
        public Guid? Supplier_Id { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public Guid? Country { get; set; }

        [DataMember]
        public Guid? City { get; set; }

        [DataMember]
        public string MappingType { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string SupplierRoomName { get; set; }

        [DataMember]
        public string CalledFromTLGX { get; set; }

    }
}
