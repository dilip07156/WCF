using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data.SqlClient;
using DataContracts;
using System.Text.RegularExpressions;

namespace DataLayer
{
    public class DL_Accomodation : IDisposable
    {
        public void Dispose()
        {

        }

        #region Accomodation Search
        public List<DataContracts.DC_Accomodation_Search_RS> AccomodationSearch(DataContracts.DC_Accomodation_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {


                    //var accoSearch = from a in context.Accommodations
                    //                 where a.ProductCategory == RQ.ProductCategory
                    //                 && a.ProductCategorySubType == RQ.ProductCategorySubType
                    //                 select a;
                    var accoSearch = from a in context.Accommodations
                                     select a;

                    if (!string.IsNullOrWhiteSpace(RQ.ProductCategory))
                    {
                        accoSearch = from a in accoSearch
                                     where a.ProductCategory == RQ.ProductCategory
                                     select a;

                    }
                    if (!string.IsNullOrWhiteSpace(RQ.ProductCategorySubType))
                    {
                        accoSearch = from a in accoSearch
                                     where a.ProductCategorySubType == RQ.ProductCategorySubType
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        bool isActive = (RQ.Status == "ACTIVE" ? true : false);
                        accoSearch = from a in accoSearch
                                     where a.IsActive == isActive
                                     select a;
                    }
                    if (RQ.CompanyHotelId != null)
                    {
                        accoSearch = from a in accoSearch
                                     where a.CompanyHotelID == RQ.CompanyHotelId
                                     select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.HotelName))
                    {
                        if (RQ.HotelName.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.HotelName.TrimStart().TrimEnd().ToUpper().Replace(" ", "").Replace("Hotel", "").Replace("'", "").Replace("-", "").Contains(RQ.HotelName.TrimStart().TrimEnd().ToUpper().Replace(" ", "").Replace("Hotel", "").Replace("'", "").Replace("-", ""))
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Country))
                    {
                        if (RQ.Country.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.country == RQ.Country
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City))
                    {
                        if (RQ.City.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.city == RQ.City
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Location))
                    {
                        if (RQ.Location.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Location == RQ.Location
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Chain))
                    {
                        if (RQ.Chain.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Chain.Contains(RQ.Chain)
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Brand))
                    {
                        if (RQ.Brand.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Brand.Contains(RQ.Brand)
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Google_Place_Id))
                    {
                        if (RQ.Google_Place_Id.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Google_Place_Id.Equals(RQ.Google_Place_Id)
                                         select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.AccomodationId))
                    {
                        Guid _guidAccomodationId = Guid.Parse(RQ.AccomodationId);
                        accoSearch = from a in accoSearch
                                     where a.Accommodation_Id == _guidAccomodationId
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Starrating))
                    {
                        accoSearch = from a in accoSearch
                                     where a.HotelRating == RQ.Starrating
                                     select a;
                    }

                    int total;

                    total = accoSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;

                    var accoList = (from a in accoSearch
                                    orderby a.HotelName
                                    select new DataContracts.DC_Accomodation_Search_RS
                                    {
                                        AccomodationId = a.Accommodation_Id.ToString(),
                                        CompanyHotelId = a.CompanyHotelID.ToString(),
                                        CompanyName = a.CompanyName,
                                        City = a.city,
                                        Country = a.country,
                                        HotelBrand = a.Brand,
                                        Starrating = a.HotelRating,
                                        HotelChain = a.Chain,
                                        HotelName = a.HotelName,
                                        Location = a.Location,
                                        PostalCode = a.PostalCode,
                                        Status = a.IsActive ?? false == true ? "Active" : "InActive",
                                        TotalRecords = total,
                                        Google_Place_Id = a.Google_Place_Id,
                                        FullAddress = (a.StreetNumber ?? string.Empty) + ", " + (a.StreetName ?? string.Empty) + ", " + (a.Street3 ?? string.Empty) + ", " + (a.Street4 ?? string.Empty) + ", " + (a.Street5 ?? string.Empty) + ", " + (a.PostalCode ?? string.Empty) + ", " + (a.city ?? string.Empty) + ", " + (a.country ?? string.Empty),
                                        MapCount = 0,
                                        //MapCount = (
                                        //    from am in context.Accommodation_ProductMapping
                                        //    where am.Accommodation_Id == a.Accommodation_Id
                                        //    select new { am.Accommodation_ProductMapping_Id }
                                        //).Count(),
                                        HotelNameWithCode = a.HotelName + " / " + a.CompanyHotelID.ToString()
                                    }).Skip(skip).Take(RQ.PageSize).ToList();

                    if (!string.IsNullOrWhiteSpace(RQ.Searchfrom) && RQ.Searchfrom.ToLower().Trim() != "hotalsearch")
                    {
                        accoList = accoList.Select(c =>
                        {
                            c.MapCount = (context.Accommodation_ProductMapping
                                            .Where(s => (s.Accommodation_Id == Guid.Parse(c.AccomodationId)))
                                            .Select(s1 => s1.Accommodation_ProductMapping_Id)
                                            .Count()
                                            );
                            return c;
                        }).ToList();
                    }

                    return accoList;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<string> GetAccomodationNames(DataContracts.DC_Accomodation_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var accoSearch = from a in context.Accommodations
                                     where a.ProductCategory == RQ.ProductCategory
                                     && a.ProductCategorySubType == RQ.ProductCategorySubType
                                     select a;

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        bool isActive = (RQ.Status == "ACTIVE" ? true : false);
                        accoSearch = from a in accoSearch
                                     where a.IsActive == isActive
                                     select a;
                    }
                    if (!string.IsNullOrWhiteSpace(RQ.Country))
                    {
                        if (RQ.Country.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.country == RQ.Country
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.City))
                    {
                        if (RQ.City.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.city == RQ.City
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Chain))
                    {
                        if (RQ.Chain.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Chain.Contains(RQ.Chain)
                                         select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Brand))
                    {
                        if (RQ.Brand.Length > 0)
                        {
                            accoSearch = from a in accoSearch
                                         where a.Brand.Contains(RQ.Brand)
                                         select a;
                        }
                    }

                    var acco = (from a in accoSearch
                                where a.HotelName.Contains(RQ.HotelName)
                                orderby a.HotelName
                                select a.HotelName + ", " + a.city + ", " + a.country).ToList();

                    return acco;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation list", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.DC_Accomodation> GetAccomodationList(int PageNo, int PageSize)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var total = context.Accommodations.Select(p => p.Accommodation_Id).Count();

                    var skip = PageSize * (PageNo - 1);

                    var canPage = skip < total;

                    if (!canPage) // do what you wish if you can page no further
                        return null;

                    var acco = (from a in context.Accommodations
                                orderby a.HotelName
                                select new DataContracts.DC_Accomodation
                                {
                                    Accommodation_Id = a.Accommodation_Id,
                                    Affiliation = a.Affiliation,
                                    Area = a.Area,
                                    AwardsReceived = a.AwardsReceived,
                                    Brand = a.Brand,
                                    CarbonFootPrint = a.CarbonFootPrint,
                                    Chain = a.Chain,
                                    CheckInTime = a.CheckInTime,
                                    CheckOutTime = a.CheckOutTime,
                                    City = a.city,
                                    City_ISO = a.City_ISO,
                                    CompanyHotelID = a.CompanyHotelID,
                                    CompanyName = a.CompanyName,
                                    CompanyRating = a.CompanyRating,
                                    CompanyRecommended = (a.CompanyRecommended ?? false),
                                    Country = a.country,
                                    Country_ISO = a.Country_ISO,
                                    Create_Date = a.Create_Date,
                                    Create_User = a.Create_User,
                                    DisplayName = a.DisplayName,
                                    Edit_Date = a.Edit_Date,
                                    Edit_User = a.Edit_User,
                                    FinanceControlID = a.FinanceControlID,
                                    Hashtag = a.Hashtag,
                                    HotelName = a.HotelName,
                                    HotelRating = a.HotelRating,
                                    InternalRemarks = a.InternalRemarks,
                                    IsActive = a.IsActive,
                                    IsMysteryProduct = (a.IsMysteryProduct ?? false),
                                    Latitude = a.Latitude,
                                    LEGACY_CITY = a.LEGACY_CITY,
                                    LEGACY_COUNTRY = a.LEGACY_COUNTRY,
                                    Legacy_HTL_ID = a.Legacy_HTL_ID,
                                    LEGACY_STATE = a.LEGACY_STATE,
                                    Location = a.Location,
                                    Longitude = a.Longitude,
                                    OfflineDate = a.OfflineDate,
                                    OnlineDate = a.OnlineDate,
                                    PostalCode = a.PostalCode,
                                    ProductCategory = a.ProductCategory,
                                    ProductCategorySubType = a.ProductCategorySubType,
                                    RatingDate = a.RatingDate,
                                    Reason = a.Reason,
                                    RecommendedFor = a.RecommendedFor,
                                    Remarks = a.Remarks,
                                    State_ISO = a.State_ISO,
                                    State_Name = a.State_Name,
                                    Street3 = a.Street3,
                                    Street4 = a.Street4,
                                    Street5 = a.Street5,
                                    StreetName = a.StreetName,
                                    StreetNumber = a.StreetNumber,
                                    SuburbDowntown = a.SuburbDowntown,
                                    TotalFloors = a.TotalFloors,
                                    TotalRooms = a.TotalRooms,
                                    Town = a.Town,
                                    YearBuilt = a.YearBuilt,
                                    Google_Place_Id = a.Google_Place_Id

                                    //,
                                    //Accommodation_Descriptions = null,
                                    //Accommodation_Facility = null,
                                    //Accommodation_HealthAndSafety = null,
                                    //Accommodation_HotelUpdates = null,
                                    //Accommodation_Media = null,
                                    //Accommodation_NearbyPlaces = null,
                                    //Accommodation_PaxOccupancy = null,
                                    //Accommodation_RoomInfo = null,
                                    //Accommodation_RouteInfo = null,
                                    //Accommodation_RuleInfo = null,
                                    //Accomodation_Contact = null
                                }).Skip(skip).Take(PageSize);

