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
                    StringBuilder sbselectRoles = new StringBuilder();
                    sbselectRoles.Append(@"select SysUserRole.RoleId from AspNetUsers SysUser with(nolock) join AspNetUserRoles SysUserRole with(nolock) on SysUser.Id= SysUserRole.UserId  where SysUser.UserName  ='" + RQ.UserName);
                    List<int> listRoles = new List<int>();
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { listRoles = context.Database.SqlQuery<int>(sbselectRoles.ToString()).ToList(); } catch (Exception ex) { }
                    }

                    StringBuilder sbsqlselect = new StringBuilder();
                    StringBuilder sbsqlfrom = new StringBuilder();
                    StringBuilder sbsqlwhere = new StringBuilder();
                    List<DataContracts.Schedulers.SupplierScheduledTask> result = new List<DataContracts.Schedulers.SupplierScheduledTask>();

                    #region select 
                    sbsqlselect.Append(@"select d.Name SuppllierName,c.Entity,a.Log_Id,b.Schedule_Datetime as ScheduledDate,b.Status as Status,
                                         case when b.Status='Pending' then DATEDIFF(d,GETDATE(),b.Schedule_Datetime) else '0' end as PendingFordays, ");

                    #endregion

                    #region from
                    sbsqlfrom.AppendLine(" from Supplier_Scheduled_Task_Log a with (nolock) inner join Supplier_Scheduled_Task b with (nolock) on a.Task_Id = b.Task_Id inner join Supplier_Schedule c with (nolock) on c.SupplierScheduleID = b.Schedule_Id inner join Supplier d with (nolock) on d.Supplier_Id = c.Supplier_ID");

                    #endregion

                    sbsqlwhere.AppendLine(" where 1=1 ");
                    if(!string.IsNullOrWhiteSpace(RQ.RedirectFrom))
                    {
                        sbsqlwhere.AppendLine(" and a.log_type = '" + RQ.RedirectFrom + "' ");
                    }

                    if (listRoles.Count>0)
                    {
                        sbsqlwhere.AppendLine(" and c.user_role_id (" + listRoles.ToString().TrimEnd(',') + ");");
                    }

                    if (RQ.FromDate != null)
                    {
                        sbsqlwhere.AppendLine(" and b.Schedule_Datetime >= '" + RQ.FromDate + "' ");
                    }

                    if (RQ.ToDate != null)
                    {
                        sbsqlwhere.AppendLine(" and b.Schedule_Datetime <= '" + RQ.ToDate + "' ");
                    }

                    if (RQ.Supplier_Id != null && RQ.Supplier_Id != Guid.Empty)
                    {
                        sbsqlwhere.AppendLine(" and c.Supplier_ID = '" + RQ.Supplier_Id + "' ");
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Status))
                    {
                        sbsqlwhere.AppendLine(" and b.Status = '" + RQ.Status + "' ");
                    }

                    if (!string.IsNullOrWhiteSpace(RQ.Entity))
                    {
                        sbsqlwhere.AppendLine(" and c.Entity = '" + RQ.Entity + "' ");
                    }

                    int skip = 0;
                    int total = 0;
                    skip = RQ.PageSize * RQ.PageNo;

                    StringBuilder sbsqlselectcount = new StringBuilder();
                    sbsqlselectcount.Append("select count(a.log_id) ");
                    sbsqlselectcount.Append(" " + sbsqlfrom);
                    sbsqlselectcount.Append(" " + sbsqlwhere);

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { total = context.Database.SqlQuery<int>(sbsqlselectcount.ToString()).FirstOrDefault(); } catch (Exception ex) { }
                    }

                    if (total > 0)
                    {
                        sbsqlselect.Append(total.ToString() + " AS  TotalRecord ");
                    }

                    //if (total <= skip)
                    //{
                    //    int PageIndex = 0;
                    //    int intReminder = total % RQ.PageSize;
                    //    int intQuotient = total / RQ.PageSize;

                    //    if (intReminder > 0)
                    //    {
                    //        PageIndex = intQuotient + 1;
                    //    }
                    //    else
                    //    {
                    //        PageIndex = intQuotient;
                    //    }

                    //    skip = RQ.PageSize * (PageIndex - 1);
                    //}

                    //StringBuilder sbOrderby = new StringBuilder();
                    //sbOrderby.Append(" ORDER BY d.Name ");
                    //sbOrderby.Append(" OFFSET ");
                    //sbOrderby.Append((skip).ToString());
                    //sbOrderby.Append(" ROWS FETCH NEXT ");
                    //sbOrderby.Append(RQ.PageSize.ToString());
                    //sbOrderby.Append(" ROWS ONLY ");

                    StringBuilder sbfinalQuery = new StringBuilder();
                    sbfinalQuery.Append(sbsqlselect + " ");
                    sbfinalQuery.Append(" " + sbsqlfrom + " ");
                    sbfinalQuery.Append(" " + sbsqlwhere + " ");
                    //sbfinalQuery.Append(" " + sbOrderby);

                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { result = context.Database.SqlQuery<DataContracts.Schedulers.SupplierScheduledTask>(sbfinalQuery.ToString()).ToList(); } catch (Exception ex) { }
                    }

                    return result;                    
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
