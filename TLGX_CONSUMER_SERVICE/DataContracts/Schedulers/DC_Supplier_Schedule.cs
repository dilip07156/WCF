using System;
using System.Runtime.Serialization;

namespace DataContracts.Schedulers
{
    [DataContract]
    public class DC_Supplier_Schedule
    {
        public void Dispose()
        { }

        [DataMember]
        public Guid SupplierScheduleID { get; set; }
        [DataMember]
        public Guid Suppllier_ID { get; set; }
        [DataMember]
        public bool ISXMLSupplier { get; set; }
        [DataMember]
        public bool ISUpdateFrequence { get; set; }
        [DataMember]
        public string FrequencyTypeCode { get; set; }
        [DataMember]
        public int Recur_No { get; set; }
        [DataMember]
        public int? MonthOfYear { get; set; }
        [DataMember]
        public string DayOfWeek { get; set; }
        [DataMember]
        public int? DateOfMonth { get; set; }
        [DataMember]
        public int? WeekOfMonth { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Edit_User { get; set; }


    }
}