                    return acco.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation list", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Accomodation GetAccomodationShortInfo(Guid Accomodation_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var acco = from a in context.Accommodations
                               where a.Accommodation_Id == Accomodation_Id
                               select new DataContracts.DC_Accomodation
                               {
                                   Accommodation_Id = a.Accommodation_Id,
                                   Brand = a.Brand,
                                   Chain = a.Chain,
                                   City = a.city,
                                   City_ISO = a.City_ISO,
                                   CompanyHotelID = a.CompanyHotelID,
                                   Country = a.country,
                                   Country_ISO = a.Country_ISO,
                                   HotelName = a.HotelName,
                                   HotelRating = a.HotelRating,
                                   IsActive = (a.IsActive ?? true),
                                   StreetName = a.StreetName,
                                   Accomodation_Contact = (from ac in context.Accommodation_Contact
                                                           where ac.Accommodation_Id == a.Accommodation_Id
                                                           select new DataContracts.DC_Accommodation_Contact
                                                           {
                                                               Accommodation_Contact_Id = ac.Accommodation_Contact_Id,
                                                               Accommodation_Id = ac.Accommodation_Id,
                                                               Create_Date = ac.Create_Date,
                                                               Create_User = ac.Create_User,
                                                               Edit_Date = ac.Edit_Date,
                                                               Edit_User = ac.Edit_User,
                                                               Email = ac.Email,
                                                               Fax = ac.Fax,
                                                               Legacy_Htl_Id = ac.Legacy_Htl_Id,
                                                               Telephone = ac.Telephone,
                                                               WebSiteURL = ac.WebSiteURL,
                                                               IsActive = (ac.IsActive ?? true)
                                                           }).ToList()
                               };
                    return acco.FirstOrDefault();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching accomodation details",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Accomodation GetAccomodationDetails(Guid Accomodation_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var acco = from a in context.Accommodations
                               where a.Accommodation_Id == Accomodation_Id
                               select new DataContracts.DC_Accomodation
                               {
                                   Accommodation_Id = a.Accommodation_Id,
                                   Affiliation = a.Affiliation,
                                   Area = a.Area,
                                   AwardsReceived = a.AwardsReceived,
                                   Brand = a.Brand,
                                   CarbonFootPrint = a.CarbonFootPrint,
                                   Chain = a.Chain,
                                   CheckInTime = a.CheckInTime,
                                   CheckOutTime = a.CheckOutTime,
                                   City = a.city,
                                   City_ISO = a.City_ISO,
                                   CompanyHotelID = a.CompanyHotelID,
                                   CompanyName = a.CompanyName,
                                   CompanyRating = a.CompanyRating,
                                   CompanyRecommended = (a.CompanyRecommended ?? false),
                                   Country = a.country,
                                   Country_ISO = a.Country_ISO,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   DisplayName = a.DisplayName,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   FinanceControlID = a.FinanceControlID,
                                   Hashtag = a.Hashtag,
                                   HotelName = a.HotelName,
                                   HotelRating = a.HotelRating,
                                   InternalRemarks = a.InternalRemarks,
                                   IsActive = (a.IsActive ?? true),
                                   IsMysteryProduct = (a.IsMysteryProduct ?? false),
                                   Latitude = a.Latitude,
                                   LEGACY_CITY = a.LEGACY_CITY,
                                   LEGACY_COUNTRY = a.LEGACY_COUNTRY,
                                   Legacy_HTL_ID = a.Legacy_HTL_ID,
                                   LEGACY_STATE = a.LEGACY_STATE,
                                   Location = a.Location,
                                   Longitude = a.Longitude,
                                   OfflineDate = a.OfflineDate,
                                   OnlineDate = a.OnlineDate,
                                   PostalCode = a.PostalCode,
                                   ProductCategory = a.ProductCategory,
                                   ProductCategorySubType = a.ProductCategorySubType,
                                   RatingDate = a.RatingDate,
                                   Reason = a.Reason,
                                   RecommendedFor = a.RecommendedFor,
                                   Remarks = a.Remarks,
                                   State_ISO = a.State_ISO,
                                   State_Name = a.State_Name,
                                   Street3 = a.Street3,
                                   Street4 = a.Street4,
                                   Street5 = a.Street5,
                                   StreetName = a.StreetName,
                                   StreetNumber = a.StreetNumber,
                                   SuburbDowntown = a.SuburbDowntown,
                                   TotalFloors = a.TotalFloors,
                                   TotalRooms = a.TotalRooms,
                                   Town = a.Town,
                                   YearBuilt = a.YearBuilt,
                                   Google_Place_Id = a.Google_Place_Id,
                                   Accommodation_Descriptions = (from ad in context.Accommodation_Descriptions
                                                                 where ad.Accommodation_Id == a.Accommodation_Id
                                                                 select new DataContracts.DC_Accommodation_Descriptions
                                                                 {
                                                                     Accommodation_Description_Id = ad.Accommodation_Description_Id,
                                                                     Accommodation_Id = ad.Accommodation_Id,
                                                                     Create_Date = ad.Create_Date,
                                                                     Create_User = ad.Create_User,
                                                                     Description = ad.Description,
                                                                     DescriptionType = ad.DescriptionType,
                                                                     Edit_Date = ad.Edit_Date,
                                                                     Edit_User = ad.Edit_User,
                                                                     FromDate = ad.FromDate,
                                                                     IsActive = (ad.IsActive ?? true),
                                                                     Language_Code = ad.Language_Code,
                                                                     Legacy_Htl_Id = ad.Legacy_Htl_Id,
                                                                     Source = ad.Source,
                                                                     ToDate = ad.ToDate
                                                                 }
                                                                  ).ToList(),
                                   Accommodation_Facility = (from af in context.Accommodation_Facility
                                                             where af.Accommodation_Id == a.Accommodation_Id
                                                             select new DataContracts.DC_Accommodation_Facility
                                                             {
                                                                 Accommodation_Facility_Id = af.Accommodation_Facility_Id,
                                                                 Accommodation_Id = af.Accommodation_Id,
                                                                 FacilityCategory = af.FacilityCategory,
                                                                 FacilityName = af.FacilityName,
                                                                 FacilityType = af.FacilityType,
                                                                 Description = af.Description,
                                                                 Legacy_Htl_Id = af.Legacy_Htl_Id,
                                                                 Create_Date = af.Create_Date,
                                                                 Create_User = af.Create_User,
                                                                 Edit_Date = af.Edit_Date,
                                                                 Edit_User = af.Edit_User,
                                                                 IsActive = (af.IsActive ?? true)
                                                             }
                                                             ).ToList(),
                                   Accommodation_HealthAndSafety = (from ahs in context.Accommodation_HealthAndSafety
                                                                    where ahs.Accommodation_Id == a.Accommodation_Id
                                                                    select new DataContracts.DC_Accommodation_HealthAndSafety
                                                                    {
                                                                        Accommodation_HealthAndSafety_Id = ahs.Accommodation_HealthAndSafety_Id,
                                                                        Accommodation_Id = ahs.Accommodation_Id,
                                                                        Description = ahs.Description,
                                                                        Name = ahs.Name,
                                                                        Category = ahs.Category,
                                                                        Create_Date = ahs.Create_Date,
                                                                        Create_User = ahs.Create_User,
                                                                        Edit_Date = ahs.Edit_Date,
                                                                        Edit_User = ahs.Edit_User,
                                                                        Legacy_Htl_Id = ahs.Legacy_Htl_Id,
                                                                        Remarks = ahs.Remarks,
                                                                        IsActive = (ahs.IsActive ?? true)
                                                                    }).ToList(),
                                   Accommodation_HotelUpdates = (from ah in context.Accommodation_HotelUpdates
                                                                 where ah.Accommodation_Id == a.Accommodation_Id
                                                                 select new DataContracts.DC_Accommodation_HotelUpdates
                                                                 {
                                                                     Accommodation_HotelUpdates_Id = ah.Accommodation_HotelUpdates_Id,
                                                                     Accommodation_Id = ah.Accommodation_Id,
                                                                     Create_Date = ah.Create_Date,
                                                                     Create_User = ah.Create_User,
                                                                     Description = ah.Description,
                                                                     Edit_Date = ah.Edit_Date,
                                                                     Edit_User = ah.Edit_User,
                                                                     FromDate = ah.FromDate,
                                                                     Source = ah.Source,
                                                                     ToDate = ah.ToDate,
                                                                     IsActive = (ah.IsActive ?? true)
                                                                 }).ToList(),
                                   Accommodation_Media = (from am in context.Accommodation_Media
                                                          where am.Accommodation_Id == a.Accommodation_Id
                                                          select new DataContracts.DC_Accommodation_Media
                                                          {
                                                              Accommodation_Id = am.Accommodation_Id,
                                                              Accommodation_Media_Id = am.Accommodation_Media_Id,
                                                              Create_Date = am.Create_Date,
                                                              Create_User = am.Create_User,
                                                              Edit_Date = am.Edit_Date,
                                                              Edit_User = am.Edit_User,
                                                              IsActive = (am.IsActive ?? true),
                                                              Legacy_Htl_Id = am.Legacy_Htl_Id,
                                                              MediaName = am.MediaName,
                                                              MediaType = am.MediaType,
                                                              Media_Path = am.Media_Path,
                                                              Media_Position = am.Media_Position,
                                                              Media_URL = am.Media_URL,
                                                              RoomCategory = am.RoomCategory,
                                                              ValidFrom = am.ValidFrom,
                                                              ValidTo = am.ValidTo,
                                                              Category = am.Category,
                                                              Description = am.Description,
                                                              FileFormat = am.FileFormat,
                                                              MediaID = am.MediaID,
                                                              SubCategory = am.SubCategory,
                                                              Media_Attributes = (from AMA in context.Media_Attributes
                                                                                  where AMA.Media_Id == am.Accommodation_Media_Id
                                                                                  select new DataContracts.DC_Accomodation_Media_Attributes
                                                                                  {
                                                                                      Accomodation_Media_Attributes_Id = AMA.Media_Attributes_Id,
                                                                                      Accomodation_Media_Id = AMA.Media_Id,
                                                                                      AttributeType = AMA.AttributeType,
                                                                                      AttributeValue = AMA.AttributeValue,
                                                                                      Create_Date = AMA.Create_Date,
                                                                                      Create_User = AMA.Create_User,
                                                                                      Edit_Date = AMA.Edit_Date,
                                                                                      Edit_User = AMA.Edit_User,
                                                                                      IsActive = ((AMA.IsActive ?? true) && (am.IsActive ?? true)),
                                                                                      IsMediaActive = (am.IsActive ?? true)
                                                                                  }).ToList()
                                                          }).ToList(),
                                   Accommodation_NearbyPlaces = (from an in context.Accommodation_NearbyPlaces
                                                                 where an.Accomodation_Id == a.Accommodation_Id
                                                                 select new DataContracts.DC_Accommodation_NearbyPlaces
                                                                 {
                                                                     Accommodation_NearbyPlace_Id = an.Accommodation_NearbyPlace_Id,
                                                                     Accomodation_Id = an.Accomodation_Id,
                                                                     Create_Date = an.Create_Date,
                                                                     Create_User = an.Create_User,
                                                                     Description = an.Description,
                                                                     DistanceFromProperty = an.DistanceFromProperty,
                                                                     DistanceUnit = an.DistanceUnit,
                                                                     Edit_Date = an.Edit_Date,
                                                                     Edit_User = an.Edit_User,
                                                                     Legacy_Htl_Id = an.Legacy_Htl_Id,
                                                                     PlaceCategory = an.PlaceCategory,
                                                                     PlaceName = an.PlaceName,
                                                                     PlaceType = an.PlaceType,
                                                                     IsActive = (an.IsActive ?? true)
                                                                 }).ToList(),
                                   Accommodation_PaxOccupancy = (from ap in context.Accommodation_PaxOccupancy
                                                                 join ri in context.Accommodation_RoomInfo on ap.Accommodation_RoomInfo_Id equals ri.Accommodation_RoomInfo_Id
                                                                 where ap.Accommodation_Id == a.Accommodation_Id
                                                                 select new DataContracts.DC_Accommodation_PaxOccupancy
                                                                 {
                                                                     Accommodation_Id = ap.Accommodation_Id,
                                                                     Accommodation_PaxOccupancy_Id = ap.Accommodation_PaxOccupancy_Id,
                                                                     Accommodation_RoomInfo_Id = ap.Accommodation_RoomInfo_Id,
                                                                     Category = ap.category,
                                                                     CompanyName = ap.companyName,
                                                                     Create_Date = ap.Create_Date,
                                                                     Create_User = ap.Create_User,
                                                                     Edit_Date = ap.Edit_Date,
                                                                     Edit_User = ap.Edit_User,
                                                                     FromAgeForCIOR = ap.FromAgeForCIOR,
                                                                     FromAgeForCNB = ap.FromAgeForCNB,
                                                                     FromAgeForExtraBed = ap.FromAgeForExtraBed,
                                                                     Legacy_Htl_Id = ap.Legacy_Htl_Id,
                                                                     MaxAdults = ap.MaxAdults,
                                                                     MaxChild = ap.MaxChild,
                                                                     MaxCNB = ap.MaxCNB,
                                                                     MaxPax = ap.MaxPax,
                                                                     MaxPaxWithExtraBed = ap.MaxPaxWithExtraBed,
                                                                     RoomDisplayNumber = ap.RoomDisplayNumber,
                                                                     RoomType = ap.RoomType,
                                                                     ToAgeForCIOR = ap.ToAgeForCIOR,
                                                                     ToAgeForCNB = ap.ToAgeForCNB,
                                                                     ToAgeForExtraBed = ap.ToAgeForExtraBed,
                                                                     IsActive = (ap.IsActive ?? true) && (ri.IsActive ?? true)
                                                                 }).ToList(),
                                   Accommodation_RoomInfo = (from ar in context.Accommodation_RoomInfo
                                                             where ar.Accommodation_Id == a.Accommodation_Id
                                                             select new DataContracts.DC_Accommodation_RoomInfo
                                                             {
                                                                 Accommodation_Id = ar.Accommodation_Id,
                                                                 Accommodation_RoomInfo_Id = ar.Accommodation_RoomInfo_Id,
                                                                 AmenityTypes = ar.AmenityTypes,
                                                                 BathRoomType = ar.BathRoomType,
                                                                 BedType = ar.BedType,
                                                                 Category = ar.Category,
                                                                 CompanyName = ar.CompanyName,
                                                                 CompanyRoomCategory = ar.CompanyRoomCategory,
                                                                 Create_Date = ar.Create_Date,
                                                                 Create_User = ar.Create_User,
                                                                 Description = ar.Description,
                                                                 Edit_Date = ar.Edit_Date,
                                                                 Edit_User = ar.Edit_User,
                                                                 FloorName = ar.FloorName,
                                                                 FloorNumber = ar.FloorNumber,
                                                                 Legacy_Htl_Id = ar.Legacy_Htl_Id,
                                                                 MysteryRoom = ar.MysteryRoom,
                                                                 NoOfInterconnectingRooms = ar.NoOfInterconnectingRooms,
                                                                 NoOfRooms = ar.NoOfRooms,
                                                                 RoomCategory = ar.RoomCategory,
                                                                 RoomDecor = ar.RoomDecor,
                                                                 RoomId = ar.RoomId,
                                                                 RoomName = ar.RoomName,
                                                                 RoomSize = ar.RoomSize,
                                                                 RoomView = ar.RoomView,
                                                                 Smoking = ar.Smoking,
                                                                 IsActive = (ar.IsActive ?? true),
                                                                 RoomFacilities = (from rf in context.Accommodation_RoomFacility
                                                                                   where rf.Accommodation_Id == ar.Accommodation_Id
                                                                                   && rf.Accommodation_RoomInfo_Id == ar.Accommodation_RoomInfo_Id
                                                                                   select new DataContracts.DC_Accomodation_RoomFacilities
                                                                                   {
                                                                                       Accommodation_Id = rf.Accommodation_Id,
                                                                                       Accommodation_RoomFacility_Id = rf.Accommodation_RoomFacility_Id,
                                                                                       Accommodation_RoomInfo_Id = rf.Accommodation_RoomInfo_Id,
                                                                                       AmenityName = rf.AmenityName,
                                                                                       AmenityType = rf.AmenityType,
                                                                                       Create_Date = rf.Create_Date,
                                                                                       Create_User = rf.Create_User,
                                                                                       Description = rf.Description,
                                                                                       Edit_Date = rf.Edit_Date,
                                                                                       Edit_user = rf.Edit_user,
                                                                                       IsActive = (rf.IsActive ?? true) && (ar.IsActive ?? true)
                                                                                   }).ToList()
                                                             }).ToList(),
                                   Accommodation_RouteInfo = (from ari in context.Accommodation_RouteInfo
                                                              where ari.Accommodation_Id == a.Accommodation_Id
                                                              select new DataContracts.DC_Accommodation_RouteInfo
                                                              {
                                                                  AccommodationCode = ari.AccommodationCode,
                                                                  Accommodation_Id = ari.Accommodation_Id,
                                                                  Accommodation_Route_Id = ari.Accommodation_Route_Id,
                                                                  ApproximateDuration = ari.ApproximateDuration,
                                                                  CompanyName = ari.CompanyName,
                                                                  Create_Date = ari.Create_Date,
                                                                  Create_User = ari.Create_User,
                                                                  Description = ari.Description,
                                                                  DistanceFromProperty = ari.DistanceFromProperty,
                                                                  DistanceUnit = ari.DistanceUnit,
                                                                  DrivingDirection = ari.DrivingDirection,
                                                                  Edit_Date = ari.Edit_Date,
                                                                  Edit_User = ari.Edit_User,
                                                                  FromPlace = ari.FromPlace,
                                                                  Legacy_Htl_Id = ari.Legacy_Htl_Id,
                                                                  ModeOfTransport = ari.ModeOfTransport,
                                                                  NameOfPlace = ari.NameOfPlace,
                                                                  TransportType = ari.TransportType,
                                                                  ValidFrom = ari.ValidFrom,
                                                                  ValidTo = ari.ValidTo,
                                                                  IsActive = (ari.IsActive ?? true)
                                                              }).ToList(),
                                   Accommodation_RuleInfo = (from arl in context.Accommodation_RuleInfo
                                                             where arl.Accommodation_Id == a.Accommodation_Id
                                                             select new DataContracts.DC_Accommodation_RuleInfo
                                                             {
                                                                 Accommodation_Id = arl.Accommodation_Id,
                                                                 Accommodation_RuleInfo_Id = arl.Accommodation_RuleInfo_Id,
                                                                 Create_Date = arl.Create_Date,
                                                                 Create_User = arl.Create_User,
                                                                 Description = arl.Description,
                                                                 Edit_Date = arl.Edit_Date,
                                                                 Edit_User = arl.Edit_User,
                                                                 Legacy_Htl_Id = arl.Legacy_Htl_Id,
                                                                 RuleType = arl.RuleType,
                                                                 IsActive = (arl.IsActive ?? true)
                                                             }).ToList(),
                                   Accomodation_Contact = (from ac in context.Accommodation_Contact
                                                           where ac.Accommodation_Id == a.Accommodation_Id
                                                           select new DataContracts.DC_Accommodation_Contact
                                                           {
                                                               Accommodation_Contact_Id = ac.Accommodation_Contact_Id,
                                                               Accommodation_Id = ac.Accommodation_Id,
                                                               Create_Date = ac.Create_Date,
                                                               Create_User = ac.Create_User,
                                                               Edit_Date = ac.Edit_Date,
                                                               Edit_User = ac.Edit_User,
                                                               Email = ac.Email,
                                                               Fax = ac.Fax,
                                                               Legacy_Htl_Id = ac.Legacy_Htl_Id,
                                                               Telephone = ac.Telephone,
                                                               WebSiteURL = ac.WebSiteURL,
                                                               IsActive = (ac.IsActive ?? true)
                                                           }).ToList(),
                                   Accomodation_Status = (from ast in context.Accommodation_Status
                                                          where ast.Accommodation_Id == a.Accommodation_Id
                                                          select new DataContracts.DC_Accommodation_Status
                                                          {
                                                              Accommodation_Id = ast.Accommodation_Id,
                                                              Accommodation_Status_Id = ast.Accommodation_Status_Id,
                                                              CompanyMarket = ast.CompanyMarket,
                                                              DeactivationReason = ast.DeactivationReason,
                                                              From = ast.From,
                                                              Status = ast.Status,
                                                              To = ast.To,
                                                              Create_Date = ast.Create_Date,
                                                              Create_User = ast.Create_User,
                                                              Edit_Date = ast.Edit_Date,
                                                              Edit_User = ast.Edit_User,
                                                              IsActive = (ast.IsActive ?? true)
                                                          }).ToList(),
                                   Accomodation_DynamicAttributes = (from da in context.DynamicAttributes
                                                                     where da.Object_Id == a.Accommodation_Id
                                                                     select new DataContracts.Masters.DC_DynamicAttributes
                                                                     {
                                                                         DynamicAttribute_Id = da.DynamicAttribute_Id,
                                                                         AttributeClass = da.AttributeClass,
                                                                         AttributeName = da.AttributeName,
                                                                         AttributeValue = da.AttributeValue,
                                                                         ObjectSubElement_Id = da.ObjectSubElement_Id,
                                                                         ObjectType = da.ObjectType,
                                                                         Object_Id = da.Object_Id,
                                                                         Create_Date = da.Create_Date,
                                                                         Create_User = da.Create_User,
                                                                         Edit_Date = da.Edit_Date,
                                                                         Edit_User = da.Edit_User,
                                                                         IsActive = (da.IsActive ?? true)
                                                                     }).ToList(),
                                   Accomodation_ClassificationAttributes = (from aca in context.Accommodation_ClassificationAttributes
                                                                            where aca.Accommodation_Id == Accomodation_Id
                                                                            select new DataContracts.DC_Accomodation_ClassificationAttributes
                                                                            {
                                                                                Accommodation_ClassificationAttribute_Id = aca.Accommodation_ClassificationAttribute_Id,
                                                                                Accommodation_Id = aca.Accommodation_Id,
                                                                                AttributeSubType = aca.AttributeSubType,
                                                                                AttributeType = aca.AttributeType,
                                                                                AttributeValue = aca.AttributeValue,
                                                                                Create_Date = aca.Create_Date,
                                                                                Create_User = aca.Create_User,
                                                                                Edit_Date = aca.Edit_Date,
                                                                                Edit_User = aca.Edit_User,
                                                                                InternalOnly = aca.InternalOnly,
                                                                                IsActive = (aca.IsActive ?? true)
                                                                            }).ToList()
                               };

