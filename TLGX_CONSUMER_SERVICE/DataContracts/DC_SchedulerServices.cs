using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace DataContracts
{
    [DataContract]
    public class DC_SchedulerServicesTasks
    {
        //Parameters For Copy
        //Task_Id
        //Schedule_Id
        //[DataMember]
        //Schedule_Datetime
        //Api_Call_Log_Id
        //Status
        //Create_User
        //IsActive

        [DataMember]
        public Guid Task_Id { get; set; }

        [DataMember]
        public Guid Schedule_Id { get; set; }

        [DataMember]
        public DateTime Schedule_Datetime { get; set; }

        [DataMember]
        public Guid? Api_Call_Log_Id { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string Operation { get; set; }
    }



    [DataContract]
    public class DC_SchedulerServicesLogs
    {
        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public Guid Log_Id { get; set; }

        [DataMember]
        public Guid Task_Id { get; set; }

        [DataMember]
        public string Status_Message { get; set; }

        [DataMember]
        public string Log_Type { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public string Create_User { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }

    [DataContract]
    public class DC_UnprocessedData
    {
        [DataMember]
        public Guid Supplier_ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public Guid SupplierScheduleID { get; set; }

        [DataMember]
        public DateTime ScheduleDate { get; set; }

        [DataMember]
        public string CronExpression { get; set; }

        [DataMember]
        public string API_Path { get; set; }

        [DataMember]
        public bool ISXMLSupplier { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string PentahoStatus { get; set; }

        [DataMember]
        public Guid? api_Call_Log_Id { get; set; }

        [DataMember]
        public Guid? Supplier_APILocation_Id { get; set; }

        [DataMember]
        public Guid? PentahoCall_Id { get; set; }


    }

    [DataContract]
    public class DC_UnprocessedExecuterData
    {
        [DataMember]
        public Guid SupplierScheduleID { get; set; }

        [DataMember]
        public Guid? Supplier_APILocation_Id { get; set; }

        [DataMember]
        public Guid Task_Id { get; set; }

        [DataMember]
        public DateTime ScheduleDate { get; set; }

        [DataMember]
        public Guid Api_Call_Log_Id { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }

    [DataContract]
    public class DC_LoggerData
    {
        [DataMember]
        public Guid SupplierApiCallLog_Id { get; set; }

        [DataMember]
        public Guid SupplierApiLocation_Id { get; set; }

        [DataMember]
        public Guid PentahoCall_Id { get; set; }

        [DataMember]
        public Guid Task_Id { get; set; }

        [DataMember]
        public string PentahoStatus { get; set; }

        [DataMember]
        public string TaskStatus { get; set; }
    }
}
