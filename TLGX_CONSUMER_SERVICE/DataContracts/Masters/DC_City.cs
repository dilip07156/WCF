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
    public class DC_City
    {
        System.Guid _City_Id;
        string _Name;
        string _Code;
        string _CountryName;
        System.Guid _Country_Id;
        string _CountryCode;
        Nullable<System.DateTime> _Create_Date;
        string _Create_User;
        Nullable<System.DateTime> _Edit_Date;
        string _Edit_User;
        string _Status;
        Nullable<System.Guid> _State_Id;
        string _StateName;
        string _StateCode;
        string _Google_PlaceId;
        int _TotalRecords;
        int? _TotalHotelRecords;
        int? _TotalAttractionsRecords;
        int? _TotalHolidaysRecords;
        int? _TotalSupplierCityRecords;


        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Rank { get; set; }

        [DataMember]
        public string Priority { get; set; }

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
        public string Google_PlaceId
        {
            get
            {
                return _Google_PlaceId;
            }

            set
            {
                _Google_PlaceId = value;
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
        public int? TotalHotelRecords
        {
            get
            {
                return _TotalHotelRecords;
            }

            set
            {
                _TotalHotelRecords = value;
            }
        }

        [DataMember]
        public int? TotalAttractionsRecords
        {
            get
            {
                return _TotalAttractionsRecords;
            }

            set
            {
                _TotalAttractionsRecords = value;
            }
        }

        [DataMember]
        public int? TotalHolidaysRecords
        {
            get
            {
                return _TotalHolidaysRecords;
            }

            set
            {
                _TotalHolidaysRecords = value;
            }
        }

        [DataMember]
        public int? TotalSupplierCityRecords
        {
            get
            {
                return _TotalSupplierCityRecords;
            }

            set
            {
                _TotalSupplierCityRecords = value;
            }
        }
    }

    [DataContract]
    public class DC_City_Search_RQ
    {
        Guid? _Country_Id;
        string _Country_Name;
        Guid? _State_Id;
        string _State_Name;
        Guid? _City_Id;
        string _City_Name;
        int? _PageNo;
        int? _PageSize;
        string _AlphaPageIndex;
        string _status;



        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Rank { get; set; }

        [DataMember]
        public string Priority { get; set; }

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
        public string AlphaPageIndex
        {
            get
            {
                return _AlphaPageIndex;
            }

            set
            {
                _AlphaPageIndex = value;
            }
        }
        [DataMember]
        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
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

    [DataContract]
    public class City_AlphaPage
    {
        string _alpha;

        [DataMember]
        public string Alpha
        {
            get
            {
                return _alpha;
            }

            set
            {
                _alpha = value;
            }
        }
    }

    [DataContract]
    public class DC_City_Master_DDL
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid City_Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Time { get; set; }
    }

    //GAURAV_TMAP_876
    [DataContract]
    public class DC_Priorities
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
     
    }

    [DataContract]
    public class DC_Keys
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }

    }

    [DataContract]
    public class DC_Ranks
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }

    }


    [DataContract]
    public class DC_CityAreaLocation
    {

        Guid _CityAreaLocation_Id;
        Guid _City_Id;
        string _Name;
        string _Code;
        DateTime? _Create_Date;
        string _Create_User;
        DateTime? _Edit_Date;
        string _Edit_User;
        Guid _CityArea_Id;
        string _option;
        [DataMember]
        public Guid CityAreaLocation_Id
        {
            get
            {
                return _CityAreaLocation_Id;
            }

            set
            {
                _CityAreaLocation_Id = value;
            }
        }
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
        public Guid CityArea_Id
        {
            get
            {
                return _CityArea_Id;
            }

            set
            {
                _CityArea_Id = value;
            }
        }
        [DataMember]
        public string Option
        {
            get
            {
                return _option;
            }

            set
            {
                _option = value;
            }
        }
    }

    [DataContract]
    public class DC_CityArea
    {
        Guid _CityArea_Id;
        Guid? _City_Id;
        string _Name;
        string _Code;
        DateTime? _Create_Date;
        string _Create_User;
        DateTime? _Edit_Date;
        string _Edit_User;
        string _option;

        [DataMember]
        public Guid CityArea_Id
        {
            get
            {
                return _CityArea_Id;
            }

            set
            {
                _CityArea_Id = value;
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
        public string Option
        {
            get
            {
                return _option;
            }

            set
            {
                _option = value;
            }
        }
    }

    [DataContract]
    public class DC_CitywithMultipleCountry_Search_RQ
    {
        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public List<Guid> CountryIdList { get; set; }
    }
}
