using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace DataLayer
{
    public class DL_ScheduledTask : IDisposable
    {
        public void Dispose()
        {
        }

        public IList<DataContracts.Schedulers.SupplierScheduledTask> GetScheduleTaskList(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ RQ)
        {
            try
            {
                if (RQ.UserName != null && !String.IsNullOrWhiteSpace(RQ.UserName))
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        var prodMapSearch = (from a in context.Supplier_Scheduled_Task select a).AsQueryable();

                        if (!string.IsNullOrWhiteSpace(RQ.UserName))
                        {
                            List<string> Entities = new List<string>();
                            var roles = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims
                                            .Where(c => c.Type == ClaimTypes.Role)
                                            .Select(c => c.Value);
                            var claimsIdentity = System.Web.HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
                            var customUserClaim = claimsIdentity != null ? claimsIdentity.Claims.FirstOrDefault(x => x.Type == "cutomType") : null;
                            var customTypeValue = customUserClaim != null ? customUserClaim.Value : System.Web.HttpContext.Current.User.Identity.GetUserName();
                            var roleOfUser = claimsIdentity != null ? claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value : "User";

                        }
                        if (RQ.FromDate != null)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.Task_Date >= RQ.FromDate
                                            select a;
                        }
                        if (RQ.ToDate != null)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.Task_Date <= RQ.ToDate
                                            select a;
                        }
                        if (RQ.Supplier_Id != null && RQ.Supplier_Id != Guid.Empty)
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.Supplier_ID == RQ.Supplier_Id
                                            select a;
                        }
                        if (!string.IsNullOrWhiteSpace(RQ.Status))
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.Entity == RQ.Status
                                            select a;
                        }

                        if (!string.IsNullOrWhiteSpace(RQ.Entity))
                        {
                            prodMapSearch = from a in prodMapSearch
                                            where a.Entity == RQ.Entity
                                            select a;
                        }
                        int total;
                        total = prodMapSearch.Count();
                        var skip = RQ.PageSize * RQ.PageNo;
                        if (RQ.PageSize == 0)
                            RQ.PageSize = 5;
                        IQueryable<DataContracts.Schedulers.SupplierScheduledTask> supplerschedulertasklist;
                        supplerschedulertasklist = (from a in prodMapSearch
                                                    join sup in context.Supplier on a.Supplier_ID equals sup.Supplier_Id
                                                    select new DataContracts.Schedulers.SupplierScheduledTask
                                                    {
                                                        SuppllierName = sup.Name,
                                                        Entity = a.Entity,
                                                        SupplierScheduleTaskID = a.SupplierScheduledTaskID,
                                                        Suppllier_ID = a.Supplier_ID,
                                                        TotalRecord = total,
                                                        ISComplete = a.Is_Complete.Value,
                                                        ScheduledDate = a.Task_Date.Value,
                                                        PendingFordays = a.Is_Complete == true ? 0 : (DateTime.Now.Day - a.Task_Date.Value.Day),

                                                        
                                                    }).OrderBy(p => p.ScheduledDate).Skip(skip).Take(RQ.PageSize);
                        var result = supplerschedulertasklist.ToList();
                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching Supplier Scheduled task", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
    }
}
