using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataContracts.Pentaho
{
    [DataContract]
    [XmlRoot("webresult")]
    public sealed class DC_PentahoApiCallResult
    {
        [DataMember]
        [XmlElement("result")]
        public string result { get; set; }

        [DataMember]
        [XmlElement("message")]
        public string message { get; set; }

        [DataMember]
        [XmlElement("id")]
        public string id { get; set; }
    }
}
