using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace DataContracts.STG
{
    [DataContract]
    public class DC_STG_Mapping_Table_Ids
    {
        [DataMember]
        public System.Guid STG_Mapping_Table_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> STG_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> Mapping_Id { get; set; }

        [DataMember]
        public Nullable<System.Guid> File_Id { get; set; }
    }
}
