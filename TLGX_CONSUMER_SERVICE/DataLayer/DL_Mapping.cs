using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;
using EntityFramework.Extensions;

namespace DataLayer
{
    public class DL_Mapping : IDisposable
    {
        public DL_UploadStaticData USD = new DL_UploadStaticData();
        DC_SupplierImportFile_Progress PLog = new DC_SupplierImportFile_Progress();
        public void Dispose()
        {
        }

        public void CallLogVerbose(Guid SupplierImportFile_Id, string Step, string Message)
        {
            DC_SupplierImportFile_VerboseLog obj = new DC_SupplierImportFile_VerboseLog()
            {
                SupplierImportFile_VerboseLog_Id = Guid.NewGuid(),
                SupplierImportFile_Id = SupplierImportFile_Id,
                Step = Step,
                Message = Message,
                TimeStamp = DateTime.Now
            };
            DataContracts.DC_Message RM = USD.AddStaticDataUploadVerboseLog(obj);
        }
        public bool DeleteSTGMappingTableIDs(Guid File_Id)
        {
            bool ret = false;
            try
            {
                if (File_Id != null && File_Id != Guid.Empty)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        DC_SupplierImportFileDetails_RQ RQ = new DC_SupplierImportFileDetails_RQ();
                        RQ.SupplierImportFile_Id = File_Id;
                        RQ.PageNo = 0;
                        RQ.PageSize = int.MaxValue;
                        var FileDetails = USD.GetStaticDataFileDetail(RQ);

                        string SupplierName = FileDetails[0].Supplier.ToString();
                        string Entity = FileDetails[0].Entity.ToString().ToUpper();

                        switch (Entity)
                        {
                            case "COUNTRY":
                                var stgCountries = (from y in context.stg_SupplierCountryMapping
                                                    where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                    select y).ToList();
                                context.stg_SupplierCountryMapping.RemoveRange(stgCountries);
                                context.SaveChanges();
                                break;
                            case "CITY":
                                var stgCities = (from y in context.stg_SupplierCityMapping
                                                 where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                 select y).ToList();
                                context.stg_SupplierCityMapping.RemoveRange(stgCities);
                                context.SaveChanges();
                                break;
                            case "HOTEL":
                                var stgHotel = (from y in context.stg_SupplierProductMapping
                                                where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                select y).ToList();
                                context.stg_SupplierProductMapping.RemoveRange(stgHotel);
                                context.SaveChanges();
                                break;
                            case "ROOMTYPE":
                                var stgRoomType = (from y in context.stg_SupplierHotelRoomMapping
                                                   where y.SupplierName.ToString().ToUpper() == SupplierName.ToString().ToUpper()
                                                   select y).ToList();
                                context.stg_SupplierHotelRoomMapping.RemoveRange(stgRoomType);
                                context.SaveChanges();
                                break;
                        }

                        var oldRecords = (from y in context.STG_Mapping_TableIds
                                          where y.File_Id == File_Id
                                          select y).ToList();
                        context.STG_Mapping_TableIds.RemoveRange(oldRecords);
                        context.SaveChanges();
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool AddSTGMappingTableIDs(List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj)
        {
            bool ret = false;
            if (lstobj.Count > 0)
            {
                Guid File_Id = Guid.Parse(lstobj[0].File_Id.ToString());
                try
                {
                    if (File_Id != null && File_Id != Guid.Empty)
                    {
                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            //var oldRecords = (from y in context.STG_Mapping_TableIds
                            //                  where y.File_Id == File_Id
                            //                  select y).ToList();
                            //context.STG_Mapping_TableIds.RemoveRange(oldRecords);
                            //context.SaveChanges();
                            List<DataLayer.STG_Mapping_TableIds> lstobjnew = new List<STG_Mapping_TableIds>();

                            lstobjnew = (from a in lstobj
                                         select new STG_Mapping_TableIds()
                            {
                                    STG_Mapping_Table_Id = Guid.NewGuid(),
                                    File_Id = File_Id,
                                             Mapping_Id = a.Mapping_Id,
                                             STG_Id = a.STG_Id,
                                             Batch = a.Batch
                                         }).ToList();

                            //foreach (DataContracts.STG.DC_STG_Mapping_Table_Ids obj in lstobj)
                            //{
                            //    //DataLayer.STG_Mapping_TableIds objnew = 
                            //    lstobjnew.Add(new STG_Mapping_TableIds()
                            //    {
                            //        STG_Mapping_Table_Id = Guid.NewGuid(),
                            //        File_Id = File_Id,
                            //        Mapping_Id = obj.Mapping_Id,
                            //        STG_Id = obj.STG_Id,
                            //        Batch = obj.Batch
                            //    });
                            //}
                            context.STG_Mapping_TableIds.AddRange(lstobjnew);
                            context.SaveChanges();
                            ret = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return ret;
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


        public string[] GetMappingHotelDataForTTFU(DataContracts.Masters.DC_Supplier obj)
        {
            string[] ret = new string[] { };
            if (obj != null)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Guid File_Id = new Guid();
                    File_Id = Guid.Parse(obj.File_Id.ToString());
                    string CurSupplierName = obj.Name;
                    Guid? CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;
                    List<DC_Accomodation_ProductMapping> lstProdMap = new List<DC_Accomodation_ProductMapping>();
                    prodMapList = (from s in context.STG_Mapping_TableIds.AsNoTracking()
                                   join stg in context.stg_SupplierProductMapping.AsNoTracking() on s.STG_Id equals stg.stg_AccoMapping_Id
                                   where s.File_Id == obj.File_Id
                                   select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                   {
                                       Accommodation_ProductMapping_Id = s.Mapping_Id ?? Guid.Empty
                                   });
                    lstProdMap = prodMapList.ToList();
                    List<string> str = new List<string>();
                    if (lstProdMap.Count > 0)
                    {
                        foreach (DC_Accomodation_ProductMapping rec in lstProdMap)
                        {
                            str.Add(rec.Accommodation_ProductMapping_Id.ToString());
                        }
                    }
                    ret = str.ToArray();
                }
            }
            return ret;
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
                                       FullAddress = (a.address != null ? a.address : (a.Street + ", " + a.Street2 + " " + a.Street3 + " " + a.Street4 + " " + a.PostCode + ", " + a.CityName + ", " + a.StateName + ", " + a.CountryName)),
                                       // FullAddress = (a.address ?? string.Empty) + ", " + (a.Street ?? string.Empty) + ", " + (a.Street2 ?? string.Empty) + " " + (a.Street3 ?? string.Empty) + " " + (a.Street4 ?? string.Empty) + " " + (a.PostCode ?? string.Empty) + ", " + (a.CityName ?? string.Empty) + ", " + (a.StateName ?? string.Empty) + ", " + (a.CountryName ?? string.Empty),
                                       SystemFullAddress = ((jd.FullAddress != string.Empty) ? jd.FullAddress :
                                       (jd.StreetNumber ?? string.Empty) + ", " + (jd.StreetName ?? string.Empty) + ", " + (jd.Street3 ?? string.Empty) + ", " + (jd.Street4 ?? string.Empty) + ", " + (jd.Street5 ?? string.Empty) + ", " + (jd.PostalCode ?? string.Empty) + ", " + (jd.city ?? string.Empty) + ", " + (jd.country ?? string.Empty)
                                       ),
                                       StarRating = a.StarRating
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
                                    //item.FullAddress = searchprod.FullAddress;
                                    item.SystemFullAddress = searchprod.FullAddress;
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

        public bool HotelTTFUTelephone(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            if (obj != null)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Guid File_Id = new Guid();
                    File_Id = Guid.Parse(obj.File_Id.ToString());
                    string CurSupplierName = obj.Name;
                    Guid? CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());
                    List<DC_Accomodation_ProductMapping> telephones = new List<DC_Accomodation_ProductMapping>();
                    telephones = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                  join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                  where a.Supplier_Id == CurSupplier_Id && s.File_Id == obj.File_Id
                                  select new DC_Accomodation_ProductMapping
                                  {
                                      Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                      TelephoneNumber = a.TelephoneNumber ?? "",
                                      TelephoneNumber_tx = a.TelephoneNumber_tx ?? ""
                                  }
                                  ).ToList();

                    telephones = telephones.Select(c =>
                    {
                        c.TelephoneNumber = CommonFunctions.GetDigits(c.TelephoneNumber, 8);
                        return c;
                    }).ToList();

                    telephones.RemoveAll(p => p.TelephoneNumber == p.TelephoneNumber_tx);
                    foreach (var telephone in telephones)
                    {
                        var search = (from a in context.Accommodation_ProductMapping
                                      where a.Accommodation_ProductMapping_Id == telephone.Accommodation_ProductMapping_Id
                                      select a).FirstOrDefault();
                        if (search != null)
                        {
                            search.TelephoneNumber_tx = telephone.TelephoneNumber;
                            context.SaveChanges();
                        }
                    }
                }
            }
            return ret;
        }

        public void CheckHotelAlreadyExist(Guid CurSupplier_Id, List<DataContracts.STG.DC_stg_SupplierProductMapping> stg, out List<DC_Accomodation_ProductMapping> updateMappingList, out List<DataContracts.STG.DC_stg_SupplierProductMapping> insertSTGList)
        {
            bool ret = false;

            using (ConsumerEntities context = new ConsumerEntities())
            {
                //var prodMapList = context.Accommodation_ProductMapping.AsNoTracking().Where(w => stg.Any(a => a.ProductId == w.SupplierProductReference)).Select(s => s.SupplierProductReference ).ToList();
                var accomap = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                               where a.Supplier_Id == CurSupplier_Id
                               select a);
                var toUpdate = (from a in accomap
                                    //join s in stg on a.SupplierProductReference equals s.ProductId
                                join s in context.stg_SupplierProductMapping.AsNoTracking() on
                                new { Supplier_Id = a.Supplier_Id, SupplierProductReference = a.SupplierProductReference }
                                equals new { Supplier_Id = s.Supplier_Id, SupplierProductReference = s.ProductId }
                                select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                {
                                    Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                    Accommodation_Id = a.Accommodation_Id,
                                    Supplier_Id = a.Supplier_Id,
                                    //SupplierId = a.SupplierId,
                                    //SupplierName = a.SupplierName,
                                    SupplierProductReference = a.SupplierProductReference,
                                    ProductName = s.ProductName,
                                    oldProductName = a.ProductName,
                                    //Street = a.Street,
                                    //Street2 = a.Street2,
                                    //Street3 = a.Street3,
                                    //Street4 = a.Street4,
                                    //CountryCode = a.CountryCode,
                                    //CountryName = a.CountryName,
                                    //CityCode = a.CityCode,
                                    //CityName = a.CityName,
                                    //StateCode = a.StateCode,
                                    //StateName = a.StateName,
                                    //PostCode = a.PostCode,
                                    //TelephoneNumber = a.TelephoneNumber,
                                    //Fax = a.Fax,
                                    //Email = a.Email,
                                    //Website = a.Website,
                                    //Latitude = a.Latitude,
                                    //Longitude = a.Longitude,
                                    Status = a.Status,
                                    //Create_Date = a.Create_Date,
                                    //Create_User = a.Create_User,
                                    //Edit_Date = DateTime.Now,
                                    //Edit_User = a.Edit_User,
                                    IsActive = (a.IsActive ?? true),
                                    //ProductId = a.SupplierProductReference,
                                    Remarks = a.Remarks,
                                    //MapId = a.MapId,
                                    //StarRating = a.StarRating,
                                    //Country_Id = a.Country_Id,
                                    //City_Id = a.City_Id,
                                    ActionType = (a.ProductName != s.ProductName) ? "UPDATE" : "",
                                    stg_AccoMapping_Id = (a.ProductName != s.ProductName) ? s.stg_AccoMapping_Id : Guid.Empty,
                                    Latitude_Tx = a.Latitude_Tx,
                                    Longitude_Tx = a.Longitude_Tx
                                }).ToList();


                insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductReference == w.ProductId)).ToList();
                updateMappingList = toUpdate.Where(w => w.ProductName != w.oldProductName).ToList();

