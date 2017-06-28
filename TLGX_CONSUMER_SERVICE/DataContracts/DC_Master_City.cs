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
    public class DC_Master_City
    {
        Guid _City_Id;
        string _City_Name;
        string _City_Code;
        Guid _Country_Id;
        string _Country_Name;
        string _State_Code;
        string _State_Name;

        [DataMember]
        public Guid City_Id
        {
            get
            {
                return _City_Id;
            }

            set
            {
                _City_Id = value;
            }
        }

        [DataMember]
        public string City_Name
        {
            get
            {
                return _City_Name;
            }

            set
            {
                _City_Name = value;
            }
        }

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
        public string City_Code
        {
            get
            {
                return _City_Code;
            }

            set
            {
                _City_Code = value;
            }
        }

        [DataMember]
        public string State_Code
        {
            get
            {
                return _State_Code;
            }

            set
            {
                _State_Code = value;
            }
        }

        [DataMember]
        public string State_Name
        {
            get
            {
                return _State_Name;
            }

            set
            {
                _State_Name = value;
            }
        }
    }


}
