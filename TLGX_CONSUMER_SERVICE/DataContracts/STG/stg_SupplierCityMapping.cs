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
    public class List_DC_stg_SupplierCityMapping
    {
        [DataMember]
        public List<DC_stg_SupplierCityMapping> l_DC_stg_SupplierCityMapping;
    }

    [DataContract]
    public class DC_stg_SupplierCityMapping
    {
        [DataMember]
        public System.Guid stg_City_Id { get; set; }

        [DataMember]
        public string SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string ActiveFrom { get; set; }

        [DataMember]
        public string ActiveTo { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string UpdateType { get; set; }

        [DataMember]
        public Nullable<System.DateTime> Insert_Date { get; set; }

        [DataMember]
        public string ActionText { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }
    }

    [DataContract]
    public class DC_stg_SupplierCityMapping_RQ
    {
        [DataMember]
        public Nullable<System.Guid> stg_City_Id { get; set; }

        [DataMember]
        public System.Guid Supplier_Id { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
