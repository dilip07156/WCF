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
    
    public partial class NewDashBoardReport
    {
        public System.Guid Report_ID { get; set; }
        public Nullable<System.Guid> HotelID { get; set; }
        public Nullable<int> CommonHotelID { get; set; }
        public string TLGxAccoID { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string StarRating { get; set; }
        public string HotelName { get; set; }
        public Nullable<bool> IsPreferredHotel { get; set; }
        public string AccoPriority { get; set; }
        public Nullable<System.Guid> Country_Id { get; set; }
        public Nullable<System.Guid> City_Id { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCategorySubType { get; set; }
        public Nullable<int> Count_APM_Mapped_And_AutoMapped { get; set; }
        public Nullable<int> Count_APM_Review { get; set; }
        public Nullable<int> Count_SRT_Attached_to_HOTEL { get; set; }
        public Nullable<int> Count_SRT_Mapped { get; set; }
        public Nullable<int> Count_SRT_Automapped { get; set; }
        public Nullable<int> Count_SRT_Review { get; set; }
        public Nullable<int> Count_SRT_Unmapped { get; set; }
        public Nullable<int> Count_SRT_Add { get; set; }
        public Nullable<int> Count_SuppliersAcco { get; set; }
        public Nullable<int> Count_SuppliersRoom { get; set; }
        public Nullable<int> Count_AccoRooms { get; set; }
        public string ReportType { get; set; }
        public Nullable<int> TotalNoOfHotelRooms { get; set; }
        public Nullable<int> TotalNoOfHotels { get; set; }
        public string Priority { get; set; }
        public string Rank { get; set; }
        public string Key { get; set; }
    }
}
