using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_EzeegoHotelVsSupplierHotelMappingReport
    {
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public int? Priority { get; set; }

        [DataMember]
        public int? Legacy_HTL_ID { get; set; }

        [DataMember]
        public string TLGXAccoId { get; set; }

        [DataMember]
        public string HotelName { get; set; }

        [DataMember]
        public string Status { get; set; }
    }

    [DataContract]
    public class DC_EzeegoHotelVsSupplierHotelMappingReport_RQ
    {
        [DataMember]
        public List<string> Region { get; set; }

        [DataMember]
        public List<string> Country { get; set; }

        [DataMember]
        public List<string> City { get; set; }

        [DataMember]
        public List<string> AccoPriority { get; set; }

        [DataMember]
        public List<string> Supplier { get; set; }

        [DataMember]
        public string selectedHotelId { get; set; }
    }
}
