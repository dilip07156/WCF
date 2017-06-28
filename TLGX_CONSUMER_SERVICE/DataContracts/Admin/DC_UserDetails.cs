using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_UserDetails
    {
        [DataMember]
        public string Userid { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int? EntityTypeID { get; set; }

        [DataMember]
        public string EntityType { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public Guid ApplicationId { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public DateTime? Create_Date { get; set; }
        [DataMember]
        public DateTime? Edit_Date { get; set; }
        [DataMember]
        public string Create_User { get; set; }
        [DataMember]
        public string Edit_User { get; set; }

    }
}
