﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using DataContracts.Mapping;
using DataContracts.UploadStaticData;
using System.Runtime.Serialization;
using DataContracts.Masters;
using DataContracts;
using System.Text.RegularExpressions;
using System.Configuration;

namespace DataLayer
{
    public class DL_Activity : IDisposable
    {
        public void Dispose()
        {

        }

        #region "Activity Search"
        public List<DataContracts.Masters.DC_ActivitySearch_RS> ActivitySearch(DataContracts.Masters.DC_Activity_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activities
                                 select a;

                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }

                    if (RQ.Country != null)
                    {
                        search = from a in search
                                 where a.Country.Trim().TrimStart().ToUpper() == RQ.Country.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.City != null)
                    {
                        search = from a in search
                                 where a.City.Trim().TrimStart().ToUpper() == RQ.City.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Name != null)
                    {
                        search = from a in search
                                 where a.Product_Name.Trim().TrimStart().ToUpper() == RQ.Name.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategory != null)
                    {
                        search = from a in search
                                 where a.ProductCategory.Trim().TrimStart().ToUpper() == RQ.ProductCategory.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductCategorySubType != null)
                    {
                        search = from a in search
                                 where a.ProductCategorySubType.Trim().TrimStart().ToUpper() == RQ.ProductCategorySubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductType != null)
                    {
                        search = from a in search
                                 where a.ProductType.Trim().TrimStart().ToUpper() == RQ.ProductType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ProductNameSubType != null)
                    {
                        search = from a in search
                                 join at in context.Activity_Flavour on a.Activity_Id equals at.Activity_Id
                                 where at.ProductNameSubType.Trim().TrimStart().ToUpper() == RQ.ProductNameSubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Status != null)
                    {
                        bool IsActive = false;
                        if (RQ.Status.ToString().Trim().TrimStart().ToUpper() == "ACTIVE")
                            IsActive = true;

                        search = from a in search
                                 where (a.IsActive ?? false) == IsActive
                                 select a;
                    }

