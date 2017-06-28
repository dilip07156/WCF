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
    public class DC_Country
    {
        System.Guid _Country_Id;
        string _Name;
        string _Code;
        string _Status;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        string _ISOofficial_name_en;
        string _ISO3166_1_Alpha_2;
        string _ISO3166_1_Alpha_3;
        string _ISO3166_1_M49;
        string _ISO3166_1_ITU;
        string _MARC;
        string _WMO;
        string _DS;
        string _Dial;
        string _ISO4217_currency_alphabetic_code;
        string _ISO4217_currency_country_name;
        string _ISO4217_currency_minor_unit;
        string _ISO4217_currency_name;
        string _ISO4217_currency_numeric_code;
        string _ISO3166_1_Capital;
        string _ISO3166_1_Continent;
        string _ISO3166_1_TLD;
        string _ISO3166_1_Languages;
        string _ISO3166_1_Geoname_ID;
        string _ISO3166_1_EDGAR;
        string _GooglePlaceID;
        string _action;
        int _TotalRecords;

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

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
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                _Code = value;
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return _Status;
            }

            set
            {
                _Status = value;
            }
        }

        [DataMember]
        public DateTime? Create_Date
        {
            get
            {
                return _Create_Date;
            }

            set
            {
                _Create_Date = value;
            }
        }

        [DataMember]
        public string Create_User
        {
            get
            {
                return _Create_User;
            }

            set
            {
                _Create_User = value;
            }
        }

        [DataMember]
        public DateTime? Edit_Date
        {
            get
            {
                return _Edit_Date;
            }

            set
            {
                _Edit_Date = value;
            }
        }

        [DataMember]
        public string Edit_User
        {
            get
            {
                return _Edit_User;
            }

            set
            {
                _Edit_User = value;
            }
        }

        [DataMember]
        public string ISOofficial_name_en
        {
            get
            {
                return _ISOofficial_name_en;
            }

            set
            {
                _ISOofficial_name_en = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Alpha_2
        {
            get
            {
                return _ISO3166_1_Alpha_2;
            }

            set
            {
                _ISO3166_1_Alpha_2 = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Alpha_3
        {
            get
            {
                return _ISO3166_1_Alpha_3;
            }

            set
            {
                _ISO3166_1_Alpha_3 = value;
            }
        }

        [DataMember]
        public string ISO3166_1_M49
        {
            get
            {
                return _ISO3166_1_M49;
            }

            set
            {
                _ISO3166_1_M49 = value;
            }
        }

        [DataMember]
        public string ISO3166_1_ITU
        {
            get
            {
                return _ISO3166_1_ITU;
            }

            set
            {
                _ISO3166_1_ITU = value;
            }
        }

        [DataMember]
        public string MARC
        {
            get
            {
                return _MARC;
            }

            set
            {
                _MARC = value;
            }
        }

        [DataMember]
        public string WMO
        {
            get
            {
                return _WMO;
            }

            set
            {
                _WMO = value;
            }
        }

        [DataMember]
        public string DS
        {
            get
            {
                return _DS;
            }

            set
            {
                _DS = value;
            }
        }

        [DataMember]
        public string Dial
        {
            get
            {
                return _Dial;
            }

            set
            {
                _Dial = value;
            }
        }

        [DataMember]
        public string ISO4217_currency_alphabetic_code
        {
            get
            {
                return _ISO4217_currency_alphabetic_code;
            }

            set
            {
                _ISO4217_currency_alphabetic_code = value;
            }
        }

        [DataMember]
        public string ISO4217_currency_country_name
        {
            get
            {
                return _ISO4217_currency_country_name;
            }

            set
            {
                _ISO4217_currency_country_name = value;
            }
        }

        [DataMember]
        public string ISO4217_currency_minor_unit
        {
            get
            {
                return _ISO4217_currency_minor_unit;
            }

            set
            {
                _ISO4217_currency_minor_unit = value;
            }
        }

        [DataMember]
        public string ISO4217_currency_name
        {
            get
            {
                return _ISO4217_currency_name;
            }

            set
            {
                _ISO4217_currency_name = value;
            }
        }

        [DataMember]
        public string ISO4217_currency_numeric_code
        {
            get
            {
                return _ISO4217_currency_numeric_code;
            }

            set
            {
                _ISO4217_currency_numeric_code = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Capital
        {
            get
            {
                return _ISO3166_1_Capital;
            }

            set
            {
                _ISO3166_1_Capital = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Continent
        {
            get
            {
                return _ISO3166_1_Continent;
            }

            set
            {
                _ISO3166_1_Continent = value;
            }
        }

        [DataMember]
        public string ISO3166_1_TLD
        {
            get
            {
                return _ISO3166_1_TLD;
            }

            set
            {
                _ISO3166_1_TLD = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Languages
        {
            get
            {
                return _ISO3166_1_Languages;
            }

            set
            {
                _ISO3166_1_Languages = value;
            }
        }

        [DataMember]
        public string ISO3166_1_Geoname_ID
        {
            get
            {
                return _ISO3166_1_Geoname_ID;
            }

            set
            {
                _ISO3166_1_Geoname_ID = value;
            }
        }

        [DataMember]
        public string ISO3166_1_EDGAR
        {
            get
            {
                return _ISO3166_1_EDGAR;
            }

            set
            {
                _ISO3166_1_EDGAR = value;
            }
        }

        [DataMember]
        public string GooglePlaceID
        {
            get
            {
                return _GooglePlaceID;
            }

            set
            {
                _GooglePlaceID = value;
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
        public string Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
            }
        }
    }

    [DataContract]
    public class DC_Country_Search_RQ
    {
        Guid? _Country_Id;
        string _Country_Name;
        string _Country_Code;
        int? _PageNo;
        int? _PageSize;


        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }
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

        [DataMember]
        public int? PageNo
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
        public int? PageSize
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
    }
}
