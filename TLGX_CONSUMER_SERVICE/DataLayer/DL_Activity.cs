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
    }
}