                context.Dispose();
                #region "Commented Code"
                /*
                List<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapRes = new List<DC_Accomodation_ProductMapping>();
                foreach (DataContracts.STG.DC_stg_SupplierProductMapping curSTG in stg)
                {
                    string curSupplierProductReference = curSTG.ProductId;

                    prodMapRes.InsertRange(prodMapRes.Count, context.Accommodation_ProductMapping.AsNoTracking()
                        .Where(w => w.Supplier_Id == CurSupplier_Id && w.SupplierProductReference == curSupplierProductReference)
                        .Select
                    (a => new DC_Accomodation_ProductMapping
                    {
                        Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                        Accommodation_Id = a.Accommodation_Id,
                        Supplier_Id = a.Supplier_Id,
                        SupplierId = a.SupplierId,
                        SupplierName = a.SupplierName,
                        SupplierProductReference = a.SupplierProductReference,
                        ProductName = a.ProductName,
                        oldProductName = a.ProductName,
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
                        Edit_Date = DateTime.Now,
                        Edit_User = "TLGX_DataHandler",
                        IsActive = (a.IsActive ?? true),
                        ProductId = a.SupplierProductReference,
                        Remarks = a.Remarks,
                        MapId = a.MapId,
                        StarRating = a.StarRating,
                        Country_Id = a.Country_Id,
                        City_Id = a.City_Id
                    }
                    ));
                }

                prodMapRes = prodMapRes.Select(c =>
                {
                    c.ProductName = (stg
                    //.Where(s => (s.ProductId ?? s.ProductName) == (c.SupplierProductReference ?? c.ProductName) && s.Country_Id == c.Country_Id && s.City_Id == c.City_Id)
                    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                    .Where(s => (s.ProductId == c.SupplierProductReference) 
                    //&& ((s.CityCode == null) ? (s.CityName == c.CityName) : (s.CityCode == c.CityCode)) && ((s.CountryCode == null) ? (s.CountryName == c.CountryName) : (s.CountryCode == c.CountryCode))
                    )
                    .Select(s1 => s1.ProductName)
                    .FirstOrDefault()
                    ) ?? c.ProductName;
                    c.Edit_Date = DateTime.Now;
                    c.Edit_User = "TLGX_DataHandler";
                    c.ActionType = "UPDATE";
                    c.stg_AccoMapping_Id = (stg
                    //.Where(s => (s.ProductId ?? s.ProductName) == (c.SupplierProductReference ?? c.ProductName) && s.Country_Id == c.Country_Id && s.City_Id == c.City_Id)
                    .Where(s => (s.ProductId == c.SupplierProductReference) 
                    //&& ((s.CityCode == null) ? (s.CityName == c.CityName) : (s.CityCode == c.CityCode)) && ((s.CountryCode == null) ? (s.CountryName == c.CountryName) : (s.CountryCode == c.CountryCode))
                    )
                    .Select(s1 => s1.stg_AccoMapping_Id)
                    .FirstOrDefault()
                    );
                    return c;
                }).ToList();
                */
                #endregion
            }
        }
        public bool HotelMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;
            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            //List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            //DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            //fileRQ.SupplierImportFile_Id = File_Id;
            //file = staticdata.GetStaticDataFileDetail(fileRQ);
            if (obj != null)
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierProductMapping> clsSTGHotel = new List<DataContracts.STG.DC_stg_SupplierProductMapping>();
                List<DataContracts.STG.DC_stg_SupplierProductMapping> clsSTGHotelInsert = new List<DataContracts.STG.DC_stg_SupplierProductMapping>();
                List<DC_Accomodation_ProductMapping> clsMappingHotel = new List<DC_Accomodation_ProductMapping>();

                //CallLogVerbose(File_Id, "MAP", "Fetching Staged Hotels.");
                DataContracts.STG.DC_stg_SupplierProductMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierProductMapping_RQ();
                RQ.Supplier_Id = CurSupplier_Id;
                //RQ.PageNo = 0;
                //RQ.PageSize = int.MaxValue;
                clsSTGHotel = staticdata.GetSTGHotelData(RQ);


                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);
                //CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.");

                /*DC_Mapping_ProductSupplier_Search_RQ RQM = new DC_Mapping_ProductSupplier_Search_RQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQM.SupplierName = CurSupplierName;
                else
                    RQM.Supplier_Id = CurSupplier_Id;
                RQM.PageNo = 0;
                RQM.PageSize = int.MaxValue;
                RQM.CalledFromTLGX = "TLGX";
                clsMappingHotel = GetMappingHotelData(RQM);*/
                CheckHotelAlreadyExist(CurSupplier_Id, clsSTGHotel, out clsMappingHotel, out clsSTGHotelInsert);
                PLog.PercentageValue = 26;
                USD.AddStaticDataUploadProcessLog(PLog);

                //CallLogVerbose(File_Id, "MAP", "Updating Existing Hotels.");
                //clsMappingHotel = clsMappingHotel.Select(c =>
                //{
                //    c.ProductName = (clsSTGHotel
                //    //.Where(s => (s.ProductId ?? s.ProductName) == (c.SupplierProductReference ?? c.ProductName) && s.Country_Id == c.Country_Id && s.City_Id == c.City_Id)
                //    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                //    .Where(s => (s.ProductId == c.SupplierProductReference) && ((s.CityCode == null) ? (s.CityName == c.CityName) : (s.CityCode == c.CityCode)) && ((s.CountryCode == null) ? (s.CountryName == c.CountryName) : (s.CountryCode == c.CountryCode)))
                //    .Select(s1 => s1.ProductName)
                //    .FirstOrDefault()
                //    ) ?? c.ProductName;
                //    c.Edit_Date = DateTime.Now;
                //    c.Edit_User = "TLGX_DataHandler";
                //    c.ActionType = "UPDATE";
                //    c.stg_AccoMapping_Id = (clsSTGHotel
                //    //.Where(s => (s.ProductId ?? s.ProductName) == (c.SupplierProductReference ?? c.ProductName) && s.Country_Id == c.Country_Id && s.City_Id == c.City_Id)
                //    .Where(s => (s.ProductId == c.SupplierProductReference) && ((s.CityCode == null) ? (s.CityName == c.CityName) : (s.CityCode == c.CityCode)) && ((s.CountryCode == null) ? (s.CountryName == c.CountryName) : (s.CountryCode == c.CountryCode)))
                //    .Select(s1 => s1.stg_AccoMapping_Id)
                //    .FirstOrDefault()
                //    );
                //    return c;
                //}).ToList();

                List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_AccoMapping_Id != null && a.ActionType == "UPDATE"
                    && (a.stg_AccoMapping_Id ?? Guid.Empty) != Guid.Empty
                //&& a.stg_AccoMapping_Id == (clsSTGHotel
                //    .Where(s => s.ProductId == a.SupplierProductReference)
                //    .Select(s1 => s1.stg_AccoMapping_Id)
                //    .FirstOrDefault())
                ).Select
                   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                   {
                       STG_Mapping_Table_Id = Guid.NewGuid(),
                       File_Id = obj.File_Id,
                       STG_Id = g.stg_AccoMapping_Id,
                       Mapping_Id = g.Accommodation_ProductMapping_Id,
                       Batch = obj.CurrentBatch ?? 0
                   }));

                //CallLogVerbose(File_Id, "MAP", "Checking for New Hotels in File.");
                //clsSTGHotelInsert = clsSTGHotel.Where(p => !clsMappingHotel.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                // && (
                //    p2.SupplierProductReference == p.ProductId
                //    && ((p2.CityCode == null) ? (p2.CityName == p.CityName) : (p2.CityCode == p.CityCode))
                //    && ((p2.CountryCode == null) ? (p2.CountryName == p.CountryName) : (p2.CountryCode == p.CountryCode))
                //    /*&& 
                //    (((p2.ProductName ?? string.Empty).ToString().Trim().ToUpper() == (p.ProductName ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.PostCode ?? string.Empty).ToString().Trim().ToUpper() == (p.PostalCode ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.StateName ?? string.Empty).ToString().Trim().ToUpper() == (p.StateName ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.Country_Id ?? Guid.Empty) == (p.Country_Id ?? Guid.Empty)))
                //    && (((p2.City_Id ?? Guid.Empty) == (p.City_Id ?? Guid.Empty)))*/
                //))).ToList();

                //CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.");
                clsSTGHotel.RemoveAll(p => clsSTGHotelInsert.Any(p2 => (p2.stg_AccoMapping_Id == p.stg_AccoMapping_Id)));
                clsMappingHotel.RemoveAll(p => p.ProductName == p.oldProductName);
                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);

                //CallLogVerbose(File_Id, "MAP", "Inserting New Hotels.");
                clsMappingHotel.InsertRange(clsMappingHotel.Count, clsSTGHotelInsert.Select
                    (g => new DC_Accomodation_ProductMapping
                    {
                        Accommodation_ProductMapping_Id = Guid.NewGuid(),
                        Accommodation_Id = null,
                        SupplierProductReference = g.ProductId,
                        ProductName = g.ProductName,
                        ProductId = g.ProductId,

                        Country_Id = g.Country_Id,
                        City_Id = g.City_Id,
                        CountryName = g.CountryName,
                        CountryCode = g.CountryCode,
                        CityCode = g.CityCode,
                        CityName = g.CityName,
                        StateCode = g.StateCode,
                        StateName = g.StateName,

                        Supplier_Id = CurSupplier_Id,
                        SupplierName = g.SupplierName,
                        Status = "UNMAPPED",
                        Create_Date = DateTime.Now,
                        Create_User = "TLGX_DataHandler",
                        Edit_Date = null,
                        Edit_User = null,
                        MapId = null,
                        Latitude = g.Latitude,
                        Longitude = g.Longitude,
                        Email = g.Email,
                        Fax = g.Fax,
                        Google_Place_Id = g.Google_Place_Id,
                        PostCode = g.PostalCode,
                        Street = (g.Address == null ? (g.StreetNo + " " + g.StreetName) : ""),
                        //Street2 = (g.Address == null ? g.Street2 : (g.StreetNo + " " + g.StreetName + " " + g.Street2)),
                        Street2 = (g.Address == null ? g.Street2 : ""),
                        Street3 = (g.Address == null ? g.Street3 : ""),
                        Street4 = (g.Street4 ?? "") + " " + (g.Street5 ?? ""),
                        StarRating = g.StarRating,
                        SupplierId = g.SupplierId,
                        TelephoneNumber = g.TelephoneNumber,
                        TelephoneNumber_tx = (g.TelephoneNumber != null) ? CommonFunctions.GetDigits(g.TelephoneNumber, 8) : g.TelephoneNumber,
                        Website = g.Website,
                        ActionType = "INSERT",
                        stg_AccoMapping_Id = g.stg_AccoMapping_Id,
                        FullAddress = g.Address == null ? ((g.StreetNo ?? "") + (((g.StreetNo ?? "") != "") ? ", " : "")
                                       + (g.StreetName ?? "") + (((g.StreetName ?? "") != "") ? ", " : "")
                                       + (g.Street2 ?? "") + (((g.Street2 ?? "") != "") ? ", " : "")
                                       + (g.Street3 ?? "") + (((g.Street3 ?? "") != "") ? ", " : "")
                                       + (g.Street4 ?? "") + (((g.Street4 ?? "") != "") ? ", " : "")
                                       + (g.Street5 ?? "") + (((g.Street5 ?? "") != "") ? ", " : "")
                                       + (g.PostalCode ?? "") + (((g.PostalCode ?? "") != "") ? ", " : "")) : g.Address
                                       + (g.PostalCode ?? "") + (((g.PostalCode ?? "") != "") ? ", " : "")
                        ,
                        Latitude_Tx = CommonFunctions.LatLongTX(g.Latitude),
                        Longitude_Tx = CommonFunctions.LatLongTX(g.Longitude),
                        Remarks = "" //DictionaryLookup(mappingPrefix, "Remarks", stgPrefix, "")
                    }));

                lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_AccoMapping_Id != null && a.ActionType == "INSERT"
                    && (a.stg_AccoMapping_Id ?? Guid.Empty) != Guid.Empty)
               .Select
                  (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                  {
                      STG_Mapping_Table_Id = Guid.NewGuid(),
                      File_Id = obj.File_Id,
                      STG_Id = g.stg_AccoMapping_Id,
                      Mapping_Id = g.Accommodation_ProductMapping_Id,
                      Batch = obj.CurrentBatch ?? 0
                  }));
                bool idinsert = AddSTGMappingTableIDs(lstobj);

                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.");

                var cities = clsMappingHotel.Select(x => ((x.CityCode ?? "") + (x.CityName ?? "")).ToUpper()).Distinct();

                //foreach(string city in cities)
                //{
                //    using (ConsumerEntities context = new ConsumerEntities())
                //    {
                //        var CityMapping = context.m_CityMapping.Select(s => s).AsQueryable();
                //        var search = (from a in CityMapping
                //                      where a.Supplier_Id == CurSupplier_Id 
                //                      && ((a.CityCode ?? "") + (a.CityName ?? "")).ToUpper() == city
                //                      select a).FirstOrDefault();
                //        if (search != null)
                //        {
                //            if (search.ListedService.IndexOf("H |") < 0)
                //            {
                //                search.ListedService = (search.ListedService ?? "") + " H |";
                //                context.SaveChanges();
                //            }
                //        }
                //    }
                //}

                if (clsMappingHotel.Count > 0)
                {
                    ret = UpdateAccomodationProductMapping(clsMappingHotel);
                    /*if (obj.CurrentBatch == 1)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.FinalStatus = file[0].STATUS;
                        objStat.TotalRows = clsMappingHotel.Count;
                        objStat.Process_Date = DateTime.Now;
                        objStat.Process_User = file[0].PROCESS_USER;
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                    }*/
                }

            }
            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch " + (obj.CurrentBatch ?? 0).ToString());
            return ret;
        }

        public int GetSTGMappingIDTableCount(DC_SupplierImportFileDetails file)
        {
            int ret = 0;
            Guid File_Id = file.SupplierImportFile_Id;
            Guid Supplier_Id = file.Supplier_Id;
            using (ConsumerEntities context = new ConsumerEntities())
            {
                List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();
                IQueryable<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT1;
                int i = 0;
                if (file.Entity.ToUpper().Trim() == "HOTEL")
                {
                    //var chkcount = (from a in context.Accommodation_ProductMapping
                    //                where a.Supplier_Id == Supplier_Id
                    //                && (a.Status == "UNMAPPED" || a.Accommodation_Id == null)
                    //                orderby a.Create_Date descending
                    //                let rank = (i+ 1)
                    //                select new
                    //                {
                    //                    i = rank,
                    //                    id = a.Accommodation_ProductMapping_Id,
                    //                    batch = (rank / 250) + 1
                    //                }).ToList();
                    lstSMT1 = (from a in context.Accommodation_ProductMapping
                               where a.Supplier_Id == Supplier_Id
                               && (a.Status == "UNMAPPED" || a.Accommodation_Id == null)
                               select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                               {
                                   STG_Mapping_Table_Id = Guid.NewGuid(),
                                   File_Id = File_Id,
                                   Mapping_Id = a.Accommodation_ProductMapping_Id,
                                   STG_Id = null,
                                   //Batch = (Rank / 250) + 1
                               });//.ToList();



                }
                else if (file.Entity.ToUpper().Trim() == "COUNTRY")
                {
                    lstSMT1 = (from a in context.m_CountryMapping
                               where a.Supplier_Id == Supplier_Id
                               && (a.Status == "UNMAPPED" || a.Country_Id == null)
                               select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                               {
                                   STG_Mapping_Table_Id = Guid.NewGuid(),
                                   File_Id = File_Id,
                                   Mapping_Id = a.CountryMapping_Id,
                                   STG_Id = null
                               });//.ToList();
                }
                else if (file.Entity.ToUpper().Trim() == "CITY")
                {
                    lstSMT1 = (from a in context.m_CityMapping
                               where a.Supplier_Id == Supplier_Id
                               && (a.Status == "UNMAPPED" || a.City_Id == null)
                               select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                               {
                                   STG_Mapping_Table_Id = Guid.NewGuid(),
                                   File_Id = File_Id,
                                   Mapping_Id = a.CityMapping_Id,
                                   STG_Id = null
                               });//.ToList();
                }
                else if (file.Entity.ToUpper().Trim() == "ROOMTYPE")
                {
                    lstSMT1 = (from a in context.Accommodation_SupplierRoomTypeMapping
                               where a.Supplier_Id == Supplier_Id
                               && (a.MappingStatus == "UNMAPPED" || a.Accommodation_RoomInfo_Id == null)
                               select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                               {
                                   STG_Mapping_Table_Id = Guid.NewGuid(),
                                   File_Id = File_Id,
                                   Mapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                   STG_Id = null
                               });//.ToList();
                }
                else
                {
                    lstSMT1 = (from a in context.m_CountryMapping
                               where a.Supplier_Id == Supplier_Id
                               && (a.Status == "UNMAPPED" || a.Country_Id == null)
                               select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                               {
                                   STG_Mapping_Table_Id = Guid.NewGuid(),
                                   File_Id = File_Id,
                                   Mapping_Id = a.CountryMapping_Id,
                                   STG_Id = null
                               });//.ToList();
                }
                //if (lstSMT1.Count() > 0)
                //{
                ret = lstSMT1.Count();
                /*var lstMst1 = lstSMT1.ToList()
                .Select((x, index) => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                {
                    Batch = (index + 1 / 250) + 1,
                    File_Id = x.File_Id,
                    Mapping_Id = x.Mapping_Id,
                    STG_Id = x.STG_Id,
                    STG_Mapping_Table_Id = x.STG_Mapping_Table_Id
                }).ToList();                    

                var lstMst2 = lstMst1.Select(c =>
                {
                    c.Batch = (c.Batch / 250) + 1;
                    return c;
                }).ToList();

                bool idinsert = DeleteSTGMappingTableIDs(File_Id);
                idinsert = AddSTGMappingTableIDs(lstMst2);

                if (idinsert)
                {
                    ret = (from a in context.STG_Mapping_TableIds.AsNoTracking()
                           where a.File_Id == File_Id
                           select a).Count();
                }*/
                //}
            }

            return ret;
        }
        //public List<DC_Accomodation_ProductMapping> UpdateHotelMappingStatus(DC_MappingMatch obj)

        public bool UpdateHotelMappingStatus(DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            bool DontAppend = false;
            int curConfigCount = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;
            bool Match_Direct_Master = obj.Match_Direct_Master;

            List<DC_Accomodation_ProductMapping> res = new List<DC_Accomodation_ProductMapping>();
            List<DC_Accomodation_ProductMapping> resmaster = new List<DC_Accomodation_ProductMapping>();
            IQueryable<Accommodation_ProductMapping> prodMapSearch;
            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if ((obj.FileMode ?? "ALL") != "ALL" && curPriority == 1)
                    //if (curPriority == 1)
                    {
                        List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                        DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();

                        lstSMT = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                  join j in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals j.Mapping_Id into jact
                                  from jdact in jact.DefaultIfEmpty()
                                  where a.Supplier_Id == curSupplier_Id && jdact.STG_Mapping_Table_Id == null
                                  && (a.Status == "UNMAPPED" || a.Accommodation_Id == null)
                                  select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                                  {
                                      STG_Mapping_Table_Id = Guid.NewGuid(),
                                      File_Id = obj.File_Id,
                                      Mapping_Id = a.Accommodation_ProductMapping_Id,
                                      STG_Id = null,
                                      Batch = obj.CurrentBatch ?? 1
                                  }).Take(250).ToList();
                        if (lstSMT.Count > 0)
                        {
                            bool idinsert = AddSTGMappingTableIDs(lstSMT);
                        }
                    }
                }
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    #region "Acco mapping query"
                    prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                         where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                         && a.Status.Trim().ToUpper() == "UNMAPPED"
                                         select a);

                    res = (from a in prodMapSearch
                           join act in context.m_CityMapping.AsNoTracking() on new { a.Supplier_Id, country = ((a.CountryName == null) ? a.CountryCode : a.CountryName).ToUpper().Trim(), city = ((a.CityName == null) ? a.CityCode : a.CityName).ToUpper().Trim() }
                           equals new { act.Supplier_Id, country = ((act.CountryName == null) ? act.CountryCode : act.CountryName).ToUpper().Trim(), city = ((act.CityName == null) ? act.CityCode : act.CityName).ToUpper().Trim() }
                           join mact in context.m_CityMaster.AsNoTracking() on new { country = (act.Country_Id ?? Guid.Empty) }
                           equals new { country = mact.Country_Id } into jact
                           from jdact in jact.DefaultIfEmpty()
                           where ((act.City_Id != null && (act.City_Id == jdact.City_Id)) || (act.City_Id == null && (act.CityName.ToUpper().Trim() == jdact.Name.ToUpper().Trim())))
                           select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                           {
                               Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                               Accommodation_Id = a.Accommodation_Id,
                               Address_tx = a.address_tx,
                               CityCode = a.CityCode,
                               CityName = a.CityName,
                               CountryCode = a.CountryCode,
                               CountryName = a.CountryName,
                               Create_Date = a.Create_Date,
                               Create_User = a.Create_User,
                               Edit_Date = a.Edit_Date,
                               Edit_User = a.Edit_User,
                               Email = a.Email,
                               Fax = a.Fax,
                               Latitude = a.Latitude,
                               Longitude = a.Longitude,
                               IsActive = (a.IsActive ?? true),
                               MapId = a.MapId,
                               PostCode = a.PostCode,
                               ProductName = a.ProductName,
                               Remarks = a.Remarks,
                               StarRating = a.StarRating,
                               StateCode = a.StateCode,
                               StateName = a.StateName,
                               Status = a.Status,
                               Street = a.Street,
                               Street2 = a.Street2,
                               Street3 = a.Street3,
                               Street4 = a.Street4,
                               SupplierId = a.SupplierId,
                               SupplierName = a.SupplierName,
                               SupplierProductReference = a.SupplierProductReference,
                               TelephoneNumber = a.TelephoneNumber,
                               TelephoneNumber_tx = a.TelephoneNumber_tx,
                               Website = a.Website,
                               //Country_Id = (jdact.Country_Id == Guid.Empty) ? ((act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id) : jdact.Country_Id,
                               //City_Id = (jdact.City_Id == Guid.Empty) ? ((act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id) : jdact.City_Id,
                               Country_Id = (act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id,
                               City_Id = (act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id,
                               SystemCityName = jdact.Name,
                               SystemCountryName = jdact.CountryName,
                               Latitude_Tx = a.Latitude_Tx ?? "",
                               Longitude_Tx = a.Longitude_Tx ?? ""
                           }).ToList();
                    if (Match_Direct_Master)
                    {
                        resmaster = (from a in prodMapSearch
                                     join mact in context.m_CityMaster.AsNoTracking() on new { country = (a.Country_Id ?? Guid.Empty) }
                                     equals new { country = mact.Country_Id } into jact
                                     from jdact in jact.DefaultIfEmpty()
                                     where ((a.City_Id != null && (a.City_Id == jdact.City_Id)) || (a.City_Id == null && (a.CityName.ToUpper().Trim() == jdact.Name.ToUpper().Trim())))
                                     select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                     {
                                         Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                         Accommodation_Id = a.Accommodation_Id,
                                         Address_tx = a.address_tx,
                                         CityCode = a.CityCode,
                                         CityName = a.CityName,
                                         CountryCode = a.CountryCode,
                                         CountryName = a.CountryName,
                                         Create_Date = a.Create_Date,
                                         Create_User = a.Create_User,
                                         Edit_Date = a.Edit_Date,
                                         Edit_User = a.Edit_User,
                                         Email = a.Email,
                                         Fax = a.Fax,
                                         Latitude = a.Latitude,
                                         Longitude = a.Longitude,
                                         IsActive = (a.IsActive ?? true),
                                         MapId = a.MapId,
                                         PostCode = a.PostCode,
                                         ProductName = a.ProductName,
                                         Remarks = a.Remarks,
                                         StarRating = a.StarRating,
                                         StateCode = a.StateCode,
                                         StateName = a.StateName,
                                         Status = a.Status,
                                         Street = a.Street,
                                         Street2 = a.Street2,
                                         Street3 = a.Street3,
                                         Street4 = a.Street4,
                                         SupplierId = a.SupplierId,
                                         SupplierName = a.SupplierName,
                                         SupplierProductReference = a.SupplierProductReference,
                                         TelephoneNumber = a.TelephoneNumber,
                                         TelephoneNumber_tx = a.TelephoneNumber_tx,
                                         Website = a.Website,
                                         //Country_Id = (jdact.Country_Id == Guid.Empty) ? ((act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id) : jdact.Country_Id,
                                         //City_Id = (jdact.City_Id == Guid.Empty) ? ((act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id) : jdact.City_Id,
                                         Country_Id = (jdact.Country_Id == Guid.Empty) ? a.Country_Id : jdact.Country_Id,
                                         City_Id = (jdact.City_Id == Guid.Empty) ? a.City_Id : jdact.City_Id,
                                         SystemCityName = jdact.Name,
                                         SystemCountryName = jdact.CountryName,
                                         Latitude_Tx = a.Latitude_Tx ?? "",
                                         Longitude_Tx = a.Longitude_Tx ?? ""
                                     }).ToList();
                    }
                    #endregion
                }
                PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                PLog.SupplierImportFile_Id = obj.File_Id;
                PLog.Step = "MATCH";
                PLog.Status = "MATCHING";
                PLog.TotalBatch = totPriorities;

                foreach (int priority in obj.Priorities)
                {
                    var curAttributeVals = obj.lstConfigs.Where(a => a.Priority == priority).ToList();
                    curConfigCount = curAttributeVals.Count();
                        List<DC_SupplierImportAttributeValues> configs = curAttributeVals;

                    PLog.CurrentBatch = curPriority;
                    //obj.CurrentPriority = curPriority;                    
                    totConfigs = configs.Count;                    
                    //CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");
                    
                    curConfig = 0;
                    configWhere = "";

                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                        else
                            configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);

                    res.RemoveAll(p => p.Accommodation_Id != null && p.Accommodation_Id != Guid.Empty); //Guid.Empty

                    foreach (DC_Accomodation_ProductMapping curRes in res)
                    {
                        curConfig = 0;
                        using (ConsumerEntities context = new ConsumerEntities())
                        {
                            var acco = context.Accommodations.AsNoTracking().AsQueryable();//.Select(a => a);
                            foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                            {
                                curConfig = curConfig + 1;
                                string CurrConfig = "";
                                if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                    CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                                else
                                    CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();

                                if (CurrConfig == "COUNTRY")
                                    acco = acco.Where(c => (c.country ?? "") != "" && (curRes.SystemCountryName ?? "") != "" && c.country.ToUpper().Trim() == curRes.SystemCountryName.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "CITY")
                                    acco = acco.Where(c => (c.city ?? "") != "" && (curRes.SystemCityName ?? "") != "" && c.city.ToUpper().Trim() == curRes.SystemCityName.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "PostalCode".ToUpper())
                                    acco = acco.Where(c => (c.PostalCode ?? "") != "" && (curRes.PostCode ?? "") != "" && c.PostalCode.ToUpper().Trim() == curRes.PostCode.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "HotelName".ToUpper())
                                    acco = acco.Where(c => (c.HotelName ?? "") != "" && (curRes.ProductName ?? "") != "" && c.HotelName.ToUpper().Trim().Replace("HOTEL", "").Replace(c.country ?? "", "").Replace(c.city ?? "", "").Replace("  ", " ") == curRes.ProductName.ToUpper().Trim().Replace("HOTEL", "").Replace(curRes.CountryName ?? "", "").Replace(curRes.CityName ?? "", "").Replace("  ", " "));//.Select(a => a);
                                else if (CurrConfig == "LATITUDE")
                                    acco = acco.Where(c => (c.Latitude_Tx ?? "") != "" && (curRes.Latitude_Tx ?? "") != "" 
                                    && (c.Latitude_Tx).ToString().Trim() == (curRes.Latitude_Tx).ToString().Trim()
                                    && (c.Longitude_Tx).ToString().Trim() == (curRes.Longitude_Tx).ToString().Trim()).Select(a => a);
                                else if (CurrConfig == "GOOGLE_PLACE_ID")
                                    acco = acco.Where(c => (c.Google_Place_Id ?? "") != "" && (curRes.Google_Place_Id ?? "") != "" && c.Google_Place_Id.ToUpper().Trim() == curRes.Google_Place_Id.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "Address_Tx".ToUpper())
                                    acco = acco.Where(c => (c.Address_Tx ?? "") != "" && (curRes.Address_tx ?? "") != "" && c.Address_Tx.ToUpper().Trim() == curRes.Address_tx.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                                    acco = acco.Where(c => (c.Telephone_Tx ?? "") != "" && (curRes.TelephoneNumber_tx ?? "") != "" && c.Telephone_Tx.ToUpper().Trim() == curRes.TelephoneNumber_tx.ToUpper().Trim());//.Select(a => a);
                                else if (CurrConfig == "CompanyHotelID".ToUpper())
                                    acco = acco.Where(c => (c.CompanyHotelID ?? 0) != 0 && (curRes.SupplierProductReference ?? "") != "" && c.CompanyHotelID.ToString().ToUpper().Trim() == curRes.SupplierProductReference.ToUpper().Trim());

                                
                            }
                            curRes.Accommodation_Id = acco.Select(a => a.Accommodation_Id).FirstOrDefault();
                            if (curRes.Accommodation_Id != null && curRes.Accommodation_Id != Guid.Empty)
                            {
                                context.Accommodation_ProductMapping.AsNoTracking().Where(x => x.Accommodation_ProductMapping_Id == curRes.Accommodation_ProductMapping_Id)
                                           .Update(t => new Accommodation_ProductMapping()
                                           {
                                               MatchedBy = curPriority - 1,
                                               Status = "REVIEW",
                                               Accommodation_Id = curRes.Accommodation_Id
                                           });
                            }

                        }
                    }
                    int toupdate = 0;
                    toupdate = res.Where(p => p.Accommodation_Id != Guid.Empty).Count();
                    if (totPriorities == curPriority)
                    {
                        PLog.PercentageValue = 70;
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }
                    if (Match_Direct_Master)
                    {
                        resmaster.RemoveAll(p => res.Where(w => w.Accommodation_Id != null && w.Accommodation_Id != Guid.Empty).Any(a => a.Accommodation_ProductMapping_Id == p.Accommodation_ProductMapping_Id));
                        resmaster.RemoveAll(p => p.Accommodation_Id != null && p.Accommodation_Id != Guid.Empty); //Guid.Empty
                        foreach (DC_Accomodation_ProductMapping curRes in resmaster)
                        {
                            curConfig = 0;
                            
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                var acco = context.Accommodations.AsQueryable();//.Select(a => a);
                                //string sql = "select top 1 Accommodation_ID from Accommodation WHERE ";
                                foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                                {
                                    DontAppend = false;
                                    curConfig = curConfig + 1;
                                    string CurrConfig = "";
                                    if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                        CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                                    else
                                        CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();

                                    if (CurrConfig == "COUNTRY")
                                    {
                                        //sql = sql + "(ISNULL(country, '') != '' and ltrim(rtrim(upper(country))) = '" + (curRes.SystemCountryName ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.country ?? "") != "" && (curRes.SystemCountryName ?? "") != "" && c.country.ToUpper().Trim() == curRes.SystemCountryName.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "CITY")
                                    {
                                        //sql = sql + "(ISNULL(city, '') != '' and ltrim(rtrim(upper(city))) = '" + (curRes.SystemCityName ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.city ?? "") != "" && (curRes.SystemCityName ?? "") != "" && c.city.ToUpper().Trim() == curRes.SystemCityName.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "PostalCode".ToUpper())
                                    {
                                        //sql = sql + "(ISNULL(PostalCode, '') != '' and ltrim(rtrim(upper(PostalCode))) = '" + (curRes.PostCode ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.PostalCode ?? "") != "" && (curRes.PostCode ?? "") != "" && c.PostalCode.ToUpper().Trim() == curRes.PostCode.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "HotelName".ToUpper())
                                    {
                                        //sql = sql + "(ISNULL(HotelName, '') != '' and replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(hotelname))), ' ',''), 'HOTEL',''), 'APARTMENT',''), ltrim(rtrim(upper(city))),''), ltrim(rtrim(upper(country))),''), '&',''), 'AND',''), 'THE',''), '-',''), '_','') = '"
                                        //    + (curRes.ProductName ?? "").ToUpper().Trim().Replace("HOTEL", "").Replace(curRes.CountryName ?? "", "").Replace(curRes.CityName ?? "", "").Replace("  ", " ").Replace("APARTMENT", " ").Replace("&", " ").Replace("AND", " ").Replace("THE", " ").Replace("-", " ").Replace("_", " ").Replace("'", "''") + "')";
                                        acco = acco.Where(c => (c.HotelName ?? "") != "" && (curRes.ProductName ?? "") != "" && c.HotelName.ToUpper().Trim().Replace("HOTEL", "").Replace(c.country ?? "", "").Replace(c.city ?? "", "").Replace("  ", " ") == curRes.ProductName.ToUpper().Trim().Replace("HOTEL", "").Replace(curRes.CountryName ?? "", "").Replace(curRes.CityName ?? "", "").Replace("  ", " "));//.Select(a => a);
                                    }
                                    else if (CurrConfig == "LONGITUDE")
                                    {
                                        DontAppend = true;
                                    }
                                    else if (CurrConfig == "LATITUDE")
                                    {
                                        //sql = sql + "(ISNULL(Latitude_Tx, '') != '' and ISNULL(Longitude_Tx, '') != '' and ltrim(rtrim(upper(Latitude_Tx))) = '" + (curRes.Latitude_Tx ?? "").ToUpper().Trim()
                                        //    + "' and ltrim(rtrim(upper(Longitude_Tx))) = '" + (curRes.Longitude_Tx ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.Latitude_Tx ?? "") != "" && (curRes.Latitude_Tx ?? "") != ""
                                        && (c.Latitude_Tx).ToString().Trim() == (curRes.Latitude_Tx).ToString().Trim()
                                        && (c.Longitude_Tx).ToString().Trim() == (curRes.Longitude_Tx).ToString().Trim()).Select(a => a);
                                    }
                                    else if (CurrConfig == "GOOGLE_PLACE_ID")
                                    {
                                        //sql = sql + "(ISNULL(Google_Place_Id, '') != '' and ltrim(rtrim(upper(Google_Place_Id))) = '" + (curRes.Google_Place_Id ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.Google_Place_Id ?? "") != "" && (curRes.Google_Place_Id ?? "") != "" && c.Google_Place_Id.ToUpper().Trim() == curRes.Google_Place_Id.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "Address_Tx".ToUpper())
                                    {
                                       // sql = sql + "(ISNULL(Address_Tx, '') != '' and ltrim(rtrim(upper(Address_Tx))) = '" + (curRes.Address_tx ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.Address_Tx ?? "") != "" && (curRes.Address_tx ?? "") != "" && c.Address_Tx.ToUpper().Trim() == curRes.Address_tx.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                                    {
                                        //sql = sql + "(ISNULL(Telephone_Tx, '') != '' and ltrim(rtrim(upper(Telephone_Tx))) = '" + (curRes.TelephoneNumber_tx ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.Telephone_Tx ?? "") != "" && (curRes.TelephoneNumber_tx ?? "") != "" && c.Telephone_Tx.ToUpper().Trim() == curRes.TelephoneNumber_tx.ToUpper().Trim());//.Select(a => a);
                                    }
                                    else if (CurrConfig == "CompanyHotelID".ToUpper())
                                    {
                                        //sql = sql + "(ISNULL(CompanyHotelID, '') != '' and ltrim(rtrim(upper(CompanyHotelID))) = '" + (curRes.SupplierProductReference ?? "").ToUpper().Trim() + "')";
                                        acco = acco.Where(c => (c.CompanyHotelID ?? 0) != 0 && (curRes.SupplierProductReference ?? "") != "" && c.CompanyHotelID.ToString().ToUpper().Trim() == curRes.SupplierProductReference.ToUpper().Trim());
                                    }
                                    //if (curConfig != curConfigCount && (!DontAppend))
                                    //{
                                    //    sql = sql + " AND ";
                                    //}
                                }
                                //var sqlAcco = context.Database.SqlQuery<string>(sql).ToList();
                                //Guid SQLAcco = Guid.Empty;
                                //if(sqlAcco.Count > 0)
                                //{
                                //    SQLAcco = new Guid(sqlAcco[0]);
                                //}
                                curRes.Accommodation_Id = acco.Select(a => a.Accommodation_Id).FirstOrDefault();
                                if (curRes.Accommodation_Id != null && curRes.Accommodation_Id != Guid.Empty)
                                {
                                    context.Accommodation_ProductMapping.AsNoTracking().Where(x => x.Accommodation_ProductMapping_Id == curRes.Accommodation_ProductMapping_Id)
                                               .Update(t => new Accommodation_ProductMapping()
                                               {
                                                   MatchedBy = curPriority - 1,
                                                   Status = "REVIEW",
                                                   Accommodation_Id = curRes.Accommodation_Id
                                               });
                                }

                            }
                        }
                        toupdate = toupdate + resmaster.Where(p => p.Accommodation_Id != Guid.Empty).Count();
                    }
                    CallLogVerbose(File_Id, "MATCH", toupdate.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                    if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.From = "MATCHING";
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                        using (ConsumerEntities context1 = new ConsumerEntities())
                        {
                            var oldRecords = (from y in context1.STG_Mapping_TableIds
                                              where y.File_Id == File_Id
                                              select y).ToList();
                            context1.STG_Mapping_TableIds.RemoveRange(oldRecords);
                            context1.SaveChanges();
                        }
                    }
                    retrn = true;
                    if (totPriorities == curPriority)
                    {
                        PLog.PercentageValue = 100;
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }
                    curPriority = curPriority + 1;
                }
                //if (Match_Direct_Master)
                //{
                //    bool ismatchmasterdone = UpdateHotelMappingStatusDirectMaster(obj);
                //}
                return retrn;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public bool UpdateHotelMappingStatus_older(DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;
            bool Match_Direct_Master = obj.Match_Direct_Master;

            List<DC_Accomodation_ProductMapping> res = new List<DC_Accomodation_ProductMapping>();
            IQueryable<Accommodation_ProductMapping> prodMapSearch;
            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if ((obj.FileMode ?? "ALL") != "ALL" && curPriority == 1)
                    //if (curPriority == 1)
                    {
                        List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                        DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();

                        lstSMT = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                  join j in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals j.Mapping_Id into jact
                                  from jdact in jact.DefaultIfEmpty()
                                  where a.Supplier_Id == curSupplier_Id && jdact.STG_Mapping_Table_Id == null
                                  && (a.Status == "UNMAPPED" || a.Accommodation_Id == null)
                                  select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                                  {
                                      STG_Mapping_Table_Id = Guid.NewGuid(),
                                      File_Id = obj.File_Id,
                                      Mapping_Id = a.Accommodation_ProductMapping_Id,
                                      STG_Id = null,
                                      Batch = obj.CurrentBatch ?? 1
                                  }).Take(250).ToList();
                        if (lstSMT.Count > 0)
                        {
                            bool idinsert = AddSTGMappingTableIDs(lstSMT);
                        }
                    }
                }
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    #region "Acco mapping query"
                    prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                     join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                     where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                     && a.Status.Trim().ToUpper() == "UNMAPPED"
                                     select a);

                    res = (from a in prodMapSearch
                           join act in context.m_CityMapping.AsNoTracking() on new { a.Supplier_Id, country = ((a.CountryName == null) ? a.CountryCode : a.CountryName).ToUpper().Trim(), city = ((a.CityName == null) ? a.CityCode : a.CityName).ToUpper().Trim() }
                           equals new { act.Supplier_Id, country = ((act.CountryName == null) ? act.CountryCode : act.CountryName).ToUpper().Trim(), city = ((act.CityName == null) ? act.CityCode : act.CityName).ToUpper().Trim() }
                           join mact in context.m_CityMaster.AsNoTracking() on new { country = (act.Country_Id ?? Guid.Empty) }
                           equals new { country = mact.Country_Id } into jact
                           from jdact in jact.DefaultIfEmpty()
                           where ((act.City_Id != null && (act.City_Id == jdact.City_Id)) || (act.City_Id == null && (act.CityName.ToUpper().Trim() == jdact.Name.ToUpper().Trim())))
                           select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                           {
                               Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                               Accommodation_Id = a.Accommodation_Id,
                               Address_tx = a.address_tx,
                               CityCode = a.CityCode,
                               CityName = a.CityName,
                               CountryCode = a.CountryCode,
                               CountryName = a.CountryName,
                               Create_Date = a.Create_Date,
                               Create_User = a.Create_User,
                               Edit_Date = a.Edit_Date,
                               Edit_User = a.Edit_User,
                               Email = a.Email,
                               Fax = a.Fax,
                               Latitude = a.Latitude,
                               Longitude = a.Longitude,
                               IsActive = (a.IsActive ?? true),
                               MapId = a.MapId,
                               PostCode = a.PostCode,
                               ProductName = a.ProductName,
                               Remarks = a.Remarks,
                               StarRating = a.StarRating,
                               StateCode = a.StateCode,
                               StateName = a.StateName,
                               Status = a.Status,
                               Street = a.Street,
                               Street2 = a.Street2,
                               Street3 = a.Street3,
                               Street4 = a.Street4,
                               SupplierId = a.SupplierId,
                               SupplierName = a.SupplierName,
                               SupplierProductReference = a.SupplierProductReference,
                               TelephoneNumber = a.TelephoneNumber,
                               TelephoneNumber_tx = a.TelephoneNumber_tx,
                               Website = a.Website,
                               //Country_Id = (jdact.Country_Id == Guid.Empty) ? ((act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id) : jdact.Country_Id,
                               //City_Id = (jdact.City_Id == Guid.Empty) ? ((act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id) : jdact.City_Id,
                               Country_Id = (act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id,
                               City_Id = (act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id,
                               SystemCityName = jdact.Name,
                               SystemCountryName = jdact.CountryName
                           }).ToList();
                    #endregion

                }

                foreach (int priority in obj.Priorities)
                {
                    //if (res.Count == 0)
                    //{
                    //    retrn = true;
                    //    break;
                    //}
                    var curAttributeVals = obj.lstConfigs.Where(a => a.Priority == priority).ToList();

                    List<DC_SupplierImportAttributeValues> configs = curAttributeVals;

                    //obj.CurrentPriority = curPriority;
                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCityCodeCheck = false;
                    bool isCityNameCheck = false;
                    bool isLatLongCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isPlaceIdCheck = false;
                    bool isAddressCheck = false;
                    bool isTelephoneCheck = false;
                    bool isPostCodeCheck = false;
                    totConfigs = configs.Count;
                    curConfig = 0;
                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");
                    configWhere = "";
                    foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                    {
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                        else
                            configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                        {
                            curConfig = curConfig + 1;

                            string CurrConfig = "";
                            if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                            else
                                CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                            //CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);


                            if (CurrConfig == "COUNTRY")
                                isCountryNameCheck = true;
                            else if (CurrConfig == "CITY")
                                isCityNameCheck = true;
                            else if (CurrConfig == "CompanyHotelID".ToUpper())
                                isCodeCheck = true;
                            else if (CurrConfig == "PostalCode".ToUpper())
                                isPostCodeCheck = true;
                            else if (CurrConfig == "HotelName".ToUpper())
                                isNameCheck = true;
                            else if (CurrConfig == "LATITUDE")
                                isLatLongCheck = true;
                            else if (CurrConfig == "GOOGLE_PLACE_ID")
                                isPlaceIdCheck = true;
                            else if (CurrConfig == "Address_Tx".ToUpper())
                                isAddressCheck = true;
                            else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                                isTelephoneCheck = true;

                            PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                            USD.AddStaticDataUploadProcessLog(PLog);

                        }
                    }
                    if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isAddressCheck || isTelephoneCheck || isPostCodeCheck)
                    {
                        if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isPostCodeCheck)
                        {
                            if (totPriorities == curPriority)
                            {
                                PLog.PercentageValue = 65;
                                USD.AddStaticDataUploadProcessLog(PLog);
                            }

                            // CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");

                            res.RemoveAll(p => p.Accommodation_Id != null && p.Accommodation_Id != Guid.Empty); //Guid.Empty
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                res = res.Select(c =>
                                    {
                                        c.Accommodation_Id = (context.Accommodations.AsNoTracking()
                                                        .Where(s => (
                                                                        //((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                        //((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                        //((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                                        //((isCityNameCheck && s.City_Id == c.City_Id) || (!isCityNameCheck)) &&
                                                                        ((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                        ((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                        ((isCodeCheck && s.CompanyHotelID.ToString() == c.SupplierProductReference) || (!isCodeCheck)) &&
                                                                        ((isPostCodeCheck && s.PostalCode.ToString() == c.PostCode) || (!isPostCodeCheck)) &&
                                                                        ((isNameCheck && s.HotelName.ToUpper().Replace("HOTEL", "").Replace(s.country ?? "", "").Replace(s.city ?? "", "").Replace("  ", " ").Trim() == c.ProductName.ToUpper().Replace("HOTEL", "").Replace(c.CountryName ?? "", "").Replace(c.CityName ?? "", "").Replace("  ", " ").Trim()) || (!isNameCheck)) &&
                                                                        ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck)) &&
                                                                        ((isPlaceIdCheck && s.Google_Place_Id == c.Google_Place_Id) || (!isPlaceIdCheck)) &&
                                                                        ((isAddressCheck && s.Address_Tx != null && c.Address_tx != null && s.Address_Tx == c.Address_tx) || (!isAddressCheck)) &&
                                                                        ((isTelephoneCheck && s.Telephone_Tx != null && c.TelephoneNumber_tx != null && s.Telephone_Tx == c.TelephoneNumber_tx) || (!isTelephoneCheck))
                                                                    )
                                                               )
                                                        .Select(s1 => s1.Accommodation_Id)
                                                        .FirstOrDefault()
                                                        );
                                        return c;
                                    }).ToList();
                            }
                        }
                        #region "Address Check and Telephone Check"
                        else if (isAddressCheck)
                        {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {

                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodations.AsNoTracking() on a.address_tx equals ac.Address_Tx
                                       where ac.Address_Tx != null && ac.Address_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                        else if (isTelephoneCheck)
                        {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {

                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodations.AsNoTracking() on a.TelephoneNumber_tx equals ac.Telephone_Tx
                                       where ac.Telephone_Tx != null && ac.Telephone_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                        #endregion

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        // CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");

                        var toupdate = res.Where(p => p.Accommodation_Id != Guid.Empty).Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 75;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        CallLogVerbose(File_Id, "MATCH", toupdate.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                        //CallLogVerbose(File_Id, "MATCH", "Updating into Database.");

                        foreach (DC_Accomodation_ProductMapping a in toupdate)
                        {
                            Guid curAccommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id;
                            Guid curAccommodation_Id = a.Accommodation_Id ?? Guid.Empty;

                            if (curAccommodation_Id != Guid.Empty)
                            {
                                using (ConsumerEntities context = new ConsumerEntities())
                                {
                                    context.Accommodation_ProductMapping.Where(x => x.Accommodation_ProductMapping_Id == curAccommodation_ProductMapping_Id)
                                       .Update(t => new Accommodation_ProductMapping()
                                       {
                                           MatchedBy = curPriority - 1,
                                           Status = "REVIEW",
                                           Accommodation_Id = curAccommodation_Id
                                       });
                                }
                            }
                        }

                        //var list = new List<System.Guid>();
                        //list = toupdate.Select(s =>  s.Accommodation_ProductMapping_Id).ToList();
                        //var newlist = toupdate.Select(s => new { Accommodation_ProductMapping_Id = s.Accommodation_ProductMapping_Id
                        //                                            , Accommodation_Id = s.Accommodation_Id });
                        //if (toupdate.Count > 0)
                        //{
                        //    context.Accommodation_ProductMapping
                        //         .Join(newlist, u => u.Accommodation_ProductMapping_Id, uir => uir.Accommodation_ProductMapping_Id,
                        //         (u, uir) => new { u, uir })
                        //         .Update(t => t.u);
                        //        //.Where(x => list.Contains(x.Accommodation_ProductMapping_Id))
                        //        //.Update(t => new Accommodation_ProductMapping() { MatchedBy = curPriority - 1, Status = "REVIEW"});


                        //    //.Any(a => a.Accommodation_ProductMapping_Id == x.Accommodation_ProductMapping_Id))
                        //}

                        //if (UpdateAccomodationProductMapping(toupdate))
                        //{

                        if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                        {
                            DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                            objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                            objStat.SupplierImportFile_Id = obj.File_Id;
                            objStat.From = "MATCHING";
                            DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                            using (ConsumerEntities context1 = new ConsumerEntities())
                            {
                                var oldRecords = (from y in context1.STG_Mapping_TableIds
                                                  where y.File_Id == File_Id
                                                  select y).ToList();
                                context1.STG_Mapping_TableIds.RemoveRange(oldRecords);
                                context1.SaveChanges();
                            }
                        }
                        //bool del = DeleteSTGMappingTableIDs(Guid.Parse(obj.File_Id.ToString()));

                        retrn = true;
                        //}
                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    else
                    {
                        retrn = false;
                    }

                    curPriority = curPriority + 1;
                }
                if (Match_Direct_Master)
                {
                    bool ismatchmasterdone = UpdateHotelMappingStatusDirectMaster(obj);
                }
                // }

                //}
                CallLogVerbose(File_Id, "MATCH", "Update Done.");
                return retrn;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

            return retrn;
        }


        public bool UpdateHotelMappingStatusDirectMaster(DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;
            bool Match_Direct_Master = obj.Match_Direct_Master;
            IQueryable<Accommodation_ProductMapping> prodMapSearch;
            List<DC_Accomodation_ProductMapping> res = new List<DC_Accomodation_ProductMapping>();

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    #region "Acco mapping query"
                    prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                         where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                         && a.Status.Trim().ToUpper() == "UNMAPPED"
                                         select a);

                    res = (from a in prodMapSearch
                           join mact in context.m_CityMaster.AsNoTracking() on new { country = (a.Country_Id ?? Guid.Empty) }
                           equals new { country = mact.Country_Id } into jact
                           from jdact in jact.DefaultIfEmpty()
                           where ((a.City_Id != null && (a.City_Id == jdact.City_Id)) || (a.City_Id == null && (a.CityName.ToUpper().Trim() == jdact.Name.ToUpper().Trim())))
                           select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                           {
                               Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                               Accommodation_Id = a.Accommodation_Id,
                               Address_tx = a.address_tx,
                               CityCode = a.CityCode,
                               CityName = a.CityName,
                               CountryCode = a.CountryCode,
                               CountryName = a.CountryName,
                               Create_Date = a.Create_Date,
                               Create_User = a.Create_User,
                               Edit_Date = a.Edit_Date,
                               Edit_User = a.Edit_User,
                               Email = a.Email,
                               Fax = a.Fax,
                               Latitude = a.Latitude,
                               Longitude = a.Longitude,
                               IsActive = (a.IsActive ?? true),
                               MapId = a.MapId,
                               PostCode = a.PostCode,
                               ProductName = a.ProductName,
                               Remarks = a.Remarks,
                               StarRating = a.StarRating,
                               StateCode = a.StateCode,
                               StateName = a.StateName,
                               Status = a.Status,
                               Street = a.Street,
                               Street2 = a.Street2,
                               Street3 = a.Street3,
                               Street4 = a.Street4,
                               SupplierId = a.SupplierId,
                               SupplierName = a.SupplierName,
                               SupplierProductReference = a.SupplierProductReference,
                               TelephoneNumber = a.TelephoneNumber,
                               TelephoneNumber_tx = a.TelephoneNumber_tx,
                               Website = a.Website,
                               //Country_Id = (jdact.Country_Id == Guid.Empty) ? ((act.Country_Id == Guid.Empty) ? a.Country_Id : act.Country_Id) : jdact.Country_Id,
                               //City_Id = (jdact.City_Id == Guid.Empty) ? ((act.City_Id == Guid.Empty) ? a.City_Id : act.City_Id) : jdact.City_Id,
                               Country_Id = (jdact.Country_Id == Guid.Empty) ? a.Country_Id : jdact.Country_Id,
                               City_Id = (jdact.City_Id == Guid.Empty) ? a.City_Id : jdact.City_Id,
                               SystemCityName = jdact.Name,
                               SystemCountryName = jdact.CountryName
                           }).ToList();
                    #endregion
                }
                    foreach (int priority in obj.Priorities)
                    {
                        if (res.Count == 0)
                        {
                            break;
                        }
                        var curAttributeVals = obj.lstConfigs.Where(a => a.Priority == priority).ToList();

                        List<DC_SupplierImportAttributeValues> configs = curAttributeVals;

                        //obj.CurrentPriority = curPriority;
                        bool isCountryCodeCheck = false;
                        bool isCountryNameCheck = false;
                        bool isCityCodeCheck = false;
                        bool isCityNameCheck = false;
                        bool isLatLongCheck = false;
                        bool isCodeCheck = false;
                        bool isNameCheck = false;
                        bool isPlaceIdCheck = false;
                        bool isAddressCheck = false;
                        bool isTelephoneCheck = false;
                        bool isPostCodeCheck = false;
                        totConfigs = configs.Count;
                        curConfig = 0;
                        CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");
                        configWhere = "";
                        foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                        {
                            if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                            else
                                configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                        }
                        configWhere = configWhere.Remove(configWhere.Length - 1);
                        CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);

                        foreach (DC_SupplierImportAttributeValues config in curAttributeVals)
                        {
                            curConfig = curConfig + 1;

                            string CurrConfig = "";
                            if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                                CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                            else
                                CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                            //CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);

                            if (CurrConfig == "COUNTRY")
                                isCountryNameCheck = true;
                            else if (CurrConfig == "CITY")
                                isCityNameCheck = true;
                            else if (CurrConfig == "CompanyHotelID".ToUpper())
                                isCodeCheck = true;
                            else if (CurrConfig == "PostalCode".ToUpper())
                                isPostCodeCheck = true;
                            else if (CurrConfig == "HotelName".ToUpper())
                                isNameCheck = true;
                            else if (CurrConfig == "LATITUDE")
                                isLatLongCheck = true;
                            else if (CurrConfig == "GOOGLE_PLACE_ID")
                                isPlaceIdCheck = true;
                            else if (CurrConfig == "Address_Tx".ToUpper())
                                isAddressCheck = true;
                            else if (CurrConfig == "TelephoneNumber_tx".ToUpper() || CurrConfig == "Telephone_tx".ToUpper())
                                isTelephoneCheck = true;


                        }

                        if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isAddressCheck || isTelephoneCheck || isPostCodeCheck)
                        {
                            if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isPostCodeCheck)
                            {
                               

                                //CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                res.RemoveAll(p => p.Accommodation_Id != null && p.Accommodation_Id != Guid.Empty); //Guid.Empty
                                res = res.Select(c =>
                                {
                                    c.Accommodation_Id = (context.Accommodations.AsNoTracking()
                                                    .Where(s => (
                                                                    //((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                    //((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                    //((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                                    //((isCityNameCheck && s.City_Id == c.City_Id) || (!isCityNameCheck)) &&
                                                                    ((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                                    ((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                                    ((isCodeCheck && s.CompanyHotelID.ToString() == c.SupplierProductReference) || (!isCodeCheck)) &&
                                                                    ((isPostCodeCheck && s.PostalCode.ToString() == c.PostCode) || (!isPostCodeCheck)) &&
                                                                    ((isNameCheck && s.HotelName.ToUpper().Replace("HOTEL", "").Replace(s.country ?? "", "").Replace(s.city ?? "", "").Replace("  ", " ").Trim() == c.ProductName.ToUpper().Replace("HOTEL", "").Replace(c.CountryName ?? "", "").Replace(c.CityName ?? "", "").Replace("  ", " ").Trim()) || (!isNameCheck)) &&
                                                                    ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck)) &&
                                                                    ((isPlaceIdCheck && s.Google_Place_Id == c.Google_Place_Id) || (!isPlaceIdCheck)) &&
                                                                    ((isAddressCheck && s.Address_Tx != null && c.Address_tx != null && s.Address_Tx == c.Address_tx) || (!isAddressCheck)) &&
                                                                    ((isTelephoneCheck && s.Telephone_Tx != null && c.TelephoneNumber_tx != null && s.Telephone_Tx == c.TelephoneNumber_tx) || (!isTelephoneCheck))
                                                                )
                                                           )
                                                    .Select(s1 => s1.Accommodation_Id)
                                                    .FirstOrDefault()
                                                    );
                                    return c;
                                }).ToList();
                            }
                        }
                            #region "Address Check and Telephone Check"
                            else if (isAddressCheck)
                            {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodations.AsNoTracking() on a.address_tx equals ac.Address_Tx
                                       where ac.Address_Tx != null && ac.Address_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                            else if (isTelephoneCheck)
                            {
                            using (ConsumerEntities context = new ConsumerEntities())
                            {
                                prodMapSearch = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                                 join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                                 where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id && s.Batch == obj.CurrentBatch
                                                 && a.Status.Trim().ToUpper() == "UNMAPPED"
                                                 select a);
                                res = (from a in prodMapSearch
                                       join ac in context.Accommodations.AsNoTracking() on a.TelephoneNumber_tx equals ac.Telephone_Tx
                                       where ac.Telephone_Tx != null && ac.Telephone_Tx != ""
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = ac.Accommodation_Id,
                                           Address_tx = a.address_tx,
                                           CityCode = a.CityCode,
                                           CityName = a.CityName,
                                           CountryCode = a.CountryCode,
                                           CountryName = a.CountryName,
                                           Create_Date = a.Create_Date,
                                           Create_User = a.Create_User,
                                           Edit_Date = a.Edit_Date,
                                           Edit_User = a.Edit_User,
                                           Email = a.Email,
                                           Fax = a.Fax,
                                           Latitude = a.Latitude,
                                           Longitude = a.Longitude,
                                           IsActive = (a.IsActive ?? true),
                                           MapId = a.MapId,
                                           PostCode = a.PostCode,
                                           ProductName = a.ProductName,
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating,
                                           StateCode = a.StateCode,
                                           StateName = a.StateName,
                                           Status = "REVIEW",
                                           Street = a.Street,
                                           Street2 = a.Street2,
                                           Street3 = a.Street3,
                                           Street4 = a.Street4,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           TelephoneNumber = a.TelephoneNumber,
                                           TelephoneNumber_tx = a.TelephoneNumber_tx,
                                           Website = a.Website
                                       }).ToList();
                            }
                        }
                            #endregion

                            //CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");                           
                            //BatchExtensions.Update()

                            //insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductReference == w.ProductId)).ToList();
                            //updateMappingList = toUpdate.Where(w => w.ProductName != w.oldProductName).ToList();



                            var toupdate = res.Where(p => p.Accommodation_Id != Guid.Empty).Select(c =>
                            {
                                c.MatchedBy = curPriority - 1;
                                c.Status = ("REVIEW"); return c;
                            }).ToList();

                           
                            CallLogVerbose(File_Id, "MATCH", toupdate.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                            //CallLogVerbose(File_Id, "MATCH", "Updating into Database.");                            
                            //List<Accommodation_ProductMapping> dbapm = new List<Accommodation_ProductMapping>();
                            foreach (DC_Accomodation_ProductMapping a in toupdate)
                            {
                            Guid curAccommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id;
                            Guid curAccommodation_Id = a.Accommodation_Id ?? Guid.Empty;

                            if (curAccommodation_Id != Guid.Empty)
                            {
                                using (ConsumerEntities context = new ConsumerEntities())
                                {
                                    context.Accommodation_ProductMapping.Where(x => x.Accommodation_ProductMapping_Id == curAccommodation_ProductMapping_Id)
                                       .Update(t => new Accommodation_ProductMapping()
                            {
                                           MatchedBy = curPriority - 1,
                                           Status = "REVIEW",
                                           Accommodation_Id = curAccommodation_Id
                                       });
                                }
                            }
                            }

                        //var list = new List<System.Guid>();
                        //foreach (DC_Accomodation_ProductMapping a in toupdate)
                        //{
                        //    list.Add(a.Accommodation_ProductMapping_Id);
                        //    //dbapm.Add(new Accommodation_ProductMapping() { Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id } );
                        //}

                        //if (toupdate.Count > 0)
                        //{
                        //    context.Accommodation_ProductMapping.Where(x => list.Contains(x.Accommodation_ProductMapping_Id))
                        //        //.Any(a => a.Accommodation_ProductMapping_Id == x.Accommodation_ProductMapping_Id))
                        //        .Update(t => new Accommodation_ProductMapping() { MatchedBy = curPriority - 1, Status = "REVIEW" });
                        //}
                            retrn = true;


                            //if (UpdateAccomodationProductMapping(toupdate))
                            //{
                            //    retrn = true;
                            //}


                        }
                        else
                        {
                            retrn = false;
                        }

                        curPriority = curPriority + 1;
                    }
                //}
                //CallLogVerbose(File_Id, "MATCH", "Update Done.");
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

            return retrn;
        }
        public bool UpdateHotelMappingStatusDirectMaster_Old(DC_MappingMatch obj)
        {
            bool retrn = false;
            //List<DC_Accomodation_ProductMapping> CMS = obj.lstHotelMapping;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;
            bool Match_Direct_Master = obj.Match_Direct_Master;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            List<DC_Accomodation_ProductMapping> ret = new List<DC_Accomodation_ProductMapping>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMap = (from a in context.Accommodation_ProductMapping.AsNoTracking()
                                   join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                   where s.File_Id == supdata.File_Id && a.Accommodation_Id == null && a.Supplier_Id == curSupplier_Id
                                   && a.Status.Trim().ToUpper() == "UNMAPPED" && s.Batch == obj.CurrentBatch
                                   select a);

                    //var ct = prodMap.Count();
                    /*if (obj.IsBatched)
                    {
                        prodMap = (from a in prodMap
                                   join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_ProductMapping_Id equals s.Mapping_Id
                                   //join s in context.stg_SupplierProductMapping.AsNoTracking() on a.stg_AccoMapping_Id equals s.stg_AccoMapping_Id
                                   where s.Batch == obj.CurrentBatch
                                   select a);
                    }*/
                    var prodMapSearch = prodMap; //.ToList();
                    //var ct1 = prodMap.Count();

                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCityCodeCheck = false;
                    bool isCityNameCheck = false;
                    bool isLatLongCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isPlaceIdCheck = false;
                    bool isAddressCheck = false;
                    bool isTelephoneCheck = false;
                    bool isPostCodeCheck = false;

                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation.", "").Trim() + ",";
                        else
                            configWhere = configWhere + " " + config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);


                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        if (config.AttributeValue.Replace("Accommodation.", "").Trim() != "---ALL---")
                            CurrConfig = config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper();
                        else
                            CurrConfig = config.AttributeName.Replace("Accommodation_ProductMapping.", "").Trim().ToUpper();
                        //CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);
                        //configWhere = " " + configWhere + config.AttributeName + " == " + config.AttributeValue + " AND";
                        //if (config.AttributeValue.Replace("Accommodation.", "").Trim().ToUpper() == "COUNTRYCODE")
                        //{
                        //    isCountryCodeCheck = true;
                        //    prodMapSearch = (from a in prodMapSearch
                        //                     join cm in context.m_CountryMapping on new { a.Supplier_Id, a.CountryCode } equals new { cm.Supplier_Id, cm.CountryCode }
                        //                     join m in context.m_CountryMaster on cm.Country_Id equals m.Country_Id
                        //                     join ac in context.Accommodations on m.Code equals ac.cou
                        //                     where a.CountryCode == m.Code
                        //                     select a);
                        //}
                        if (CurrConfig == "COUNTRY")
                        {
                            isCountryNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                                 //join cm in context.m_CountryMaster.AsNoTracking() on new { country = ((a.CountryName == null) ? a.CountryCode : a.CountryName).ToUpper().Trim() } equals new { country = ((cm.Name == null) ? cm.Code : cm.Name).ToUpper().Trim() }
                                             join cm in context.m_CountryMaster.AsNoTracking() on a.Country_Id equals cm.Country_Id
                                             //where ((a.CountryName == null) ? (a.CountryCode == cm.CountryCode) : (a.CountryName == cm.CountryName))
                                             //join ac in context.Accommodations.AsNoTracking() on m.Name equals ac.country
                                             select a).Distinct();//.ToList();

                        }
                        if (CurrConfig == "CITY")
                        {
                            isCityNameCheck = true;

                            /* prodMapSearch = (from a in prodMapSearch
                                              join ctm in context.m_CityMaster on new { country = ((a.CountryName == null) ? a.CountryCode : a.CountryName).ToUpper().Trim(), city = ((a.CityName == null) ? a.CityCode : a.CityName).ToUpper().Trim() } equals new { country = ((ctm.CountryName == null) ? ctm.CountryCode : ctm.CountryName).ToUpper().Trim(), city = ((ctm.Name == null) ? ctm.Code : ctm.Name).ToUpper().Trim() }
                                              //join m in context.m_CityMaster on new { Country_Id = (ctm.Country_Id ?? Guid.Empty), city = ctm.CityName.ToUpper().Trim() } equals new { Country_Id = m.Country_Id, city = m.Name.ToUpper().Trim() } //ctm.City_Id equals m.City_Id // a.CityName equals m.Name
                                              //where ((a.CityName == null) ? (a.CityCode == ctm.CityCode) : (a.CityName == ctm.CityName))

                                              //where a.CityName.Trim().ToUpper() == m.Name
                                              select a).Distinct();//.ToList();
                                                                   //var newprodMapSearch = (from a in prodMapSearch
                                                                   //                        join ctm in cities on new { a.CountryName, a.CityName } equals new { ctm.CountryName, ctm.CityName }
                                                                   //                        join c in context.m_CityMaster.AsNoTracking() on ctm.City_Id equals c.City_Id
                                                                   //                        join ac in context.Accommodations.AsNoTracking() on c.Name equals ac.city
                                                                   //                        select a).Distinct().ToList();  
                                                                   //prodMapSearch = newprodMapSearch.ToList();

                             */
                        }
                        if (CurrConfig == "CompanyHotelID".ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join ac in context.Accommodations.AsNoTracking() on new { a.Country_Id, a.City_Id } equals new { ac.Country_Id, ac.City_Id }
                                             where a.SupplierProductReference.Trim().ToUpper() == ac.CompanyHotelID.ToString()
                                             select a).Distinct();//.ToList();
                        }
                        if (CurrConfig == "PostalCode".ToUpper())
                        {
                            isPostCodeCheck = true;
                        }
                        if (CurrConfig == "HotelName".ToUpper())
                        {
                            isNameCheck = true;
                            //var cities = (from cm in context.m_CityMapping.AsNoTracking()
                            //              where cm.Supplier_Id == curSupplier_Id
                            //              select cm);                            
                            prodMapSearch = (from a in prodMapSearch
                                                 //join ctm in cities on new { a.Country_Id, a.City_Id } equals new { ctm.Country_Id, ctm.City_Id }
                                                 //join ctm in context.m_CityMaster on new { country = (a.Country_Id ?? Guid.Empty), city = a.CityName.ToUpper().Trim() } 
                                                 //equals new { country = ctm.Country_Id, city = ctm.Name.ToUpper().Trim() }
                                             join ac in context.Accommodations.AsNoTracking() on new { country = a.Country_Id, city = a.CityName.ToUpper().Trim(), hotel = (a.ProductName ?? string.Empty).ToString().ToUpper().Replace("HOTEL", "").Replace(a.CityName, "").Replace(a.CountryName, "").Replace("  ", " ").Trim() }
                                             equals new { country = ac.Country_Id, city = ac.city.ToUpper().Trim(), hotel = (ac.HotelName ?? string.Empty).ToString().ToUpper().Replace("HOTEL", "").Replace(ac.city, "").Replace(ac.country, "").Replace("  ", " ").Trim() }
                                             select a).Distinct();//.ToList();


                        }


                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }
                    List<DC_Accomodation_ProductMapping> res = new List<DC_Accomodation_ProductMapping>();

                    if (isCountryNameCheck || isCityNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isPlaceIdCheck || isAddressCheck || isTelephoneCheck || isPostCodeCheck)
                    {
                        res = (from a in prodMapSearch
                                   //join mact in context.m_CityMaster.AsNoTracking() on new { country = (a.Country_Id ?? Guid.Empty), city = a.CityName.ToUpper().Trim() } 
                                   //equals new { country = mact.Country_Id, city = mact.Name.ToUpper().Trim() } //into jact
                                   //from jdact in jact.DefaultIfEmpty()
                               select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                               {
                                   Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                   Accommodation_Id = a.Accommodation_Id,
                                   Address_tx = a.address_tx,
                                   CityCode = a.CityCode,
                                   CityName = a.CityName,
                                   CountryCode = a.CountryCode,
                                   CountryName = a.CountryName,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   Email = a.Email,
                                   Fax = a.Fax,
                                   Latitude = a.Latitude,
                                   Longitude = a.Longitude,
                                   IsActive = (a.IsActive ?? true),
                                   MapId = a.MapId,
                                   PostCode = a.PostCode,
                                   ProductName = a.ProductName,
                                   Remarks = a.Remarks,
                                   StarRating = a.StarRating,
                                   StateCode = a.StateCode,
                                   StateName = a.StateName,
                                   Status = a.Status,
                                   Street = a.Street,
                                   Street2 = a.Street2,
                                   Street3 = a.Street3,
                                   Street4 = a.Street4,
                                   SupplierId = a.SupplierId,
                                   SupplierName = a.SupplierName,
                                   SupplierProductReference = a.SupplierProductReference,
                                   TelephoneNumber = a.TelephoneNumber,
                                   TelephoneNumber_tx = a.TelephoneNumber_tx,
                                   Website = a.Website,
                                   Country_Id = a.Country_Id,
                                   City_Id = a.City_Id,
                                   //SystemCityName = jdact.Name,
                                   //SystemCountryName = jdac.Name
                               }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");

                        res = res.Select(c =>
                        {
                            c.Accommodation_Id = (context.Accommodations.AsNoTracking()
                                            .Where(s => (
                                                            //((isCountryNameCheck && s.country.ToUpper().Trim() == c.SystemCountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                            //((isCityNameCheck && s.city.ToUpper().Trim() == c.SystemCityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                            ((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                            //((isCityNameCheck && s.City_Id == c.City_Id) || (!isCityNameCheck)) &&
                                                            //((isCountryNameCheck && s.country.ToUpper().Trim() == c.CountryName.ToUpper().Trim()) || (!isCountryNameCheck)) &&
                                                            ((isCityNameCheck && s.city.ToUpper().Trim() == c.CityName.ToUpper().Trim()) || (!isCityNameCheck)) &&
                                                            ((isCodeCheck && s.CompanyHotelID.ToString() == c.SupplierProductReference) || (!isCodeCheck)) &&
                                                            ((isPostCodeCheck && s.PostalCode.ToString() == c.PostCode) || (!isPostCodeCheck)) &&
                                                            ((isNameCheck && s.HotelName.ToUpper().Replace("HOTEL", "").Replace(s.country, "").Replace(s.city, "").Replace("  ", " ").Trim() == c.ProductName.ToUpper().Replace("HOTEL", "").Replace(c.CountryName, "").Replace(c.CityName, "").Replace("  ", " ").Trim()) || (!isNameCheck)) &&
                                                            ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck)) &&
                                                            ((isPlaceIdCheck && s.Google_Place_Id == c.Google_Place_Id) || (!isPlaceIdCheck)) &&
                                                            ((isAddressCheck && s.Address_Tx != null && c.Address_tx != null && s.Address_Tx == c.Address_tx) || (!isAddressCheck)) &&
                                                            ((isTelephoneCheck && s.Telephone_Tx != null && c.TelephoneNumber_tx != null && s.Telephone_Tx == c.TelephoneNumber_tx) || (!isTelephoneCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.Accommodation_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");
                        res.RemoveAll(p => p.Accommodation_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 75;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.");
                        if (UpdateAccomodationProductMapping(res))
                        {
                            retrn = true;
                        }
                    }
                    else
                    {
                        retrn = false;
                    }
                }
                return retrn;
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        //public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(int PageNo, int PageSize, Guid Accomodation_Id, string Status)
        public IList<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetAccomodationProductMapping(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping select a;

                    if (obj.Accommodation_Id != Guid.Empty)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Accommodation_Id == obj.Accommodation_Id
                                        select a;
                    }

                    if (obj.Status != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status.Trim().ToUpper() ?? "UNMAPPED") == obj.Status.Trim().ToUpper()
                                        select a;
                    }
                    if (obj.StatusExcept != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where (a.Status.Trim().ToUpper() ?? "UNMAPPED") != obj.StatusExcept.Trim().ToUpper()
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;
                    System.Linq.IQueryable<DataContracts.Mapping.DC_Accomodation_ProductMapping> prodMapList;

                    if (obj.Accommodation_Id != Guid.Empty)
                    {
                        prodMapList = (from a in prodMapSearch
                                       join ac in context.Accommodations on a.Accommodation_Id equals ac.Accommodation_Id
                                       where ac.Accommodation_Id == obj.Accommodation_Id
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
                                           MapId = a.MapId,
                                           StarRating = a.StarRating
                                       }).Skip(skip).Take(obj.PageSize);
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
                                           Remarks = a.Remarks,
                                           StarRating = a.StarRating
                                       }).Skip(skip).Take(obj.PageSize);
                    }

                    return prodMapList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetMappingHotelData(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping.AsNoTracking() select a; //where a.Accommodation_Id == null 

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

                    if (obj.Supplier_Id != null)
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.Supplier_Id == obj.Supplier_Id
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
                    /*if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCountryMapping = (from a in context.m_CountryMapping.AsNoTracking() select new { a.Country_Id, a.CountryName }).Distinct();
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
                            var distCityMapping = (from a in context.m_CityMapping.AsNoTracking() select new { a.City_Id, a.CityName }).Distinct();
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
                    */

                    if (!string.IsNullOrWhiteSpace(obj.CountryName))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Source) && obj.Source.ToUpper() == "SYSTEMDATA")
                        {
                            var distCountryMapping = (from a in context.m_CountryMapping.AsNoTracking() select new { a.Country_Id, a.CountryCode, a.CountryName, a.Supplier_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCountryMapping on new { a.Supplier_Id } equals new { ct.Supplier_Id }
                                            join mct in context.m_CountryMaster on ct.Country_Id equals mct.Country_Id
                                            where mct.Name == obj.CountryName
                                            && ((a.CountryName == null) ? (a.CountryCode == ct.CountryCode) : (a.CountryName == ct.CountryName))
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
                            var distCityMapping = (from a in context.m_CityMapping.AsNoTracking() select new { a.City_Id, a.CityName, a.Supplier_Id, a.Country_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCityMapping on new { a.Supplier_Id, a.Country_Id, a.CityName } equals new { ct.Supplier_Id, ct.Country_Id, ct.CityName }
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
                    if (!string.IsNullOrWhiteSpace(obj.StarRating))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.StarRating.Contains(obj.StarRating)
                                        select a;
                    }

                    int total;

                    //total = prodMapSearch.Count();

                    //var skip = obj.PageSize * obj.PageNo;

                    //var canPage = skip < total;

                    var prodMapList = (from a in prodMapSearch
                                       select new DataContracts.Mapping.DC_Accomodation_ProductMapping
                                       {
                                           Accommodation_ProductMapping_Id = a.Accommodation_ProductMapping_Id,
                                           Accommodation_Id = a.Accommodation_Id,
                                           Supplier_Id = a.Supplier_Id,
                                           SupplierId = a.SupplierId,
                                           SupplierName = a.SupplierName,
                                           SupplierProductReference = a.SupplierProductReference,
                                           ProductName = a.ProductName,
                                           oldProductName = a.ProductName,
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
                                           Remarks = a.Remarks,
                                           MapId = a.MapId,
                                           StarRating = a.StarRating,
                                           Country_Id = a.Country_Id,
                                           City_Id = a.City_Id

                                       });//.Skip(skip).Take(obj.PageSize);

                    var result = prodMapList.ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while searching accomodation product mapping by supplier search", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<DataContracts.Mapping.DC_Accomodation_ProductMapping> GetProductSupplierMappingSearch(DataContracts.Mapping.DC_Mapping_ProductSupplier_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var prodMapSearch = from a in context.Accommodation_ProductMapping.AsNoTracking() select a; //where a.Accommodation_Id == null 

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
                                        where a.ProductName.ToLower().Replace("hotel", "").Trim().Contains(obj.ProductName.ToLower().Replace("hotel", "").Trim())
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
                            var distCountryMapping = (from a in context.m_CountryMapping.AsNoTracking() select new { a.Country_Id, a.CountryCode, a.CountryName, a.Supplier_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCountryMapping on new { a.Supplier_Id } equals new { ct.Supplier_Id }
                                            join mct in context.m_CountryMaster on ct.Country_Id equals mct.Country_Id
                                            where mct.Name == obj.CountryName
                                            && ((a.CountryName == null) ? (a.CountryCode == ct.CountryCode) : (a.CountryName == ct.CountryName))
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
                            var distCityMapping = (from a in context.m_CityMapping.AsNoTracking() select new { a.City_Id, a.CityName, a.Supplier_Id, a.Country_Id }).Distinct();
                            prodMapSearch = from a in prodMapSearch
                                            join ct in distCityMapping on new { a.Supplier_Id, a.Country_Id, a.CityName } equals new { ct.Supplier_Id, ct.Country_Id, ct.CityName }
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
                    if (!string.IsNullOrWhiteSpace(obj.StarRating))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.StarRating.Contains(obj.StarRating)
                                        select a;
                    }

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var canPage = skip < total;

                    //if (!canPage)
                    //    return null;

                    var prodMapList = (from a in prodMapSearch
                                       join ma in context.Accommodations.AsNoTracking() on a.Accommodation_Id equals ma.Accommodation_Id into ja
                                       from jda in ja.DefaultIfEmpty()
                                       join mact in context.m_CityMaster.AsNoTracking() on a.City_Id equals mact.City_Id into jact
                                       from jdact in jact.DefaultIfEmpty()
                                       join mac in context.m_CountryMaster.AsNoTracking() on a.Country_Id equals mac.Country_Id into jac
                                       from jdac in jac.DefaultIfEmpty()
                                           //where jda.Location != null
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
                                           oldProductName = a.ProductName,
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
                                           //SystemCityName = (jda.city ?? (context.Accommodations.Where(x => x.city == a.CityName).Select(x => x.city)).FirstOrDefault()),
                                           SystemCityName = (jda.city ?? jdact.Name),
                                           //SystemCountryName = (jda.country ?? (context.Accommodations.Where(x => x.country == a.CountryName).Select(x => x.country)).FirstOrDefault()),
                                           SystemCountryName = (jda.country ?? jdac.Name) ?? (context.m_CountryMaster.Where(x => x.Country_Id == jdact.Country_Id).Select(c => c.Name).FirstOrDefault()),
                                           MapId = a.MapId,
                                           FullAddress = (a.address != null ? a.address : (a.Street + ", " + a.Street2 + " " + a.Street3 + " " + a.Street4 + " " + a.PostCode + ", " + a.CityName + ", " + a.StateName + ", " + a.CountryName)),
                                           //(a.address ?? string.Empty) + ", " + (a.Street ?? string.Empty) + ", " + (a.Street2 ?? string.Empty) + " " + (a.Street3 ?? string.Empty) + " " + (a.Street4 ?? string.Empty) + " " + (a.PostCode ?? string.Empty) + ", " + (a.CityName ?? string.Empty) + ", " + (a.StateName ?? string.Empty) + ", " + (a.CountryName ?? string.Empty),
                                           SystemFullAddress = (jda.FullAddress ?? string.Empty),
                                           StarRating = a.StarRating,
                                           Location = jda.Location
                                       }).Skip(skip).Take(obj.PageSize);

                    var result = prodMapList.ToList();
                    //if (obj.CalledFromTLGX == null || obj.CalledFromTLGX != "TLGX")
                    if (string.IsNullOrWhiteSpace(obj.CalledFromTLGX))
                    {
                        if (!string.IsNullOrWhiteSpace(obj.Status) && !string.IsNullOrWhiteSpace(obj.CityName) && obj.Status.ToLower().Trim() == "unmapped")
                        {
                            foreach (var item in result)
                            {
                                if (!string.IsNullOrWhiteSpace(item.ProductName))
                                {
                                    var prodname = item.ProductName.ToLower().Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").Replace(item.CityName.ToLower(), "").Replace(item.CountryName.ToLower(), "").ToUpper();
                                    var prodcode = item.ProductId;
                                    if (!string.IsNullOrWhiteSpace(prodname) && prodname.ToUpper() != "&NBSP;")
                                    {
                                        var searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Equals(prodname)).FirstOrDefault();
                                        if (searchprod == null)
                                            searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).Replace("*", "").Replace("-", "").Replace(" ", "").Replace("#", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("hotel", "").ToUpper().Contains(prodname)).FirstOrDefault();
                                        //if (!string.IsNullOrWhiteSpace(prodcode) && prodname.ToUpper() != "&NBSP;")
                                        //{
                                        //    if (searchprod == null)
                                        //        searchprod = context.Accommodations.Where(a => (a.country == item.SystemCountryName) && (a.city == item.SystemCityName) && (a.HotelName ?? string.Empty).ToUpper().Equals(prodcode)).FirstOrDefault();
                                        //}
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

            List<DataLayer.Accommodation_ProductMapping> lstobjNew = new List<Accommodation_ProductMapping>();

            using (ConsumerEntities context = new ConsumerEntities())
            {
                foreach (var PM in obj)
                {
                    if (PM.Accommodation_ProductMapping_Id == null) //|| PM.Accommodation_Id == null|| PM.Supplier_Id == null
                    {
                        continue;
                    }

                    try
                    {
                        DataLayer.Accommodation_ProductMapping search = new DataLayer.Accommodation_ProductMapping();

                        /*search = (from a in context.Accommodation_ProductMapping
                                      where a.Accommodation_ProductMapping_Id == PM.Accommodation_ProductMapping_Id
                                  select a).FirstOrDefault();*/
                        search = context.Accommodation_ProductMapping.Find(PM.Accommodation_ProductMapping_Id);
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
                            if (PM.MatchedBy != null)
                                search.MatchedBy = PM.MatchedBy;
                            search.Status = PM.Status;
                            search.IsActive = PM.IsActive;
                            search.Edit_Date = PM.Edit_Date;
                            search.Edit_User = PM.Edit_User;
                            search.Remarks = PM.Remarks;
                            //}

                            context.SaveChanges();
                        }

                        if (search == null)
                        {
                            DataLayer.Accommodation_ProductMapping objNew = new Accommodation_ProductMapping();
                            objNew.Accommodation_ProductMapping_Id = PM.Accommodation_ProductMapping_Id;
                            objNew.Accommodation_Id = PM.Accommodation_Id;
                            objNew.CityCode = PM.CityCode;
                            objNew.CityName = PM.CityName;
                            objNew.CountryCode = PM.CountryCode;
                            objNew.CountryName = PM.CountryName;
                            objNew.Create_Date = PM.Create_Date;
                            objNew.Create_User = PM.Create_User;
                            objNew.Email = PM.Email;
                            objNew.Fax = PM.Fax;
                            objNew.Google_Place_Id = PM.Google_Place_Id;
                            objNew.IsActive = PM.IsActive;
                            objNew.Latitude = PM.Latitude;
                            objNew.Longitude = PM.Longitude;
                            objNew.PostCode = PM.PostCode;
                            objNew.ProductName = PM.ProductName;
                            objNew.Remarks = PM.Remarks;
                            objNew.StarRating = PM.StarRating;
                            objNew.StateCode = PM.StateCode;
                            objNew.StateName = PM.StateName;
                            objNew.Status = PM.Status;
                            objNew.Street = PM.Street;
                            objNew.Street2 = PM.Street2;
                            objNew.Street3 = PM.Street3;
                            objNew.Street4 = PM.Street4;
                            objNew.SupplierId = PM.SupplierId;
                            objNew.SupplierName = PM.SupplierName;
                            objNew.SupplierProductReference = PM.SupplierProductReference;
                            objNew.Supplier_Id = PM.Supplier_Id;
                            objNew.TelephoneNumber = PM.TelephoneNumber;
                            objNew.Country_Id = PM.Country_Id;
                            objNew.City_Id = PM.City_Id;
                            objNew.Website = PM.Website;
                            objNew.address = PM.FullAddress;
                            objNew.Latitude_Tx = PM.Latitude_Tx;
                            objNew.Longitude_Tx = PM.Longitude_Tx;
                            lstobjNew.Add(objNew);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                    }
                }
                if (lstobjNew.Count > 0)
                {

                    context.Accommodation_ProductMapping.AddRange(lstobjNew);
                    context.SaveChanges();
                    //context.USP_UpdateMapID("product");
                }
            }
            //}
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

        public void CheckRoomTypeAlreadyExist(Guid CurSupplier_Id, List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> stg, out List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> updateMappingList, out List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> insertSTGList)
        {
            bool ret = false;

            using (ConsumerEntities context = new ConsumerEntities())
            {
                //var prodMapList = context.Accommodation_ProductMapping.AsNoTracking().Where(w => stg.Any(a => a.ProductId == w.SupplierProductReference)).Select(s => s.SupplierProductReference ).ToList();
                var accomap = (from a in context.Accommodation_SupplierRoomTypeMapping
                               where a.Supplier_Id == CurSupplier_Id
                               select a);
                var toUpdate = (from a in accomap
                                    //join s in stg on a.SupplierProductReference equals s.ProductId
                                join s in context.stg_SupplierHotelRoomMapping on new { Supplier_Id = a.Supplier_Id, SupplierProductReference = a.SupplierRoomTypeCode, ProductId = a.SupplierProductId }
                                equals new { Supplier_Id = s.Supplier_Id, SupplierProductReference = s.SupplierRoomTypeCode, ProductId = s.SupplierProductId }
                                select new DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                {
                                    ProductName = a.SupplierProductName,
                                    Accommodation_Id = a.Accommodation_Id,
                                    SupplierName = a.SupplierName,
                                    Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                    Accommodation_RoomInfo_Name = a.SupplierRoomName,
                                    SupplierRoomName = s.RoomName,
                                    OldSupplierRoomName = a.SupplierRoomName,
                                    Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                    SupplierProductId = a.SupplierProductId,
                                    MaxAdults = a.MaxAdults,
                                    MaxChild = a.MaxChild,
                                    MapId = a.MapId,
                                    MappingStatus = a.MappingStatus,
                                    MaxGuestOccupancy = a.MaxGuestOccupancy,
                                    MaxInfants = a.MaxInfants,
                                    Quantity = a.Quantity,
                                    SupplierRoomCategory = a.SupplierRoomCategory,
                                    RatePlan = a.RatePlan,
                                    RatePlanCode = a.RatePlanCode,
                                    SupplierProductName = a.SupplierProductName,
                                    SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                    SupplierRoomId = a.SupplierRoomId,
                                    SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                    Supplier_Id = a.Supplier_Id,
                                    Tx_ReorderedName = a.Tx_ReorderedName,
                                    TX_RoomName = a.TX_RoomName,
                                    Tx_StrippedName = a.Tx_StrippedName,
                                    RoomDescription = a.RoomDescription,
                                    stg_SupplierHotelRoomMapping_Id = s.stg_SupplierHotelRoomMapping_Id,
                                    Oldstg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                    ActionType = (a.SupplierRoomName != s.RoomName) ? "UPDATE" : ""
                                    //stg_AccoMapping_Id = (a.ProductName != s.ProductName) ? s.stg_AccoMapping_Id : Guid.Empty
                                }).ToList();


                insertSTGList = stg.Where(w => !toUpdate.Any(a => a.SupplierProductId == w.SupplierProductId && a.SupplierRoomTypeCode == w.SupplierRoomTypeCode)).ToList();
                updateMappingList = toUpdate.Where(w => w.SupplierRoomName != w.OldSupplierRoomName).ToList();
            }
        }

        public bool RoomTypeMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;
            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            fileRQ.SupplierImportFile_Id = File_Id;
            file = staticdata.GetStaticDataFileDetail(fileRQ);
            if (obj != null)
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> clsSTGHotel = new List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping>();
                List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping> clsSTGHotelInsert = new List<DataContracts.STG.DC_stg_SupplierHotelRoomMapping>();
                List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> clsMappingHotel = new List<DC_Accommodation_SupplierRoomTypeMap_SearchRS>();

                CallLogVerbose(File_Id, "MAP", "Fetching Staged Room Types.");
                DataContracts.STG.DC_stg_SupplierHotelRoomMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierHotelRoomMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.Supplier_Id = CurSupplier_Id;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                clsSTGHotel = staticdata.GetSTGRoomTypeData(RQ);
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.");
                /*DC_Accommodation_SupplierRoomTypeMap_SearchRQ RQM = new DC_Accommodation_SupplierRoomTypeMap_SearchRQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQM.Supplier_Id = CurSupplier_Id;
                if (!string.IsNullOrWhiteSpace(CurSupplierName))
                    RQM.SupplierName = CurSupplierName;
                RQM.PageNo = 0;
                RQM.PageSize = int.MaxValue;
                RQM.CalledFromTLGX = "TLGX";
                clsMappingHotel = SupplierRoomTypeMapping_Search(RQM);*/

                CheckRoomTypeAlreadyExist(CurSupplier_Id, clsSTGHotel, out clsMappingHotel, out clsSTGHotelInsert);
                PLog.PercentageValue = 26;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Updating Existing Room Types.");
                /*clsMappingHotel = clsMappingHotel.Select(c =>
                {
                    c.SupplierRoomName = (clsSTGHotel
                    .Where(s => s.SupplierProductId == c.SupplierProductId && s.SupplierRoomTypeCode == c.SupplierRoomTypeCode && s.SupplierRoomId == c.SupplierRoomId)
                    .Select(s1 => s1.RoomName)
                    .FirstOrDefault()
                    ) ?? c.SupplierRoomName;
                    //c.Edit_Date = DateTime.Now;
                    //c.Edit_User = "TLGX_DataHandler";
                    c.stg_SupplierHotelRoomMapping_Id = (clsSTGHotel
                    .Where(s => s.SupplierProductId == c.SupplierProductId && s.SupplierRoomTypeCode == c.SupplierRoomTypeCode && s.SupplierRoomId == c.SupplierRoomId)
                    .Select(s1 => s1.stg_SupplierHotelRoomMapping_Id)
                    .FirstOrDefault()
                    ); //?? c.stg_SupplierHotelRoomMapping_Id;
                    c.ActionType = "UPDATE";
                    return c;
                }).ToList();*/

                List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_SupplierHotelRoomMapping_Id != null && a.ActionType == "UPDATE"
                    && (a.stg_SupplierHotelRoomMapping_Id ?? Guid.Empty) != Guid.Empty).Select
                   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                   {
                       STG_Mapping_Table_Id = Guid.NewGuid(),
                       File_Id = obj.File_Id,
                       STG_Id = g.stg_SupplierHotelRoomMapping_Id,
                       Mapping_Id = g.Accommodation_SupplierRoomTypeMapping_Id,
                       Batch = obj.CurrentBatch ?? 0
                   }));


                PLog.PercentageValue = 37;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Checking for New Room Types in File.");
                //clsSTGHotelInsert = clsSTGHotel.Where(p => !clsMappingHotel.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                // && (
                //    (((p2.SupplierProductName ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierProductName ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.SupplierProductId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierProductId ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.SupplierRoomTypeCode ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomTypeCode ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.SupplierRoomId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomId ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.SupplierRoomName ?? string.Empty).ToString().Trim().ToUpper() == (p.RoomName ?? string.Empty).ToString().Trim().ToUpper()))
                //    && (((p2.SupplierRoomCategoryId ?? string.Empty).ToString().Trim().ToUpper() == (p.SupplierRoomCategoryId ?? string.Empty).ToString().Trim().ToUpper()))
                //))).ToList();
                PLog.PercentageValue = 48;
                USD.AddStaticDataUploadProcessLog(PLog);

                clsSTGHotel.RemoveAll(p => clsSTGHotelInsert.Any(p2 => (p2.stg_SupplierHotelRoomMapping_Id == p.stg_SupplierHotelRoomMapping_Id)));

                //clsMappingHotel.RemoveAll(p => p.SupplierRoomName == p.OldSupplierRoomName && (((p.stg_SupplierHotelRoomMapping_Id == Guid.Empty) ? p.Oldstg_SupplierHotelRoomMapping_Id : p.stg_SupplierHotelRoomMapping_Id) == p.Oldstg_SupplierHotelRoomMapping_Id));

                CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.");
                clsMappingHotel.RemoveAll(p => p.SupplierRoomName == p.OldSupplierRoomName);

                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Inserting New Room Types.");
                clsMappingHotel.InsertRange(clsMappingHotel.Count, clsSTGHotelInsert.Select
                    (g => new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                    {
                        Accommodation_SupplierRoomTypeMapping_Id = Guid.NewGuid(),
                        SupplierRoomName = g.RoomName,
                        OldSupplierRoomName = g.RoomName,
                        SupplierRoomCategoryId = g.SupplierRoomCategoryId,
                        Accommodation_Id = null,
                        Accommodation_RoomInfo_Id = null,
                        Accommodation_RoomInfo_Name = null,
                        CommonProductId = null,
                        Location = null,
                        MapId = null,
                        MappingStatus = "UNMAPPED",
                        MaxAdults = g.MaxAdults,
                        MaxChild = g.MaxChild,
                        MaxGuestOccupancy = g.MaxGuestOccupancy,
                        MaxInfants = g.MaxInfant,
                        ProductName = null,
                        Quantity = (!string.IsNullOrWhiteSpace(g.Quantity)) ? Convert.ToInt32(g.Quantity) : 0,
                        RatePlan = g.RatePlan,
                        RatePlanCode = g.RatePlanCode,
                        RoomTypeAttributes = null,
                        SupplierName = g.SupplierName,
                        SupplierProductId = g.SupplierProductId,
                        SupplierProductName = g.SupplierProductName,
                        SupplierRoomCategory = g.SupplierRoomCategory,
                        SupplierRoomId = g.SupplierRoomId,
                        SupplierRoomTypeCode = g.SupplierRoomTypeCode,
                        Supplier_Id = g.Supplier_Id,
                        stg_SupplierHotelRoomMapping_Id = g.stg_SupplierHotelRoomMapping_Id,
                        ActionType = "INSERT"
                    }));

                lstobj.InsertRange(lstobj.Count, clsMappingHotel.Where(a => a.stg_SupplierHotelRoomMapping_Id != null && a.ActionType == "INSERT"
                    && (a.stg_SupplierHotelRoomMapping_Id ?? Guid.Empty) != Guid.Empty)
                .Select
                   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                   {
                       STG_Mapping_Table_Id = Guid.NewGuid(),
                       File_Id = obj.File_Id,
                       STG_Id = g.stg_SupplierHotelRoomMapping_Id,
                       Mapping_Id = g.Accommodation_SupplierRoomTypeMapping_Id,
                       Batch = obj.CurrentBatch ?? 0
                   }));
                bool idinsert = AddSTGMappingTableIDs(lstobj);

                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.");
                if (clsMappingHotel.Count > 0)
                {
                    ret = SupplierRoomTypeMapping_InsertUpdate(clsMappingHotel);
                    /*if (obj.CurrentBatch == 1)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.FinalStatus = file[0].STATUS;
                        objStat.TotalRows = clsMappingHotel.Count;
                        objStat.Process_Date = DateTime.Now;
                        objStat.Process_User = file[0].PROCESS_USER;
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                    }*/
                }
            }

            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch " + (obj.CurrentBatch ?? 0).ToString());
            return ret;
        }

        public void DataHandler_RoomName_Attributes_Update(DC_SupplierRoomName_Details SRNDetails)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SRNDetails.AttributeList != null && SRNDetails.RoomTypeMap_Id != null)
                    {
                        context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in SRNDetails.AttributeList
                                                                                   select new Accommodation_SupplierRoomTypeAttributes
                                                                                   {
                                                                                       RoomTypeMapAttribute_Id = Guid.NewGuid(),
                                                                                       RoomTypeMap_Id = SRNDetails.RoomTypeMap_Id,
                                                                                       SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                                                                                       SystemAttributeKeyword = a.SystemAttributeKeyword,
                                                                                       SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                                                                                   }).ToList());
                        context.SaveChanges();
                    }

                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while inserting supplier room type mapping attribute list",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> SupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var roomTypeSearch = from a in context.Accommodation_SupplierRoomTypeMapping
                                         where a.Supplier_Id == (obj.Supplier_Id ?? a.Supplier_Id)
                                         && a.SupplierName.Trim().ToUpper() == (obj.SupplierName.Trim().ToUpper() ?? a.SupplierName.Trim().ToUpper())
                                         select new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                         {
                                             Accommodation_Id = a.Accommodation_Id,
                                             SupplierName = a.SupplierName,
                                             Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                             Accommodation_RoomInfo_Name = a.SupplierRoomName,
                                             SupplierRoomName = a.SupplierRoomName,
                                             OldSupplierRoomName = a.SupplierRoomName,
                                             Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                             SupplierProductId = a.SupplierProductId,
                                             MaxAdults = a.MaxAdults,
                                             MaxChild = a.MaxChild,
                                             MapId = a.MapId,
                                             MappingStatus = a.MappingStatus,
                                             MaxGuestOccupancy = a.MaxGuestOccupancy,
                                             MaxInfants = a.MaxInfants,
                                             Quantity = a.Quantity,
                                             SupplierRoomCategory = a.SupplierRoomCategory,
                                             RatePlan = a.RatePlan,
                                             RatePlanCode = a.RatePlanCode,
                                             SupplierProductName = a.SupplierProductName,
                                             SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                             SupplierRoomId = a.SupplierRoomId,
                                             SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                             Supplier_Id = a.Supplier_Id,
                                             Tx_ReorderedName = a.Tx_ReorderedName,
                                             TX_RoomName = a.TX_RoomName,
                                             Tx_StrippedName = a.Tx_StrippedName,
                                             RoomDescription = a.RoomDescription,
                                             stg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id,
                                             Oldstg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id
                                         };
                    var result = roomTypeSearch.ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching product supplier mapping",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> AccomodationSupplierRoomTypeMapping_Search(DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    var Accommodation_SupplierRoomTypeMapping = (from a in context.Accommodation_SupplierRoomTypeMapping select a).AsQueryable();
                    var Accommodation = (from a in context.Accommodations select a).AsQueryable();
                    var Country = (from a in context.m_CountryMaster select a).AsQueryable();
                    var City = (from a in context.m_CityMaster select a).AsQueryable();
                    var Accommodation_RoomInfo = (from a in context.Accommodation_RoomInfo select a).AsQueryable();
                    var Accommodation_SupplierRoomTypeAttributes = (from a in context.Accommodation_SupplierRoomTypeAttributes select a).AsQueryable();
                    var Keyword = (from a in context.m_keyword select a).AsQueryable();

                    if (obj.Supplier_Id != null)
                    {
                        Accommodation_SupplierRoomTypeMapping = Accommodation_SupplierRoomTypeMapping.Where(w => w.Supplier_Id == obj.Supplier_Id).Select(s => s);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.SupplierRoomName))
                    {
                        Accommodation_SupplierRoomTypeMapping = Accommodation_SupplierRoomTypeMapping.Where(w => w.SupplierRoomName.Contains(obj.SupplierRoomName)).Select(s => s);
                    }
                    if (!string.IsNullOrWhiteSpace(obj.Status))
                    {
                        Accommodation_SupplierRoomTypeMapping = Accommodation_SupplierRoomTypeMapping.Where(w => w.MappingStatus == obj.Status).Select(s => s);
                    }

                    IQueryable<Guid> AccommodationIds = null;
                    if (!string.IsNullOrWhiteSpace(obj.ProductName))
                    {
                        AccommodationIds = Accommodation.Where(w => w.HotelName.ToUpper() == obj.ProductName.ToUpper().Trim()).Select(s => s.Accommodation_Id).AsQueryable();
                    }

                    if (AccommodationIds != null)
                    {
                        Accommodation_SupplierRoomTypeMapping = Accommodation_SupplierRoomTypeMapping.Where(w => AccommodationIds.Contains(w.Accommodation_Id ?? Guid.Empty)).Select(s => s);
                        Accommodation = Accommodation.Where(w => AccommodationIds.Contains(w.Accommodation_Id)).Select(s => s);
                        Accommodation_RoomInfo = Accommodation_RoomInfo.Where(w => AccommodationIds.Contains(w.Accommodation_Id ?? Guid.Empty)).Select(s => s);
                    }

                    if (obj.Country != null)
                    {
                        Accommodation = Accommodation.Where(w => w.Country_Id == obj.Country).Select(s => s);
                        Country = Country.Where(w => w.Country_Id == obj.Country).Select(s => s);
                    }

                    if (obj.City != null)
                    {
                        Accommodation = Accommodation.Where(w => w.City_Id == obj.City).Select(s => s);
                        City = City.Where(w => w.City_Id == obj.City).Select(s => s);
                    }

                    var roomTypeSearch = (from asrtm in Accommodation_SupplierRoomTypeMapping
                                          join acco in Accommodation on asrtm.Accommodation_Id equals acco.Accommodation_Id
                                          join country in Country on acco.Country_Id equals country.Country_Id
                                          join city in City on acco.City_Id equals city.City_Id
                                          join accori in Accommodation_RoomInfo on new { AccoId = acco.Accommodation_Id, AccoRIId = asrtm.Accommodation_RoomInfo_Id ?? Guid.Empty } equals new { AccoId = accori.Accommodation_Id ?? Guid.Empty, AccoRIId = accori.Accommodation_RoomInfo_Id } into accoritemp
                                          from accorinew in accoritemp.DefaultIfEmpty()
                                          select new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                          {
                                              Accommodation_Id = asrtm.Accommodation_Id,
                                              Accommodation_RoomInfo_Id = accorinew.Accommodation_RoomInfo_Id,
                                              Accommodation_RoomInfo_Name = accorinew.RoomCategory,
                                              Accommodation_SupplierRoomTypeMapping_Id = asrtm.Accommodation_SupplierRoomTypeMapping_Id,
                                              CommonProductId = acco.CompanyHotelID.ToString(),
                                              Location = city.Name + "(" + country.Code + ")",
                                              MapId = asrtm.MapId,
                                              MappingStatus = asrtm.MappingStatus,
                                              MaxAdults = asrtm.MaxAdults,
                                              MaxChild = asrtm.MaxChild,
                                              MaxGuestOccupancy = asrtm.MaxGuestOccupancy,
                                              MaxInfants = asrtm.MaxInfants,
                                              NumberOfRooms = (Accommodation_RoomInfo.Where(w => w.Accommodation_Id == acco.Accommodation_Id).Count()),
                                              ProductName = acco.HotelName,
                                              Quantity = asrtm.Quantity,
                                              RatePlan = asrtm.RatePlan,
                                              RatePlanCode = asrtm.RatePlanCode,
                                              RoomTypeAttributes = (Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == asrtm.Accommodation_SupplierRoomTypeMapping_Id).Select(s => new DC_SupplierRoomTypeAttributes { Accommodation_SupplierRoomTypeMapAttribute_Id = s.RoomTypeMapAttribute_Id, Accommodation_SupplierRoomTypeMap_Id = s.RoomTypeMap_Id, SupplierRoomTypeAttribute = s.SupplierRoomTypeAttribute, SystemAttributeKeyword = s.SystemAttributeKeyword, SystemAttributeKeyword_Id = s.SystemAttributeKeyword_Id, IconClass = Keyword.Where(kw => kw.Keyword_Id == s.SystemAttributeKeyword_Id).Select(kws => kws.Icon).FirstOrDefault() }).ToList()),
                                              SupplierName = asrtm.SupplierName,
                                              SupplierProductId = asrtm.SupplierProductId,
                                              SupplierProductName = asrtm.SupplierProductName,
                                              SupplierRoomCategory = asrtm.SupplierRoomCategory,
                                              SupplierRoomCategoryId = asrtm.SupplierRoomCategoryId,
                                              SupplierRoomId = asrtm.SupplierRoomId,
                                              SupplierRoomName = asrtm.SupplierRoomName,
                                              SupplierRoomTypeCode = asrtm.SupplierRoomTypeCode,
                                              Supplier_Id = asrtm.Supplier_Id,
                                              TotalRecords = 0,
                                              Tx_ReorderedName = asrtm.Tx_ReorderedName,
                                              TX_RoomName = asrtm.TX_RoomName,
                                              Tx_StrippedName = asrtm.Tx_StrippedName,
                                              RoomDescription = asrtm.RoomDescription
                                          }).AsQueryable();

                    int total = roomTypeSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var roomTypeSearchList = (from a in roomTypeSearch
                                              orderby a.Accommodation_SupplierRoomTypeMapping_Id
                                              select new DC_Accommodation_SupplierRoomTypeMap_SearchRS
                                              {
                                                  Accommodation_Id = a.Accommodation_Id,
                                                  Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                                  Accommodation_RoomInfo_Name = a.Accommodation_RoomInfo_Name,
                                                  Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                                  CommonProductId = a.CommonProductId,
                                                  Location = a.Location,
                                                  MapId = a.MapId,
                                                  MappingStatus = a.MappingStatus,
                                                  MaxAdults = a.MaxAdults,
                                                  MaxChild = a.MaxChild,
                                                  MaxGuestOccupancy = a.MaxGuestOccupancy,
                                                  MaxInfants = a.MaxInfants,
                                                  NumberOfRooms = a.NumberOfRooms,
                                                  ProductName = a.ProductName,
                                                  Quantity = a.Quantity,
                                                  RatePlan = a.RatePlan,
                                                  RatePlanCode = a.RatePlanCode,
                                                  RoomTypeAttributes = a.RoomTypeAttributes,
                                                  SupplierName = a.SupplierName,
                                                  SupplierProductId = a.SupplierProductId,
                                                  SupplierProductName = a.SupplierProductName,
                                                  SupplierRoomCategory = a.SupplierRoomCategory,
                                                  SupplierRoomCategoryId = a.SupplierRoomCategoryId,
                                                  SupplierRoomId = a.SupplierRoomId,
                                                  SupplierRoomName = a.SupplierRoomName,
                                                  SupplierRoomTypeCode = a.SupplierRoomTypeCode,
                                                  Supplier_Id = a.Supplier_Id,
                                                  TotalRecords = total,
                                                  Tx_ReorderedName = a.Tx_ReorderedName,
                                                  TX_RoomName = a.TX_RoomName,
                                                  Tx_StrippedName = a.Tx_StrippedName,
                                                  RoomDescription = a.RoomDescription,
                                              }).Skip(skip).Take(obj.PageSize);

                    var result = roomTypeSearchList.ToList();

                    if (string.IsNullOrWhiteSpace(obj.CalledFromTLGX))
                    {
                        foreach (var item in result)
                        {
                            if (item.Accommodation_RoomInfo_Id == null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.Tx_StrippedName))
                                {
                                    var resultRoomCategory = Accommodation_RoomInfo.Where(w => w.Accommodation_Id == item.Accommodation_Id && w.RoomCategory.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim() == item.Tx_StrippedName.ToLower().Replace("room", string.Empty).Replace("rooms", string.Empty).Trim()).Select(s => s).FirstOrDefault();
                                    if (resultRoomCategory != null)
                                    {
                                        item.Accommodation_RoomInfo_Id = resultRoomCategory.Accommodation_RoomInfo_Id;
                                        item.Accommodation_RoomInfo_Name = resultRoomCategory.RoomCategory;
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while searching accomodation product supplier room type mapping",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_UpdateMap(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_Update> obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    foreach (DC_Accommodation_SupplierRoomTypeMap_Update item in obj)
                    {
                        if (item.Accommodation_SupplierRoomTypeMapping_Id != null && !string.IsNullOrWhiteSpace(item.Status))
                        {
                            var accoSuppRoomTypeMap = context.Accommodation_SupplierRoomTypeMapping.Find(item.Accommodation_SupplierRoomTypeMapping_Id);
                            var acco = context.Accommodations.Find(item.Accommodation_Id);

                            if (acco != null && accoSuppRoomTypeMap != null)
                            {
                                //item.Status -- ADD
                                if (item.Status.ToUpper() == "ADD")
                                {
                                    //Check if Same room already exists. If yes, fetch and map to existing instead of adding a new one.
                                    var Acco_RoomInfo = (from a in context.Accommodation_RoomInfo select a).AsQueryable();
                                    Acco_RoomInfo = Acco_RoomInfo.Where(w => w.Accommodation_Id == item.Accommodation_Id && w.RoomCategory.ToUpper().Trim() == item.RoomCategory.ToUpper().Trim()).Select(s => s);
                                    var ExistingRoomInfo = Acco_RoomInfo.ToList();
                                    if (ExistingRoomInfo.Count > 0)
                                    {
                                        //Update existing Room ID to SRTM table
                                        item.Accommodation_RoomInfo_Id = ExistingRoomInfo[0].Accommodation_RoomInfo_Id;
                                        item.Status = "MAPPED";

                                    }
                                    else
                                    {
                                        Guid _newRoomId = Guid.NewGuid();

                                        Accommodation_RoomInfo _newObj = new Accommodation_RoomInfo();
                                        _newObj.Accommodation_RoomInfo_Id = _newRoomId;
                                        _newObj.Accommodation_Id = acco.Accommodation_Id;
                                        _newObj.Legacy_Htl_Id = acco.CompanyHotelID;
                                        _newObj.RoomId = CommonFunctions.GenerateRoomId(Guid.Parse(item.Accommodation_Id.ToString()));
                                        _newObj.NoOfRooms = accoSuppRoomTypeMap.Quantity;
                                        _newObj.Description = accoSuppRoomTypeMap.SupplierRoomName + " : " + accoSuppRoomTypeMap.RoomDescription;
                                        _newObj.RoomCategory = item.RoomCategory;

                                        _newObj.Create_User = item.Edit_User;
                                        _newObj.Create_Date = DateTime.Now;
                                        _newObj.IsActive = true;

                                        var accoSuppRoomTypeMapAttributes = context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == item.Accommodation_SupplierRoomTypeMapping_Id).ToList();
                                        if (accoSuppRoomTypeMapAttributes != null)
                                        {
                                            if (accoSuppRoomTypeMapAttributes.Count > 0)
                                            {
                                                foreach (var attribute in accoSuppRoomTypeMapAttributes)
                                                {
                                                    var keyword = context.m_keyword.Find(attribute.SystemAttributeKeyword_Id);
                                                    if (keyword != null)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(keyword.AttributeLevel))
                                                        {
                                                            if (keyword.AttributeLevel.ToUpper() == "ROOM AMENITY")
                                                            {
                                                                Guid _newFacilityId = Guid.NewGuid();
                                                                Accommodation_RoomFacility RF = new Accommodation_RoomFacility();
                                                                RF.Accommodation_Id = acco.Accommodation_Id;
                                                                RF.Accommodation_RoomFacility_Id = _newFacilityId;
                                                                RF.Accommodation_RoomInfo_Id = _newRoomId;
                                                                RF.AmenityName = keyword.Keyword;
                                                                RF.AmenityType = keyword.AttributeSubLevel;
                                                                RF.Create_Date = DateTime.Now;
                                                                RF.Create_User = item.Edit_User;
                                                                context.Accommodation_RoomFacility.Add(RF);
                                                                RF = null;
                                                                attribute.Accommodation_RoomFacility_Id = _newFacilityId;
                                                            }
                                                            else if (keyword.AttributeLevel.ToUpper() == "ROOM INFO")
                                                            {
                                                                if (!string.IsNullOrWhiteSpace(keyword.AttributeSubLevel))
                                                                {
                                                                    if (keyword.AttributeSubLevel.ToUpper() == "NUMBER OF ROOMS")
                                                                    {
                                                                        int NoOfRooms = 0;
                                                                        int.TryParse(attribute.SystemAttributeKeyword, out NoOfRooms);
                                                                        _newObj.NoOfRooms = NoOfRooms;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM CATEGORY")
                                                                    {
                                                                        _newObj.RoomCategory = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "FLOOR NAME")
                                                                    {
                                                                        _newObj.FloorName = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "FLOOR NUMBER")
                                                                    {
                                                                        _newObj.FloorNumber = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM VIEW")
                                                                    {
                                                                        _newObj.RoomView = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM DECOR")
                                                                    {
                                                                        _newObj.RoomDecor = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "BED TYPE")
                                                                    {
                                                                        _newObj.BedType = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "BATHROOM TYPE")
                                                                    {
                                                                        _newObj.BathRoomType = keyword.AttributeSubLevelValue;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "SMOKING")
                                                                    {
                                                                        _newObj.Smoking = ((keyword.AttributeSubLevelValue ?? string.Empty) == "YES" ? true : false);
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "ROOM SIZE")
                                                                    {
                                                                        _newObj.RoomSize = attribute.SystemAttributeKeyword;
                                                                    }
                                                                    else if (keyword.AttributeSubLevel.ToUpper() == "INTER ROOMS")
                                                                    {
                                                                        int NoOfInterRooms = 0;
                                                                        int.TryParse(attribute.SystemAttributeKeyword, out NoOfInterRooms);
                                                                        _newObj.NoOfInterconnectingRooms = NoOfInterRooms;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        context.Accommodation_RoomInfo.Add(_newObj);

                                        //Update new Room ID to SRTM table
                                        item.Accommodation_RoomInfo_Id = _newRoomId;
                                        item.Status = "MAPPED";
                                    }
                                }
                                else if (item.Status.ToUpper() == "UNMAPPED")
                                {
                                    item.Accommodation_RoomInfo_Id = null;
                                }

                                accoSuppRoomTypeMap.Accommodation_RoomInfo_Id = item.Accommodation_RoomInfo_Id;
                                accoSuppRoomTypeMap.MappingStatus = item.Status.ToUpper();
                                accoSuppRoomTypeMap.Edit_Date = DateTime.Now;
                                accoSuppRoomTypeMap.Edit_User = item.Edit_User;

                            }
                        }
                        context.SaveChanges();
                    }

                }

                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "All Valid Records are successfully updated." };
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation product supplier mapping" + ex.Message, ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool SupplierRoomTypeMapping_InsertUpdate(List<DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS> lstobj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (DC_Accommodation_SupplierRoomTypeMap_SearchRS obj in lstobj)
                    {
                        if (obj.Accommodation_SupplierRoomTypeMapping_Id == null)
                        {
                            continue;
                        }

                        var search = (from a in context.Accommodation_SupplierRoomTypeMapping
                                      where a.Accommodation_SupplierRoomTypeMapping_Id == obj.Accommodation_SupplierRoomTypeMapping_Id
                                      select a).FirstOrDefault();
                        if (search == null)
                        {
                            DataLayer.Accommodation_SupplierRoomTypeMapping objNew = new DataLayer.Accommodation_SupplierRoomTypeMapping()
                            {
                                Accommodation_SupplierRoomTypeMapping_Id = obj.Accommodation_SupplierRoomTypeMapping_Id,
                                Accommodation_Id = (context.Accommodation_ProductMapping.Where(w => w.SupplierProductReference == obj.SupplierProductId).Select(a => a.Accommodation_Id).FirstOrDefault()),
                                Accommodation_RoomInfo_Id = null,
                                SupplierProductId = obj.SupplierProductId,
                                Create_Date = DateTime.Now,
                                Create_User = "TLGX",
                                Edit_Date = null,
                                Edit_User = null,
                                MapId = null,
                                MappingStatus = obj.MappingStatus,
                                MaxAdults = obj.MaxAdults,
                                MaxChild = obj.MaxChild,
                                MaxGuestOccupancy = obj.MaxGuestOccupancy,
                                MaxInfants = obj.MaxInfants,
                                Quantity = obj.Quantity,
                                RatePlan = obj.RatePlan,
                                RatePlanCode = obj.RatePlanCode,
                                //stg_SupplierHotelRoomMapping_Id = obj.stg_SupplierHotelRoomMapping_Id,
                                SupplierName = obj.SupplierName,
                                SupplierProductName = obj.SupplierProductName,
                                SupplierRoomCategory = obj.SupplierRoomCategory,
                                SupplierRoomCategoryId = obj.SupplierRoomCategoryId,
                                SupplierRoomId = obj.SupplierRoomId,
                                SupplierRoomName = obj.SupplierRoomName,
                                SupplierRoomTypeCode = obj.SupplierRoomTypeCode,
                                Supplier_Id = obj.Supplier_Id,
                                Tx_ReorderedName = null,
                                TX_RoomName = null,
                                Tx_StrippedName = null
                            };
                            context.Accommodation_SupplierRoomTypeMapping.Add(objNew);
                        }
                        else
                        {
                            search.Accommodation_RoomInfo_Id = obj.Accommodation_RoomInfo_Id;
                            search.MappingStatus = obj.MappingStatus;
                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = "TLGX";
                            //search.stg_SupplierHotelRoomMapping_Id = obj.stg_SupplierHotelRoomMapping_Id;
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                    //context.USP_UpdateMapID("roomtype");
                }
                return true;
            }
            catch (Exception e)
            {
                //throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                //{
                //    ErrorMessage = "Error while updating accomodation product supplier mapping",
                //    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                //});
                return false;
            }
        }

        public List<DC_SupplierRoomType_TTFU_RQ> GetRoomTypeMapping_For_TTFU(DataContracts.Masters.DC_Supplier obj)
        {
            string CurSupplierName = obj.Name;
            Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

            using (ConsumerEntities context = new ConsumerEntities())
            {
                var res = (from a in context.Accommodation_SupplierRoomTypeMapping
                           join j in context.STG_Mapping_TableIds on a.Accommodation_SupplierRoomTypeMapping_Id equals j.Mapping_Id
                           join s in context.stg_SupplierHotelRoomMapping on j.STG_Id equals s.stg_SupplierHotelRoomMapping_Id  //a.stg_SupplierHotelRoomMapping_Id equals s.stg_SupplierHotelRoomMapping_Id
                           select new DC_SupplierRoomType_TTFU_RQ
                           {
                               Acco_RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                               Edit_User = a.Edit_User
                           }).ToList();
                return res;
            }
        }

        public DataContracts.DC_Message AccomodationSupplierRoomTypeMapping_TTFUALL(List<DC_SupplierRoomType_TTFU_RQ> Acco_RoomTypeMap_Ids)
        {
            try
            {
                PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                PLog.SupplierImportFile_Id = Acco_RoomTypeMap_Ids[0].File_Id;
                PLog.Step = "KEYWORD";
                PLog.Status = "KEYWORDREPLACE";
                PLog.CurrentBatch = Acco_RoomTypeMap_Ids[0].CurrentBatch ?? 0;
                PLog.TotalBatch = Acco_RoomTypeMap_Ids[0].TotalBatch ?? 0;
                bool ISProgressLog = false;

                if (Acco_RoomTypeMap_Ids[0].File_Id.HasValue)
                    ISProgressLog = true;

                #region Get All Keywords & Room Names

                List<DataContracts.Masters.DC_Keyword> Keywords = new List<DataContracts.Masters.DC_Keyword>();
                using (DL_Masters objDL = new DL_Masters())
                {
                    Keywords = objDL.SearchKeyword(null);
                }

                List<DataContracts.Masters.DC_Keyword> Attributes = Keywords.Where(w => w.Attribute == true && !w.Keyword.StartsWith("##")).ToList();

                //Get All Supplier Room Type Name
                List<DC_SupplierRoomName_Details> asrtmd;
                string Edit_User = string.Empty;
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (Acco_RoomTypeMap_Ids != null)
                    {
                        var MapIdsFilter = (from a in Acco_RoomTypeMap_Ids select a.Acco_RoomTypeMap_Id).ToList();

                        asrtmd = context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                     .Where(w => MapIdsFilter.Contains(w.Accommodation_SupplierRoomTypeMapping_Id))
                                     .OrderBy(o => o.Accommodation_SupplierRoomTypeMapping_Id)
                                     .Select(s => new DC_SupplierRoomName_Details
                                     {
                                         RoomTypeMap_Id = s.Accommodation_SupplierRoomTypeMapping_Id,
                                         SupplierRoomName = s.SupplierRoomName
                                     }).ToList();
                        if (ISProgressLog)
                        {
                            PLog.PercentageValue = 25;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        if (asrtmd.Count > 0)
                        {
                            Edit_User = Acco_RoomTypeMap_Ids[0].Edit_User;
                        }
                        else
                        {
                            Edit_User = "TTFU BY SYSTEM";
                        }

                    }
                    else
                    {
                        asrtmd = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                  orderby a.Accommodation_SupplierRoomTypeMapping_Id
                                  select new DC_SupplierRoomName_Details
                                  {
                                      RoomTypeMap_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                      SupplierRoomName = a.SupplierRoomName
                                  }).ToList();
                        if (ISProgressLog)
                        {
                            PLog.PercentageValue = 25;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        Edit_User = "TTFU BY SYSTEM";
                    }
                }

                if (ISProgressLog)
                {
                    PLog.PercentageValue = 15;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
                #endregion


                int i = 0;
                List<DC_SupplierRoomName_AttributeList> AttributeList;
                var unitNumerMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN" };
                foreach (DC_SupplierRoomName_Details srn in asrtmd)
                {
                    i = i + 1;

                    AttributeList = new List<DC_SupplierRoomName_AttributeList>();

                    string BaseRoomName = srn.SupplierRoomName;

                    #region PRE TTFU

                    #region HTML Decode
                    BaseRoomName = System.Web.HttpUtility.HtmlDecode(BaseRoomName);
                    #endregion

                    #region To Upper
                    BaseRoomName = BaseRoomName.ToUpper();
                    #endregion

                    #region Replace the braces
                    BaseRoomName = BaseRoomName.Replace('{', '(');
                    BaseRoomName = BaseRoomName.Replace('}', ')');
                    BaseRoomName = BaseRoomName.Replace('[', '(');
                    BaseRoomName = BaseRoomName.Replace(']', ')');

                    BaseRoomName = BaseRoomName.Replace("( ", "(");
                    BaseRoomName = BaseRoomName.Replace(" )", ")");

                    BaseRoomName = BaseRoomName.Replace(",", " ");
                    #endregion

                    //Necessary Replace
                    //BaseRoomName = BaseRoomName.Replace("/", " OR ");

                    #region Replace Multiple whitespaces into One Whitespace
                    BaseRoomName = System.Text.RegularExpressions.Regex.Replace(BaseRoomName, @"\s{2,}", " ");
                    #endregion

                    #region trim both end
                    BaseRoomName = BaseRoomName.Trim();
                    #endregion

                    #endregion

                    #region Take only valid characters
                    string RoomName_ValidChars = string.Empty;
                    foreach (char c in BaseRoomName)
                    {
                        if ((Convert.ToInt16(c) >= 32 && Convert.ToInt16(c) <= 196))// || (Convert.ToInt16(c) >= 97 && Convert.ToInt16(c) <= 122) || Convert.ToInt16(c) == 32)
                        {
                            RoomName_ValidChars = RoomName_ValidChars + c;
                        }
                    }

                    BaseRoomName = RoomName_ValidChars;
                    #endregion

                    #region Check for Spaced Keyword and Replace
                    List<DataContracts.Masters.DC_Keyword> SpacedKeywords = Keywords.Where(w => !w.Keyword.StartsWith("##") && w.Attribute == false && w.Alias.Any(a => a.Value.Contains(' '))).ToList();
                    foreach (DataContracts.Masters.DC_Keyword spacedkey in SpacedKeywords.OrderBy(o => o.Sequence))
                    {
                        var spacedAliases = spacedkey.Alias.Where(w => w.Value.Contains(' ')).OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                        foreach (var alias in spacedAliases)
                        {
                            if (BaseRoomName.Contains(alias.Value.ToUpper()))
                            {
                                BaseRoomName = BaseRoomName.Replace(alias.Value.ToUpper(), spacedkey.Keyword);
                                BaseRoomName = BaseRoomName.Replace("()", string.Empty);
                                BaseRoomName = BaseRoomName.Trim();

                                alias.NewHits += 1;
                            }
                        }
                    }
                    #endregion

                    #region Keyword Replacement
                    //Split words and replace keywords
                    string[] roomWords = BaseRoomName.Split(' ');

                    BaseRoomName = " " + BaseRoomName + " ";

                    foreach (string word in roomWords)
                    {
                        DataContracts.Masters.DC_Keyword keywordSearch = Keywords.Where(k => k.Alias.Any(a => a.Value.ToUpper() == word.ToUpper()) && k.Attribute == false && !k.Keyword.StartsWith("##")).FirstOrDefault();

                        if (keywordSearch != null)
                        {
                            BaseRoomName = BaseRoomName.Replace(" " + word + " ", " " + keywordSearch.Keyword + " ");
                            var foundAlias = keywordSearch.Alias.Where(w => w.Value.ToUpper() == word.ToUpper()).FirstOrDefault();
                            foundAlias.NoOfHits += 1;
                        }

                        keywordSearch = null;
                    }

                    BaseRoomName = BaseRoomName.Trim();

                    #endregion

                    //Transformed Supplier RoomName
                    srn.TX_SupplierRoomName = BaseRoomName;

                    #region Attribute Extraction

                    bool isRoomHaveAttribute = false;
                    string sAttributeAlias = string.Empty;
                    foreach (var Attribute in Attributes.OrderBy(o => o.Sequence))
                    {
                        //if (Attribute.Keyword == "NON-SMOKING-ROOM")
                        //{
                        //    int ifdfs = 1;
                        //}

                        isRoomHaveAttribute = false;

                        var aliases = Attribute.Alias.OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                        foreach (var alias in aliases)
                        {

                            //if(alias.Value.ToUpper() == "RO")
                            //{
                            //    int iStop = 1;
                            //}

                            isRoomHaveAttribute = false;
                            sAttributeAlias = alias.Value.Replace(",", " ").Trim().ToUpper();
                            sAttributeAlias = System.Text.RegularExpressions.Regex.Replace(sAttributeAlias, @"\s{2,}", " ");

                            if (sAttributeAlias.StartsWith("(") || sAttributeAlias.EndsWith(")"))
                            {
                                if (BaseRoomName.Trim().Contains(sAttributeAlias))
                                {
                                    isRoomHaveAttribute = true;
                                }
                                else
                                {
                                    isRoomHaveAttribute = false;
                                }
                            }
                            else
                            {
                                if ((" " + BaseRoomName.Trim() + " ").Contains(" " + sAttributeAlias + " "))
                                {
                                    isRoomHaveAttribute = true;
                                }
                                else
                                {
                                    isRoomHaveAttribute = false;
                                }
                            }

                            if (isRoomHaveAttribute)
                            {
                                AttributeList.Add(new DC_SupplierRoomName_AttributeList
                                {
                                    SystemAttributeKeywordID = Attribute.Keyword_Id,
                                    SupplierRoomTypeAttribute = alias.Value,
                                    SystemAttributeKeyword = Attribute.Keyword
                                });

                                if ((Attribute.AttributeType ?? string.Empty).ToUpper().Contains("STRIP"))
                                {
                                    BaseRoomName = BaseRoomName.Replace(sAttributeAlias, string.Empty);
                                }
                                else if ((Attribute.AttributeType ?? string.Empty).ToUpper().Contains("REPLACE"))
                                {
                                    BaseRoomName = BaseRoomName.Replace(sAttributeAlias, Attribute.Keyword);
                                }

                                BaseRoomName = System.Text.RegularExpressions.Regex.Replace(BaseRoomName, @"\s{2,}", " ");
                                BaseRoomName = BaseRoomName.Replace("( )", string.Empty);
                                BaseRoomName = BaseRoomName.Replace("()", string.Empty);

                                BaseRoomName = BaseRoomName.Trim();

                                alias.NewHits += 1;

                                isRoomHaveAttribute = false;

                                break;

                            }

                        }
                    }
                    #endregion

                    #region Perform Special Operations
                    //List<DataContracts.Masters.DC_Keyword> SpecialKeywords = Keywords.Where(w => w.Keyword.StartsWith("##") && w.Attribute == false).ToList();
                    //foreach(var keyword in SpecialKeywords)
                    //{
                    //    if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_START")
                    //    {
                    //        foreach(var alias in keyword.Alias)
                    //        {

                    //        }
                    //    }

                    //    if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_END")
                    //    {
                    //        foreach (var alias in keyword.Alias)
                    //        {

                    //        }
                    //    }

                    //    if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_STRING")
                    //    {
                    //        foreach (var alias in keyword.Alias)
                    //        {

                    //        }
                    //    }

                    //    if (keyword.Keyword.ToUpper() == "##_REMOVE_ANYWHERE_IN_STRING")
                    //    {
                    //        foreach (var alias in keyword.Alias)
                    //        {

                    //        }
                    //    }
                    //}
                    #endregion

                    #region Replace 1 to 10 with words
                    roomWords = BaseRoomName.Split(' ');
                    foreach (string word in roomWords)
                    {
                        int numCheck;
                        if (int.TryParse(word, out numCheck))
                        {
                            if (numCheck >= 0 && numCheck <= 10)
                            {
                                BaseRoomName = BaseRoomName.Replace(word, unitNumerMap[numCheck]);
                            }
                        }
                    }
                    #endregion

                    #region POST TTFU

                    #region Replace UnNecessary chars
                    BaseRoomName = BaseRoomName.Replace('<', ' ');
                    BaseRoomName = BaseRoomName.Replace('>', ' ');
                    BaseRoomName = BaseRoomName.Replace('?', ' ');
                    BaseRoomName = BaseRoomName.Replace('#', ' ');
                    BaseRoomName = BaseRoomName.Replace('!', ' ');
                    BaseRoomName = BaseRoomName.Replace('@', ' ');
                    BaseRoomName = BaseRoomName.Replace("&", " AND ");
                    //BaseRoomName = BaseRoomName.Replace("+", " INCLUDING ");
                    BaseRoomName = BaseRoomName.Replace('(', ' ');
                    BaseRoomName = BaseRoomName.Replace(')', ' ');
                    BaseRoomName = BaseRoomName.Replace('-', ' ');
                    BaseRoomName = BaseRoomName.Replace(',', ' ');
                    BaseRoomName = BaseRoomName.Replace('.', ' ');
                    BaseRoomName = BaseRoomName.Replace('"', ' ');
                    #endregion

                    #region Replace Multiple whitespaces into One Whitespace
                    BaseRoomName = System.Text.RegularExpressions.Regex.Replace(BaseRoomName, @"\s{2,}", " ");
                    #endregion

                    #region trim whitespace both end
                    BaseRoomName = BaseRoomName.Trim();
                    #endregion

                    #region Remove logical words from end
                    int lastIndex = BaseRoomName.Trim().LastIndexOf(" ");
                    if (BaseRoomName.EndsWith(" AND") || BaseRoomName.EndsWith(" OR"))
                    {
                        if (lastIndex != -1)
                        {
                            BaseRoomName = BaseRoomName.Trim().Substring(0, lastIndex).Trim();
                        }
                    }
                    #endregion

                    #endregion

                    #region UpdateToDB
                    //Value assignment
                    srn.TX_SupplierRoomName_Stripped = BaseRoomName;
                    srn.TX_SupplierRoomName_Stripped_ReOrdered = BaseRoomName;
                    srn.AttributeList = AttributeList.ToList();

                    //Update Room Name Stripped and Attributes
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        //Remove Existing Attribute List Records
                        context.Accommodation_SupplierRoomTypeAttributes.RemoveRange(context.Accommodation_SupplierRoomTypeAttributes.Where(w => w.RoomTypeMap_Id == srn.RoomTypeMap_Id));

                        context.Accommodation_SupplierRoomTypeAttributes.AddRange((from a in srn.AttributeList
                                                                                   select new Accommodation_SupplierRoomTypeAttributes
                                                                                   {
                                                                                       RoomTypeMapAttribute_Id = Guid.NewGuid(),
                                                                                       RoomTypeMap_Id = srn.RoomTypeMap_Id,
                                                                                       SupplierRoomTypeAttribute = a.SupplierRoomTypeAttribute,
                                                                                       SystemAttributeKeyword = a.SystemAttributeKeyword,
                                                                                       SystemAttributeKeyword_Id = a.SystemAttributeKeywordID
                                                                                   }).ToList());

                        var srnm = context.Accommodation_SupplierRoomTypeMapping.Find(srn.RoomTypeMap_Id);
                        if (srnm != null)
                        {
                            srnm.TX_RoomName = srn.TX_SupplierRoomName;
                            srnm.Tx_StrippedName = srn.TX_SupplierRoomName_Stripped;
                            srnm.Tx_ReorderedName = srn.TX_SupplierRoomName_Stripped_ReOrdered;
                            srnm.Edit_Date = DateTime.Now;
                            srnm.Edit_User = Edit_User;
                        }

                        context.SaveChanges();

                    }

                    #endregion

                    #region Update Progress Log
                    if (ISProgressLog)
                    {
                        if (i % 5 == 0)
                        {
                            PLog.PercentageValue = 25 + ((60 * i) / asrtmd.Count);
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    #endregion
                }

                #region Update No Of Hits
                var updatableAliases = (from k in Keywords
                                        from ka in k.Alias
                                        where ka.NewHits != 0
                                        select ka).ToList();
                if (updatableAliases.Count > 0)
                {
                    using (DL_Masters objDL = new DL_Masters())
                    {
                        objDL.DataHandler_Keyword_Update_NoOfHits(updatableAliases);
                    }
                }

                #endregion

                #region Update Progress Log
                if (ISProgressLog)
                {
                    PLog.PercentageValue = 100;
                    USD.AddStaticDataUploadProcessLog(PLog);
                }
                #endregion

                return new DataContracts.DC_Message { StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success, StatusMessage = "Keyword Replace and Attribute Extraction has been done." };

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while TTFU", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateRoomTypeMappingStatus(DC_MappingMatch obj)
        {
            bool retrn = false;
            //List<DC_Accomodation_ProductMapping> CMS = obj.lstHotelMapping;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());

            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }

            List<DC_Accomodation_SupplierRoomTypeMapping> ret = new List<DC_Accomodation_SupplierRoomTypeMapping>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if ((obj.FileMode ?? "ALL") != "ALL" && curPriority == 1)
                    {
                        List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                        DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();

                        lstSMT = (from a in context.Accommodation_SupplierRoomTypeMapping
                                  join j in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_SupplierRoomTypeMapping_Id equals j.Mapping_Id into jact
                                  from jdact in jact.DefaultIfEmpty()
                                  where a.Supplier_Id == curSupplier_Id && jdact.STG_Mapping_Table_Id == null
                                  && (a.MappingStatus == "UNMAPPED" || a.Accommodation_RoomInfo_Id == null)
                                  select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                                  {
                                      STG_Mapping_Table_Id = Guid.NewGuid(),
                                      File_Id = obj.File_Id,
                                      Mapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                      STG_Id = null,
                                      Batch = obj.CurrentBatch ?? 1
                                  }).Take(250).ToList();
                        if (lstSMT.Count > 0)
                        {
                            //idinsert = DeleteSTGMappingTableIDs(File_Id);
                            bool idinsert = AddSTGMappingTableIDs(lstSMT);
                        }
                    }

                    var prodMap = (from a in context.Accommodation_SupplierRoomTypeMapping.AsNoTracking()
                                   join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_SupplierRoomTypeMapping_Id equals s.Mapping_Id
                                   where s.File_Id == supdata.File_Id && a.Accommodation_RoomInfo_Id == null && a.Supplier_Id == curSupplier_Id && a.Accommodation_Id != null
                                   && a.MappingStatus.Trim().ToUpper() == "UNMAPPED" && s.Batch == obj.CurrentBatch
                                   select a);


                    /*if (obj.IsBatched)
                    {
                        prodMap = (from a in prodMap
                                   join s in context.STG_Mapping_TableIds.AsNoTracking() on a.Accommodation_SupplierRoomTypeMapping_Id equals s.Mapping_Id
                                   //join s in context.stg_SupplierHotelRoomMapping.AsNoTracking() on a.stg_SupplierHotelRoomMapping_Id equals s.stg_SupplierHotelRoomMapping_Id
                                   where s.Batch == obj.CurrentBatch
                                   select a);
                    }*/
                    var prodMapSearch = prodMap.ToList();
                    bool isRoomNameCheck = false;
                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = configWhere + " " + config.AttributeValue.Replace("Accommodation_RoomInfo.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        CurrConfig = config.AttributeValue.Replace("Accommodation_RoomInfo.", "").Trim().ToUpper();
                        CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);
                        if (CurrConfig == "ROOMCATEGORY")
                        {
                            isRoomNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join ac in context.Accommodation_RoomInfo.AsNoTracking() on a.Accommodation_Id equals ac.Accommodation_Id
                                             where (((a.Tx_ReorderedName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (ac.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((a.Tx_StrippedName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (ac.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((a.TX_RoomName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (ac.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((a.SupplierRoomName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (ac.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    )
                                             select a).Distinct().ToList();
                        }

                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }

                    List<DC_Accommodation_SupplierRoomTypeMap_SearchRS> res = new List<DC_Accommodation_SupplierRoomTypeMap_SearchRS>();
                    if (isRoomNameCheck)
                    {
                        res = (from a in prodMapSearch
                               select new DataContracts.Mapping.DC_Accommodation_SupplierRoomTypeMap_SearchRS
                               {
                                   Accommodation_SupplierRoomTypeMapping_Id = a.Accommodation_SupplierRoomTypeMapping_Id,
                                   Accommodation_Id = a.Accommodation_Id,
                                   Accommodation_RoomInfo_Id = a.Accommodation_RoomInfo_Id,
                                   SupplierProductName = a.SupplierProductName,
                                   SupplierName = a.SupplierName,
                                   SupplierProductId = a.SupplierProductId,
                                   SupplierRoomCategory = a.SupplierRoomCategory,
                                   SupplierRoomId = a.SupplierRoomId,
                                   SupplierRoomName = a.SupplierRoomName,
                                   Supplier_Id = a.Supplier_Id,
                                   Tx_ReorderedName = a.Tx_ReorderedName,
                                   TX_RoomName = a.TX_RoomName,
                                   Tx_StrippedName = a.Tx_StrippedName,
                                   MappingStatus = a.MappingStatus,
                                   MapId = a.MapId,
                                   stg_SupplierHotelRoomMapping_Id = a.stg_SupplierHotelRoomMapping_Id

                               }).Distinct().ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");
                        res = res.Select(c =>
                        {
                            c.Accommodation_RoomInfo_Id = (context.Accommodation_RoomInfo.AsNoTracking()
                                            .Where(s => (
                                                            ((isRoomNameCheck && (((c.Tx_ReorderedName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (s.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((c.Tx_StrippedName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (s.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((c.TX_RoomName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (s.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    ||
                                                    ((c.SupplierRoomName ?? string.Empty).ToString().Trim().ToUpper()
                                                    == (s.RoomCategory ?? string.Empty).ToString().Trim().ToUpper())
                                                    )) || (!isRoomNameCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.Accommodation_RoomInfo_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");
                        res.RemoveAll(p => p.Accommodation_RoomInfo_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MappingStatus = ("REVIEW"); return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 75;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.");

                        if (SupplierRoomTypeMapping_InsertUpdate(res))
                        {
                            if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                            {
                                DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                                objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                                objStat.SupplierImportFile_Id = obj.File_Id;
                                objStat.From = "MATCHING";
                                DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                                bool del = DeleteSTGMappingTableIDs(Guid.Parse(obj.File_Id.ToString()));
                                using (ConsumerEntities context1 = new ConsumerEntities())
                                {
                                    var oldRecords = (from y in context1.STG_Mapping_TableIds
                                                      where y.File_Id == File_Id
                                                      select y).ToList();
                                    context1.STG_Mapping_TableIds.RemoveRange(oldRecords);
                                    context1.SaveChanges();
                                }
                            }

                            retrn = true;
                        }
                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    CallLogVerbose(File_Id, "MATCH", "Update Done.");
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating hotel mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
            CallLogVerbose(File_Id, "MATCH", "Update Done.");
            return true;
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
                                       OldCountryCode = a.CountryCode,
                                       CountryName = a.CountryName,
                                       Create_Date = a.Create_Date,
                                       Create_User = a.Create_User,
                                       Edit_Date = a.Edit_Date,
                                       Edit_User = a.Edit_User,
                                       MapID = a.MapID,
                                       Status = a.Status,
                                       SupplierName = a.SupplierName,
                                       Code = jd.Code,
                                       Name = jd.Name,
                                       Latitude = a.Latitude,
                                       Longitude = a.Longitude,
                                       ContinentCode = a.ContinentCode,
                                       ContinentName = a.ContinentName
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

        public bool CountryMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = true;
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;

            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            fileRQ.SupplierImportFile_Id = File_Id;
            file = staticdata.GetStaticDataFileDetail(fileRQ);

            if (obj != null)
            {
                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierCountryMapping> clsSTGCountry = new List<DataContracts.STG.DC_stg_SupplierCountryMapping>();
                List<DataContracts.STG.DC_stg_SupplierCountryMapping> clsSTGCountryInsert = new List<DataContracts.STG.DC_stg_SupplierCountryMapping>();
                List<DC_CountryMapping> clsMappingCountry = new List<DC_CountryMapping>();

                CallLogVerbose(File_Id, "MAP", "Fetching Staged Countries.");
                DataContracts.STG.DC_stg_SupplierCountryMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierCountryMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                clsSTGCountry = staticdata.GetSTGCountryData(RQ);
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.");
                DC_CountryMappingRQ RQMapping = new DC_CountryMappingRQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQMapping.Supplier_Id = CurSupplier_Id;
                RQMapping.PageNo = 0;
                RQMapping.PageSize = int.MaxValue;
                RQMapping.Status = "ALL";
                clsMappingCountry = GetCountryMapping(RQMapping);
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Updating Existing Countries.");
                clsMappingCountry = clsMappingCountry.Select(c =>
                {
                    c.CountryCode = (clsSTGCountry
                    .Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode))
                    .Select(s1 => s1.CountryCode)
                    .FirstOrDefault()
                    ) ?? c.CountryCode;
                    c.Edit_Date = DateTime.Now;
                    c.Edit_User = "TLGX";
                    c.ActionType = "UPDATE";
                    c.stg_Country_Id = (clsSTGCountry
                    .Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode))
                    .Select(s1 => s1.stg_Country_Id)
                    .FirstOrDefault()
                    );
                    c.ContinentCode = (clsSTGCountry
                    .Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode))
                    .Select(s1 => s1.ContinentCode)
                    .FirstOrDefault()
                    ) ?? c.ContinentCode;
                    c.ContinentName = (clsSTGCountry
                    .Where(s => (s.CountryName ?? s.CountryCode) == (c.CountryName ?? c.CountryCode))
                    .Select(s1 => s1.ContinentName)
                    .FirstOrDefault()
                    ) ?? c.ContinentName;
                    return c;
                }).ToList();

                List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                lstobj.InsertRange(lstobj.Count, clsMappingCountry.Where(a => ((a.stg_Country_Id == Guid.Empty) ? Guid.Empty : a.stg_Country_Id) != Guid.Empty && a.ActionType == "UPDATE"
                    && (a.stg_Country_Id ?? Guid.Empty) != Guid.Empty).Select
                   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                   {
                       STG_Mapping_Table_Id = Guid.NewGuid(),
                       File_Id = obj.File_Id,
                       STG_Id = g.stg_Country_Id,
                       Mapping_Id = g.CountryMapping_Id,
                       Batch = obj.CurrentBatch ?? 0
                   }));

                CallLogVerbose(File_Id, "MAP", "Checking for New Countries in File.");
                clsSTGCountryInsert = clsSTGCountry.Where(p => !clsMappingCountry.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                && (
                    (p.CountryCode != null && p2.CountryCode == p.CountryCode)
                    || (p.CountryCode == null && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper())
                ))).ToList();

                PLog.PercentageValue = 48;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.");
                clsSTGCountry.RemoveAll(p => clsSTGCountryInsert.Any(p2 => (p2.stg_Country_Id == p.stg_Country_Id)));
                clsMappingCountry.RemoveAll(p => p.CountryCode == p.OldCountryCode);

                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Inserting New Countries.");
                clsMappingCountry.InsertRange(clsMappingCountry.Count, clsSTGCountryInsert.Select
                (g => new DC_CountryMapping
                {
                    CountryMapping_Id = Guid.NewGuid(),
                    Country_Id = null,
                    Supplier_Id = CurSupplier_Id, //Guid.Parse(DictionaryLookup(mappingPrefix, "Supplier_Id", stgPrefix, Cursupplier_Id.ToString())),
                    SupplierName = g.SupplierName, //DictionaryLookup(mappingPrefix, "SupplierName", stgPrefix, CurSupplierName), 
                    CountryName = g.CountryName,  //DictionaryLookup(mappingPrefix, "CountryName", stgPrefix, g.CountryName), 
                    CountryCode = g.CountryCode, //DictionaryLookup(mappingPrefix, "CountryCode", stgPrefix, g.CountryCode), 
                    Status = "UNMAPPED", //DictionaryLookup(mappingPrefix, "Status", stgPrefix, "UNMAPPED"),                    
                    Create_Date = DateTime.Now,
                    Create_User = "TLGX_DataHandler",
                    Edit_Date = null,
                    Edit_User = null,
                    MapID = null,
                    Latitude = g.Latitude,
                    Longitude = g.Longitude,
                    ActionType = "INSERT",
                    stg_Country_Id = g.stg_Country_Id,
                    ContinentCode = g.ContinentCode,
                    ContinentName = g.ContinentName,
                    Remarks = "" //DictionaryLookup(mappingPrefix, "Remarks", stgPrefix, "")
                }));

                lstobj.InsertRange(lstobj.Count, clsMappingCountry.Where(a => a.stg_Country_Id != null && a.ActionType == "INSERT"
                && (a.stg_Country_Id ?? Guid.Empty) != Guid.Empty)
               .Select
                  (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                  {
                      STG_Mapping_Table_Id = Guid.NewGuid(),
                      File_Id = obj.File_Id,
                      STG_Id = g.stg_Country_Id,
                      Mapping_Id = g.CountryMapping_Id,
                      Batch = obj.CurrentBatch ?? 0
                  }));
                bool idinsert = AddSTGMappingTableIDs(lstobj);

                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.");
                if (clsMappingCountry.Count > 0)
                {
                    ret = UpdateCountryMapping(clsMappingCountry);
                    /* if (obj.CurrentBatch == 1)
                     {
                         DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                         objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                         objStat.SupplierImportFile_Id = obj.File_Id;
                         objStat.FinalStatus = file[0].STATUS;
                         objStat.TotalRows = clsMappingCountry.Count;
                         objStat.Process_Date = DateTime.Now;
                         objStat.Process_User = file[0].PROCESS_USER;
                         DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                     }*/
                }
            }

            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch " + (obj.CurrentBatch ?? 0).ToString());
            return ret;
        }

        public List<DC_CountryMapping> UpdateCountryMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            List<DC_CountryMapping> CMS = obj.lstCountryMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());

            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;
            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            List<DC_CountryMapping> ret = new List<DC_CountryMapping>();
            //foreach (var CM in CMS)
            //{
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if ((obj.FileMode ?? "ALL") != "ALL" && curPriority == 1)
                    {
                        List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                        DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();

                        lstSMT = (from a in context.m_CountryMapping
                                  join j in context.STG_Mapping_TableIds.AsNoTracking() on a.CountryMapping_Id equals j.Mapping_Id into jact
                                  from jdact in jact.DefaultIfEmpty()
                                  where a.Supplier_Id == curSupplier_Id && jdact.STG_Mapping_Table_Id == null
                                  && (a.Status == "UNMAPPED" || a.Country_Id == null)
                                  select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                                  {
                                      STG_Mapping_Table_Id = Guid.NewGuid(),
                                      File_Id = obj.File_Id,
                                      Mapping_Id = a.CountryMapping_Id,
                                      STG_Id = null,
                                      Batch = obj.CurrentBatch ?? 1
                                  }).Take(250).ToList();
                        if (lstSMT.Count > 0)
                        {
                            //idinsert = DeleteSTGMappingTableIDs(File_Id);
                            bool idinsert = AddSTGMappingTableIDs(lstSMT);
                        }
                    }

                    var prodMapSearch = (from a in context.m_CountryMapping
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CountryMapping_Id equals s.Mapping_Id
                                         where s.File_Id == obj.File_Id && a.Country_Id == null && a.Supplier_Id == curSupplier_Id
                                         && a.Status == "UNMAPPED" && s.Batch == obj.CurrentBatch
                                         select a);

                    /*if (obj.IsBatched)
                    {
                        prodMapSearch = (from a in prodMapSearch
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CountryMapping_Id equals s.Mapping_Id
                                         //join s in context.stg_SupplierCountryMapping.AsNoTracking() on a.stg_Country_Id equals s.stg_Country_Id
                                         where s.Batch == obj.CurrentBatch
                                         select a);
                    }*/
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isLatLongCheck = false;

                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = configWhere + " " + config.AttributeValue.Replace("m_CountryMaster.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        CurrConfig = config.AttributeValue.Replace("m_CountryMaster.", "").Trim().ToUpper();
                        CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);
                        if (CurrConfig == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.Code
                                             where a.CountryCode != null && m.Code != null
                                             select a);
                        }
                        if (CurrConfig == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryName equals m.Name
                                             where a.CountryName != null && m.Name != null
                                             select a);
                        }
                        if (CurrConfig == "ISO3166-1-Alpha-2".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_2
                                             where a.CountryCode != null && m.ISO3166_1_Alpha_2 != null
                                             select a);
                        }
                        if (CurrConfig == "ISO3166-1-Alpha-3".Trim().ToUpper())
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on a.CountryCode equals m.ISO3166_1_Alpha_3
                                             where a.CountryCode != null && m.ISO3166_1_Alpha_3 != null
                                             select a);
                        }
                        if (CurrConfig == "LATITUDE")
                        {
                            isLatLongCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMaster on new { a.Latitude, a.Longitude } equals new { m.Latitude, m.Longitude }
                                             where m.Latitude != null && a.Latitude != null && m.Longitude != null && a.Longitude != null
                                             select a);
                        }
                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        //PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + (PerForEachPriority / totConfigs);
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);
                    }

                    //var prodMapSearch1 = prodMapSearch.ToList().Select(c =>
                    //{
                    //    c.Country_Id = (from cm in context.m_CountryMaster where cm.Code == c.CountryCode && isCodeCheck select cm.Country_Id).FirstOrDefault();
                    //    return c;
                    //}).ToList();
                    List<DC_CountryMapping> res = new List<DC_CountryMapping>();

                    if (isNameCheck || isCodeCheck || isLatLongCheck)
                    {
                        res = (from a in prodMapSearch
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
                                   SupplierName = a.SupplierName,
                                   Latitude = a.Latitude,
                                   Longitude = a.Longitude
                               }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");
                        res = res.Select(c =>
                        {
                            c.Country_Id = (context.m_CountryMaster
                                            .Where(s => (
                                                            //(isCodeCheck && s.Code == c.CountryCode && isNameCheck && s.Name == c.CountryName) ||
                                                            //(isCodeCheck && (s.Code == c.CountryCode || s.ISO3166_1_Alpha_2 == c.CountryCode || s.ISO3166_1_Alpha_3 == c.CountryCode) && (!isNameCheck)) ||
                                                            //((!isCodeCheck) && isNameCheck && s.Name == c.CountryName)

                                                            ((isCodeCheck && (s.Code == c.CountryCode || s.ISO3166_1_Alpha_2 == c.CountryCode || s.ISO3166_1_Alpha_3 == c.CountryCode)) || (!isCodeCheck)) &&
                                                            ((isNameCheck && s.Name == c.CountryName) || (!isNameCheck)) &&
                                                            ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.Country_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        #region "Old Code"
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

                        #endregion

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");
                        //foreach (DC_CountryMapping v in res)
                        //{
                        //    if (v.Country_Id != null)
                        //    {
                        //        if (v.Country_Id != Guid.Empty)
                        //            v.Status = "REVIEW";
                        //        else
                        //        {
                        //            v.Country_Id = null;
                        //            res.Remove(v);
                        //        }
                        //    }
                        //    else
                        //        res.Remove(v);
                        //}
                        res.RemoveAll(p => p.Country_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 75;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.");
                        if (UpdateCountryMapping(res))
                        {
                            if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                            {
                                DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                                objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                                objStat.SupplierImportFile_Id = obj.File_Id;
                                objStat.From = "MATCHING";
                                DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                                using (ConsumerEntities context1 = new ConsumerEntities())
                                {
                                    var oldRecords = (from y in context1.STG_Mapping_TableIds
                                                      where y.File_Id == File_Id
                                                      select y).ToList();
                                    context1.STG_Mapping_TableIds.RemoveRange(oldRecords);
                                    context1.SaveChanges();
                                }
                                //bool del = DeleteSTGMappingTableIDs(Guid.Parse(obj.File_Id.ToString()));
                            }

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

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    else
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
                    CallLogVerbose(File_Id, "MATCH", "Update Done.");
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
                            if (CM.MatchedBy != null)
                                search.MatchedBy = CM.MatchedBy;
                            search.Status = CM.Status;
                            search.Edit_Date = CM.Edit_Date;
                            search.Edit_User = CM.Edit_User;
                            search.Remarks = CM.Remarks;
                            if (CM.ContinentCode != null)
                                search.ContinentCode = CM.ContinentCode;
                            if (CM.ContinentName != null)
                                search.ContinentName = CM.ContinentName;
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
                            objNew.Latitude = CM.Latitude;
                            objNew.Longitude = CM.Longitude;
                            objNew.ContinentCode = CM.ContinentCode;
                            objNew.ContinentName = CM.ContinentName;
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


                    var prodMapSearch = (from a in context.m_CityMapping select a).AsQueryable();
                    var countrymaster = context.m_CountryMaster.Select(s => s).AsQueryable();

                    var SuppliersMaster = context.Supplier.Select(s => s).AsQueryable();
                    var CityMaster = context.m_CityMaster.Select(s => s).AsQueryable();

                    var EntityMaster = context.m_CityMapping_EntityCount.AsQueryable();


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
                                        join c in countrymaster on a.Country_Id equals c.Country_Id
                                        where (a.CountryName.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        || c.Name.ToUpper().Contains(param.SupplierCountryName.ToUpper())
                                        )
                                        select a;
                        if (param.IsExact)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            join c in countrymaster on a.Country_Id equals c.Country_Id
                                            where (a.CountryName.ToUpper() == param.SupplierCountryName.ToUpper()
                                            || c.Name.ToUpper() == param.SupplierCountryName.ToUpper()
                                            )
                                            select a;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(param.SupplierCityName))
                    {
                        prodMapSearch = from a in prodMapSearch
                                        where a.CityName.Contains(param.SupplierCityName)
                                        select a;
                        if (param.IsExact)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.CityName == param.SupplierCityName
                                            select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(param.EntityType))
                    {
                        string[] FilterArray = param.EntityType.Split(',');
                        var newEntityMaster = (from x in EntityMaster
                                               where FilterArray.Contains(x.EntityType)
                                               select x).AsQueryable();
                        prodMapSearch = (from p in prodMapSearch
                                         join e in newEntityMaster on p.CityMapping_Id equals e.CityMapping_Id
                                         select p);
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
                                 join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                 join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                 from ctld in ctl.DefaultIfEmpty()
                                 join m in countrymaster on a.Country_Id equals m.Country_Id into j
                                 from jd in j.DefaultIfEmpty()
                                 select a.CityName).Distinct().Count();
                        var prodMapList = (from a in prodMapSearch
                                           join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                           join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in countrymaster on a.Country_Id equals m.Country_Id into j
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
                                           join s in SuppliersMaster on a.Supplier_Id equals s.Supplier_Id
                                           join ct in CityMaster on a.City_Id equals ct.City_Id into ctl
                                           from ctld in ctl.DefaultIfEmpty()
                                           join m in countrymaster on a.Country_Id equals m.Country_Id into j
                                           from jd in j.DefaultIfEmpty()
                                               //orderby (param.SortBy)
                                           orderby a.CityName
                                           select new DataContracts.Mapping.DC_CityMapping
                                           {
                                               CityMapping_Id = a.CityMapping_Id,
                                               City_Id = a.City_Id,
                                               CityCode = a.CityCode,
                                               CityName = a.CityName,
                                               oldCityName = a.CityName,
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
                                               CountryCode = a.CountryCode,
                                               CountryName = a.CountryName,
                                               MasterCountryCode = jd.Code,
                                               MasterCountryName = jd.Name,
                                               MasterCityCode = ctld.Code,
                                               Master_CityName = ctld.Name,
                                               MasterStateName = ctld.StateName,
                                               MasterStateCode = ctld.StateCode,
                                               StateCode = a.StateCode,
                                               StateName = a.StateName,
                                               Latitude = a.Latitude,
                                               Longitude = a.Longitude,
                                               EntityTypeFlag = (EntityMaster.Where(w => w.CityMapping_Id == a.CityMapping_Id)
                                                                .Select(s => new DC_CityMapping_EntityCount { EntityCityMapping_Id = s.EntityCityMapping_Id, CityMapping_Id = s.CityMapping_Id, EntityType = s.EntityType, Count = s.Count })
                                                                .OrderBy(p => p.EntityType).ToList()),
                                           }).Skip(skip).Take(param.PageSize).ToList();

                        //CityMapList = CityMapList.Select(c =>
                        //{
                        //    c.MasterStateName = (context.m_States
                        //                    .Where(s => (s.Country_Id == c.Country_Id))
                        //                    .Select(s => s.StateName)
                        //                    .FirstOrDefault()
                        //                    );
                        //    return c;
                        //}).ToList();

                        //if (param.CalledFromTLGX == null || (param.CalledFromTLGX.ToString() != "TLGX"))
                        if (string.IsNullOrWhiteSpace(param.CalledFromTLGX))
                        {
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
                                            var searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityName.ToUpper().Trim())).FirstOrDefault();
                                            if (searchCity == null)
                                            {
                                                searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityName.ToUpper().Trim())).FirstOrDefault();
                                                //if (searchCity == null)
                                                //{
                                                //    searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && (a.Name.ToUpper().Trim() == cityCode.ToUpper().Trim())).FirstOrDefault();
                                                //    if (searchCity == null)
                                                //        searchCity = CityMaster.Where(a => a.Country_Id == item.Country_Id && a.Name.ToUpper().Trim().Contains(cityCode.ToUpper().Trim())).FirstOrDefault();
                                                //}
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

        public bool CityMappingMatch(DataContracts.Masters.DC_Supplier obj)
        {
            bool ret = false;
            DL_UploadStaticData staticdata = new DL_UploadStaticData();
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());
            PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
            PLog.SupplierImportFile_Id = obj.File_Id;
            PLog.Step = "MAP";
            PLog.Status = "MAPPING";
            PLog.CurrentBatch = obj.CurrentBatch ?? 0;
            PLog.TotalBatch = obj.TotalBatch ?? 0;
            List<DC_SupplierImportFileDetails> file = new List<DC_SupplierImportFileDetails>();
            DC_SupplierImportFileDetails_RQ fileRQ = new DC_SupplierImportFileDetails_RQ();
            fileRQ.SupplierImportFile_Id = File_Id;
            file = staticdata.GetStaticDataFileDetail(fileRQ);
            if (obj != null)
            {

                string CurSupplierName = obj.Name;
                Guid CurSupplier_Id = Guid.Parse(obj.Supplier_Id.ToString());

                List<DataContracts.STG.DC_stg_SupplierCityMapping> clsSTGCity = new List<DataContracts.STG.DC_stg_SupplierCityMapping>();
                List<DataContracts.STG.DC_stg_SupplierCityMapping> clsSTGCityInsert = new List<DataContracts.STG.DC_stg_SupplierCityMapping>();
                List<DC_CityMapping> clsMappingCity = new List<DC_CityMapping>();

                CallLogVerbose(File_Id, "MAP", "Fetching Staged Cities.");
                DataContracts.STG.DC_stg_SupplierCityMapping_RQ RQ = new DataContracts.STG.DC_stg_SupplierCityMapping_RQ();
                RQ.SupplierName = CurSupplierName;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                clsSTGCity = staticdata.GetSTGCityData(RQ);
                PLog.PercentageValue = 15;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Fetching Existing Mapping Data.");
                DC_CityMapping_RQ RQCity = new DC_CityMapping_RQ();
                if (CurSupplier_Id != Guid.Empty)
                    RQCity.Supplier_Id = CurSupplier_Id;
                RQCity.PageNo = 0;
                RQCity.PageSize = int.MaxValue;
                RQCity.CalledFromTLGX = "TLGX";
                //RQ.Status = "ALL";
                clsMappingCity = GetCityMapping(RQCity);
                PLog.PercentageValue = 26;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Updating Existing Cities.");
                clsMappingCity = clsMappingCity.Select(c =>
                {
                    c.CityName = (clsSTGCity
                    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                    .Where(s => s.CityCode == c.CityCode)
                    .Select(s1 => s1.CityName)
                    .FirstOrDefault()
                    ) ?? c.CityName;
                    c.Edit_Date = DateTime.Now;
                    c.Edit_User = "TLGX_DataHandler";
                    c.ActionType = "UPDATE";
                    c.stg_City_Id = (clsSTGCity
                    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                    .Where(s => s.CityCode == c.CityCode)
                    .Select(s1 => s1.stg_City_Id)
                    .FirstOrDefault()
                    );
                    c.StateCode = (clsSTGCity
                    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                    .Where(s => s.CityCode == c.CityCode)
                    .Select(s1 => s1.CityName)
                    .FirstOrDefault()
                    ) ?? c.StateCode;
                    c.StateName = (clsSTGCity
                    //.Where(s => (s.CityCode ?? s.CityName) == (c.CityCode ?? c.CityName) && s.Country_Id == c.Country_Id)
                    .Where(s => s.CityCode == c.CityCode)
                    .Select(s1 => s1.CityName)
                    .FirstOrDefault()
                    ) ?? c.StateName;
                    return c;
                }).ToList();

                List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstobj = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                lstobj.InsertRange(lstobj.Count, clsMappingCity.Where(a => a.stg_City_Id != null && a.ActionType == "UPDATE"
                && (a.stg_City_Id ?? Guid.Empty) != Guid.Empty).Select
                   (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                   {
                       STG_Mapping_Table_Id = Guid.NewGuid(),
                       File_Id = obj.File_Id,
                       STG_Id = g.stg_City_Id,
                       Mapping_Id = g.CityMapping_Id,
                       Batch = obj.CurrentBatch ?? 0
                   }));
                PLog.PercentageValue = 37;
                USD.AddStaticDataUploadProcessLog(PLog);

                CallLogVerbose(File_Id, "MAP", "Checking for New Cities in File.");
                clsSTGCityInsert = clsSTGCity.Where(p => !clsMappingCity.Any(p2 => (p2.SupplierName.ToString().Trim().ToUpper() == p.SupplierName.ToString().Trim().ToUpper())
                    && (
                        //(((p2.StateName ?? string.Empty).ToString().Trim().ToUpper() == (p.StateName ?? string.Empty).ToString().Trim().ToUpper()))

                        //&& (p2.Country_Id == p.Country_Id)
                        p2.CityCode == p.CityCode
                    //&& (((p2.CityName ?? string.Empty).ToString().Trim().ToUpper() == (p.CityName ?? string.Empty).ToString().Trim().ToUpper()))
                    ))).ToList();
                PLog.PercentageValue = 48;
                USD.AddStaticDataUploadProcessLog(PLog);

                #region "Commented Code"
                //(p.CityCode != null && p.CityName != null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper() && p2.CityCode == p.CityCode) 
                //|| (p.CityCode == null && p.CityName != null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //|| (p.CityCode != null && p.CityName == null && p2.CityCode == p.CityCode)

                //&& (
                //    (p.CityCode != null && p.CountryCode == null && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode != null && p2.CountryCode == p.CountryCode && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode == null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode != null && p2.CountryCode == p.CountryCode && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //    || (p.CityCode != null && p.CountryCode == null && p.CountryName == null && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode == null && p.CountryName != null && p2.CountryName == p.CountryName && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode == null && p.CountryName == null && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode == null && p.CountryName != null && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper() && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //    || (p.CityCode != null && p.CountryCode != null && p.CountryName == null && p2.CountryCode == p.CountryCode && p2.CityCode == p.CityCode)
                //    || (p.CityCode != null && p.CountryCode != null && p.CountryName != null && p2.CountryCode == p.CountryCode && p2.CountryName == p.CountryName && p2.CityCode == p.CityCode)
                //    || (p.CityCode == null && p.CountryCode != null && p.CountryName == null && p2.CountryCode == p.CountryCode && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())
                //    || (p.CityCode == null && p.CountryCode != null && p.CountryName != null && p2.CountryCode == p.CountryCode && p2.CountryName.ToString().Trim().ToUpper() == p.CountryName.ToString().Trim().ToUpper() && p2.CityName.ToString().Trim().ToUpper() == p.CityName.ToString().Trim().ToUpper())

                //   ))).ToList();

                #endregion

                CallLogVerbose(File_Id, "MAP", "Removing UnEdited Data.");
                clsSTGCity.RemoveAll(p => clsSTGCityInsert.Any(p2 => (p2.stg_City_Id == p.stg_City_Id)));



                //clsMappingCity = clsMappingCity.Where(a => a.oldCityName != a.CityName).ToList();
                clsMappingCity.RemoveAll(p => (p.CityName == p.oldCityName && p.StateCode == null));
                PLog.PercentageValue = 53;
                USD.AddStaticDataUploadProcessLog(PLog);


                CallLogVerbose(File_Id, "MAP", "Inserting New Cities.");
                clsMappingCity.InsertRange(clsMappingCity.Count, clsSTGCityInsert.Select
                    (g => new DC_CityMapping
                    {
                        CityMapping_Id = Guid.NewGuid(),
                        City_Id = null,
                        CityCode = g.CityCode,
                        CityName = g.CityName,
                        Country_Id = null,
                        Supplier_Id = CurSupplier_Id,
                        SupplierName = g.SupplierName,
                        CountryName = g.CountryName,
                        CountryCode = g.CountryCode,
                        Status = "UNMAPPED",
                        Create_Date = DateTime.Now,
                        Create_User = "TLGX_DataHandler",
                        Edit_Date = null,
                        Edit_User = null,
                        MapID = null,
                        Latitude = g.Latitude,
                        Longitude = g.Longitude,
                        oldCityName = g.CityName,
                        ActionType = "INSERT",
                        stg_City_Id = g.stg_City_Id,
                        Remarks = "", //DictionaryLookup(mappingPrefix, "Remarks", stgPrefix, "")
                        StateCode = g.StateCode,
                        StateName = g.StateName
                    }));

                lstobj.InsertRange(lstobj.Count, clsMappingCity.Where(a => a.stg_City_Id != null && a.ActionType == "INSERT"
                && (a.stg_City_Id ?? Guid.Empty) != Guid.Empty)
               .Select
                  (g => new DataContracts.STG.DC_STG_Mapping_Table_Ids
                  {
                      STG_Mapping_Table_Id = Guid.NewGuid(),
                      File_Id = obj.File_Id,
                      STG_Id = g.stg_City_Id,
                      Mapping_Id = g.CityMapping_Id,
                      Batch = obj.CurrentBatch ?? 0
                  }));
                bool idinsert = AddSTGMappingTableIDs(lstobj);
                PLog.PercentageValue = 58;
                USD.AddStaticDataUploadProcessLog(PLog);
                CallLogVerbose(File_Id, "MAP", "Updating / Inserting Database.");
                if (clsMappingCity.Count > 0)
                {
                    ret = UpdateCityMappingMatch(clsMappingCity, File_Id);
                    /*if (obj.CurrentBatch == 1)
                    {
                        DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                        objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                        objStat.SupplierImportFile_Id = obj.File_Id;
                        objStat.FinalStatus = file[0].STATUS;
                        objStat.TotalRows = clsMappingCity.Count;
                        objStat.Process_Date = DateTime.Now;
                        objStat.Process_User = file[0].PROCESS_USER;
                        DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);
                    }*/
                }
                else
                    ret = true;
            }
            PLog.PercentageValue = 100;
            USD.AddStaticDataUploadProcessLog(PLog);
            CallLogVerbose(File_Id, "MAP", "MAP Process Complete for Batch " + (obj.CurrentBatch ?? 0).ToString());
            return ret;
        }


        public bool UpdateCityMappingMatch(List<DataContracts.Mapping.DC_CityMapping> obj, Guid File_Id)
        {
            bool ret = false;
            if (obj.Count > 0)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var CM in obj)
                    {
                        if (CM.CityMapping_Id == null || CM.Supplier_Id == null)
                        {
                            continue;
                        }

                        try
                        {

                            var search = (from a in context.m_CityMapping
                                          where a.CityMapping_Id == CM.CityMapping_Id
                                          select a).FirstOrDefault();
                            if (search != null)
                            {
                                try
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
                                    if (search.StateCode == null)
                                        search.StateCode = CM.StateCode;
                                    if (search.StateName == null)
                                        search.StateName = CM.StateName;
                                    //}
                                    context.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    DC_SupplierImportFile_ErrorLog objE = new DC_SupplierImportFile_ErrorLog();
                                    objE.SupplierImportFile_ErrorLog_Id = Guid.NewGuid();
                                    objE.SupplierImportFile_Id = File_Id;
                                    objE.ErrorCode = 0;
                                    objE.ErrorDescription = e.Message.ToString() + ", " + e.StackTrace;
                                    objE.ErrorMessage_UI = "Error while updating city data for " + (CM.CountryName ?? (CM.CountryCode ?? "")) + " - " + (CM.CityName ?? (CM.CityCode ?? "")).ToString() + " - " + CM.City_Id.ToString();
                                    objE.Error_DATE = DateTime.Now;
                                    objE.Error_USER = "TLGX_DataHandler";
                                    DL_UploadStaticData ups = new DL_UploadStaticData();
                                    DataContracts.DC_Message dc = new DataContracts.DC_Message();
                                    dc = ups.AddStaticDataUploadErrorLog(objE);
                                }
                                //context.SaveChanges();
                            }
                            else
                            {
                                try
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
                                    objNew.Latitude = CM.Latitude;
                                    objNew.Longitude = CM.Longitude;
                                    objNew.StateCode = CM.StateCode;
                                    objNew.StateName = CM.StateName;
                                    objNew.stg_City_Id = CM.stg_City_Id;
                                    // objNew.Country_Id = CM.Country_Id;
                                    objNew.Country_Id = ((from a in context.m_CountryMapping.AsNoTracking()
                                                          where a.Supplier_Id == CM.Supplier_Id &&
                                                          ((a.CountryName.Trim().ToUpper() == CM.CountryName.Trim().ToUpper()) && a.CountryName != null && CM.CountryName != null)
                                                          //&& ((CM.CountryName != null && a.CountryName == CM.CountryName) || CM.CountryName == null)
                                                          //&& ((CM.CountryCode != null && a.CountryCode == CM.CountryCode) || CM.CountryCode == null)
                                                          //&& a.Supplier_Id == CM.Supplier_Id
                                                          select a.Country_Id).FirstOrDefault()) ?? ((from a in context.m_CountryMapping.AsNoTracking()
                                                                                                      where a.Supplier_Id == CM.Supplier_Id &&
                                                                                                      ((a.CountryCode == CM.CountryCode) && a.CountryCode != null && CM.CountryCode != null)
                                                                                                      select a.Country_Id).FirstOrDefault());
                                    context.m_CityMapping.Add(objNew);
                                }
                                catch (Exception e)
                                {
                                    DC_SupplierImportFile_ErrorLog objE = new DC_SupplierImportFile_ErrorLog();
                                    objE.SupplierImportFile_ErrorLog_Id = Guid.NewGuid();
                                    objE.SupplierImportFile_Id = File_Id;
                                    objE.ErrorCode = 0;
                                    objE.ErrorDescription = e.Message.ToString() + ", " + e.StackTrace;
                                    objE.ErrorMessage_UI = "Error while inserting city data for " + (CM.CountryName ?? (CM.CountryCode ?? "")) + " - " + (CM.CityName ?? (CM.CityCode ?? "")).ToString();
                                    objE.Error_DATE = DateTime.Now;
                                    objE.Error_USER = "TLGX_DataHandler";
                                    DL_UploadStaticData ups = new DL_UploadStaticData();
                                    DataContracts.DC_Message dc = new DataContracts.DC_Message();
                                    dc = ups.AddStaticDataUploadErrorLog(objE);
                                }
                            }

                            ret = true;


                        }
                        catch
                        {
                            ret = false;
                            throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                        }
                    }
                    if (ret)
                    {
                        context.SaveChanges();
                        context.USP_UpdateMapID("city");
                    }
                }
            }
            else
                ret = true;
            return ret;
        }

        //public List<DC_CityMapping> UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        public bool UpdateCityMappingStatus(DataContracts.Mapping.DC_MappingMatch obj)
        {
            bool retrn = false;
            DataContracts.Masters.DC_Supplier supdata = new DataContracts.Masters.DC_Supplier();
            supdata = obj.SupplierDetail;
            //List<DC_CityMapping> CMS = obj.lstCityMapping;
            List<DC_SupplierImportAttributeValues> configs = obj.lstConfigs;
            string configWhere = "";
            string curSupplier = "";
            Guid? curSupplier_Id = Guid.Empty;
            configWhere = "";
            Guid File_Id = new Guid();
            File_Id = Guid.Parse(obj.File_Id.ToString());



            int totPriorities = obj.TotalPriorities;
            int curPriority = obj.CurrentPriority;
            int totConfigs = 0;
            int curConfig = 0;

            if (totPriorities <= 0)
                totPriorities = 1;
            int PerForEachPriority = 60 / totPriorities;

            if (supdata != null)
            {
                curSupplier = supdata.Name;
                curSupplier_Id = supdata.Supplier_Id;
            }
            List<DC_CityMapping> ret = new List<DC_CityMapping>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if ((obj.FileMode ?? "ALL") != "ALL" && curPriority == 1)
                    {
                        List<DataContracts.STG.DC_STG_Mapping_Table_Ids> lstSMT = new List<DataContracts.STG.DC_STG_Mapping_Table_Ids>();
                        DataContracts.STG.DC_STG_Mapping_Table_Ids SMT = new DataContracts.STG.DC_STG_Mapping_Table_Ids();

                        lstSMT = (from a in context.m_CityMapping
                                  join j in context.STG_Mapping_TableIds.AsNoTracking() on a.CityMapping_Id equals j.Mapping_Id into jact
                                  from jdact in jact.DefaultIfEmpty()
                                  where a.Supplier_Id == curSupplier_Id && jdact.STG_Mapping_Table_Id == null
                                  && (a.Status == "UNMAPPED" || a.City_Id == null)
                                  select new DataContracts.STG.DC_STG_Mapping_Table_Ids
                                  {
                                      STG_Mapping_Table_Id = Guid.NewGuid(),
                                      File_Id = obj.File_Id,
                                      Mapping_Id = a.CityMapping_Id,
                                      STG_Id = null,
                                      Batch = obj.CurrentBatch ?? 1
                                  }).Take(250).ToList();
                        if (lstSMT.Count > 0)
                        {
                            //idinsert = DeleteSTGMappingTableIDs(File_Id);
                            bool idinsert = AddSTGMappingTableIDs(lstSMT);
                        }
                    }

                    var prodMapSearch = (from a in context.m_CityMapping
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CityMapping_Id equals s.Mapping_Id
                                         where s.File_Id == obj.File_Id && a.City_Id == null && a.Supplier_Id == curSupplier_Id
                                         && a.Status == "UNMAPPED" && s.Batch == obj.CurrentBatch
                                         select a);


                    /*if (obj.IsBatched)
                    {
                        prodMapSearch = (from a in prodMapSearch
                                         join s in context.STG_Mapping_TableIds.AsNoTracking() on a.CityMapping_Id equals s.Mapping_Id
                                         //join stg in context.stg_SupplierCityMapping.AsNoTracking() on a.stg_City_Id equals stg.stg_City_Id
                                         where s.Batch == obj.CurrentBatch
                                         select a);
                    }*/
                    bool isCountryCodeCheck = false;
                    bool isCountryNameCheck = false;
                    bool isCodeCheck = false;
                    bool isNameCheck = false;
                    bool isStateNameCheck = false;
                    bool isLatLongCheck = false;
                    totConfigs = configs.Count;
                    curConfig = 0;

                    PLog.SupplierImportFileProgress_Id = Guid.NewGuid();
                    PLog.SupplierImportFile_Id = obj.File_Id;
                    PLog.Step = "MATCH";
                    PLog.Status = "MATCHING";
                    PLog.CurrentBatch = curPriority;
                    PLog.TotalBatch = totPriorities;
                    CallLogVerbose(File_Id, "MATCH", "Applying Matching Combination " + curPriority.ToString() + " out of Total " + totPriorities + " Combinations.");

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        configWhere = configWhere + " " + config.AttributeValue.Replace("m_CityMaster.", "").Trim() + ",";
                    }
                    configWhere = configWhere.Remove(configWhere.Length - 1);
                    CallLogVerbose(File_Id, "MATCH", "Matching Combination " + curPriority.ToString() + " consist of Match by " + configWhere);

                    foreach (DC_SupplierImportAttributeValues config in configs)
                    {
                        curConfig = curConfig + 1;
                        string CurrConfig = "";
                        CurrConfig = config.AttributeValue.Replace("m_CityMaster.", "").Trim().ToUpper();
                        CallLogVerbose(File_Id, "MATCH", "Applying Check for " + CurrConfig);

                        if (CurrConfig == "COUNTRYCODE")
                        {
                            isCountryCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMapping on a.Supplier_Id equals m.Supplier_Id
                                             where a.CountryCode != null && m.CountryCode != null
                                             && a.CountryCode.Trim().ToUpper() == m.CountryCode.Trim().ToUpper()
                                             select a);
                            //join cm in context.m_CountryMapping on new { a.Supplier_Id, a.CountryCode } equals new { cm.Supplier_Id, cm.CountryCode }
                            //join m in context.m_CountryMaster on cm.Country_Id equals m.Country_Id
                            //select a);
                        }
                        if (CurrConfig == "COUNTRYNAME")
                        {
                            isCountryNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CountryMapping on a.Supplier_Id equals m.Supplier_Id
                                             where a.CountryName != null && m.CountryName != null
                                             && a.CountryName.Trim().ToUpper() == m.CountryName.Trim().ToUpper()
                                             select a);
                            //join cm in context.m_CountryMapping on new { a.Supplier_Id, a.CountryName } equals new { cm.Supplier_Id, cm.CountryName }
                            //join m in context.m_CountryMaster on cm.Country_Id equals m.Country_Id
                            //select a);
                        }
                        if (CurrConfig == "CODE")
                        {
                            isCodeCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id
                                             //join mc in context.m_CountryMaster on m.Country_Id equals mc.Country_Id
                                             //join cm in context.m_CountryMapping on new { a.Country_Id, a.Supplier_Id } equals new { cm.Country_Id, cm.Supplier_Id }
                                             where a.CityCode != null && m.Code != null
                                              && a.CityCode == m.Code
                                             select a);
                        }
                        if (CurrConfig == "NAME")
                        {
                            isNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             //join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id // a.CityName equals m.Name
                                             where a.CityName != null //&& m.Name != null
                                             //&& a.CityName == m.Name
                                             select a);
                        }
                        if (CurrConfig == "STATENAME")
                        {
                            isStateNameCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             //join m in context.m_CityMaster on mc.Country_Id equals m.Country_Id // a.CityName equals m.Name
                                             where a.CityName != null //&& m.Name != null
                                             //&& a.CityName == m.Name
                                             select a);
                        }
                        if (CurrConfig == "LATITUDE")
                        {
                            isLatLongCheck = true;
                            prodMapSearch = (from a in prodMapSearch
                                             join m in context.m_CityMaster on new { a.Latitude, a.Longitude } equals new { m.Latitude, m.Longitude }
                                             join mc in context.m_CountryMaster on a.Country_Id equals mc.Country_Id
                                             where mc.Country_Id == m.Country_Id
                                             && m.Latitude != null && a.Latitude != null && m.Longitude != null && a.Longitude != null
                                             select a);
                        }
                        //PLog.PercentageValue = (70 / totPriorities) / totConfigs;
                        PLog.PercentageValue = (PerForEachPriority * (curPriority - 1)) + ((PerForEachPriority / totConfigs) * curConfig);
                        USD.AddStaticDataUploadProcessLog(PLog);

                    }

                    List<DC_CityMapping> res = new List<DC_CityMapping>();

                    if (isCountryCodeCheck || isCountryNameCheck || isCodeCheck || isNameCheck || isLatLongCheck || isStateNameCheck)
                    {
                        res = (from a in prodMapSearch
                                   //join m in context.m_CountryMapping on new
                                   //{
                                   //    supplier = a.Supplier_Id,
                                   //    country = ((a.CountryName == null) ? a.CountryCode : a.CountryName).ToUpper().Trim()
                                   //}
                                   //equals new
                                   //{
                                   //    supplier = m.Supplier_Id,
                                   //    country = ((m.CountryName == null) ? m.CountryCode : m.CountryName).ToUpper().Trim()
                                   //}
                               join ms in context.m_CountryMaster on a.Country_Id equals ms.Country_Id
                               //where m.Status.ToUpper() == "MAPPED" || m.Status.ToUpper() == "REVIEW"
                               select new DataContracts.Mapping.DC_CityMapping
                               {
                                   CityMapping_Id = a.CityMapping_Id,
                                   CityCode = a.CityCode,
                                   CityName = a.CityName,
                                   //CityName = (a.CityName.Replace(a.CountryName.Trim(),"") == "") ? a.CityName : a.CityName.Replace(a.CountryName.Trim(), ""),
                                   City_Id = a.City_Id,
                                   StateCode = a.StateCode,
                                   StateName = a.StateName,
                                   Country_Id = a.Country_Id,
                                   Supplier_Id = a.Supplier_Id,
                                   CountryCode = ms.Code,
                                   CountryName = ms.Name,
                                   Create_Date = a.Create_Date,
                                   Create_User = a.Create_User,
                                   Edit_Date = a.Edit_Date,
                                   Edit_User = a.Edit_User,
                                   MapID = a.MapID,
                                   Status = a.Status,
                                   SupplierName = a.SupplierName,
                                   Latitude = a.Latitude,
                                   Longitude = a.Longitude
                               }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 65;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Looking for Match in Master Data for Matching Combination " + curPriority.ToString() + ".");
                        res = res.Select(c =>
                        {
                            c.City_Id = (context.m_CityMaster
                                            .Where(s => (
                                            #region "Old Code"
                                                            //(isCountryCodeCheck && isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.CountryName == c.CountryName && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                            //(isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode) ||
                                                            //(isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.Name == c.CityName) ||
                                                            //(isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.Code == c.CityCode) ||
                                                            //(isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                            //(isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.CountryName == c.CountryName) ||
                                                            //(isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.CountryName == c.CountryName && s.Name == c.CityName) ||
                                                            //(isCountryCodeCheck && isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryCode == c.CountryCode
                                                            //    && s.CountryName == c.CountryName && s.Code == c.CityCode) ||

                                                            //(!isCountryCodeCheck && isCountryNameCheck && isCodeCheck && isNameCheck && s.CountryName == c.CountryName
                                                            //    && s.Code == c.CityCode && s.Name == c.CityName) ||
                                                            //(!isCountryCodeCheck && !isCountryNameCheck && !isCodeCheck && isNameCheck && s.Name == c.CityName) ||
                                                            //(!isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && !isNameCheck && s.Code == c.CityCode) ||
                                                            //(!isCountryCodeCheck && !isCountryNameCheck && isCodeCheck && isNameCheck && s.Code == c.CityCode 
                                                            //    && s.Name == c.CityName) ||
                                                            //(!isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && !isNameCheck && s.CountryName == c.CountryName) ||
                                                            //(!isCountryCodeCheck && isCountryNameCheck && !isCodeCheck && isNameCheck && s.CountryName == c.CountryName
                                                            //    && s.Name == c.CityName) ||
                                                            //(!isCountryCodeCheck && isCountryNameCheck && isCodeCheck && !isNameCheck && s.CountryName == c.CountryName
                                                            //    && s.Code == c.CityCode)
                                            #endregion
                                                            //((isCountryCodeCheck && s.CountryCode == c.CountryCode) || (!isCountryCodeCheck)) &&
                                                            //((isCountryNameCheck && s.CountryName == c.CountryName) || (!isCountryNameCheck)) &&
                                                            ((isCountryCodeCheck && s.Country_Id == c.Country_Id) || (!isCountryCodeCheck)) &&
                                                            ((isCountryNameCheck && s.Country_Id == c.Country_Id) || (!isCountryNameCheck)) &&
                                                            ((isCodeCheck && s.Code == c.CityCode) || (!isCodeCheck)) &&
                                                            ((isNameCheck && s.Name.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "") == ((c.CityName.Replace(c.CountryName.Trim(), "") == "") ? c.CityName : c.CityName.Replace(c.CountryName.Trim(), "")).Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "")) || (!isNameCheck)) &&
                                                            ((isStateNameCheck && s.StateName.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "") == c.StateName.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(" ", "").Replace("-", "")) || (!isStateNameCheck)) &&
                                                            ((isLatLongCheck && s.Latitude == c.Latitude && s.Longitude == c.Longitude) || (!isLatLongCheck))
                                                        )
                                                   )
                                            .Select(s1 => s1.City_Id)
                                            .FirstOrDefault()
                                            );
                            return c;
                        }).ToList();

                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 70;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }

                        CallLogVerbose(File_Id, "MATCH", "Setting Appropriate Status.");
                        res.RemoveAll(p => p.City_Id == Guid.Empty);
                        res = res.Select(c =>
                        {
                            c.MatchedBy = curPriority - 1;
                            c.Status = ("REVIEW"); return c;
                        }).ToList();


                        CallLogVerbose(File_Id, "MATCH", res.Count.ToString() + " Matches Found for Combination " + curPriority.ToString() + ".");
                        CallLogVerbose(File_Id, "MATCH", "Updating into Database.");
                        if (UpdateCityMapping(res))
                        {
                            if ((obj.FileMode ?? "ALL") == "ALL" && totPriorities == curPriority)
                            {
                                PLog.PercentageValue = 75;
                                USD.AddStaticDataUploadProcessLog(PLog);

                                DataContracts.UploadStaticData.DC_SupplierImportFile_Statistics objStat = new DC_SupplierImportFile_Statistics();
                                objStat.SupplierImportFile_Statistics_Id = Guid.NewGuid();
                                objStat.SupplierImportFile_Id = obj.File_Id;
                                objStat.From = "MATCHING";
                                DataContracts.DC_Message stat = USD.AddStaticDataUploadStatistics(objStat);

                                using (ConsumerEntities context1 = new ConsumerEntities())
                                {
                                    var oldRecords = (from y in context1.STG_Mapping_TableIds
                                                      where y.File_Id == File_Id
                                                      select y).ToList();
                                    context1.STG_Mapping_TableIds.RemoveRange(oldRecords);
                                    context1.SaveChanges();
                                }
                            }
                            //bool del = DeleteSTGMappingTableIDs(Guid.Parse(obj.File_Id.ToString()));
                            retrn = true;
                            CallLogVerbose(File_Id, "MATCH", "Update Done.");
                            //if (curSupplier_Id != null)
                            //{
                            //    DataContracts.Mapping.DC_CityMapping_RQ RQ = new DC_CityMapping_RQ();
                            //    RQ.Supplier_Id = curSupplier_Id;
                            //    RQ.PageNo = 0;
                            //    RQ.PageSize = int.MaxValue;
                            //    RQ.Status = "UNMAPPED";
                            //    RQ.CalledFromTLGX = "TLGX";
                            //    res = GetCityMapping(RQ);
                            //}
                        }
                        if (totPriorities == curPriority)
                        {
                            PLog.PercentageValue = 100;
                            USD.AddStaticDataUploadProcessLog(PLog);
                        }
                    }
                    else
                    {
                        retrn = true;
                        //if (curSupplier_Id != null)
                        //{
                        //    DataContracts.Mapping.DC_CityMapping_RQ RQ = new DC_CityMapping_RQ();
                        //    RQ.Supplier_Id = curSupplier_Id;
                        //    RQ.PageNo = 0;
                        //    RQ.PageSize = int.MaxValue;
                        //    RQ.Status = "UNMAPPED";
                        //    RQ.CalledFromTLGX = "TLGX";
                        //    res = GetCityMapping(RQ);
                        //}
                    }
                    //return res;
                }
                return retrn;
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
            if (obj.Count > 0)
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var CountryMapping = context.m_CountryMapping.Where(w => w.CountryName != null || w.CountryCode != null).AsQueryable();
                    var CityMapping = context.m_CityMapping.Select(s => s).AsQueryable();
                    foreach (var CM in obj)
                    {
                        if (CM.CityMapping_Id == null || CM.Supplier_Id == null)
                        {
                            continue;
                        }

                        try
                        {

                            var search = (from a in CityMapping
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

                                if (CM.MatchedBy != null)
                                    search.MatchedBy = CM.MatchedBy;
                                search.Status = CM.Status;
                                search.Edit_Date = CM.Edit_Date;
                                search.Edit_User = CM.Edit_User;
                                search.Remarks = CM.Remarks;
                                if (search.StateCode == null)
                                    search.StateCode = CM.StateCode;
                                if (search.StateName == null)
                                    search.StateName = CM.StateName;
                                //}
                                context.SaveChanges();

                                //context.SaveChanges();
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
                                objNew.Latitude = CM.Latitude;
                                objNew.Longitude = CM.Longitude;
                                objNew.StateCode = CM.StateCode;
                                objNew.StateName = CM.StateName;
                                // objNew.Country_Id = CM.Country_Id;
                                objNew.Country_Id = ((from a in CountryMapping
                                                      where a.Supplier_Id == CM.Supplier_Id && a.CountryName == CM.CountryName
                                                      //((a.CountryName == CM.CountryName) && a.CountryName != null && CM.CountryName != null)
                                                      //&& ((CM.CountryName != null && a.CountryName == CM.CountryName) || CM.CountryName == null)
                                                      //&& ((CM.CountryCode != null && a.CountryCode == CM.CountryCode) || CM.CountryCode == null)
                                                      //&& a.Supplier_Id == CM.Supplier_Id
                                                      select a.Country_Id).FirstOrDefault()) ?? ((from a in CountryMapping
                                                                                                  where a.Supplier_Id == CM.Supplier_Id && a.CountryCode == CM.CountryCode
                                                                                                  select a.Country_Id).FirstOrDefault());
                                context.m_CityMapping.Add(objNew);
                            }

                            ret = true;


                        }
                        catch
                        {
                            ret = false;
                            throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating city mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
                        }
                    }
                    if (ret)
                    {
                        context.SaveChanges();
                        context.USP_UpdateMapID("city");
                    }
                }
            }
            else
                ret = true;
            return ret;
        }
        #endregion

        #region Mapping Stats
        public List<DataContracts.Mapping.DC_MappingStats> GetMappingStatistics(Guid SupplierID, int Priority)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<Dashboard_MappingStat> searchResult = new List<Dashboard_MappingStat>();

                    context.Database.CommandTimeout = 0;
                    List<DataContracts.Mapping.DC_MappingStats> returnObj = new List<DataContracts.Mapping.DC_MappingStats>();
                    List<DataContracts.Mapping.DC_MappingStatsFor> newmapstatsforList = new List<DataContracts.Mapping.DC_MappingStatsFor>();
                    DataContracts.Mapping.DC_MappingStats newmapstats = new DataContracts.Mapping.DC_MappingStats();

                    int CitySupplierCount = 0;
                    int CountrySupplierCount = 0;
                    int HotelSupplierCount = 0;
                    int RoomSupplierCount = 0;
                    int ActivitySupplierCount = 0;

                    newmapstats.SupplierId = SupplierID;
                    newmapstats.SupplierName = string.Empty;

                    var search = context.Dashboard_MappingStat.AsQueryable().Where(w => w.Batch == 1); //&& w.SupplierName != "ALL"

                    if (SupplierID != Guid.Empty)
                    {
                        searchResult.AddRange(search.Where(w => w.supplier_id == SupplierID).ToList());

                        var run = (from s in context.Schedule_NextOccurance where s.Schedule_ID == SupplierID select s.Execution_StartDate).ToList();
                        if (run.Count > 0)
                        {
                            newmapstats.NextRun = Convert.ToString(run[0].Date);
                        }
                        else
                        {
                            newmapstats.NextRun = null;
                        }
                        //newmapstats.NextRun = (from s in context.Schedule_NextOccurance where s.Schedule_ID == SupplierID select s.Execution_StartDate).FirstOrDefault().ToString();
                    }
                    else if (SupplierID == Guid.Empty)
                    {
                        newmapstats.NextRun = "Not Scheduled";

                        if (Priority > 0)
                        {
                            CitySupplierCount = context.Supplier.Where(w => w.Priority == Priority && search.Where(sw => sw.MappingFor == "City").Any(a => a.supplier_id == w.Supplier_Id)).Select(sel => sel.Supplier_Id).Count();

                            //int CitySupplierCountt = (from t1 in search
                            //                     join t2 in context.Supplier on t1.supplier_id equals t2.Supplier_Id
                            //                     where t2.Priority == Priority && t1.MappingFor == "City"
                            //                     select t2.Supplier_Id).Distinct().Count();

                            CountrySupplierCount = context.Supplier.Where(w => w.Priority == Priority && search.Where(sw => sw.MappingFor == "Country").Any(a => a.supplier_id == w.Supplier_Id)).Select(sel => sel.Supplier_Id).Count();

                            HotelSupplierCount = context.Supplier.Where(w => w.Priority == Priority && search.Where(sw => sw.MappingFor == "Product").Any(a => a.supplier_id == w.Supplier_Id)).Select(sel => sel.Supplier_Id).Count();

                            RoomSupplierCount = context.Supplier.Where(w => w.Priority == Priority && search.Where(sw => sw.MappingFor == "HotelRoom").Any(a => a.supplier_id == w.Supplier_Id)).Select(sel => sel.Supplier_Id).Count();

                            ActivitySupplierCount = context.Supplier.Where(w => w.Priority == Priority && search.Where(sw => sw.MappingFor == "Activity").Any(a => a.supplier_id == w.Supplier_Id)).Select(sel => sel.Supplier_Id).Count();


                            searchResult.AddRange((from t1 in search.ToList()
                                                   join t2 in context.Supplier on t1.supplier_id equals t2.Supplier_Id
                                                   where t2.Priority == Priority
                                                   group t1 by new { MapFor = t1.MappingFor, Stat = t1.Status } into sg
                                                   select new Dashboard_MappingStat
                                                   {
                                                       supplier_id = Guid.Empty,
                                                       SupplierName = "ALL",
                                                       totalcount = sg.Sum(x => x.totalcount) ?? 0,
                                                       Status = sg.Key.Stat,
                                                       MappingFor = sg.Key.MapFor,
                                                   }).ToList());
                        }
                        else if (Priority == 0)
                        {
                            searchResult.AddRange(search.Where(w => w.SupplierName == "ALL").ToList());
                            CitySupplierCount = (from s in searchResult where s.MappingFor == "City" && s.Status == "ALL" select s.SuppliersCount).FirstOrDefault() ?? 0;
                            CountrySupplierCount = (from s in searchResult where s.MappingFor == "Country" && s.Status == "ALL" select s.SuppliersCount).FirstOrDefault() ?? 0;
                            HotelSupplierCount = (from s in searchResult where s.MappingFor == "Product" && s.Status == "ALL" select s.SuppliersCount).FirstOrDefault() ?? 0;
                            RoomSupplierCount = (from s in searchResult where s.MappingFor == "HotelRoom" && s.Status == "ALL" select s.SuppliersCount).FirstOrDefault() ?? 0;
                            ActivitySupplierCount = (from s in searchResult where s.MappingFor == "Activity" && s.Status == "ALL" select s.SuppliersCount).FirstOrDefault() ?? 0;
                        }


                    }

                    var MapForList = "City,Country,Product,Activity,HotelRoom".Split(',');
                    //var MapForList = (from s in search select s.MappingFor).ToList().Distinct();

                    foreach (var mapfor in MapForList)
                    {
                        DataContracts.Mapping.DC_MappingStatsFor newmapstatsfor = new DataContracts.Mapping.DC_MappingStatsFor();

                        newmapstatsfor.MappingFor = mapfor;

                        var AllCount = (from s in searchResult where s.MappingFor == mapfor && s.Status == "ALL" select s.totalcount).FirstOrDefault();

                        var MappedCount = (from s in searchResult where s.MappingFor == mapfor && s.Status == "MAPPED" select s.totalcount).FirstOrDefault();

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
                        //suppliercount
                        int supCount;
                        if (mapfor == "City")
                        {
                            supCount = CitySupplierCount;
                        }
                        else if (mapfor == "Country")
                        {
                            supCount = CountrySupplierCount;
                        }
                        else if (mapfor == "Product")
                        {
                            supCount = HotelSupplierCount;
                        }
                        else if (mapfor == "Activity")
                        {
                            supCount = ActivitySupplierCount;
                        }
                        else if (mapfor == "HotelRoom")
                        {
                            supCount = RoomSupplierCount;
                        }
                        else { supCount = 0; }
                        //end SupCount

                        newmapstatsfor.MappingData = (from s in searchResult
                                                      where s.MappingFor == mapfor
                                                      orderby s.Status
                                                      select new DataContracts.Mapping.DC_MappingData
                                                      {
                                                          Status = s.Status,
                                                          TotalCount = (s.totalcount ?? 0),
                                                          SuppliersCount = supCount,
                                                      }).ToList();


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


        public List<DataContracts.Mapping.DC_MappingStatsForSuppliers> GetMappingStatisticsForSuppliers(int PriorityId)
        {
            List<DataContracts.Mapping.DC_MappingStatsForSuppliers> objLst = new List<DataContracts.Mapping.DC_MappingStatsForSuppliers>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;

                    var dashBoard = context.Dashboard_MappingStat.Where(w => w.Status == "UNMAPPED" || w.Status == "REVIEW").AsQueryable();
                    dashBoard = dashBoard.Where(w => w.supplier_id != Guid.Empty);
                    dashBoard = dashBoard.Where(w => w.Batch == 1);

                    if (PriorityId != 0)
                    {
                        dashBoard = (from t1 in dashBoard
                                     join t2 in context.Supplier on t1.supplier_id equals t2.Supplier_Id
                                     where t2.Priority == PriorityId
                                     select t1);
                    }

                    objLst = dashBoard.GroupBy(cat => new { cat.SupplierName, cat.supplier_id, cat.MappingFor })
                                .Select(group => new DC_MappingStatsForSuppliers
                                {
                                    SupplierName = group.Key.SupplierName,
                                    //SupplierId = group.Key.supplier_id,
                                    Mappingfor = group.Key.MappingFor,
                                    totalcount = group.Sum(x => x.totalcount) ?? 0
                                }).OrderBy(x => x.SupplierName).ToList();

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
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
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
        public List<DataContracts.Mapping.DC_RollOffReportStatus> getStatisticforStatusReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportStatus> objLst = new List<DC_RollOffReportStatus>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_Status
                                  join t1 in context.Accommodations on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.From >= fd && t.To <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      market = t.CompanyMarket,
                                      status = t.Status,
                                      fromd = t.From,
                                      tod = t.To,
                                      reason = t.DeactivationReason,
                                      // flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportStatus obj = new DC_RollOffReportStatus();
                        obj.Hotelid = item.HotelID.Value;
                        obj.Hotelname = item.HotelName;
                        obj.Companymarket = item.market;
                        obj.Status = item.status;

                        if (item.fromd != null)
                        {
                            obj.Validfrom = item.fromd.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validfrom = ""; }
                        if (item.tod != null)
                        {
                            obj.Validto = item.tod.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validto = ""; }
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
                        //obj.Validto = item.tod.Value.ToString("dd/MM/yyyy") ?? "";
                        //obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy") ?? "";
                        obj.Reason = item.reason;
                        // obj.Internal_Flag = item.flag;
                        obj.LastupdatedBy = item.LupdateBy;
                        objLst.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objLst;
        }
        public List<DataContracts.Mapping.DC_RollOffReportUpdate> getStatisticforUpdateReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_RollOffReportUpdate> objLst = new List<DC_RollOffReportUpdate>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodation_HotelUpdates
                                  join t1 in context.Accommodations on t.Accommodation_Id equals t1.Accommodation_Id
                                  where (t.FromDate >= fd && t.ToDate <= td)
                                  select new
                                  {
                                      HotelID = t1.CompanyHotelID,
                                      HotelName = t1.HotelName,
                                      HotelUpdate = t.Description,
                                      source = t.Source,
                                      fromd = t.FromDate,
                                      tod = t.ToDate,
                                      flag = t.IsInternal == true ? "YES" : "NO",
                                      LupdateDate = t.Edit_Date == null ? t.Create_Date : t.Edit_Date,
                                      LupdateBy = t.Edit_User == null ? t.Create_User : t.Edit_User
                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_RollOffReportUpdate obj = new DC_RollOffReportUpdate();
                        obj.Hotelid = item.HotelID.Value;
                        obj.Hotelname = item.HotelName;
                        obj.Hotelupdate = item.HotelUpdate;
                        obj.Descriptionsource = item.source;
                        if (item.fromd != null)
                        {
                            obj.Validfrom = item.fromd.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validfrom = ""; }
                        if (item.tod != null)
                        {
                            obj.Validto = item.tod.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Validto = ""; }
                        if (item.LupdateDate != null)
                        {
                            obj.LastupdateDate = item.LupdateDate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.LastupdateDate = ""; }
                        // obj.Validfrom = Convert.ToString(item.fromd);
                        // obj.Validto = Convert.ToString(item.tod);
                        obj.Internal_Flag = item.flag;
                        // obj.LastupdateDate = Convert.ToString(item.LupdateDate);
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

        #region rdlc reports
        public List<DataContracts.Mapping.DC_supplierwiseUnmappedReport> GetsupplierwiseUnmappedDataReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_supplierwiseUnmappedReport> _objList = new List<DC_supplierwiseUnmappedReport>();
            DataContracts.Mapping.DC_supplierwiseUnmappedReport _obj = new DC_supplierwiseUnmappedReport();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcountry = (from s in context.m_CountryMapping
                                             where s.Supplier_Id == SupplierID
        && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        var searchcity = (from s in context.m_CityMapping
                                          where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                          select s).ToList();
                        var searchproduct = (from s in context.Accommodation_ProductMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        var searchactivity = (from s in context.Activity_SupplierProductMapping
                                              where s.Supplier_ID == SupplierID && (s.MappingStatus == "UNMAPPED" || s.MappingStatus == "REVIEW")
                                              select s).ToList();
                        List<DC_UnmappedCountryReport> _lstCountry = new List<DC_UnmappedCountryReport>();
                        foreach (var item in searchcountry)
                        {
                            DC_UnmappedCountryReport objCo = new DC_UnmappedCountryReport();
                            objCo.Countrycode = item.CountryCode;
                            objCo.Contryname = item.CountryName;
                            _lstCountry.Add(objCo);

                        }
                        _obj.Unmappedcountry = _lstCountry;
                        List<DC_UnmappedCityReport> _lstCity = new List<DC_UnmappedCityReport>();
                        foreach (var item in searchcity)
                        {
                            DC_UnmappedCityReport objcity = new DC_UnmappedCityReport();
                            objcity.Countrycode = item.CountryCode;
                            objcity.Countryname = item.CountryName;
                            objcity.Citycode = item.CityCode;
                            objcity.Cityname = item.CityName;
                            _lstCity.Add(objcity);

                        }
                        _obj.Unmappedcity = _lstCity;

                        List<DC_unmappedProductReport> _lstproduct = new List<DC_unmappedProductReport>();
                        foreach (var item in searchproduct)
                        {
                            DC_unmappedProductReport objpro = new DC_unmappedProductReport();
                            objpro.Hotelname = item.ProductName;
                            objpro.Country = item.CountryName;
                            objpro.City = item.CityName;
                            objpro.Address = item.address;
                            _lstproduct.Add(objpro);

                        }
                        _obj.Unmappedproduct = _lstproduct;

                        List<DC_unmappedActivityReport> _lstactivity = new List<DC_unmappedActivityReport>();

                        foreach (var item in searchactivity)
                        {
                            DC_unmappedActivityReport objact = new DC_unmappedActivityReport();
                            objact.Activityname = item.SupplierProductName;
                            objact.Country = item.SupplierCountryName;
                            objact.City = item.SupplierCityName;
                            objact.Address = item.Address;
                            _lstactivity.Add(objact);
                        }
                        _obj.Unmappedactivity = _lstactivity;
                        _objList.Add(_obj);

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;
        }
        public List<DataContracts.Mapping.DC_UnmappedCountryReport> GetsupplierwiseUnmappedCountryReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_UnmappedCountryReport> _objList = new List<DC_UnmappedCountryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcountry = (from s in context.m_CountryMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        foreach (var item in searchcountry)
                        {
                            DC_UnmappedCountryReport objCo = new DC_UnmappedCountryReport();
                            objCo.Countrycode = item.CountryCode;
                            objCo.Contryname = item.CountryName;
                            _objList.Add(objCo);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }
        public List<DataContracts.Mapping.DC_UnmappedCityReport> GetsupplierwiseUnmappedCityReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_UnmappedCityReport> _objList = new List<DC_UnmappedCityReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchcity = (from s in context.m_CityMapping
                                          where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                          select s).ToList();
                        foreach (var item in searchcity)
                        {
                            DC_UnmappedCityReport objCi = new DC_UnmappedCityReport();
                            objCi.Countrycode = item.CountryCode;
                            objCi.Countryname = item.CountryName;
                            objCi.Citycode = item.CityCode;
                            objCi.Cityname = item.CityName;
                            _objList.Add(objCi);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }

        public List<DataContracts.Mapping.DC_unmappedProductReport> GetsupplierwiseUnmappedProductReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_unmappedProductReport> _objList = new List<DC_unmappedProductReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchproduct = (from s in context.Accommodation_ProductMapping
                                             where s.Supplier_Id == SupplierID && (s.Status == "UNMAPPED" || s.Status == "REVIEW")
                                             select s).ToList();
                        foreach (var item in searchproduct)
                        {
                            DC_unmappedProductReport objPi = new DC_unmappedProductReport();
                            objPi.Hotelname = item.ProductName;
                            objPi.Country = item.CountryName;
                            objPi.City = item.CityName;
                            objPi.Address = item.address;
                            objPi.SupplierName = item.SupplierName;
                            objPi.SupplierHotelId = item.SupplierProductReference;
                            _objList.Add(objPi);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }
        public List<DataContracts.Mapping.DC_unmappedActivityReport> GetsupplierwiseUnmappedActivityReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_unmappedActivityReport> _objList = new List<DC_unmappedActivityReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchactivity = (from s in context.Activity_SupplierProductMapping
                                              where s.Supplier_ID == SupplierID && (s.MappingStatus == "UNMAPPED" || s.MappingStatus == "REVIEW")
                                              select s).ToList();
                        foreach (var item in searchactivity)
                        {
                            DC_unmappedActivityReport objac = new DC_unmappedActivityReport();
                            objac.Activityname = item.SupplierProductName;
                            objac.Country = item.SupplierCountryName;
                            objac.City = item.SupplierCityName;
                            objac.Address = item.Address;
                            objac.SupplierName = item.SupplierName;
                            objac.SupplierActivityId = item.SupplierCode;
                            _objList.Add(objac);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;

        }
        public List<DataContracts.Mapping.DC_supplierwisesummaryReport> GetsupplierwiseSummaryReport()
        {
            List<DataContracts.Mapping.DC_supplierwisesummaryReport> _objList = new List<DC_supplierwisesummaryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var searchasummary = context.vwMappingStats.
                         Where(c => c.SupplierName != "ALL" && c.SupplierName != null && (c.Status == "UNMAPPED" || c.Status == "REVIEW") && c.supplier_id != Guid.Empty)
                        .GroupBy(c => new { c.supplier_id, c.SupplierName })
                        .Select(g => new
                        {
                            Country = g.Where(c => c.MappinFor == "Country").Sum(c => c.totalcount),
                            City = g.Where(c => c.MappinFor == "City").Sum(c => c.totalcount),
                            Product = g.Where(c => c.MappinFor == "Product").Sum(c => c.totalcount),
                            hotelroom = g.Where(c => c.MappinFor == "HotelRoom").Sum(c => c.totalcount),
                            activity = g.Where(c => c.MappinFor == "Activity").Sum(c => c.totalcount),
                            suppliername = g.Key.SupplierName,
                        }
                        ).ToList();
                    foreach (var item in searchasummary)
                    {
                        DC_supplierwisesummaryReport objsu = new DC_supplierwisesummaryReport();
                        objsu.Country = item.Country.GetValueOrDefault();
                        objsu.City = item.City.GetValueOrDefault();
                        objsu.Product = item.Product.GetValueOrDefault();
                        objsu.Activity = item.activity.GetValueOrDefault();
                        objsu.Hotelrooom = item.hotelroom.GetValueOrDefault();
                        objsu.Suppliername = item.suppliername;
                        _objList.Add(objsu);

                    }

                }


            }
            catch (Exception ex)
            {

            }
            return _objList;

        }
        public List<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> GetsupplierwiseUnmappedSummaryReport(Guid SupplierID)
        {
            List<DataContracts.Mapping.DC_supplierwiseunmappedsummaryReport> _objList = new List<DC_supplierwiseunmappedsummaryReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (SupplierID != Guid.Empty)
                    {
                        var searchasummary = (from cm in context.m_CountryMapping
                                              join ctm in context.m_CityMapping on
                                                  new { cm.CountryName, cm.Supplier_Id } equals new { ctm.CountryName, ctm.Supplier_Id }
                                              let prod = (from apm in context.Accommodation_ProductMapping
                                                          where (apm.Status == "UNMAPPED" || apm.Status == "REVIEW") &&
                                                          apm.CityName == ctm.CityName && apm.CountryName == cm.CountryName
                                                          && apm.Supplier_Id == ctm.Supplier_Id
                                                          select apm
                                                    ).Count()
                                              where (ctm.Status == "UNMAPPED" || ctm.Status == "REVIEW")
                                                     && ctm.Supplier_Id == SupplierID
                                              select new DC_supplierwiseunmappedsummaryReport
                                              {
                                                  Suppliername = cm.SupplierName,
                                                  Noofproducts = prod,
                                                  Cityname = ctm.CityName,
                                                  Countryname = cm.CountryName
                                              }).Distinct().ToList();
                        foreach (var item in searchasummary)
                        {
                            DC_supplierwiseunmappedsummaryReport objsu = new DC_supplierwiseunmappedsummaryReport();
                            objsu.Suppliername = item.Suppliername;
                            objsu.Noofproducts = item.Noofproducts;
                            objsu.Cityname = item.Cityname;
                            objsu.Countryname = item.Countryname;
                            _objList.Add(objsu);

                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return _objList;
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
                                      join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
                                      join ma in context.m_masterattribute on a.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                      orderby sup.Name
                                      select new DataContracts.Mapping.DC_MasterAttributeMapping_RS
                                      {
                                          MasterAttributeMapping_Id = a.MasterAttributeMapping_Id,
                                          Supplier_Attribute_Type = a.SupplierMasterAttribute,
                                          Supplier_Code = sup.Code,
                                          Supplier_Name = sup.Name,
                                          System_Attribute_Type = ma.Name,
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
                                  join sup in context.Supplier on a.Supplier_Id equals sup.Supplier_Id
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

        public List<DataContracts.Mapping.DC_MasterAttributeValueMappingRS> GetMasterAttributeValueMapping(DataContracts.Mapping.DC_MasterAttributeValueMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var m_MasterAttributeMapping = (from a in context.m_MasterAttributeMapping select a).AsQueryable();
                    var m_masterattribute = (from a in context.m_masterattribute select a).AsQueryable();
                    var m_masterattributevalue = (from a in context.m_masterattributevalue select a).AsQueryable();
                    var m_masterattributevalueGlobal = (from a in context.m_masterattributevalue select a).AsQueryable();
                    var m_MasterAttributeValueMapping = (from a in context.m_MasterAttributeValueMapping select a).AsQueryable();

                    if (RQ.MasterAttributeMapping_Id != null)
                    {
                        if (RQ.MasterAttributeMapping_Id != Guid.Empty)
                        {
                            m_MasterAttributeMapping = from a in m_MasterAttributeMapping where a.MasterAttributeMapping_Id == RQ.MasterAttributeMapping_Id select a;
                            // m_MasterAttributeValueMapping = from a in m_MasterAttributeValueMapping where a.MasterAttributeMapping_Id == RQ.MasterAttributeMapping_Id select a;
                        }
                    }

                    if (RQ.SystemMasterAttributeValue_Id != null)
                    {
                        if (RQ.SystemMasterAttributeValue_Id != Guid.Empty)
                        {
                            m_masterattributevalue = from a in m_masterattributevalue where a.MasterAttributeValue_Id == RQ.SystemMasterAttributeValue_Id select a;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.SystemMasterAttributeValue))
                    {
                        m_masterattributevalue = from a in m_masterattributevalue where a.AttributeValue.ToUpper().Trim() == RQ.SystemMasterAttributeValue.ToUpper().Trim() select a;
                    }

                    int total = (from map in m_MasterAttributeMapping
                                 join ma in m_masterattribute on map.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                 join mav in m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                                 where mav.IsActive == true
                                 select mav).Count();

                    var skip = RQ.PageSize * RQ.PageNo;

                    var searchReturn = (from map in m_MasterAttributeMapping
                                        join ma in m_masterattribute on map.SystemMasterAttribute_Id equals ma.MasterAttribute_Id
                                        join mav in m_masterattributevalue on ma.MasterAttribute_Id equals mav.MasterAttribute_Id
                                        join mavp in m_masterattributevalueGlobal on (mav.ParentAttributeValue_Id ?? Guid.Empty) equals mavp.MasterAttributeValue_Id into mavploj
                                        from mavplojr in mavploj.DefaultIfEmpty()
                                        where mav.IsActive == true
                                        orderby (mavplojr.AttributeValue ?? string.Empty).Trim(), mav.AttributeValue.Trim()
                                        select new DataContracts.Mapping.DC_MasterAttributeValueMappingRS
                                        {
                                            MasterAttributeMapping_Id = map.MasterAttributeMapping_Id,
                                            ParentAttributeValue = (mavplojr.AttributeValue ?? string.Empty).Trim(),
                                            SystemMasterAttributeValue = mav.AttributeValue.Trim(),
                                            SystemMasterAttributeValue_Id = mav.MasterAttributeValue_Id,
                                            TotalRecords = total,
                                            SupplierAttributeValues = (from imavm in m_MasterAttributeValueMapping
                                                                       where imavm.MasterAttributeMapping_Id == map.MasterAttributeMapping_Id
                                                                       && imavm.SystemMasterAttributeValue_Id == mav.MasterAttributeValue_Id
                                                                       select new DataContracts.Mapping.DC_SupplierAttributeValues
                                                                       {
                                                                           MasterAttributeValueMapping_Id = imavm.MasterAttributeValueMapping_Id,
                                                                           SupplierMasterAttributeValue = imavm.SupplierMasterAttributeValue.Trim(),
                                                                           SupplierMasterAttributeCode = imavm.SupplierMasterAttributeCode.Trim(),
                                                                           Create_User = imavm.Create_User,
                                                                           Create_Date = imavm.Create_Date,
                                                                           Edit_Date = imavm.Edit_Date,
                                                                           Edit_User = imavm.Edit_User,
                                                                           IsActive = imavm.IsActive
                                                                       }).ToList()
                                        }).Skip(skip).Take(RQ.PageSize);

                    return searchReturn.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching master attribute value mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_MasterAttributeMappingAdd_RS AddMasterAttributeMapping(DataContracts.Mapping.DC_MasterAttributeMapping param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var duplicateSearch = from a in context.m_MasterAttributeMapping
                                          where a.Supplier_Id == param.Supplier_Id
                                          && a.SystemMasterAttribute_Id == param.SystemMasterAttribute_Id
                                          select a;
                    if (duplicateSearch.Count() > 0)
                    {
                        return new DC_MasterAttributeMappingAdd_RS
                        {
                            Message = new DataContracts.DC_Message
                            {
                                StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate,
                                StatusMessage = "Data already exist."
                            }
                        };
                    }
                    else
                    {
                        m_MasterAttributeMapping newObj = new m_MasterAttributeMapping();

                        if (param.MasterAttributeMapping_Id == Guid.Empty)
                        {
                            param.MasterAttributeMapping_Id = Guid.NewGuid();
                        }

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

                        return new DC_MasterAttributeMappingAdd_RS
                        {
                            Message = new DataContracts.DC_Message
                            {
                                StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                                StatusMessage = "Data saved successfully."
                            },
                            AttributeMapping_Id = param.MasterAttributeMapping_Id
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

        public DataContracts.DC_Message UpdateMasterAttributeValueMapping(List<DataContracts.Mapping.DC_MasterAttributeValueMapping> paramList)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var param in paramList)
                    {
                        var search = context.m_MasterAttributeValueMapping.Find(param.MasterAttributeValueMapping_Id);

                        if (search != null)
                        {
                            search.IsActive = param.IsActive ?? false;
                            search.Edit_Date = DateTime.Now;
                            search.Edit_User = param.Edit_User;
                            search.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;
                            search.SupplierMasterAttributeCode = param.SupplierMasterAttributeCode;
                            context.SaveChanges();
                        }
                        else
                        {
                            m_MasterAttributeValueMapping newObj = new m_MasterAttributeValueMapping();
                            newObj.MasterAttributeMapping_Id = param.MasterAttributeMapping_Id;
                            newObj.MasterAttributeValueMapping_Id = param.MasterAttributeValueMapping_Id ?? Guid.NewGuid();
                            newObj.SupplierMasterAttributeValue = param.SupplierMasterAttributeValue;
                            newObj.SupplierMasterAttributeCode = param.SupplierMasterAttributeCode;
                            newObj.SystemMasterAttributeValue_Id = param.SystemMasterAttributeValue_Id;
                            newObj.IsActive = param.IsActive ?? false;
                            newObj.Create_Date = DateTime.Now;
                            newObj.Create_User = param.Create_User;
                            context.m_MasterAttributeValueMapping.Add(newObj);
                            context.SaveChanges();
                        }
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

        public DataContracts.DC_Message DeleteMasterAttributeValueMapping(DC_SupplierAttributeValues_RQ param)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (param.MasterAttributeValueMapping_Id != null)
                    {
                        var search = context.m_MasterAttributeValueMapping.Find(param.MasterAttributeValueMapping_Id);
                        if (search != null)
                        {
                            context.m_MasterAttributeValueMapping.Remove(search);
                        }
                        context.SaveChanges();
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success,
                            StatusMessage = "Data Deleted successfully."
                        };
                    }
                    else
                    {
                        return new DataContracts.DC_Message
                        {
                            StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning,
                            StatusMessage = "No data found to delete."
                        };
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while Deleting Supplier attribute value", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
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
                    //if (!string.IsNullOrWhiteSpace(obj.SearchFor) &&  obj.SearchFor.ToLower() == "activitymapping")
                    //{
                    //    //Get Supplier name 
                    //    var result = GetActivitySupplierProductMappingSearchForDDL(new DC_Acitivity_SupplierProductMapping_Search_RQ() { SupplierCountryName = obj.SupplierCountryName, SupplierCityName = obj.SupplierCityName});
                    //    prodMapSearch = from a in prodMapSearch
                    //                    join lstsupplier in result on a.Supplier_ID equals lstsupplier.Supplier_ID
                    //                    select a;
                    //}

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
                                   join act in context.Activities on a.Activity_ID equals act.Activity_Id into ja
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
                                        item.Activity_ID = searchActivity.Activity_Id;
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

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMapping> GetActivitySupplierProductMappingSearchForMapping(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
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
                    prodMapSearch = from a in prodMapSearch
                                    join sup in context.Supplier on a.Supplier_ID equals sup.Supplier_Id
                                    select a;

                    int total;

                    total = prodMapSearch.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMap = (from a in prodMapSearch
                                   join act in context.Activities on a.Activity_ID equals act.Activity_Id into ja
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
                                        item.Activity_ID = searchActivity.Activity_Id;
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
        public bool IsMappedWithSupplier(Guid masterActivityID, Guid supplierID)
        {
            bool isMapped = false;
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (supplierID != Guid.Empty && masterActivityID != Guid.Empty)
                    {
                        var result = context.Activity_SupplierProductMapping.Where(x => x.Supplier_ID == supplierID && x.MappingStatus.ToLower() == "mapped" && x.Activity_ID == masterActivityID).Select(x => x.ActivitySupplierProductMapping_Id).ToList();
                        if (result != null && result.Count > 0)
                        {
                            isMapped = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isMapped;
        }

        public List<DataContracts.Mapping.DC_Acitivity_SupplierProductMappingForDDL> GetActivitySupplierProductMappingSearchForDDL(DataContracts.Mapping.DC_Acitivity_SupplierProductMapping_Search_RQ obj)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var prodMapSearch = from a in context.Activity_SupplierProductMapping select a;

                    if (!string.IsNullOrWhiteSpace(obj.SystemCountryName))
                    {
                        var distCountryMapping = (from a in context.m_CountryMapping select new { a.Country_Id, a.CountryName, a.Supplier_Id }).Distinct();
                        prodMapSearch = from a in prodMapSearch
                                        join mas in distCountryMapping on a.SupplierCountryName.ToLower().Trim() equals mas.CountryName.ToLower().Trim()
                                        join mct in context.m_CountryMaster on mas.Country_Id equals mct.Country_Id
                                        join sup in context.Supplier on a.Supplier_ID equals sup.Supplier_Id
                                        where mct.Name == obj.SystemCountryName
                                        select a;
                    }
                    var result = prodMapSearch.GroupBy(x => x.Supplier_ID).Select(x => x.FirstOrDefault());

                    int total;

                    total = result.Count();

                    var skip = obj.PageSize * obj.PageNo;

                    var prodMapList = result.ToList();

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
                                       join act in context.Activities on a.Activity_ID equals act.Activity_Id
                                       where act.Activity_Id == Activity_Id
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

        #region hotel report
        public List<DataContracts.Mapping.DC_newHotelsReport> getNewHotelsAddedReport(DataContracts.Mapping.DC_RollOFParams parm)
        {
            List<DataContracts.Mapping.DC_newHotelsReport> objLst = new List<DC_newHotelsReport>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DateTime fd = Convert.ToDateTime(parm.Fromdate);
                    DateTime td = Convert.ToDateTime(parm.ToDate);
                    var search = (from t in context.Accommodations
                                  where (t.Create_Date >= fd && t.Create_Date <= td) && (t.InsertFrom == true)
                                  select new
                                  {
                                      HotelID = t.CompanyHotelID,
                                      HotelName = t.HotelName,
                                      country = t.country,
                                      city = t.city,
                                      createdate = t.Create_Date,
                                      createby = t.Create_User

                                  }).ToList();
                    foreach (var item in search)
                    {

                        DC_newHotelsReport obj = new DC_newHotelsReport();
                        obj.Hotelid = item.HotelID.Value;
                        obj.Hotelname = item.HotelName;
                        obj.Country = item.country;
                        obj.City = item.city;

                        if (item.createdate != null)
                        {
                            obj.Createdate = item.createdate.Value.ToString("dd/MM/yyyy");
                        }
                        else { obj.Createdate = ""; }
                        obj.Createdby = item.createby;
                        objLst.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return objLst;
        }

        #endregion

        #region velocity Dashboard
        public List<DataContracts.Mapping.DC_VelocityMappingStats> GetVelocityDashboard(DataContracts.Mapping.DC_VelocityReport parm)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<DC_AllSupplierMappedData> searchResult = new List<DC_AllSupplierMappedData>();

                    context.Database.CommandTimeout = 0;
                    List<DataContracts.Mapping.DC_VelocityMappingStats> returnObj = new List<DataContracts.Mapping.DC_VelocityMappingStats>();
                    DataContracts.Mapping.DC_VelocityMappingStats newmapstats = new DataContracts.Mapping.DC_VelocityMappingStats();
                    List<DataContracts.Mapping.DC_VelocityMappingStatsFor> newmapstatsforList = new List<DataContracts.Mapping.DC_VelocityMappingStatsFor>();


                    newmapstats.SupplierId = parm.SupplierID;
                    var unmapData = (from s in context.Dashboard_MappingStat.AsQueryable()
                         .Where(cat => (cat.Batch == 1) && (cat.Status == "UNMAPPED" || cat.Status == "REVIEW"))
                                     select s);

                    var search = (from p in context.Dashboard_UserwiseMappedStat.AsQueryable()
                                  where p.SupplierName != null && p.Batch == 1 && (p.EditDate >= parm.Fromdate && p.EditDate <= parm.ToDate)
                                  group p by new { p.Supplier_id, p.Username, p.MappingFor, p.Sequence, p.Status, p.SupplierName } into g
                                  select new DC_AllSupplierMappedData
                                  {
                                      supplierid = g.Key.Supplier_id,
                                      SupplierName = g.Key.SupplierName,
                                      Username = g.Key.Username,
                                      totalcount = (g.Sum(x => x.Totalcount) ?? 0),
                                      MappinFor = g.Key.MappingFor,
                                      Sequence = g.Key.Sequence ?? 0,
                                      Status = g.Key.Status
                                  });
                    if (parm.SupplierID != Guid.Empty)
                    {
                        searchResult.AddRange(search.Where(w => w.supplierid == parm.SupplierID).ToList());
                        unmapData = (from s in unmapData
                                     where s.supplier_id == parm.SupplierID
                                     select s);
                    }
                    else if (parm.SupplierID == Guid.Empty)
                    {
                        if (parm.Priority > 0)
                        {
                            searchResult.AddRange((from t1 in search.ToList()
                                                   join t2 in context.Supplier on t1.supplierid equals t2.Supplier_Id
                                                   where t2.Priority == parm.Priority
                                                   group t1 by new { t1.Username, t1.MappinFor, t1.Sequence, t1.Status, } into g
                                                   select new DC_AllSupplierMappedData
                                                   {
                                                       supplierid = Guid.Empty,
                                                       SupplierName = "ALL",
                                                       Username = g.Key.Username,
                                                       totalcount = (g.Sum(x => x.totalcount) ?? 0),
                                                       MappinFor = g.Key.MappinFor,
                                                       Sequence = g.Key.Sequence,
                                                       Status = g.Key.Status
                                                   }).ToList());
                            unmapData = (from t in unmapData
                                         join t2 in context.Supplier on t.supplier_id equals t2.Supplier_Id
                                         where t2.Priority == parm.Priority
                                         select t);
                        }
                        else if (parm.Priority == 0)
                        {
                            searchResult.AddRange(search.Where(w => w.SupplierName == "ALL").ToList());
                            unmapData = (from t in unmapData
                                         where t.supplier_id == Guid.Empty
                                         select t);
                        }
                    }
                    var res_supplierName = (from s in searchResult where s.supplierid == parm.SupplierID select s).FirstOrDefault();
                    if (res_supplierName != null)
                        newmapstats.SupplierName = res_supplierName.SupplierName;

                    var MapForList = "City,Country,Product,Activity,HotelRoom".Split(','); //(from s in search select s.MappinFor).ToList().Distinct();

                    foreach (var mapfor in MapForList)
                    {
                        DataContracts.Mapping.DC_VelocityMappingStatsFor newmapstatsfor = new DataContracts.Mapping.DC_VelocityMappingStatsFor();

                        newmapstatsfor.MappingFor = mapfor;
                        var unMappedDataCount = (from s in unmapData
                                                 where s.MappingFor == mapfor
                                                 select s.totalcount).Sum();

                        var totalmappeddata = (from s in searchResult
                                               where s.MappinFor == mapfor && s.Username == "Total" && s.Status == "Total"
                                               select s.totalcount).FirstOrDefault();

                        if (unMappedDataCount != null)
                        {
                            newmapstatsfor.Unmappeddata = unMappedDataCount.Value;
                            if (unMappedDataCount.Value > 0 && totalmappeddata > 0)
                            {
                                double days;
                                if (parm.Fromdate == parm.ToDate)
                                    days = 1;
                                else
                                    days = (Convert.ToDateTime(parm.ToDate) - Convert.ToDateTime(parm.Fromdate)).TotalDays;
                                var perday = (totalmappeddata / days);
                                newmapstatsfor.Estimate = Convert.ToInt32(unMappedDataCount.Value / perday);
                            }
                            else
                            {
                                newmapstatsfor.Estimate = 0;
                            }
                        }
                        else
                        {
                            newmapstatsfor.Unmappeddata = 0;
                            newmapstatsfor.Estimate = 0;
                        }
                        if (totalmappeddata > 0)
                        {
                            newmapstatsfor.MappingData = (from s in searchResult
                                                          where s.MappinFor == mapfor
                                                          orderby s.MappinFor, s.Sequence
                                                          select new DataContracts.Mapping.DC_VelocityMappingdata { Username = s.Username, Sequence = s.Sequence, Totalcount = (s.totalcount ?? 0) }).ToList();
                        }

                        newmapstatsforList.Add(newmapstatsfor);
                    }

                    newmapstats.MappingStatsFor = newmapstatsforList;
                    returnObj.Add(newmapstats);
                    return returnObj;
                }

            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching  Velocity mapping statistics", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region
        public List<DC_HotelListByCityCode> GetHotelListByCityCode(DataContracts.Mapping.DC_HotelListByCityCode_RQ param)
        {
            try
            {
                List<DC_HotelListByCityCode> _lstresult = new List<DC_HotelListByCityCode>();

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (!string.IsNullOrWhiteSpace(param.CityMapping_Id))
                    {
                        string strGoFor = string.IsNullOrWhiteSpace(param.GoFor) ? string.Empty : param.GoFor.Trim().ToUpper();
                        IQueryable<Accommodation_ProductMapping> query;
                        //context.Accommodation_ProductMapping.Where(w => false).Select(s => s).AsQueryable();

                        Guid _CityMapId = Guid.Parse(param.CityMapping_Id);
                        var selectedcity = context.m_CityMapping.Find(_CityMapId);
                        if (selectedcity != null)
                        {
                            var supplierid = selectedcity.Supplier_Id;
                            var suppliercitycode = (selectedcity.CityCode ?? string.Empty).Trim().ToLower();
                            var suppliercityname = (selectedcity.CityName ?? string.Empty).Trim().ToLower();
                            var supplierCountryname = (selectedcity.CountryName ?? string.Empty).Trim().ToLower();
                            var supplierCountryCode = (selectedcity.CountryCode ?? string.Empty).Trim().ToLower();

                            query = context.Accommodation_ProductMapping.Where(w => w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                            if (strGoFor == "CITYCODE")
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercitycode))
                                {
                                    //query = context.Accommodation_ProductMapping.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode && w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                                    query = query.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode).Select(s => s).AsQueryable();
                                }

                            }
                            else if (strGoFor == "CITYNAME")
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercityname))
                                {
                                    //query = context.Accommodation_ProductMapping.Where(w => w.CityName.Trim().ToLower() == suppliercityname && w.Supplier_Id == supplierid).Select(s => s).AsQueryable();
                                    query = query.Where(w => w.CityName.Trim().ToLower() == suppliercityname).Select(s => s).AsQueryable();
                                    if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                    {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                }

                            }
                            else if (strGoFor == string.Empty)
                            {
                                if (!string.IsNullOrWhiteSpace(suppliercitycode))
                                {
                                    query = query.Where(w => w.CityCode.Trim().ToLower() == suppliercitycode).Select(s => s).AsQueryable();
                                    //&& w.Supplier_Id == supplierid

                                }
                                else if (!string.IsNullOrWhiteSpace(suppliercityname))
                                {
                                    query = query.Where(w => w.CityName.Trim().ToLower() == suppliercityname).Select(s => s).AsQueryable();
                                    //&& w.Supplier_Id == supplierid
                                    if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                }
                            }


                            int total = query.Count();

                            //Supplier Country Filter check
                            /*if (total > 0)
                            {
                                if (!string.IsNullOrWhiteSpace(supplierCountryCode))
                                {
                                    if (query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s).Count() > 0)
                                    {
                                        query = query.Where(w => w.CountryCode.Trim().ToLower() == supplierCountryCode).Select(s => s);
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                        {
                                            if (query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s).Count() > 0)
                                            {
                                                query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                            }
                                            else
                                            {
                                                return _lstresult;
                                            }
                                        }
                                        else
                                        {
                                            return _lstresult;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrWhiteSpace(supplierCountryname))
                                {
                                    if (query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s).Count() > 0)
                                    {
                                        query = query.Where(w => w.CountryName.Trim().ToLower() == supplierCountryname).Select(s => s);
                                    }
                                    else
                                    {
                                        return _lstresult;
                                    }
                                }
                                else
                                {
                                    return _lstresult;
                                }
                            }

                            total = query.Count();*/

                            if (total > 0)
                            {
                                var skip = param.PageSize * param.PageNo;

                                _lstresult = (from CM in query
                                              orderby CM.address_tx.Length descending
                                              select new DC_HotelListByCityCode
                                    {
                                                  HotelName = CM.ProductName,
                                                  Address = (CM.address ?? string.Empty)
                                                            + "," + (CM.CityName ?? string.Empty) + "(" + (CM.CityCode ?? string.Empty) + ")"
                                                            + "," + (CM.StateName ?? string.Empty) + "(" + (CM.StateCode ?? string.Empty) + ")"
                                                            + "," + (CM.CountryName ?? string.Empty) + "(" + (CM.CountryCode ?? string.Empty) + ")",
                                        CityMapping_Id = param.CityMapping_Id,
                                        GoFor = strGoFor,
                                        TotalRecords = total
                                              }).Skip(skip).Take(param.PageSize).ToList();

                                //var res = (from CM in query
                                //           orderby CM.Street.Length descending
                                //           select CM
                                //       ).Skip(skip).Take(param.PageSize).ToList();

                                //foreach (var item in res)
                                //{
                                //    StringBuilder sb = new StringBuilder();
                                //    sb.Append(item.Street);
                                //    sb.Append(", ");
                                //    sb.Append(item.Street2);
                                //    sb.Append(", ");
                                //    sb.Append(item.Street3);
                                //    sb.Append(", ");
                                //    sb.Append(item.CityName);
                                //    sb.Append(", ");
                                //    sb.Append(item.StateName);
                                //    sb.Append(", ");
                                //    sb.Append(item.StateCode);
                                //    sb.Append(", ");
                                //    sb.Append(item.CountryName);
                                //    sb.Append(", ");
                                //    sb.Append(item.CountryCode);
                                //    sb.Append(", ");
                                //    sb.Append(item.PostCode);
                                //    _lstresult.Add(new DC_HotelListByCityCode
                                //    {
                                //        HotelName = item.ProductName,
                                //        Address = Convert.ToString(sb),
                                //        CityMapping_Id = param.CityMapping_Id,
                                //        GoFor = strGoFor,
                                //        TotalRecords = total
                                //    });
                                //    sb.Clear();

                                //}

                                return _lstresult;
                            }
                            else
                            {
                                return _lstresult;
                            }
                        }
                        else
                        {
                            return _lstresult;
                        }
                    }
                    else
                    {
                        return _lstresult;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
