using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Masters
{
    [DataContract]
    public class DC_PortMaster
    {
        System.Guid _Port_Id;
        string _OAG_loc;
        string _OAG_multicity;
        string _OAG_type;
        string _OAG_subtype;
        string _oag_name;
        string _oag_portname;
        string _oag_ctry;
        string _oag_subctry;
        string _oag_ctryname;
        string _oag_state;
        string _oag_substate;
        string _oag_timediv;
        string _oag_lat;
        string _oag_lon;
        string _oag_inactive;
        Nullable<System.Guid> _Country_Id;
        string _CountryName;
        string _CountryCode;
        Nullable<System.Guid> _State_Id;
        string _StateName;
        string _StateCode;
        Nullable<System.Guid> _City_Id;
        string _CityName;
        string _CityCode;
        Nullable<System.Guid> _MultiCity_Id;
        string _MultiCityName;
        string _MultiCityCode;
        string _MappingStatus;
        int _TotalRecords;

        [DataMember]
        public Guid Port_Id
        {
            get
            {
                return _Port_Id;
            }

            set
            {
                _Port_Id = value;
            }
        }

        [DataMember]
        public string OAG_loc
        {
            get
            {
                return _OAG_loc;
            }

            set
            {
                _OAG_loc = value;
            }
        }

        [DataMember]
        public string OAG_multicity
        {
            get
            {
                return _OAG_multicity;
            }

            set
            {
                _OAG_multicity = value;
            }
        }

        [DataMember]
        public string OAG_type
        {
            get
            {
                return _OAG_type;
            }

            set
            {
                _OAG_type = value;
            }
        }

        [DataMember]
        public string OAG_subtype
        {
            get
            {
                return _OAG_subtype;
            }

            set
            {
                _OAG_subtype = value;
            }
        }

        [DataMember]
        public string Oag_name
        {
            get
            {
                return _oag_name;
            }

            set
            {
                _oag_name = value;
            }
        }

        [DataMember]
        public string Oag_portname
        {
            get
            {
                return _oag_portname;
            }

            set
            {
                _oag_portname = value;
            }
        }

        [DataMember]
        public string Oag_ctry
        {
            get
            {
                return _oag_ctry;
            }

            set
            {
                _oag_ctry = value;
            }
        }

        [DataMember]
        public string Oag_subctry
        {
            get
            {
                return _oag_subctry;
            }

            set
            {
                _oag_subctry = value;
            }
        }

        [DataMember]
        public string Oag_ctryname
        {
            get
            {
                return _oag_ctryname;
            }

            set
            {
                _oag_ctryname = value;
            }
        }

        [DataMember]
        public string Oag_state
        {
            get
            {
                return _oag_state;
            }

            set
            {
                _oag_state = value;
            }
        }

        [DataMember]
        public string Oag_substate
        {
            get
            {
                return _oag_substate;
            }

            set
            {
                _oag_substate = value;
            }
        }

        [DataMember]
        public string Oag_timediv
        {
            get
            {
                return _oag_timediv;
            }

            set
            {
                _oag_timediv = value;
            }
        }

        [DataMember]
        public string Oag_lat
        {
            get
            {
                return _oag_lat;
            }

            set
            {
                _oag_lat = value;
            }
        }

        [DataMember]
        public string Oag_lon
        {
            get
            {
                return _oag_lon;
            }

            set
            {
                _oag_lon = value;
            }
        }

        [DataMember]
        public string Oag_inactive
        {
            get
            {
                return _oag_inactive;
            }

            set
            {
                _oag_inactive = value;
            }
        }

        [DataMember]
        public Guid? Country_Id
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
        public string CountryName
        {
            get
            {
                return _CountryName;
            }

            set
            {
                _CountryName = value;
            }
        }

        [DataMember]
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                _CountryCode = value;
            }
        }

        [DataMember]
        public Guid? State_Id
        {
            get
            {
                return _State_Id;
            }

            set
            {
                _State_Id = value;
            }
        }

        [DataMember]
        public string CityName
        {
            get
            {
                return _CityName;
            }

            set
            {
                _CityName = value;
            }
        }

        [DataMember]
        public string CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                _CityCode = value;
            }
        }

        [DataMember]
        public Guid? City_Id
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
        public string StateName
        {
            get
            {
                return _StateName;
            }

            set
            {
                _StateName = value;
            }
        }

        [DataMember]
        public string StateCode
        {
            get
            {
                return _StateCode;
            }

            set
            {
                _StateCode = value;
            }
        }

        [DataMember]
        public string MappingStatus
        {
            get
            {
                return _MappingStatus;
            }

            set
            {
                _MappingStatus = value;
            }
        }

        [DataMember]
        public int TotalRecords
        {
            get
            {
                return _TotalRecords;
            }

            set
            {
                _TotalRecords = value;
            }
        }

        [DataMember]
        public Guid? MultiCity_Id
        {
            get
            {
                return _MultiCity_Id;
            }

            set
            {
                _MultiCity_Id = value;
            }
        }

        [DataMember]
        public string MultiCityName
        {
            get
            {
                return _MultiCityName;
            }

            set
            {
                _MultiCityName = value;
            }
        }

        [DataMember]
        public string MultiCityCode
        {
            get
            {
                return _MultiCityCode;
            }

            set
            {
                _MultiCityCode = value;
            }
        }
    }

    [DataContract]
    public class DC_PortMaster_RQ
    {
        string _Country_Name;
        string _City_Name;
        string _Port_Country_Name;
        string _Port_City_Name;
        string _Mapping_Status;
        Guid? _Country_Id;
        Guid? _City_Id;
        string _oag_portname;
        Guid? _m_Port_Id;
        int _PageSize;
        int _PageNo;

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
        public string Port_Country_Name
        {
            get
            {
                return _Port_Country_Name;
            }

            set
            {
                _Port_Country_Name = value;
            }
        }

        [DataMember]
        public string Port_City_Name
        {
            get
            {
                return _Port_City_Name;
            }

            set
            {
                _Port_City_Name = value;
            }
        }

        [DataMember]
        public string Mapping_Status
        {
            get
            {
                return _Mapping_Status;
            }

            set
            {
                _Mapping_Status = value;
            }
        }

        [DataMember]
        public int PageSize
        {
            get
            {
                return _PageSize;
            }

            set
            {
                _PageSize = value;
            }
        }

        [DataMember]
        public int PageNo
        {
            get
            {
                return _PageNo;
            }

            set
            {
                _PageNo = value;
            }
        }
        [DataMember]
        public Guid? Country_Id
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
        public Guid? City_Id
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

        public string Oag_portname
        {
            get
            {
                return _oag_portname;
            }

            set
            {
                _oag_portname = value;
            }
        }
        [DataMember]

        public Guid? Port_Id
        {
            get
            {
                return _m_Port_Id;
            }

            set
            {
                _m_Port_Id = value;
            }
        }
    }
}
