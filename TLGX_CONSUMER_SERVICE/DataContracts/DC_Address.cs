using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContracts.DC_Address
{
    [DataContract]
    public class DC_Address_Physical
    {
        Guid? _Product_Id;
        string _Street;
        string _City_AreaOrDistrict;
        string _CityOrTownOrVillage;
        string _CountyOrState;
        string _PostalCode;
        string _Country;

        [DataMember]
        public string Street
        {
            get
            {
                return _Street;
            }

            set
            {
                _Street = value;
            }
        }

        [DataMember]
        public string CityAreaOrDistrict
        {
            get
            {
                return _City_AreaOrDistrict;
            }

            set
            {
                _City_AreaOrDistrict = value;
            }
        }

        [DataMember]
        public string CityOrTownOrVillage
        {
            get
            {
                return _CityOrTownOrVillage;
            }

            set
            {
                _CityOrTownOrVillage = value;
            }
        }

        [DataMember]
        public string CountyOrState
        {
            get
            {
                return _CountyOrState;
            }

            set
            {
                _CountyOrState = value;
            }
        }

        [DataMember]
        public string PostalCode
        {
            get
            {
                return _PostalCode;
            }

            set
            {
                _PostalCode = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                _Country = value;
            }
        }

        [DataMember]
        public Guid? Product_Id
        {
            get
            {
                return _Product_Id;
            }

            set
            {
                _Product_Id = value;
            }
        }
    }

    [DataContract]
    public class DC_Address_GeoCode
    {
        Guid? _Product_Id;
        float _Latitude;
        float _Longitude;

        [DataMember]
        public float Latitude
        {
            get
            {
                return _Latitude;
            }

            set
            {
                _Latitude = value;
            }
        }

        [DataMember]
        public float Longitude
        {
            get
            {
                return _Longitude;
            }

            set
            {
                _Longitude = value;
            }
        }

        [DataMember]
        public Guid? Product_Id
        {
            get
            {
                return _Product_Id;
            }

            set
            {
                _Product_Id = value;
            }
        }
    }
    [DataContract]
    public class DC_Address_GeoCodeForNearBy
    {
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string radius { get; set; }
        [DataMember]
        public string PlaceType { get; set; }
    }

    [DataContract]
    public class DC_Country_State_City_Area_Location
    {
        string _Country;
        string _State;
        string _City;
        string _Area;
        string _Location;

        [DataMember]
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                _Country = value;
            }
        }

        [DataMember]
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                _State = value;
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                _City = value;
            }
        }

        [DataMember]
        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }

        [DataMember]
        public string Location
        {
            get
            {
                return _Location;
            }

            set
            {
                _Location = value;
            }
        }
    }
}
