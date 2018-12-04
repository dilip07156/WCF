using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataLayer
{
    public class DL_SchedulerServices : IDisposable
    {
        public void Dispose() { }

        #region CRUD Declarations Supplier_Scheduled_Task

        /// <summary>
        /// This service will return All Scheduled Tasks.
        /// </summary>
        /// <param name="RqDC_SchedulerServices"></param>
        /// /// RqDC_SchedulerServices.Operation  ==> Will decide which Set of data needs to return through service.
        /// ///                                   ==> ALL stands for specific Columns with All Data will return.        
        /// /// <param name="JsonRequest">{"Operation":"ALL"}</param>
        /// <returns>Json Response Below</returns>
        ///[{"Api_Call_Log_Id": "GUID/NEWID","Create_User": "varun","IsActive": true,"Operation": null,"Schedule_Datetime": "SomeDate","Schedule_Id": ""GUID/NEWID"","Status": "Pendsdfing","Task_Id": ""GUID/NEWID""},
        ///{"Api_Call_Log_Id": "GUID/NEWID","Create_User": "varun","IsActive": true,"Operation": null,"Schedule_Datetime": "SomeDate","Schedule_Id": ""GUID/NEWID"","Status": "Pendsdfing","Task_Id": ""GUID/NEWID""}] 
        public List<DC_SchedulerServicesTasks> Get_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            try
            {
                List<DC_SchedulerServicesTasks> List = new List<DC_SchedulerServicesTasks>();
                IQueryable<DC_SchedulerServicesTasks> search;
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    switch (RqDC_SchedulerServices.Operation)
                    {
                        case "ALL":
                            search = from s in context.Supplier_Scheduled_Task
                                     select new DC_SchedulerServicesTasks
                                     {
                                         Task_Id = s.Task_Id,
                                         Schedule_Id = (Guid)s.Schedule_Id,
                                         Schedule_Datetime = (DateTime)s.Schedule_Datetime,
                                         Api_Call_Log_Id = (Guid)s.Api_Call_Log_Id,
                                         Status = s.Status,
                                         Create_User = s.Create_User,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        case "SCHEDULEID":
                            search = from s in context.Supplier_Scheduled_Task
                                     where s.Schedule_Id == RqDC_SchedulerServices.Schedule_Id
                                     select new DC_SchedulerServicesTasks
                                     {
                                         Task_Id = s.Task_Id,
                                         Schedule_Id = (Guid)s.Schedule_Id,
                                         Schedule_Datetime = (DateTime)s.Schedule_Datetime,
                                         Api_Call_Log_Id = (Guid)s.Api_Call_Log_Id,
                                         Status = s.Status,
                                         Create_User = s.Create_User,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        case "TASKID":
                            search = from s in context.Supplier_Scheduled_Task
                                     where s.Task_Id == RqDC_SchedulerServices.Task_Id
                                     select new DC_SchedulerServicesTasks
                                     {
                                         Task_Id = s.Task_Id,
                                         Schedule_Id = (Guid)s.Schedule_Id,
                                         Schedule_Datetime = (DateTime)s.Schedule_Datetime,
                                         Api_Call_Log_Id = (Guid)s.Api_Call_Log_Id,
                                         Status = s.Status,
                                         Create_User = s.Create_User,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        default:
                            return new List<DC_SchedulerServicesTasks>();
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Scheduled Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }

        /// <summary></summary>
        /// <param name="Json Request">
        /// {"Api_Call_Log_Id":"29D2EEAA-3252-45AA-9D5F-1A9BF6EFD5F2","Create_User":"varun.phadke@coxandkings.com","IsActive":true,"Schedule_Id":"5EFDE08C-95A2-4449-8D1B-044FA5C981EB",
        /// "Status":"Pending","Schedule_Datetime":"\/Date(928129800000+0530)\/","Task_Id":"AAEF6E0B-C8AB-4BC7-985F-A3D9BC6799B4"}</param>
        /// <returns - Json Response>
        /// {  "StatusCode": 1,  "StatusMessage": "5efde08c-95a2-4449-8d1b-044fa5c981eb has been added successfully."}
        /// </returns>
        public DC_Message Add_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            DC_Message _msg = new DC_Message();

            bool isExist = false;
            if (RqDC_SchedulerServices.Task_Id == Guid.Empty || RqDC_SchedulerServices.Task_Id == null || RqDC_SchedulerServices.Schedule_Id == null || RqDC_SchedulerServices.Schedule_Id == null)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }

            try
            {

                DC_SchedulerServicesTasks RQ = new DC_SchedulerServicesTasks();
                RQ.Schedule_Id = RqDC_SchedulerServices.Schedule_Id;
                RQ.Operation = RqDC_SchedulerServices.Operation ?? "SCHEDULEID";

                List<DC_SchedulerServicesTasks> existScheduleId = Get_Scheduled_Tasks(RQ).ToList();

                if (existScheduleId != null)
                {
                    if (existScheduleId.Count > 0)
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        _msg.StatusMessage = RqDC_SchedulerServices.Schedule_Id.ToString() + DataContracts.ReadOnlyMessage.strAlreadyExist;
                        return _msg;

                    }
                    else
                    { isExist = false; }
                }
                else { isExist = false; }

                if (!isExist)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        Supplier_Scheduled_Task objNew = new Supplier_Scheduled_Task();

                        objNew.Task_Id = RqDC_SchedulerServices.Task_Id;
                        objNew.Schedule_Id = RqDC_SchedulerServices.Schedule_Id;
                        objNew.Schedule_Datetime = RqDC_SchedulerServices.Schedule_Datetime;

                        if (RqDC_SchedulerServices.Api_Call_Log_Id != null) { objNew.Api_Call_Log_Id = RqDC_SchedulerServices.Api_Call_Log_Id; }

                        objNew.Status = RqDC_SchedulerServices.Status;
                        objNew.Create_Date = (System.DateTime)DateTime.Now;
                        objNew.Create_User = RqDC_SchedulerServices.Create_User;
                        objNew.IsActive = RqDC_SchedulerServices.IsActive;
                        context.Supplier_Scheduled_Task.Add(objNew);
                        context.SaveChanges();
                        objNew = null;

                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        _msg.StatusMessage = RqDC_SchedulerServices.Schedule_Id + DataContracts.ReadOnlyMessage.strAddedSuccessfully;
                        return _msg;
                    }
                }
                else
                {
                    _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                    _msg.StatusMessage = RqDC_SchedulerServices.Schedule_Id + DataContracts.ReadOnlyMessage.strAlreadyExist;
                    return _msg;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding state master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message Update_Scheduled_Tasks(DC_SchedulerServicesTasks param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();

            bool IsExist = false;

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //To check dupicate state
                    DC_SchedulerServicesTasks RQ = new DC_SchedulerServicesTasks();
                    RQ.Task_Id = param.Task_Id;
                    RQ.Operation = "TASKID";

                    var result = Get_Scheduled_Tasks(RQ);

                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            var search = context.Supplier_Scheduled_Task.Find(param.Task_Id);
                            if (search != null)
                            {
                                using (var trn = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                                {
                                    search.Status = param.Status;

                                    // Condition for Change Running Status to completed.
                                    if (param.Api_Call_Log_Id != null) { search.Api_Call_Log_Id = param.Api_Call_Log_Id; }

                                    search.Edit_User = param.Create_User;
                                    search.Edit_Date = DateTime.Now;
                                    context.SaveChanges();

                                    trn.Commit();
                                }
                            }
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strUpdatedSuccessfully;
                            return _msg;
                        }
                        else
                        {
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                            return _msg;
                        }
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                        _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                        return _msg;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Supplier Scheduled Task.", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message Delete_Scheduled_Tasks(DC_SchedulerServicesTasks param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();

            bool IsExist = false;

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //To check dupicate state
                    DC_SchedulerServicesTasks RQ = new DC_SchedulerServicesTasks();
                    RQ.Task_Id = param.Task_Id;
                    RQ.Operation = "TASKID";

                    var result = Get_Scheduled_Tasks(RQ);

                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            var search = context.Supplier_Scheduled_Task.Find(param.Task_Id);

                            if (search != null)
                            {
                                if (param.Operation.ToLower() == "softdelete")
                                {


                                    using (var trn = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                                    {
                                        search.Edit_User = param.Create_User;
                                        search.Edit_Date = DateTime.Now;
                                        search.IsActive = param.IsActive;
                                        context.SaveChanges();
                                        trn.Commit();
                                    }


                                }
                                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                                _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strDeleted.Replace("deleted", (param.IsActive ? "restored" : "deleted"));
                                return _msg;
                            }
                            else
                            {
                                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                                _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                                return _msg;
                            }

                        }
                        else
                        {
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                            return _msg;
                        }
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                        _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                        return _msg;
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Supplier Scheduled Task.", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region CRUD Declarations Supplier_Scheduled_Task

        public List<DC_SchedulerServicesLogs> Get_Scheduled_Logs(DC_SchedulerServicesLogs RqDC_SchedulerServices)
        {
            try
            {
                List<DC_SchedulerServicesLogs> List = new List<DC_SchedulerServicesLogs>();
                IQueryable<DC_SchedulerServicesLogs> search;
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    switch (RqDC_SchedulerServices.Operation)
                    {
                        case "ALL":
                            search = from s in context.Supplier_Scheduled_Task_Log
                                     select new DC_SchedulerServicesLogs
                                     {
                                         Log_Id = s.Log_Id,
                                         Task_Id = (Guid)s.Task_Id,
                                         Status_Message = s.Status_Message,
                                         Log_Type = s.Log_Type,
                                         Remarks = s.Remarks,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        case "TASKID":
                            search = from s in context.Supplier_Scheduled_Task_Log
                                     where s.Task_Id == (Guid)RqDC_SchedulerServices.Task_Id
                                     select new DC_SchedulerServicesLogs
                                     {
                                         Log_Id = s.Log_Id,
                                         Task_Id = (Guid)s.Task_Id,
                                         Status_Message = s.Status_Message,
                                         Log_Type = s.Log_Type,
                                         Remarks = s.Remarks,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        case "LOGID":
                            search = from s in context.Supplier_Scheduled_Task_Log
                                     where s.Log_Id == (Guid)RqDC_SchedulerServices.Log_Id
                                     select new DC_SchedulerServicesLogs
                                     {
                                         Log_Id = s.Log_Id,
                                         Task_Id = (Guid)s.Task_Id,
                                         Status_Message = s.Status_Message,
                                         Log_Type = s.Log_Type,
                                         Remarks = s.Remarks,
                                         IsActive = (bool)s.IsActive
                                     };

                            return search.ToList();
                        default:
                            return new List<DC_SchedulerServicesLogs>();
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Scheduled Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return null;
        }

        public DC_Message Add_Scheduled_Tasklog(DC_SchedulerServicesLogs RQ)
        {
            DC_Message _msg = new DC_Message();

            bool isExist = false;
            if (RQ.Task_Id == Guid.Empty || RQ.Task_Id == null || RQ.Log_Id == null || RQ.Log_Id == null)
            {
                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                _msg.StatusMessage = DataContracts.ReadOnlyMessage.strFailed;
                return _msg;
            }

            try
            {

                DC_SchedulerServicesLogs Req = new DC_SchedulerServicesLogs();
                Req.Task_Id = RQ.Task_Id;
                Req.Operation = "TASKID";

                List<DC_SchedulerServicesLogs> existScheduleId = Get_Scheduled_Logs(RQ).ToList();

                if (existScheduleId != null)
                {
                    if (existScheduleId.Count > 0)
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                        _msg.StatusMessage = RQ.Task_Id.ToString() + DataContracts.ReadOnlyMessage.strAlreadyExist;
                        return _msg;

                    }
                    else
                    { isExist = false; }
                }
                else { isExist = false; }

                if (!isExist)
                {
                    using (ConsumerEntities context = new ConsumerEntities())
                    {
                        Supplier_Scheduled_Task_Log objNew = new Supplier_Scheduled_Task_Log();

                        objNew.Log_Id = RQ.Log_Id;
                        objNew.Task_Id = RQ.Task_Id;

                        objNew.Status_Message = RQ.Status_Message;
                        objNew.Log_Type = RQ.Log_Type;
                        objNew.Remarks = RQ.Remarks;
                        objNew.Create_Date = DateTime.Now;
                        objNew.Create_User = RQ.Create_User;
                        objNew.IsActive = RQ.IsActive;

                        context.Supplier_Scheduled_Task_Log.Add(objNew);
                        context.SaveChanges();
                        objNew = null;

                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                        _msg.StatusMessage = RQ.Task_Id + DataContracts.ReadOnlyMessage.strAddedSuccessfully;
                        return _msg;
                    }
                }
                else
                {
                    _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Duplicate;
                    _msg.StatusMessage = RQ.Task_Id + DataContracts.ReadOnlyMessage.strAlreadyExist;
                    return _msg;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while adding Task log", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message Update_Scheduled_Logs(DC_SchedulerServicesLogs param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();

            bool IsExist = false;

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //To check dupicate state
                    DC_SchedulerServicesLogs RQ = new DC_SchedulerServicesLogs();
                    RQ.Log_Id = param.Log_Id;
                    RQ.Operation = "LOGID";

                    var result = Get_Scheduled_Logs(RQ);

                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            var search = context.Supplier_Scheduled_Task_Log.Find(param.Log_Id);

                            if (search != null)
                            {
                                search.Status_Message = param.Status_Message;
                                search.Log_Type = param.Log_Type;
                                search.Remarks = param.Remarks;
                                search.Edit_User = param.Create_User;
                                search.Edit_Date = DateTime.Now;
                                context.SaveChanges();
                            }
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strUpdatedSuccessfully;
                            return _msg;
                        }
                        else
                        {
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                            return _msg;
                        }
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                        _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                        return _msg;
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Supplier Scheduled Logs.", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public DC_Message Delete_Scheduled_Logs(DC_SchedulerServicesLogs param)
        {
            DataContracts.DC_Message _msg = new DataContracts.DC_Message();

            bool IsExist = false;

            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //To check dupicate state
                    DC_SchedulerServicesLogs RQ = new DC_SchedulerServicesLogs();
                    RQ.Log_Id = param.Log_Id;
                    RQ.Operation = "LOGID";

                    var result = Get_Scheduled_Logs(RQ);

                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            var search = context.Supplier_Scheduled_Task_Log.Find(param.Log_Id);
                            if (search != null)
                            {
                                if (param.Operation.ToLower() == "softdelete")
                                {
                                    search.Edit_User = param.Create_User;
                                    search.Edit_Date = DateTime.Now;
                                    search.IsActive = param.IsActive;
                                    context.SaveChanges();
                                }

                                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Success;
                                _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strDeleted.Replace("deleted", (param.IsActive ? "restored" : "deleted"));
                                return _msg;
                            }
                            else
                            {
                                _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                                _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                                return _msg;
                            }

                        }
                        else
                        {
                            _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                            _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                            return _msg;
                        }
                    }
                    else
                    {
                        _msg.StatusCode = DataContracts.ReadOnlyMessage.StatusCode.Warning;
                        _msg.StatusMessage = param.Task_Id + DataContracts.ReadOnlyMessage.strnotExist;
                        return _msg;
                    }
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating Supplier Scheduled Task.", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        #endregion

        #region TackChecker Activities

        public List<DC_UnprocessedData> getTaskGeneratorFiles()
        {
            List<DC_UnprocessedData> lstUnprocessedData = new List<DC_UnprocessedData>();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" ; with CTE AS (");
                sb.Append("  select ");
                sb.Append("      ROW_NUMBER() over(partition by SupplierScheduleID order by tsk.Create_date Desc) RowNum, SupplierScheduleID, tsk.Status, ");
                sb.Append("  	case when tsk.Status = 'Pending' then tsk.Schedule_Datetime else Getdate() end ScheduleDate, sch.CronExpression, Api_Call_Log_Id As Api_Call_Log_Id, ");
                sb.Append("      isnull(sch.ISXMLSupplier, 0) ISXMLSupplier, sch.Supplier_ID, spr.Name, sch.Entity ");
                sb.Append("  from Supplier_Schedule sch with(nolock) ");
                sb.Append("  inner join Supplier spr with(nolock) on sch.Supplier_ID = spr.Supplier_Id ");
                sb.Append("  left join Supplier_Scheduled_Task tsk with(nolock) on sch.SupplierScheduleID = tsk.Schedule_Id ");
                sb.Append("  where spr.StatusCode = 'ACTIVE' and sch.Entity is not null and len(CronExpression) > 0 ");
                sb.Append("  ) ,  ");
                sb.Append(" CTE1 AS( ");
                sb.Append(" select ");
                sb.Append("     mav.AttributeValue, sa.API_Path, ss.SupplierScheduleID, sa.Supplier_APILocation_Id ");
                sb.Append(" from Supplier_Schedule ss with(nolock) ");
                sb.Append(" inner join m_masterattributevalue mav with(nolock) on LOWER(LTRIM(RTRIM(mav.AttributeValue))) = LOWER(LTRIM(RTRIM(ss.Entity))) ");
                sb.Append(" inner join m_masterattribute ma  with(nolock) on ma.MasterAttribute_Id = mav.MasterAttribute_Id ");
                sb.Append(" left join Supplier_APILocation sa with(nolock) on sa.Supplier_Id = ss.Supplier_ID and sa.Entity_Id = mav.MasterAttributeValue_Id ");
                sb.Append(" where ma.Name = 'MappingEntity' and ma.MasterFor = 'MappingFileConfig' ");
                sb.Append(" )  ");
                sb.Append(" select Supplier_ID, Name, Entity, CTE.SupplierScheduleID, ScheduleDate, CronExpression,Convert(varchar(max),CTE1.API_Path) API_Path, ISXMLSupplier, Status ");
                sb.Append(" , null Api_Call_Log_Id,null Supplier_APILocation_Id,null PentahoCall_Id from CTE ");
                sb.Append(" LEFT JOIN CTE1 On CTE.SupplierScheduleID = CTE1.SupplierScheduleID ");
                sb.Append(" where RowNum = 1 And ISXMLSupplier = 1 AND Convert(Date, ScheduleDate) <= Convert(Date, GETDATE()) ");


                //sb.Append("with CTE AS(select ROW_NUMBER() over(partition by SupplierScheduleID order by tsk.Create_date Desc) RowNum, SupplierScheduleID, tsk.Status, ");
                //sb.Append("isnull(tsk.Schedule_Datetime, getdate()) As ScheduleDate, sch.CronExpression, Api_Call_Log_Id As Api_Call_Log_Id, isnull(sch.ISXMLSupplier, 0) ISXMLSupplier ");
                //sb.Append("from Supplier_Schedule sch with(nolock) inner join Supplier spr with(nolock) on sch.Supplier_ID = spr.Supplier_Id left join Supplier_Scheduled_Task tsk ");
                //sb.Append("with(nolock) on sch.SupplierScheduleID = tsk.Schedule_Id where spr.StatusCode = 'ACTIVE' and sch.Entity is not null and len(CronExpression) > 0) ");
                //sb.Append("select RowNum, SupplierScheduleID, ScheduleDate, CronExpression, Api_Call_Log_Id, ");
                //sb.Append("ISXMLSupplier, Status from CTE where RowNum = 1 And ISNULL(status,'') <> 'Pending' And Convert(Date, ScheduleDate) <= Convert(Date, GETDATE()); ");

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    lstUnprocessedData = context.Database.SqlQuery<DC_UnprocessedData>(sb.ToString()).ToList();
                    //return lstUnprocessedData;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Scheduled Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return lstUnprocessedData;
        }
        #endregion

        #region Task Executer

        /// <summary>
        /// Below Function will return the Tasks that will be scheduled for today to Execute.
        /// </summary>
        /// <returns> List of UnprocessedData</returns>
        public List<DC_UnprocessedExecuterData> getExecutableTasks()
        {
            List<DC_UnprocessedExecuterData> lstUnprocessedData = new List<DC_UnprocessedExecuterData>();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" with CTE AS( ");
                sb.Append(" select ROW_NUMBER() over(partition by SupplierScheduleID order by tsk.Create_date Desc) RowNum, SupplierScheduleID, tsk.Status, tsk.Schedule_Datetime ScheduleDate, tsk.Task_Id, ");
                sb.Append(" sch.CronExpression, Api_Call_Log_Id As Api_Call_Log_Id, isnull(sch.ISXMLSupplier, 1) ISXMLSupplier from Supplier_Schedule sch with(nolock) ");
                sb.Append(" inner join Supplier spr with(nolock) on sch.Supplier_ID = spr.Supplier_Id ");
                sb.Append(" left join Supplier_Scheduled_Task tsk with(nolock) on sch.SupplierScheduleID = tsk.Schedule_Id ");
                sb.Append(" where spr.StatusCode = 'ACTIVE' and sch.Entity is not null and len(CronExpression) > 0) , ");
                sb.Append(" CTE1 AS( ");
                sb.Append(" select mav.AttributeValue, sa.API_Path, ss.SupplierScheduleID,sa.Supplier_APILocation_Id from Supplier_Schedule ss with(nolock) ");
                sb.Append(" inner join m_masterattributevalue mav with(nolock) on LOWER(LTRIM(RTRIM(mav.AttributeValue))) = LOWER(LTRIM(RTRIM(ss.Entity))) ");
                sb.Append(" inner join m_masterattribute ma  with(nolock) on ma.MasterAttribute_Id = mav.MasterAttribute_Id ");
                sb.Append(" left join Supplier_APILocation sa with(nolock) on sa.Supplier_Id = ss.Supplier_ID and sa.Entity_Id = mav.MasterAttributeValue_Id ");
                sb.Append(" where ma.Name = 'MappingEntity' and ma.MasterFor = 'MappingFileConfig' ");
                sb.Append(" ) ");
                sb.Append(" select CTE.SupplierScheduleID, ScheduleDate, CTE1.API_Path Api_Call_Log_Id, Status,Task_Id,Supplier_APILocation_Id from CTE LEFT JOIN CTE1 On CTE.SupplierScheduleID = CTE1.SupplierScheduleID ");
                sb.Append(" where RowNum = 1 And status = 'Pending' And ISXMLSupplier = 1 And Convert(Date, ScheduleDate) <= Convert(Date, GETDATE()) order by ScheduleDate; ");


                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    lstUnprocessedData = context.Database.SqlQuery<DC_UnprocessedExecuterData>(sb.ToString()).ToList();
                    //return lstUnprocessedData;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Scheduled Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return lstUnprocessedData;
        }
        #endregion


        #region MyRegion
        public List<DC_LoggerData> getLoggerTasks()
        {
            List<DC_LoggerData> lstUnprocessedData = new List<DC_LoggerData>();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" ; with CTE AS(");
                sb.Append("  select ROW_NUMBER() over(partition by SupplierScheduleID order by tsk.Create_date Desc) RowNum, SupplierScheduleID, tsk.Status,");
                sb.Append("  case when tsk.Status = 'Pending' then tsk.Schedule_Datetime else Getdate() end ScheduleDate, sch.CronExpression, Api_Call_Log_Id As Api_Call_Log_Id,");
                sb.Append("  isnull(sch.ISXMLSupplier, 0) ISXMLSupplier, sch.Supplier_ID, spr.Name, sch.Entity from Supplier_Schedule sch with(nolock)");
                sb.Append("  inner join Supplier spr with(nolock) on sch.Supplier_ID = spr.Supplier_Id");
                sb.Append("  left join Supplier_Scheduled_Task tsk with(nolock) on sch.SupplierScheduleID = tsk.Schedule_Id");
                sb.Append("  where spr.StatusCode = 'ACTIVE' and sch.Entity is not null and len(CronExpression) > 0");
                sb.Append("  ) ,  ");
                sb.Append(" CTE1 AS(");
                sb.Append(" select mav.AttributeValue, sa.API_Path, ss.SupplierScheduleID, sa.Supplier_APILocation_Id from Supplier_Schedule ss with(nolock)");
                sb.Append(" inner join m_masterattributevalue mav with(nolock) on LOWER(LTRIM(RTRIM(mav.AttributeValue))) = LOWER(LTRIM(RTRIM(ss.Entity)))");
                sb.Append(" inner join m_masterattribute ma  with(nolock) on ma.MasterAttribute_Id = mav.MasterAttribute_Id");
                sb.Append(" left join Supplier_APILocation sa with(nolock) on sa.Supplier_Id = ss.Supplier_ID and sa.Entity_Id = mav.MasterAttributeValue_Id");
                sb.Append(" where ma.Name = 'MappingEntity' and ma.MasterFor = 'MappingFileConfig'");
                sb.Append(" ) ,");
                sb.Append(" CTE2 AS(");
                sb.Append(" select* from (select tsk.Status TaskStatus, replace(t.Status,'FAILED','ERROR') PentahoStatus, tsk.Schedule_Id,  SupplierApiCallLog_Id, ");
                sb.Append(" SupplierApiLocation_Id, PentahoCall_Id from(select ROW_NUMBER() over(partition by sa.Supplier_APILocation_Id order by api.Create_Date desc) rowNum,");
                sb.Append(" mav.AttributeValue, sa.API_Path,ss.SupplierScheduleID,api.Status,api.SupplierApiCallLog_Id,api.SupplierApiLocation_Id,api.PentahoCall_Id");
                sb.Append(" from Supplier_Schedule ss with(nolock)");
                sb.Append(" inner join m_masterattributevalue mav with(nolock) on LOWER(LTRIM(RTRIM(mav.AttributeValue))) = LOWER(LTRIM(RTRIM(ss.Entity)))");
                sb.Append(" inner join m_masterattribute ma  with(nolock) on ma.MasterAttribute_Id = mav.MasterAttribute_Id");
                sb.Append(" left join Supplier_APILocation sa with(nolock) on sa.Supplier_Id = ss.Supplier_ID and sa.Entity_Id = mav.MasterAttributeValue_Id");
                sb.Append(" inner join Supplier_ApiCallLog api with(nolock) on sa.Supplier_APILocation_Id = api.SupplierApiLocation_Id");
                sb.Append(" where ma.Name = 'MappingEntity' and ma.MasterFor = 'MappingFileConfig' ) t");
                sb.Append(" left join Supplier_Scheduled_Task tsk with(nolock) on t.SupplierScheduleID = tsk.Schedule_Id where t.rowNum = 1 AND tsk.Status = 'Running'");
                sb.Append(" ) t )");
                sb.Append(" select Supplier_ID, Name, Entity, CTE.SupplierScheduleID, ScheduleDate, CronExpression,Convert(varchar(max), CTE1.API_Path) API_Path, ISXMLSupplier, Status ,");
                sb.Append(" CTE2.PentahoStatus, CTE2.SupplierApiCallLog_Id api_Call_Log_Id, CTE2.SupplierApiLocation_Id Supplier_APILocation_Id, CTE2.PentahoCall_Id PentahoCall_Id from CTE");
                sb.Append("   LEFT JOIN CTE1 On CTE.SupplierScheduleID = CTE1.SupplierScheduleID");
                sb.Append(" left join CTE2 on cte.SupplierScheduleID = CTE2.Schedule_Id");
                sb.Append(" where RowNum = 1 And ISXMLSupplier = 1 AND Convert(Date, ScheduleDate) <= Convert(Date, GETDATE())");

                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    lstUnprocessedData = context.Database.SqlQuery<DC_LoggerData>(sb.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Logger Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return lstUnprocessedData;
        }


        public DC_UnprocessedExecuterData getRunningCount()
        {
            DC_UnprocessedExecuterData RunningData = new DC_UnprocessedExecuterData();

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" DECLARE @CountOfRunning int;DECLARE @default uniqueidentifier = cast(cast(0 as binary) as uniqueidentifier);select @CountOfRunning = count(0) from Supplier_ApiCallLog where status = 'RUNNING';");
                sb.Append(" select  @CountOfRunning TotalCount; ");


                using (ConsumerEntities context = new ConsumerEntities())
                {
                    context.Database.CommandTimeout = 0;
                    RunningData = context.Database.SqlQuery<DC_UnprocessedExecuterData>(sb.ToString()).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus
                {
                    ErrorMessage = "Error while fetching Logger Tasks",
                    ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }

            return RunningData;
        }

        #endregion
    }

}
