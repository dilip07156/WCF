using DataContracts;
using DataContracts.Schedulers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
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
                    sbselectRoles.Append(@"select SysUserRole.RoleId from AspNetUsers SysUser with(nolock) join AspNetUserRoles SysUserRole with(nolock) on SysUser.Id= SysUserRole.UserId  where SysUser.UserName  ='" + RQ.UserName + "'");
                    List<string> listRoles = new List<string>();
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { listRoles = context.Database.SqlQuery<string>(sbselectRoles.ToString()).ToList(); } catch (Exception ex) { }
                    }

                    StringBuilder sbsqlselect = new StringBuilder();
                    StringBuilder sbsqlfrom = new StringBuilder();
                    StringBuilder sbsqlwhere = new StringBuilder();
                    List<DataContracts.Schedulers.SupplierScheduledTask> result = new List<DataContracts.Schedulers.SupplierScheduledTask>();

                    #region select 
                    sbsqlselect.Append(@"select d.Name SuppllierName,d.Supplier_Id as Suppllier_ID,c.Entity,b.Task_Id as Task_Id,b.Schedule_Datetime as ScheduledDate,b.Status as Status,
                                         case when b.Status='Pending' then DATEDIFF(d,b.Schedule_Datetime,GETDATE()) else '0' end as PendingFordays,case when ISXMLSupplier=1 then 'API' else 'File' end as LogType,e.PentahoCall_Id as Pentahocall_id,f.API_Path as ApiPath,e.Status as APIStatus, ");

                    #endregion

                    #region from
                   // sbsqlfrom.AppendLine(" from Supplier_Scheduled_Task_Log a with (nolock) inner join Supplier_Scheduled_Task b with (nolock) on a.Task_Id = b.Task_Id inner join Supplier_Schedule c with (nolock) on c.SupplierScheduleID = b.Schedule_Id inner join Supplier d with (nolock) on d.Supplier_Id = c.Supplier_ID Left join Supplier_ApiCallLog e with (nolock) on e.SupplierApiCallLog_Id=b.Api_Call_Log_Id Left join Supplier_APILocation f with (nolock) on f.Supplier_APILocation_Id=e.SupplierApiLocation_Id");

                    sbsqlfrom.AppendLine(" from Supplier_Scheduled_Task b with (nolock) inner join Supplier_Schedule c with (nolock) on c.SupplierScheduleID = b.Schedule_Id inner join Supplier d with (nolock) on d.Supplier_Id = c.Supplier_ID Left join Supplier_ApiCallLog e with (nolock) on e.SupplierApiCallLog_Id=b.Api_Call_Log_Id Left join Supplier_APILocation f with (nolock) on f.Supplier_APILocation_Id=e.SupplierApiLocation_Id");


                    #endregion

                    sbsqlwhere.AppendLine(" where 1=1 ");

                    if(!string.IsNullOrWhiteSpace(RQ.Notification))
                    {
                        sbsqlfrom.AppendLine(" inner join (Select Task_Id, Log_Type,Status_Message from( select row_number() over(partition by task_id order by create_date desc) as RowNo, Log_Type, Task_Id,Status_Message " +
                            "from Supplier_Scheduled_Task_Log with (nolock) where  Create_Date < GETDATE()) T Where RowNo = 1) g on g.Task_Id = b.Task_Id");
                        sbsqlwhere.AppendLine(" and log_type='" + RQ.Notification + "' and Status_Message <> 'Completed'");

                    }

                    //if (!string.IsNullOrWhiteSpace(RQ.RedirectFrom))
                    //{
                    //    sbsqlfrom.AppendLine(" inner join (select distinct Task_Id,log_type from  Supplier_Scheduled_Task_Log with (nolock)) g on g.Task_Id = b.Task_Id ");
                    //    sbsqlwhere.AppendLine(" and g.log_type = '" + RQ.RedirectFrom + "' ");
                    //}

                    if (listRoles.Count>0)
                    {
                        sbsqlwhere.AppendLine(" and c.user_role_id IN ('" + String.Join(",", listRoles) + "')");
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
                    sbsqlselectcount.Append("select count(b.Task_Id) ");
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

                    if (total <= skip)
                    {
                        int PageIndex = 0;
                        int intReminder = total % RQ.PageSize;
                        int intQuotient = total / RQ.PageSize;

                        if (intReminder > 0)
                        {
                            PageIndex = intQuotient + 1;
                        }
                        else
                        {
                            PageIndex = intQuotient;
                        }

                        skip = RQ.PageSize * (PageIndex - 1);
                    }

                   
                     StringBuilder sbOrderby = new StringBuilder();
                    if (RQ.PageSize > 0)
                    {
                        sbOrderby.Append(" ORDER BY b.Schedule_Datetime ");
                        sbOrderby.Append(" OFFSET ");
                        sbOrderby.Append((skip).ToString());
                        sbOrderby.Append(" ROWS FETCH NEXT ");
                        sbOrderby.Append(RQ.PageSize.ToString());
                        sbOrderby.Append(" ROWS ONLY ");
                    }
                    StringBuilder sbfinalQuery = new StringBuilder();
                    sbfinalQuery.Append(sbsqlselect + " ");
                    sbfinalQuery.Append(" " + sbsqlfrom + " ");
                    sbfinalQuery.Append(" " + sbsqlwhere + " ");
                    sbfinalQuery.Append(" " + sbOrderby);

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

        public DataContracts.DC_Message UpdateTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ obj)
        {
            try
            {
                DC_Message _msg = new DC_Message();
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    if (obj.TaskId != Guid.Empty)
                    {
                        //var result = (from a in context.Supplier_Scheduled_Task_Log
                        //             join b in context.Supplier_Scheduled_Task on a.Task_Id equals b.Task_Id
                        //             where a.Log_Id == obj.LogId
                        //             select b).FirstOrDefault();
                        var result = (from  a in context.Supplier_Scheduled_Task 
                                      where a.Task_Id==obj.TaskId
                                      select a).FirstOrDefault();

                        if (result != null)
                        {
                            var unprocessedtask = from a in context.SupplierImportFileDetails
                                                  where a.Supplier_Id == obj.Supplier_Id && a.Entity == obj.Entity && a.STATUS != "PROCESSED" && result.Schedule_Datetime < a.CREATE_DATE
                                                  select a;
                            if (unprocessedtask.Count() == 0 )
                            {
                                result.Edit_Date = obj.Edit_Date;
                                result.Edit_User = obj.Edit_User;
                                result.Status = "Completed";
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
                    
                }
                return _msg;
            }
            catch (Exception ex)
            {
                throw new FaultException<DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while saving Supplier Schedule", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }


        }

        public IList<DataContracts.Schedulers.Supplier_Task_Logs> GetScheduleTaskLogList(string Task_Id)
        {
            try
            {
               
                if (Task_Id!=string.Empty)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        Guid _newId = Guid.Parse(Task_Id);
                           var result = from a in context.Supplier_Scheduled_Task_Log
                                     where a.Task_Id == _newId
                                     select new Supplier_Task_Logs
                                     {
                                         Log_id = a.Log_Id,
                                         Task_id = a.Task_Id,
                                         StatusMessage = a.Status_Message,
                                         LogType = a.Log_Type,
                                         Remarks = a.Remarks,
                                         CreateDate = a.Create_Date

                                     };
                            return result.ToList();
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

        public IList<DataContracts.Schedulers.Supplier_Task_Notifications> GetScheduleNotificationTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ RQ)
        {
            try
            {
                if (RQ.UserName != null && !String.IsNullOrWhiteSpace(RQ.UserName))
                {
                    StringBuilder sbselectRoles = new StringBuilder();
                    sbselectRoles.Append(@"select SysUserRole.RoleId from AspNetUsers SysUser with(nolock) join AspNetUserRoles SysUserRole with(nolock) on SysUser.Id= SysUserRole.UserId  where SysUser.UserName  ='" + RQ.UserName+"'");
                    List<string> listRoles = new List<string>();
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        context.Configuration.AutoDetectChangesEnabled = false;
                        try { listRoles = context.Database.SqlQuery<string>(sbselectRoles.ToString()).ToList(); } catch (Exception ex) { }
                    }

                    StringBuilder sbFinalselectQuery = new StringBuilder();
                    StringBuilder sbsqlFinalwhere = new StringBuilder();
                    StringBuilder sbsqlFinalGroupby = new StringBuilder();
                    StringBuilder sbsselectQuery = new StringBuilder();
                    StringBuilder sbsqlfrom = new StringBuilder();
                    StringBuilder sbsqlwhere = new StringBuilder();
                    List<DataContracts.Schedulers.Supplier_Task_Notifications> result = new List<DataContracts.Schedulers.Supplier_Task_Notifications>();

                    #region 
                    //sbsqlselectQuery.Append(@"Select count(1) as Notification_Count, Log_Type from (select row_number() over (partition by task_id order by create_date desc) as RowNo,Log_Type 
                    //                      from Supplier_Scheduled_Task_Log where Status_Message <> 'Completed' and Create_Date<GETDATE()) T Where RowNo = 1
                    //                      group by Log_Type");

                    sbFinalselectQuery.Append(@"Select count(1) as Notification_Count,case when ISXMLSupplier=0 then 'File' else 'API' end as NotificationType,Status_Message from (");

                    sbsselectQuery.Append(@"select row_number() over (partition by task_log.task_id order by task_log.create_date desc) as RowNo,Log_Type,Status_Message,sup.ISXMLSupplier
                                            from Supplier_Schedule sup inner join Supplier_Scheduled_Task sup_task on sup.SupplierScheduleID=sup_task.Schedule_Id
                                            inner join Supplier_Scheduled_Task_Log task_log on task_log.Task_Id=sup_task.Task_Id  and task_log.Create_Date<GETDATE()");


                    #endregion
                    sbsqlwhere.AppendLine(" where 1=1 ");
                    if (listRoles.Count > 0)
                    {
                        sbsqlwhere.AppendLine(" and sup.user_role_id IN ('" + String.Join(",", listRoles) + "')");
                    }

                    sbsqlwhere.AppendLine(") T where RowNo=1 ");
                    sbsqlFinalGroupby.Append(@"group by ISXMLSupplier,Status_Message");

                    StringBuilder sbfinalQuery = new StringBuilder();
                    sbfinalQuery.Append(sbFinalselectQuery + " ");
                    sbfinalQuery.Append(" " + sbsselectQuery + " ");
                    sbfinalQuery.Append(" " + sbsqlwhere + " ");
                    sbfinalQuery.Append(" " + sbsqlFinalGroupby);

                    using (ConsumerEntities context = new ConsumerEntities())
                        {
                            context.Configuration.AutoDetectChangesEnabled = false;
                            try { result = context.Database.SqlQuery<DataContracts.Schedulers.Supplier_Task_Notifications>(sbfinalQuery.ToString()).ToList(); } catch (Exception ex) { }
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
