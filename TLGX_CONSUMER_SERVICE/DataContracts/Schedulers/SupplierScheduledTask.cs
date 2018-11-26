using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Schedulers
{
    [DataContract]
    public class SupplierScheduledTask
    {
        public void Dispose()
        { }
        [DataMember]
        public Guid SupplierScheduleTaskID { get; set; }
        [DataMember]
        public Guid Suppllier_ID { get; set; }
        [DataMember]
        public string SuppllierName { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public bool ISComplete { get; set; }
        [DataMember]
        public bool ISActive { get; set; }
        [DataMember]
        public int PendingFordays{ get; set; }
        [DataMember]
        public DateTime ScheduledDate { get; set; }
        [DataMember]
        public int TotalRecord { get; set; }
    }


    [DataContract]
    public class DC_SupplierScheduledTaskRQ
    {
        [DataMember]
        public Guid? Supplier_Id { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public string SortBy { get; set; }
        [DataMember]
        public string UserName { get; set; }
               
       
    }
}
