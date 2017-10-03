using System;
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
        public List<DC_Activity_Contact> GetActivityContacts(Guid Activity_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ac in context.Activity_Contact
                                 where ac.Activity_Id == Activity_Id
                                 && ac.Activity_Contact_Id == (DataKey_Id == Guid.Empty ? ac.Activity_Contact_Id : DataKey_Id)
                                 select new DC_Activity_Contact
                                 {
                                     Activity_Contact_Id = ac.Activity_Contact_Id,
                                     Activity_Id = ac.Activity_Id,
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
                if (AC.Activity_Id == null)
                {
                    return false;
                }

                if (AC.Activity_Id == null)
                {
                    AC.Activity_Id = Guid.NewGuid();
                }

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    Activity_Contact newObj = new Activity_Contact();
                    newObj.Activity_Contact_Id = AC.Activity_Contact_Id;
                    newObj.Activity_Id = AC.Activity_Id;
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
        
        public string GetLegacyProductId(Guid Activity_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = (from ac in context.Activity_Contact
                                  where ac.Activity_Id == Activity_Id
                                  select new { ac.Legacy_Product_ID }).FirstOrDefault();

                    return Convert.ToString(search);
                    
                }
            }
            catch (Exception e)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region "Activity Status"
        public List<DC_Activity_Status> GetActivityStatus(Guid Activity_Id, Guid DataKey_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from ast in context.Activity_Status
                                 where ast.Activity_Id == Activity_Id
                                 && ast.Activity_Status_Id == (DataKey_Id == Guid.Empty ? ast.Activity_Status_Id : DataKey_Id)
                                 select new DC_Activity_Status
                                 {
                                     Activity_Id = ast.Activity_Id,
                                     Activity_Status_Id = ast.Activity_Status_Id,
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
                    if (AS.Activity_Id == null)
                    {
                        return false;
                    }

                    Activity_Status objNew = new Activity_Status();

                    if (AS.Activity_Status_Id== null)
                    {
                        AS.Activity_Status_Id = Guid.NewGuid();
                    }

                    objNew.Activity_Status_Id = AS.Activity_Status_Id;
                    objNew.Activity_Id = AS.Activity_Id;
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
                                 where a.Activity_Id == RQ.Activity_Media_Id
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
                                 where a.ValidFrom== RQ.ValidFrom
                                 select a;
                    }

                    if (RQ.ValidTo != null)
                    {
                        search = from a in search
                                 where a.ValidTo == RQ.ValidTo
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.MediaName
                                 select new DataContracts.Masters.DC_Activity_Media
                                 {
                                     Activity_Id=a.Activity_Id,
                                     Activity_Media_Id=a.Activity_Media_Id,
                                      IsActive=a.IsActive,
                                      MediaName=a.MediaName,
                                      Create_Date=a.Create_Date,
                                      Create_User=a.Create_User,
                                      Edit_Date=a.Edit_Date,
                                      Edit_User=a.Edit_User,
                                      MediaType=a.MediaType,
                                      Category=a.Category,
                                      SubCategory=a.SubCategory,
                                      Description=a.Description,
                                      FileFormat=a.FileFormat,
                                      Legacy_Product_Id=a.Legacy_Product_Id,
                                      MediaFileMaster=a.MediaFileMaster,
                                      MediaID=a.MediaID,
                                      Media_Path=a.Media_Path,
                                      Media_Position=a.Media_Position,
                                      Media_URL=a.Media_URL,
                                      RoomCategory=a.RoomCategory,
                                      ValidFrom=a.ValidFrom,
                                      ValidTo=a.ValidTo,
                                      TotalRecords=total
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
                    var isduplicate = (from a in context.Activity_Media
                                       where a.MediaName.Trim().TrimStart().ToUpper() == RQ.MediaName.Trim().TrimStart().ToUpper() && a.Activity_Media_Id != RQ.Activity_Media_Id
                                       select a).Count() == 0 ? false : true;

                    if (isduplicate)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        return _msg;
                    }
                    if (RQ.Activity_Media_Id != null)
                    {
                        var res = context.Activity_Media.Find(RQ.Activity_Media_Id);
                        if (res != null)
                        {
                            res.Activity_Media_Id = RQ.Activity_Media_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                            res.IsActive = RQ.IsActive;
                            res.FileFormat = RQ.FileFormat;
                            res.Media_URL = RQ.Media_URL;
                            res.Media_Position = RQ.Media_Position;
                            res.SubCategory = RQ.SubCategory;
                            res.MediaFileMaster = RQ.MediaFileMaster;
                            res.Description = RQ.Description;
                            res.MediaID = RQ.MediaID;
                            res.MediaType = RQ.MediaType;
                            res.RoomCategory = RQ.RoomCategory;
                            res.MediaName = RQ.MediaName;
                            res.Media_Path = RQ.Media_Path;
                            res.Category = RQ.Category;
                            res.ValidFrom = RQ.ValidFrom;
                            res.ValidTo = RQ.ValidTo;
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
                        newmed.MediaID = RQ.MediaID;
                        newmed.MediaType = RQ.MediaType;
                        newmed.RoomCategory = RQ.RoomCategory;
                        newmed.MediaName = RQ.MediaName;
                        newmed.Media_Path = RQ.Media_Path;
                        newmed.Category = RQ.Category;
                        newmed.ValidFrom = RQ.ValidFrom;
                        newmed.ValidTo = RQ.ValidTo;
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
        #endregion

        #region  Activity Inclusions
        public List<DC_Activity_Inclusions_RS> GetActivityInclusions(DC_Activity_Inclusions_RQ RQ)
        {
            try {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from a in context.Activity_Inclusions
                                 select a;

                    if (RQ.Activity_Inclusions_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_Inclusions_Id
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
                                 where a.Activity_Flavour_Id== RQ.Activity_Flavour_Id
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

                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var searchresult = from s in search
                                       orderby s.InclusionName
                                       select new DataContracts.Masters.DC_Activity_Inclusions_RS
                                       {
                                           Activity_Inclusions_Id = s.Activity_Inclusions_Id,
                                           InclusionFrom = s.InclusionFrom,
                                           InclusionTo = s.InclusionTo,
                                           InclusionFor = s.InclusionFor,
                                           InclusionName = s.InclusionName,
                                           InclusionDescription = s.InclusionFor,
                                           InclusionType = s.InclusionType,
                                           IsAudioCommentary = s.IsAudioCommentary,
                                           IsDriver = s.IsDriver,
                                           IsInclusion = s.IsInclusion,
                                           Legacy_Product_Id = s.Legacy_Product_Id,
                                           Activity_Flavour_Id = s.Activity_Flavour_Id,
                                           Create_Date = s.Create_Date,
                                           RestaurantStyle = s.RestaurantStyle,
                                           TotalRecords = total,
                                           Activity_Id = s.Activity_Id
                                       };
                     return searchresult.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch(Exception ex) {
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
                            res.Activity_Inclusions_Id = RQ.Activity_Inclusions_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Legacy_Product_Id = RQ.Legacy_Product_Id;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            res.IsInclusion = RQ.IsInclusion;
                            res.InclusionName = RQ.InclusionName;
                            res.InclusionDescription = RQ.InclusionDescription;
                            res.InclusionFor = RQ.InclusionFor;
                            res.InclusionFrom = RQ.InclusionFrom;
                            res.InclusionTo = RQ.InclusionTo;
                            res.InclusionType = RQ.InclusionType;
                            res.IsDriver = RQ.IsDriver;
                            res.IsAudioCommentary = RQ.IsAudioCommentary;
                            res.RestaurantStyle = RQ.RestaurantStyle;
                            res.Create_Date = DateTime.Now;
                            res.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
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
                        else isinsert = true;
                    }
                    else isinsert = true;

                    if (isinsert)
                    {
                        Activity_Inclusions obj = new Activity_Inclusions();
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
                        obj.RestaurantStyle = RQ.RestaurantStyle;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
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
            catch(Exception EX)
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
                    var search = from a in context.Activity_ClassificationAttributes
                                 select a;

                    if (RQ.Activity_ClassificationAttribute_Id != null)
                    {
                        search = from a in search
                                 where a.Activity_Id == RQ.Activity_ClassificationAttribute_Id
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

                    if (RQ.Legacy_Product_Id != null)
                    {
                        search = from a in search
                                 where a.Legacy_Product_ID == RQ.Legacy_Product_Id
                                 select a;
                    }
                    int total = search.Count();
                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var result = from a in search
                                 orderby a.AttributeValue
                                 select new DataContracts.Masters.DC_Activity_ClassificationAttributes
                                 {
                                     Activity_Id = a.Activity_Id,
                                     IsActive = a.IsActive,
                                     CreateDate = a.Create_Date,
                                     CreateUser = a.Create_User,
                                     EditDate = a.Edit_Date,
                                     EditUser = a.Edit_User,
                                     Legacy_Product_Id = a.Legacy_Product_ID,
                                     AttributeType=a.AttributeType,
                                     Activity_Flavour_Id=a.Activity_Flavour_Id,
                                     Activity_ClassificationAttribute_Id=a.Activity_ClassificationAttribute_Id,
                                     AttributeSubType=a.AttributeSubType,
                                     AttributeValue=a.AttributeValue,
                                     InternalOnly=a.InternalOnly,
                                     TotalRecords = total
                                 };
                    return result.Skip(skip).Take((RQ.PageSize ?? total)).ToList();
                }
            }
            catch(Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while fetching Activity Classification Attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });

            }
        }
        public DC_Message AddUpdateActivityClassifiationAttributes(DC_Activity_ClassificationAttributes RQ)
        {
            bool IsInsert = false;
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                     var isduplicate = (from a in context.Activity_ClassificationAttributes
                                                      where a.AttributeValue.Trim().TrimStart().ToUpper() == RQ.AttributeValue.Trim().TrimStart().ToUpper() && a.Activity_ClassificationAttribute_Id != RQ.Activity_ClassificationAttribute_Id
                                        select a).Count() == 0 ? false : true;
                    if (isduplicate)
                    {
                        _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                        _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                        return _msg;
                    }
                    if (RQ.Activity_ClassificationAttribute_Id != null)
                    {
                        var res = context.Activity_ClassificationAttributes.Find(RQ.Activity_ClassificationAttribute_Id);
                        if (res != null)
                        {
                            res.Activity_ClassificationAttribute_Id = RQ.Activity_ClassificationAttribute_Id ?? Guid.Empty;
                            res.Activity_Id = RQ.Activity_Id;
                            res.Legacy_Product_ID = RQ.Legacy_Product_Id;
                            res.IsActive = RQ.IsActive;
                            res.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                            res.AttributeSubType = RQ.AttributeSubType;
                            res.AttributeType = RQ.AttributeType;
                            res.AttributeValue = RQ.AttributeValue;
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
                        else
                            IsInsert = true;
                    }
                    else
                        IsInsert = true;

                    if (IsInsert)
                    {
                        DataLayer.Activity_ClassificationAttributes obj = new DataLayer.Activity_ClassificationAttributes();
                        obj.Activity_ClassificationAttribute_Id = RQ.Activity_ClassificationAttribute_Id ?? Guid.NewGuid();
                        obj.Activity_Id = RQ.Activity_Id;
                        obj.Legacy_Product_ID = RQ.Legacy_Product_Id;
                        obj.IsActive = RQ.IsActive;
                        obj.Activity_Flavour_Id = RQ.Activity_Flavour_Id;
                        obj.AttributeSubType = RQ.AttributeSubType;
                        obj.AttributeType = RQ.AttributeType;
                        obj.AttributeValue = RQ.AttributeValue;
                        obj.Create_Date = DateTime.Now;
                        obj.Create_User = System.Web.HttpContext.Current.User.Identity.Name;
                        context.Activity_ClassificationAttributes.Add(obj);
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
                throw new FaultException<DC_ErrorStatus>(new DC_ErrorStatus { ErrorMessage = "Error while adding Activity Classification Attributes", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
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
                                 where a.Activity_Id == RQ.Activity_PickUpDrop_Id
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
                                     Activity_PickUpDrop_Id=a.Activity_PickUpDrop_Id,
                                     Activity_Id = a.Activity_Id,
                                     IsAC = a.IsAC,
                                     Create_Date = a.Create_Date,
                                     Create_User = a.Create_User,
                                     Edit_Date = a.Edit_Date,
                                     Edit_User = a.Edit_User,
                                     Legacy_Product_Id = a.Legacy_Product_Id,
                                    Supplier_Id=a.Supplier_Id,
                                    SupplierName=a.SupplierName,
                                    Activity_Flavour_Id=a.Activity_Flavour_Id,
                                    TransferType=a.TransferType,
                                    VehicleCategory=a.VehicleCategory,
                                     VehicleName=a.VehicleName,
                                     VehicleType=a.VehicleType,
                                     TotalRecords = total
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
                            res.Activity_PickUpDrop_Id = RQ.Activity_PickUpDrop_Id??Guid.Empty;
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

                        obj.Activity_PickUpDrop_Id =RQ.Activity_PickUpDrop_Id??Guid.NewGuid();
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
    }
}
