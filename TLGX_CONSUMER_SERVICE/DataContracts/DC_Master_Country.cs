using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts
{
    [DataContract]
    public class DC_Master_Country
    {
        Guid _Country_Id;
        string _Country_Name;
        string _Country_Code;

        [DataMember]
        public Guid Country_Id
        {
            get
            {
                return _Country_Id;
            }

            set
            {
                _Country_Id = value;
            }
        }

        [DataMember]
        public string Country_Name
        {
            get
            {
                return _Country_Name;
            }

            set
            {
                _Country_Name = value;
            }
        }

        [DataMember]
        public string Country_Code
        {
            get
            {
                return _Country_Code;
            }

            set
            {
                _Country_Code = value;
            }
        }
    }

    public class DC_CountryMaster
    {
        public Guid Country_ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string NameWithCode { get; set; }
        public string ISO3166_1_Alpha_2 { get; set; }
        public string ISO3166_1_Alpha_3 { get; set; }
    }
}
