using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OperationContracts;
using DataContracts;
using BusinessLayer;

namespace ConsumerSvc
{
    public partial class Consumer : IConsumer
    {
        public IList<DataContracts.Schedulers.DC_Supplier_Schedule> GetSchedule(DataContracts.Schedulers.DC_Supplier_Schedule_RQ RQ)
        {
            using (BusinessLayer.BL_Schedule obj = new BL_Schedule())
            {
                return obj.GetSchedule(RQ);
            }
        }

        public DataContracts.DC_Message AddUpdateSchedule(DataContracts.Schedulers.DC_Supplier_Schedule obj)
        {
            using (BusinessLayer.BL_Schedule objBL = new BL_Schedule())
            {
                return objBL.AddUpdateSchedule(obj);
            }
        }

        public IList<DataContracts.Schedulers.SupplierScheduledTask> GetScheduledTaskByRoles(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ obj)
        {
            using (BL_Schedule _obj = new BL_Schedule())
            {
                return _obj.GetScheduledTaskByRoles(obj);
            }
        }

        //UpdateTaskLog
        public DataContracts.DC_Message UpdateTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ obj)
        {
            using (BusinessLayer.BL_Schedule objBL = new BL_Schedule())
            {
                return objBL.UpdateTaskLog(obj);
            }
        }

        public IList<DataContracts.Schedulers.Supplier_Task_Logs> GetScheduleTaskLogList(string Task_Id)
        {
            using (BL_Schedule _obj = new BL_Schedule())
            {
                return _obj.GetScheduleTaskLogList(Task_Id);
            }
        }

        public IList<DataContracts.Schedulers.Supplier_Task_Notifications> GetScheduleNotificationTaskLog(DataContracts.Schedulers.DC_SupplierScheduledTaskRQ RQ)
        {
            using (BL_Schedule _obj = new BL_Schedule())
            {
                return _obj.GetScheduleNotificationTaskLog(RQ);
            }
        }


    }
}