                    //For Keyword
                    if (!string.IsNullOrWhiteSpace(RQ.Keyword))
                    {
                        search = from a in search
                                 join b in context.Activity_Content on a.Activity_Id equals b.Activity_Id
                                 where a.Product_Name.Contains(RQ.Keyword) || b.Content_Text.Contains(RQ.Keyword)
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.Product_Name
                                 join at in context.Activity_Flavour on a.Activity_Id equals at.Activity_Id into atl
                                 from atld in atl.DefaultIfEmpty()
                                 select new DataContracts.Masters.DC_ActivitySearch_RS
                                 {
                                     Activity_Id = a.Activity_Id,
                                     CommonProductID = a.CommonProductID,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     Product_Name = a.Product_Name,
                                     Display_Name = a.Display_Name,
                                     Country = a.Country,
                                     City = a.City,
                                     ProductType = a.ProductType,
                                     ProductCategory = a.ProductCategory,
                                     ProductCategorySubType = a.ProductCategorySubType,
                                     Create_Date = a.Create_Date,
                                     Edit_Date = a.Edit_Date,
                                     Create_User = a.Create_User,
                                     Edit_User = a.Edit_User,
                                     IsActive = a.IsActive,
                                     CompanyRecommended = a.CompanyRecommended,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     TourType = a.TourType,
                                     Parent_Legacy_Id = a.Parent_Legacy_Id,
                                     TotalRecord = total,
                                     Affiliation = a.Affiliation,
                                     Country_Id = a.Country_Id,
                                     City_Id = a.City_Id,
                                     CompanyProductID = a.CompanyProductID,
                                     CompanyRating = a.CompanyRating,
                                     Mode_Of_Transport = a.Mode_Of_Transport,
                                     ProductRating = a.ProductRating,
                                     Remarks = a.Remarks,
                                     Activity_Flavour_Id = atld.Activity_Flavour_Id,
                                     CommonProductNameSubType_Id = atld.CommonProductNameSubType_Id,
                                     ProductNameSubType = atld.ProductNameSubType
                                 };
                    return result.OrderBy(p => p.Product_Name).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }

            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Attr Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region "Activity Insert and Update"
        public DC_Message AddUpdateProductInfo(DataContracts.Masters.DC_Activity _objAct)
        {
            DC_Message _msg = new DC_Message();

            using (ConsumerEntities context = new ConsumerEntities())
            {
                if (_objAct.Activity_Id.HasValue) //&& _objAct.Activity_Id == Guid.Empty)|| _objAct.Activity_Id != Guid.Empty
                {
                    var results = context.Activities.Find(_objAct.Activity_Id);

                    if (results != null)
                    {
                        results.Country_Id = _objAct.Country_Id;
                        results.Country = _objAct.Country;
                        results.City_Id = _objAct.City_Id;
                        results.City = _objAct.City;
                        results.Edit_Date = DateTime.Now;
                        results.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                        results.ProductCategory = _objAct.ProductCategory;
                        results.ProductCategorySubType = _objAct.ProductCategorySubType;
                        results.ProductType = _objAct.ProductType;
                        results.Product_Name = _objAct.Product_Name;

                        results.Affiliation = _objAct.Affiliation;
                        results.CommonProductID = _objAct.CommonProductID;
                        results.CompanyProductID = _objAct.CompanyProductID;
                        results.FinanceProductID = _objAct.FinanceProductID;
                        results.CompanyRating = _objAct.CompanyRating;
                        results.CompanyRecommended = _objAct.CompanyRecommended;
                        results.Display_Name = _objAct.Display_Name;
                        results.IsActive = _objAct.IsActive;
                        results.Latitude = _objAct.Latitude;
                        results.Legacy_Product_ID = _objAct.Legacy_Product_ID;
                        results.Longitude = _objAct.Longitude;
                        results.Mode_Of_Transport = _objAct.Mode_Of_Transport;
                        results.Parent_Legacy_Id = _objAct.Parent_Legacy_Id;
                        results.Remarks = _objAct.Remarks;
                        results.TourType = _objAct.TourType;

                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }

                        return _msg;

                    }
                    else
                    {
                        Activity _obj = new Activity()
                        {
                            Activity_Id = Guid.NewGuid(),
                            Affiliation = _objAct.Affiliation,
                            Country = _objAct.Country,
                            City = _objAct.City,
                            City_Id = _objAct.City_Id,
                            Country_Id = _objAct.Country_Id,
                            Create_Date = DateTime.Now,
                            Create_User = System.Web.HttpContext.Current.User.Identity.Name,
                            CommonProductID = _objAct.CommonProductID,
                            CompanyProductID = _objAct.CompanyProductID,
                            CompanyRating = _objAct.CompanyRating,
                            CompanyRecommended = _objAct.CompanyRecommended,
                            Display_Name = _objAct.Display_Name,
                            IsActive = _objAct.IsActive,
                            Latitude = _objAct.Latitude,
                            Longitude = _objAct.Longitude,
                            Legacy_Product_ID = _objAct.Legacy_Product_ID,
                            Mode_Of_Transport = _objAct.Mode_Of_Transport,
                            Product_Name = _objAct.Product_Name,
                            ProductCategory = _objAct.ProductCategory,
                            ProductCategorySubType = _objAct.ProductCategorySubType,
                            ProductType = _objAct.ProductType,
                            ProductRating = _objAct.ProductRating,
                            Parent_Legacy_Id = _objAct.Parent_Legacy_Id,
                            Remarks = _objAct.Remarks,
                            TourType = _objAct.TourType
                        };

                        context.Activities.Add(_obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                }
            }

            return _msg;
        }

        public DataContracts.DC_Message AddUpdateActivity(DataContracts.Masters.DC_Activity _objAct)
        {
            DC_Message _msg = new DC_Message();

            using (ConsumerEntities context = new ConsumerEntities())
            {
                var isduplicate = (from a in context.Activities
                                   where a.Activity_Id != (_objAct.Activity_Id ?? Guid.Empty) && a.Product_Name == _objAct.Product_Name
                                   select a).Count() == 0 ? false : true;

                if (isduplicate)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }

                if (_objAct.Activity_Id.HasValue && _objAct.Activity_Id != Guid.Empty)
                {
                    var results = context.Activities.Find(_objAct.Activity_Id);

                    if (results != null)
                    {
                        results.Country_Id = _objAct.Country_Id;
                        results.Country = _objAct.Country;
                        results.City_Id = _objAct.City_Id;
                        results.City = _objAct.City;
                        results.Edit_Date = _objAct.Edit_Date;
                        results.Edit_User = _objAct.Edit_User;
                        results.ProductCategory = _objAct.ProductCategory;
                        results.ProductCategorySubType = _objAct.ProductCategorySubType;
                        results.ProductType = _objAct.ProductType;
                        results.Product_Name = _objAct.Product_Name;

                        results.Affiliation = _objAct.Affiliation;
                        results.CommonProductID = _objAct.CommonProductID;
                        results.CompanyProductID = _objAct.CompanyProductID;
                        results.CompanyRating = _objAct.CompanyRating;
                        results.CompanyRecommended = _objAct.CompanyRecommended;
                        results.Display_Name = _objAct.Display_Name;
                        results.IsActive = _objAct.IsActive;
                        results.Latitude = _objAct.Latitude;
                        results.Legacy_Product_ID = _objAct.Legacy_Product_ID;
                        results.Longitude = _objAct.Longitude;
                        results.Mode_Of_Transport = _objAct.Mode_Of_Transport;
                        results.Parent_Legacy_Id = _objAct.Parent_Legacy_Id;
                        results.Remarks = _objAct.Remarks;
                        results.TourType = _objAct.TourType;

                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }

                        return _msg;

                    }
                    else
                    {
                        Activity _obj = new Activity()
                        {
                            Activity_Id = Guid.NewGuid(),
                            Affiliation = _objAct.Affiliation,
                            Country = _objAct.Country,
                            City = _objAct.City,
                            City_Id = _objAct.City_Id,
                            Country_Id = _objAct.Country_Id,
                            Create_Date = _objAct.Create_Date,
                            Create_User = _objAct.Create_User,
                            CommonProductID = _objAct.CommonProductID,
                            CompanyProductID = _objAct.CompanyProductID,
                            CompanyRating = _objAct.CompanyRating,
                            CompanyRecommended = _objAct.CompanyRecommended,
                            Display_Name = _objAct.Display_Name,
                            IsActive = _objAct.IsActive,
                            Latitude = _objAct.Latitude,
                            Longitude = _objAct.Longitude,
                            Legacy_Product_ID = _objAct.Legacy_Product_ID,
                            Mode_Of_Transport = _objAct.Mode_Of_Transport,
                            Product_Name = _objAct.Product_Name,
                            ProductCategory = _objAct.ProductCategory,
                            ProductCategorySubType = _objAct.ProductCategorySubType,
                            ProductType = _objAct.ProductType,
                            ProductRating = _objAct.ProductRating,
                            Parent_Legacy_Id = _objAct.Parent_Legacy_Id,
                            Remarks = _objAct.Remarks,
                            TourType = _objAct.TourType
                        };

                        context.Activities.Add(_obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                }
            }

            return _msg;
        }
        #endregion

        #region "Activity Contacts"
        public List<DC_Activity_Contact> GetActivityContacts(Guid Activity_Flavour_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ac in context.Activity_Contact
                                 where ac.Activity_Flavour_Id == Activity_Flavour_Id
                                 && ac.Activity_Contact_Id == (DataKey_Id == Guid.Empty ? ac.Activity_Contact_Id : DataKey_Id)
                                 select new DC_Activity_Contact
                                 {
                                     Activity_Contact_Id = ac.Activity_Contact_Id,
                                     Activity_Id = ac.Activity_Id,
                                     Activity_Flavour_Id = ac.Activity_Flavour_Id,
                                     Create_Date = ac.Create_Date,
                                     Create_User = ac.Create_User,
                                     Edit_Date = ac.Edit_Date,
                                     Edit_User = ac.Edit_User,
                                     Email = ac.Email,
                                     Fax = ac.Fax,
                                     Legacy_Product_ID = ac.Legacy_Product_ID,
                                     Telephone = ac.Telephone,
                                     WebSiteURL = ac.WebSiteURL,
                                     IsActive = (ac.IsActive ?? true)
                                 };
                    return search.ToList();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateActivityContacts(DC_Activity_Contact AC)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ac in context.Activity_Contact
                                  where ac.Activity_Contact_Id == AC.Activity_Contact_Id
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
                            search.Legacy_Product_ID = AC.Legacy_Product_ID;
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
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating activity contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddActivityContacts(DC_Activity_Contact AC)
        {
            try
            {
                //if (AC.Activity_Contact_Id == null)
                //{
                //    return false;
                //}

                if (AC.Activity_Contact_Id == null)
                {
                    AC.Activity_Contact_Id = Guid.NewGuid();
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Activity_Contact newObj = new Activity_Contact();
                    newObj.Activity_Contact_Id = AC.Activity_Contact_Id;
                    newObj.Activity_Id = AC.Activity_Id;
                    newObj.Activity_Flavour_Id = AC.Activity_Flavour_Id;
                    newObj.Create_Date = AC.Create_Date;
                    newObj.Create_User = AC.Create_User;
                    newObj.Email = AC.Email;
                    newObj.Fax = AC.Fax;
                    newObj.Legacy_Product_ID = AC.Legacy_Product_ID;
                    newObj.Telephone = AC.Telephone;
                    newObj.WebSiteURL = AC.WebSiteURL;
                    newObj.IsActive = AC.IsActive;
                    context.Activity_Contact.Add(newObj);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public int GetLegacyProductId(Guid Activity_Flavour_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ac in context.Activity_Contact
                                  where ac.Activity_Flavour_Id == Activity_Flavour_Id
                                  select new { ac.Legacy_Product_ID }).FirstOrDefault();

                    return Convert.ToInt32(search);

                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Legacy Id", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region "Activity Status"
        public List<DC_Activity_Status> GetActivityStatus(Guid Activity_Flavour_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ast in context.Activity_Status
                                 where ast.Activity_Flavour_Id == Activity_Flavour_Id
                                 && ast.Activity_Status_Id == (DataKey_Id == Guid.Empty ? ast.Activity_Status_Id : DataKey_Id)
                                 select new DC_Activity_Status
                                 {
                                     Activity_Id = ast.Activity_Id,
                                     Activity_Status_Id = ast.Activity_Status_Id,
                                     Activity_Flavour_Id = ast.Activity_Flavour_Id,
                                     Legacy_Product_ID = ast.Legacy_Product_ID,
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
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching activity status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateActivityStatus(DC_Activity_Status AS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Activity_Status
                                  where a.Activity_Status_Id == AS.Activity_Status_Id
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
                            search.Activity_Id = AS.Activity_Id;
                            search.CompanyMarket = AS.CompanyMarket;
                            search.DeactivationReason = AS.DeactivationReason;
                            search.Legacy_Product_ID = AS.Legacy_Product_ID;
                            search.Activity_Flavour_Id = AS.Activity_Flavour_Id;
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
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating activity status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool AddActivityStatus(DC_Activity_Status AS)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //if (AS.Activity_Id == null)
                    //{
                    //    return false;
                    //}

                    Activity_Status objNew = new Activity_Status();

                    if (AS.Activity_Status_Id == null)
                    {
                        AS.Activity_Status_Id = Guid.NewGuid();
                    }

                    objNew.Activity_Status_Id = AS.Activity_Status_Id;
                    objNew.Activity_Id = AS.Activity_Id;
                    objNew.Activity_Flavour_Id = AS.Activity_Flavour_Id;
                    objNew.CompanyMarket = AS.CompanyMarket;
                    objNew.DeactivationReason = AS.DeactivationReason;
                    objNew.From = AS.From;
                    objNew.Status = AS.Status;
                    objNew.To = AS.To;
                    objNew.IsActive = AS.IsActive;
                    objNew.Create_Date = AS.Create_Date;
                    objNew.Create_User = AS.Create_User;

                    context.Activity_Status.Add(objNew);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding activity status", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Media
        public List<DC_Activity_Media> GetActivityMedia(DC_Activity_Media_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Media
                                 select a;

                    if (RQ.Activity_Media_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Media_Id == RQ.Activity_Media_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.MediaType != null)
                    {
                        search = from a in search
                                 where a.MediaType.Trim().TrimStart().ToUpper() == RQ.MediaType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.MediaName != null)
                    {
                        search = from a in search
                                 where a.MediaName.Trim().TrimStart().ToUpper() == RQ.MediaName.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Category != null)
                    {
                        search = from a in search
                                 where a.Category.Trim().TrimStart().ToUpper() == RQ.Category.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.SubCategory != null)
                    {
                        search = from a in search
                                 where a.SubCategory.Trim().TrimStart().ToUpper() == RQ.SubCategory.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.ValidFrom != null)
                    {
                        search = from a in search
                                 where a.ValidFrom == RQ.ValidFrom
                                 select a;
                    }

                    if (RQ.ValidTo != null)
                    {
                        search = from a in search
                                 where a.ValidTo == RQ.ValidTo
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 join b in context.Activity_Flavour on a.Activity_Flavour_Id equals b.Activity_Flavour_Id
                                 orderby a.MediaName
                                 select new DataContracts.Masters.DC_Activity_Media
                                 {
                                     Activity_Id = a.Activity_Id,
                                     Activity_Media_Id = a.Activity_Media_Id,
                                     IsActive = a.IsActive,
                                     MediaName = a.MediaName,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     MediaType = a.MediaType,
                                     Category = a.Category,
                                     SubCategory = a.SubCategory,
                                     Description = a.Description,
                                     FileFormat = a.FileFormat,
                                     Legacy_Product_Id = a.Legacy_Product_Id,
                                     MediaFileMaster = a.MediaFileMaster,
                                     MediaID = a.MediaID.ToString(),
                                     Media_Path = a.Media_Path,
                                     Media_Position = a.Media_Position,
                                     Media_URL = a.Media_URL,
                                     RoomCategory = a.RoomCategory,
                                     ValidFrom = a.ValidFrom,
                                     ValidTo = a.ValidTo,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Media_Caption = a.Media_Caption,
                                     Media_Height = a.Media_Height,
                                     Media_Width = a.Media_Width,
                                     CommonProductNameSubType_Id=b.CommonProductNameSubType_Id,
                                     IsWaterMark=a.IsWaterMark,
                                     IsRelevent=a.IsRelevent,
                                     IsDuplicate=a.IsDuplicate,
                                     Media_Feedback=a.Media_Feedback,
                                     TotalRecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Media", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DC_Message AddUpdateActivityMedia(DC_Activity_Media RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_Media_Id != null)
                    {
                        var res = context.Activity_Media.Find(RQ.Activity_Media_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_Media_Id = RQ.Activity_Media_Id ?? Guid.Empty;
                                res.Activity_Id = RQ.Activity_Id;
                                res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                                res.FileFormat = RQ.FileFormat;
                                res.Media_URL = RQ.Media_URL;
                                res.Media_Position = RQ.Media_Position;
                                res.SubCategory = RQ.SubCategory;
                                res.MediaFileMaster = RQ.MediaFileMaster;
                                res.Description = RQ.Description;
                                res.MediaID = Convert.ToInt32(RQ.MediaID);
                                res.MediaType = RQ.MediaType;
                                res.RoomCategory = RQ.RoomCategory;
                                res.MediaName = RQ.MediaName;
                                res.Media_Path = RQ.Media_Path;
                                res.Category = RQ.Category;
                                res.ValidFrom = RQ.ValidFrom;
                                res.ValidTo = RQ.ValidTo;
                                res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                                res.Media_Caption = RQ.Media_Caption;
                                res.Media_Height = RQ.Media_Height;
                                res.Media_Width = RQ.Media_Width;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;
                    if (IsInsert)
                    {
                        Activity_Media newmed = new Activity_Media();
                        newmed.Activity_Media_Id = RQ.Activity_Media_Id ?? Guid.NewGuid();
                        newmed.Activity_Id = RQ.Activity_Id;
                        newmed.Legacy_Product_Id = RQ.Legacy_Product_Id;
                        newmed.IsActive = RQ.IsActive;
                        newmed.FileFormat = RQ.FileFormat;
                        newmed.Media_URL = RQ.Media_URL;
                        newmed.Media_Position = RQ.Media_Position;
                        newmed.SubCategory = RQ.SubCategory;
                        newmed.MediaFileMaster = RQ.MediaFileMaster;
                        newmed.Description = RQ.Description;
                        newmed.MediaID = Convert.ToInt32(RQ.MediaID);
                        newmed.MediaType = RQ.MediaType;
                        newmed.RoomCategory = RQ.RoomCategory;
                        newmed.MediaName = RQ.MediaName;
                        newmed.Media_Path = RQ.Media_Path;
                        newmed.Category = RQ.Category;
                        newmed.ValidFrom = RQ.ValidFrom;
                        newmed.ValidTo = RQ.ValidTo;
                        newmed.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        newmed.Media_Caption = RQ.Media_Caption;
                        newmed.Media_Height = RQ.Media_Height;
                        newmed.Media_Width = RQ.Media_Width;
                        newmed.Create_Date = DateTime.Now;
                        newmed.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_Media.Add(newmed);
                        //context.SaveChanges();
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity media", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message AddUpdateActivityMediaReview(DC_Activity_MediaReview RQ)
        {           
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_Media_Id != null)
                    {
                        var res = context.Activity_Media.Find(RQ.Activity_Media_Id);
                        if (res != null)
                        {
                            res.IsWaterMark = RQ.IsWaterMark;
                            res.IsRelevent = RQ.IsRelevent;
                            res.IsDuplicate = RQ.IsDuplicate;
                            res.Media_Feedback = RQ.Media_ReviewFeedback;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                        }

                    }
                }
                      
            
                return _msg;
            }
            
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity media", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region  Activity Inclusions
        public List<DC_Activity_Inclusions> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Inclusions
                                 select a;

                    if (RQ.Activity_Inclusions_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Inclusions_Id == RQ.Activity_Inclusions_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_Id != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_Id == RQ.Legacy_Product_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.InclusionFrom != null)
                    {
                        search = from a in search
                                 where a.InclusionFrom == RQ.InclusionFrom
                                 select a;
                    }
                    if (RQ.InclusionTo != null)
                    {
                        search = from a in search
                                 where a.InclusionTo == RQ.InclusionTo
                                 select a;
                    }
                    if (RQ.InclusionType != null)
                    {
                        search = from a in search
                                 where a.InclusionType.Trim().TrimStart().ToUpper() == RQ.InclusionType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.InclusionName != null)
                    {
                        search = from a in search
                                 where a.InclusionName.Trim().TrimStart().ToUpper() == RQ.InclusionName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var searchresult = from s in search
                                       orderby s.InclusionName
                                       select new DataContracts.Masters.DC_Activity_Inclusions
                                       {
                                           Activity_Inclusions_Id = s.Activity_Inclusions_Id,
                                           InclusionFrom = s.InclusionFrom,
                                           InclusionTo = s.InclusionTo,
                                           InclusionFor = s.InclusionFor,
                                           InclusionName = s.InclusionName,
                                           InclusionDescription = s.InclusionDescription,
                                           InclusionType = s.InclusionType,
                                           IsAudioCommentary = s.IsAudioCommentary,
                                           IsDriver = s.IsDriver,
                                           IsInclusion = s.IsInclusion,
                                           Legacy_Product_Id = s.Legacy_Product_Id,
                                           Activity_Flavour_Id = s.Activity_Flavour_Id,
                                           Create_Date = s.Create_Date,
                                           RestaurantStyle = s.RestaurantStyle,
                                           TotalRecords = total,
                                           Activity_Id = s.Activity_Id,
                                           IsActive = s.IsActive
                                       };
                    return searchresult.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Inclusions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityInclusions(DataContracts.Masters.DC_Activity_Inclusions RQ)
        {
            bool isinsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_Inclusions_Id != null)
                    {
                        var res = context.Activity_Inclusions.Find(RQ.Activity_Inclusions_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? false))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                //res.Activity_Inclusions_Id = RQ.Activity_Inclusions_Id ?? Guid.Empty;
                                //res.Activity_Id = RQ.Activity_Id;
                                //res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                                //res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                                res.IsInclusion = RQ.IsInclusion;
                                res.InclusionName = RQ.InclusionName;
                                res.InclusionDescription = RQ.InclusionDescription;
                                res.InclusionFor = RQ.InclusionFor;
                                res.InclusionFrom = RQ.InclusionFrom;
                                res.InclusionTo = RQ.InclusionTo;
                                res.InclusionType = RQ.InclusionType;
                                //res.IsDriver = RQ.IsDriver;
                                //res.IsAudioCommentary = RQ.IsAudioCommentary;
                                //res.RestaurantStyle = RQ.RestaurantStyle;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }

                        }
                        else isinsert = true;
                    }
                    else isinsert = true;

                    if (isinsert)
                    {
                        DataLayer.Activity_Inclusions obj = new DataLayer.Activity_Inclusions();
                        obj.Activity_Inclusions_Id = RQ.Activity_Inclusions_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_Id = RQ.Legacy_Product_Id;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.IsInclusion = RQ.IsInclusion;
                        obj.InclusionName = RQ.InclusionName;
                        obj.InclusionDescription = RQ.InclusionDescription;
                        obj.InclusionFor = RQ.InclusionFor;
                        obj.InclusionFrom = RQ.InclusionFrom;
                        obj.InclusionTo = RQ.InclusionTo;
                        obj.InclusionType = RQ.InclusionType;
                        obj.IsDriver = RQ.IsDriver;
                        obj.IsAudioCommentary = RQ.IsAudioCommentary;
                        obj.IsActive = RQ.IsActive;
                        obj.RestaurantStyle = RQ.RestaurantStyle;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = RQ.Create_User;

                        context.Activity_Inclusions.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception EX)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity inclusions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Inclusion Details
        public List<DC_Activity_InclusionsDetails> GetActivityInclusionDetails(DC_Activity_InclusionDetails_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_InclusionDetails
                                 select a;

                    if (RQ.Activity_InclusionDetails_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_InclusionDetails_Id == RQ.Activity_InclusionDetails_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_Id != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_Id == RQ.Legacy_Product_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.InclusionDetailFrom != null)
                    {
                        search = from a in search
                                 where a.InclusionDetailFrom == RQ.InclusionDetailFrom
                                 select a;
                    }
                    if (RQ.InclusionDetailTo != null)
                    {
                        search = from a in search
                                 where a.InclusionDetailTo == RQ.InclusionDetailTo
                                 select a;
                    }
                    if (RQ.InclusionDetailType != null)
                    {
                        search = from a in search
                                 where a.InclusionDetailType.Trim().TrimStart().ToUpper() == RQ.InclusionDetailType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.InclusionDetailName != null)
                    {
                        search = from a in search
                                 where a.InclusionDetailName.Trim().TrimStart().ToUpper() == RQ.InclusionDetailName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.GuideLanguage != null)
                    {
                        search = from a in search
                                 where a.GuideLanguage.Trim().TrimStart().ToUpper() == RQ.GuideLanguage.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var searchresult = from s in search
                                       orderby s.InclusionDetailName
                                       select new DataContracts.Masters.DC_Activity_InclusionsDetails
                                       {
                                           Activity_InclusionDetails_Id = s.Activity_Inclusions_Id,
                                           Activity_Inclusion_Id = s.Activity_InclusionDetails_Id,
                                           Activity_Id = s.Activity_Id,
                                           InclusionDetailFrom = s.InclusionDetailFrom,
                                           InclusionDetailTo = s.InclusionDetailTo,
                                           InclusionDetailFor = s.InclusionDetailFor,
                                           InclusionDetailName = s.InclusionDetailName,
                                           InclusionDetailDescription = s.InclusionDetailDescription,
                                           InclusionDetailType = s.InclusionDetailType,
                                           FromTime = s.FromTime,
                                           ToTime = s.ToTime,
                                           CreateDate = s.Create_Date,
                                           GuideLanguage = s.GuideLanguage,
                                           Activity_Flavour_Id = s.Activity_Flavour_Id,
                                           DaysOfWeek = s.DaysofWeek,
                                           Legacy_Product_Id = s.Legacy_Product_Id,
                                           GuideLanguageCode = s.GuideLanguageCode,
                                           TotalRecords = total,
                                           IsActive = s.IsActive
                                       };
                    return searchresult.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Inclusion Details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateInclusionDetails(DataContracts.Masters.DC_Activity_InclusionsDetails RQ)
        {
            bool isinsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_InclusionDetails_Id != null)
                    {
                        var res = context.Activity_InclusionDetails.Find(RQ.Activity_InclusionDetails_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.EditDate;
                                res.Edit_User = RQ.EditUser;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_InclusionDetails_Id = RQ.Activity_InclusionDetails_Id ?? Guid.Empty;
                                res.Activity_Id = RQ.Activity_Id;
                                res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                                res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                                res.InclusionDetailName = RQ.InclusionDetailName;
                                res.InclusionDetailDescription = RQ.InclusionDetailDescription;
                                res.InclusionDetailFor = RQ.InclusionDetailFor;
                                res.Activity_Inclusions_Id = RQ.Activity_Inclusion_Id;
                                res.InclusionDetailFrom = RQ.InclusionDetailFrom;
                                res.InclusionDetailTo = RQ.InclusionDetailTo;
                                res.InclusionDetailType = RQ.InclusionDetailType;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                res.DaysofWeek = RQ.DaysOfWeek;
                                res.FromTime = RQ.FromTime;
                                res.ToTime = RQ.ToTime;
                                res.GuideLanguage = RQ.GuideLanguage;
                                res.GuideLanguageCode = RQ.GuideLanguageCode;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                        }
                        else isinsert = true;
                    }
                    else isinsert = true;

                    if (isinsert)
                    {
                        DataLayer.Activity_InclusionDetails obj = new DataLayer.Activity_InclusionDetails();
                        obj.Activity_InclusionDetails_Id = RQ.Activity_InclusionDetails_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_Id = RQ.Legacy_Product_Id;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.Activity_Inclusions_Id = RQ.Activity_Inclusion_Id;
                        obj.InclusionDetailDescription = RQ.InclusionDetailDescription;
                        obj.InclusionDetailFor = RQ.InclusionDetailFor;
                        obj.InclusionDetailType = RQ.InclusionDetailType;
                        obj.InclusionDetailName = RQ.InclusionDetailName;
                        obj.InclusionDetailFrom = RQ.InclusionDetailFrom;
                        obj.InclusionDetailTo = RQ.InclusionDetailTo;
                        obj.IsActive = RQ.IsActive;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        obj.DaysofWeek = RQ.DaysOfWeek;
                        obj.FromTime = RQ.FromTime;
                        obj.ToTime = RQ.ToTime;
                        obj.GuideLanguage = RQ.GuideLanguage;
                        context.Activity_InclusionDetails.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception EX)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity inclusions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activiy Clasification Attributes
        public List<DC_Activity_ClassificationAttributes> GetActivityClasificationAttributes(DC_Activity_ClassificationAttributes_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Activity_ClassificationAttributes
                                  select a).AsQueryable();

                    if (RQ.Activity_ClassificationAttribute_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_ClassificationAttribute_Id == RQ.Activity_ClassificationAttribute_Id
                                 select a;
                    }

                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.AttributeType != null)
                    {
                        search = from a in search
                                 where a.AttributeType.Trim().TrimStart().ToUpper() == RQ.AttributeType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.AttributeValue != null)
                    {
                        search = from a in search
                                 where a.AttributeValue.Trim().TrimStart().ToUpper() == RQ.AttributeValue.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.AttributeSubType != null)
                    {
                        search = from a in search
                                 where a.AttributeSubType.Trim().TrimStart().ToUpper() == RQ.AttributeSubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.Activity_FlavourOptions_Id == null)
                    {
                        if (RQ.WithActivity_FlavourOptions_Id == null)
                        {
                            search = from a in search
                                     where a.Activity_FlavourOptions_Id == RQ.Activity_FlavourOptions_Id
                                     select a;
                        }
                    }
                    else
                    {
                        search = from a in search
                                 where a.Activity_FlavourOptions_Id == RQ.Activity_FlavourOptions_Id
                                 select a;
                    }

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.AttributeValue
                                 select new DataContracts.Masters.DC_Activity_ClassificationAttributes
                                 {
                                     Activity_ClassificationAttribute_Id = a.Activity_ClassificationAttribute_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_FlavourOptions_Id = a.Activity_FlavourOptions_Id,
                                     CreateUser = a.Create_User,
                                     EditDate = a.Edit_Date,
                                     EditUser = a.Edit_User,
                                     Activity_Id = a.Activity_Id,
                                     IsActive = a.IsActive,
                                     CreateDate = a.Create_Date,
                                     Legacy_Product_Id = a.Legacy_Product_ID,
                                     AttributeType = a.AttributeType,
                                     AttributeSubType = a.AttributeSubType,
                                     AttributeValue = a.AttributeValue,
                                     InternalOnly = a.InternalOnly,
                                     TotalRecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Classification Attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }

        public bool AddActivityClassifiationAttributes(DC_Activity_ClassificationAttributes RQ)
        {
            try
            {
                if (RQ.Activity_Flavour_Id == null)
                {
                    return false;
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    DataLayer.Activity_ClassificationAttributes obj = new Activity_ClassificationAttributes();

                    if (RQ.Activity_ClassificationAttribute_Id == null)
                    {
                        RQ.Activity_ClassificationAttribute_Id = Guid.NewGuid();
                    }

                    obj.Activity_ClassificationAttribute_Id = RQ.Activity_ClassificationAttribute_Id ?? Guid.NewGuid();
                    obj.Activity_Id = RQ.Activity_Id;
                    obj.Legacy_Product_ID = RQ.Legacy_Product_Id;
                    obj.IsActive = RQ.IsActive;
                    obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                    obj.AttributeSubType = RQ.AttributeSubType;
                    obj.AttributeType = RQ.AttributeType;
                    obj.AttributeValue = RQ.AttributeValue;
                    obj.InternalOnly = RQ.InternalOnly;
                    obj.Create_Date = RQ.CreateDate;
                    obj.Create_User = RQ.CreateUser;

                    context.Activity_ClassificationAttributes.Add(obj);
                    context.SaveChanges();

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding Activity classification attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateActivityClassifiationAttributes(DataContracts.Masters.DC_Activity_ClassificationAttributes RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Activity_ClassificationAttributes
                                  where a.Activity_ClassificationAttribute_Id == RQ.Activity_ClassificationAttribute_Id
                                  select a).First();
                    if (search != null)
                    {
                        if ((RQ.IsActive) != (search.IsActive ?? true))
                        {
                            search.IsActive = RQ.IsActive;
                            search.Edit_Date = RQ.EditDate;
                            search.Edit_User = RQ.EditUser;
                        }
                        else
                        {
                            search.Activity_Id = RQ.Activity_Id;
                            search.Legacy_Product_ID = RQ.Legacy_Product_Id;
                            search.IsActive = RQ.IsActive;
                            search.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            search.AttributeSubType = RQ.AttributeSubType;
                            search.AttributeType = RQ.AttributeType;
                            search.AttributeValue = RQ.AttributeValue;
                            search.InternalOnly = RQ.InternalOnly;
                            search.Edit_Date = RQ.EditDate;
                            search.Edit_User = RQ.EditUser;
                        }

                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Activity classification attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Activity PickUpDrop
        public List<DataContracts.Masters.DC_Activity_PickUpDrop> GetActivityPickUpDrop(DataContracts.Masters.DC_Activity_PickUpDrop_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_PickUpDrop
                                 select a;

                    if (RQ.Activity_PickUpDrop_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_PickUpDrop_Id == RQ.Activity_PickUpDrop_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.SupplierName != null)
                    {
                        search = from a in search
                                 where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Supplier_Id != null)
                    {
                        search = from a in search
                                 where a.Supplier_Id == RQ.Supplier_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_Id != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_Id == RQ.Legacy_Product_Id
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.SupplierName
                                 select new DataContracts.Masters.DC_Activity_PickUpDrop
                                 {
                                     Activity_PickUpDrop_Id = a.Activity_PickUpDrop_Id,
                                     Activity_Id = a.Activity_Id,
                                     IsAC = a.IsAC,
                                     Create_Date = a.Create_Date,
                                     Legacy_Product_Id = a.Legacy_Product_Id,
                                     Supplier_Id = a.Supplier_Id,
                                     SupplierName = a.SupplierName,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     TransferType = a.TransferType,
                                     VehicleCategory = a.VehicleCategory,
                                     VehicleName = a.VehicleName,
                                     VehicleType = a.VehicleType,
                                     TotalRecords = total,
                                     ForSupplier = a.ForSupplier,
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity  PickUpDrops", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }
        public DC_Message AddUpdatePickUpDrop(DC_Activity_PickUpDrop RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_PickUpDrop_Id != null)
                    {
                        var res = context.Activity_PickUpDrop.Find(RQ.Activity_PickUpDrop_Id);
                        if (res != null)
                        {
                            res.Activity_PickUpDrop_Id = RQ.Activity_PickUpDrop_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                            res.IsAC = RQ.IsAC;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            res.SupplierName = RQ.SupplierName;
                            res.Supplier_Id = RQ.Supplier_Id;
                            res.TransferType = RQ.TransferType;
                            res.VehicleCategory = RQ.VehicleCategory;
                            res.VehicleName = RQ.VehicleName;
                            res.VehicleType = RQ.VehicleType;
                            res.ForSupplier = RQ.ForSupplier;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_PickUpDrop obj = new DataLayer.Activity_PickUpDrop();

                        obj.Activity_PickUpDrop_Id = RQ.Activity_PickUpDrop_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_Id = RQ.Legacy_Product_Id;
                        obj.IsAC = RQ.IsAC;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.SupplierName = RQ.SupplierName;
                        obj.Supplier_Id = RQ.Supplier_Id;
                        obj.TransferType = RQ.TransferType;
                        obj.VehicleCategory = RQ.VehicleCategory;
                        obj.VehicleName = RQ.VehicleName;
                        obj.VehicleType = RQ.VehicleType;
                        obj.ForSupplier = RQ.ForSupplier;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_PickUpDrop.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity pickUpDrop", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Description
        public List<DataContracts.Masters.DC_Activity_Descriptions> GetActivityDescription(DataContracts.Masters.DC_Activity_Descriptions_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Descriptions
                                 select a;

                    if (RQ.Activity_Description_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Description_Id == RQ.Activity_Description_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_ID != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_ID == RQ.Legacy_Product_ID
                                 select a;
                    }

                    if (RQ.DescriptionFor != null)
                    {
                        search = from a in search
                                 where a.DescriptionFor.Trim().TrimStart().ToUpper() == RQ.DescriptionFor.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.DescriptionType
                                 select new DataContracts.Masters.DC_Activity_Descriptions
                                 {
                                     Activity_Description_Id = a.Activity_Description_Id,
                                     Activity_Id = a.Activity_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     IsActive = a.IsActive,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     Create_Date = a.Create_Date,
                                     DescriptionType = a.DescriptionType,
                                     DescriptionSubType = a.DescriptionSubType,
                                     Description = a.Description,
                                     Description_Name = a.Description_Name,
                                     DescriptionFor = a.DescriptionFor,
                                     FromDate = a.FromDate,
                                     ToDate = a.ToDate,
                                     Language_Code = a.Language_Code,
                                     Source = a.Source,
                                     TotalRecords = total,


                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Description", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityDescription(DC_Activity_Descriptions RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_Description_Id != null)
                    {
                        var res = context.Activity_Descriptions.Find(RQ.Activity_Description_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_Description_Id = RQ.Activity_Description_Id ?? Guid.NewGuid();
                                res.Activity_Id = RQ.Activity_Id;
                                res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                                res.Legacy_Product_ID = RQ.Legacy_Product_ID;
                                res.DescriptionType = RQ.DescriptionType;
                                res.DescriptionSubType = RQ.DescriptionSubType;
                                res.Description = RQ.Description;
                                res.Description_Name = RQ.Description_Name;
                                res.DescriptionFor = RQ.DescriptionFor;
                                res.FromDate = RQ.FromDate;
                                res.ToDate = RQ.ToDate;
                                res.Language_Code = RQ.Language_Code;
                                res.Source = RQ.Source;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;
                    if (IsInsert)
                    {
                        Activity_Descriptions newmed = new Activity_Descriptions();
                        newmed.Activity_Description_Id = RQ.Activity_Description_Id ?? Guid.NewGuid();
                        newmed.Activity_Id = RQ.Activity_Id;
                        newmed.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        newmed.IsActive = RQ.IsActive;
                        newmed.Legacy_Product_ID = RQ.Legacy_Product_ID;
                        newmed.DescriptionType = RQ.DescriptionType;
                        newmed.DescriptionSubType = RQ.DescriptionSubType;
                        newmed.Description = RQ.Description;
                        newmed.Description_Name = RQ.Description_Name;
                        newmed.DescriptionFor = RQ.DescriptionFor;
                        newmed.FromDate = RQ.FromDate;
                        newmed.ToDate = RQ.ToDate;
                        newmed.Language_Code = RQ.Language_Code;
                        newmed.Source = RQ.Source;
                        newmed.Create_Date = DateTime.Now;
                        newmed.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_Descriptions.Add(newmed);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Descriptions", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region PickUpDrop Details
        public List<DataContracts.Masters.DC_Activity_PickUpDropDetails> GetPickUpDropDetails(DataContracts.Masters.DC_Activity_PickUpDropDetails_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_PickUpDropDetail
                                 select a;

                    if (RQ.Activity_PickUpDropDetail_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_PickUpDropDetail_Id == RQ.Activity_PickUpDropDetail_Id
                                 select a;
                    }

                    if (RQ.Activity_PickUpDrop_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_PickUpDrop_Id == RQ.Activity_PickUpDrop_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_Id != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_Id == RQ.Legacy_Product_Id
                                 select a;
                    }
                    if (RQ.Accommodation_Id != null)
                    {
                        search = from a in search
                                 where a.Accommodation_Id == RQ.Accommodation_Id
                                 select a;
                    }
                    if (RQ.FromToType != null)
                    {
                        search = from a in search
                                 where a.FromToType.Trim().TrimStart().ToUpper() == RQ.FromToType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Acco_State != null)
                    {
                        search = from a in search
                                 where a.Acco_State.Trim().TrimStart().ToUpper() == RQ.Acco_State.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Acco_Country != null)
                    {
                        search = from a in search
                                 where a.Acco_Country.Trim().TrimStart().ToUpper() == RQ.Acco_Country.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Acco_City != null)
                    {
                        search = from a in search
                                 where a.Acco_City.Trim().TrimStart().ToUpper() == RQ.Acco_City.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.LocationName != null)
                    {
                        search = from a in search
                                 where a.LocationName.Trim().TrimStart().ToUpper() == RQ.LocationName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.PickUpDropType != null)
                    {
                        search = from a in search
                                 where a.PickUpDropType.Trim().TrimStart().ToUpper() == RQ.PickUpDropType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.LocationName
                                 select new DataContracts.Masters.DC_Activity_PickUpDropDetails
                                 {
                                     Activity_PickUpDropDetail_Id = a.Activity_PickUpDropDetail_Id,
                                     Activity_PickUpDrop_Id = a.Activity_PickUpDrop_Id,
                                     Activity_Id = a.Activity_Id,
                                     Create_Date = a.Create_Date,
                                     Legacy_Product_Id = a.Legacy_Product_Id,
                                     TotalRecords = total,
                                     LocationName = a.LocationName,
                                     LocationType = a.LocationType,
                                     FromToType = a.FromToType,
                                     PickUpDropType = a.PickUpDropType,
                                     Accommodation_Id = a.Accommodation_Id,
                                     Acco_Address = a.Acco_Address,
                                     Acco_Area = a.Acco_Area,
                                     Acco_City = a.Acco_City,
                                     Acco_State = a.Acco_State,
                                     Acco_Country = a.Acco_Country,
                                     Acco_ContactNotes = a.Acco_ContactNotes,
                                     Acco_Email = a.Acco_Email,
                                     Acco_Fax = a.Acco_Fax,
                                     Acco_PostalCode = a.Acco_PostalCode,
                                     Acco_Location = a.Acco_Location,
                                     Acco_Name = a.Acco_Name,
                                     Acco_Telephone = a.Acco_Telephone,
                                     Acco_Website = a.Acco_Website,
                                     AreaNameofPlace = a.AreaNameofPlace,
                                     AreaSearchFor = a.AreaSearchFor
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity  PickUpDrop Details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }
        public DataContracts.DC_Message AddUpdatePickUpDropDetails(DC_Activity_PickUpDropDetails RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_PickUpDropDetail_Id != null)
                    {
                        var res = context.Activity_PickUpDropDetail.Find(RQ.Activity_PickUpDropDetail_Id);
                        if (res != null)
                        {
                            res.Activity_PickUpDropDetail_Id = RQ.Activity_PickUpDropDetail_Id ?? Guid.Empty;
                            res.Activity_PickUpDrop_Id = RQ.Activity_PickUpDrop_Id;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                            res.LocationName = RQ.LocationName;
                            res.LocationType = RQ.LocationType;
                            res.FromToType = RQ.FromToType;
                            res.PickUpDropType = RQ.PickUpDropType;
                            res.Accommodation_Id = RQ.Accommodation_Id;
                            res.Acco_Address = RQ.Acco_Address;
                            res.Acco_Area = RQ.Acco_Area;
                            res.Acco_City = RQ.Acco_City;
                            res.Acco_State = RQ.Acco_State;
                            res.Acco_Country = RQ.Acco_Country;
                            res.Acco_ContactNotes = RQ.Acco_ContactNotes;
                            res.Acco_Email = RQ.Acco_Email;
                            res.Acco_Fax = RQ.Acco_Fax;
                            res.Acco_PostalCode = RQ.Acco_PostalCode;
                            res.Acco_Location = RQ.Acco_Location;
                            res.Acco_Name = RQ.Acco_Name;
                            res.Acco_Telephone = RQ.Acco_Telephone;
                            res.Acco_Website = RQ.Acco_Website;
                            res.AreaNameofPlace = RQ.AreaNameofPlace;
                            res.AreaSearchFor = RQ.AreaSearchFor;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_PickUpDropDetail obj = new DataLayer.Activity_PickUpDropDetail();

                        obj.Activity_PickUpDropDetail_Id = RQ.Activity_PickUpDropDetail_Id ?? Guid.NewGuid();
                        obj.Activity_PickUpDrop_Id = RQ.Activity_PickUpDrop_Id;
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_Id = RQ.Legacy_Product_Id;
                        obj.LocationName = RQ.LocationName;
                        obj.LocationType = RQ.LocationType;
                        obj.FromToType = RQ.FromToType;
                        obj.PickUpDropType = RQ.PickUpDropType;
                        obj.Accommodation_Id = RQ.Accommodation_Id;
                        obj.Acco_Address = RQ.Acco_Address;
                        obj.Acco_Area = RQ.Acco_Area;
                        obj.Acco_City = RQ.Acco_City;
                        obj.Acco_State = RQ.Acco_State;
                        obj.Acco_Country = RQ.Acco_Country;
                        obj.Acco_ContactNotes = RQ.Acco_ContactNotes;
                        obj.Acco_Email = RQ.Acco_Email;
                        obj.Acco_Fax = RQ.Acco_Fax;
                        obj.Acco_PostalCode = RQ.Acco_PostalCode;
                        obj.Acco_Location = RQ.Acco_Location;
                        obj.Acco_Name = RQ.Acco_Name;
                        obj.Acco_Telephone = RQ.Acco_Telephone;
                        obj.Acco_Website = RQ.Acco_Website;
                        obj.AreaNameofPlace = RQ.AreaNameofPlace;
                        obj.AreaSearchFor = RQ.AreaSearchFor;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_PickUpDropDetail.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity pickUpDrop Details", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Flavour
        public List<DataContracts.Masters.DC_Activity_Flavour> GetActivityFlavour(DataContracts.Masters.DC_Activity_Flavour_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from a in context.Activity_Flavour
                                  select a).AsQueryable();

                    var ActFlv = search;

                    var spmQ = context.Activity_SupplierProductMapping.AsQueryable();

                    var ActTypes = context.Activity_CategoriesType.AsQueryable();

                    var ActTypeUnMap = ActTypes;

                    var aCA = context.Activity_ClassificationAttributes.AsQueryable();

                    var aDOW = context.Activity_DaysOfWeek.AsQueryable();
                    var aMedia = context.Activity_Media.Select(x => x.Activity_Flavour_Id).Distinct().AsQueryable();

                    bool isCAFilter = false;
                    bool isDurationFilter = false;
                    bool isTypeUnMap = false;
                    bool isTypeMap = false;

                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    else
                    {
                        if (RQ.NoPhysicalIntensity)
                        {
                            aCA = aCA.Where(w => w.AttributeType == "Product" && w.AttributeSubType == "PhysicalIntensity");
                            isCAFilter = true;
                        }

                        if (RQ.NoSpecials)
                        {
                            aCA = aCA.Where(w => w.AttributeType == "Product" && w.AttributeSubType == "Specials");
                            isCAFilter = true;
                        }

                        if (RQ.NoSuitableFor)
                        {
                            aCA = aCA.Where(w => w.AttributeType == "Product" && w.AttributeSubType == "SuitableFor");
                            isCAFilter = true;
                        }

                        if (RQ.OnlyMedia)
                        {
                            search = (from a in search
                                      join m in aMedia on a.Activity_Flavour_Id equals m.Value
                                      select a);
                        }

                        if (isCAFilter)
                        {
                            search = search.Except((from a in ActFlv
                                                    join ca in aCA on a.Activity_Flavour_Id equals ca.Activity_Flavour_Id
                                                    select a).AsQueryable());
                        }

                        if (RQ.NoOpsSchedule)
                        {
                            isDurationFilter = true;
                        }

                        if (RQ.NoSession)
                        {
                            aDOW = aDOW.Where(w => w.Duration != null && w.Duration != "00.00:00");
                            isDurationFilter = true;
                        }

                        if (isDurationFilter)
                        {
                            search = search.Except((from a in ActFlv
                                                    join ca in aDOW on a.Activity_Flavour_Id equals ca.Activity_Flavor_ID
                                                    select a).AsQueryable());
                        }

                        if (RQ.Supplier_Id != null)
                        {
                            spmQ = spmQ.Where(w => w.Supplier_ID == RQ.Supplier_Id);
                        }
                        //For Supplier value Country and city
                        if (!string.IsNullOrWhiteSpace(RQ.SupplierCountryName))
                        {
                            spmQ = spmQ.Where(w => w.SupplierCountryName == RQ.SupplierCountryName);
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.SupplierCityName))
                        {
                            spmQ = spmQ.Where(w => w.SupplierCityName == RQ.SupplierCityName);
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.ProductName))
                        {
                            search = from a in search
                                     where a.ProductName.Trim().ToUpper().Contains(RQ.ProductName.Trim().ToUpper())
                                     select a;
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.Country))
                        {
                            if (RQ.Country.Contains("UNMAPPED"))
                            {
                                search = from a in search
                                         where a.Country_Id == null
                                         select a;
                            }
                            else
                            {
                                if (RQ.Country_Id != null)
                                {
                                    search = from a in search
                                             where a.Country_Id == RQ.Country_Id
                                             select a;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.City))
                        {
                            if (RQ.City.Contains("UNMAPPED"))
                            {
                                search = from a in search
                                         where a.City_Id == null
                                         select a;
                            }
                            else
                            {
                                if (RQ.City_Id != null)
                                {
                                    search = from a in search
                                             where a.City_Id == RQ.City_Id
                                             select a;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.ProductCategorySubType))
                        {
                            if (RQ.ProductCategorySubType.Contains("UNMAPPED"))
                            {
                                //search = from a in search
                                //         where a.ProductCategorySubType == null || a.ProductCategorySubType.Trim() == string.Empty
                                //         select a;

                                isTypeUnMap = true;
                                ActTypeUnMap = ActTypeUnMap.Where(w => w.SystemProductCategorySubType_ID != null);

                                //search = search.Except((from a in ActFlv
                                //                        join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //                        where at.Activity_FlavourOptions_Id == null && at.SystemProductCategorySubType_ID != null
                                //                        select a).AsQueryable());

                            }
                            else if (RQ.ProductCategorySubType != "-ALL-")
                            {
                                //search = from a in search
                                //         where a.ProductCategorySubType.Trim().ToUpper().Contains(RQ.ProductCategorySubType.Trim().ToUpper())
                                //         select a;

                                isTypeMap = true;
                                ActTypes = ActTypes.Where(w => w.SystemProductCategorySubType_ID == RQ.ProductCategorySubTypeId);

                                //search = from a in search
                                //         join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //         where at.Activity_FlavourOptions_Id == null && at.SystemProductCategorySubType_ID == RQ.ProductCategorySubTypeId
                                //         select a;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.ProductType))
                        {
                            if (RQ.ProductType.Contains("UNMAPPED"))
                            {
                                //search = from a in search
                                //         where a.ProductType == null || a.ProductType.Trim() == string.Empty
                                //         select a;

                                isTypeUnMap = true;
                                ActTypeUnMap = ActTypeUnMap.Where(w => w.SystemProductType_ID != null);

                                //search = search.Except((from a in ActFlv
                                //                        join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //                        where at.Activity_FlavourOptions_Id == null && at.SystemProductType_ID != null
                                //                        select a).AsQueryable());
                            }
                            else if (RQ.ProductType != "-ALL-")
                            {
                                //search = from a in search
                                //         where a.ProductType.Trim().ToUpper().Contains(RQ.ProductType.Trim().ToUpper())
                                //         select a;

                                isTypeMap = true;
                                ActTypes = ActTypes.Where(w => w.SystemProductType_ID == RQ.ProductTypeId);

                                //search = from a in search
                                //         join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //         where at.Activity_FlavourOptions_Id == null && at.SystemProductType_ID == RQ.ProductTypeId
                                //         select a;
                            }
                        }


                        if (!string.IsNullOrWhiteSpace(RQ.ProductNameSubType))
                        {
                            if (RQ.ProductNameSubType.Contains("UNMAPPED"))
                            {
                                //search = from a in search
                                //         where a.ProductNameSubType == null || a.ProductNameSubType.Trim() == string.Empty
                                //         select a;

                                isTypeUnMap = true;
                                ActTypeUnMap = ActTypeUnMap.Where(w => w.SystemProductNameSubType_ID != null);

                                //search = search.Except((from a in ActFlv
                                //                        join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //                        where at.Activity_FlavourOptions_Id == null && at.SystemProductNameSubType_ID != null
                                //                        select a).AsQueryable());
                            }
                            else if (RQ.ProductNameSubType != "-ALL-")
                            {
                                //search = from a in search
                                //         where a.ProductNameSubType.Trim().ToUpper().Contains(RQ.ProductNameSubType.Trim().ToUpper())
                                //         select a;

                                isTypeMap = true;
                                ActTypes = ActTypes.Where(w => w.SystemProductNameSubType_ID == RQ.ProductNameSubTypeId);

                                //search = from a in search
                                //         join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                //         where at.Activity_FlavourOptions_Id == null && at.SystemProductNameSubType_ID == RQ.ProductNameSubTypeId
                                //         select a;
                            }



                        }

                        if (!string.IsNullOrWhiteSpace(RQ.SupplierProductNameSubType) && RQ.SupplierProductNameSubType != "-ALL-")
                        {
                            isTypeMap = true;
                            ActTypes = ActTypes.Where(w => w.SupplierProductNameSubType == RQ.SupplierProductNameSubType);
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.InterestType))
                        {
                            if (RQ.InterestType.Contains("UNMAPPED"))
                            {
                                isTypeUnMap = true;
                                ActTypeUnMap = ActTypeUnMap.Where(w => w.SystemInterestType_ID != null);
                            }
                            else if (RQ.InterestType != "-ALL-")
                            {
                                isTypeMap = true;
                                ActTypes = ActTypes.Where(w => w.SystemInterestType_ID == RQ.InterestTypeId);
                            }
                        }


                        if (isTypeUnMap)
                        {
                            search = search.Except((from a in ActFlv
                                                    join at in ActTypeUnMap on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                                    where at.Activity_FlavourOptions_Id == null
                                                    select a));
                        }

                        if (isTypeMap)
                        {
                            search = from a in search
                                     join at in ActTypes on a.Activity_Flavour_Id equals at.Activity_Flavour_Id
                                     where at.Activity_FlavourOptions_Id == null
                                     select a;
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.Activity_Status))
                        {
                            search = search.Where(w => w.Activity_Status == RQ.Activity_Status);
                        }

                    }

                    int total = (from a in search
                                 join spm in spmQ on a.Activity_Flavour_Id equals spm.Activity_ID
                                 join s in context.Supplier on spm.Supplier_ID equals s.Supplier_Id
                                 where s.StatusCode == "ACTIVE"
                                 select a).Count();

                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 join spm in spmQ on a.Activity_Flavour_Id equals spm.Activity_ID
                                 join s in context.Supplier on spm.Supplier_ID equals s.Supplier_Id
                                 where s.StatusCode == "ACTIVE"
                                 orderby a.ProductName
                                 select new DataContracts.Masters.DC_Activity_Flavour
                                 {
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_Id = a.Activity_Id,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     ProductName = a.ProductName,

                                     Country = a.Country,
                                     City = a.City,
                                     Country_Id = a.Country_Id,
                                     City_Id = a.City_Id,
                                     CityCode = a.CityCode,
                                     CountryCode = a.CountryCode,

                                     Area = a.Area,
                                     CommonProductNameSubType_Id = a.CommonProductNameSubType_Id,
                                     CompanyProductNameSubType_Id = a.CompanyProductNameSubType_Id,
                                     CompanyReccom = a.CompanyReccom ?? false,
                                     Duration = a.Duration,
                                     EndingPoint = a.EndingPoint,
                                     FinanceControlId = a.FinanceControlId,
                                     IsPickUpDropDefined = a.IsPickUpDropDefined ?? false,
                                     //Latitude = a.Latitude,
                                     //Longitude = a.Longitude,
                                     Location = a.Location,
                                     MustSeeInCountry = a.MustSeeInCountry ?? false,
                                     PlaceOfEvent = a.PlaceOfEvent,
                                     PostalCode = a.PostalCode,
                                     StartingPoint = a.StartingPoint,
                                     Street = a.Street,
                                     Street2 = a.Street2,
                                     Street3 = a.Street3,
                                     Street4 = a.Street4,
                                     Street5 = a.Street5,
                                     USP = a.USP,
                                     Create_Date = a.Create_Date,
                                     TotalRecords = total,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,

                                     ProductCategory = a.ProductCategory,

                                     //ProductCategorySubType = a.ProductCategorySubType,
                                     //ProductNameSubType = a.ProductNameSubType,
                                     //ProductType = a.ProductType,

                                     //ProductCategorySubType = string.Join(",", context.Activity_CategoriesType.AsNoTracking().Where(w => w.Activity_Flavour_Id == a.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null).Select(s => s.SupplierProductCategorySubType).Distinct().ToArray()),
                                     //ProductNameSubType = string.Join(",", context.Activity_CategoriesType.AsNoTracking().Where(w => w.Activity_Flavour_Id == a.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null).Select(s => s.SupplierProductNameSubType).Distinct().ToArray()),
                                     //ProductType = string.Join(",", context.Activity_CategoriesType.AsNoTracking().Where(w => w.Activity_Flavour_Id == a.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null).Select(s => s.SupplierProductType).Distinct().ToArray()),

                                     SupplierCity = spm.SupplierCityName + " (" + spm.SupplierCityCode + ")",
                                     SupplierCountry = spm.SupplierCountryName + " (" + spm.SupplierCountryCode + ")",
                                     SupplierProductCategory = "Activities",
                                     //SupplierProductCategorySubType = spm.SupplierType,
                                     SupplierProductCode = spm.SuplierProductCode,
                                     //SupplierProductNameSubType = spm.SupplierProductType,
                                     //SupplierProductType = spm.SupplierType,
                                     Supplier_Id = s.Supplier_Id,
                                     SupplierCode = s.Code,
                                     SupplierName = s.Name,
                                     Activity_Status = a.Activity_Status,
                                     Activity_Status_Edit_Date = a.Activity_Status_Edit_Date,
                                     Activity_Status_Edit_User = a.Activity_Status_Edit_User,
                                     Activity_StatusNotes = a.Activity_StatusNotes,
                                     //TourType
                                     Activity_TourType = a.TourType,
                                     SupplierTourType = spm.SupplierTourType,
                                     TLGXDisplaySubType = a.TLGXDisplaySubType,

                                     //TourType


                                     // Area/Address

                                     SupplierLocation = spm.Location,

                                     Categories = (from ct in context.Activity_CategoriesType
                                                   where ct.Activity_Flavour_Id == a.Activity_Flavour_Id && ct.Activity_FlavourOptions_Id == null && ct.IsActive == true
                                                   select new DC_Activity_CategoryTypes
                                                   {
                                                       Activity_CategoriesType_ID = ct.Activity_CategoriesType_ID,
                                                       Activity_FlavourOptions_Id = ct.Activity_FlavourOptions_Id,
                                                       Activity_Flavour_Id = ct.Activity_Flavour_Id,
                                                       SupProdCat = ct.SupplierProductCategory,
                                                       SupProdSubCat = ct.SupplierProductCategory,
                                                       SupProdSubType = ct.SupplierProductNameSubType,
                                                       SupProdType = ct.SupplierProductType,
                                                       SysProdCat = ct.SystemProductCategory,
                                                       SysProdCatId = ct.SystemProductCategory_ID,
                                                       SysProdSubCat = ct.SystemProductCategorySubType,
                                                       SysProdSubCatId = ct.SystemProductCategorySubType_ID,
                                                       SysProdSubType = ct.SystemProductNameSubType,
                                                       SysProdSubTypeId = ct.SystemProductNameSubType_ID,
                                                       SysProdType = ct.SystemProductType,
                                                       SysProdTypeId = ct.SystemProductType_ID,
                                                       SysInterestType = ct.SystemInterestType,
                                                       SysInterestTypeId = ct.SystemInterestType_ID
                                                   }).ToList()
                                 };


                    var returnResult = result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();

                    foreach (var item in returnResult)
                    {
                        var CatType = context.Activity_CategoriesType.AsNoTracking().Where(w => w.Activity_Flavour_Id == item.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null).Select(s => s);

                        item.InterestType = string.Join(",", CatType.Select(s => s.SystemInterestType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');
                        item.ProductCategorySubType = string.Join(",", CatType.Select(s => s.SystemProductCategorySubType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');
                        item.ProductNameSubType = string.Join(",", CatType.Select(s => s.SystemProductNameSubType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');
                        item.ProductType = string.Join(",", CatType.Select(s => s.SystemProductType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');

                        item.SupplierProductType = string.Join(",", CatType.Select(s => s.SupplierProductType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');
                        item.SupplierProductCategorySubType = string.Join(",", CatType.Select(s => s.SupplierProductCategorySubType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');
                        item.SupplierProductNameSubType = string.Join(",", CatType.Select(s => s.SupplierProductNameSubType).Distinct().ToArray()).TrimEnd(',').TrimStart(',');

                        //item.Categories = (from ct in context.Activity_CategoriesType
                        //                   where ct.Activity_Flavour_Id == item.Activity_Flavour_Id && ct.Activity_FlavourOptions_Id == null
                        //                   select new DC_Activity_CategoryTypes
                        //                   {
                        //                       Activity_CategoriesType_ID = ct.Activity_CategoriesType_ID,
                        //                       Activity_FlavourOptions_Id = ct.Activity_FlavourOptions_Id,
                        //                       Activity_Flavour_Id = ct.Activity_Flavour_Id,
                        //                       SupProdCat = ct.SupplierProductCategory,
                        //                       SupProdSubCat = ct.SupplierProductCategory,
                        //                       SupProdSubType = ct.SupplierProductNameSubType,
                        //                       SupProdType = ct.SupplierProductType,
                        //                       SysProdCat = ct.SystemProductCategory,
                        //                       SysProdCatId = ct.SystemProductCategory_ID,
                        //                       SysProdSubCat = ct.SystemProductCategorySubType,
                        //                       SysProdSubCatId = ct.SystemProductCategorySubType_ID,
                        //                       SysProdSubType = ct.SystemProductNameSubType,
                        //                       SysProdSubTypeId = ct.SystemProductNameSubType_ID,
                        //                       SysProdType = ct.SystemProductType,
                        //                       SysProdTypeId = ct.SystemProductType_ID
                        //                   }).ToList();
                    }


                    return returnResult;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Activity Flavour",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityFlavour(DC_Activity_Flavour RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_Flavour_Id != null)
                    {
                        var res = context.Activity_Flavour.Find(RQ.Activity_Flavour_Id);

                        if (res != null)
                        {
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = RQ.Edit_User;

                            res.ProductCategory = RQ.ProductCategory;
                            res.TLGXDisplaySubType = RQ.TLGXDisplaySubType;

                            if (!string.IsNullOrWhiteSpace(RQ.ProductCategorySubType))
                            {
                                res.ProductCategorySubType = string.Empty;
                                foreach (var str in RQ.ProductCategorySubType.Split(','))
                                {
                                    Guid strGuid = Guid.Parse(str);
                                    res.ProductCategorySubType = res.ProductCategorySubType + context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == strGuid).Select(s => s.AttributeValue).First() + ",";
                                }
                                res.ProductCategorySubType = res.ProductCategorySubType.TrimEnd(',');
                            }

                            if (!string.IsNullOrWhiteSpace(RQ.ProductType))
                            {
                                res.ProductType = string.Empty;
                                foreach (var str in RQ.ProductType.Split(','))
                                {
                                    Guid strGuid = Guid.Parse(str);
                                    res.ProductType = res.ProductType + context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == strGuid).Select(s => s.AttributeValue).First() + ",";
                                }
                                res.ProductType = res.ProductType.TrimEnd(',');
                            }

                            if (!string.IsNullOrWhiteSpace(RQ.ProductNameSubType))
                            {
                                res.ProductNameSubType = string.Empty;
                                foreach (var str in RQ.ProductNameSubType.Split(','))
                                {
                                    Guid strGuid = Guid.Parse(str);
                                    res.ProductNameSubType = res.ProductNameSubType + context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == strGuid).Select(s => s.AttributeValue).First() + ",";
                                }
                                res.ProductNameSubType = res.ProductNameSubType.TrimEnd(',');
                            }
                            if (!string.IsNullOrWhiteSpace(RQ.InterestType))
                            {
                                res.InterestType = string.Empty;
                                foreach (var str in RQ.InterestType.Split(','))
                                {
                                    Guid strGuid = Guid.Parse(str);
                                    res.InterestType = res.InterestType + context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == strGuid).Select(s => s.AttributeValue).First() + ",";
                                }
                                res.InterestType = res.InterestType.TrimEnd(',');
                            }

                            if ((RQ.Country_Id ?? Guid.Empty) != Guid.Empty)
                            {
                                var country = context.m_CountryMaster.AsNoTracking().Where(w => w.Country_Id == RQ.Country_Id).Select(s => s).FirstOrDefault();
                                if (country != null)
                                {
                                    res.Country_Id = country.Country_Id;
                                    res.Country = country.Name;
                                    res.CountryCode = country.Code;
                                }
                                else
                                {
                                    res.Country_Id = null;
                                    res.Country = null;
                                    res.CountryCode = null;
                                }
                            }
                            else
                            {
                                res.Country_Id = null;
                                res.Country = null;
                                res.CountryCode = null;
                            }

                            if ((RQ.City_Id ?? Guid.Empty) != Guid.Empty)
                            {
                                var city = context.m_CityMaster.AsNoTracking().Where(w => w.City_Id == RQ.City_Id).Select(s => s).FirstOrDefault();
                                if (city != null)
                                {
                                    res.City_Id = city.City_Id;
                                    res.City = city.Name;
                                    res.CityCode = city.Code;
                                }
                                else
                                {
                                    res.City_Id = null;
                                    res.City = null;
                                    res.CityCode = null;
                                }
                            }
                            else
                            {
                                res.City_Id = null;
                                res.City = null;
                                res.CityCode = null;
                            }

                            //context.Activity_CategoriesType.RemoveRange(context.Activity_CategoriesType.Where(w => w.Activity_Flavour_Id == RQ.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null));

                            //Set IsActive false 
                            var lstCT = context.Activity_CategoriesType.Where(w => w.Activity_Flavour_Id == RQ.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null && w.IsActive == true).ToList();
                            if (lstCT != null && lstCT.Count() > 0)
                            {
                                foreach (var item in lstCT)
                                    item.IsActive = false;
                            }


                            foreach (var type in RQ.Categories)
                            {
                                var subtype = context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == type.SysProdSubTypeId).Select(s => s).FirstOrDefault();
                                //Adding Interest Type

                                //var interestType =  context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == type.SysInterestTypeId).Select(s => s).FirstOrDefault();
                                if (subtype != null)
                                {

                                    var prodtype = context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == subtype.ParentAttributeValue_Id).Select(s => s).FirstOrDefault();
                                    if (prodtype != null)
                                    {
                                        var subcat = context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == prodtype.ParentAttributeValue_Id).Select(s => s).FirstOrDefault();
                                        if (subcat != null)
                                        {
                                            var isInterstType = subcat.ParentAttributeValue_Id ?? Guid.Empty;
                                            var InterstType = context.m_masterattributevalue.AsNoTracking().Where(w => w.MasterAttributeValue_Id == isInterstType).Select(s => s).FirstOrDefault();

                                            var Cat_item = context.Activity_CategoriesType.Where(w => w.Activity_Flavour_Id == RQ.Activity_Flavour_Id && w.SystemProductNameSubType_ID == subtype.MasterAttributeValue_Id && w.SystemProductCategorySubType_ID == subcat.MasterAttributeValue_Id && w.SystemProductType_ID == prodtype.MasterAttributeValue_Id).FirstOrDefault();
                                            if (Cat_item != null)
                                            {
                                                //Update
                                                Cat_item.IsActive = true;
                                                Cat_item.Edit_Date = RQ.Create_Date;
                                                Cat_item.Edit_User = RQ.Edit_User;

                                            }
                                            else
                                            {
                                                //Insert
                                                //context.Activity_CategoriesType.Add(new Activity_CategoriesType
                                                //{
                                                //    Activity_CategoriesType_ID = Guid.NewGuid(),
                                                //    Activity_FlavourOptions_Id = null,
                                                //    Activity_Flavour_Id = type.Activity_Flavour_Id ?? RQ.Activity_Flavour_Id ?? Guid.Empty,
                                                //    SystemProductNameSubType_ID = subtype.MasterAttributeValue_Id,
                                                //    SystemProductNameSubType = subtype.AttributeValue,
                                                //    SystemProductType_ID = prodtype.MasterAttributeValue_Id,
                                                //    SystemProductType = prodtype.AttributeValue,
                                                //    SystemProductCategorySubType_ID = subcat.MasterAttributeValue_Id,
                                                //    SystemProductCategorySubType = subcat.AttributeValue,
                                                //    SystemProductCategory = "ACTIVITIES",
                                                //    Create_Date = DateTime.Now,
                                                //    Create_User = type.User,
                                                //    IsActive = true
                                                //});

                                                Activity_CategoriesType _objCT = new Activity_CategoriesType();
                                                _objCT.Activity_CategoriesType_ID = Guid.NewGuid();
                                                _objCT.Activity_FlavourOptions_Id = null;
                                                _objCT.Activity_Flavour_Id = type.Activity_Flavour_Id ?? RQ.Activity_Flavour_Id ?? Guid.Empty;
                                                _objCT.SystemProductNameSubType_ID = subtype.MasterAttributeValue_Id;
                                                _objCT.SystemProductNameSubType = subtype.AttributeValue;
                                                _objCT.SystemProductType_ID = prodtype.MasterAttributeValue_Id;
                                                _objCT.SystemProductType = prodtype.AttributeValue;
                                                _objCT.SystemProductCategorySubType_ID = subcat.MasterAttributeValue_Id;
                                                _objCT.SystemProductCategorySubType = subcat.AttributeValue;
                                                _objCT.SystemProductCategory = "ACTIVITIES";
                                                _objCT.Create_Date = DateTime.Now;
                                                _objCT.Create_User = type.User;
                                                _objCT.IsActive = true;

                                                //Check parent (interest type )
                                                if (InterstType != null)
                                                {
                                                    _objCT.SystemInterestType = InterstType.AttributeValue;
                                                    _objCT.SystemInterestType_ID = InterstType.MasterAttributeValue_Id;
                                                }

                                                context.Activity_CategoriesType.Add(_objCT);
                                            }
                                        }
                                    }
                                }
                            }

                            //res.Activity_Id = RQ.Activity_Id;
                            //res.Legacy_Product_ID = RQ.Legacy_Product_ID;
                            //res.ProductName = RQ.ProductName;
                            //res.Area = RQ.Area;
                            //res.CommonProductNameSubType_Id = RQ.CommonProductNameSubType_Id;
                            //res.CompanyProductNameSubType_Id = RQ.CompanyProductNameSubType_Id;
                            //res.CompanyReccom = RQ.CompanyReccom;
                            //res.Duration = RQ.Duration;
                            //res.EndingPoint = RQ.EndingPoint;
                            //res.FinanceControlId = RQ.FinanceControlId;
                            //res.IsPickUpDropDefined = RQ.IsPickUpDropDefined;
                            //res.Latitude = RQ.Latitude;
                            //res.Longitude = RQ.Longitude;
                            //res.Location = RQ.Location;
                            //res.MustSeeInCountry = RQ.MustSeeInCountry;
                            //res.PlaceOfEvent = RQ.PlaceOfEvent;
                            //res.PostalCode = RQ.PostalCode;
                            //res.StartingPoint = RQ.StartingPoint;
                            //res.Street = RQ.Street;
                            //res.Street2 = RQ.Street2;
                            //res.Street3 = RQ.Street3;
                            //res.Street4 = RQ.Street4;
                            //res.Street5 = RQ.Street5;
                            //res.USP = RQ.USP;


                            //TourType
                            res.TourType = RQ.Activity_TourType;
                            res.Location = RQ.Location;


                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }

                    if (IsInsert)
                    {
                        DataLayer.Activity_Flavour obj = new DataLayer.Activity_Flavour();

                        RQ.Activity_Flavour_Id = RQ.Activity_Flavour_Id ?? Guid.NewGuid();
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_ID = RQ.Legacy_Product_ID;
                        obj.ProductName = RQ.ProductName;
                        obj.ProductNameSubType = RQ.ProductNameSubType;
                        obj.ProductType = RQ.ProductType;
                        obj.Country = RQ.Country;
                        obj.City = RQ.City;
                        obj.Country_Id = RQ.Country_Id;
                        obj.City_Id = RQ.City_Id;
                        obj.CityCode = RQ.CityCode;
                        obj.CountryCode = RQ.CountryCode;
                        obj.ProductCategory = RQ.ProductCategory;
                        obj.ProductCategorySubType = RQ.ProductCategorySubType;
                        obj.Area = RQ.Area;
                        obj.CommonProductNameSubType_Id = RQ.CommonProductNameSubType_Id;
                        obj.CompanyProductNameSubType_Id = RQ.CompanyProductNameSubType_Id;
                        obj.CompanyReccom = RQ.CompanyReccom;
                        obj.Duration = RQ.Duration;
                        obj.EndingPoint = RQ.EndingPoint;
                        obj.FinanceControlId = RQ.FinanceControlId;
                        obj.IsPickUpDropDefined = RQ.IsPickUpDropDefined;
                        obj.Latitude = RQ.Latitude;
                        obj.Longitude = RQ.Longitude;
                        obj.Location = RQ.Location;
                        obj.MustSeeInCountry = RQ.MustSeeInCountry;
                        obj.PlaceOfEvent = RQ.PlaceOfEvent;
                        obj.PostalCode = RQ.PostalCode;
                        obj.StartingPoint = RQ.StartingPoint;
                        obj.Street = RQ.Street;
                        obj.Street2 = RQ.Street2;
                        obj.Street3 = RQ.Street3;
                        obj.Street4 = RQ.Street4;
                        obj.Street5 = RQ.Street5;
                        obj.USP = RQ.USP;
                        obj.TLGXDisplaySubType = RQ.TLGXDisplaySubType;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        obj.TourType = RQ.Activity_TourType;
                        context.Activity_Flavour.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }

                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity flavour", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityCA(List<DC_Activity_CA_CRUD> RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {


                    if (RQ != null)
                    {
                        foreach (var item in RQ)
                        {
                            IEnumerable<Activity_ClassificationAttributes> dataToRemove = null;
                            if (item.Activity_FlavourOptions_Id == null)
                            {
                                dataToRemove = context.Activity_ClassificationAttributes.Where(w => w.Activity_Flavour_Id == item.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == null && w.AttributeType == item.AttributeType && w.AttributeSubType == item.AttributeSubType).Select(s => s);
                            }
                            else
                            {
                                dataToRemove = context.Activity_ClassificationAttributes.Where(w => w.Activity_Flavour_Id == item.Activity_Flavour_Id && w.Activity_FlavourOptions_Id == item.Activity_FlavourOptions_Id && w.AttributeType == item.AttributeType && w.AttributeSubType == item.AttributeSubType).Select(s => s);
                            }

                            if (dataToRemove != null)
                            {
                                context.Activity_ClassificationAttributes.RemoveRange(dataToRemove);
                            }

                            foreach (var str in item.AttributeValues)
                            {
                                context.Activity_ClassificationAttributes.Add(new Activity_ClassificationAttributes
                                {
                                    Activity_ClassificationAttribute_Id = Guid.NewGuid(),
                                    Activity_Flavour_Id = item.Activity_Flavour_Id,
                                    Activity_FlavourOptions_Id = item.Activity_FlavourOptions_Id,
                                    AttributeType = item.AttributeType,
                                    AttributeSubType = item.AttributeSubType,
                                    AttributeValue = str,
                                    IsActive = true,
                                    InternalOnly = false,
                                    Create_Date = DateTime.Now,
                                    Create_User = item.User
                                });
                            }


                        }
                    }

                    if (context.SaveChanges() == 1)
                    {

                        return new DC_Message
                        {
                            StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully,
                            StatusCode = ReadOnlyMessage.StatusCode.Success
                        };
                    }
                    else
                    {
                        return new DC_Message
                        {
                            StatusMessage = ReadOnlyMessage.strFailed,
                            StatusCode = ReadOnlyMessage.StatusCode.Failed
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating Activity CAs", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region Activity Flavour Options
        public List<DataContracts.Masters.DC_Activity_Flavour_Options> GetActivityFlavourOptions(DataContracts.Masters.DC_Activity_Flavour_Options_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_FlavourOptions
                                 select a;

                    if (RQ.Activity_FlavourOptions_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_FlavourOptions_Id == RQ.Activity_FlavourOptions_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }

                    if (RQ.Activity_OptionCode != null)
                    {
                        search = from a in search
                                 where a.Activity_OptionCode.Trim().TrimStart().ToUpper() == RQ.Activity_OptionCode.Trim().TrimStart().ToUpper()
                                 select a;
                    }


                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.Activity_FlavourName
                                 select new DataContracts.Masters.DC_Activity_Flavour_Options
                                 {
                                     Activity_FlavourOptions_Id = a.Activity_FlavourOptions_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_FlavourName = a.Activity_FlavourName,
                                     Activity_OptionCode = a.Activity_OptionCode,
                                     Activity_OptionName = a.Activity_OptionName,
                                     Activity_DealText = a.Activity_DealText,
                                     Activity_Type = a.Activity_Type,
                                     Create_Date = a.Create_Date,
                                     Status = a.Status,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     Create_User = a.Create_User,
                                     TotalRecords = total,
                                     Activity_OptionDescription = a.Activity_OptionDescription,
                                     Activity_OptionInternalCode = a.TLGXActivityOptionCode
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Flavour Options", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityFlavourOptions(DC_Activity_Flavour_Options RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_Flavour_Id != null)
                    {
                        var res = context.Activity_FlavourOptions.Find(RQ.Activity_FlavourOptions_Id);
                        if (res != null)
                        {
                            res.Activity_FlavourOptions_Id = RQ.Activity_FlavourOptions_Id ?? Guid.Empty;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id ?? Guid.Empty;
                            res.Activity_FlavourName = RQ.Activity_FlavourName;
                            res.Activity_DealText = RQ.Activity_DealText;
                            res.Activity_OptionCode = RQ.Activity_OptionCode;
                            res.Activity_OptionName = RQ.Activity_OptionName;
                            res.Activity_Type = RQ.Activity_Type;
                            res.Status = RQ.Status;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_FlavourOptions obj = new DataLayer.Activity_FlavourOptions();

                        obj.Activity_FlavourOptions_Id = RQ.Activity_FlavourOptions_Id ?? Guid.NewGuid();
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id ?? Guid.NewGuid();
                        obj.Activity_FlavourName = RQ.Activity_FlavourName;
                        obj.Activity_OptionCode = RQ.Activity_OptionCode;
                        obj.Activity_OptionName = RQ.Activity_OptionName;
                        obj.Activity_DealText = RQ.Activity_DealText;
                        obj.Activity_Type = RQ.Activity_Type;
                        obj.Status = RQ.Status;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_FlavourOptions.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Flavour Options", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Deals
        public List<DataContracts.Masters.DC_Activity_Deals> GetActivityDeals(DataContracts.Masters.DC_Activity_Deals_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Deals
                                 select a;
                    if (RQ.Activity_Deals_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Deals_Id == RQ.Activity_Deals_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }

                    if (RQ.DealName != null)
                    {
                        search = from a in search
                                 where a.DealName.Trim().TrimStart().ToUpper() == RQ.DealName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.DealCode != null)
                    {
                        search = from a in search
                                 where a.DealCode.Trim().TrimStart().ToUpper() == RQ.DealCode.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.DealName
                                 select new DataContracts.Masters.DC_Activity_Deals
                                 {
                                     Activity_Deals_Id = a.Activity_Deals_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_Id = a.Activity_Id,
                                     DealName = a.DealName,
                                     DealCode = a.DealCode,
                                     DealText = a.DealText,
                                     Deal_Price = a.Deal_Price,
                                     Deal_Currency = a.Deal_Currency,
                                     Deal_TnC = a.Deal_TnC,
                                     Create_Date = a.Create_Date,
                                     TotalRecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Deals", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }
        public DataContracts.DC_Message AddUpdateActivityDeals(DC_Activity_Deals RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_Deals_Id != null)
                    {
                        var res = context.Activity_Deals.Find(RQ.Activity_Deals_Id);
                        if (res != null)
                        {
                            res.Activity_Deals_Id = RQ.Activity_Deals_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            res.DealName = RQ.DealName;
                            res.DealCode = RQ.DealCode;
                            res.DealText = RQ.DealText;
                            res.Deal_Currency = RQ.Deal_Currency;
                            res.Deal_Price = RQ.Deal_Price;
                            res.Deal_TnC = RQ.Deal_TnC;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_Deals obj = new DataLayer.Activity_Deals();

                        obj.Activity_Deals_Id = RQ.Activity_Deals_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.DealName = RQ.DealName;
                        obj.DealCode = RQ.DealCode;
                        obj.DealText = RQ.DealText;
                        obj.Deal_Currency = RQ.Deal_Currency;
                        obj.Deal_Price = RQ.Deal_Price;
                        obj.Deal_TnC = RQ.Deal_TnC;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_Deals.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Deals", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Prices
        public List<DataContracts.Masters.DC_Activity_Prices> GetActivityPrices(DataContracts.Masters.DC_Activity_Prices_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Prices
                                 select a;

                    var flavourOptions = context.Activity_FlavourOptions.AsNoTracking().Where(w => w.Activity_Flavour_Id == RQ.Activity_Flavour_Id);

                    if (RQ.Activity_Prices_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Prices_Id == RQ.Activity_Prices_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }

                    int total = search.Count();
                    int? skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);
                    if (total <= skip)
                    {
                        int? PageIndex = 0;
                        int? intReminder = total % RQ.PageSize;
                        int? intQuotient = total / RQ.PageSize;

                        if (intReminder > 0 || (intReminder == 0 && intQuotient == 0))
                        {
                            PageIndex = intQuotient;
                        }
                        else if (intReminder == 0 && intQuotient > 0)
                        {
                            PageIndex = intQuotient - 1;
                        }

                        skip = RQ.PageSize * PageIndex;
                    }


                    var result = from a in search
                                 join fo in flavourOptions on a.Activity_FlavourOptions_Id equals fo.Activity_FlavourOptions_Id into folj
                                 from foljd in folj.DefaultIfEmpty()
                                 orderby a.PriceCode
                                 select new DataContracts.Masters.DC_Activity_Prices
                                 {
                                     Activity_Prices_Id = a.Activity_Prices_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_Id = a.Activity_Id,
                                     PriceBasis = a.PriceBasis,
                                     PriceCode = a.PriceCode,
                                     PriceCurrency = a.PriceCurrency,
                                     Price = a.Price,
                                     Price_For = a.Price_For,
                                     Price_Type = a.Price_Type,
                                     Price_OptionCode = a.Price_OptionCode,
                                     Totalrecords = total,
                                     Create_Date = a.Create_Date,
                                     IsActive = a.IsActive,
                                     FromPax = a.FromPax,
                                     ToPax = a.ToPax,
                                     Market = a.Market,
                                     PersonType = a.PersonType,
                                     Price_ValidFrom = a.Price_ValidFrom,
                                     Price_ValidTo = a.Price_ValidTo,
                                     Price_InternalOptionCode = foljd.TLGXActivityOptionCode,
                                     AgeFrom = a.AgeFrom,
                                     AgeTo = a.AgeTo
                                 };
                    return result.Skip(skip ?? 0).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity prices", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }
        public DataContracts.DC_Message AddUpdateActivityPrices(DC_Activity_Prices RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_Prices_Id != null)
                    {
                        var res = context.Activity_Prices.Find(RQ.Activity_Prices_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            res.Activity_Prices_Id = RQ.Activity_Prices_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            res.PriceBasis = RQ.PriceBasis;
                            res.PriceCode = RQ.PriceCode;
                            res.PriceCurrency = RQ.PriceCurrency;
                            res.Price = RQ.Price;
                            res.Price_For = RQ.Price_For;
                            res.Price_Type = RQ.Price_Type;
                            res.AgeFrom = RQ.AgeFrom;
                            res.AgeTo = RQ.AgeTo;
                            res.PersonType = RQ.Price_Type;
                            res.FromPax = RQ.FromPax;
                            res.ToPax = RQ.ToPax;
                            res.Market = RQ.Market;
                            res.Price_OptionCode = RQ.Price_OptionCode;
                            res.Edit_Date = DateTime.Now;
                            res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_Prices obj = new DataLayer.Activity_Prices();
                        obj.Activity_Prices_Id = RQ.Activity_Prices_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.PriceBasis = RQ.PriceBasis;
                        obj.PriceCode = RQ.PriceCode;
                        obj.PriceCurrency = RQ.PriceCurrency;
                        obj.Price = RQ.Price;
                        obj.Price_For = RQ.Price_For;
                        obj.Price_Type = RQ.Price_Type;
                        obj.Price_OptionCode = RQ.Price_OptionCode;
                        obj.IsActive = RQ.IsActive;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_Prices.Add(obj);

                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Prices", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Supplier Product mapping
        public List<DataContracts.Masters.DC_Activity_SupplierProductMapping> GetActSupplierProdMapping(DataContracts.Masters.DC_Activity_SupplierProductMapping_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_SupplierProductMapping
                                 join af in context.Activity_Flavour on a.Activity_ID equals af.Activity_Flavour_Id
                                 select a;

                    if (RQ.ActivitySupplierProductMapping_Id != null)
                    {
                        search = from a in search
                                 where a.ActivitySupplierProductMapping_Id == RQ.ActivitySupplierProductMapping_Id
                                 select a;
                    }
                    if (RQ.Activity_ID != null)
                    {
                        search = from a in search
                                 where a.Activity_ID == RQ.Activity_ID
                                 select a;
                    }

                    if (RQ.Supplier_ID != null)
                    {
                        search = from a in search
                                 where a.Supplier_ID == RQ.Supplier_ID
                                 select a;
                    }
                    if (RQ.SuplierProductCode != null)
                    {
                        search = from a in search
                                 where a.SuplierProductCode.Trim().TrimStart().ToUpper() == RQ.SuplierProductCode.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierCityName != null)
                    {
                        search = from a in search
                                 where a.SupplierCityName.Trim().TrimStart().ToUpper() == RQ.SupplierCityName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierCountryName != null)
                    {
                        search = from a in search
                                 where a.SupplierCountryName.Trim().TrimStart().ToUpper() == RQ.SupplierCountryName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierCode != null)
                    {
                        search = from a in search
                                 where a.SupplierCode.Trim().TrimStart().ToUpper() == RQ.SupplierCode.Trim().TrimStart().ToUpper()
                                 select a;
                    }

                    if (RQ.SupplierName != null)
                    {
                        search = from a in search
                                 where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierProductType != null)
                    {
                        search = from a in search
                                 where a.SupplierProductType.Trim().TrimStart().ToUpper() == RQ.SupplierProductType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierType != null)
                    {
                        search = from a in search
                                 where a.SupplierType.Trim().TrimStart().ToUpper() == RQ.SupplierType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.SupplierName
                                 select new DataContracts.Masters.DC_Activity_SupplierProductMapping
                                 {
                                     ActivitySupplierProductMapping_Id = a.ActivitySupplierProductMapping_Id,
                                     Activity_ID = a.Activity_ID,
                                     AdditionalInformation = a.AdditionalInformation,
                                     Address = a.Address,
                                     SupplierName = a.SupplierName,
                                     Supplier_ID = a.Supplier_ID,
                                     SupplierType = a.SupplierType,
                                     Area = a.Area,
                                     BlockOutDateFrom = a.BlockOutDateFrom,
                                     BlockOutDateTo = a.BlockOutDateTo,
                                     Conditions = a.Conditions,
                                     Create_Date = a.Create_Date,
                                     Currency = a.Currency,
                                     DateFrom = a.DateFrom,
                                     DateTo = a.DateTo,
                                     SupplierCityIATACode = a.SupplierCityIATACode,
                                     DayPattern = a.DayPattern,
                                     DepartureDate = a.DepartureDate,
                                     DeparturePoint = a.DeparturePoint,
                                     DepartureTime = a.DepartureTime,
                                     Distance = a.Distance,
                                     Duration = a.Duration,
                                     DurationLength = a.DurationLength,
                                     Exclusions = a.Exclusions,
                                     ImgURL = a.ImgURL,
                                     Inclusions = a.Inclusions,
                                     Introduction = a.Introduction,
                                     IsActive = a.IsActive,
                                     Latitude = a.Latitude,
                                     Longitude = a.Longitude,
                                     Location = a.Location,
                                     MapID = a.MapID,
                                     MappingStatus = a.MappingStatus,
                                     OptionCode = a.OptionCode,
                                     TotalActivities = a.TotalActivities,
                                     OptionDescription = a.OptionDescription,
                                     OptionTitle = a.OptionTitle,
                                     PassengerNumbers = a.PassengerNumbers,
                                     PhysicalIntensity = a.PhysicalIntensity,
                                     Price = a.Price,
                                     ProductDescription = a.ProductDescription,
                                     ProductValidFor = a.ProductValidFor,
                                     Rating = a.Rating,
                                     Recommonded = a.Recommonded,
                                     Session = a.Session,
                                     Specials = a.Specials,
                                     SuplierProductCode = a.SuplierProductCode,
                                     SupplierCityName = a.SupplierCityName,
                                     SupplierCityCode = a.SupplierCityCode,
                                     SupplierCountryName = a.SupplierCountryName,
                                     SupplierCountryCode = a.SupplierCountryCode,
                                     SupplierCode = a.SupplierCode,
                                     SupplierDataLangaugeCode = a.SupplierLocationId,
                                     SupplierLocationId = a.SupplierLocationId,
                                     SupplierLocationName = a.SupplierLocationName,
                                     SupplierProductName = a.SupplierProductName,
                                     SupplierProductType = a.SupplierProductType,
                                     SupplierStateCode = a.SupplierStateCode,
                                     SupplierStateName = a.SupplierStateName,
                                     SupplierTourType = a.SupplierTourType,
                                     Theme = a.Theme,
                                     TicketingDetails = a.TicketingDetails,
                                     Timing = a.Timing,
                                     TourActivityLangauageCode = a.TourActivityLangauageCode,
                                     TourActivityLanguage = a.TourActivityLanguage,
                                     TotalRecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Supplier Produvt Mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }
        public DataContracts.DC_Message AddUpdateActSupplierProdMapping(DC_Activity_SupplierProductMapping RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.ActivitySupplierProductMapping_Id != null)
                    {
                        var res = context.Activity_SupplierProductMapping.Find(RQ.ActivitySupplierProductMapping_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.ActivitySupplierProductMapping_Id = RQ.ActivitySupplierProductMapping_Id ?? Guid.Empty;
                                res.Activity_ID = RQ.Activity_ID;
                                res.AdditionalInformation = RQ.AdditionalInformation;
                                res.Address = RQ.Address;
                                res.SupplierName = RQ.SupplierName;
                                res.Supplier_ID = RQ.Supplier_ID.Value;
                                res.SupplierType = RQ.SupplierType;
                                res.Area = RQ.Area;
                                res.BlockOutDateFrom = RQ.BlockOutDateFrom;
                                res.BlockOutDateTo = RQ.BlockOutDateTo;
                                res.Conditions = RQ.Conditions;
                                res.Currency = RQ.Currency;
                                res.DateFrom = RQ.DateFrom;
                                res.DateTo = RQ.DateTo;
                                res.SupplierCityIATACode = RQ.SupplierCityIATACode;
                                res.DayPattern = RQ.DayPattern;
                                res.DepartureDate = RQ.DepartureDate;
                                res.DeparturePoint = RQ.DeparturePoint;
                                res.DepartureTime = RQ.DepartureTime;
                                res.Distance = RQ.Distance;
                                res.Duration = RQ.Duration;
                                res.DurationLength = RQ.DurationLength;
                                res.Exclusions = RQ.Exclusions;
                                res.ImgURL = RQ.ImgURL;
                                res.Inclusions = RQ.Inclusions;
                                res.Introduction = RQ.Introduction;
                                res.Latitude = RQ.Latitude;
                                res.Longitude = RQ.Longitude;
                                res.Location = RQ.Location;
                                res.MapID = RQ.MapID;
                                res.MappingStatus = RQ.MappingStatus;
                                res.OptionCode = RQ.OptionCode;
                                res.TotalActivities = RQ.TotalActivities;
                                res.OptionDescription = RQ.OptionDescription;
                                res.OptionTitle = RQ.OptionTitle;
                                res.PassengerNumbers = RQ.PassengerNumbers;
                                res.PhysicalIntensity = RQ.PhysicalIntensity;
                                res.Price = RQ.Price;
                                res.ProductDescription = RQ.ProductDescription;
                                res.ProductValidFor = RQ.ProductValidFor;
                                res.Rating = RQ.Rating;
                                res.Recommonded = RQ.Recommonded;
                                res.Session = RQ.Session;
                                res.Specials = RQ.Specials;
                                res.SuplierProductCode = RQ.SuplierProductCode;
                                res.SupplierCityName = RQ.SupplierCityName;
                                res.SupplierCityCode = RQ.SupplierCityCode;
                                res.SupplierCountryName = RQ.SupplierCountryName;
                                res.SupplierCountryCode = RQ.SupplierCountryCode;
                                res.SupplierCode = RQ.SupplierCode;
                                res.SupplierDataLangaugeCode = RQ.SupplierLocationId;
                                res.SupplierLocationId = RQ.SupplierLocationId;
                                res.SupplierLocationName = RQ.SupplierLocationName;
                                res.SupplierProductName = RQ.SupplierProductName;
                                res.SupplierProductType = RQ.SupplierProductType;
                                res.SupplierStateCode = RQ.SupplierStateCode;
                                res.SupplierStateName = RQ.SupplierStateName;
                                res.SupplierTourType = RQ.SupplierTourType;
                                res.Theme = RQ.Theme;
                                res.TicketingDetails = RQ.TicketingDetails;
                                res.Timing = RQ.Timing;
                                res.TourActivityLangauageCode = RQ.TourActivityLangauageCode;
                                res.TourActivityLanguage = RQ.TourActivityLanguage;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }

                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_SupplierProductMapping obj = new DataLayer.Activity_SupplierProductMapping();
                        obj.ActivitySupplierProductMapping_Id = RQ.ActivitySupplierProductMapping_Id ?? Guid.NewGuid();
                        obj.Activity_ID = RQ.Activity_ID;
                        obj.AdditionalInformation = RQ.AdditionalInformation;
                        obj.Address = RQ.Address;
                        obj.SupplierName = RQ.SupplierName;
                        obj.Supplier_ID = RQ.Supplier_ID.Value;
                        obj.SupplierType = RQ.SupplierType;
                        obj.Area = RQ.Area;
                        obj.BlockOutDateFrom = RQ.BlockOutDateFrom;
                        obj.BlockOutDateTo = RQ.BlockOutDateTo;
                        obj.Conditions = RQ.Conditions;
                        obj.Currency = RQ.Currency;
                        obj.DateFrom = RQ.DateFrom;
                        obj.DateTo = RQ.DateTo;
                        obj.SupplierCityIATACode = RQ.SupplierCityIATACode;
                        obj.DayPattern = RQ.DayPattern;
                        obj.DepartureDate = RQ.DepartureDate;
                        obj.DeparturePoint = RQ.DeparturePoint;
                        obj.DepartureTime = RQ.DepartureTime;
                        obj.Distance = RQ.Distance;
                        obj.Duration = RQ.Duration;
                        obj.DurationLength = RQ.DurationLength;
                        obj.Exclusions = RQ.Exclusions;
                        obj.ImgURL = RQ.ImgURL;
                        obj.Inclusions = RQ.Inclusions;
                        obj.Introduction = RQ.Introduction;
                        obj.Latitude = RQ.Latitude;
                        obj.Longitude = RQ.Longitude;
                        obj.Location = RQ.Location;
                        obj.MapID = RQ.MapID;
                        obj.MappingStatus = RQ.MappingStatus;
                        obj.OptionCode = RQ.OptionCode;
                        obj.TotalActivities = RQ.TotalActivities;
                        obj.OptionDescription = RQ.OptionDescription;
                        obj.OptionTitle = RQ.OptionTitle;
                        obj.PassengerNumbers = RQ.PassengerNumbers;
                        obj.PhysicalIntensity = RQ.PhysicalIntensity;
                        obj.Price = RQ.Price;
                        obj.ProductDescription = RQ.ProductDescription;
                        obj.ProductValidFor = RQ.ProductValidFor;
                        obj.Rating = RQ.Rating;
                        obj.Recommonded = RQ.Recommonded;
                        obj.Session = RQ.Session;
                        obj.Specials = RQ.Specials;
                        obj.SuplierProductCode = RQ.SuplierProductCode;
                        obj.SupplierCityName = RQ.SupplierCityName;
                        obj.SupplierCityCode = RQ.SupplierCityCode;
                        obj.SupplierCountryName = RQ.SupplierCountryName;
                        obj.SupplierCountryCode = RQ.SupplierCountryCode;
                        obj.SupplierCode = RQ.SupplierCode;
                        obj.SupplierDataLangaugeCode = RQ.SupplierLocationId;
                        obj.SupplierLocationId = RQ.SupplierLocationId;
                        obj.SupplierLocationName = RQ.SupplierLocationName;
                        obj.SupplierProductName = RQ.SupplierProductName;
                        obj.SupplierProductType = RQ.SupplierProductType;
                        obj.SupplierStateCode = RQ.SupplierStateCode;
                        obj.SupplierStateName = RQ.SupplierStateName;
                        obj.SupplierTourType = RQ.SupplierTourType;
                        obj.Theme = RQ.Theme;
                        obj.TicketingDetails = RQ.TicketingDetails;
                        obj.Timing = RQ.Timing;
                        obj.TourActivityLangauageCode = RQ.TourActivityLangauageCode;
                        obj.TourActivityLanguage = RQ.TourActivityLanguage;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;

                        context.Activity_SupplierProductMapping.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Supplier Product Mapping", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Reviews And Scores
        public List<DataContracts.Masters.DC_Activity_ReviewsAndScores> GetActReviewsAndScores(DataContracts.Masters.DC_Activity_ReviewsAndScores_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_ReviewsAndScores
                                 select a;
                    if (RQ.Activity_ReviewsAndScores_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_ReviewsAndScores_Id == RQ.Activity_ReviewsAndScores_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Review_Title != null)
                    {
                        search = from a in search
                                 where a.Review_Title.Trim().TrimStart().ToUpper() == RQ.Review_Title.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Review_Author != null)
                    {
                        search = from a in search
                                 where a.Review_Author.Trim().TrimStart().ToUpper() == RQ.Review_Author.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.Review_Status != null)
                    {
                        search = from a in search
                                 where a.Review_Status.Trim().TrimStart().ToUpper() == RQ.Review_Status.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.Create_Date
                                 select new DataContracts.Masters.DC_Activity_ReviewsAndScores
                                 {
                                     Activity_ReviewsAndScores_Id = a.Activity_ReviewsAndScores_Id,
                                     Activity_Flavour_Id = a.Activity_ReviewsAndScores_Id,
                                     Activity_Id = a.Activity_Id,
                                     Review_Author = a.Review_Author,
                                     IsCustomerReview = a.IsCustomerReview,
                                     IsActive = a.IsActive,
                                     Review_Description = a.Review_Description,
                                     Review_PostedDate = a.Review_PostedDate,
                                     Review_Score = a.Review_Score,
                                     Review_Title = a.Review_Title,
                                     Review_Type = a.Review_Type,
                                     Review_Source = a.Review_Source,
                                     Create_Date = a.Create_Date,
                                     Totalrecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Reviews and scores", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActReviewsNScores(DC_Activity_ReviewsAndScores RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_ReviewsAndScores_Id != null)
                    {
                        var res = context.Activity_ReviewsAndScores.Find(RQ.Activity_ReviewsAndScores_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_ReviewsAndScores_Id = RQ.Activity_ReviewsAndScores_Id ?? Guid.Empty;
                                res.Activity_Id = RQ.Activity_Id;
                                res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                                res.IsCustomerReview = RQ.IsCustomerReview;
                                res.Review_Author = RQ.Review_Author;
                                res.Review_Description = RQ.Review_Description;
                                res.Review_PostedDate = RQ.Review_PostedDate;
                                res.Review_Author = RQ.Review_Author;
                                res.Review_Description = RQ.Review_Description;
                                res.Review_Title = RQ.Review_Title;
                                res.Review_Status = RQ.Review_Status;
                                res.Review_Type = RQ.Review_Type;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }

                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_ReviewsAndScores obj = new DataLayer.Activity_ReviewsAndScores();
                        obj.Activity_ReviewsAndScores_Id = RQ.Activity_ReviewsAndScores_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.IsCustomerReview = RQ.IsCustomerReview;
                        obj.Review_Author = RQ.Review_Author;
                        obj.Review_Description = RQ.Review_Description;
                        obj.Review_PostedDate = RQ.Review_PostedDate;
                        obj.Review_Author = RQ.Review_Author;
                        obj.Review_Description = RQ.Review_Description;
                        obj.Review_Title = RQ.Review_Title;
                        obj.Review_Status = RQ.Review_Status;
                        obj.Review_Type = RQ.Review_Type;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_ReviewsAndScores.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Reviews and Scores", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Supplier Product mapping_CA
        public List<DataContracts.Masters.DC_Activity_SupplierProductMapping_CA> GetActSupplierProdMapping_CA(DataContracts.Masters.DC_Activity_SupplierProductMapping_CA_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_SupplierProductMapping_CA
                                 select a;
                    if (RQ.Activity_SupplierProductMapping_CA_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_SupplierProductMapping_CA_Id == RQ.Activity_SupplierProductMapping_CA_Id
                                 select a;
                    }
                    if (RQ.Supplier_ID != null)
                    {
                        search = from a in search
                                 where a.Supplier_ID == RQ.Supplier_ID
                                 select a;
                    }
                    if (RQ.SupplierCode != null)
                    {
                        search = from a in search
                                 where a.SupplierCode.Trim().TrimStart().ToUpper() == RQ.SupplierCode.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.SupplierName != null)
                    {
                        search = from a in search
                                 where a.SupplierName.Trim().TrimStart().ToUpper() == RQ.SupplierName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.AttributeType != null)
                    {
                        search = from a in search
                                 where a.AttributeType.Trim().TrimStart().ToUpper() == RQ.AttributeType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    if (RQ.AttributeSubType != null)
                    {
                        search = from a in search
                                 where a.AttributeSubType.Trim().TrimStart().ToUpper() == RQ.AttributeSubType.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 select new DataContracts.Masters.DC_Activity_SupplierProductMapping_CA
                                 {
                                     Activity_SupplierProductMapping_CA_Id = a.Activity_SupplierProductMapping_CA_Id,
                                     Supplier_ID = a.Supplier_ID,
                                     AttributeSubType = a.AttributeSubType,
                                     AttributeType = a.AttributeType,
                                     AttributeValue = a.AttributeValue,
                                     SupplierCode = a.SupplierCode,
                                     SupplierName = a.SupplierName,
                                     SuplierProductCode = a.SuplierProductCode,
                                     SupplierProductName = a.SupplierProductName,
                                     IsActive = a.IsActive,
                                     Create_Date = a.Create_Date,
                                     Totalrecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity SupplierProductMapping_CA", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActSupplierProdMapping_CA(DC_Activity_SupplierProductMapping_CA RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    if (RQ.Activity_SupplierProductMapping_CA_Id != null)
                    {
                        var res = context.Activity_SupplierProductMapping_CA.Find(RQ.Activity_SupplierProductMapping_CA_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_SupplierProductMapping_CA_Id = RQ.Activity_SupplierProductMapping_CA_Id ?? Guid.Empty;
                                res.Supplier_ID = RQ.Supplier_ID;
                                res.AttributeSubType = RQ.AttributeSubType;
                                res.AttributeType = RQ.AttributeType;
                                res.AttributeValue = RQ.AttributeValue;
                                res.SupplierCode = RQ.SupplierCode;
                                res.SupplierName = RQ.SupplierName;
                                res.SuplierProductCode = RQ.SuplierProductCode;
                                res.SupplierProductName = RQ.SupplierProductName;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }

                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_SupplierProductMapping_CA obj = new DataLayer.Activity_SupplierProductMapping_CA();
                        obj.Activity_SupplierProductMapping_CA_Id = RQ.Activity_SupplierProductMapping_CA_Id ?? Guid.Empty;
                        obj.Supplier_ID = RQ.Supplier_ID;
                        obj.AttributeSubType = RQ.AttributeSubType;
                        obj.AttributeType = RQ.AttributeType;
                        obj.AttributeValue = RQ.AttributeValue;
                        obj.SupplierCode = RQ.SupplierCode;
                        obj.SupplierName = RQ.SupplierName;
                        obj.SuplierProductCode = RQ.SuplierProductCode;
                        obj.SupplierProductName = RQ.SupplierProductName;
                        obj.IsActive = RQ.IsActive;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_SupplierProductMapping_CA.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity SupplierProductMapping_CA", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity Policy
        public List<DataContracts.Masters.DC_Activity_Policy> GetActivityPolicy(DataContracts.Masters.DC_Activity_Policy_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Policy
                                 select a;
                    if (RQ.Activity_Policy_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Policy_Id == RQ.Activity_Policy_Id
                                 select a;
                    }
                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Flavour_Id == RQ.Activity_Flavour_Id
                                 select a;
                    }
                    if (RQ.Activity_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Id
                                 select a;
                    }
                    if (RQ.Legacy_Product_ID != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_ID == RQ.Legacy_Product_ID
                                 select a;
                    }
                    if (RQ.PolicyName != null)
                    {
                        search = from a in search
                                 where a.PolicyName.Trim().TrimStart().ToUpper() == RQ.PolicyName.Trim().TrimStart().ToUpper()
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.Create_Date
                                 select new DataContracts.Masters.DC_Activity_Policy
                                 {
                                     Activity_Policy_Id = a.Activity_Policy_Id,
                                     Activity_Flavour_Id = a.Activity_Flavour_Id,
                                     Activity_Id = a.Activity_Id,
                                     IsActive = a.IsActive,
                                     AllowedYN = a.AllowedYN,
                                     PolicyName = a.PolicyName,
                                     Policy_Type = a.Policy_Type,
                                     PolicyDescription = a.PolicyDescription,
                                     Legacy_Product_ID = a.Legacy_Product_ID,
                                     Create_Date = a.Create_Date,
                                     Totalrecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity SupplierProductMapping_CA", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityPolicy(DataContracts.Masters.DC_Activity_Policy RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (RQ.Activity_Policy_Id != null)
                    {
                        var res = context.Activity_Policy.Find(RQ.Activity_Policy_Id);
                        if (res != null)
                        {
                            if ((RQ.IsActive) != (res.IsActive ?? true))
                            {
                                res.IsActive = RQ.IsActive;
                                res.Edit_Date = RQ.Edit_Date;
                                res.Edit_User = RQ.Edit_User;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUnDeleted;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                            else
                            {
                                res.Activity_Policy_Id = RQ.Activity_Policy_Id ?? Guid.Empty;
                                res.Activity_Id = RQ.Activity_Id;
                                res.AllowedYN = RQ.AllowedYN;
                                res.PolicyName = RQ.PolicyName;
                                res.Policy_Type = RQ.Policy_Type;
                                res.PolicyDescription = RQ.PolicyDescription;
                                res.Legacy_Product_ID = RQ.Legacy_Product_ID;
                                res.Edit_Date = DateTime.Now;
                                res.Edit_User = System.Web.HttpContext.Current.User.Identity.Name;
                                if (context.SaveChanges() == 1)
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                                }
                                else
                                {
                                    _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                                }
                            }
                        }
                        else IsInsert = true;
                    }
                    else IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_Policy obj = new DataLayer.Activity_Policy();
                        obj.Activity_Policy_Id = RQ.Activity_Policy_Id ?? Guid.Empty;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.AllowedYN = RQ.AllowedYN;
                        obj.PolicyName = RQ.PolicyName;
                        obj.Policy_Type = RQ.Policy_Type;
                        obj.PolicyDescription = RQ.PolicyDescription;
                        obj.Legacy_Product_ID = RQ.Legacy_Product_ID;
                        obj.IsActive = RQ.IsActive;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_Policy.Add(obj);
                        if (context.SaveChanges() == 1)
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strAddedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                    }
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity SupplierProductMapping_CA", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Activity DaysOfWeek
        public List<DataContracts.Masters.DC_Activity_OperatingDays> GetActivityOperatingDays(Guid Activity_Flavour_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    List<DataContracts.Masters.DC_Activity_OperatingDays> returnData = new List<DataContracts.Masters.DC_Activity_OperatingDays>();

                    var DaysOfOperation = context.Activity_DaysOfOperation.AsNoTracking().Where(w => w.Activity_Flavor_ID == Activity_Flavour_Id && w.IsOperatingDays == true).Select(s => s);
                    if (DaysOfOperation.Count() > 0)
                    {
                        foreach (var perdayofOps in DaysOfOperation)
                        {
                            DataContracts.Masters.DC_Activity_OperatingDays OperatingDay = new DC_Activity_OperatingDays
                            {
                                Activity_DaysOfOperation_Id = perdayofOps.Activity_DaysOfOperation_Id,
                                Activity_Flavor_ID = perdayofOps.Activity_Flavor_ID,
                                CreateUser = perdayofOps.CreateUser,
                                EditUser = perdayofOps.EditUser,
                                EndDate = perdayofOps.ToDate,
                                FromDate = perdayofOps.FromDate,
                                IsActive = perdayofOps.IsActive,
                                IsOperatingDays = perdayofOps.IsOperatingDays,
                                DaysOfWeek = (from s in context.Activity_DaysOfWeek.AsNoTracking()
                                              where s.Activity_DaysOfOperation_Id == perdayofOps.Activity_DaysOfOperation_Id && s.Activity_Flavor_ID == Activity_Flavour_Id
                                              select new DC_Activity_DaysOfWeek
                                              {
                                                  Activity_DaysOfOperation_Id = s.Activity_DaysOfOperation_Id,
                                                  Activity_Flavor_ID = s.Activity_Flavor_ID,
                                                  Activity_DaysOfWeek_ID = s.Activity_DaysOfWeek_ID,
                                                  Duration = s.Duration,
                                                  EndTime = s.EndTime,
                                                  Fri = s.Fri ?? false,
                                                  IsActive = s.IsActive ?? false,
                                                  Mon = s.Mon ?? false,
                                                  Sat = s.Sat ?? false,
                                                  Session = s.Session,
                                                  StartTime = s.StartTime,
                                                  Sun = s.Sun ?? false,
                                                  SupplierDuration = s.SupplierDuration,
                                                  SupplierEndTime = s.SupplierEndTime,
                                                  SupplierFrequency = s.SupplierFrequency,
                                                  SupplierSession = s.SupplierSession,
                                                  SupplierStartTime = s.SupplierStartTime,
                                                  Thur = s.Thur ?? false,
                                                  Tues = s.Tues ?? false,
                                                  Wed = s.Wed ?? false,
                                                  DurationType = s.DurationType
                                              }).ToList()
                            };
                            returnData.Add(OperatingDay);
                        }
                    }

                    var DaysOfWeekNotLinkedToOperatingDays = context.Activity_DaysOfWeek.AsNoTracking().Where(w => w.Activity_Flavor_ID == Activity_Flavour_Id && w.Activity_DaysOfOperation_Id == null).Count();
                    if (DaysOfWeekNotLinkedToOperatingDays > 0)
                    {
                        Guid DaysOfOperation_Id = Guid.NewGuid();

                        DataContracts.Masters.DC_Activity_OperatingDays OperatingDay = new DC_Activity_OperatingDays
                        {
                            Activity_DaysOfOperation_Id = DaysOfOperation_Id,
                            Activity_Flavor_ID = Activity_Flavour_Id,
                            IsActive = true,
                            IsOperatingDays = true,
                            DaysOfWeek = (from s in context.Activity_DaysOfWeek.AsNoTracking()
                                          where s.Activity_DaysOfOperation_Id == null && s.Activity_Flavor_ID == Activity_Flavour_Id
                                          select new DC_Activity_DaysOfWeek
                                          {
                                              Activity_DaysOfOperation_Id = s.Activity_DaysOfOperation_Id,
                                              Activity_Flavor_ID = s.Activity_Flavor_ID,
                                              Activity_DaysOfWeek_ID = s.Activity_DaysOfWeek_ID,
                                              Duration = s.Duration,
                                              EndTime = s.EndTime,
                                              Fri = s.Fri ?? false,
                                              IsActive = s.IsActive ?? false,
                                              Mon = s.Mon ?? false,
                                              Sat = s.Sat ?? false,
                                              Session = s.Session,
                                              StartTime = s.StartTime,
                                              Sun = s.Sun ?? false,
                                              SupplierDuration = s.SupplierDuration,
                                              SupplierEndTime = s.SupplierEndTime,
                                              SupplierFrequency = s.SupplierFrequency,
                                              SupplierSession = s.SupplierSession,
                                              SupplierStartTime = s.SupplierStartTime,
                                              Thur = s.Thur ?? false,
                                              Tues = s.Tues ?? false,
                                              Wed = s.Wed ?? false,
                                              DurationType = s.DurationType
                                          }).ToList()
                        };
                        returnData.Add(OperatingDay);
                    }

                    return returnData;
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity OperatingDays", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityOperatingDays(List<DataContracts.Masters.DC_Activity_OperatingDays> RQ)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                Guid? Activity_Flavour_Id = null;

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var OD in RQ)
                    {
                        if (Activity_Flavour_Id == null)
                        {
                            Activity_Flavour_Id = OD.Activity_Flavor_ID;
                        }

                        //Check Activity_DaysOfOperation_Id
                        if (OD.Activity_DaysOfOperation_Id != Guid.Empty)
                        {
                            var searchOD = context.Activity_DaysOfOperation.Find(OD.Activity_DaysOfOperation_Id);
                            if (searchOD == null)
                            {
                                context.Activity_DaysOfOperation.Add(new Activity_DaysOfOperation
                                {
                                    Activity_DaysOfOperation_Id = OD.Activity_DaysOfOperation_Id,
                                    Activity_Flavor_ID = OD.Activity_Flavor_ID,
                                    FromDate = OD.FromDate,
                                    ToDate = OD.EndDate,
                                    CreateDate = DateTime.Now,
                                    CreateUser = OD.EditUser,
                                    IsActive = true,
                                    IsOperatingDays = true
                                });
                            }
                            else
                            {
                                searchOD.FromDate = OD.FromDate;
                                searchOD.ToDate = OD.EndDate;
                                searchOD.EditDate = DateTime.Now;
                                searchOD.EditUser = OD.EditUser;
                                searchOD.IsActive = true;
                                searchOD.IsOperatingDays = true;
                            }

                            foreach (var DOW in OD.DaysOfWeek)
                            {
                                var searchDOW = context.Activity_DaysOfWeek.Find(DOW.Activity_DaysOfWeek_ID);
                                if (searchDOW == null)
                                {
                                    context.Activity_DaysOfWeek.Add(new Activity_DaysOfWeek
                                    {
                                        Activity_DaysOfOperation_Id = DOW.Activity_DaysOfOperation_Id,
                                        Activity_DaysOfWeek_ID = DOW.Activity_DaysOfWeek_ID,
                                        CreateDate = DateTime.Now,
                                        CreateUser = OD.EditUser,
                                        Activity_Flavor_ID = DOW.Activity_Flavor_ID,
                                        Duration = DOW.Duration,
                                        EndTime = DOW.EndTime,
                                        Fri = DOW.Fri,
                                        IsActive = true,
                                        Mon = DOW.Mon,
                                        Sat = DOW.Sat,
                                        Session = DOW.Session,
                                        Sun = DOW.Sun,
                                        StartTime = DOW.StartTime,
                                        Thur = DOW.Thur,
                                        Tues = DOW.Tues,
                                        Wed = DOW.Wed,
                                        DurationType = DOW.DurationType
                                    });
                                }
                                else
                                {
                                    searchDOW.Activity_DaysOfOperation_Id = DOW.Activity_DaysOfOperation_Id;
                                    searchDOW.Activity_DaysOfWeek_ID = DOW.Activity_DaysOfWeek_ID;
                                    searchDOW.EditDate = DateTime.Now;
                                    searchDOW.EditUser = OD.EditUser;
                                    searchDOW.Activity_Flavor_ID = DOW.Activity_Flavor_ID;
                                    searchDOW.Duration = DOW.Duration;
                                    searchDOW.EndTime = DOW.EndTime;
                                    searchDOW.Fri = DOW.Fri;
                                    searchDOW.IsActive = true;
                                    searchDOW.Mon = DOW.Mon;
                                    searchDOW.Sat = DOW.Sat;
                                    searchDOW.Session = DOW.Session;
                                    searchDOW.Sun = DOW.Sun;
                                    searchDOW.StartTime = DOW.StartTime;
                                    searchDOW.Thur = DOW.Thur;
                                    searchDOW.Tues = DOW.Tues;
                                    searchDOW.Wed = DOW.Wed;
                                    searchDOW.DurationType = DOW.DurationType;
                                }
                            }

                            context.SaveChanges();
                        }
                    }

                    if (Activity_Flavour_Id != null)
                    {
                        var OD_VALID = (from a in RQ select a.Activity_DaysOfOperation_Id).Distinct();
                        var OD_TO_REMOVE = context.Activity_DaysOfOperation.Where(p => p.Activity_Flavor_ID == Activity_Flavour_Id && p.IsOperatingDays == true && !OD_VALID.Any(p2 => p2 == p.Activity_DaysOfOperation_Id));
                        context.Activity_DaysOfOperation.RemoveRange(OD_TO_REMOVE);

                        //var DOW_TO_REMOVE_WHERE_OD_REMOVED = context.Activity_DaysOfWeek.Where(p => p.Activity_Flavor_ID == Activity_Flavour_Id && !OD_VALID.Any(p2 => p2 == p.Activity_DaysOfOperation_Id));
                        //context.Activity_DaysOfWeek.RemoveRange(DOW_TO_REMOVE_WHERE_OD_REMOVED);

                        var DOW_VALID = RQ.SelectMany(d => d.DaysOfWeek).Select(s => s.Activity_DaysOfWeek_ID);
                        var DOW_TO_REMOVE = context.Activity_DaysOfWeek.Where(p => p.Activity_Flavor_ID == Activity_Flavour_Id && !DOW_VALID.Any(p2 => p2 == p.Activity_DaysOfWeek_ID));
                        context.Activity_DaysOfWeek.RemoveRange(DOW_TO_REMOVE);

                        context.SaveChanges();

                        if (Activity_Flavour_Id != Guid.Empty)
                        {
                            DL_MongoPush _obj = new DL_MongoPush();
                            _obj.SyncActivityFlavour(Activity_Flavour_Id ?? Guid.Empty);
                        }
                    }

                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Saved Successfully" };
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating Activity Days Of Week", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public List<DataContracts.Masters.DC_Activity_OperatingDays> GetActivityNonOperatingDays(Guid Activity_Flavour_Id, int? pageSize, int? pageNo)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var totalcount = context.Activity_DaysOfOperation.Where(x => x.Activity_Flavor_ID == Activity_Flavour_Id && x.IsOperatingDays == false).Count();
                    var DaysOfNonOperation = (from nod in context.Activity_DaysOfOperation.AsNoTracking()
                                              where nod.Activity_Flavor_ID == Activity_Flavour_Id && nod.IsOperatingDays == false
                                              select new DC_Activity_OperatingDays
                                              {
                                                  Activity_DaysOfOperation_Id = nod.Activity_DaysOfOperation_Id,
                                                  Activity_Flavor_ID = nod.Activity_Flavor_ID,
                                                  CreateUser = nod.CreateUser,
                                                  EditUser = nod.EditUser,
                                                  EndDate = nod.ToDate,
                                                  FromDate = nod.FromDate,
                                                  IsActive = nod.IsActive,
                                                  IsOperatingDays = nod.IsOperatingDays,
                                                  TotalRecords = totalcount
                                              }).OrderByDescending(x => x.FromDate).ToList();


                    int? skip = (pageNo ?? 0) * (pageSize ?? 0);
                    if (totalcount <= skip)
                    {
                        int? PageIndex = 0;
                        int? intReminder = totalcount % pageSize;
                        int? intQuotient = totalcount / pageSize;

                        if (intReminder > 0 || (intReminder == 0 && intQuotient == 0))
                        {
                            PageIndex = intQuotient;
                        }
                        else if (intReminder == 0 && intQuotient > 0)
                        {
                            PageIndex = intQuotient - 1;
                        }

                        skip = pageSize * PageIndex;
                    }

                    //return DaysOfNonOperation;
                    return DaysOfNonOperation.Skip(skip ?? 0).Take((pageSize ?? totalcount)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Non OperatingDays", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateActivityNonOperatingDays(List<DataContracts.Masters.DC_Activity_OperatingDays> RQ)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                Guid? Activity_Flavour_Id = null;

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (var NOD in RQ)
                    {
                        if (Activity_Flavour_Id == null)
                        {
                            Activity_Flavour_Id = NOD.Activity_Flavor_ID;
                        }

                        var searchNOD = context.Activity_DaysOfOperation.Find(NOD.Activity_DaysOfOperation_Id);
                        if (searchNOD == null)
                        {
                            context.Activity_DaysOfOperation.Add(new Activity_DaysOfOperation
                            {
                                Activity_DaysOfOperation_Id = NOD.Activity_DaysOfOperation_Id,
                                Activity_Flavor_ID = NOD.Activity_Flavor_ID,
                                FromDate = NOD.FromDate,
                                ToDate = NOD.EndDate,
                                CreateDate = DateTime.Now,
                                CreateUser = NOD.EditUser,
                                IsActive = true,
                                IsOperatingDays = false
                            });
                        }
                        else
                        {
                            searchNOD.FromDate = NOD.FromDate;
                            searchNOD.ToDate = NOD.EndDate;
                            searchNOD.EditDate = DateTime.Now;
                            searchNOD.EditUser = NOD.EditUser;
                            searchNOD.IsActive = true;
                            searchNOD.IsOperatingDays = false;
                        }

                        context.SaveChanges();
                    }

                    //if (Activity_Flavour_Id != null)
                    //{
                    //    var NOD_VALID = (from a in RQ select a.Activity_DaysOfOperation_Id).Distinct();
                    //    var NOD_TO_REMOVE = context.Activity_DaysOfOperation.Where(p => p.Activity_Flavor_ID == Activity_Flavour_Id && p.IsOperatingDays == false && !NOD_VALID.Any(p2 => p2 == p.Activity_DaysOfOperation_Id));
                    //    context.Activity_DaysOfOperation.RemoveRange(NOD_TO_REMOVE);

                    //    context.SaveChanges();
                    //}

                    return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Saved Successfully" };
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while updating Activity Non Operating Days", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DataContracts.DC_Message DeleteActivityNonOperatingDaysById(Guid ActivityDaysOfOperationId)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var ToRemoveRecord = context.Activity_DaysOfOperation.Find(ActivityDaysOfOperationId);
                    if (ToRemoveRecord != null)
                    {
                        context.Activity_DaysOfOperation.Remove(ToRemoveRecord);
                        context.SaveChanges();
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Delete Successfully" };
                    }
                    else
                    {
                        return new DC_Message { StatusCode = ReadOnlyMessage.StatusCode.Success, StatusMessage = "Delete Failed" };
                    }
                }
            }

            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while Deleting Activity Non Operating Days", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region Acitivity_Status
        public DC_Message AddUpdateActivityFlavoursStatus(DataContracts.Masters.DC_ActivityFlavoursStatus _objAct)
        {
            DC_Message _msg = new DC_Message();

            using (ConsumerEntities context = new ConsumerEntities())
            {
                if (_objAct.Activity_Flavour_Id.HasValue) //&& _objAct.Activity_Id == Guid.Empty)|| _objAct.Activity_Id != Guid.Empty
                {
                    var results = context.Activity_Flavour.Find(_objAct.Activity_Flavour_Id);
                    var resultsSupplierMappingData = context.Activity_SupplierProductMapping.Where(x => x.Activity_ID == _objAct.Activity_Flavour_Id).FirstOrDefault();

                    if (results != null)
                    {

                        results.Activity_Status = _objAct.Activity_Status;
                        results.Activity_StatusNotes = _objAct.Activity_StatusNotes;
                        results.Activity_Status_Edit_Date = _objAct.Activity_Status_Edit_Date;
                        results.Activity_Status_Edit_User = _objAct.Activity_Status_Edit_User;

                        if (resultsSupplierMappingData != null)
                        {
                            if (_objAct.Activity_Status == "Review Completed")
                            {
                                resultsSupplierMappingData.MappingStatus = "MAPPED";
                                resultsSupplierMappingData.Edit_Date = _objAct.Activity_Status_Edit_Date;
                                resultsSupplierMappingData.Edit_User = _objAct.Activity_Status_Edit_User;
                            }
                            else
                            {
                                resultsSupplierMappingData.MappingStatus = "REVIEW";
                                resultsSupplierMappingData.Edit_Date = _objAct.Activity_Status_Edit_Date;
                                resultsSupplierMappingData.Edit_User = _objAct.Activity_Status_Edit_User;
                            }
                        }

                        if (context.SaveChanges() == 1)
                        {
                            if (_objAct.Activity_Flavour_Id != Guid.Empty)
                            {
                                DL_MongoPush _obj = new DL_MongoPush();
                                _obj.SyncActivityFlavour(_objAct.Activity_Flavour_Id ?? Guid.Empty);
                            }

                            _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;

                        }
                        else
                        {
                            _msg.StatusMessage = ReadOnlyMessage.strFailed;
                            _msg.StatusCode = ReadOnlyMessage.StatusCode.Failed;
                        }
                        return _msg;
                    }

                }
            }
            return _msg;
        }
        #endregion

        #region
        public List<DataContracts.Masters.DC_Activity_CategoryTypes_DDL> GetSupplierProductSubType(DataContracts.Masters.DC_Supplier_DDL _objAct)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var categorizedProducts = context.m_masterattribute
                         .Join(context.m_MasterAttributeMapping, ma => ma.MasterAttribute_Id, mam => mam.SystemMasterAttribute_Id, (ma, mam) => new { ma, mam })
                         .Join(context.Supplier, mamS => mamS.mam.Supplier_Id, s => s.Supplier_Id, (mamS, s) => new { mamS, s })
                         .Join(context.m_MasterAttributeValueMapping, mamST => mamST.mamS.mam.MasterAttributeMapping_Id, mamv => mamv.MasterAttributeMapping_Id, (mamST, mamv) => new { mamST, mamv })
                         .Where(w => w.mamST.mamS.ma.MasterFor == "Activity" && w.mamST.mamS.ma.Name == "ActivityProductSubType" && w.mamST.s.Supplier_Id == _objAct.Supplier_Id)
                         .Select(m => new DC_Activity_CategoryTypes_DDL
                         {
                             SupProdSubType = m.mamv.SupplierMasterAttributeValue,
                             SupProdSubTypeCode = (m.mamv.SupplierMasterAttributeCode == string.Empty ? m.mamv.SupplierMasterAttributeValue : m.mamv.SupplierMasterAttributeCode)
                         }).OrderBy(x => x.SupProdSubType).ToList();

                    return categorizedProducts;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Activities Reports
        //DC_Activity_Report_RS
        public List<DataContracts.Masters.DC_Activity_Report_RS> GetActivitiesReport(DC_ActivityCountStats _obj)
        {
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbGroupBy = new StringBuilder();
            StringBuilder sborderBy = new StringBuilder();
            List<DC_Activity_Report_RS> result = new List<DC_Activity_Report_RS>();
            try
            {

                sbSelect.Append(" Select spm.SupplierName, Count(spm.SuplierProductCode) As TotalCount ");
                sbFrom.Append(" From Activity_Flavour F with(NOLOCK) ");
                sbJoin.Append(" Join Activity_SupplierProductMapping spm with(nolock) on F.Activity_Flavour_Id = spm.Activity_ID");
                sbJoin.Append(@" join [Supplier_ProductCategory] SP with(nolock) on spm.Supplier_ID = sp.Supplier_Id 
                                        and SP.ProductCategory = 'Activities' and SP.ProductCategorySubType = 'Sightseeing'");


                sbWhere.Append(" where Country_Id is not null and  City_Id is not null ");
                sbGroupBy.Append(" group by SupplierName");
                sborderBy.Append(" order by SupplierName");
                bool flagCountry = false;
                if (!string.IsNullOrEmpty(_obj.SupplierID) && _obj.SupplierID != "0")
                {
                    sbWhere.Append(" and SPM.Supplier_Id='" + Guid.Parse(_obj.SupplierID) + "' ");
                }
                if (_obj.CountryID == "1") //All countries
                {
                    flagCountry = true;
                    sbSelect.Append(" ,F.Country  ");
                    sbGroupBy.Append(" ,F.Country ");
                    sborderBy.Append(" ,F.Country ");
                    //sbWhere.Append(@"F.Country_Id='" + Guid.Parse(_obj.CountryID) + "' ");
                }
                else if (_obj.CountryID != "0" && _obj.CountryID != "1") //Single countries
                {
                    flagCountry = true;
                    sbSelect.Append(" ,F.Country ");
                    sbGroupBy.Append(" ,F.Country ");
                    sborderBy.Append(" ,F.Country ");
                    sbWhere.Append(@" and F.Country_Id='" + Guid.Parse(_obj.CountryID) + "' ");
                }
                if (_obj.CityID == "1")
                {
                    if (!flagCountry)
                    {
                        sbSelect.Append(" ,F.Country ");
                        sbGroupBy.Append(" ,F.Country ");
                        sborderBy.Append(" ,F.Country ");
                    }
                    sbSelect.Append(" ,F.City ");
                    sbGroupBy.Append(" ,F.City ");
                    sborderBy.Append(" ,F.City ");

                }
                else if (_obj.CityID != "1" && _obj.CityID != "0")
                {
                    if (!flagCountry)
                    {
                        sbSelect.Append(" ,F.Country ");
                        sbGroupBy.Append(" ,F.Country ");
                        sborderBy.Append(" ,F.Country ");
                    }
                    sbSelect.Append(" ,F.City ");
                    sbGroupBy.Append(" ,F.City ");
                    sborderBy.Append(" ,F.City ");
                    sbWhere.Append(@" and F.City_Id='" + Guid.Parse(_obj.CityID) + "' ");

                }


                StringBuilder sbfinalquery = new StringBuilder();
                sbfinalquery.Append(sbSelect); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbFrom); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbJoin); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbWhere); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbGroupBy);
                sbfinalquery.Append(sborderBy);

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    try
                    {
                        result = context.Database.SqlQuery<DC_Activity_Report_RS>(sbfinalquery.ToString()).ToList();

                        if (result.Count == 0)
                            return result = new List<DC_Activity_Report_RS> { };
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return result;
                // }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        public List<DataContracts.Masters.DC_ActivityProductDetailsReport> GetActivitiesProductDetailsReport(DC_ActivityCountStats _obj)
        {
            List<DC_ActivityProductDetailsReport> result = new List<DC_ActivityProductDetailsReport>();
            StringBuilder sbSelect = new StringBuilder();
            StringBuilder sbFrom = new StringBuilder();
            StringBuilder sbJoin = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbGroupBy = new StringBuilder();
            StringBuilder sborderBy = new StringBuilder();


            try
            {
                sbSelect.Append("select spm.SupplierName,spm.SuplierProductCode As ActivityCode,F.ProductName As ActivityName");
                sbSelect.Append(" ,LongDescription = (Select DE.Description from Activity_Descriptions De with(nolock) where De.Activity_Flavour_Id = spm.Activity_ID and De.DescriptionType = 'Long')  ");
                sbSelect.Append(",ShortDescription = (Select De.Description from Activity_Descriptions De with(nolock) where De.Activity_Flavour_Id = spm.Activity_ID and De.DescriptionType = 'Short') ");

                sbSelect.Append(",InterestType = (Select STUFF((SELECT ', ' + QUOTENAME(SystemInterestType) FROM Activity_CategoriesType with(nolock) where Activity_Flavour_Id = spm.Activity_ID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");
                sbSelect.Append(",CategorySubType = (Select STUFF((SELECT ', ' + QUOTENAME(SystemProductCategorySubType) FROM Activity_CategoriesType with(nolock) where Activity_Flavour_Id = spm.Activity_ID FOR  XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");
                sbSelect.Append(",ProductType = (Select STUFF((SELECT ',' + QUOTENAME(SystemProductType) FROM Activity_CategoriesType with(nolock) where Activity_Flavour_Id = spm.Activity_ID  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");
                sbSelect.Append(",ProductSubType = (Select STUFF((SELECT ',' + QUOTENAME(SystemProductNameSubType) FROM Activity_CategoriesType with(nolock) where Activity_Flavour_Id = spm.Activity_ID  FOR  XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");

                sbSelect.Append(",SuitableFor = (Select STUFF((SELECT ', ' + QUOTENAME(AttributeValue) FROM Activity_ClassificationAttributes with(nolock) where Activity_Flavour_Id = spm.Activity_ID and AttributeType='Product'  and AttributeSubType = 'SuitableFor' FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");
                sbSelect.Append(",PhysicalLevel = (Select STUFF((SELECT ', ' + QUOTENAME(AttributeValue) FROM Activity_ClassificationAttributes with(nolock) where Activity_Flavour_Id = spm.Activity_ID and  AttributeType='Product' and AttributeSubType = 'PhysicalIntensity' FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");
                sbSelect.Append(",spm.Location As AreaAddress");

                sbSelect.Append(",Inclusions = (SELECT CASE  WHEN EXISTS ( SELECT COUNT(1) FROM Activity_Inclusions with(nolock) WHERE IsInclusion = 1 and Activity_Flavour_Id = spm.Activity_ID) THEN 'YES' Else 'NO' END)");
                sbSelect.Append(",Exclusions = (SELECT CASE WHEN EXISTS ( SELECT COUNT(1) FROM Activity_Inclusions with(nolock) WHERE IsInclusion = 0 and Activity_Flavour_Id = spm.Activity_ID) THEN 'YES' Else 'NO' END)");

                //sbSelect.Append(",FromDate = (Select CASE WHEN  Price_ValidFrom is not null  THEN MIN(ISNULL(Price_ValidFrom,coalesce( convert(varchar(20),Price_ValidFrom) , 22 )))  else NULL END FROM Activity_Prices where Activity_Flavour_Id = spm.Activity_ID group by Activity_Flavour_Id,Price_ValidFrom)");
                //sbSelect.Append(",ToDate = (Select CASE WHEN  Price_ValidTo is not null  THEN MAX(ISNULL(Price_ValidTo,coalesce( convert(varchar(20),Price_ValidTo) , 22 )))  else NULL END FROM Activity_Prices where Activity_Flavour_Id=spm.Activity_ID group by Activity_Flavour_Id,Price_ValidTo)");

                sbSelect.Append(",FromDate = (Select MIN(ISNULL(Price_ValidFrom, NULL)) FROM Activity_Prices with(nolock) where Activity_Flavour_Id = spm.Activity_ID)");
                sbSelect.Append(",ToDate = (Select MAX(ISNULL(Price_ValidTo, NULL)) FROM Activity_Prices with(nolock) where Activity_Flavour_Id = spm.Activity_ID)");

                sbSelect.Append(",Duration = (Select STUFF((SELECT ', ' + QUOTENAME(CASE WHEN CAST(SUBSTRING(Duration,1,charindex('.',Duration)-1) as int) > 0 then SUBSTRING(Duration,1,charindex('.',Duration)-1) + ' Days' ELSE '' END + CASE WHEN CAST(SUBSTRING(Duration,charindex('.',Duration) + 1 ,2) as int) > 0 then SUBSTRING(Duration,charindex('.',Duration) + 1 ,2) + ' Hours' ELSE '' END  + CASE WHEN CAST(SUBSTRING(Duration,charindex(':',Duration) + 1,2) as int) > 0 then SUBSTRING(Duration,charindex(':',Duration) + 1,2) + ' Mins' ELSE '' END  ) from Activity_DaysOfWeek where Activity_Flavor_ID = spm.Activity_ID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,1,''))");


                sbFrom.Append("from Activity_Flavour F with(nolock)");



                sbJoin.Append(" join Activity_SupplierProductMapping spm with(nolock) on F.Activity_Flavour_ID = spm.Activity_ID ");
                sbJoin.Append(" join Supplier S with(nolock) on spm.Supplier_ID = S.Supplier_Id and S.StatusCode = 'ACTIVE' ");




                sbWhere.Append(" where Country_Id is not null and  City_Id is not null ");
                //sbGroupBy.Append(" group by SupplierName");
                sborderBy.Append(" order by SupplierName");
                bool flagCountry = false;
                if (!string.IsNullOrEmpty(_obj.SupplierID) && _obj.SupplierID != "0")
                {
                    sbWhere.Append(" and SPM.Supplier_Id='" + Guid.Parse(_obj.SupplierID) + "' ");
                }
                if (_obj.CountryID == "1") //All countries
                {
                    flagCountry = true;
                    sbSelect.Append(" ,F.Country  ");
                    // sbGroupBy.Append(" ,F.Country ");
                    sborderBy.Append(" ,F.Country ");
                    //sbWhere.Append(@"F.Country_Id='" + Guid.Parse(_obj.CountryID) + "' ");
                }
                else if (_obj.CountryID != "0" && _obj.CountryID != "1") //Single countries
                {
                    flagCountry = true;
                    sbSelect.Append(" ,F.Country ");
                    //sbGroupBy.Append(" ,F.Country ");
                    sborderBy.Append(" ,F.Country ");
                    sbWhere.Append(@" and F.Country_Id='" + Guid.Parse(_obj.CountryID) + "' ");
                }
                if (_obj.CityID == "1")
                {
                    if (!flagCountry)
                    {
                        sbSelect.Append(" ,F.Country ");
                        //   sbGroupBy.Append(" ,F.Country ");
                        sborderBy.Append(" ,F.Country ");
                    }
                    sbSelect.Append(" ,F.City ");
                    //  sbGroupBy.Append(" ,F.City ");
                    sborderBy.Append(" ,F.City ");

                }
                else if (_obj.CityID != "1" && _obj.CityID != "0")
                {
                    if (!flagCountry)
                    {
                        sbSelect.Append(" ,F.Country ");
                        sbGroupBy.Append(" ,F.Country ");
                        sborderBy.Append(" ,F.Country ");
                    }
                    sbSelect.Append(" ,F.City ");
                    // sbGroupBy.Append(" ,F.City ");
                    sborderBy.Append(" ,F.City ");
                    sbWhere.Append(@" and F.City_Id='" + Guid.Parse(_obj.CityID) + "' ");

                }

                StringBuilder sbfinalquery = new StringBuilder();
                sbfinalquery.Append(sbSelect); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbFrom); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbJoin); sbfinalquery.Append(" ");
                sbfinalquery.Append(sbWhere); sbfinalquery.Append(" ");
                //sbfinalquery.Append(sbGroupBy);
                sbfinalquery.Append(sborderBy);


                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    try
                    {
                        result = context.Database.SqlQuery<DC_ActivityProductDetailsReport>(sbfinalquery.ToString()).ToList();

                        if (result.Count == 0)
                            return result = new List<DC_ActivityProductDetailsReport> { };
                    }
                    catch (Exception ex)
                    {
                        throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus
                        {
                            ErrorMessage = "Error while fetching Activity Product Details",
                            ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                        });

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            return result;
        }
        #endregion


        #region Activity MediaAttributes
        public bool AddActivityMediaAttributes(DataContracts.Masters.DC_Activity_MediaAttributes AM)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Activity_MediaAttributes objNew = new Activity_MediaAttributes();

                    if (AM.Activity_MediaAttributes_Id == null)
                    {
                        AM.Activity_MediaAttributes_Id = Guid.NewGuid();
                    }

                    objNew.Activity_MediaAttributes_Id = AM.Activity_MediaAttributes_Id;
                    objNew.Activity_Media_Id = AM.Activity_Media_Id;
                    objNew.AttributeType = AM.AttributeType;
                    objNew.AttributeValue = AM.AttributeValue;
                    objNew.Create_Date = AM.Create_Date;
                    objNew.Create_User = AM.Create_User;
                    objNew.Status = AM.Status;
                    objNew.IsActive = AM.IsActive;
                    objNew.IsSystemAttribute = AM.IsSystemAttribute;

                    context.Activity_MediaAttributes.Add(objNew);
                    context.SaveChanges();

                    objNew = null;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding accomodation media attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public List<Activity_MediaDetailsForAttribute> GetActivityMediaForAttributes()
        {
            List<Activity_MediaDetailsForAttribute> lstActivity_MediaDetailsForAttribute = new List<Activity_MediaDetailsForAttribute>();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    StringBuilder sbQuery = new StringBuilder();

                    sbQuery.Append(" Select M.Activity_Media_Id, M.Activity_Flavour_Id, M.Media_URL,M.FileFormat,spm.SupplierName,F.CommonProductNameSubType_Id+'_' + REVERSE(SUBSTRING(REVERSE(Media_URL),0,charindex('/',REVERSE(Media_URL)))) AS filename ");
                    sbQuery.Append(@"  from Activity_Media M with(nolock)
                                        join Activity_Flavour F with(nolock) on M.Activity_Flavour_Id = F.Activity_Flavour_Id
                                        join Activity_SupplierProductMapping spm with(nolock) on M.Activity_Flavour_Id = spm.Activity_ID
                                        where IsStaticFileAvailable = 1 
                                        and M.Activity_Media_Id NOT IN (Select Distinct Activity_Media_Id from Activity_MediaAttributes with(nolock))
                                        order by spm.SupplierName ");

                    try { lstActivity_MediaDetailsForAttribute = context.Database.SqlQuery<Activity_MediaDetailsForAttribute>(sbQuery.ToString()).ToList(); } catch (Exception ex) { }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstActivity_MediaDetailsForAttribute;
        }

        public List<DC_Activity_MediaAttributesForImageReview> GetActivityMediaAttributesForImageReview(DC_Activity_Media_Search_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_MediaAttributes
                                 select a;

                    if (RQ.Activity_Flavour_Id != null)
                    {
                        search = from a in search
                                 join b in context.Activity_Media on a.Activity_Media_Id equals b.Activity_Media_Id
                                 where b.Activity_Flavour_Id==RQ.Activity_Flavour_Id
                                 select a;
                    }

                    // string AttributeType = ConfigurationManager.AppSettings["ActivityMediaAttribute"];
                    //List<string> test= AttributeType.Split(',').ToList<string>();
                    // if ((AttributeType != null && AttributeType != string.Empty))
                    // {
                    //     search = from a in search
                    //              where test.Contains(a.AttributeType)
                    //              select a;
                    // }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);
                    var result = from a in search
                                 orderby a.AttributeType
                                 select new DataContracts.Masters.DC_Activity_MediaAttributesForImageReview
                                 {
                                     Activity_MediaAttributes_Id = a.Activity_MediaAttributes_Id,
                                     Activity_Media_Id = a.Activity_Media_Id,
                                     AttributeType = a.AttributeType,
                                     AttributeValue = a.AttributeValue

                                 };
                    return result.OrderBy(p => p.Activity_Media_Id).Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        #endregion

    }
}