                    return acco.FirstOrDefault();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region AccomodationInfo
        public List<DataContracts.DC_Accomodation> GetAccomodationInfo(Guid Accomodation_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var acco = from a in context.Accommodations
                               where a.Accommodation_Id == Accomodation_Id
                               select new DataContracts.DC_Accomodation
                               {
                                   Accommodation_Id = a.Accommodation_Id,
                                   Affiliation = a.Affiliation,
                                   Area = a.Area,
                                   AwardsReceived = a.AwardsReceived,
                                   Brand = a.Brand,
                                   CarbonFootPrint = a.CarbonFootPrint,
                                   Chain = a.Chain,
                                   CheckInTime = a.CheckInTime,
                                   CheckOutTime = a.CheckOutTime,
                                   City = a.city,
                                   City_ISO = a.City_ISO,
                                   CompanyHotelID = a.CompanyHotelID,
                                   CompanyName = a.CompanyName,
                                   CompanyRating = a.CompanyRating,
                                   CompanyRecommended = (a.CompanyRecommended ?? false),
                                   Country = a.country,
                                   Country_ISO = a.Country_ISO,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   DisplayName = a.DisplayName,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   FinanceControlID = a.FinanceControlID,
                                   Hashtag = a.Hashtag,
                                   HotelName = a.HotelName,
                                   HotelRating = a.HotelRating,
                                   InternalRemarks = a.InternalRemarks,
                                   IsActive = a.IsActive,
                                   IsMysteryProduct = (a.IsMysteryProduct ?? false),
                                   Latitude = a.Latitude,
                                   LEGACY_CITY = a.LEGACY_CITY,
                                   LEGACY_COUNTRY = a.LEGACY_COUNTRY,
                                   Legacy_HTL_ID = a.Legacy_HTL_ID,
                                   LEGACY_STATE = a.LEGACY_STATE,
                                   Location = a.Location,
                                   Longitude = a.Longitude,
                                   OfflineDate = a.OfflineDate,
                                   OnlineDate = a.OnlineDate,
                                   PostalCode = a.PostalCode,
                                   ProductCategory = a.ProductCategory,
                                   ProductCategorySubType = a.ProductCategorySubType,
                                   RatingDate = a.RatingDate,
                                   Reason = a.Reason,
                                   RecommendedFor = a.RecommendedFor,
                                   Remarks = a.Remarks,
                                   State_ISO = a.State_ISO,
                                   State_Name = a.State_Name,
                                   Street3 = a.Street3,
                                   Street4 = a.Street4,
                                   Street5 = a.Street5,
                                   StreetName = a.StreetName,
                                   StreetNumber = a.StreetNumber,
                                   SuburbDowntown = a.SuburbDowntown,
                                   TotalFloors = a.TotalFloors,
                                   TotalRooms = a.TotalRooms,
                                   Town = a.Town,
                                   YearBuilt = a.YearBuilt,
                                   Google_Place_Id = a.Google_Place_Id,
                                   Accomodation_Contact = (from ac in context.Accommodation_Contact
                                                           where ac.Accommodation_Id == a.Accommodation_Id
                                                           select new DataContracts.DC_Accommodation_Contact
                                                           {
                                                               Accommodation_Contact_Id = ac.Accommodation_Contact_Id,
                                                               Accommodation_Id = ac.Accommodation_Id,
                                                               Create_Date = ac.Create_Date,
                                                               Create_User = ac.Create_User,
                                                               Edit_Date = ac.Edit_Date,
                                                               Edit_User = ac.Edit_User,
                                                               Email = ac.Email,
                                                               Fax = ac.Fax,
                                                               Legacy_Htl_Id = ac.Legacy_Htl_Id,
                                                               Telephone = ac.Telephone,
                                                               WebSiteURL = ac.WebSiteURL,
                                                               IsActive = (ac.IsActive ?? true)
                                                           }).ToList()
                               };

                    return acco.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationGoogleInfo(DataContracts.DC_Accomodation AccomodationDetails)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodations
                                  where a.Accommodation_Id == AccomodationDetails.Accommodation_Id
                                  select a).First();

                    if (search != null)
                    {
                        search.Latitude = AccomodationDetails.Latitude;
                        search.Longitude = AccomodationDetails.Longitude;
                        search.Google_Place_Id = AccomodationDetails.Google_Place_Id;
                        search.Edit_Date = AccomodationDetails.Edit_Date;
                        search.Edit_User = AccomodationDetails.Edit_User;
                        context.SaveChanges();

                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public bool UpdateAccomodationInfo(DataContracts.DC_Accomodation AccomodationDetails)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodations
                                  where a.Accommodation_Id == AccomodationDetails.Accommodation_Id
                                  select a).First();

