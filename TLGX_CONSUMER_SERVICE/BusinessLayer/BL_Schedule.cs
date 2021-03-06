﻿using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Schedule : IDisposable
    {
        public void Dispose()
        {

        }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            using (DataLayer.DL_Schedule obj = new DataLayer.DL_Schedule())
            {
                return obj.GetSchedule(RQ);
            }
        }
        public DataContracts.DC_Message AddUpdateSchedule(DataContracts.Schedulers.DC_Supplier_Schedule obj)
        {
            using (DataLayer.DL_Schedule objDL = new DataLayer.DL_Schedule())
            {
                return objDL.AddUpdateSchedule(obj);
            }
        }
        public IList<DataContracts.Schedulers.SupplierScheduledTask> GetScheduledTaskByRoles(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ _obj)
        {
            using (DataLayer.DL_ScheduledTask obj = new DataLayer.DL_ScheduledTask())
            {
                return obj.GetScheduleTaskList(_obj);
            }
        }

        public bool CheckExistingSupplierSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            using (DataLayer.DL_Schedule obj = new DataLayer.DL_Schedule())
            {
                return obj.CheckExistingSupplierSchedule(RQ);
            }
        }

        public IList<DataContracts.Schedulers.DC_Supplier_Schedule_RS> GetScheduleBySupplier(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            using (DataLayer.DL_Schedule obj = new DataLayer.DL_Schedule())
            {
                return obj.GetScheduleBySupplier(RQ);
            }
        }

        public bool UpdateSupplierSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            using (DataLayer.DL_Schedule obj = new DataLayer.DL_Schedule())
            {
                return obj.UpdateSupplierSchedule(RQ);
            }
        }

        //UpdateTaskLog

        public DataContracts.DC_Message UpdateTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ obj)
        {
            using (DataLayer.DL_ScheduledTask objDL = new DataLayer.DL_ScheduledTask())
            {
                return objDL.UpdateTaskLog(obj);
            }
        }

        public IList<DataContracts.Schedulers.Supplier_Task_Logs> GetScheduleTaskLogList(string Task_Id)
        {
            using (DataLayer.DL_ScheduledTask obj = new DataLayer.DL_ScheduledTask())
            {
                return obj.GetScheduleTaskLogList(Task_Id);
            }
        }

        public IList<DataContracts.Schedulers.Supplier_Task_Notifications> GetScheduleNotificationTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ RQ)
        {
            using (DataLayer.DL_ScheduledTask obj = new DataLayer.DL_ScheduledTask())
            {
                return obj.GetScheduleNotificationTaskLog(RQ);
            }
        }

    }
}
