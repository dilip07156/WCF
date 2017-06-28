using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;

namespace DataContracts.Admin
{
    [DataContract]
    public class DC_EntityDetails
    {
        [DataMember]
        public Nullable<Guid> EntityID { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public int EntityTypeID { get; set; }
    }
}
