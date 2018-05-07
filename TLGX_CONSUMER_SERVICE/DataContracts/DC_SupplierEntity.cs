using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SupplierEntity
    {

        [DataMember]
        public Guid? Supplier_Id { get; set; }

        [DataMember]
        public string Supplier_Name { get; set; }

        [DataMember]
        public string Element { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string LastUpdated { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public int? MongoPushCount { get; set; }

    }
   }

