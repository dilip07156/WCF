using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.STG
{
    [DataContract]
    public class List_DC_stg_SupplierProductMapping
    {
        [DataMember]
        public List<DC_stg_SupplierProductMapping> l_DC_stg_SupplierProductMapping;
    }

    [DataContract]
    public class DC_stg_SupplierProductMapping
    {
        [DataMember]
        public System.Guid stg_AccoMapping_Id { get; set; }

        [DataMember]
        public string SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string TelephoneNumber { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string ActiveFrom { get; set; }

        [DataMember]
        public string ActiveTo { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string ActionText { get; set; }

        [DataMember]
        public string UpdateType { get; set; }

        [DataMember]
        public Nullable<System.DateTime> InsertDate { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string StreetName { get; set; }

        [DataMember]
        public string Street2 { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string Street3 { get; set; }

        [DataMember]
        public string Street4 { get; set; }

        [DataMember]
        public string Street5 { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string StreetNo { get; set; }

        [DataMember]
        public string TX_COUNTRYNAME { get; set; }


        [DataMember]
        public string StarRating { get; set; }
    }
}
