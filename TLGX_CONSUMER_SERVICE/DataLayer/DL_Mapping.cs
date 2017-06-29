using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;

namespace DataLayer
{
    public class DL_Mapping : IDisposable
    {
        public void Dispose()
        {
        }

        #region Accomodation Product Mapping

        public bool ShiftAccommodationMappings(DataContracts.Mapping.DC_Mapping_ShiftMapping_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var AccoMappingIds = (from a in context.Accommodation_ProductMapping
                                          where a.Accommodation_Id == obj.Accommodation_From_Id
                                          select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                          {
                                              Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id
                                          });
                    foreach (DataContracts.Mapping.DC_Accomodation_ProductMapping AccoMappingId in AccoMappingIds)
                    {
                        using (ConsumerEntities context1 = new ConsumerEntities())
                        {
                            var curRecord = (from a in context1.Accommodation_ProductMapping
                                             where a.Accommodation_ProductMapping_Id == AccoMappingId.Accommodation_ProductMapping_Id
                                             select a).FirstOrDefault();
                            if (curRecord != null)
                            {
                                curRecord.Accommodation_Id = obj.Accommodation_To_Id;
                                curRecord.Edit_Date = obj.Edit_Date;
                                curRecord.Edit_User = obj.Edit_User;
                                curRecord.Remarks = obj.Remarks;
                                context1.SaveChanges();
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            return true;
        }


        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMappingById(Guid Accommodation_ProductMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping select a;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;

                    prodMapList = (from a in prodMapSearch
                                   join m in context.Accommodations on a.Accommodation_Id equals m.Accommodation_Id into j
                                   from jd in j.DefaultIfEmpty()
                                       //join ac in context.Accommodations on a.Accommodation_Id equals ac.Accommodation_Id
                                   where a.Accommodation_ProductMapping_Id == Accommodation_ProductMapping_Id
                                   orderby a.SupplierName, a.ProductName, a.SupplierProductReference
                                   select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                   {
                                       Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                       Accommodation_Id = a.Accommodation_Id,
                                       Supplier_Id = a.Supplier_Id,
                                       SupplierId = a.SupplierId,
                                       SupplierName = a.SupplierName,
                                       SupplierProductReference = a.SupplierProductReference,
                                       ProductName = a.ProductName,
                                       Street = a.Street,
                                       Street2 = a.Street2,
                                       Street3 = a.Street3,
                                       Street4 = a.Street4,
                                       CountryCode = a.CountryCode,
                                       CountryName = a.CountryName,
                                       CityCode = a.CityCode,
                                       CityName = a.CityName,
                                       StateCode = a.StateCode,
                                       StateName = a.StateName,
                                       PostCode = a.PostCode,
                                       TelephoneNumber = a.TelephoneNumber,
                                       Fax = a.Fax,
                                       Email = a.Email,
                                       Website = a.Website,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       Status = a.Status,
                                       Create_Date = a.Create_Date,
                                       Create_User = a.Create_User,
                                       Edit_Date = a.Edit_Date,
                                       Edit_User = a.Edit_User,
                                       IsActive = (a.IsActive ?? true),
                                       ProductId = a.SupplierProductReference,
                                       SystemCountryName = (jd.country ?? (context.m_CountryMaster.Where(x => x.Name == a.CountryName).Select(x => x.Name)).FirstOrDefault()),
                                       SystemCityName = (jd.city ?? (context.m_CityMaster.Where(x => x.Name == a.CityName && x.CountryName == a.CountryName).Select(x => x.Name)).FirstOrDefault()),
                                       SystemProductName = jd.HotelName,
                                       SystemStateName = (jd.State_Name ?? (context.m_States.Where(x => x.StateName == a.StateName).Select(x => x.StateName)).FirstOrDefault()),
                                       Remarks = a.Remarks,
                                       MapId = a.MapId,
                                       FullAddress = (a.address ?? string.Empty) + ", " + (a.Street ?? string.Empty) + ", " + (a.Street2 ?? string.Empty) + " " + (a.Street3 ?? string.Empty) + " " + (a.Street4 ?? string.Empty) + " " + (a.PostCode ?? string.Empty) + ", " + (a.CityName ?? string.Empty) + ", " + (a.StateName ?? string.Empty) + ", " + (a.CountryName ?? string.Empty)
                                   });

                    var result = prodMapList.ToList();

                    foreach (var item in result)
                    {
                        if (!string.IsNullOrWhiteSpace(item.ProductName))
                        {
                            var prodname = item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper();
                            var prodcode = item.ProductId;
                            if (!string.IsNullOrWhiteSpace(prodname) && prodname.ToUpper() != "&NBSP;")
                            {
                                var searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Equals(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                if (searchprod == null)
                                    searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Contains(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(prodcode) && prodname.ToUpper() != "&NBSP;")
                                {
                                    if (searchprod == null)
                                        searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).ToUpper().Equals(prodcode)).FirstOrDefault();
                                }
                                if (searchprod != null)
                                {
                                    item.SystemProductName = searchprod.HotelName;
                                    item.FullAddress = searchprod.FullAddress;
                                }
                            }
                        }

                    }
                    return result;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }



        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(int PageNo, int PageSize, Guid Accomodation_Id, string Status)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping select a;

                    if (Accomodation_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Accommodation_Id == Accomodation_Id
                                        select a;
                    }

                    if (Status.Trim().ToUpper() != "ALL")
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status ?? "UNMAPPED") == Status
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;

                    if (Accomodation_Id != Guid.Empty)
                    {
                        prodMapList = (from a in prodMapSearch
                                       join ac in context.Accommodations on a.Accommodation_Id equals ac.Accommodation_Id
                                       where ac.Accommodation_Id == Accomodation_Id
                                       orderby a.SupplierName, a.ProductName, a.SupplierProductReference
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           TotalRecords = total,
                                           ProductId = ac.CompanyHotelID.ToString(),
                                           SystemCountryName = ac.country,
                                           SystemCityName = ac.city,
                                           SystemProductName = ac.HotelName,
                                           Remarks = a.Remarks,
                                           MapId = a.MapId
                                       }).Skip(skip).Take(PageSize);
                    }
                    else
                    {

                        prodMapList = (from a in prodMapSearch
                                       orderby a.SupplierName, a.SupplierProductReference
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           TotalRecords = total,
                                           ProductId = a.SupplierProductReference,
                                           MapId = a.MapId,
                                           Remarks = a.Remarks
                                       }).Skip(skip).Take(PageSize);
                    }

                    return prodMapList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping select a; //where a.Accommodation_Id == null 

