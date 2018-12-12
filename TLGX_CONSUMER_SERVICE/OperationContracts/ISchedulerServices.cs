using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OperationContracts
{
    [ServiceContract]
    public interface ISchedulerServices
    {
        #region CRUD Declarations Supplier_Scheduled_Task

        /// <summary>
        /// Comment for All Links inside the given CRUD Ops Region
        /// </summary>
        /// <param name="C reate">  SchedulerServices/ScheduledTask/Add    </param>
        /// <param name="R ead"  >  SchedulerServices/ScheduledTask/Get    </param>
        /// <param name="U pdate">  SchedulerServices/ScheduledTask/Update </param>
        /// <param name="D elete">  SchedulerServices/ScheduledTask/Delete </param>
        /// <returns></returns>

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Add_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_SchedulerServicesTasks> Get_Scheduled_Tasks(DC_SchedulerServicesTasks RqDC_SchedulerServices);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Update_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledTask/Delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Delete_Scheduled_Tasks(DC_SchedulerServicesTasks RQ);

        #endregion

        #region CRUD Declarations Supplier_Scheduled_Task_Log

        /// <summary>
        /// Comment for All Links inside the given CRUD Ops Region
        /// </summary>
        /// <param name="C reate">  SchedulerServices/ScheduledLog/Add    </param>
        /// <param name="R ead"  >  SchedulerServices/ScheduledLog/Get    </param>
        /// <param name="U pdate">  SchedulerServices/ScheduledLog/Update </param>
        /// <param name="D elete">  SchedulerServices/ScheduledLog/Delete </param>
        /// <returns></returns>

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Add", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Add_Scheduled_Tasklog(DC_SchedulerServicesLogs RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<DC_SchedulerServicesLogs> Get_Scheduled_Logs(DC_SchedulerServicesLogs RqDC_SchedulerServices);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Update_Scheduled_Logs(DC_SchedulerServicesLogs RQ);

        [OperationContract]
        [FaultContract(typeof(DataContracts.DC_ErrorStatus))]
        [WebInvoke(Method = "POST", UriTemplate = "SchedulerServices/ScheduledLog/Delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        DC_Message Delete_Scheduled_Logs(DC_SchedulerServicesLogs RQ);

        #endregion
    }
}
