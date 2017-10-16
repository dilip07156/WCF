﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ConsumerEntities : DbContext
    {
        public ConsumerEntities()
            : base("name=ConsumerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accommodation_Contact> Accommodation_Contact { get; set; }
        public virtual DbSet<Accommodation_Facility> Accommodation_Facility { get; set; }
        public virtual DbSet<Accommodation_HealthAndSafety> Accommodation_HealthAndSafety { get; set; }
        public virtual DbSet<Accommodation_HotelUpdates> Accommodation_HotelUpdates { get; set; }
        public virtual DbSet<Accommodation_Media> Accommodation_Media { get; set; }
        public virtual DbSet<Accommodation_NearbyPlaces> Accommodation_NearbyPlaces { get; set; }
        public virtual DbSet<Accommodation_PaxOccupancy> Accommodation_PaxOccupancy { get; set; }
        public virtual DbSet<Accommodation_RoomInfo> Accommodation_RoomInfo { get; set; }
        public virtual DbSet<Accommodation_RouteInfo> Accommodation_RouteInfo { get; set; }
        public virtual DbSet<Accommodation_RuleInfo> Accommodation_RuleInfo { get; set; }
        public virtual DbSet<Accommodation_Status> Accommodation_Status { get; set; }
        public virtual DbSet<DynamicAttribute> DynamicAttributes { get; set; }
        public virtual DbSet<Accommodation_RoomFacility> Accommodation_RoomFacility { get; set; }
        public virtual DbSet<Accommodation_ClassificationAttributes> Accommodation_ClassificationAttributes { get; set; }
        public virtual DbSet<Accommodation_ProductMapping> Accommodation_ProductMapping { get; set; }
        public virtual DbSet<m_CityMapping> m_CityMapping { get; set; }
        public virtual DbSet<m_CountryMapping> m_CountryMapping { get; set; }
        public virtual DbSet<m_Activity_Master> m_Activity_Master { get; set; }
        public virtual DbSet<m_Approval_RoleMaster> m_Approval_RoleMaster { get; set; }
        public virtual DbSet<m_Approval_StatusFlow> m_Approval_StatusFlow { get; set; }
        public virtual DbSet<m_Approval_StatusMaster> m_Approval_StatusMaster { get; set; }
        public virtual DbSet<m_TeamMembers> m_TeamMembers { get; set; }
        public virtual DbSet<m_Teams> m_Teams { get; set; }
        public virtual DbSet<m_WorkflowMaster> m_WorkflowMaster { get; set; }
        public virtual DbSet<m_WorkFlowMessage> m_WorkFlowMessage { get; set; }
        public virtual DbSet<m_WorkflowSteps> m_WorkflowSteps { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Media_Attributes> Media_Attributes { get; set; }
        public virtual DbSet<m_CityArea> m_CityArea { get; set; }
        public virtual DbSet<m_CityAreaLocation> m_CityAreaLocation { get; set; }
        public virtual DbSet<m_CityMaster> m_CityMaster { get; set; }
        public virtual DbSet<m_CountryMaster> m_CountryMaster { get; set; }
        public virtual DbSet<m_States> m_States { get; set; }
        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<GoogleGeoCode> GoogleGeoCodes { get; set; }
        public virtual DbSet<m_EntityType> m_EntityType { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Organisation_Company> Organisation_Company { get; set; }
        public virtual DbSet<UserEntity> UserEntities { get; set; }
        public virtual DbSet<Counter> Counters { get; set; }
        public virtual DbSet<vwMappingStat> vwMappingStats { get; set; }
        public virtual DbSet<SiteMap> SiteMaps { get; set; }
        public virtual DbSet<m_PortMaster> m_PortMaster { get; set; }
        public virtual DbSet<m_masterattribute> m_masterattribute { get; set; }
        public virtual DbSet<m_masterattributevalue> m_masterattributevalue { get; set; }
        public virtual DbSet<m_Statuses> m_Statuses { get; set; }
        public virtual DbSet<m_FrequencyType> m_FrequencyType { get; set; }
        public virtual DbSet<Schedule_NextOccurance> Schedule_NextOccurance { get; set; }
        public virtual DbSet<Supplier_Schedule> Supplier_Schedule { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Supplier_Market> Supplier_Market { get; set; }
        public virtual DbSet<Supplier_ProductCategory> Supplier_ProductCategory { get; set; }
        public virtual DbSet<m_MasterAttributeMapping> m_MasterAttributeMapping { get; set; }
        public virtual DbSet<m_MasterAttributeValueMapping> m_MasterAttributeValueMapping { get; set; }
        public virtual DbSet<m_SupplierImportAttributes> m_SupplierImportAttributes { get; set; }
        public virtual DbSet<m_SupplierImportAttributeValues> m_SupplierImportAttributeValues { get; set; }
        public virtual DbSet<SupplierImportFile_ErrorLog> SupplierImportFile_ErrorLog { get; set; }
        public virtual DbSet<SupplierImportFileDetail> SupplierImportFileDetails { get; set; }
        public virtual DbSet<Activity_SupplierProductMapping> Activity_SupplierProductMapping { get; set; }
        public virtual DbSet<stg_SupplierCityMapping> stg_SupplierCityMapping { get; set; }
        public virtual DbSet<stg_SupplierCountryMapping> stg_SupplierCountryMapping { get; set; }
        public virtual DbSet<stg_SupplierHotelRoomMapping> stg_SupplierHotelRoomMapping { get; set; }
        public virtual DbSet<stg_SupplierProductMapping> stg_SupplierProductMapping { get; set; }
        public virtual DbSet<m_keyword> m_keyword { get; set; }
        public virtual DbSet<m_keyword_alias> m_keyword_alias { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Accommodation_SupplierRoomTypeMapping> Accommodation_SupplierRoomTypeMapping { get; set; }
        public virtual DbSet<SupplierImportFile_Progress> SupplierImportFile_Progress { get; set; }
        public virtual DbSet<SupplierImportFile_VerboseLog> SupplierImportFile_VerboseLog { get; set; }
        public virtual DbSet<vwUserwisemappedStat> vwUserwisemappedStats { get; set; }
        public virtual DbSet<SupplierImportFile_Statistics> SupplierImportFile_Statistics { get; set; }
        public virtual DbSet<STG_Mapping_TableIds> STG_Mapping_TableIds { get; set; }
        public virtual DbSet<Accommodation_SupplierRoomTypeAttributes> Accommodation_SupplierRoomTypeAttributes { get; set; }
        public virtual DbSet<stg_SupplierActivityMapping> stg_SupplierActivityMapping { get; set; }
        public virtual DbSet<Supplier_APILocation> Supplier_APILocation { get; set; }
        public virtual DbSet<Supplier_ApiCallLog> Supplier_ApiCallLog { get; set; }
        public virtual DbSet<Activity_ClassificationAttributes> Activity_ClassificationAttributes { get; set; }
        public virtual DbSet<Activity_Content> Activity_Content { get; set; }
        public virtual DbSet<Activity_Facility> Activity_Facility { get; set; }
        public virtual DbSet<Activity_Flavour> Activity_Flavour { get; set; }
        public virtual DbSet<Activity_Itinerary> Activity_Itinerary { get; set; }
        public virtual DbSet<Activity_PickUpDrop> Activity_PickUpDrop { get; set; }
        public virtual DbSet<Activity_PickUpDropDetail> Activity_PickUpDropDetail { get; set; }
        public virtual DbSet<Activity_PickupDropSchedule> Activity_PickupDropSchedule { get; set; }
        public virtual DbSet<Activity_Tips> Activity_Tips { get; set; }
        public virtual DbSet<Activity_Types> Activity_Types { get; set; }
        public virtual DbSet<Activity_Updates> Activity_Updates { get; set; }
        public virtual DbSet<Activity_WeatherInformation> Activity_WeatherInformation { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Activity_Ancillary> Activity_Ancillary { get; set; }
        public virtual DbSet<Activity_Deals> Activity_Deals { get; set; }
        public virtual DbSet<Activity_Descriptions> Activity_Descriptions { get; set; }
        public virtual DbSet<Activity_Media> Activity_Media { get; set; }
        public virtual DbSet<Activity_Prices> Activity_Prices { get; set; }
        public virtual DbSet<Activity_SupplierActivityImageMapping> Activity_SupplierActivityImageMapping { get; set; }
        public virtual DbSet<Activity_SupplierActivityMetaDataMapping> Activity_SupplierActivityMetaDataMapping { get; set; }
        public virtual DbSet<Activity_SupplierActivityReviews> Activity_SupplierActivityReviews { get; set; }
        public virtual DbSet<Activity_SupplierActivityTypeMapping> Activity_SupplierActivityTypeMapping { get; set; }
        public virtual DbSet<Activity_SupplierProductMapping_CA> Activity_SupplierProductMapping_CA { get; set; }
        public virtual DbSet<Activity_SupplierProductMapping_Deals> Activity_SupplierProductMapping_Deals { get; set; }
        public virtual DbSet<Accommodation_Descriptions> Accommodation_Descriptions { get; set; }
        public virtual DbSet<Activity_Contact> Activity_Contact { get; set; }
        public virtual DbSet<Activity_Status> Activity_Status { get; set; }
        public virtual DbSet<Activity_InclusionDetails> Activity_InclusionDetails { get; set; }
        public virtual DbSet<Activity_Inclusions> Activity_Inclusions { get; set; }
        public virtual DbSet<Activity_Policy> Activity_Policy { get; set; }
        public virtual DbSet<Activity_ReviewsAndScores> Activity_ReviewsAndScores { get; set; }
    
        public virtual int USP_UpdateMapID(string updateIn)
        {
            var updateInParameter = updateIn != null ?
                new ObjectParameter("UpdateIn", updateIn) :
                new ObjectParameter("UpdateIn", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_UpdateMapID", updateInParameter);
        }
    }
}
