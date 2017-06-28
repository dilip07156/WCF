using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataContracts.Masters;
using DataContracts;

namespace DataLayer
{
    public class DL_Schedule : IDisposable
    {
        public void Dispose() { }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(string Supplier_Id)
        {
            try
            {
                using (ConsumerEntities context = new ConsumerEntities())
                {
                    var search = from sup in context.Supplier_Schedule
                                 select sup;
                    if (!string.IsNullOrWhiteSpace(Supplier_Id))
                    {
                        Guid _newID = Guid.Parse(Supplier_Id);
                        var result = from a in search
                                     where a.Suppllier_ID == _newID
                                     select new DataContracts.Schedulers.DC_Supplier_Schedule
                                     {
                                         SupplierScheduleID = a.SupplierScheduleID,
                                         Suppllier_ID = a.Suppllier_ID,
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
                                         Edit_User = a.Edit_User
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

                    if (obj.SupplierScheduleID != Guid.Empty)
                    {
                        var result = context.Supplier_Schedule.Find(obj.SupplierScheduleID);

                        if (result != null)
                        {
                            result.Suppllier_ID = obj.Suppllier_ID;
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
                            if (context.SaveChanges() == 1)
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strUpdatedSuccessfully;
                                _msg.StatusCode = ReadOnlyMessage.StatusCode.Success;
                            }
                            else
                            {
                                _msg.StatusMessage = ReadOnlyMessage.strFailed;
                                _msg.StatusCode =ReadOnlyMessage.StatusCode.Failed;
                            }
                        }
                        return _msg;

                    }
                    else
                    {
                        Supplier_Schedule _obj = new Supplier_Schedule
                        {
                            SupplierScheduleID = Guid.NewGuid(),
                            Suppllier_ID = obj.Suppllier_ID,
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
                            Create_User = obj.Create_User
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
                return _msg;
            }
            catch (Exception ex)
            {
                throw new FaultException<DataContracts.DC_ErrorStatus>(new DataContracts.DC_ErrorStatus { ErrorMessage = "Error while saving Supplier Schedule", ErrorStatusCode = System.Net.HttpStatusCode.InternalServerError });
            }


        }
    }
}
