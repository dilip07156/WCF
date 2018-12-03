using DataContracts;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_SchedulerServices : IDisposable
    {
        public void Dispose() { }

        #region CRUD Declarations Supplier_Scheduled_Task


        public List<DC_SchedulerServicesTasks> Get_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Get_Scheduled_Tasks(RqDC_SchedulerServices);
            }
        }

        public DC_Message Add_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Add_Scheduled_Tasks(RqDC_SchedulerServices);
            }
        }

        public DC_Message Update_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Update_Scheduled_Tasks(RqDC_SchedulerServices);
            }
        }

        public DC_Message Delete_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Delete_Scheduled_Tasks(RqDC_SchedulerServices);
            }
        }
        #endregion

        #region CRUD Declarations Supplier_Scheduled_Task

        public List<DC_SchedulerServicesLogs> Get_Scheduled_Logs(DC_SchedulerServicesLogs RQ)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Get_Scheduled_Logs(RQ);
            }
        }

        public DC_Message Add_Scheduled_Tasklog(DC_SchedulerServicesLogs RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Add_Scheduled_Tasklog(RqDC_SchedulerServices);
            }
        }

        public DC_Message Update_Scheduled_Logs(DC_SchedulerServicesLogs RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Update_Scheduled_Logs(RqDC_SchedulerServices);
            }
        }

        public DC_Message Delete_Scheduled_Logs(DC_SchedulerServicesLogs RqDC_SchedulerServices)
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.Delete_Scheduled_Logs(RqDC_SchedulerServices);
            }
        }

        #endregion

        #region Task Checker Activities

        public List<DC_UnprocessedData> getTaskGeneratorFiles()
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.getTaskGeneratorFiles();
            }
        }

        public List<DC_UnprocessedExecuterData> getExecutableTasks()
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.getExecutableTasks();
            }
        }

        public List<DC_LoggerData> getLoggerTasks()
        {
            using (DL_SchedulerServices obj = new DL_SchedulerServices())
            {
                return obj.getLoggerTasks();
            }
        }
        
        #endregion

    }
}
