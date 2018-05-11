using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class DC_SRT_ML_Response
    {
        [DataMember]
        public DC_SRT_ML_Response_Syntactic _objMLSyn { get; set; }
        [DataMember]
        public DC_SRT_ML_Response_Semantic _objMLSem { get; set; }
        [DataMember]
        public DC_SRT_ML_Response_Supervised_Semantic _objMLSupSem { get; set; }
    }

}
