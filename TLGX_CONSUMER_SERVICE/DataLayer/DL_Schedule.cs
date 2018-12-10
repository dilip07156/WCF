using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataContracts.Masters;
using DataContracts;
using DataContracts.Schedulers;

namespace DataLayer
{
    public class DL_Schedule : IDisposable
    {
        public void Dispose() { }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier_Schedule
                                 select sup;
                    if (RQ.Suppllier_ID!= Guid.Empty)
                    {
                        //Guid _newID = RQ.Suppllier_ID;
                        var result = from a in search
                                     where a.Supplier_ID == RQ.Suppllier_ID
                                     select new DataContracts.Schedulers.DC_Supplier_Schedule
                                     {
                                         SupplierScheduleID = a.SupplierScheduleID,
                                         Suppllier_ID = a.Supplier_ID,
                                         ISXMLSupplier = a.ISXMLSupplier,
                                         ISUpdateFrequence = (a.ISUpdateFrequence ?? false),
                                         FrequencyTypeCode = a.FrequencyTypeCode,
                                         Recur_No = (a.Recur_No ?? 0),
                                         MonthOfYear = a.MonthOfYear,
                                         DayOfWeek = a.DayOfWeek,
                                         DateOfMonth = a.DateOfMonth,
                                         WeekOfMonth = a.WeekOfMonth,
                                         StartTime = a.StartTime,
                                         EndTime = a.EndTime,
                                         Status = a.Status,
                                         Create_Date= a.Create_Date,
                                         Create_User = a.Create_User,
                                         Edit_Date= a.Edit_Date,
                                         Edit_User = a.Edit_User,
                                         Entity=a.Entity,
                                         User_Role_Id=a.User_Role_Id,
                                         IsActive=a.IsActive.Value,
                                     };

                        return result.ToList();
                    }
                    else if (RQ.SupplierScheduleID!= Guid.Empty)
                    {
                        var result = from a in search
                                     where a.SupplierScheduleID == RQ.SupplierScheduleID
                                     select new DataContracts.Schedulers.DC_Supplier_Schedule
                                     {
                                         SupplierScheduleID = a.SupplierScheduleID,
                                         Suppllier_ID = a.Supplier_ID,
                                         ISXMLSupplier = a.ISXMLSupplier,
                                         ISUpdateFrequence = (a.ISUpdateFrequence ?? false),
                                         FrequencyTypeCode = a.FrequencyTypeCode,
                                         Recur_No = (a.Recur_No ?? 0),
                                         MonthOfYear = a.MonthOfYear,
                                         DayOfWeek = a.DayOfWeek,
                                         DateOfMonth = a.DateOfMonth,
                                         WeekOfMonth = a.WeekOfMonth,
                                         StartTime = a.StartTime,
                                         EndTime = a.EndTime,
                                         Status = a.Status,
                                         Create_Date = a.Create_Date,
                                         Create_User = a.Create_User,
                                         Edit_Date = a.Edit_Date,
                                         Edit_User = a.Edit_User,
                                         Entity = a.Entity,
                                         User_Role_Id=a.User_Role_Id,
                                         IsActive=a.IsActive.Value,
                                     };

                        return result.ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }
        public DataContracts.DC_Message AddUpdateSchedule(DataContracts.Schedulers.DC_Supplier_Schedule obj)
        {
            try
            {
                DC_Message _msg = new DC_Message();
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    foreach (string strentity in obj.lstEnity)
                    {

                        if (obj.SupplierScheduleID != Guid.Empty)
                        {
                           
                            var result = (from sup in context.Supplier_Schedule
                                         where sup.Supplier_ID == obj.Suppllier_ID && sup.Entity == strentity
                                         select sup).FirstOrDefault();

                            if (result != null)
                            {
                                result.Supplier_ID = obj.Suppllier_ID;
                                result.ISXMLSupplier = obj.ISXMLSupplier;
                                result.ISUpdateFrequence = obj.ISUpdateFrequence;
                                result.Recur_No = obj.Recur_No;
                                result.MonthOfYear = obj.MonthOfYear;
                                result.FrequencyTypeCode = obj.FrequencyTypeCode;
                                result.DateOfMonth = obj.DateOfMonth;
                                result.DayOfWeek = obj.DayOfWeek;
                                result.WeekOfMonth = obj.WeekOfMonth;
                                result.StartTime = obj.StartTime;
                                result.EndTime = obj.EndTime;
                                result.Status = obj.Status;
                                result.Edit_User = obj.Edit_User;
                                result.Edit_Date = obj.Edit_Date;
                                result.Entity = strentity;
                                result.CronExpression = obj.CronExpression;
                                result.IsActive = true;
                                result.User_Role_Id = obj.User_Role_Id;
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
                            return _msg;

                        }
                        else
                        {
                            Supplier_Schedule _obj = new Supplier_Schedule
                            {
                                SupplierScheduleID = Guid.NewGuid(),
                                Supplier_ID = obj.Suppllier_ID,
                                ISXMLSupplier = obj.ISXMLSupplier,
                                ISUpdateFrequence = obj.ISUpdateFrequence,
                                FrequencyTypeCode = obj.FrequencyTypeCode,
                                Recur_No = obj.Recur_No,
                                MonthOfYear = obj.MonthOfYear,
                                DayOfWeek = obj.DayOfWeek,
                                DateOfMonth = obj.DateOfMonth,
                                WeekOfMonth = obj.WeekOfMonth,
                                StartTime = obj.StartTime,
                                EndTime = obj.EndTime,
                                Create_Date = obj.Create_Date,
                                Create_User = obj.Create_User,
                                Entity = strentity,
                                CronExpression = obj.CronExpression,
                                Status= obj.Status,
                                IsActive = true,
                                User_Role_Id=obj.User_Role_Id,
                        };
                            context.Supplier_Schedule.Add(_obj);
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
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while saving Supplier Schedule", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }

        }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule_RS> GetScheduleBySupplier(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier_Schedule
                                 where sup.Supplier_ID==RQ.Suppllier_ID //&& sup.Status=="Active"
                                 select sup;
                    int total;

                    total = search.Count();

                    if (RQ.PageSize == 0)
                        RQ.PageSize = 10;

                    int skip = (RQ.PageNo ?? 0) * (RQ.PageSize ?? 0);

                    var canPage = skip < total;
                    var result = from a in search
                                 select new DataContracts.Schedulers.DC_Supplier_Schedule_RS
                                 {
                                     SupplierScheduleID = a.SupplierScheduleID,
                                     Suppllier_ID=a.Supplier_ID,
                                     Entity = a.Entity,
                                     FrequencyTypeCode = a.FrequencyTypeCode,
                                     TotalSize = total,
                                     Status= a.Status,

                                 };

                    return result.OrderBy(p => p.Suppllier_ID).Skip(skip).Take((RQ.PageSize ?? total)).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while fetching City Master", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool UpdateSupplierSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {

                    var search = (from ac in context.Supplier_Schedule
                                  where ac.SupplierScheduleID == RQ.SupplierScheduleID
                                  select ac).First();
                    if(RQ.IsActive==true)
                    {
                        var Activecount = (from a in context.Supplier_Schedule
                                     where a.Supplier_ID == RQ.Suppllier_ID && a.Entity == RQ.Enitity && a.IsActive==true
                                     select a).Count();
                        if(Activecount == 0)
                        {
                            search.IsActive = RQ.IsActive;
                            search.Edit_Date = RQ.Edit_Date;
                            search.Edit_User = RQ.Edit_User;
                            context.SaveChanges();
                        }
                    }
                   else if (search != null)
                    {
                        search.IsActive = RQ.IsActive;
                        search.Edit_Date = RQ.Edit_Date;
                        search.Edit_User = RQ.Edit_User;                       
                        context.SaveChanges();
                    }

                    return true;
                }
            }
            catch
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

        public bool CheckExistingSupplierSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    //var search  = from ac in context.Supplier_Schedule where ac.Supplier_ID equals RQ.sup

                    var search = (from ac in context.Supplier_Schedule
                                  //join r in RQ on ac.Supplier_ID equals r.Suppllier_ID 
                                  where ac.Supplier_ID==RQ.Suppllier_ID && RQ.Entities.Contains(ac.Entity) 
                                  select ac).ToList();
                    if(search.Count>0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while updating accomodation contacts", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }
        }

    }
}
