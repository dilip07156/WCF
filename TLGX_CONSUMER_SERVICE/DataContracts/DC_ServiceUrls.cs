using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataContracts.ServiceUrls
{
    [DataContract]
    public static class Accommodation
    {
        [DataMember]
        public static string Search = "Accomodation/Search";

        [DataMember]
        public static string GetInfo = "GetAccomodation/Info/{Accomodation_Id}";

        [DataMember]
        public static string AddInfo = "AddAccomodation/Info";

        [DataMember]
        public static string UpdateInfo = "UpdateAccomodation/Info";

    }

    [DataContract]
    public static class Masters
    {
        [DataMember]
        public static string GetCountry = "GetAllCountries";

        [DataMember]
        public static string GetCity = "GetAllCities";

        [DataMember]
        public static string GetCityByCountryId = "GetCitiesByCountry/{Country_Id}";

        [DataMember]
        public static string GetDynamicAttributes = "DynamicAttributes/Get";

        [DataMember]
        public static string UpdateDynamicAttributes = "DynamicAttributes/Update";

        [DataMember]
        public static string AddDynamicAttributes = "DynamicAttributes/Add";

    }

}
