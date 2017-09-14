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
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Attr Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        #endregion

        #region "Activity Insert and Update"
        public DataContracts.DC_Message AddUpdateActivity(DataContracts.Masters.DC_Activity _objAct)
        {
            DC_Message _msg = new DC_Message();

            using (ConsumerEntities context = new ConsumerEntities())
            {
                var isduplicate = (from a in context.Activities
                                   where a.Activity_Id != (_objAct.Activity_Id ?? Guid.Empty)  && a.Display_Name == _objAct.Display_Name
                                   select a).Count() == 0 ? false:true;

                if(isduplicate)
                {
                    _msg.StatusMessage = ReadOnlyMessage.strAlreadyExist;
                    _msg.StatusCode = ReadOnlyMessage.StatusCode.Duplicate;
                    return _msg;
                }

                if(_objAct.Activity_Id.HasValue && _objAct.Activity_Id!=Guid.Empty)
                {
                    var results = context.Activities.Find(_objAct.Activity_Id);

                    if(results!=null)
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

                        if (context.SaveChanges()==1)
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
                            CompanyProductID =_objAct.CompanyProductID,
                            CompanyRating = _objAct.CompanyRating,
                            CompanyRecommended = _objAct.CompanyRecommended,
                            Display_Name = _objAct.Display_Name,
                            IsActive = _objAct.IsActive,
                            Latitude = _objAct.Latitude,
                            Longitude = _objAct.Longitude,
                            Legacy_Product_ID =_objAct.Legacy_Product_ID,
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
                    }
                }
            }
            
            return _msg;
        }
        #endregion
    }
}