                    if (!string.IsNullOrWhiteSpace(obj.SupplierId))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierId == obj.SupplierId
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierName == obj.SupplierName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street))
                    {
                        string address_tx = CommonFunctions.RemoveSpecialCharacters(obj.Street);
                        prodMapSearch = from a in prodMapSearch
                                        where a.address_tx == address_tx
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street2))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Street2.Contains(obj.Street2)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Street3))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Street3.Contains(obj.Street3)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.ProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ProductName.Contains(obj.ProductName)
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.PostCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.PostCode.Contains(obj.PostCode)
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.TelephoneNumber))
                    {
                        string telephoneNumber_tx = CommonFunctions.GetCharacter(obj.TelephoneNumber, 8);
                        prodMapSearch = from a in prodMapSearch
                                        where a.TelephoneNumber_tx == telephoneNumber_tx
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCountryMapping on a.CountryName equals ct.CountryName
                                            join mct in context.m_CountryMaster on ct.Country_Id equals mct.Country_Id
                                            where mct.Name == obj.CountryName
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CountryName == obj.CountryName
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(obj.CityName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCityMapping = (from a in context.m_CityMapping select new { a.City_Id, a.CityName }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCityMapping on a.CityName equals ct.CityName
                                            join mct in context.m_CityMaster on ct.City_Id equals mct.City_Id
                                            where mct.Name == obj.CityName
                                            select a;
                        }
                        else
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityName == obj.CityName
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(obj.CityCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityCode == obj.CityCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.CountryCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryCode == obj.CountryCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Status))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status ?? "UNMAPPED") == obj.Status
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Chain))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join m in context.Accommodations on a.Accommodation_Id equals m.Accommodation_Id
                                        where m.Chain == obj.Chain
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.Brand))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join m in context.Accommodations on a.Accommodation_Id equals m.Accommodation_Id
                                        where m.Brand == obj.Brand
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.CountryName.ToUpper().Contains(obj.SupplierCountryName.ToUpper()))
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName.Contains(obj.SupplierCityName)
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ProductName.Contains(obj.SupplierProductName)
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;

                    var prodMapList = (from a in prodMapSearch
                                       join ma in context.Accommodations on a.Accommodation_Id equals ma.Accommodation_Id into ja
                                       from jda in ja.DefaultIfEmpty()
                                       orderby a.SupplierName, a.ProductName, a.SupplierProductReference
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           PostCode = a.PostCode,
                                           TelephoneNumber = a.TelephoneNumber,
                                           Fax = a.Fax,
                                           Email = a.Email,
                                           Website = a.Website,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           Status = a.Status,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           IsActive = (a.IsActive ?? true),
                                           TotalRecords = total,
                                           ProductId = a.SupplierProductReference,
                                           Remarks = a.Remarks,
                                           SystemProductName = jda.HotelName,
                                           SystemCityName = (jda.city ?? (context.Accommodations.Where(x => x.city == a.CityName).Select(x => x.city)).FirstOrDefault()),
                                           SystemCountryName = (jda.country ?? (context.Accommodations.Where(x => x.country == a.CountryName).Select(x => x.country)).FirstOrDefault()),
                                           MapId = a.MapId,
                                           FullAddress = (a.address ?? string.Empty) + ", " + (a.Street ?? string.Empty) + ", " + (a.Street2 ?? string.Empty) + " " + (a.Street3 ?? string.Empty) + " " + (a.Street4 ?? string.Empty) + " " + (a.PostCode ?? string.Empty) + ", " + (a.CityName ?? string.Empty) + ", " + (a.StateName ?? string.Empty) + ", " + (a.CountryName ?? string.Empty),
                                           SystemFullAddress = (jda.FullAddress ?? string.Empty)

                                       }).Skip(skip).Take(obj.PageSize);

                    var result = prodMapList.ToList();
                    if (!string.IsNullOrWhiteSpace(obj.Status) && !string.IsNullOrWhiteSpace(obj.CityName) && obj.Status.ToLower().Trim() == "unmapped")
                    {
                        foreach (var item in result)
                        {
                            if (!string.IsNullOrWhiteSpace(item.ProductName))
                            {
                                var prodname = item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper();
                                var prodcode = item.ProductId;
                                if (!string.IsNullOrWhiteSpace(prodname) && prodname.ToUpper() != "&NBSP;")
                                {
                                    var searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Equals(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                    if (searchprod == null)
                                        searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Contains(item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper())).FirstOrDefault();
                                    if (!string.IsNullOrWhiteSpace(prodcode) && prodname.ToUpper() != "&NBSP;")
                                    {
                                        if (searchprod == null)
                                            searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).ToUpper().Equals(prodcode)).FirstOrDefault();
                                    }
                                    if (searchprod != null)
                                    {
                                        item.mstAcco_Id = Convert.ToString(searchprod.Accommodation_Id);
                                        item.mstHotelName = searchprod.HotelName;
                                        item.FullAddress = searchprod.FullAddress;
                                    }
                                }
                            }

                        }
                    }

                    return result;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationProductMapping(List<DataContracts.Mapping.DC_Accomodation_ProductMapping> obj)
        {
            foreach (var PM in obj)
            {
                if (PM.Accommodation_ProductMapping_Id == null || PM.Accommodation_Id == null) //|| PM.Supplier_Id == null
                {
                    continue;
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.Accommodation_ProductMapping
                                      where a.Accommodation_ProductMapping_Id == PM.Accommodation_ProductMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            //if (PM.IsActive != (search.IsActive ?? true))
                            //{
                            //    search.IsActive = PM.IsActive;
                            //    search.Edit_Date = PM.Edit_Date;
                            //    search.Edit_User = PM.Edit_User;
                            //}
                            //else 
                            //if (PM.Status != (search.Status ?? string.Empty))
                            //{
                            //    search.Accommodation_Id = PM.Accommodation_Id;
                            //    search.Status = PM.Status;
                            //    search.Edit_Date = PM.Edit_Date;
                            //    search.Edit_User = PM.Edit_User;
                            //    search.Remarks = PM.Remarks;
                            //}
                            //else
                            //{
                            search.Accommodation_Id = PM.Accommodation_Id;
                            if (PM.Supplier_Id != null)
                                search.Supplier_Id = PM.Supplier_Id;
                            search.Status = PM.Status;
                            search.IsActive = PM.IsActive;
                            search.Edit_Date = PM.Edit_Date;
                            search.Edit_User = PM.Edit_User;
                            search.Remarks = PM.Remarks;
                            //}

                            context.SaveChanges();
                        }


                    }
                }
                catch
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return true;
        }
        #endregion

        #region Supplier Room Type Mapping
        public IList<DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping> GetAccomodationSupplierRoomTypeMapping(int PageNo, int PageSize, Guid Accomodation_Id, Guid Supplier_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodSupMapSearch = from a in context.Accommodation_SupplierRoomTypeMapping
                                           where a.Accommodation_Id == Accomodation_Id
                                           select a;

                    if (Supplier_Id != Guid.Empty)
                    {
                        prodSupMapSearch = from a in prodSupMapSearch
                                           where a.Supplier_Id == Supplier_Id
                                           select a;
                    }

                    int total;

                    total = prodSupMapSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    if (!canPage)
                        return null;

                    var prodSupMapList = (from a in prodSupMapSearch
                                          orderby a.Accommodation_SupplierRoomTypeMapping_Id
                                          select new DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping
                                          {
                                              Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                              Supplier_Id = a.Supplier_Id,
                                              Accommodation_Id = a.Accommodation_Id,
                                              //Commented for table structure change 
                                              ////Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                              ////Amenities = a.Amenities,
                                              ////BathRoomType = a.BATHROOMTYPE,
                                              ////BedTypeCode = a.BEDTYPECODE,
                                              ////Description = a.ROOM_DESCRIPTION,
                                              ////FloorName = a.FLOOR_NAME,
                                              ////FloorNumber = a.FLOORNUMBER,
                                              ////MaxAdults = a.MAX_ADULTS,
                                              ////MaxChild = a.MAX_CHILD,
                                              ////MaxGuest = a.MAX_GUEST_OCCUPANCY,
                                              ////MaxInfant = a.MAX_INFANT,
                                              ////Quantity = a.QUANTITY,
                                              ////RoomLocationCode = a.RoomLocationCode,
                                              ////RoomTypeCode = a.ROOMTYPECODE,
                                              ////RoomViewCode = a.ROOMVIEWCODE,
                                              //SizeMeasurement = a.SizeMeasurement,
                                              //SupplierHotelCode = a.SupplierHotelCode,
                                              //SupplierHotelName = a.SupplierHotelName,
                                              SupplierName = a.SupplierName,
                                              //SupplierProvider = a.SupplierProvider,
                                              //SupplierRoomCategory = a.SupplierRoomCategory,
                                              //SupplierRoomId = a.SupplierRoomId,
                                              //SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                          }).Skip(skip).Take(PageSize);

                    return prodSupMapList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product supplier mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateAccomodationSupplierRoomTypeMapping(DataContracts.Mapping.DC_Accomodation_SupplierRoomTypeMapping obj)
        {
            if (obj.Accommodation_SupplierRoomTypeMapping_Id == null || obj.Accommodation_RoomInfo_Id == null)
            {
                return false;
            }

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Accommodation_SupplierRoomTypeMapping
                                  where a.Accommodation_SupplierRoomTypeMapping_Id == obj.Accommodation_SupplierRoomTypeMapping_Id
                                  select a).FirstOrDefault();
                    if (search != null)
                    {
                        //if (obj. != (search.IsActive ?? true))
                        //{
                        //    search.IsActive = PM.IsActive;
                        //    search.Edit_Date = PM.Edit_Date;
                        //    search.Edit_User = PM.Edit_User;
                        //}
                        //else if (PM.Status != (search.Status ?? string.Empty))
                        //{
                        //    search.Status = PM.Status;
                        //    search.Edit_Date = PM.Edit_Date;
                        //    search.Edit_User = PM.Edit_User;
                        //}
                        //else
                        //{
                        // search.Accommodation_RoomInfo_Id = obj.Accommodation_RoomInfo_Id;
                        //}
                    }
                    context.SaveChanges();
                }

                return true;
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while updating accomodation product supplier mapping",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region Country Mapping
        //public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(int PageNo, int PageSize, Guid Supplier_Id, Guid Country_Id, string sts, string SortBy)
        public List<DataContracts.Mapping.DC_CountryMapping> GetCountryMapping(DataContracts.Mapping.DC_CountryMappingRQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.m_CountryMapping select a;

                    if (RQ.Supplier_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == RQ.Supplier_Id
                                        select a;
                    }

                    //if (RQ.Country_Id != Guid.Empty)
                    //{
                    //    prodMapSearch = from a in prodMapSearch
                    //                    where a.Country_Id == RQ.Country_Id
                    //                    select a;
                    //}
                    if (!string.IsNullOrWhiteSpace(RQ.SystemCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryName.Contains(RQ.SystemCountryName)
                                        select a;
                    }

                    if (RQ.Status.ToUpper().IndexOf("ALL") == -1)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status == RQ.Status
                                        select a;
                    }

                    if (RQ.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != RQ.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CountryName.Contains(RQ.SupplierCountryName)
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    IQueryable<DataContracts.Mapping.DC_CountryMapping> prodMapList;

                    prodMapList = (from a in prodMapSearch
                                   join m in context.m_CountryMaster on a.Country_Id equals m.Country_Id into j
                                   from jd in j.DefaultIfEmpty()
                                       //orderby (SortBy)
                                   orderby a.CountryName
                                   select new DataContracts.Mapping.DC_CountryMapping
                                   {
                                       CountryMapping_Id = a.CountryMapping_Id,
                                       Country_Id = a.Country_Id,
                                       TotalRecord = total,
                                       Supplier_Id = a.Supplier_Id,
                                       CountryCode = a.CountryCode,
                                       CountryName = a.CountryName,
                                       Create_Date = a.Create_Date,
                                       Create_User = a.Create_User,
                                       Edit_Date = a.Edit_Date,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       Status = a.Status,
                                       SupplierName = a.SupplierName,
                                       Code = jd.Code,
                                       Name = jd.Name
                                   }).Skip(skip).Take(RQ.PageSize);
                    var result = prodMapList.ToList();

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {

                        foreach (var item in result)
                        {
                            if (RQ.Status.ToUpper().Trim() == "UNMAPPED" && string.IsNullOrWhiteSpace(item.Name))
                            {
                                if (item.CountryName != null && item.CountryCode != null)
                                {
                                    var countryName = item.CountryName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("country", "").ToUpper();
                                    var countryCode = item.CountryCode.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("country", "").ToUpper();
                                    var searchCountry = context.m_CountryMaster.Where(a => (a.Name.ToUpper().Trim() == countryName.ToUpper().Trim())).FirstOrDefault();
                                    if (searchCountry == null)
                                    {
                                        searchCountry = context.m_CountryMaster.Where(a => a.Name.ToUpper().Trim().Contains(countryName.ToUpper().Trim())).FirstOrDefault();
                                        if (searchCountry == null)
                                        {
                                            searchCountry = context.m_CountryMaster.Where(a => (a.Code.ToUpper().Trim() == countryCode.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCountry == null)
                                                searchCountry = context.m_CountryMaster.Where(a => a.ISO3166_1_Alpha_2.ToUpper().Trim().Equals(countryCode.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCountry == null)
                                                searchCountry = context.m_CountryMaster.Where(a => a.ISO3166_1_Alpha_3.ToUpper().Trim().Equals(countryCode.ToUpper().Trim())).FirstOrDefault();
                                        }
                                    }
                                    if (searchCountry != null)
                                    {
                                        item.MasterNameWithCode = searchCountry.Name + " (" + searchCountry.Code + ")";
                                        item.Code = searchCountry.Code;
                                        item.MasterCountry_Id = Convert.ToString(searchCountry.Country_Id);
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            List<DC_CountryMapping> CMS = obj.lstCountryMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";

            curSupplier = CMS[0].SupplierName;
            curSupplier_Id = CMS[0].Supplier_Id;

            List<DC_CountryMapping> ret = new List<DC_CountryMapping>();
            //foreach (var CM in CMS)
            //{
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //var prodMapSearch = from a in context.m_CountryMapping
                    //                    join m in context.m_CountryMaster on a.CountryName equals m.Name into j
                    //                    from jd in j.DefaultIfEmpty()
                    //                    select new
                    //                    {
                    //                        a.CountryMapping_Id, mCountry_ID =  a.Country_Id, a.CountryName, a.CountryCode, a.Supplier_Id, a.SupplierName, a.Status
                    //                        , jd.Name, Country_Id = jd.Country_Id, jd.Code, jd.GooglePlaceID, jd.ISO3166_1_Alpha_2, jd.ISO3166_1_Alpha_3
                    //                    };

                    var prodMapSearch = (from a in context.m_CountryMapping
                                             //join p in CMS on m_CountryMapping.CountryMapping_Id equals p.CountryMapping_Id
                                         where a.Country_Id == null && a.Supplier_Id == curSupplier_Id
                                         select a);
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = " " + configWhere + config.AttributeName + " == " + config.AttributeValue + " AND";

                        if (config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper() == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.Code
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper() == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryName equals m.Name
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper() == "ISO3166-1-Alpha-2".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_2
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper() == "ISO3166-1-Alpha-3".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_3
                                             select a);
                        }
                    }

                    //var prodMapSearch1 = prodMapSearch.ToList().Select(c =>
                    //{
                    //    c.Country_Id = (from cm in context.m_CountryMaster where cm.Code == c.CountryCode && isCodeCheck select cm.Country_Id).FirstOrDefault();
                    //    return c;
                    //}).ToList();

                    var res = (from a in prodMapSearch
                               select new DataContracts.Mapping.DC_CountryMapping
                               {
                                   CountryMapping_Id = a.CountryMapping_Id,
                                   Country_Id = a.Country_Id,
                                   Supplier_Id = a.Supplier_Id,
                                   CountryCode = a.CountryCode,
                                   CountryName = a.CountryName,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   MapID = a.MapID,
                                   Status = a.Status,
                                   SupplierName = a.SupplierName
                               }).ToList();

                    res = res.Select(c =>
                    {
                        c.Country_Id = (context.m_CountryMaster
                                        .Where(s => (
                                                        (isCodeCheck && s.Code == c.CountryCode && isNameCheck && s.Name == c.CountryName) ||
                                                        (isCodeCheck && (s.Code == c.CountryCode || s.ISO3166_1_Alpha_2 == c.CountryCode || s.ISO3166_1_Alpha_3 == c.CountryCode) && (!isNameCheck)) ||
                                                        ((!isCodeCheck) && isNameCheck && s.Name == c.CountryName)
                                                    )
                                               )
                                        .Select(s1 => s1.Country_Id)
                                        .FirstOrDefault()
                                        );
                        return c;
                    }).ToList();


                    //    (c =>
                    //{
                    //    c.Country_Id = (tempCountryMaster
                    //                    .Where(s => (isCodeCheck == true && s.Code == c.CountryCode))
                    //                    .Select(s1 => s1.Country_Id)
                    //                    .FirstOrDefault()
                    //                    );
                    //    return c;
                    //}).ToList();



                    //configWhere = configWhere.Substring(0, configWhere.Length - 3);

                    //var search = (from m_CountryMapping in context.m_CountryMapping
                    //              join p in CMS on m_CountryMapping.CountryMapping_Id equals p.CountryMapping_Id
                    //              where m_CountryMapping.Country_Id == null
                    //              select new DC_CountryMapping //m_CountryMapping
                    //              {
                    //                  CountryMapping_Id = m_CountryMapping.CountryMapping_Id,
                    //                  CountryCode = m_CountryMapping.CountryCode,
                    //                  CountryName = m_CountryMapping.CountryName,
                    //                  Supplier_Id = m_CountryMapping.Supplier_Id,
                    //                  SupplierName = m_CountryMapping.SupplierName,
                    //                  Create_Date = m_CountryMapping.Create_Date,
                    //                  Create_User = m_CountryMapping.Create_User,
                    //                  Edit_Date = m_CountryMapping.Edit_Date,
                    //                  Edit_User = m_CountryMapping.Edit_User,
                    //                  MapID = m_CountryMapping.MapID,
                    //                  Remarks = m_CountryMapping.Remarks,
                    //                  Status = m_CountryMapping.Status,
                    //                  //Country_Id = (context.m_CountryMaster.Where(p => p.Name == m_CountryMapping.CountryName && p.Code == m_CountryMapping.CountryCode
                    //                  //    )).FirstOrDefault().Country_Id
                    //              }).ToList();

                    //var res = search;

                    foreach (DC_CountryMapping v in res)
                    {
                        if (v.Country_Id != null)
                        {
                            if (v.Country_Id != Guid.Empty)
                                v.Status = "MAPPED";
                            else
                            {
                                v.Country_Id = null;
                                res.Remove(v);
                            }
                        }
                        else
                            res.Remove(v);
                    }

                    if (UpdateCountryMapping(res))
                    {
                        if (curSupplier_Id != null)
                        {
                            DataContracts.Mapping.DC_CountryMappingRQ RQ = new DC_CountryMappingRQ();
                            RQ.Supplier_Id = curSupplier_Id;
                            RQ.PageNo = 0;
                            RQ.PageSize = int.MaxValue;
                            RQ.Status = "UNMAPPED";
                            res = GetCountryMapping(RQ);
                        }
                    }

                    return res;
                    //var result = prodMapSearch.Join(context.m_CountryMaster, "new(Id as firstKey,SomeOtherId as secondKey)"
                    //    , "new(PersonId as firstKey,AlternativeId as secondKey)", "new (inner as r, outer as p)");

                    //prodMapSearch.Join(context.m_CountryMaster, 
                    //    "new(get_Item(0) as FundId, get_Item(1) as Date)",
                    //    "new(get_Item(0) as FundId, get_Item(1) as Date)",
                    //    "new(outer.get_Item(0) as FundId, outer.get_Item(2) as CodeA, inner.get_Item(2) as CodeB)"
                    //);

                    //var res = (from a in prodMapSearch
                    //           select a).AsQueryable().Where()
                }

            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            //}
        }

        public bool UpdateCountryMapping(List<DataContracts.Mapping.DC_CountryMapping> obj)
        {
            foreach (var CM in obj)
            {
                if (CM.CountryMapping_Id == null || CM.Supplier_Id == null)
                {
                    continue;
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.m_CountryMapping
                                      where a.CountryMapping_Id == CM.CountryMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            //if (CM.Status != (search.Status ?? string.Empty))
                            //{
                            //    search.Status = CM.Status;
                            //    search.Edit_Date = CM.Edit_Date;
                            //    search.Edit_User = CM.Edit_User;
                            //}
                            //else
                            //{
                            search.Country_Id = CM.Country_Id;
                            search.Supplier_Id = CM.Supplier_Id;
                            search.Status = CM.Status;
                            search.Edit_Date = CM.Edit_Date;
                            search.Edit_User = CM.Edit_User;
                            search.Remarks = CM.Remarks;
                            //}

                            context.SaveChanges();
                        }
                        else
                        {
                            DataLayer.m_CountryMapping objNew = new m_CountryMapping();
                            objNew.CountryMapping_Id = CM.CountryMapping_Id;
                            objNew.Country_Id = CM.Country_Id;
                            objNew.Supplier_Id = CM.Supplier_Id;
                            objNew.SupplierName = CM.SupplierName;
                            objNew.CountryName = CM.CountryName;
                            objNew.CountryCode = CM.CountryCode;
                            objNew.Status = CM.Status;
                            objNew.Create_Date = CM.Create_Date;
                            objNew.Create_User = CM.Create_User;
                            objNew.Edit_Date = CM.Edit_Date;
                            objNew.Edit_User = CM.Edit_User;
                            objNew.MapID = CM.MapID;
                            objNew.Remarks = CM.Remarks;
                            context.m_CountryMapping.Add(objNew);
                            context.SaveChanges();

                            context.USP_UpdateMapID("country");
                        }

                        //return true;
                    }
                }
                catch
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating country mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return true;
        }

        #endregion

        #region City Mapping
        public List<DataContracts.Mapping.DC_CityMapping> GetCityMapping(DataContracts.Mapping.DC_CityMapping_RQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.m_CityMapping select a;

                    if (param.Supplier_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == param.Supplier_Id
                                        select a;
                    }

                    if (param.City_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.City_Id == param.City_Id
                                        select a;
                    }

                    if (param.Country_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Country_Id == param.Country_Id
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.CityCode))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityCode == param.CityCode
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.CityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName == param.CityName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.Status))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status == param.Status
                                        select a;
                    }

                    if (param.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Status.ToUpper() != param.StatusExcept.ToUpper()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(param.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        join c in context.m_CountryMaster on a.Country_Id equals c.Country_Id
                                        where (a.CountryName.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        || c.Name.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        )
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(param.SupplierCityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName.Contains(param.SupplierCityName)
                                        select a;
                    }



                    int total;

                    total = prodMapSearch.Count();

                    var skip = param.PageSize * param.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    if (!string.IsNullOrWhiteSpace(param.ResultSet))
                    {
                        total = (from a in prodMapSearch
                                 join s in context.Suppliers on a.Supplier_Id equals s.Supplier_Id
                                 join ct in context.m_CityMaster on a.City_Id equals ct.City_Id into ctl
                                 from ctld in ctl.DefaultIfEmpty()
                                 join m in context.m_CountryMaster on a.Country_Id equals m.Country_Id into j
                                 from jd in j.DefaultIfEmpty()
                                 select a.CityName).Distinct().Count();
                        var prodMapList = (from a in prodMapSearch
                                           join s in context.Suppliers on a.Supplier_Id equals s.Supplier_Id
                                           join ct in context.m_CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in context.m_CountryMaster on a.Country_Id equals m.Country_Id into j
                                           from jd in j.DefaultIfEmpty()
                                           select new DataContracts.Mapping.DC_CityMapping
                                           {
                                               CityName = a.CityName,
                                               TotalRecords = total
                                           }).Distinct().OrderBy(o => o.CityName).Skip(skip).Take(param.PageSize);
                        return prodMapList.ToList();
                    }
                    else
                    {
                        var CityMapList = (from a in prodMapSearch
                                           join s in context.Suppliers on a.Supplier_Id equals s.Supplier_Id
                                           join ct in context.m_CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in context.m_CountryMaster on a.Country_Id equals m.Country_Id into j
                                           from jd in j.DefaultIfEmpty()
                                               //orderby (param.SortBy)
                                           orderby a.CityName
                                           select new DataContracts.Mapping.DC_CityMapping
                                           {
                                               CityMapping_Id = a.CityMapping_Id,
                                               City_Id = a.City_Id,
                                               CityCode = a.CityCode,
                                               CityName = a.CityName,
                                               Country_Id = a.Country_Id,
                                               Create_Date = a.Create_Date,
                                               Create_User = a.Create_User,
                                               Edit_Date = a.Edit_Date,
                                               Edit_User = a.Edit_User,
                                               MapID = a.MapID,
                                               Supplier_Id = a.Supplier_Id,
                                               Status = a.Status,
                                               TotalRecords = total,
                                               SupplierName = s.Name,
                                               CountryCode = jd.Code,
                                               CountryName = jd.Name,
                                               MasterCountryCode = jd.Code,
                                               MasterCountryName = jd.Name,
                                               MasterCityCode = ctld.Code,
                                               Master_CityName = ctld.Name,
                                               StateCode = a.StateCode,
                                               StateName = a.StateName

                                           }).Skip(skip).Take(param.PageSize).ToList();

                        if (!string.IsNullOrWhiteSpace(param.Status))
                        {

                            foreach (var item in CityMapList)
                            {
                                if (param.Status.ToUpper().Trim() == "UNMAPPED" && string.IsNullOrWhiteSpace(item.Master_CityName))
                                {
                                    if (item.CityName != null && item.CityCode != null)
                                    {
                                        var cityName = item.CityName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("city", "").ToUpper();
                                        var cityCode = item.CityCode.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("city", "").ToUpper();
                                        var searchCity = context.m_CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityName.ToUpper().Trim())).FirstOrDefault();
                                        if (searchCity == null)
                                        {
                                            searchCity = context.m_CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityName.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCity == null)
                                            {
                                                searchCity = context.m_CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityCode.ToUpper().Trim())).FirstOrDefault();
                                                if (searchCity == null)
                                                    searchCity = context.m_CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityCode.ToUpper().Trim())).FirstOrDefault();
                                            }
                                        }
                                        if (searchCity != null)
                                        {
                                            item.Master_CityName = searchCity.Name;
                                            item.MasterCityCode = searchCity.Code;
                                            item.City_Id = searchCity.City_Id;
                                        }
                                    }
                                }
                            }
                        }
                        return CityMapList;
                    }
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            List<DC_CityMapping> CMS = obj.lstCityMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";

            curSupplier = CMS[0].SupplierName;
            curSupplier_Id = CMS[0].Supplier_Id;

            List<DC_CityMapping> ret = new List<DC_CityMapping>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = (from a in context.m_CityMapping
                                             //join p in CMS on m_CountryMapping.CountryMapping_Id equals p.CountryMapping_Id
                                         where a.City_Id == null && a.Supplier_Id == curSupplier_Id
                                         select a);
                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = " " + configWhere + config.AttributeName + " == " + config.AttributeValue + " AND";


                        if (config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper() == "COUNTRYCODE")
                        {
                            isCountryCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.Code
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper() == "COUNTRYNAME")
                        {
                            isCountryNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryName equals m.Name
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper() == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CityMaster on a.CityCode equals m.Code
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             //join mc in context.m_CountryMaster on m.Country_Id equals mc.Country_Id
                                             //join cm in context.m_CountryMapping on new { a.Country_Id, a.Supplier_Id } equals new { cm.Country_Id, cm.Supplier_Id }
                                             where mc.Country_Id == m.Country_Id
                                             select a);
                        }
                        if (config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper() == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CityMaster on a.CityName equals m.Name
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             where mc.Country_Id == m.Country_Id
                                             select a);
                        }
                    }


                    var res = (from a in prodMapSearch
                               select new DataContracts.Mapping.DC_CityMapping
                               {
                                   CityMapping_Id = a.CityMapping_Id,
                                   CityCode = a.CityCode,
                                   CityName = a.CityName,
                                   City_Id = a.City_Id,
                                   StateCode = a.StateCode,
                                   StateName = a.StateName,
                                   Country_Id = a.Country_Id,
                                   Supplier_Id = a.Supplier_Id,
                                   CountryCode = a.CountryCode,
                                   CountryName = a.CountryName,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   MapID = a.MapID,
                                   Status = a.Status,
                                   SupplierName = a.SupplierName
                               }).ToList();

                    res = res.Select(c =>
                    {
                        c.City_Id = (context.m_CityMaster
                                        .Where(s => (
                                                        (isCountryCodeCheck && isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.CountryName == c.CountryName && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                        (isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode) ||
                                                        (isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.Name == c.CityName) ||
                                                        (isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.Code == c.CityCode) ||
                                                        (isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                        (isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.CountryName == c.CountryName) ||
                                                        (isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.CountryName == c.CountryName && s.Name == c.CityName) ||
                                                        (isCountryCodeCheck && isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            && s.CountryName == c.CountryName && s.Code == c.CityCode) ||

                                                        (!isCountryCodeCheck && isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryName == c.CountryName
                                                            && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                        (!isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && isNameCheck && s.Name == c.CityName) ||
                                                        (!isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && !isNameCheck && s.Code == c.CityCode) ||
                                                        (!isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && isNameCheck && s.Code == c.CityCode
                                                            && s.Name == c.CityName) ||
                                                        (!isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryName == c.CountryName) ||
                                                        (!isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryName == c.CountryName
                                                            && s.Name == c.CityName) ||
                                                        (!isCountryCodeCheck && isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryName == c.CountryName
                                                            && s.Code == c.CityCode)
                                                    )
                                               )
                                        .Select(s1 => s1.City_Id)
                                        .FirstOrDefault()
                                        );
                        return c;
                    }).ToList();


                    res.RemoveAll(p => p.City_Id == Guid.Empty);
                    res = res.Select(c =>
                    {
                        c.Status = ("REVIEW"); return c;
                    }).ToList();

                    if (UpdateCityMapping(res))
                    {
                        if (curSupplier_Id != null)
                        {
                            DataContracts.Mapping.DC_CityMapping_RQ RQ = new DC_CityMapping_RQ();
                            RQ.Supplier_Id = curSupplier_Id;
                            RQ.PageNo = 0;
                            RQ.PageSize = int.MaxValue;
                            RQ.Status = "UNMAPPED";
                            res = GetCityMapping(RQ);
                        }
                    }

                    return res;
                }

            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            //}
        }
        public bool UpdateCityMapping(List<DataContracts.Mapping.DC_CityMapping> obj)
        {
            bool ret = false;
            foreach (var CM in obj)
            {
                if (CM.CityMapping_Id == null || CM.Supplier_Id == null)
                {
                    continue;
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.m_CityMapping
                                      where a.CityMapping_Id == CM.CityMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            //if (CM.Status != (search.Status ?? string.Empty))
                            //{
                            //    search.City_Id = CM.City_Id;
                            //    search.Status = CM.Status;
                            //    search.Edit_Date = CM.Edit_Date;
                            //    search.Edit_User = CM.Edit_User;
                            //}
                            //else
                            //{
                            search.City_Id = CM.City_Id;
                            search.Country_Id = CM.Country_Id;
                            search.Supplier_Id = CM.Supplier_Id;
                            search.Status = CM.Status;
                            search.Edit_Date = CM.Edit_Date;
                            search.Edit_User = CM.Edit_User;
                            search.Remarks = CM.Remarks;
                            search.StateCode = CM.StateCode;
                            search.StateName = CM.StateName;
                            //}

                            context.SaveChanges();
                        }
                        else
                        {
                            DataLayer.m_CityMapping objNew = new m_CityMapping();
                            objNew.CityMapping_Id = CM.CityMapping_Id;
                            objNew.City_Id = CM.City_Id;
                            objNew.CityName = CM.CityName;
                            objNew.CityCode = CM.CityCode;
                            objNew.Supplier_Id = CM.Supplier_Id;
                            objNew.SupplierName = CM.SupplierName;
                            objNew.CountryName = CM.CountryName;
                            objNew.CountryCode = CM.CountryCode;
                            objNew.Status = CM.Status;
                            objNew.Create_Date = CM.Create_Date;
                            objNew.Create_User = CM.Create_User;
                            objNew.Edit_Date = CM.Edit_Date;
                            objNew.Edit_User = CM.Edit_User;
                            objNew.MapID = CM.MapID;
                            objNew.Remarks = CM.Remarks;
                            // objNew.Country_Id = CM.Country_Id;
                            objNew.Country_Id = (from a in context.m_CountryMapping
                                                 where a.CountryName == CM.CountryName && a.Supplier_Id == CM.Supplier_Id
                                                 select a.Country_Id).FirstOrDefault();
                            context.m_CityMapping.Add(objNew);
                            context.SaveChanges();

                            context.USP_UpdateMapID("city");
                        }

                        ret = true;
                    }
                }
                catch
                {
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            return ret;
        }
        #endregion

        #region Mapping Stats
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(Guid SupplierID)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    List<vwMappingStat> search = new List<vwMappingStat>();

                    //if (SupplierID == Guid.Empty)
                    //{
                    //    //Country
                    //    var resCountryList = (from m in context.m_CountryMapping
                    //                          where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                          group m by (m.Status ?? "UNMAPPED") into g
                    //                          select new vwMappingStat
                    //                          {
                    //                              supplier_id = Guid.Empty,
                    //                              SupplierName = "ALL",
                    //                              Status = g.Key,
                    //                              MappinFor = "Country",
                    //                              totalcount = g.Count()
                    //                          }).ToList();
                    //    search.AddRange(resCountryList);
                    //    resCountryList = null;

                    //    var resCountryOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "Country",
                    //        totalcount = (from m in context.m_CountryMapping
                    //                      where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resCountryOne);
                    //    resCountryOne = null;

                    //    //City
                    //    var resCityList = (from m in context.m_CityMapping
                    //                       where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                       group m by (m.Status ?? "UNMAPPED") into g
                    //                       select new vwMappingStat
                    //                       {
                    //                           supplier_id = Guid.Empty,
                    //                           SupplierName = "ALL",
                    //                           Status = g.Key,
                    //                           MappinFor = "City",
                    //                           totalcount = g.Count()
                    //                       }).ToList();
                    //    search.AddRange(resCityList);
                    //    resCityList = null;

                    //    var resCityOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "City",
                    //        totalcount = (from m in context.m_CityMapping
                    //                      where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resCityOne);
                    //    resCityOne = null;

                    //    //Accommodation
                    //    var resAccommodationList = (from m in context.Accommodation_ProductMapping
                    //                                where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                                group m by (m.Status ?? "UNMAPPED") into g
                    //                                select new vwMappingStat
                    //                                {
                    //                                    supplier_id = Guid.Empty,
                    //                                    SupplierName = "ALL",
                    //                                    Status = g.Key,
                    //                                    MappinFor = "Product",
                    //                                    totalcount = g.Count()
                    //                                }).ToList();
                    //    search.AddRange(resAccommodationList);
                    //    resAccommodationList = null;

                    //    var resAccommodationOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "Product",
                    //        totalcount = (from m in context.Accommodation_ProductMapping
                    //                      where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resAccommodationOne);
                    //    resAccommodationOne = null;

                    //    //Activity
                    //    var resActivityList = (from m in context.Activity_SupplierProductMapping
                    //                           where (m.MappingStatus ?? "UNMAPPED") != "DELETE"
                    //                           group m by (m.MappingStatus ?? "UNMAPPED") into g
                    //                           select new vwMappingStat
                    //                           {
                    //                               supplier_id = Guid.Empty,
                    //                               SupplierName = "ALL",
                    //                               Status = g.Key,
                    //                               MappinFor = "Activity",
                    //                               totalcount = g.Count()
                    //                           }).ToList();
                    //    search.AddRange(resActivityList);
                    //    resActivityList = null;

                    //    var resActivityOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "Activity",
                    //        totalcount = (from m in context.Activity_SupplierProductMapping
                    //                      where (m.MappingStatus ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resActivityOne);
                    //    resActivityOne = null;
                    //}
                    //else //Individual Supplier (SupplierId is not Empty)
                    //{
                    //    //Country
                    //    var resCountryList = (from m in context.m_CountryMapping
                    //                          where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                          && m.Supplier_Id == SupplierID
                    //                          group m by (m.Status ?? "UNMAPPED") into g
                    //                          select new vwMappingStat
                    //                          {
                    //                              supplier_id = g.Select(m => m.Supplier_Id).FirstOrDefault() ?? Guid.Empty,
                    //                              SupplierName = g.Select(m => m.SupplierName).FirstOrDefault(),
                    //                              Status = g.Key,
                    //                              MappinFor = "Country",
                    //                              totalcount = g.Count()
                    //                          }).ToList();
                    //    search.AddRange(resCountryList);
                    //    resCountryList = null;

                    //    var resCountryOne = (from m in context.m_CountryMapping
                    //                         where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                         && m.Supplier_Id == SupplierID
                    //                         group m by new { Supplier_Id = m.Supplier_Id, Supplier_Name = m.SupplierName } into g
                    //                         select new vwMappingStat
                    //                         {
                    //                             supplier_id = g.Key.Supplier_Id ?? Guid.Empty,
                    //                             SupplierName = g.Key.Supplier_Name,
                    //                             Status = "ALL",
                    //                             MappinFor = "Country",
                    //                             totalcount = g.Count()
                    //                         }).FirstOrDefault();

                    //    search.Add(resCountryOne);
                    //    resCountryOne = null;

                    //    //------------------------

                    //    //City
                    //    var resCityList = (from m in context.m_CityMapping
                    //                       where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                       group m by (m.Status ?? "UNMAPPED") into g
                    //                       select new vwMappingStat
                    //                       {
                    //                           supplier_id = Guid.Empty,
                    //                           SupplierName = "ALL",
                    //                           Status = g.Key,
                    //                           MappinFor = "City",
                    //                           totalcount = g.Count()
                    //                       }).ToList();
                    //    search.AddRange(resCityList);
                    //    resCityList = null;

                    //    var resCityOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "City",
                    //        totalcount = (from m in context.m_CityMapping
                    //                      where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resCityOne);
                    //    resCityOne = null;

                    //    //Accommodation
                    //    var resAccommodationList = (from m in context.Accommodation_ProductMapping
                    //                                where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                                group m by (m.Status ?? "UNMAPPED") into g
                    //                                select new vwMappingStat
                    //                                {
                    //                                    supplier_id = Guid.Empty,
                    //                                    SupplierName = "ALL",
                    //                                    Status = g.Key,
                    //                                    MappinFor = "Product",
                    //                                    totalcount = g.Count()
                    //                                }).ToList();
                    //    search.AddRange(resAccommodationList);
                    //    resAccommodationList = null;

                    //    var resAccommodationOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "Product",
                    //        totalcount = (from m in context.Accommodation_ProductMapping
                    //                      where (m.Status ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resAccommodationOne);
                    //    resAccommodationOne = null;

                    //    //Activity
                    //    var resActivityList = (from m in context.Activity_SupplierProductMapping
                    //                           where (m.MappingStatus ?? "UNMAPPED") != "DELETE"
                    //                           group m by (m.MappingStatus ?? "UNMAPPED") into g
                    //                           select new vwMappingStat
                    //                           {
                    //                               supplier_id = Guid.Empty,
                    //                               SupplierName = "ALL",
                    //                               Status = g.Key,
                    //                               MappinFor = "Activity",
                    //                               totalcount = g.Count()
                    //                           }).ToList();
                    //    search.AddRange(resActivityList);
                    //    resActivityList = null;

                    //    var resActivityOne = new vwMappingStat
                    //    {
                    //        supplier_id = Guid.Empty,
                    //        SupplierName = "ALL",
                    //        Status = "ALL",
                    //        MappinFor = "Activity",
                    //        totalcount = (from m in context.Activity_SupplierProductMapping
                    //                      where (m.MappingStatus ?? "UNMAPPED") != "DELETE"
                    //                      select m).Count()
                    //    };

                    //    search.Add(resActivityOne);
                    //    resActivityOne = null;
                    //}



                    if (SupplierID != Guid.Empty)
                    {
                        search = (from s in context.vwMappingStats
                                  where s.supplier_id == SupplierID && s.SupplierName != null
                                  select s).ToList();
                    }
                    else
                    {
                        search = (from s in context.vwMappingStats
                                  where (s.supplier_id == Guid.Empty) && s.SupplierName != null
                                  select s).ToList();
                        //search = (from vw in context.vwMappingStats select vw)
                        //            .GroupBy(g => new { g.MappinFor, g.Status })
                        //            .Select(s => new vwMappingStat
                        //            {
                        //                SupplierName = "ALL",
                        //                supplier_id = Guid.Empty,
                        //                MappinFor = s.Key.MappinFor,
                        //                Status = s.Key.Status,
                        //                totalcount = s.Sum(x => x.totalcount)
                        //            });

                    }

                    List<DataContracts.Mapping.DC_MappingStats> returnObj = new List<DataContracts.Mapping.DC_MappingStats>();

                    //var SupList = (from s in search select new { SupplierId = (s.supplier_id ?? Guid.Empty), SupplierName = s.SupplierName }).ToList().Distinct();

                    //foreach (var item in SupList)
                    //{
                    DataContracts.Mapping.DC_MappingStats newmapstats = new DataContracts.Mapping.DC_MappingStats();
                    newmapstats.SupplierId = SupplierID; //item.SupplierId;
                    newmapstats.SupplierName = string.Empty; //item.SupplierName;
                    if (SupplierID != Guid.Empty)
                        newmapstats.NextRun = (from s in context.Schedule_NextOccurance where s.Schedule_ID == SupplierID select s.Execution_StartDate).FirstOrDefault().ToString();
                    else
                        newmapstats.NextRun = "Not Scheduled";

                    List<DataContracts.Mapping.DC_MappingStatsFor> newmapstatsforList = new List<DataContracts.Mapping.DC_MappingStatsFor>();

                    var MapForList = (from s in search select s.MappinFor).ToList().Distinct();

                    foreach (var mapfor in MapForList)
                    {
                        DataContracts.Mapping.DC_MappingStatsFor newmapstatsfor = new DataContracts.Mapping.DC_MappingStatsFor();

                        newmapstatsfor.MappingFor = mapfor;


                        var MappedCount = (from s in search where s.MappinFor == mapfor && s.Status == "MAPPED" select s.totalcount).FirstOrDefault();
                        var AllCount = (from s in search where s.MappinFor == mapfor && s.Status == "ALL" select s.totalcount).FirstOrDefault();

                        if (MappedCount == null)
                            MappedCount = 0;

                        if (AllCount == null)
                            AllCount = 0;

                        if (AllCount == 0)
                        {
                            newmapstatsfor.MappedPercentage = Convert.ToDecimal(0);
                        }
                        else
                        {
                            newmapstatsfor.MappedPercentage = Math.Round((Convert.ToDecimal(MappedCount) / Convert.ToDecimal(AllCount) * Convert.ToDecimal(100)), 2);
                        }

                        if (newmapstatsfor.MappedPercentage >= 80)
                        {
                            newmapstatsfor.ProgressCss = "progress-bar-success";
                        }
                        else if (newmapstatsfor.MappedPercentage < 80 && newmapstatsfor.MappedPercentage >= 60)
                        {
                            newmapstatsfor.ProgressCss = "progress-bar-info";
                        }
                        else if (newmapstatsfor.MappedPercentage < 60 && newmapstatsfor.MappedPercentage >= 40)
                        {
                            newmapstatsfor.ProgressCss = "progress-bar-warning";
                        }
                        else if (newmapstatsfor.MappedPercentage < 40)
                        {
                            newmapstatsfor.ProgressCss = "progress-bar-danger";
                        }

                        newmapstatsfor.MappingData = (from s in search
                                                      where s.MappinFor == mapfor
                                                      orderby s.Status
                                                      select new DataContracts.Mapping.DC_MappingData { Status = s.Status, TotalCount = (s.totalcount ?? 0) }).ToList();

                        newmapstatsforList.Add(newmapstatsfor);
                    }

                    newmapstats.MappingStatsFor = newmapstatsforList;

                    returnObj.Add(newmapstats);
                    //}

                    return returnObj;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching mapping statistics", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }


        public List<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers()
        {
            List<DataContracts.Mapping.DC_MappingStatsForSuppliers> objLst = new List<DataContracts.Mapping.DC_MappingStatsForSuppliers>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.vwMappingStats.Where(cat => (cat.SupplierName != "ALL") && (cat.SupplierName != null) && (cat.supplier_id != Guid.Empty) && (cat.Status == "UNMAPPED" || cat.Status == "REVIEW"))
                                                        .GroupBy(cat => new { cat.SupplierName, cat.supplier_id, cat.MappinFor })
                                                        .Select(group => new
                                                        {
                                                            SupplierName = group.Key.SupplierName,
                                                            SupplierId = group.Key.supplier_id,
                                                            Mappinfor = group.Key.MappinFor,
                                                            totalcount = group.Sum(x => x.totalcount)
                                                        }).OrderBy(x => x.SupplierName).ToList();

                    foreach (var item in search)
                    {
                        DC_MappingStatsForSuppliers _obj = new DC_MappingStatsForSuppliers();
                        _obj.SupplierName = item.SupplierName;
                        //_obj.SupplierId = item.supplier_id;
                        _obj.Mappingfor = item.Mappinfor;
                        _obj.totalcount = item.totalcount.Value;
                        objLst.Add(_obj);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objLst;
        }

        #endregion
        #region roll_off_reports
        public List<DataContracts.Mapping.DC_RollOffReportRule> getStatisticforRuleReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportRule> objLst = new List<DC_RollOffReportRule>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_RuleInfo
                                  join t1 in context.Accommodations on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.Create_Date >= fd && t.Edit_Date <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      RuleName = t.RuleType,
                                      Description = t.Description,
                                      flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportRule obj = new DC_RollOffReportRule();
                        obj.Hotelid = item.HotelID.Value;
                        obj.Hotelname = item.HotelName;
                        obj.RuleName = item.RuleName;
                        obj.Description = item.Description;
                        obj.Internal_Flag = item.flag;
                        obj.LastupdateDate = Convert.ToString(item.LupdateDate);
                        obj.LastupdatedBy = item.LupdateBy;
                        objLst.Add(obj);
                    }
                }

            }
            catch
            {

            }
            return objLst;
        }

        #endregion
        #region Master Attribute Mapping
        public List<DataContracts.Mapping.DC_MasterAttributeMapping_RS> SearchMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var search = from a in context.m_MasterAttributeMapping select a;
                    if (RQ.MasterAttributeType_Id.HasValue)
                    {
                        search = from a in search
                                 where a.SystemMasterAttribute_Id == RQ.MasterAttributeType_Id
                                 select a;
                    }

                    if (RQ.Supplier_Id.HasValue)
                    {
                        search = from a in search
                                 where a.Supplier_Id == RQ.Supplier_Id
                                 select a;
                    }

                    int total;

                    total = search.Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var searchList = (from a in search
                                      join sup in context.Suppliers on a.Supplier_Id equals sup.Supplier_Id
                                      join mav in context.m_masterattributevalue on a.SystemMasterAttribute_Id equals mav.MasterAttributeValue_Id
                                      orderby sup.Name, mav.AttributeValue
                                      select new DataContracts.Mapping.DC_MasterAttributeMapping_RS
                                      {
                                          MasterAttributeMapping_Id = a.MasterAttributeMapping_Id,
                                          Supplier_Attribute_Type = a.SupplierMasterAttribute,
                                          Supplier_Code = sup.Code,
                                          Supplier_Name = sup.Name,
                                          System_Attribute_Type = mav.AttributeValue,
                                          Status = a.Status,
                                          IsActive = a.IsActive,
                                          TotalRecords = total
                                      }).Skip(skip).Take(RQ.PageSize).ToList();

                    return searchList;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.Mapping.DC_MasterAttributeMapping GetMasterAttributeMapping(Guid MasterAttributeMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.m_MasterAttributeMapping
                                  join sup in context.Suppliers on a.Supplier_Id equals sup.Supplier_Id
                                  join mav in context.m_masterattributevalue on a.SystemMasterAttribute_Id equals mav.MasterAttributeValue_Id
                                  where a.MasterAttributeMapping_Id == MasterAttributeMapping_Id
                                  select new DataContracts.Mapping.DC_MasterAttributeMapping
                                  {
                                      MasterAttributeMapping_Id = a.MasterAttributeMapping_Id,
                                      Create_Date = a.Create_Date,
                                      Create_User = a.Create_User,
                                      Edit_Date = a.Edit_Date,
                                      Edit_User = a.Edit_User,
                                      IsActive = a.IsActive,
                                      Status = a.Status,
                                      SupplierMasterAttribute = a.SupplierMasterAttribute,
                                      Supplier_Code = sup.Code,
                                      Supplier_Id = a.Supplier_Id,
                                      Supplier_Name = sup.Name,
                                      SystemMasterAttribute = mav.AttributeValue,
                                      SystemMasterAttribute_Id = a.SystemMasterAttribute_Id
                                  }).FirstOrDefault();

                    return search;

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_MasterAttributeValueMapping> GetMasterAttributeValueMapping(Guid MasterAttributeMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from map in context.m_MasterAttributeMapping
                                 join mapv in context.m_masterattributevalue on map.SystemMasterAttribute_Id equals mapv.MasterAttributeValue_Id
                                 join mav in context.m_masterattributevalue on mapv.MasterAttributeValue_Id equals mav.ParentAttributeValue_Id
                                 join mavm in context.m_MasterAttributeValueMapping on mav.MasterAttributeValue_Id equals mavm.SystemMasterAttributeValue_Id into mavm_l
                                 from lr in mavm_l.DefaultIfEmpty()
                                 where map.MasterAttributeMapping_Id == MasterAttributeMapping_Id
                                 select new DataContracts.Mapping.DC_MasterAttributeValueMapping
                                 {
                                     MasterAttributeMapping_Id = map.MasterAttributeMapping_Id,
                                     Create_Date = map.Create_Date,
                                     Create_User = map.Create_User,
                                     Edit_Date = map.Edit_Date,
                                     Edit_User = map.Edit_User,
                                     IsActive = map.IsActive,
                                     MasterAttributeValueMapping_Id = (lr == null ? Guid.NewGuid() : lr.MasterAttributeValueMapping_Id),
                                     SupplierMasterAttributeValue = (lr == null ? string.Empty : lr.SupplierMasterAttributeValue),
                                     SystemMasterAttributeValue = mav.AttributeValue,
                                     SystemMasterAttributeValue_Id = mav.MasterAttributeValue_Id
                                 };

                    return search.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching master attribute value mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var duplicateSearch = from a in context.m_MasterAttributeMapping
                                          where a.SupplierMasterAttribute == param.SupplierMasterAttribute.Trim()
                                          && a.Supplier_Id == param.Supplier_Id
                                          && a.SystemMasterAttribute_Id == param.SystemMasterAttribute_Id
                                          select a;
                    if (duplicateSearch.Count() > 0)
                    {
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate,
                            StatusMessage = "Data already exist."
                        };
                    }
                    else
                    {
                        m_MasterAttributeMapping newObj = new m_MasterAttributeMapping();

                        newObj.MasterAttributeMapping_Id = param.MasterAttributeMapping_Id;
                        newObj.SupplierMasterAttribute = param.SupplierMasterAttribute;
                        newObj.Supplier_Id = param.Supplier_Id;
                        newObj.SystemMasterAttribute_Id = param.SystemMasterAttribute_Id;
                        newObj.Status = param.Status;
                        newObj.IsActive = true;
                        newObj.Create_Date = DateTime.Now;
                        newObj.Create_User = param.Create_User;

                        context.m_MasterAttributeMapping.Add(newObj);
                        context.SaveChanges();

                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                            StatusMessage = "Data saved successfully."
                        };
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var duplicateSearch = from a in context.m_MasterAttributeMapping
                                          where a.SupplierMasterAttribute == param.SupplierMasterAttribute.Trim()
                                          && a.Supplier_Id == param.Supplier_Id
                                          && a.SystemMasterAttribute_Id == param.SystemMasterAttribute_Id
                                          && a.MasterAttributeMapping_Id != param.MasterAttributeMapping_Id
                                          select a;
                    if (duplicateSearch.Count() > 0)
                    {
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate,
                            StatusMessage = "Data already exist."
                        };
                    }
                    else
                    {
                        DataContracts.DC_Message msg = new DataContracts.DC_Message();

                        var search = context.m_MasterAttributeMapping.Find(param.MasterAttributeMapping_Id);

                        if (search != null)
                        {
                            if (search.IsActive != param.IsActive)
                            {
                                search.IsActive = param.IsActive;

                                if (param.IsActive)
                                {
                                    msg.StatusMessage = "Data undeleted successfully";
                                }
                                else
                                {
                                    msg.StatusMessage = "Data deleted successfully";
                                }
                            }
                            else
                            {
                                search.Status = param.Status;
                                search.SupplierMasterAttribute = param.SupplierMasterAttribute;

                                msg.StatusMessage = "Data updated successfully";
                            }

                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = param.Edit_User;

                        }

                        context.SaveChanges();

                        msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        return msg;
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating master attribute mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = context.m_MasterAttributeValueMapping.Find(param.MasterAttributeValueMapping_Id);

                    if (search != null)
                    {
                        search.IsActive = param.IsActive;
                        search.Edit_Date = DateTime.Now;
                        search.Edit_User = param.Edit_User;
                        search.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;

                        context.SaveChanges();

                    }
                    else
                    {
                        m_MasterAttributeValueMapping newObj = new m_MasterAttributeValueMapping();
                        newObj.MasterAttributeMapping_Id = param.MasterAttributeMapping_Id;
                        newObj.MasterAttributeValueMapping_Id = param.MasterAttributeValueMapping_Id ?? Guid.NewGuid();
                        newObj.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;
                        newObj.SystemMasterAttributeValue_Id = param.SystemMasterAttributeValue_Id;
                        newObj.IsActive = param.IsActive;
                        newObj.Create_Date = DateTime.Now;
                        newObj.Create_User = param.Create_User;
                        context.m_MasterAttributeValueMapping.Add(newObj);
                        context.SaveChanges();
                    }


                    return new DataContracts.DC_Message
                    {
                        StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                        StatusMessage = "Data saved successfully."
                    };
                }


            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating master attribute value mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Activity Mapping
        public DataContracts.Mapping.DC_Acitivity_SupplierProductMapping GetActivitySupplierProductMappingById(Guid ActivitySupplierProductMapping_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;
                    //System.Linq.IQueryable<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> prodMapList;

                    var result = (from a in prodMapSearch
                                  where a.ActivitySupplierProductMapping_Id == ActivitySupplierProductMapping_Id
                                  orderby a.SupplierName
                                  select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                  {
                                      Activity_ID = a.Activity_ID,
                                      Supplier_ID = a.Supplier_ID,
                                      ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                      SupplierCode = a.SupplierCode,
                                      SupplierName = a.SupplierName,
                                      SuplierProductCode = a.SuplierProductCode,
                                      SupplierProductType = a.SupplierProductType,
                                      SupplierType = a.SupplierType,
                                      SupplierLocationId = a.SupplierLocationId,
                                      SupplierLocationName = a.SupplierLocationName,
                                      SupplierCountryName = a.SupplierCountryName,
                                      SupplierCityName = a.SupplierCityName,
                                      SupplierCountryCode = a.SupplierCountryCode,
                                      SupplierCityCode = a.SupplierCityCode,
                                      SupplierStateName = a.SupplierStateName,
                                      SupplierStateCode = a.SupplierStateCode,
                                      SupplierCityIATACode = a.SupplierCityIATACode,
                                      Duration = a.Duration,
                                      SupplierProductName = a.SupplierProductName,
                                      SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                      Introduction = a.Introduction,
                                      Conditions = a.Conditions,
                                      Inclusions = a.Inclusions,
                                      Exclusions = a.Exclusions,
                                      AdditionalInformation = a.AdditionalInformation,
                                      DeparturePoint = a.DeparturePoint,
                                      TicketingDetails = a.TicketingDetails,
                                      Currency = a.Currency,
                                      DepartureTime = a.DepartureTime,
                                      DepartureDate = a.DepartureDate,
                                      DateFrom = a.DateFrom,
                                      DateTo = a.DateTo,
                                      BlockOutDateFrom = a.BlockOutDateFrom,
                                      BlockOutDateTo = a.BlockOutDateTo,
                                      OptionTitle = a.OptionTitle,
                                      OptionCode = a.OptionCode,
                                      OptionDescription = a.OptionDescription,
                                      TourActivityLangauageCode = a.TourActivityLangauageCode,
                                      ProductDescription = a.ProductDescription,
                                      TourActivityLanguage = a.TourActivityLanguage,
                                      ImgURL = a.ImgURL,
                                      ProductValidFor = a.ProductValidFor,
                                      Address = a.Address,
                                      Latitude = a.Latitude,
                                      Longitude = a.Longitude,
                                      DayPattern = a.DayPattern,
                                      Theme = a.Theme,
                                      Distance = a.Distance,
                                      SupplierTourType = a.SupplierTourType,
                                      MappingStatus = a.MappingStatus,
                                      Create_Date = a.Create_Date,
                                      Edit_Date = a.Edit_Date,
                                      Create_User = a.Create_User,
                                      Edit_User = a.Edit_User,
                                      MapID = a.MapID
                                  }).FirstOrDefault();
                    if (result != null && !string.IsNullOrWhiteSpace(result.SupplierCountryName) && !string.IsNullOrWhiteSpace(result.SupplierCityName))
                    {
                        var resultCity = context.m_CityMapping.Where(a => (a.Supplier_Id == result.Supplier_ID) && (a.Status == "MAPPED")).ToList();
                        if (resultCity != null && resultCity.Count > 0)
                        {
                            var citymaster = (from ct in resultCity where ct.CityName == result.SupplierCityName select ct).FirstOrDefault();
                            if (citymaster != null)
                            {
                                var mastercityName = (from a in context.m_CityMaster where a.City_Id == citymaster.City_Id select a.Name).FirstOrDefault();
                                if (mastercityName != null)
                                    result.SystemCityName = mastercityName;
                            }
                        }


                    }
                    return result;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearch(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a; //where a.Accommodation_Id == null 

                    if (obj.ActivitySupplierProductMappling_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.ActivitySupplierProductMapping_Id == obj.ActivitySupplierProductMappling_Id
                                        select a;
                    }
                    if (obj.Activity_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Activity_ID == obj.Activity_ID
                                        select a;
                    }
                    if (obj.Supplier_ID != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_ID == obj.Supplier_ID
                                        select a;
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SystemCityName))
                    {
                        var distCityMapping = (from a in context.m_CityMapping select new { a.City_Id, a.CityName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCityMapping on a.SupplierCityName.ToLower().Trim() equals mas.CityName.ToLower().Trim()
                                        join mct in context.m_CityMaster on mas.City_Id equals mct.City_Id
                                        where mct.Name == obj.SystemCityName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToLower().Trim() == obj.MappingStatus.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierName == obj.SupplierName
                                        select a;
                    }


                    if (!string.IsNullOrWhiteSpace(obj.SupplierCountryName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCountryName.ToLower().Trim() == obj.SupplierCountryName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierCityName))
                    {

                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierCityName.ToLower().Trim() == obj.SupplierCityName.ToLower().Trim()
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SupplierProductName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.SupplierProductName)
                                        select a;
                    }
                    //Bykeyword
                    if (!string.IsNullOrWhiteSpace(obj.KeyWord))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.SupplierProductName.Contains(obj.KeyWord) || a.Inclusions.Contains(obj.KeyWord) || a.Exclusions.Contains(obj.KeyWord)
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMap = (from a in prodMapSearch
                                   join act in context.Activities on a.Activity_ID equals act.Acivity_Id into ja
                                   from jda in ja.DefaultIfEmpty()
                                   orderby a.SupplierName
                                   select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                   {
                                       Activity_ID = a.Activity_ID,
                                       Supplier_ID = a.Supplier_ID,
                                       ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                       SupplierCode = a.SupplierCode,
                                       SupplierName = a.SupplierName,
                                       SuplierProductCode = a.SuplierProductCode,
                                       SupplierProductType = a.SupplierProductType,
                                       SupplierType = a.SupplierType,
                                       SupplierLocationId = a.SupplierLocationId,
                                       SupplierLocationName = a.SupplierLocationName,
                                       SupplierCountryName = a.SupplierCountryName,
                                       SupplierCityName = a.SupplierCityName,
                                       SupplierCountryCode = a.SupplierCountryCode,
                                       SupplierCityCode = a.SupplierCityCode,
                                       SupplierStateName = a.SupplierStateName,
                                       SupplierStateCode = a.SupplierStateCode,
                                       SupplierCityIATACode = a.SupplierCityIATACode,
                                       Duration = a.Duration,
                                       SupplierProductName = a.SupplierProductName,
                                       SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                       Introduction = a.Introduction,
                                       Conditions = a.Conditions,
                                       Inclusions = a.Inclusions,
                                       Exclusions = a.Exclusions,
                                       AdditionalInformation = a.AdditionalInformation,
                                       DeparturePoint = a.DeparturePoint,
                                       TicketingDetails = a.TicketingDetails,
                                       Currency = a.Currency,
                                       DepartureTime = a.DepartureTime,
                                       DepartureDate = a.DepartureDate,
                                       DateFrom = a.DateFrom,
                                       DateTo = a.DateTo,
                                       BlockOutDateFrom = a.BlockOutDateFrom,
                                       BlockOutDateTo = a.BlockOutDateTo,
                                       OptionTitle = a.OptionTitle,
                                       OptionCode = a.OptionCode,
                                       OptionDescription = a.OptionDescription,
                                       TourActivityLangauageCode = a.TourActivityLangauageCode,
                                       ProductDescription = a.ProductDescription,
                                       TourActivityLanguage = a.TourActivityLanguage,
                                       ImgURL = a.ImgURL,
                                       ProductValidFor = a.ProductValidFor,
                                       Address = a.Address,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       DayPattern = a.DayPattern,
                                       Theme = a.Theme,
                                       Distance = a.Distance,
                                       SupplierTourType = a.SupplierTourType,
                                       MappingStatus = a.MappingStatus,
                                       Create_Date = a.Create_Date,
                                       Edit_Date = a.Edit_Date,
                                       Create_User = a.Create_User,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       ourRef = jda.CommonProductID,
                                       SystemCountryName = jda.Country,
                                       SystemCityName = jda.City,
                                       CKIS_Master = jda.Product_Name,
                                       TotalCount = total
                                   }).Skip(skip ?? 0).Take(obj.PageSize ?? total);

                    var prodMapList = prodMap.ToList();

                    if (!string.IsNullOrWhiteSpace(obj.MappingStatus))
                    {
                        foreach (var item in prodMapList)
                        {
                            if (obj.MappingStatus.ToUpper().Trim() == "UNMAPPED" && !string.IsNullOrWhiteSpace(item.SupplierProductName))
                            {
                                if (item.SupplierCityName != null && item.SupplierCountryName != null)
                                {
                                    var _supplierProductName = item.SupplierProductName.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "");
                                    var searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim() == _supplierProductName.ToUpper().Trim()).FirstOrDefault();
                                    if (searchActivity == null)
                                    {
                                        searchActivity = context.Activities.Where(a => a.Country.ToUpper().Trim() == item.SupplierCountryName.ToUpper().Trim() && a.City.ToUpper().Trim() == item.SupplierCityName.ToUpper().Trim() && a.Product_Name.Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("activity", "").ToUpper().Trim().Contains(_supplierProductName.ToUpper().Trim())).FirstOrDefault();
                                    }

                                    if (searchActivity != null)
                                    {
                                        item.CKIS_Master = searchActivity.Product_Name;
                                        item.Activity_ID = searchActivity.Acivity_Id;
                                    }
                                }
                            }
                        }
                    }
                    return prodMapList;

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    // 1. Distinct Supplier form activity supplier product mapping
                    var DistSupplier = context.Activity_SupplierProductMapping.GroupBy(x => x.SupplierName).Select(x => x.First()).ToList();

                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;
                    //context.Activity_SupplierProductMapping.Where(a =>a.SupplierName.Contains(DistSupplier))

                    // 2. Fillter supplier with country name
                    // 3. Fillter supplier with city name




                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }

                    if (!string.IsNullOrWhiteSpace(obj.SystemCityName))
                    {
                        var distCityMapping = (from a in context.m_CityMapping select new { a.City_Id, a.CityName }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCityMapping on a.SupplierCityName.ToLower().Trim() equals mas.CityName.ToLower().Trim()
                                        join mct in context.m_CityMaster on mas.City_Id equals mct.City_Id
                                        where mct.Name == obj.SystemCityName
                                        select a;
                    }

                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.MappingStatus.ToUpper() != obj.StatusExcept.ToUpper()
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMapList = prodMapSearch.ToList();

                    var prodMap = (from a in prodMapList
                                   orderby a.SupplierName
                                   select new DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL
                                   {
                                       Supplier_ID = a.Supplier_ID,
                                       SupplierName = a.SupplierName,
                                   }).Skip(skip ?? 0).Take(obj.PageSize ?? total);

                    return prodMap.ToList();

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMapping(int PageNo, int PageSize, Guid Activity_Id, string Status)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;

                    if (Activity_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Activity_ID == Activity_Id
                                        select a;
                    }

                    if (Status.Trim().ToUpper() != "ALL")
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.MappingStatus ?? "UNMAPPED") == Status
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = PageSize * PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> prodMapList;

                    if (Activity_Id != Guid.Empty)
                    {
                        prodMapList = (from a in prodMapSearch
                                       join act in context.Activities on a.Activity_ID equals act.Acivity_Id
                                       where act.Acivity_Id == Activity_Id
                                       orderby a.SupplierName
                                       select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                       {
                                           Activity_ID = a.Activity_ID,
                                           Supplier_ID = a.Supplier_ID,
                                           ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                           SupplierCode = a.SupplierCode,
                                           SupplierName = a.SupplierName,
                                           SuplierProductCode = a.SuplierProductCode,
                                           SupplierProductType = a.SupplierProductType,
                                           SupplierType = a.SupplierType,
                                           SupplierLocationId = a.SupplierLocationId,
                                           SupplierLocationName = a.SupplierLocationName,
                                           SupplierCountryName = a.SupplierCountryName,
                                           SupplierCityName = a.SupplierCityName,
                                           SupplierCountryCode = a.SupplierCountryCode,
                                           SupplierCityCode = a.SupplierCityCode,
                                           SupplierStateName = a.SupplierStateName,
                                           SupplierStateCode = a.SupplierStateCode,
                                           SupplierCityIATACode = a.SupplierCityIATACode,
                                           Duration = a.Duration,
                                           SupplierProductName = a.SupplierProductName,
                                           SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                           Introduction = a.Introduction,
                                           Conditions = a.Conditions,
                                           Inclusions = a.Inclusions,
                                           Exclusions = a.Exclusions,
                                           AdditionalInformation = a.AdditionalInformation,
                                           DeparturePoint = a.DeparturePoint,
                                           TicketingDetails = a.TicketingDetails,
                                           Currency = a.Currency,
                                           DepartureTime = a.DepartureTime,
                                           DepartureDate = a.DepartureDate,
                                           DateFrom = a.DateFrom,
                                           DateTo = a.DateTo,
                                           BlockOutDateFrom = a.BlockOutDateFrom,
                                           BlockOutDateTo = a.BlockOutDateTo,
                                           OptionTitle = a.OptionTitle,
                                           OptionCode = a.OptionCode,
                                           OptionDescription = a.OptionDescription,
                                           TourActivityLangauageCode = a.TourActivityLangauageCode,
                                           ProductDescription = a.ProductDescription,
                                           TourActivityLanguage = a.TourActivityLanguage,
                                           ImgURL = a.ImgURL,
                                           ProductValidFor = a.ProductValidFor,
                                           Address = a.Address,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           DayPattern = a.DayPattern,
                                           Theme = a.Theme,
                                           Distance = a.Distance,
                                           SupplierTourType = a.SupplierTourType,
                                           MappingStatus = a.MappingStatus,
                                           Create_Date = a.Create_Date,
                                           Edit_Date = a.Edit_Date,
                                           Create_User = a.Create_User,
                                           Edit_User = a.Edit_User,
                                           MapID = a.MapID,
                                           SystemCountryName = act.Country,
                                           SystemCityName = act.City,
                                           ourRef = act.CommonProductID,
                                           CKIS_Master = act.Product_Name,
                                           TotalCount = total
                                       }).Skip(skip).Take(PageSize);
                    }
                    else
                    {

                        prodMapList = (from a in prodMapSearch
                                       orderby a.SupplierName
                                       select new DataContracts.Mapping.DC_Acitivity_SupplierProductMapping
                                       {
                                           Activity_ID = a.Activity_ID,
                                           Supplier_ID = a.Supplier_ID,
                                           ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                           SupplierCode = a.SupplierCode,
                                           SupplierName = a.SupplierName,
                                           SuplierProductCode = a.SuplierProductCode,
                                           SupplierProductType = a.SupplierProductType,
                                           SupplierType = a.SupplierType,
                                           SupplierLocationId = a.SupplierLocationId,
                                           SupplierLocationName = a.SupplierLocationName,
                                           SupplierCountryName = a.SupplierCountryName,
                                           SupplierCityName = a.SupplierCityName,
                                           SupplierCountryCode = a.SupplierCountryCode,
                                           SupplierCityCode = a.SupplierCityCode,
                                           SupplierStateName = a.SupplierStateName,
                                           SupplierStateCode = a.SupplierStateCode,
                                           SupplierCityIATACode = a.SupplierCityIATACode,
                                           Duration = a.Duration,
                                           SupplierProductName = a.SupplierProductName,
                                           SupplierDataLangaugeCode = a.SupplierDataLangaugeCode,
                                           Introduction = a.Introduction,
                                           Conditions = a.Conditions,
                                           Inclusions = a.Inclusions,
                                           Exclusions = a.Exclusions,
                                           AdditionalInformation = a.AdditionalInformation,
                                           DeparturePoint = a.DeparturePoint,
                                           TicketingDetails = a.TicketingDetails,
                                           Currency = a.Currency,
                                           DepartureTime = a.DepartureTime,
                                           DepartureDate = a.DepartureDate,
                                           DateFrom = a.DateFrom,
                                           DateTo = a.DateTo,
                                           BlockOutDateFrom = a.BlockOutDateFrom,
                                           BlockOutDateTo = a.BlockOutDateTo,
                                           OptionTitle = a.OptionTitle,
                                           OptionCode = a.OptionCode,
                                           OptionDescription = a.OptionDescription,
                                           TourActivityLangauageCode = a.TourActivityLangauageCode,
                                           ProductDescription = a.ProductDescription,
                                           TourActivityLanguage = a.TourActivityLanguage,
                                           ImgURL = a.ImgURL,
                                           ProductValidFor = a.ProductValidFor,
                                           Address = a.Address,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           DayPattern = a.DayPattern,
                                           Theme = a.Theme,
                                           Distance = a.Distance,
                                           SupplierTourType = a.SupplierTourType,
                                           MappingStatus = a.MappingStatus,
                                           Create_Date = a.Create_Date,
                                           Edit_Date = a.Edit_Date,
                                           Create_User = a.Create_User,
                                           Edit_User = a.Edit_User,
                                           MapID = a.MapID,
                                           TotalCount = total
                                       }).Skip(skip).Take(PageSize);
                    }

                    return prodMapList.ToList();
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message UpdateActivitySupplierProductMapping(List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> obj)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            string strStatus = string.Empty;
            foreach (var PM in obj)
            {
                if (PM.MappingStatus != "UNMAPPED")
                {
                    if (PM.ActivitySupplierProductMapping_Id == null || PM.Activity_ID == null) //|| PM.Supplier_Id == null
                    {
                        continue;
                    }
                }

                try
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var search = (from a in context.Activity_SupplierProductMapping
                                      where a.ActivitySupplierProductMapping_Id == PM.ActivitySupplierProductMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            strStatus = PM.MappingStatus.ToLower();
                            search.Activity_ID = PM.Activity_ID;
                            search.MappingStatus = PM.MappingStatus;
                            search.Edit_Date = PM.Edit_Date;
                            search.Edit_User = PM.Edit_User;

                            context.SaveChanges();
                        }


                    }
                }
                catch
                {
                    //_msg.StatusMessage = "Something went wrong. Please contact system administrator!";
                    //_msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                    throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                }
            }
            _msg.StatusMessage = "Activity " + strStatus + " successfully";
            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
            return _msg;
        }
        #endregion
    }
}