                    if (search != null)
                    {
                        search.Affiliation = AccomodationDetails.Affiliation;
                        search.Area = AccomodationDetails.Area;
                        search.AwardsReceived = AccomodationDetails.AwardsReceived;
                        search.Brand = AccomodationDetails.Brand;
                        search.CarbonFootPrint = AccomodationDetails.CarbonFootPrint;
                        search.Chain = AccomodationDetails.Chain;
                        search.CheckInTime = AccomodationDetails.CheckInTime;
                        search.CheckOutTime = AccomodationDetails.CheckOutTime;
                        search.city = AccomodationDetails.City;
                        search.City_ISO = AccomodationDetails.City_ISO;
                        //search.CompanyHotelID = AccomodationDetails.CompanyHotelID;
                        search.CompanyName = AccomodationDetails.CompanyName;
                        search.CompanyRating = AccomodationDetails.CompanyRating;
                        search.CompanyRecommended = AccomodationDetails.CompanyRecommended;
                        search.country = AccomodationDetails.Country;
                        search.Country_ISO = AccomodationDetails.Country_ISO;
                        search.DisplayName = AccomodationDetails.DisplayName;
                        search.Edit_Date = AccomodationDetails.Edit_Date;
                        search.Edit_User = AccomodationDetails.Edit_User;
                        search.FinanceControlID = AccomodationDetails.FinanceControlID;
                        search.Hashtag = AccomodationDetails.Hashtag;
                        search.HotelName = AccomodationDetails.HotelName;
                        search.InternalRemarks = AccomodationDetails.InternalRemarks;
                        search.IsActive = AccomodationDetails.IsActive;
                        search.IsMysteryProduct = AccomodationDetails.IsMysteryProduct;
                        search.Latitude = AccomodationDetails.Latitude;
                        search.LEGACY_CITY = AccomodationDetails.LEGACY_CITY;
                        search.LEGACY_COUNTRY = AccomodationDetails.LEGACY_COUNTRY;
                        //search.Legacy_HTL_ID = AccomodationDetails.Legacy_HTL_ID;
                        search.LEGACY_STATE = AccomodationDetails.LEGACY_STATE;
                        search.Location = AccomodationDetails.Location;
                        search.Longitude = AccomodationDetails.Longitude;
                        search.OfflineDate = AccomodationDetails.OfflineDate;
                        search.OnlineDate = AccomodationDetails.OnlineDate;
                        search.PostalCode = AccomodationDetails.PostalCode;
                        search.ProductCategory = AccomodationDetails.ProductCategory;
                        search.ProductCategorySubType = AccomodationDetails.ProductCategorySubType;

                        //Check Ratting changes
                        if (search.HotelRating != AccomodationDetails.HotelRating)
                            search.RatingDate = DateTime.Today.Date;

                        search.HotelRating = AccomodationDetails.HotelRating;
                        search.Reason = AccomodationDetails.Reason;
                        search.RecommendedFor = AccomodationDetails.RecommendedFor;
                        search.Remarks = AccomodationDetails.Remarks;
                        search.State_ISO = AccomodationDetails.State_ISO;
                        search.State_Name = AccomodationDetails.State_Name;
                        search.Street3 = AccomodationDetails.Street3;
                        search.Street4 = AccomodationDetails.Street4;
                        search.Street5 = AccomodationDetails.Street5;
                        search.StreetName = AccomodationDetails.StreetName;
                        search.StreetNumber = AccomodationDetails.StreetNumber;
                        search.SuburbDowntown = AccomodationDetails.SuburbDowntown;
                        search.TotalFloors = AccomodationDetails.TotalFloors;
                        search.TotalRooms = AccomodationDetails.TotalRooms;
                        search.Town = AccomodationDetails.Town;
                        search.YearBuilt = AccomodationDetails.YearBuilt;
                        search.Google_Place_Id = AccomodationDetails.Google_Place_Id;

                        context.SaveChanges();

                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationInfo(DataContracts.DC_Accomodation AccomodationDetails)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    Accommodation newAcco = new Accommodation();

                    if (AccomodationDetails.Accommodation_Id == null)
                    {
                        newAcco.Accommodation_Id = Guid.NewGuid();
                    }
                    else
                    {
                        newAcco.Accommodation_Id = AccomodationDetails.Accommodation_Id;
                    }

                    newAcco.Affiliation = AccomodationDetails.Affiliation;
                    newAcco.Area = AccomodationDetails.Area;
                    newAcco.AwardsReceived = AccomodationDetails.AwardsReceived;
                    newAcco.Brand = AccomodationDetails.Brand;
                    newAcco.CarbonFootPrint = AccomodationDetails.CarbonFootPrint;
                    newAcco.Chain = AccomodationDetails.Chain;
                    newAcco.CheckInTime = AccomodationDetails.CheckInTime;
                    newAcco.CheckOutTime = AccomodationDetails.CheckOutTime;
                    newAcco.city = AccomodationDetails.City;
                    newAcco.City_ISO = AccomodationDetails.City_ISO;
                    newAcco.CompanyHotelID = AccomodationDetails.CompanyHotelID;
                    newAcco.CompanyName = AccomodationDetails.CompanyName;
                    newAcco.CompanyRating = AccomodationDetails.CompanyRating;
                    newAcco.CompanyRecommended = AccomodationDetails.CompanyRecommended;
                    newAcco.country = AccomodationDetails.Country;
                    newAcco.Country_ISO = AccomodationDetails.Country_ISO;
                    newAcco.Create_Date = AccomodationDetails.Create_Date;
                    newAcco.Create_User = AccomodationDetails.Create_User;
                    newAcco.DisplayName = AccomodationDetails.DisplayName;
                    newAcco.FinanceControlID = AccomodationDetails.FinanceControlID;
                    newAcco.Hashtag = AccomodationDetails.Hashtag;
                    newAcco.HotelName = AccomodationDetails.HotelName;
                    newAcco.HotelRating = AccomodationDetails.HotelRating;
                    newAcco.InternalRemarks = AccomodationDetails.InternalRemarks;
                    newAcco.IsActive = AccomodationDetails.IsActive;
                    newAcco.IsMysteryProduct = AccomodationDetails.IsMysteryProduct;
                    newAcco.Latitude = AccomodationDetails.Latitude;
                    newAcco.LEGACY_CITY = AccomodationDetails.LEGACY_CITY;
                    newAcco.LEGACY_COUNTRY = AccomodationDetails.LEGACY_COUNTRY;
                    newAcco.Legacy_HTL_ID = AccomodationDetails.Legacy_HTL_ID;
                    newAcco.LEGACY_STATE = AccomodationDetails.LEGACY_STATE;
                    newAcco.Location = AccomodationDetails.Location;
                    newAcco.Longitude = AccomodationDetails.Longitude;
                    newAcco.OfflineDate = AccomodationDetails.OfflineDate;
                    newAcco.OnlineDate = AccomodationDetails.OnlineDate;
                    newAcco.PostalCode = AccomodationDetails.PostalCode;
                    newAcco.ProductCategory = AccomodationDetails.ProductCategory;
                    newAcco.ProductCategorySubType = AccomodationDetails.ProductCategorySubType;
                    newAcco.RatingDate = AccomodationDetails.RatingDate;
                    newAcco.Reason = AccomodationDetails.Reason;
                    newAcco.RecommendedFor = AccomodationDetails.RecommendedFor;
                    newAcco.Remarks = AccomodationDetails.Remarks;
                    newAcco.State_ISO = AccomodationDetails.State_ISO;
                    newAcco.State_Name = AccomodationDetails.State_Name;
                    newAcco.Street3 = AccomodationDetails.Street3;
                    newAcco.Street4 = AccomodationDetails.Street4;
                    newAcco.Street5 = AccomodationDetails.Street5;
                    newAcco.StreetName = AccomodationDetails.StreetName;
                    newAcco.StreetNumber = AccomodationDetails.StreetNumber;
                    newAcco.SuburbDowntown = AccomodationDetails.SuburbDowntown;
                    newAcco.TotalFloors = AccomodationDetails.TotalFloors;
                    newAcco.TotalRooms = AccomodationDetails.TotalRooms;
                    newAcco.Town = AccomodationDetails.Town;
                    newAcco.YearBuilt = AccomodationDetails.YearBuilt;
                    newAcco.Google_Place_Id = AccomodationDetails.Google_Place_Id;
                    context.Accommodations.Add(newAcco);

                    context.SaveChanges();

                    context.USP_UpdateMapID("accommodation");

                    newAcco = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationDetailStatus(DataContracts.DC_Accomodation_UpdateStatus_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodations
                                  where a.Accommodation_Id == obj.Accommodation_Id
                                  select a).First();

                    if (search != null)
                    {
                        search.IsActive = obj.Status;
                        search.Edit_Date = obj.Edit_Date;
                        search.Edit_User = obj.Edit_User;
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation Contacts
        public List<DataContracts.DC_Accommodation_Contact> GetAccomodationContacts(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ac in context.Accommodation_Contact
                                 where ac.Accommodation_Id == Accomodation_Id
                                 && ac.Accommodation_Contact_Id == (DataKey_Id == Guid.Empty ? ac.Accommodation_Contact_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_Contact
                                 {
                                     Accommodation_Contact_Id = ac.Accommodation_Contact_Id,
                                     Accommodation_Id = ac.Accommodation_Id,
                                     Create_Date = ac.Create_Date,
                                     Create_User = ac.Create_User,
                                     Edit_Date = ac.Edit_Date,
                                     Edit_User = ac.Edit_User,
                                     Email = ac.Email,
                                     Fax = ac.Fax,
                                     Legacy_Htl_Id = ac.Legacy_Htl_Id,
                                     Telephone = ac.Telephone,
                                     WebSiteURL = ac.WebSiteURL,
                                     IsActive = (ac.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ac in context.Accommodation_Contact
                                  where ac.Accommodation_Contact_Id == AC.Accommodation_Contact_Id
                                  select ac).First();
                    if (search != null)
                    {
                        if ((AC.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = AC.IsActive;
                            search.Edit_Date = AC.Edit_Date;
                            search.Edit_User = AC.Edit_User;
                        }
                        else
                        {
                            search.Edit_Date = AC.Edit_Date;
                            search.Edit_User = AC.Edit_User;
                            search.Email = AC.Email;
                            search.Fax = AC.Fax;
                            search.Legacy_Htl_Id = AC.Legacy_Htl_Id;
                            search.Telephone = AC.Telephone;
                            search.WebSiteURL = AC.WebSiteURL;
                            search.IsActive = AC.IsActive;
                        }
                        context.SaveChanges();
                    }

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationContacts(DataContracts.DC_Accommodation_Contact AC)
        {
            try
            {
                if (AC.Accommodation_Id == null)
                {
                    return false;
                }

                if (AC.Accommodation_Contact_Id == null)
                {
                    AC.Accommodation_Contact_Id = Guid.NewGuid();
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Accommodation_Contact newObj = new Accommodation_Contact();
                    newObj.Accommodation_Contact_Id = AC.Accommodation_Contact_Id;
                    newObj.Accommodation_Id = AC.Accommodation_Id;
                    newObj.Create_Date = AC.Create_Date;
                    newObj.Create_User = AC.Create_User;
                    newObj.Email = AC.Email;
                    newObj.Fax = AC.Fax;
                    newObj.Legacy_Htl_Id = AC.Legacy_Htl_Id;
                    newObj.Telephone = AC.Telephone;
                    newObj.WebSiteURL = AC.WebSiteURL;
                    newObj.IsActive = AC.IsActive;

                    context.Accommodation_Contact.Add(newObj);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation Descriptions
        public List<DataContracts.DC_Accommodation_Descriptions> GetAccomodationDescriptions(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ad in context.Accommodation_Descriptions
                                 where ad.Accommodation_Id == Accomodation_Id
                                 && ad.Accommodation_Description_Id == (DataKey_Id == Guid.Empty ? ad.Accommodation_Description_Id : DataKey_Id)
                                 //&& (ad.IsActive ?? true) == true
                                 select new DataContracts.DC_Accommodation_Descriptions
                                 {
                                     Accommodation_Description_Id = ad.Accommodation_Description_Id,
                                     Accommodation_Id = ad.Accommodation_Id,
                                     Create_Date = ad.Create_Date,
                                     Create_User = ad.Create_User,
                                     Description = ad.Description,
                                     DescriptionType = ad.DescriptionType,
                                     Edit_Date = ad.Edit_Date,
                                     Edit_User = ad.Edit_User,
                                     FromDate = ad.FromDate,
                                     IsActive = (ad.IsActive ?? true),
                                     Language_Code = ad.Language_Code,
                                     Legacy_Htl_Id = ad.Legacy_Htl_Id,
                                     Source = ad.Source,
                                     ToDate = ad.ToDate
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation descriptions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public bool UpdateAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ad in context.Accommodation_Descriptions
                                  where ad.Accommodation_Description_Id == AD.Accommodation_Description_Id
                                  select ad).First();
                    if (search != null)
                    {
                        if ((AD.IsActive ?? true) != (search.IsActive ?? true))
                        {
                            search.IsActive = AD.IsActive;
                            search.Edit_Date = AD.Edit_Date;
                            search.Edit_User = AD.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = AD.Accommodation_Id;
                            search.Description = AD.Description;
                            search.DescriptionType = AD.DescriptionType;
                            search.Edit_Date = AD.Edit_Date;
                            search.Edit_User = AD.Edit_User;
                            search.FromDate = AD.FromDate;
                            search.IsActive = AD.IsActive;
                            search.Legacy_Htl_Id = AD.Legacy_Htl_Id;
                            search.Source = AD.Source;
                            search.ToDate = AD.ToDate;
                            search.Language_Code = AD.Language_Code;
                        }

                        context.SaveChanges();
                    }

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation descriptions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationDescriptions(DataContracts.DC_Accommodation_Descriptions AD)
        {
            try
            {
                if (AD.Accommodation_Id == null)
                {
                    return false;
                }

                if (AD.Accommodation_Description_Id == null)
                {
                    AD.Accommodation_Description_Id = Guid.NewGuid();
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Accommodation_Descriptions newObj = new Accommodation_Descriptions();

                    newObj.Accommodation_Description_Id = AD.Accommodation_Description_Id;
                    newObj.Accommodation_Id = AD.Accommodation_Id;
                    newObj.Create_Date = AD.Create_Date;
                    newObj.Create_User = AD.Create_User;
                    newObj.Description = AD.Description;
                    newObj.DescriptionType = AD.DescriptionType;
                    newObj.FromDate = AD.FromDate;
                    newObj.IsActive = AD.IsActive;
                    newObj.Language_Code = AD.Language_Code;
                    newObj.Legacy_Htl_Id = AD.Legacy_Htl_Id;
                    newObj.Source = AD.Source;
                    newObj.ToDate = AD.ToDate;

                    context.Accommodation_Descriptions.Add(newObj);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation Facilities
        public List<DataContracts.DC_Accommodation_Facility> GetAccomodationFacilities(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from af in context.Accommodation_Facility
                                 where af.Accommodation_Id == Accomodation_Id
                                 && af.Accommodation_Facility_Id == (DataKey_Id == Guid.Empty ? af.Accommodation_Facility_Id : DataKey_Id)
                                 orderby af.FacilityCategory
                                 select new DataContracts.DC_Accommodation_Facility
                                 {
                                     Accommodation_Facility_Id = af.Accommodation_Facility_Id,
                                     Accommodation_Id = af.Accommodation_Id,
                                     FacilityCategory = af.FacilityCategory,
                                     FacilityName = af.FacilityName,
                                     FacilityType = af.FacilityType,
                                     Description = af.Description,
                                     Legacy_Htl_Id = af.Legacy_Htl_Id,
                                     Create_Date = af.Create_Date,
                                     Create_User = af.Create_User,
                                     Edit_Date = af.Edit_Date,
                                     Edit_User = af.Edit_User,
                                     IsActive = (af.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation facilities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public bool UpdateAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodation_Facility
                                  where a.Accommodation_Facility_Id == AF.Accommodation_Facility_Id
                                  select a).First();
                    if (search != null)
                    {
                        if ((AF.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = AF.IsActive;
                            search.Edit_Date = AF.Edit_Date;
                            search.Edit_User = AF.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = AF.Accommodation_Id;
                            search.Description = AF.Description;
                            search.FacilityCategory = AF.FacilityCategory;
                            search.FacilityName = AF.FacilityName;
                            search.FacilityType = AF.FacilityType;
                            search.Legacy_Htl_Id = AF.Legacy_Htl_Id;
                            search.Edit_Date = AF.Edit_Date;
                            search.Edit_User = AF.Edit_User;
                            search.IsActive = AF.IsActive;
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation facilities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationFacilities(DataContracts.DC_Accommodation_Facility AF)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (AF.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_Facility objNew = new Accommodation_Facility();

                    if (AF.Accommodation_Facility_Id == null)
                    {
                        AF.Accommodation_Facility_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Facility_Id = AF.Accommodation_Facility_Id;
                    objNew.Accommodation_Id = AF.Accommodation_Id;
                    objNew.Create_Date = AF.Create_Date;
                    objNew.Create_User = AF.Create_User;
                    objNew.Description = AF.Description;
                    objNew.FacilityCategory = AF.FacilityCategory;
                    objNew.FacilityName = AF.FacilityName;
                    objNew.FacilityType = AF.FacilityType;
                    objNew.Legacy_Htl_Id = AF.Legacy_Htl_Id;
                    objNew.IsActive = AF.IsActive;

                    context.Accommodation_Facility.Add(objNew);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation facilities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Accomodation HealthAndSafety
        public List<DataContracts.DC_Accommodation_HealthAndSafety> GetAccomodationHealthAndSafety(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ahs in context.Accommodation_HealthAndSafety
                                 where ahs.Accommodation_Id == Accomodation_Id
                                 && ahs.Accommodation_HealthAndSafety_Id == (DataKey_Id == Guid.Empty ? ahs.Accommodation_HealthAndSafety_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_HealthAndSafety
                                 {
                                     Accommodation_HealthAndSafety_Id = ahs.Accommodation_HealthAndSafety_Id,
                                     Accommodation_Id = ahs.Accommodation_Id,
                                     Description = ahs.Description,
                                     Name = ahs.Name,
                                     Category = ahs.Category,
                                     Create_Date = ahs.Create_Date,
                                     Create_User = ahs.Create_User,
                                     Edit_Date = ahs.Edit_Date,
                                     Edit_User = ahs.Edit_User,
                                     Legacy_Htl_Id = ahs.Legacy_Htl_Id,
                                     Remarks = ahs.Remarks,
                                     IsActive = (ahs.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation hotel and safety", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (HS.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_HealthAndSafety objNew = new Accommodation_HealthAndSafety();

                    if (HS.Accommodation_HealthAndSafety_Id == null)
                    {
                        HS.Accommodation_HealthAndSafety_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_HealthAndSafety_Id = HS.Accommodation_HealthAndSafety_Id;
                    objNew.Accommodation_Id = HS.Accommodation_Id;
                    objNew.Create_Date = HS.Create_Date;
                    objNew.Create_User = HS.Create_User;
                    objNew.Description = HS.Description;
                    objNew.Category = HS.Category;
                    objNew.Legacy_Htl_Id = HS.Legacy_Htl_Id;
                    objNew.Name = HS.Name;
                    objNew.Remarks = HS.Remarks;
                    objNew.IsActive = HS.IsActive;

                    context.Accommodation_HealthAndSafety.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation Health and Safety", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationHealthAndSafety(DataContracts.DC_Accommodation_HealthAndSafety HS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_HealthAndSafety.Find(HS.Accommodation_HealthAndSafety_Id);

                    if (search != null)
                    {
                        if ((HS.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = HS.IsActive;
                            search.Edit_Date = HS.Edit_Date;
                            search.Edit_User = HS.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = HS.Accommodation_Id;
                            search.Edit_Date = HS.Edit_Date;
                            search.Edit_User = HS.Edit_User;
                            search.Description = HS.Description;
                            search.Category = HS.Category;
                            search.Description = HS.Description;
                            search.Legacy_Htl_Id = HS.Legacy_Htl_Id;
                            search.Name = HS.Name;
                            search.Remarks = HS.Remarks;
                            search.IsActive = HS.IsActive;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation Hotel and Safety", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation HotelUpdates
        public List<DataContracts.DC_Accommodation_HotelUpdates> GetAccomodationHotelUpdates(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ah in context.Accommodation_HotelUpdates
                                 where ah.Accommodation_Id == Accomodation_Id
                                 && ah.Accommodation_HotelUpdates_Id == (DataKey_Id == Guid.Empty ? ah.Accommodation_HotelUpdates_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_HotelUpdates
                                 {
                                     Accommodation_HotelUpdates_Id = ah.Accommodation_HotelUpdates_Id,
                                     Accommodation_Id = ah.Accommodation_Id,
                                     Create_Date = ah.Create_Date,
                                     Create_User = ah.Create_User,
                                     Description = ah.Description,
                                     Edit_Date = ah.Edit_Date,
                                     Edit_User = ah.Edit_User,
                                     FromDate = ah.FromDate,
                                     Source = ah.Source,
                                     ToDate = ah.ToDate,
                                     IsActive = (ah.IsActive ?? true),
                                     IsInternal = (ah.IsInternal ?? false)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation updates", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (HU.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_HotelUpdates objNew = new Accommodation_HotelUpdates();

                    if (HU.Accommodation_HotelUpdates_Id == null)
                    {
                        HU.Accommodation_HotelUpdates_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_HotelUpdates_Id = HU.Accommodation_HotelUpdates_Id;
                    objNew.Accommodation_Id = HU.Accommodation_Id;
                    objNew.Create_Date = HU.Create_Date;
                    objNew.Create_User = HU.Create_User;
                    objNew.Description = HU.Description;
                    objNew.FromDate = HU.FromDate;
                    objNew.Source = HU.Source;
                    objNew.ToDate = HU.ToDate;
                    objNew.IsActive = HU.IsActive;
                    objNew.IsInternal = HU.IsInternal;

                    context.Accommodation_HotelUpdates.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation Updates", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationHotelUpdates(DataContracts.DC_Accommodation_HotelUpdates HU)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_HotelUpdates.Find(HU.Accommodation_HotelUpdates_Id);

                    if (search != null)
                    {
                        if ((HU.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = HU.IsActive;
                            search.Edit_Date = HU.Edit_Date;
                            search.Edit_User = HU.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = HU.Accommodation_Id;
                            search.Edit_Date = HU.Edit_Date;
                            search.Edit_User = HU.Edit_User;
                            search.Description = HU.Description;
                            search.FromDate = HU.FromDate;
                            search.Source = HU.Source;
                            search.ToDate = HU.ToDate;
                            search.IsActive = HU.IsActive;
                            search.IsInternal = HU.IsInternal;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation updates", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation Media
        public List<DataContracts.DC_Accommodation_Media> GetAccomodationMedia(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from am in context.Accommodation_Media
                                 where am.Accommodation_Id == Accomodation_Id
                                 && am.Accommodation_Media_Id == (DataKey_Id == Guid.Empty ? am.Accommodation_Media_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_Media
                                 {
                                     Accommodation_Id = am.Accommodation_Id,
                                     Accommodation_Media_Id = am.Accommodation_Media_Id,
                                     Create_Date = am.Create_Date,
                                     Create_User = am.Create_User,
                                     Edit_Date = am.Edit_Date,
                                     Edit_User = am.Edit_User,
                                     IsActive = (am.IsActive ?? true),
                                     Legacy_Htl_Id = am.Legacy_Htl_Id,
                                     MediaName = am.MediaName,
                                     MediaType = am.MediaType,
                                     Media_Path = am.Media_Path,
                                     Media_Position = am.Media_Position,
                                     Media_URL = am.Media_URL,
                                     RoomCategory = am.RoomCategory,
                                     ValidFrom = am.ValidFrom,
                                     ValidTo = am.ValidTo,
                                     Category = am.Category,
                                     Description = am.Description,
                                     FileFormat = am.FileFormat,
                                     MediaID = am.MediaID,
                                     SubCategory = am.SubCategory,
                                     MediaFileMaster = am.MediaFileMaster
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation media", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (AM.Accommodation_Id == null)
                    {
                        return false;
                    }
                    var maxMediaPosition = (context.Accommodation_Media.Where(x => x.Accommodation_Id == AM.Accommodation_Id).Max(a => a.Media_Position));
                    var maxMediaId = (context.Accommodation_Media.Where(x => x.Accommodation_Id == AM.Accommodation_Id).Max(a => a.MediaID));

                    int intmaxMediaID = 0;
                    if (maxMediaId != null)
                        intmaxMediaID = int.Parse(Convert.ToString(maxMediaId));
                    intmaxMediaID = intmaxMediaID + 1;

                    Accommodation_Media objNew = new Accommodation_Media();

                    if (AM.Accommodation_Media_Id == null)
                    {
                        AM.Accommodation_Media_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Media_Id = AM.Accommodation_Media_Id;
                    objNew.Accommodation_Id = AM.Accommodation_Id;
                    objNew.Create_Date = AM.Create_Date;
                    objNew.Create_User = AM.Create_User;
                    objNew.IsActive = AM.IsActive;
                    objNew.Legacy_Htl_Id = AM.Legacy_Htl_Id;
                    objNew.MediaName = AM.MediaName;
                    objNew.MediaType = AM.MediaType;
                    objNew.Media_Path = AM.Media_Path;
                    objNew.Media_Position = AM.Media_Position;

                    objNew.Media_URL = AM.Media_URL;
                    objNew.RoomCategory = AM.RoomCategory;
                    objNew.ValidFrom = AM.ValidFrom;
                    objNew.ValidTo = AM.ValidTo;
                    objNew.Category = AM.Category;
                    objNew.Description = AM.Description;
                    objNew.FileFormat = AM.FileFormat;
                    objNew.MediaID = Convert.ToString(intmaxMediaID);
                    //  objNew.MediaID = AM.MediaID;
                    objNew.SubCategory = AM.SubCategory;
                    objNew.MediaFileMaster = AM.MediaFileMaster;

                    context.Accommodation_Media.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation media", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationMedia(DataContracts.DC_Accommodation_Media AM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_Media.Find(AM.Accommodation_Media_Id);

                    if (search != null)
                    {
                        if ((AM.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = AM.IsActive;
                            search.Edit_Date = AM.Edit_Date;
                            search.Edit_User = AM.Edit_User;
                        }
                        else
                        {
                            search.Edit_Date = AM.Edit_Date;
                            search.Edit_User = AM.Edit_User;
                            search.MediaType = AM.MediaType;
                            search.Media_Position = AM.Media_Position;
                            search.RoomCategory = AM.RoomCategory;
                            search.ValidFrom = AM.ValidFrom;
                            search.ValidTo = AM.ValidTo;
                            search.Category = AM.Category;
                            search.Description = AM.Description;
                            search.SubCategory = AM.SubCategory;
                            search.MediaFileMaster = AM.MediaFileMaster;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation near by places", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool CheckMediaPositionDuplicateforAccommodation(Guid Accommodation_Id, int MediaPosition, Guid Accommodation_Media_Id)
        {
            bool IsDuplicate = false;
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var search = (from x in context.Accommodation_Media
                              where x.Accommodation_Id == Accommodation_Id
                                    && x.Media_Position == MediaPosition
                              select x);
                if (Accommodation_Media_Id != Guid.Empty)
                {
                    search = from x in search where x.Accommodation_Media_Id != Accommodation_Media_Id select x;
                }
                var result = (from x in search select x.Accommodation_Id).ToList();
                if (result != null && result.Count() > 0)
                    IsDuplicate = true;
            }
            return IsDuplicate;
        }
        #endregion

        #region Accomodation Media Attributes
        public List<DataContracts.DC_Accomodation_Media_Attributes> GetAccomodationMediaAttributes(Guid Accomodation_Media_Id, Guid DataKey_Id, int PageNo, int PageSize)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    int total = (from AM in context.Accommodation_Media
                                 join AMA in context.Media_Attributes on AM.Accommodation_Media_Id equals AMA.Media_Id
                                 where AM.Accommodation_Media_Id == Accomodation_Media_Id
                                 && AMA.Media_Attributes_Id == (DataKey_Id == Guid.Empty ? AMA.Media_Attributes_Id : DataKey_Id)
                                 select AMA).Count();

                    int skip = PageNo * PageSize;

                    var search = (from AM in context.Accommodation_Media
                                  join AMA in context.Media_Attributes on AM.Accommodation_Media_Id equals AMA.Media_Id
                                  where AM.Accommodation_Media_Id == Accomodation_Media_Id
                                  && AMA.Media_Attributes_Id == (DataKey_Id == Guid.Empty ? AMA.Media_Attributes_Id : DataKey_Id)
                                  orderby AMA.AttributeType
                                  select new DataContracts.DC_Accomodation_Media_Attributes
                                  {
                                      Accomodation_Media_Attributes_Id = AMA.Media_Attributes_Id,
                                      Accomodation_Media_Id = AMA.Media_Id,
                                      AttributeType = AMA.AttributeType,
                                      AttributeValue = AMA.AttributeValue,
                                      Create_Date = AMA.Create_Date,
                                      Create_User = AMA.Create_User,
                                      Edit_Date = AMA.Edit_Date,
                                      Edit_User = AMA.Edit_User,
                                      IsActive = ((AMA.IsActive ?? true) && (AM.IsActive ?? true)),
                                      IsMediaActive = (AM.IsActive ?? true),
                                      Status = AMA.Status,
                                      TotalRecords = total,
                                      IsSystemAttribute = (AMA.IsSystemAttribute ?? true)
                                  }).Skip(skip).Take(PageSize);

                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation media attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Media_Attributes objNew = new Media_Attributes();

                    if (AM.Accomodation_Media_Attributes_Id == null)
                    {
                        AM.Accomodation_Media_Attributes_Id = Guid.NewGuid();
                    }

                    objNew.Media_Attributes_Id = AM.Accomodation_Media_Attributes_Id;
                    objNew.Media_Id = AM.Accomodation_Media_Id;
                    objNew.AttributeType = AM.AttributeType;
                    objNew.AttributeValue = AM.AttributeValue;
                    objNew.Create_Date = AM.Create_Date;
                    objNew.Create_User = AM.Create_User;
                    objNew.IsActive = AM.IsActive;
                    objNew.IsSystemAttribute = AM.IsSystemAttribute;

                    context.Media_Attributes.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation media attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationMediaAttributes(DataContracts.DC_Accomodation_Media_Attributes AM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Media_Attributes.Find(AM.Accomodation_Media_Attributes_Id);

                    if (search != null)
                    {
                        if ((AM.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = AM.IsActive;
                            search.Edit_Date = AM.Edit_Date;
                            search.Edit_User = AM.Edit_User;
                        }
                        else
                        {
                            search.AttributeType = AM.AttributeType;
                            search.AttributeValue = AM.AttributeValue;
                            search.Edit_Date = AM.Edit_Date;
                            search.Edit_User = AM.Edit_User;
                            search.IsActive = AM.IsActive;
                        }
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation media attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation NearByPlaces
        public List<DataContracts.DC_Accommodation_NearbyPlaces> GetAccomodationNearbyPlaces(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from an in context.Accommodation_NearbyPlaces
                                 where an.Accomodation_Id == Accomodation_Id
                                 && an.Accommodation_NearbyPlace_Id == (DataKey_Id == Guid.Empty ? an.Accommodation_NearbyPlace_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_NearbyPlaces
                                 {
                                     Accommodation_NearbyPlace_Id = an.Accommodation_NearbyPlace_Id,
                                     Accomodation_Id = an.Accomodation_Id,
                                     Create_Date = an.Create_Date,
                                     Create_User = an.Create_User,
                                     Description = an.Description,
                                     DistanceFromProperty = an.DistanceFromProperty,
                                     DistanceUnit = an.DistanceUnit,
                                     Edit_Date = an.Edit_Date,
                                     Edit_User = an.Edit_User,
                                     Legacy_Htl_Id = an.Legacy_Htl_Id,
                                     PlaceCategory = an.PlaceCategory,
                                     PlaceName = an.PlaceName,
                                     PlaceType = an.PlaceType,
                                     IsActive = (an.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation nearby places", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (NP.Accomodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_NearbyPlaces objNew = new Accommodation_NearbyPlaces();

                    if (NP.Accommodation_NearbyPlace_Id == null)
                    {
                        NP.Accommodation_NearbyPlace_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_NearbyPlace_Id = NP.Accommodation_NearbyPlace_Id;
                    objNew.Accomodation_Id = NP.Accomodation_Id;
                    objNew.Create_Date = NP.Create_Date;
                    objNew.Create_User = NP.Create_User;
                    objNew.Description = NP.Description;
                    objNew.DistanceFromProperty = NP.DistanceFromProperty;
                    objNew.DistanceUnit = NP.DistanceUnit;
                    objNew.Legacy_Htl_Id = NP.Legacy_Htl_Id;
                    objNew.PlaceCategory = NP.PlaceCategory;
                    objNew.PlaceName = NP.PlaceName;
                    objNew.PlaceType = NP.PlaceType;
                    objNew.IsActive = NP.IsActive;

                    context.Accommodation_NearbyPlaces.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation near by places", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationNearbyPlaces(DataContracts.DC_Accommodation_NearbyPlaces NP)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_NearbyPlaces.Find(NP.Accommodation_NearbyPlace_Id);

                    if (search != null)
                    {
                        if ((NP.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = NP.IsActive;
                            search.Edit_Date = NP.Edit_Date;
                            search.Edit_User = NP.Edit_User;
                        }
                        else
                        {
                            search.Accomodation_Id = NP.Accomodation_Id;
                            search.Description = NP.Description;
                            search.DistanceFromProperty = NP.DistanceFromProperty;
                            search.DistanceUnit = NP.DistanceUnit;
                            search.Edit_Date = NP.Edit_Date;
                            search.Edit_User = NP.Edit_User;
                            search.Legacy_Htl_Id = NP.Legacy_Htl_Id;
                            search.PlaceCategory = NP.PlaceCategory;
                            search.PlaceName = NP.PlaceName;
                            search.PlaceType = NP.PlaceType;
                            search.IsActive = NP.IsActive;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation near by places", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation PaxOccupancy
        public List<DataContracts.DC_Accommodation_PaxOccupancy> GetAccomodationPaxOccupancy(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ap in context.Accommodation_PaxOccupancy
                                 join ri in context.Accommodation_RoomInfo on ap.Accommodation_RoomInfo_Id equals ri.Accommodation_RoomInfo_Id
                                 where ap.Accommodation_Id == Accomodation_Id
                                 && ap.Accommodation_PaxOccupancy_Id == (DataKey_Id == Guid.Empty ? ap.Accommodation_PaxOccupancy_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_PaxOccupancy
                                 {
                                     Accommodation_Id = ap.Accommodation_Id,
                                     Accommodation_PaxOccupancy_Id = ap.Accommodation_PaxOccupancy_Id,
                                     Accommodation_RoomInfo_Id = ap.Accommodation_RoomInfo_Id,
                                     Category = ap.category,
                                     CompanyName = ap.companyName,
                                     Create_Date = ap.Create_Date,
                                     Create_User = ap.Create_User,
                                     Edit_Date = ap.Edit_Date,
                                     Edit_User = ap.Edit_User,
                                     FromAgeForCIOR = ap.FromAgeForCIOR,
                                     FromAgeForCNB = ap.FromAgeForCNB,
                                     FromAgeForExtraBed = ap.FromAgeForExtraBed,
                                     Legacy_Htl_Id = ap.Legacy_Htl_Id,
                                     MaxAdults = ap.MaxAdults,
                                     MaxChild = ap.MaxChild,
                                     MaxCNB = ap.MaxCNB,
                                     MaxPax = ap.MaxPax,
                                     MaxPaxWithExtraBed = ap.MaxPaxWithExtraBed,
                                     RoomDisplayNumber = ap.RoomDisplayNumber,
                                     RoomType = ap.RoomType,
                                     ToAgeForCIOR = ap.ToAgeForCIOR,
                                     ToAgeForCNB = ap.ToAgeForCNB,
                                     ToAgeForExtraBed = ap.ToAgeForExtraBed,
                                     IsActive = ((ap.IsActive ?? true) && (ri.IsActive ?? true)),
                                     IsRoomActive = (ri.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation nearby areas", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public bool AddAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (PO.Accommodation_Id == null || PO.Accommodation_RoomInfo_Id == null)
                    {
                        return false;
                    }

                    Accommodation_PaxOccupancy objNew = new Accommodation_PaxOccupancy();

                    if (PO.Accommodation_PaxOccupancy_Id == null)
                    {
                        PO.Accommodation_PaxOccupancy_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Id = PO.Accommodation_Id;
                    objNew.Accommodation_PaxOccupancy_Id = PO.Accommodation_PaxOccupancy_Id;
                    objNew.Accommodation_RoomInfo_Id = PO.Accommodation_RoomInfo_Id;
                    objNew.Create_Date = PO.Create_Date;
                    objNew.Create_User = PO.Create_User;
                    objNew.Legacy_Htl_Id = PO.Legacy_Htl_Id;
                    objNew.category = PO.Category;
                    objNew.companyName = PO.CompanyName;
                    objNew.FromAgeForCIOR = PO.FromAgeForCIOR;
                    objNew.FromAgeForCNB = PO.FromAgeForCNB;
                    objNew.FromAgeForExtraBed = PO.FromAgeForExtraBed;
                    objNew.MaxAdults = PO.MaxAdults;
                    objNew.MaxChild = PO.MaxChild;
                    objNew.MaxCNB = PO.MaxCNB;
                    objNew.MaxPax = PO.MaxPax;
                    objNew.MaxPaxWithExtraBed = PO.MaxPaxWithExtraBed;
                    objNew.RoomDisplayNumber = PO.RoomDisplayNumber;
                    objNew.RoomType = PO.RoomType;
                    objNew.ToAgeForCIOR = PO.ToAgeForCIOR;
                    objNew.ToAgeForCNB = PO.ToAgeForCNB;
                    objNew.ToAgeForExtraBed = PO.ToAgeForExtraBed;
                    objNew.IsActive = PO.IsActive;

                    context.Accommodation_PaxOccupancy.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation pax occupancy", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationPaxOccupancy(DataContracts.DC_Accommodation_PaxOccupancy PO)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_PaxOccupancy.Find(PO.Accommodation_PaxOccupancy_Id);

                    if (search != null)
                    {
                        if ((PO.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = PO.IsActive;
                            search.Edit_Date = PO.Edit_Date;
                            search.Edit_User = PO.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = PO.Accommodation_Id;
                            search.Accommodation_RoomInfo_Id = PO.Accommodation_RoomInfo_Id;
                            search.Edit_Date = PO.Edit_Date;
                            search.Edit_User = PO.Edit_User;
                            search.Legacy_Htl_Id = PO.Legacy_Htl_Id;
                            search.category = PO.Category;
                            search.companyName = PO.CompanyName;
                            search.FromAgeForCIOR = PO.FromAgeForCIOR;
                            search.FromAgeForCNB = PO.FromAgeForCNB;
                            search.FromAgeForExtraBed = PO.FromAgeForExtraBed;
                            search.MaxAdults = PO.MaxAdults;
                            search.MaxChild = PO.MaxChild;
                            search.MaxCNB = PO.MaxCNB;
                            search.MaxPax = PO.MaxPax;
                            search.MaxPaxWithExtraBed = PO.MaxPaxWithExtraBed;
                            search.RoomDisplayNumber = PO.RoomDisplayNumber;
                            search.RoomType = PO.RoomType;
                            search.ToAgeForCIOR = PO.ToAgeForCIOR;
                            search.ToAgeForCNB = PO.ToAgeForCNB;
                            search.ToAgeForExtraBed = PO.ToAgeForExtraBed;
                            search.IsActive = PO.IsActive;
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation RoomInfo
        public List<DataContracts.DC_Accommodation_RoomInfo> GetAccomodationRoomInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ar in context.Accommodation_RoomInfo
                                 where ar.Accommodation_Id == Accomodation_Id
                                 && ar.Accommodation_RoomInfo_Id == (DataKey_Id == Guid.Empty ? ar.Accommodation_RoomInfo_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_RoomInfo
                                 {
                                     Accommodation_Id = ar.Accommodation_Id,
                                     Accommodation_RoomInfo_Id = ar.Accommodation_RoomInfo_Id,
                                     AmenityTypes = ar.AmenityTypes,
                                     BathRoomType = ar.BathRoomType,
                                     BedType = ar.BedType,
                                     Category = ar.Category,
                                     CompanyName = ar.CompanyName,
                                     CompanyRoomCategory = ar.CompanyRoomCategory,
                                     Create_Date = ar.Create_Date,
                                     Create_User = ar.Create_User,
                                     Description = ar.Description,
                                     Edit_Date = ar.Edit_Date,
                                     Edit_User = ar.Edit_User,
                                     FloorName = ar.FloorName,
                                     FloorNumber = ar.FloorNumber,
                                     Legacy_Htl_Id = ar.Legacy_Htl_Id,
                                     MysteryRoom = ar.MysteryRoom,
                                     NoOfInterconnectingRooms = ar.NoOfInterconnectingRooms,
                                     NoOfRooms = ar.NoOfRooms,
                                     RoomCategory = ar.RoomCategory,
                                     RoomDecor = ar.RoomDecor,
                                     RoomId = ar.RoomId,
                                     RoomName = ar.RoomName,
                                     RoomSize = ar.RoomSize,
                                     RoomView = ar.RoomView,
                                     Smoking = ar.Smoking,
                                     IsActive = (ar.IsActive ?? true),
                                     RoomFacilities = (from rf in context.Accommodation_RoomFacility
                                                       where rf.Accommodation_Id == ar.Accommodation_Id
                                                       && rf.Accommodation_RoomInfo_Id == ar.Accommodation_RoomInfo_Id
                                                       select new DataContracts.DC_Accomodation_RoomFacilities
                                                       {
                                                           Accommodation_Id = rf.Accommodation_Id,
                                                           Accommodation_RoomFacility_Id = rf.Accommodation_RoomFacility_Id,
                                                           Accommodation_RoomInfo_Id = rf.Accommodation_RoomInfo_Id,
                                                           AmenityName = rf.AmenityName,
                                                           AmenityType = rf.AmenityType,
                                                           Create_Date = rf.Create_Date,
                                                           Create_User = rf.Create_User,
                                                           Description = rf.Description,
                                                           Edit_Date = rf.Edit_Date,
                                                           Edit_user = rf.Edit_user
                                                       }).ToList()
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.DC_Accommodation_RoomInfo> GetRoomDetailsByWithPagging(DC_Accommodation_RoomInfo_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ar in context.Accommodation_RoomInfo
                                  where ar.Accommodation_Id == RQ.Accommodation_Id
                                  select ar);

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);
                    var result = (from ar in search
                                  select new DataContracts.DC_Accommodation_RoomInfo
                                  {
                                      Accommodation_Id = ar.Accommodation_Id,
                                      Accommodation_RoomInfo_Id = ar.Accommodation_RoomInfo_Id,
                                      AmenityTypes = ar.AmenityTypes,
                                      BathRoomType = ar.BathRoomType,
                                      BedType = ar.BedType,
                                      Category = ar.Category,
                                      CompanyName = ar.CompanyName,
                                      CompanyRoomCategory = ar.CompanyRoomCategory,
                                      Create_Date = ar.Create_Date,
                                      Create_User = ar.Create_User,
                                      Description = ar.Description,
                                      Edit_Date = ar.Edit_Date,
                                      Edit_User = ar.Edit_User,
                                      FloorName = ar.FloorName,
                                      FloorNumber = ar.FloorNumber,
                                      Legacy_Htl_Id = ar.Legacy_Htl_Id,
                                      MysteryRoom = ar.MysteryRoom,
                                      NoOfInterconnectingRooms = ar.NoOfInterconnectingRooms,
                                      NoOfRooms = ar.NoOfRooms,
                                      RoomCategory = ar.RoomCategory,
                                      RoomDecor = ar.RoomDecor,
                                      RoomId = ar.RoomId,
                                      RoomName = ar.RoomName,
                                      RoomSize = ar.RoomSize,
                                      RoomView = ar.RoomView,
                                      Smoking = ar.Smoking,
                                      IsActive = (ar.IsActive ?? true),
                                      TotalRecords = total,
                                      RoomFacilities = (from rf in context.Accommodation_RoomFacility
                                                        where rf.Accommodation_Id == ar.Accommodation_Id
                                                        && rf.Accommodation_RoomInfo_Id == ar.Accommodation_RoomInfo_Id
                                                        select new DataContracts.DC_Accomodation_RoomFacilities
                                                        {
                                                            Accommodation_Id = rf.Accommodation_Id,
                                                            Accommodation_RoomFacility_Id = rf.Accommodation_RoomFacility_Id,
                                                            Accommodation_RoomInfo_Id = rf.Accommodation_RoomInfo_Id,
                                                            AmenityName = rf.AmenityName,
                                                            AmenityType = rf.AmenityType,
                                                            Create_Date = rf.Create_Date,
                                                            Create_User = rf.Create_User,
                                                            Description = rf.Description,
                                                            Edit_Date = rf.Edit_Date,
                                                            Edit_user = rf.Edit_user
                                                        }).ToList()
                                  });
                    return result.OrderBy(p => p.RoomName).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.DC_Accomodation_Category_DDL> GetAccomodationRoomInfo_RoomCategory(Guid Accomodation_Id)
        {
            using (ConsumerEntities context = new ConsumerEntities())
            {
                var search = (from ar in context.Accommodation_RoomInfo
                              where ar.Accommodation_Id == Accomodation_Id
                              select new DataContracts.DC_Accomodation_Category_DDL
                              {
                                  Accommodation_RoomInfo_Id = ar.Accommodation_RoomInfo_Id,
                                  RoomCategory = ar.RoomCategory
                              }).ToList();
                return search;
            }
        }
        public List<string> GetRoomCategoryMaster(DataContracts.DC_RoomCategoryMaster_RQ RC)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var RoomCategory = from a in context.Accommodation_RoomInfo
                                       select a;
                    if (!string.IsNullOrWhiteSpace(RC.RoomCategory))
                    {
                        RoomCategory = from a in RoomCategory
                                       where a.RoomCategory.Contains(RC.RoomCategory)
                                       select a;
                    }
                    var acco = (from a in RoomCategory
                                select a.RoomCategory).Distinct().ToList();
                    return acco;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Room Category list", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public string GetNextRoomIdNumber(string codePrefix)
        {
            string ret = "000";

            if (!string.IsNullOrWhiteSpace(codePrefix))
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var code = (from a in context.Accommodation_RoomInfo
                                where a.RoomId.ToString().ToUpper().StartsWith(codePrefix.ToString().ToUpper() + "-")
                                orderby a.RoomId descending
                                //select CommonFunctions.ReturnNumbersFromString(a.Code)).FirstOrDefault();
                                select a.RoomId).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        code = code.Replace(codePrefix + "-", "");
                        Match m = Regex.Match(code, @"\d+");
                        int number = Convert.ToInt32(m.Value);
                        number = number + 1;
                        ret = CommonFunctions.NumberTo3CharString(number);
                    }
                }
            }

            return ret;
        }

        public bool AddAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RI.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_RoomInfo objNew = new Accommodation_RoomInfo();

                    if (RI.Accommodation_RoomInfo_Id == null)
                    {
                        RI.Accommodation_RoomInfo_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_RoomInfo_Id = RI.Accommodation_RoomInfo_Id;
                    objNew.Accommodation_Id = RI.Accommodation_Id;
                    objNew.Create_Date = RI.Create_Date;
                    objNew.Create_User = RI.Create_User;
                    objNew.Description = RI.Description;
                    objNew.AmenityTypes = RI.AmenityTypes;
                    objNew.BathRoomType = RI.BathRoomType;
                    objNew.BedType = RI.BedType;
                    objNew.Category = RI.Category;
                    objNew.CompanyName = RI.CompanyName;
                    objNew.CompanyRoomCategory = RI.CompanyRoomCategory;
                    objNew.FloorName = RI.FloorName;
                    objNew.FloorNumber = RI.FloorNumber;
                    objNew.MysteryRoom = RI.MysteryRoom;
                    objNew.NoOfInterconnectingRooms = RI.NoOfInterconnectingRooms;
                    objNew.NoOfRooms = RI.NoOfRooms;
                    objNew.RoomCategory = RI.RoomCategory;
                    objNew.RoomDecor = RI.RoomDecor;
                    objNew.RoomId = CommonFunctions.GenerateRoomId(Guid.Parse(RI.Accommodation_Id.ToString())); //RI.RoomId;
                    objNew.RoomName = RI.RoomName;
                    objNew.RoomSize = RI.RoomSize;
                    objNew.RoomView = RI.RoomView;
                    objNew.Smoking = RI.Smoking;
                    objNew.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                    objNew.IsActive = RI.IsActive;

                    context.Accommodation_RoomInfo.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationRoomInfo(DataContracts.DC_Accommodation_RoomInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_RoomInfo.Find(RI.Accommodation_RoomInfo_Id);

                    if (search != null)
                    {
                        if ((RI.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = RI.IsActive;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = RI.Accommodation_Id;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                            search.Description = RI.Description;
                            search.AmenityTypes = RI.AmenityTypes;
                            search.BathRoomType = RI.BathRoomType;
                            search.BedType = RI.BedType;
                            search.Category = RI.Category;
                            search.CompanyName = RI.CompanyName;
                            search.CompanyRoomCategory = RI.CompanyRoomCategory;
                            search.FloorName = RI.FloorName;
                            search.FloorNumber = RI.FloorNumber;
                            search.MysteryRoom = RI.MysteryRoom;
                            search.NoOfInterconnectingRooms = RI.NoOfInterconnectingRooms;
                            search.NoOfRooms = RI.NoOfRooms;
                            search.RoomCategory = RI.RoomCategory;
                            search.RoomDecor = RI.RoomDecor;
                            //search.RoomId = RI.RoomId;
                            search.RoomName = RI.RoomName;
                            search.RoomSize = RI.RoomSize;
                            search.RoomView = RI.RoomView;
                            search.Smoking = RI.Smoking;
                            search.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                            search.IsActive = RI.IsActive;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message CopyAccomodationInfo(DataContracts.DC_Accomodation_CopyRoomDef RQ)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                //check Duplicate 
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var isduplicate = context.Accommodation_RoomInfo.Where(x => x.Accommodation_Id == RQ.Accommodation_Id && x.RoomCategory.ToLower().Trim() == RQ.NewRoomCategory.ToLower().Trim()).Count() == 0 ? false : true;
                    if (isduplicate)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    }
                    else
                    {
                        //1. Get RoomInfo record to get copied
                        var roominfo = context.Accommodation_RoomInfo.Where(x => x.Accommodation_Id == RQ.Accommodation_Id && x.Accommodation_RoomInfo_Id == RQ.Accommodation_RoomInfo_Id).FirstOrDefault();

                        var newAccommodation_RoomInfo_Id = Guid.NewGuid();
                        //2. Insert the new one
                        if (roominfo != null)
                        {
                            Accommodation_RoomInfo objNew = new Accommodation_RoomInfo();

                            objNew.Accommodation_RoomInfo_Id = newAccommodation_RoomInfo_Id;
                            objNew.Accommodation_Id = RQ.Accommodation_Id;
                            objNew.Create_Date = DateTime.Now;
                            objNew.Create_User = RQ.Create_User;
                            objNew.Description = roominfo.Description;
                            objNew.AmenityTypes = roominfo.AmenityTypes;
                            objNew.BathRoomType = roominfo.BathRoomType;
                            objNew.BedType = roominfo.BedType;
                            objNew.Category = roominfo.Category;
                            objNew.CompanyRoomCategory = roominfo.CompanyRoomCategory;
                            objNew.RoomCategory = RQ.NewRoomCategory;
                            objNew.RoomName = roominfo.RoomName;
                            objNew.CompanyName = roominfo.CompanyName;
                            objNew.FloorName = roominfo.FloorName;
                            objNew.FloorNumber = roominfo.FloorNumber;
                            objNew.MysteryRoom = roominfo.MysteryRoom;
                            objNew.NoOfInterconnectingRooms = roominfo.NoOfInterconnectingRooms;
                            objNew.NoOfRooms = roominfo.NoOfRooms;
                            objNew.RoomDecor = roominfo.RoomDecor;
                            objNew.RoomId = CommonFunctions.GenerateRoomId(RQ.Accommodation_Id);
                            objNew.RoomSize = roominfo.RoomSize;
                            objNew.RoomView = roominfo.RoomView;
                            objNew.Smoking = roominfo.Smoking;
                            objNew.Legacy_Htl_Id = roominfo.Legacy_Htl_Id;
                            objNew.IsActive = roominfo.IsActive;

                            context.Accommodation_RoomInfo.Add(objNew);


                            //3. Get Accommodation_RoomFacility for the roominfo
                            var rf = context.Accommodation_RoomFacility.Where(x => x.Accommodation_Id == RQ.Accommodation_Id && x.Accommodation_RoomInfo_Id == RQ.Accommodation_RoomInfo_Id).ToList();

                            //4. Insert 
                            if (rf != null && rf.Count > 0)
                            {
                                foreach (var item in rf)
                                {
                                    Accommodation_RoomFacility objrf = new Accommodation_RoomFacility();
                                    objrf.Accommodation_Id = RQ.Accommodation_Id;
                                    objrf.Accommodation_RoomFacility_Id = Guid.NewGuid();
                                    objrf.Accommodation_RoomInfo_Id = newAccommodation_RoomInfo_Id;
                                    objrf.AmenityName = item.AmenityName;
                                    objrf.AmenityType = item.AmenityType;
                                    objrf.Create_Date = DateTime.Now;
                                    objrf.Create_User = RQ.Create_User;
                                    objrf.Description = item.Description;
                                    objrf.IsActive = item.IsActive;
                                    context.Accommodation_RoomFacility.Add(objrf);
                                }
                            }
                        }
                        context.SaveChanges();
                        _msg.StatusMessage = "Room Info has been copied successfully";
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return _msg;
        }
        #endregion

        #region Accomodation RoomFacilities
        public List<DataContracts.DC_Accomodation_RoomFacilities> GetAccomodationRoomFacilities(Guid Accomodation_Id, Guid Accommodation_RoomInfo_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from rf in context.Accommodation_RoomFacility
                                 join ri in context.Accommodation_RoomInfo on rf.Accommodation_RoomInfo_Id equals ri.Accommodation_RoomInfo_Id
                                 where rf.Accommodation_Id == Accomodation_Id
                                 && rf.Accommodation_RoomInfo_Id == Accommodation_RoomInfo_Id//(Accommodation_RoomInfo_Id == Guid.Empty ? rf.Accommodation_RoomInfo_Id : Accommodation_RoomInfo_Id)
                                 && rf.Accommodation_RoomFacility_Id == (DataKey_Id == Guid.Empty ? rf.Accommodation_RoomFacility_Id : DataKey_Id)
                                 select new DataContracts.DC_Accomodation_RoomFacilities
                                 {
                                     Accommodation_Id = rf.Accommodation_Id,
                                     Accommodation_RoomFacility_Id = rf.Accommodation_RoomFacility_Id,
                                     Accommodation_RoomInfo_Id = rf.Accommodation_RoomInfo_Id,
                                     AmenityName = rf.AmenityName,
                                     AmenityType = rf.AmenityType,
                                     Create_Date = rf.Create_Date,
                                     Create_User = rf.Create_User,
                                     Description = rf.Description,
                                     Edit_Date = rf.Edit_Date,
                                     Edit_user = rf.Edit_user,
                                     IsActive = ((rf.IsActive ?? true) && (ri.IsActive ?? true)),
                                     IsRoomActive = (ri.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation room facilities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RF.Accommodation_Id == null || RF.Accommodation_RoomInfo_Id == null)
                    {
                        return false;
                    }

                    Accommodation_RoomFacility objNew = new Accommodation_RoomFacility();

                    if (RF.Accommodation_RoomFacility_Id == null)
                    {
                        RF.Accommodation_RoomFacility_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Id = RF.Accommodation_Id;
                    objNew.Accommodation_RoomFacility_Id = RF.Accommodation_RoomFacility_Id;
                    objNew.Accommodation_RoomInfo_Id = RF.Accommodation_RoomInfo_Id;
                    objNew.AmenityName = RF.AmenityName;
                    objNew.AmenityType = RF.AmenityType;
                    objNew.Create_Date = RF.Create_Date;
                    objNew.Create_User = RF.Create_User;
                    objNew.Description = RF.Description;
                    objNew.IsActive = RF.IsActive;

                    context.Accommodation_RoomFacility.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation room facilities", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationRoomFacilities(DataContracts.DC_Accomodation_RoomFacilities RF)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_RoomFacility.Find(RF.Accommodation_RoomFacility_Id);

                    if (search != null)
                    {
                        if ((RF.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = RF.IsActive;
                            search.Edit_Date = RF.Edit_Date;
                            search.Edit_user = RF.Edit_user;
                        }
                        else
                        {
                            search.Accommodation_Id = RF.Accommodation_Id;
                            search.Accommodation_RoomInfo_Id = RF.Accommodation_RoomInfo_Id;
                            search.AmenityName = RF.AmenityName;
                            search.AmenityType = RF.AmenityType;
                            search.Edit_Date = RF.Edit_Date;
                            search.Edit_user = RF.Edit_user;
                            search.Description = RF.Description;
                            search.IsActive = RF.IsActive;
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation room info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation RouteInfo
        public List<DataContracts.DC_Accommodation_RouteInfo> GetAccomodationRouteInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ari in context.Accommodation_RouteInfo
                                 where ari.Accommodation_Id == Accomodation_Id
                                 && ari.Accommodation_Route_Id == (DataKey_Id == Guid.Empty ? ari.Accommodation_Route_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_RouteInfo
                                 {
                                     AccommodationCode = ari.AccommodationCode,
                                     Accommodation_Id = ari.Accommodation_Id,
                                     Accommodation_Route_Id = ari.Accommodation_Route_Id,
                                     ApproximateDuration = ari.ApproximateDuration,
                                     CompanyName = ari.CompanyName,
                                     Create_Date = ari.Create_Date,
                                     Create_User = ari.Create_User,
                                     Description = ari.Description,
                                     DistanceFromProperty = ari.DistanceFromProperty,
                                     DistanceUnit = ari.DistanceUnit,
                                     DrivingDirection = ari.DrivingDirection,
                                     Edit_Date = ari.Edit_Date,
                                     Edit_User = ari.Edit_User,
                                     FromPlace = ari.FromPlace,
                                     Legacy_Htl_Id = ari.Legacy_Htl_Id,
                                     ModeOfTransport = ari.ModeOfTransport,
                                     NameOfPlace = ari.NameOfPlace,
                                     TransportType = ari.TransportType,
                                     ValidFrom = ari.ValidFrom,
                                     ValidTo = ari.ValidTo,
                                     IsActive = (ari.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation route info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RI.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_RouteInfo objNew = new Accommodation_RouteInfo();

                    if (RI.Accommodation_Route_Id == null)
                    {
                        RI.Accommodation_Route_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Route_Id = RI.Accommodation_Route_Id;
                    objNew.Accommodation_Id = RI.Accommodation_Id;
                    objNew.AccommodationCode = RI.AccommodationCode;
                    objNew.ApproximateDuration = RI.ApproximateDuration;
                    objNew.CompanyName = RI.CompanyName;
                    objNew.Create_Date = RI.Create_Date;
                    objNew.Create_User = RI.Create_User;
                    objNew.Description = RI.Description;
                    objNew.DistanceFromProperty = RI.DistanceFromProperty;
                    objNew.DistanceUnit = RI.DistanceUnit;
                    objNew.DrivingDirection = RI.DrivingDirection;
                    objNew.FromPlace = RI.FromPlace;
                    objNew.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                    objNew.ModeOfTransport = RI.ModeOfTransport;
                    objNew.NameOfPlace = RI.NameOfPlace;
                    objNew.TransportType = RI.TransportType;
                    objNew.ValidFrom = RI.ValidFrom;
                    objNew.ValidTo = RI.ValidTo;
                    objNew.IsActive = RI.IsActive;

                    context.Accommodation_RouteInfo.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation route info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationRouteInfo(DataContracts.DC_Accommodation_RouteInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_RouteInfo.Find(RI.Accommodation_Route_Id);

                    if (search != null)
                    {
                        if ((RI.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = RI.IsActive;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = RI.Accommodation_Id;
                            search.AccommodationCode = RI.AccommodationCode;
                            search.ApproximateDuration = RI.ApproximateDuration;
                            search.CompanyName = RI.CompanyName;
                            search.Description = RI.Description;
                            search.DistanceFromProperty = RI.DistanceFromProperty;
                            search.DistanceUnit = RI.DistanceUnit;
                            search.DrivingDirection = RI.DrivingDirection;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                            search.FromPlace = RI.FromPlace;
                            search.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                            search.ModeOfTransport = RI.ModeOfTransport;
                            search.NameOfPlace = RI.NameOfPlace;
                            search.TransportType = RI.TransportType;
                            search.ValidFrom = RI.ValidFrom;
                            search.ValidTo = RI.ValidTo;
                            search.IsActive = RI.IsActive;
                        }
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation route info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation RuleInfo
        public List<DataContracts.DC_Accommodation_RuleInfo> GetAccomodationRuleInfo(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from arl in context.Accommodation_RuleInfo
                                 where arl.Accommodation_Id == Accomodation_Id
                                 && arl.Accommodation_RuleInfo_Id == (DataKey_Id == Guid.Empty ? arl.Accommodation_RuleInfo_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_RuleInfo
                                 {
                                     Accommodation_Id = arl.Accommodation_Id,
                                     Accommodation_RuleInfo_Id = arl.Accommodation_RuleInfo_Id,
                                     Create_Date = arl.Create_Date,
                                     Create_User = arl.Create_User,
                                     Description = arl.Description,
                                     Edit_Date = arl.Edit_Date,
                                     Edit_User = arl.Edit_User,
                                     Legacy_Htl_Id = arl.Legacy_Htl_Id,
                                     RuleType = arl.RuleType,
                                     IsActive = (arl.IsActive ?? true),
                                     IsInternal = (arl.IsInternal ?? false)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation rule info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RI.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_RuleInfo objNew = new Accommodation_RuleInfo();

                    if (RI.Accommodation_RuleInfo_Id == null)
                    {
                        RI.Accommodation_RuleInfo_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_RuleInfo_Id = RI.Accommodation_RuleInfo_Id;
                    objNew.Accommodation_Id = RI.Accommodation_Id;
                    objNew.Create_Date = RI.Create_Date;
                    objNew.Create_User = RI.Create_User;
                    objNew.Description = RI.Description;
                    objNew.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                    objNew.RuleType = RI.RuleType;
                    objNew.IsActive = RI.IsActive;
                    objNew.IsInternal = RI.IsInternal;

                    context.Accommodation_RuleInfo.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation rule info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationRuleInfo(DataContracts.DC_Accommodation_RuleInfo RI)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.Accommodation_RuleInfo.Find(RI.Accommodation_RuleInfo_Id);

                    if (search != null)
                    {
                        if ((RI.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = RI.IsActive;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = RI.Accommodation_Id;
                            search.Description = RI.Description;
                            search.Edit_Date = RI.Edit_Date;
                            search.Edit_User = RI.Edit_User;
                            search.Legacy_Htl_Id = RI.Legacy_Htl_Id;
                            search.RuleType = RI.RuleType;
                            search.IsActive = RI.IsActive;
                            search.IsInternal = RI.IsInternal;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation rule info", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation Status
        public List<DataContracts.DC_Accommodation_Status> GetAccomodationStatus(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ast in context.Accommodation_Status
                                 where ast.Accommodation_Id == Accomodation_Id
                                 && ast.Accommodation_Status_Id == (DataKey_Id == Guid.Empty ? ast.Accommodation_Status_Id : DataKey_Id)
                                 select new DataContracts.DC_Accommodation_Status
                                 {
                                     Accommodation_Id = ast.Accommodation_Id,
                                     Accommodation_Status_Id = ast.Accommodation_Status_Id,
                                     CompanyMarket = ast.CompanyMarket,
                                     DeactivationReason = ast.DeactivationReason,
                                     From = ast.From,
                                     Status = ast.Status,
                                     To = ast.To,
                                     IsActive = (ast.IsActive ?? true),
                                     Create_Date = ast.Create_Date,
                                     Create_User = ast.Create_User,
                                     Edit_Date = ast.Edit_Date,
                                     Edit_User = ast.Edit_User
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodation_Status
                                  where a.Accommodation_Status_Id == AS.Accommodation_Status_Id
                                  select a).First();
                    if (search != null)
                    {
                        if ((AS.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = AS.IsActive;
                            search.Edit_Date = AS.Edit_Date;
                            search.Edit_User = AS.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = AS.Accommodation_Id;
                            search.CompanyMarket = AS.CompanyMarket;
                            search.DeactivationReason = AS.DeactivationReason;
                            search.From = AS.From;
                            search.Status = AS.Status;
                            search.To = AS.To;
                            search.Edit_Date = AS.Edit_Date;
                            search.Edit_User = AS.Edit_User;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationStatus(DataContracts.DC_Accommodation_Status AS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (AS.Accommodation_Id == null)
                    {
                        return false;
                    }

                    Accommodation_Status objNew = new Accommodation_Status();

                    if (AS.Accommodation_Status_Id == null)
                    {
                        AS.Accommodation_Status_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_Status_Id = AS.Accommodation_Status_Id;
                    objNew.Accommodation_Id = AS.Accommodation_Id;
                    objNew.CompanyMarket = AS.CompanyMarket;
                    objNew.DeactivationReason = AS.DeactivationReason;
                    objNew.From = AS.From;
                    objNew.Status = AS.Status;
                    objNew.To = AS.To;
                    objNew.IsActive = AS.IsActive;
                    objNew.Create_Date = AS.Create_Date;
                    objNew.Create_User = AS.Create_User;

                    context.Accommodation_Status.Add(objNew);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Accomodation ClassificationAttributes
        public List<DataContracts.DC_Accomodation_ClassificationAttributes> GetAccomodationClassificationAttributes(Guid Accomodation_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from aca in context.Accommodation_ClassificationAttributes
                                 where aca.Accommodation_Id == Accomodation_Id
                                 && aca.Accommodation_ClassificationAttribute_Id == (DataKey_Id == Guid.Empty ? aca.Accommodation_ClassificationAttribute_Id : DataKey_Id)
                                 select new DataContracts.DC_Accomodation_ClassificationAttributes
                                 {
                                     Accommodation_ClassificationAttribute_Id = aca.Accommodation_ClassificationAttribute_Id,
                                     Accommodation_Id = aca.Accommodation_Id,
                                     AttributeSubType = aca.AttributeSubType,
                                     AttributeType = aca.AttributeType,
                                     AttributeValue = aca.AttributeValue,
                                     Create_Date = aca.Create_Date,
                                     Create_User = aca.Create_User,
                                     Edit_Date = aca.Edit_Date,
                                     Edit_User = aca.Edit_User,
                                     InternalOnly = aca.InternalOnly,
                                     IsActive = (aca.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching accomodation classification attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodation_ClassificationAttributes
                                  where a.Accommodation_ClassificationAttribute_Id == CA.Accommodation_ClassificationAttribute_Id
                                  select a).First();
                    if (search != null)
                    {
                        if ((CA.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = CA.IsActive;
                            search.Edit_Date = CA.Edit_Date;
                            search.Edit_User = CA.Edit_User;
                        }
                        else
                        {
                            search.Accommodation_Id = CA.Accommodation_Id;
                            search.AttributeSubType = CA.AttributeSubType;
                            search.AttributeType = CA.AttributeType;
                            search.AttributeValue = CA.AttributeValue;
                            search.Edit_Date = CA.Edit_Date;
                            search.Edit_User = CA.Edit_User;
                            search.InternalOnly = CA.InternalOnly;
                            search.IsActive = CA.IsActive;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation classification attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddAccomodationClassificationAttributes(DataContracts.DC_Accomodation_ClassificationAttributes CA)
        {
            try
            {
                if (CA.Accommodation_Id == null)
                {
                    return false;
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Accommodation_ClassificationAttributes objNew = new Accommodation_ClassificationAttributes();

                    if (CA.Accommodation_ClassificationAttribute_Id == null)
                    {
                        CA.Accommodation_ClassificationAttribute_Id = Guid.NewGuid();
                    }

                    objNew.Accommodation_ClassificationAttribute_Id = CA.Accommodation_ClassificationAttribute_Id;
                    objNew.Accommodation_Id = CA.Accommodation_Id;
                    objNew.AttributeSubType = CA.AttributeSubType;
                    objNew.AttributeType = CA.AttributeType;
                    objNew.AttributeValue = CA.AttributeValue;
                    objNew.Create_Date = CA.Create_Date;
                    objNew.Create_User = CA.Create_User;
                    objNew.InternalOnly = CA.InternalOnly;
                    objNew.IsActive = CA.IsActive;

                    context.Accommodation_ClassificationAttributes.Add(objNew);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation classification attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

    }
}
