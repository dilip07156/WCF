using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace DataContracts.Mapping
{
    [DataContract]
    public class DC_Acitivity_SupplierProductMapping
    {
        [DataMember]
        public Guid ActivitySupplierProductMapping_Id { get; set; }
        [DataMember]
        public Guid? Activity_ID { get; set; }
        [DataMember]
        public Guid Supplier_ID { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SuplierProductCode { get; set; }
        [DataMember]
        public string SupplierProductType { get; set; }
        [DataMember]
        public string SupplierType { get; set; }
        [DataMember]
        public string SupplierLocationId { get; set; }
        [DataMember]
        public string SupplierLocationName { get; set; }
        [DataMember]
        public string SupplierCountryName { get; set; }
        [DataMember]
        public string SupplierCityName { get; set; }
        [DataMember]
        public string SupplierCountryCode { get; set; }
        [DataMember]
        public string SupplierCityCode { get; set; }
        [DataMember]
        public string SupplierStateName { get; set; }
        [DataMember]
        public string SupplierStateCode { get; set; }
        [DataMember]
        public string SupplierCityIATACode { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string SupplierDataLangaugeCode { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public string Conditions { get; set; }
        [DataMember]
        public string Inclusions { get; set; }
        [DataMember]
        public string Exclusions { get; set; }
        [DataMember]
        public string AdditionalInformation { get; set; }
        [DataMember]
        public string DeparturePoint { get; set; }
        [DataMember]
        public string TicketingDetails { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string DepartureDate { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public string BlockOutDateFrom { get; set; }
        [DataMember]
        public string BlockOutDateTo { get; set; }
        [DataMember]
        public string OptionTitle { get; set; }
        [DataMember]
        public string OptionCode { get; set; }
        [DataMember]
        public string OptionDescription { get; set; }
        [DataMember]
        public string TourActivityLangauageCode { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public string TourActivityLanguage { get; set; }
        [DataMember]
        public string ImgURL { get; set; }
        [DataMember]
        public string ProductValidFor { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string DayPattern { get; set; }
        [DataMember]
        public string Theme { get; set; }
        [DataMember]
        public string Distance { get; set; }
        [DataMember]
        public string SupplierTourType { get; set; }
        [DataMember]
        public int? MapID { get; set; }
        [DataMember]
        public string MappingStatus { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public string SystemCountryName { get; set; }
        [DataMember]
        public string SystemCityName { get; set; }
        [DataMember]
        public int? ourRef { get; set; }
        [DataMember]
        public string CKIS_Master { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        //[DataMember]
        //public Guid? SystemCountry_Id { get; set; }
        //[DataMember]
        //public Guid? SystemCity_Id { get; set; }
    }

    [DataContract]
    public class DC_Acitivity_SupplierProductMapping_Search_RQ
    {
        [DataMember]
        public Guid Activity_ID { get; set; }
        [DataMember]
        public Guid Supplier_ID { get; set; }
        [DataMember]
        public Guid ActivitySupplierProductMappling_Id { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SuplierProductCode { get; set; }
        [DataMember]
        public string SupplierProductType { get; set; }
        [DataMember]
        public string SupplierType { get; set; }
        [DataMember]
        public string SupplierLocationId { get; set; }
        [DataMember]
        public string SupplierLocationName { get; set; }
        [DataMember]
        public string SupplierCountryName { get; set; }
        [DataMember]
        public string SupplierCityName { get; set; }
        [DataMember]
        public string SupplierCountryCode { get; set; }
        [DataMember]
        public string SupplierCityCode { get; set; }
        [DataMember]
        public string SupplierStateName { get; set; }
        [DataMember]
        public string SupplierStateCode { get; set; }
        [DataMember]
        public string SupplierCityIATACode { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string SupplierProductName { get; set; }
        [DataMember]
        public string SupplierDataLangaugeCode { get; set; }
        [DataMember]
        public string Introduction { get; set; }
        [DataMember]
        public string Conditions { get; set; }
        [DataMember]
        public string Inclusions { get; set; }
        [DataMember]
        public string Exclusions { get; set; }
        [DataMember]
        public string AdditionalInformation { get; set; }
        [DataMember]
        public string DeparturePoint { get; set; }
        [DataMember]
        public string TicketingDetails { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string DepartureDate { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public string BlockOutDateFrom { get; set; }
        [DataMember]
        public string BlockOutDateTo { get; set; }
        [DataMember]
        public string OptionTitle { get; set; }
        [DataMember]
        public string OptionCode { get; set; }
        [DataMember]
        public string OptionDescription { get; set; }
        [DataMember]
        public string TourActivityLangauageCode { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public string TourActivityLanguage { get; set; }
        [DataMember]
        public string ImgURL { get; set; }
        [DataMember]
        public string ProductValidFor { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string DayPattern { get; set; }
        [DataMember]
        public string Theme { get; set; }
        [DataMember]
        public string Distance { get; set; }
        [DataMember]
        public string SupplierTourType { get; set; }
        [DataMember]
        public string MappingStatus { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }
        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int? PageSize { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string SystemCountryName { get; set; }
        [DataMember]
        public string SystemCityName { get; set; }
        [DataMember]
        public string StatusExcept { get; set; }
        [DataMember]
        public string KeyWord { get; set; }
        [DataMember]
        public string SearchFor { get; set; }

    }
    [DataContract]
    public class DC_Acitivity_SupplierProductMappingForDDL
    {
        [DataMember]
        public Guid Supplier_ID { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
    }

    
}
