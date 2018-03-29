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
    public class List_DC_stg_SupplierCountryMapping
    {
        [DataMember]
        public List<DC_stg_SupplierCountryMapping> l_DC_stg_SupplierCountryMapping;
    }

    [DataContract]
    public class DC_stg_SupplierCountryMapping
    {
        [DataMember]
        public System.Guid stg_Country_Id { get; set; }

        [DataMember]
        public string SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string ActiveFrom { get; set; }

        [DataMember]
        public string ActiveTo { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string UpdateType { get; set; }

        [DataMember]
        public Nullable<System.DateTime> InsertDate { get; set; }

        [DataMember]
        public string ActionText { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string ContinentCode { get; set; }

        [DataMember]
        public string ContinentName { get; set; }
        [DataMember]
        public Guid? SupplierImportFile_Id { get; set; }

    }

    [DataContract]
    public class DC_stg_SupplierCountryMapping_RQ
    {
        [DataMember]
        public Nullable<System.Guid> stg_Country_Id { get; set; }

        [DataMember]
        public System.Guid Supplier_Id { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public int PageNo { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }
    }
}
