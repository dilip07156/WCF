using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Microsoft.SqlServer;

namespace DataContracts.Masters
{

    [DataContract]
    public class DC_ZoneRQ
    {
        [DataMember]
        public Guid? Country_id { get; set; }
        [DataMember]
        public Guid? ZoneCityMapping_Id { get; set; }

        [DataMember]
        public Guid? ZoneProductMapping_Id { get; set; }
        [DataMember]
        public Guid? City_id { get; set; }
        [DataMember]
        public Guid? Zone_id { get; set; }
        [DataMember]
        public string Zone_Type { get; set; }
        [DataMember]
        public string Zone_Name { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string GooglePlaceId { get; set; }
        [DataMember]
        public double? Zone_Radius { get; set; }
        [DataMember]
        public bool? Included { get; set; }
        [DataMember]
        public string Zone_SubType { get; set; }

    }
    [DataContract]
    public class DC_ZoneSearch
    {
        [DataMember]
        public Guid Zone_id { get; set; }
        [DataMember]
        public Guid? City_id { get; set; }
        [DataMember]
        public Guid? Country_id { get; set; }
        [DataMember]
        public string Zone_Type { get; set; }
        [DataMember]
        public string Zone_Name { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? NoOfHotels { get; set; }
        [DataMember]
        public double? Zone_Radius { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }
        [DataMember]
        public string Zone_SubType { get; set; }

    }
    [DataContract]
    public class DC_ZoneCitiesSearch
    {
        [DataMember]
        public Guid? Zone_id { get; set; }
        [DataMember]
        public Guid? City_id { get; set; }
        [DataMember]
        public Guid ZoneCityMapping_Id { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string Zone_Name { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int? TotalRecords { get; set; }

    }

    [DataContract]
    public class DC_ZoneHotelList
    {
        [DataMember]
        public double? Distance { get; set; }
        [DataMember]
        public Guid Accommodation_Id { get; set; }
        [DataMember]
        public Guid ZoneProductMapping_Id { get; set; }        
        [DataMember]
        public string HotelName { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string StarRating { get; set; }
        [DataMember]
        public string ProductCategorySubType { get; set; }
        [DataMember]
        public bool? Included { get; set; }
    }
}
