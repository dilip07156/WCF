//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class m_CityMaster
    {
        public System.Guid City_Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CountryName { get; set; }
        public System.Guid Country_Id { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_User { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_User { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> State_Id { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string Google_PlaceId { get; set; }
        public string CountryCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> KeyNo { get; set; }
        public Nullable<int> RankNo { get; set; }
    }
}
